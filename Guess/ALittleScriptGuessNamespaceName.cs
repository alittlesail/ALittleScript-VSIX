using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALittle
{
    public class ALittleScriptGuessNamespaceName : ALittleScriptGuess
    {
        // 命名域
        public string namespace_name = "";

        // 元素对象
        public ALittleScriptNamespaceNameDecElement namespace_name_dec;

        public ALittleScriptGuessNamespaceName(string p_namespace_name, ALittleScriptNamespaceNameDecElement p_namespace_name_dec)
        {
            is_register = ALittleScriptUtility.IsRegister(p_namespace_name_dec);
            namespace_name = p_namespace_name;
            namespace_name_dec = p_namespace_name_dec;
        }

        public override ABnfElement GetElement()
        {
            return namespace_name_dec;
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
            var guess = new ALittleScriptGuessNamespaceName(namespace_name, namespace_name_dec);
            guess.UpdateValue();
            return guess;
        }

        public override void UpdateValue()
        {
            value = namespace_name;
        }

        public override bool IsChanged()
        {
            return ALittleScriptIndex.inst.GetGuessTypeList(namespace_name_dec) == null;
        }
    }
}
