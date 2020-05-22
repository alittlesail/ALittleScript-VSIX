using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALittle
{
    public class ALittleScriptGuessClassName : ALittleScriptGuess
    {
        // 命名域和类名
        public string namespace_name = "";
        public string class_name = "";

        // 元素对象
        public ALittleScriptClassNameDecElement class_name_dec;

        public ALittleScriptGuessClassName(string p_namespace_name, string p_class_name
            , ALittleScriptClassNameDecElement p_class_name_dec)
        {
            is_register = ALittleScriptUtility.IsRegister(p_class_name_dec);
            namespace_name = p_namespace_name;
            class_name = p_class_name;
            class_name_dec = p_class_name_dec;
        }

        public override ABnfElement GetElement()
        {
            return class_name_dec;
        }

        public override bool NeedReplace()
        {
            return false;
        }

        public override ABnfGuess ReplaceTemplate(Dictionary<string, ABnfGuess> fill_map)
        {
            return this;
        }

        public override ABnfGuess Clone()
        {
            var guess = new ALittleScriptGuessClassName(namespace_name, class_name, class_name_dec);
            guess.UpdateValue();
            return guess;
        }

        public override void UpdateValue()
        {
            value += namespace_name + "." + class_name;
        }

        public override bool IsChanged()
        {
            return ALittleScriptIndex.inst.GetGuessTypeList(class_name_dec) == null;
        }
    }
}
