using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALittle
{
    public class ALittleScriptGuessStructName : ALittleScriptGuess
    {
        // 命名域和结构体名
        public string namespace_name = "";
        public string struct_name = "";

        // 元素对象
        public ALittleScriptStructNameDecElement struct_name_dec;

        public ALittleScriptGuessStructName(string p_namespace_name, string p_struct_name
            , ALittleScriptStructNameDecElement p_struct_name_dec)
        {
            is_register = ALittleScriptUtility.IsRegister(p_struct_name_dec);
            namespace_name = p_namespace_name;
            struct_name = p_struct_name;
            struct_name_dec = p_struct_name_dec;
        }

        public override ABnfElement GetElement()
        {
            return struct_name_dec;
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
            var guess = new ALittleScriptGuessStructName(namespace_name, struct_name, struct_name_dec);
            guess.UpdateValue();
            return guess;
        }

        public override void UpdateValue()
        {
            value = namespace_name + "." + struct_name;
        }

        public override bool IsChanged()
        {
            return ALittleScriptIndex.inst.GetGuessTypeList(struct_name_dec) == null;
        }
    }
}
