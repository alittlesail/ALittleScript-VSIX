using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALittle
{
    public class ALittleScriptGuessEnumName : ALittleScriptGuess
    {
        // 命名域和枚举名
        public string namespace_name = "";
        public string enum_name = "";

        // 元素对象
        public ALittleScriptEnumNameDecElement enum_name_dec;

        public ALittleScriptGuessEnumName(string p_namespace_name, string p_enum_name
            , ALittleScriptEnumNameDecElement p_enum_name_dec)
        {
            is_register = ALittleScriptUtility.IsRegister(p_enum_name_dec);
            namespace_name = p_namespace_name;
            enum_name = p_enum_name;
            enum_name_dec = p_enum_name_dec;
        }

        public override ABnfElement GetElement()
        {
            return enum_name_dec;
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
            var guess = new ALittleScriptGuessEnumName(namespace_name, enum_name, enum_name_dec);
            guess.UpdateValue();
            return guess;
        }

        public override void UpdateValue()
        {
            value = namespace_name + "." + enum_name;
        }

        public override bool IsChanged()
        {
            return ALittleScriptIndex.inst.GetGuessTypeList(enum_name_dec) == null;
        }
    }
}
