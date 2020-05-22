
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptCustomTypeReference : ALittleScriptCustomTypeCommonReference<ALittleScriptCustomTypeElement>
    {
        public ALittleScriptCustomTypeReference(ABnfElement element) : base(element as ALittleScriptCustomTypeElement, element)
        {
            m_namespace_name = ALittleScriptUtility.GetNamespaceName(m_element);
            var name_dec = m_element.GetCustomTypeName();
            if (name_dec != null) m_key = name_dec.GetElementText();

            var dot_id = m_element.GetCustomTypeDotId();
            if (dot_id != null)
            {
                var dot_id_name = dot_id.GetCustomTypeDotIdName();
                if (dot_id_name != null)
                {
                    m_namespace_name = m_key;
                    m_key = dot_id_name.GetElementText();
                }
            }
        }

        public override ABnfGuessError CheckError()
        {
            var error = m_element.GuessTypes(out List<ABnfGuess> guess_list);
            if (error != null) return error;
            if (guess_list.Count == 0)
                return new ABnfGuessError(m_element, "未知类型");
            else if (guess_list.Count != 1)
                return new ABnfGuessError(m_element, "重复定义");
            return null;
        }
    }
}

