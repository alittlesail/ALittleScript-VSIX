
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptEnumDecReference : ALittleScriptReferenceTemplate<ALittleScriptEnumDecElement>
    {
        private string m_namespace_name;

        public ALittleScriptEnumDecReference(ABnfElement element) : base(element)
        {
            m_namespace_name = ALittleScriptUtility.GetNamespaceName(m_element);
        }

        public override ABnfGuessError GuessTypes(out List<ABnfGuess> guess_list)
        {
            guess_list = null;
            var name_dec = m_element.GetEnumNameDec();
            if (name_dec == null)
                return new ABnfGuessError(m_element, "没有定义枚举名");

            var info = new ALittleScriptGuessEnum(m_namespace_name, name_dec.GetElementText(), m_element);
            info.UpdateValue();
            guess_list = new List<ABnfGuess>() { info };
            return null;
        }

        public override ABnfGuessError CheckError()
        {
            var enum_name_dec = m_element.GetEnumNameDec();
            if (enum_name_dec == null)
                return new ABnfGuessError(m_element, "没有定义枚举名");

            var body_dec = m_element.GetEnumBodyDec();
            if (body_dec == null)
                return new ABnfGuessError(m_element, "没有定义枚举内容");

            var var_dec_list = body_dec.GetEnumVarDecList();
            var name_set = new HashSet<string>();
            foreach (var var_dec in var_dec_list)
            {
                var name_dec = var_dec.GetEnumVarNameDec();
                if (name_dec == null) continue;

                var name = name_dec.GetElementText();
                if (name_set.Contains(name))
                    return new ABnfGuessError(name_dec, "枚举字段名重复");
                name_set.Add(name);
            }

            return null;
        }
    }
}

