using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALittle
{
    public class ALittleScriptGuessStruct : ALittleScriptGuess
    {
        // 命名域和结构体名
        public string namespace_name = "";
        public string struct_name = "";

        // 元素对象
        public ALittleScriptStructDecElement struct_dec;

        public ALittleScriptGuessStruct(string p_namespace_name, string p_struct_name
            , ALittleScriptStructDecElement p_struct_dec, bool p_is_const)
        {
            is_register = ALittleScriptUtility.IsRegister(p_struct_dec);
            namespace_name = p_namespace_name;
            struct_name = p_struct_name;
            struct_dec = p_struct_dec;
            is_const = p_is_const;
        }

        public override ABnfElement GetElement()
        {
            return struct_dec;
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
            var guess = new ALittleScriptGuessStruct(namespace_name, struct_name, struct_dec, is_const);
            guess.UpdateValue();
            return guess;
        }

        public override void UpdateValue()
        {
            value = "";
            if (is_const) value += "const ";
            value += namespace_name + "." + struct_name;
        }

        public override bool IsChanged()
        {
            return ALittleScriptIndex.inst.GetGuessTypeList(struct_dec) == null;
        }
    }
}
