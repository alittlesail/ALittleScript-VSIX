
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptBindStatReference : ALittleScriptReferenceTemplate<ALittleScriptBindStatElement>
    {
        public ALittleScriptBindStatReference(ABnfElement element) : base(element) { }

        public override ABnfGuessError GuessTypes(out List<ABnfGuess> guess_list)
        {
            guess_list = null;

            var value_stat_list = m_element.GetValueStatList();
            if (value_stat_list.Count == 0)
                return new ABnfGuessError(m_element, "bind表达式不能没有参数");

            var value_stat = value_stat_list[0];
            var error = value_stat.GuessType(out ABnfGuess guess);
            if (error != null) return error;

            var guess_functor = guess as ALittleScriptGuessFunctor;
            if (guess_functor == null)
                return new ABnfGuessError(m_element, "bind表达式第一个参数必须是一个函数");

            if (guess_functor.template_param_list.Count > 0)
                return new ABnfGuessError(m_element, "bind表达式要绑定的函数不能有模板定义");

            // 开始构建类型
            var info = new ALittleScriptGuessFunctor(m_element);
            info.await_modifier = guess_functor.await_modifier;
            info.const_modifier = guess_functor.const_modifier;
            info.proto = guess_functor.proto;
            info.template_param_list.AddRange(guess_functor.template_param_list);
            info.param_list.AddRange(guess_functor.param_list);
            info.param_nullable_list.AddRange(guess_functor.param_nullable_list);
            info.param_name_list.AddRange(guess_functor.param_name_list);
            info.param_tail = guess_functor.param_tail;
            info.return_list.AddRange(guess_functor.return_list);
            info.return_tail = guess_functor.return_tail;
            // 移除已填写的参数
            int param_count = value_stat_list.Count - 1;
            while (param_count > 0
                && info.param_list.Count > 0
                && info.param_nullable_list.Count > 0
                && info.param_name_list.Count > 0)
            {
                info.param_list.RemoveAt(0);
                info.param_nullable_list.RemoveAt(0);
                info.param_name_list.RemoveAt(0);
                --param_count;
            }
            info.UpdateValue();
            guess_list = new List<ABnfGuess>() { info };
            return null;
        }

        public override ABnfGuessError CheckError()
        {
            var value_stat_list = m_element.GetValueStatList();
            if (value_stat_list.Count == 0)
                return new ABnfGuessError(m_element, "bind表达式不能没有参数");

            var value_stat = value_stat_list[0];
            var error = value_stat.GuessType(out ABnfGuess guess);
            if (error != null) return error;
            
            var guess_functor = guess as ALittleScriptGuessFunctor;
            if (guess_functor == null)
                return new ABnfGuessError(m_element, "bind表达式第一个参数必须是一个函数");

            if (guess_functor.template_param_list.Count > 0)
                return new ABnfGuessError(m_element, "bind表达式要绑定的函数不能有模板定义");

            // 后面跟的参数数量不能超过这个函数的参数个数
            if (value_stat_list.Count - 1 > guess_functor.param_list.Count)
            {
                if (guess_functor.param_tail == null)
                    return new ABnfGuessError(m_element, "bind表达式参数太多了");
            }

            // 遍历所有的表达式，看下是否符合
            for (int i = 1; i < value_stat_list.Count; ++i)
            {
                if (i - 1 >= guess_functor.param_list.Count) break;

                var param_guess = guess_functor.param_list[i - 1];
                value_stat = value_stat_list[i];

                error = ALittleScriptUtility.CalcReturnCount(value_stat, out int return_count, out _);
                if (error != null) return error;
                if (return_count != 1) return new ABnfGuessError(value_stat, "表达式必须只能是一个返回值");

                error = value_stat.GuessType(out ABnfGuess value_stat_guess);
                if (error != null) return error;
                error = ALittleScriptOp.GuessTypeEqual(param_guess, value_stat, value_stat_guess, false, false);
                if (error != null)
                    return new ABnfGuessError(value_stat, "第" + i + "个参数类型和函数定义的参数类型不同:" + error.GetError());
            }

            return null;
        }
    }
}

