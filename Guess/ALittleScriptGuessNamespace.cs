using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALittle
{
    public class ALittleScriptGuessNamespace : ALittleScriptGuess
    {
        // 命名域
        public string namespace_name = "";

        // 元素对象
        public ALittleScriptNamespaceDecElement namespace_dec;

        public ALittleScriptGuessNamespace(string p_namespace_name, ALittleScriptNamespaceDecElement p_namespace_dec)
        {
            is_register = ALittleScriptUtility.IsRegister(p_namespace_dec);
            namespace_name = p_namespace_name;
            namespace_dec = p_namespace_dec;
        }

        public override ABnfElement GetElement()
        {
            return namespace_dec;
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
            var guess = new ALittleScriptGuessNamespace(namespace_name, namespace_dec);
            guess.UpdateValue();
            return guess;
        }

        public override void UpdateValue()
        {
            value = namespace_name;
        }

        public override bool IsChanged()
        {
            return ALittleScriptIndex.inst.GetGuessTypeList(namespace_dec) == null;
        }
    }
}
