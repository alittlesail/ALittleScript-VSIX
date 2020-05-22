using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALittle
{
    public class ALittleScriptGuessFunctor : ALittleScriptGuess
    {
        // 模板参数列表
        public List<ALittleScriptGuessTemplate> template_param_list = new List<ALittleScriptGuessTemplate>();
        // 参数列表
        public List<ABnfGuess> param_list = new List<ABnfGuess>();
        // 参数名列表
        public List<string> param_name_list = new List<string>();
        // 参数是否可以为null
        public List<bool> param_nullable_list = new List<bool>();
        // 参数占位符
        public ABnfGuess param_tail;
        // 返回值列表
        public List<ABnfGuess> return_list = new List<ABnfGuess>();
        // 返回值占位符
        public ABnfGuess return_tail;
        // 协议注解
        public string proto;
        // 是否是await
        public bool await_modifier = false;
        // 是否有const修饰
        public bool const_modifier = false;
        // 产生当前Functor的节点对象
        public ABnfElement element;
        public ALittleScriptGuessFunctor(ABnfElement p_element)
        {
            is_register = ALittleScriptUtility.IsRegister(p_element);
            element = p_element;
        }

        public override bool HasAny()
        {
            foreach (var guess in param_list)
            {
                if (guess.HasAny()) return true;
            }
            foreach (var guess in return_list)
            {
                if (guess.HasAny()) return true;
            }
            return false;
        }

        public override ABnfElement GetElement()
        {
            return element;
        }

        public override bool NeedReplace()
        {
            foreach (var guess in param_list)
            {
                if (guess.NeedReplace())
                    return true;
            }
            foreach (var guess in return_list)
            {
                if (guess.NeedReplace())
                    return true;
            }
            return false;
        }

        public override ABnfGuess ReplaceTemplate(Dictionary<string, ABnfGuess> fill_map)
        {
            // 克隆一份
            var new_guess = Clone() as ALittleScriptGuessFunctor;
            // 清理参数列表，重新按模板替换
            new_guess.param_list.Clear();
            new_guess.param_nullable_list.Clear();
            for (int i = 0; i < param_list.Count; ++i)
            {
                var guess = param_list[i];
                var replace = guess.ReplaceTemplate(fill_map);
                if (replace == null) return null;
                new_guess.param_list.Add(replace);
                if (i < param_nullable_list.Count)
                    new_guess.param_nullable_list.Add(param_nullable_list[i]);
                else
                    new_guess.param_nullable_list.Add(false);
            }
            // 清理返回值列表，重新按模板替换
            new_guess.return_list.Clear();
            foreach (var guess in return_list)
            {
                var replace = guess.ReplaceTemplate(fill_map);
                if (replace == null) return null;
                new_guess.return_list.Add(replace);
            }
            // 返回拷贝
            return new_guess;
        }

        public override ABnfGuess Clone()
        {
            var guess = new ALittleScriptGuessFunctor(element);
            guess.template_param_list.AddRange(template_param_list);
            guess.param_list.AddRange(param_list);
            guess.param_nullable_list.AddRange(param_nullable_list);
            guess.param_name_list.AddRange(param_name_list);
            guess.param_tail = param_tail;
            guess.return_list.AddRange(return_list);
            guess.return_tail = return_tail;
            guess.proto = proto;
            guess.await_modifier = await_modifier;
            guess.const_modifier = const_modifier;
            guess.UpdateValue();
            return guess;
        }

        public override void UpdateValue()
        {
            value = "Functor<";

            // proto和await修饰
            List<string> pre_list = new List<string>();
            if (proto != null) pre_list.Add(proto);
            if (const_modifier) pre_list.Add("const");
            if (await_modifier) pre_list.Add("await");
            value += string.Join(",", pre_list);

            // 模板参数列表
            if (template_param_list.Count > 0)
            {
                List<string> template_string_list = new List<string>();
                foreach (var guess in template_param_list)
                    template_string_list.Add(guess.GetTotalValue());
                value += "<" + string.Join(",", template_string_list) + ">";
            }

            // 参数类型列表
            List<string> param_string_list = new List<string>();
            for (int i = 0; i < param_list.Count; ++i)
            {
                if (i < param_nullable_list.Count && param_nullable_list[i])
                    param_string_list.Add("[Nullable] " + param_list[i].GetValue());
                else
                    param_string_list.Add(param_list[i].GetValue());
            }
            if (param_tail != null)
                param_string_list.Add(param_tail.GetValue());
            value += "(" + string.Join(",", param_string_list) + ")";

            // 返回值类型列表
            List<string> return_string_list = new List<string>();
            foreach (var guess in return_list)
                return_string_list.Add(guess.GetValue());
            if (return_tail != null)
                return_string_list.Add(return_tail.GetValue());
            if (return_string_list.Count > 0) value += ":";
            value += string.Join(",", return_string_list);

            value += ">";
        }

        public override bool IsChanged()
        {
            foreach (var guess in param_list)
            {
                if (guess.IsChanged())
                    return true;
            }
            foreach (var guess in return_list)
            {
                if (guess.IsChanged())
                    return true;
            }
            if (param_tail != null && param_tail.IsChanged())
                return true;
            if (return_tail != null && return_tail.IsChanged())
                return true;
            return ALittleScriptIndex.inst.GetGuessTypeList(element) == null;
        }
    }
}
