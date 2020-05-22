using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALittle
{
    public class ALittleScriptGuessClass : ALittleScriptGuess
    {
        // 命名域和类名
        public string namespace_name = "";
        public string class_name = "";

        // 类本身定义的模板列表
        public List<ABnfGuess> template_list = new List<ABnfGuess>();
        // 填充后的模板实例
        public Dictionary<string, ABnfGuess> template_map = new Dictionary<string, ABnfGuess>();

        // 如果是using定义出来的，那么就有这个值
        public string using_name;
        public ALittleScriptClassDecElement class_dec;

        // 是否是原生类
        public bool is_native = false;

        public ALittleScriptGuessClass(string p_namespace_name, string p_class_name
            , ALittleScriptClassDecElement p_class_dec, string p_using_name, bool p_is_const, bool p_is_native)
        {
            is_register = ALittleScriptUtility.IsRegister(p_class_dec);
            namespace_name = p_namespace_name;
            class_name = p_class_name;
            class_dec = p_class_dec;
            using_name = p_using_name;
            is_const = p_is_const;
            is_native = p_is_native;
        }

        public override ABnfElement GetElement()
        {
            return class_dec;
        }

        public override bool NeedReplace()
        {
            if (template_list.Count == 0) return false;
            foreach (var pair in template_map)
            {
                if (pair.Value.NeedReplace())
                    return true;
            }
            return false;
        }
        public override ABnfGuess ReplaceTemplate(Dictionary<string, ABnfGuess> fill_map)
        {
            var new_guess = Clone() as ALittleScriptGuessClass;
            foreach (var pair in template_map)
            {
                var guess = pair.Value.ReplaceTemplate(fill_map);
                if (guess == null) return null;
                if (guess != pair.Value)
                {
                    var replace = pair.Value.ReplaceTemplate(fill_map);
                    if (replace == null) return null;
                    new_guess.template_map.Add(pair.Key, replace);
                }
            }
            return new_guess;
        }

        public override ABnfGuess Clone()
        {
            var guess = new ALittleScriptGuessClass(namespace_name, class_name, class_dec, using_name, is_const, is_native);
            guess.template_list.AddRange(template_list);
            foreach (var pair in template_map)
                guess.template_map.Add(pair.Key, pair.Value);
            guess.UpdateValue();
            return guess;
        }

        public override void UpdateValue()
        {
            value = "";
            if (is_const) value += "const ";
            if (is_native) value += "native ";
            value += namespace_name + "." + class_name;
            List<string> name_list = new List<string>();
            foreach (var template in template_list)
            {
                if (template_map.TryGetValue(template.GetValueWithoutConst(), out ABnfGuess impl))
                {
                    if (template.is_const && !impl.is_const)
                    {
                        impl = impl.Clone();
                        impl.is_const = true;
                        impl.UpdateValue();
                    }
                    name_list.Add(impl.GetValue());
                }
                else
                    name_list.Add(template.GetValue());
            }
            if (name_list.Count > 0)
                value += "<" + string.Join(",", name_list) + ">";
        }

        public override bool IsChanged()
        {
            foreach (var guess in template_list)
            {
                if (guess.IsChanged())
                    return true;
            }
            foreach (var pair in template_map)
            {
                if (pair.Value.IsChanged())
                    return true;
            }
            return ALittleScriptIndex.inst.GetGuessTypeList(class_dec) == null;
        }
    }
}
