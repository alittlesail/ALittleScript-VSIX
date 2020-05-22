
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptClassExtendsDecReference : ALittleScriptReferenceTemplate<ALittleScriptClassExtendsDecElement>
    {
        string m_namespace_name;
        string m_key;
        public ALittleScriptClassExtendsDecReference(ABnfElement element) : base(element)
        {
            if (m_element.GetNamespaceNameDec() != null)
                m_namespace_name = m_element.GetNamespaceNameDec().GetElementText();
            else
                m_namespace_name = ALittleScriptUtility.GetNamespaceName(m_element);

            m_key = "";
            if (m_element.GetClassNameDec() != null)
                m_key = m_element.GetClassNameDec().GetElementText();
        }

        public override ABnfGuessError GuessTypes(out List<ABnfGuess> guess_list)
        {
            var class_name_dec = m_element.GetClassNameDec();
            if (class_name_dec == null)
            {
                guess_list = new List<ABnfGuess>();
                return null;
            }

            return class_name_dec.GuessTypes(out guess_list);
        }

        public override bool QueryCompletion(int offset, List<ALanguageCompletionInfo> list)
        {
            {
                var dec_list = ALittleScriptIndex.inst.FindALittleNameDecList(
                    ALittleScriptUtility.ABnfElementType.CLASS_NAME, m_element.GetFile(), m_namespace_name, "", true);

                foreach (var dec in dec_list)
                    list.Add(new ALanguageCompletionInfo(dec.GetElementText(), ALittleScriptIndex.inst.sClassIcon));
            }
            return true;
        }
    }
}

