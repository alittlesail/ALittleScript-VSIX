
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptTcallStatReference : ALittleScriptReferenceTemplate<ALittleScriptTcallStatElement>
    {
        public ALittleScriptTcallStatReference(ABnfElement element) : base(element)
        {

        }

        public override ABnfGuessError GuessTypes(out List<ABnfGuess> guess_list)
        {
            guess_list = null;
            var value_stat_list = m_element.GetValueStatList();
            if (value_stat_list.Count == 0)
                return new ABnfGuessError(m_element, "tcall表达式不能没有参数");

            // 第一个参数必须是函数
            var value_stat = value_stat_list[0];
            var error = value_stat.GuessType(out ABnfGuess guess);
            if (error != null) return error;
            if (!(guess is ALittleScriptGuessFunctor))
                return new ABnfGuessError(value_stat, "tcall表达式第一个参数必须是一个函数");

            var guess_functor = guess as ALittleScriptGuessFunctor;
            if (guess_functor.template_param_list.Count > 0)
                return new ABnfGuessError(value_stat, "tcall表达式要绑定的函数不能有模板定义");

            guess_list = new List<ABnfGuess>();
            guess_list.Add(ALittleScriptIndex.inst.sStringGuess);
            guess_list.AddRange(guess_functor.return_list);
            if (guess_functor.return_tail != null)
                guess_list.Add(guess_functor.return_tail);

            return null;
        }

        public override ABnfGuessError CheckError()
        {
            var value_stat_list = m_element.GetValueStatList();
            if (value_stat_list.Count == 0)
                return new ABnfGuessError(m_element, "tcall表达式不能没有参数");

            // 第一个参数必须是函数
            var value_stat = value_stat_list[0];

            var error = ALittleScriptUtility.CalcReturnCount(value_stat, out int return_count, out _);
            if (error != null) return error;
            if (return_count != 1) return new ABnfGuessError(value_stat, "表达式必须只能是一个返回值");

            error = value_stat.GuessType(out ABnfGuess guess);
            if (error != null) return error;
            if (!(guess is ALittleScriptGuessFunctor))
                return new ABnfGuessError(value_stat, "tcall表达式第一个参数必须是一个函数");

            var guess_functor = guess as ALittleScriptGuessFunctor;
            if (guess_functor.template_param_list.Count > 0)
                return new ABnfGuessError(value_stat, "tcall表达式要绑定的函数不能有模板定义");

            // 后面跟的参数数量不能超过这个函数的参数个数
            if (value_stat_list.Count - 1 > guess_functor.param_list.Count)
            {
                if (guess_functor.param_tail == null)
                    return new ABnfGuessError(m_element, "tcall表达式参数太多了");
            }

            // 遍历所有的表达式，看下是否符合
            for (int i = 1; i < value_stat_list.Count; ++i)
            {
                if (i - 1 >= guess_functor.param_list.Count) break;
                var param_guess = guess_functor.param_list[i - 1];
                var param_value_stat = value_stat_list[i];

                error = ALittleScriptUtility.CalcReturnCount(param_value_stat, out return_count, out _);
                if (error != null) return error;
                if (return_count != 1) return new ABnfGuessError(param_value_stat, "表达式必须只能是一个返回值");

                error = param_value_stat.GuessType(out ABnfGuess param_value_stat_guess);
                if (error != null) return error;
                error = ALittleScriptOp.GuessTypeEqual(param_guess, param_value_stat, param_value_stat_guess, false, false);
                if (error != null)
                    return new ABnfGuessError(param_value_stat, "第" + i + "个参数类型和函数定义的参数类型不同:" + error.GetError());
            }

            // 检查这个函数是不是await
            if (guess_functor.await_modifier)
            {
                // 检查这次所在的函数必须要有await或者async修饰
                error = ALittleScriptUtility.CheckInvokeAwait(m_element);
                if (error != null) return error;
            }
            return null;
        }
    }
}

