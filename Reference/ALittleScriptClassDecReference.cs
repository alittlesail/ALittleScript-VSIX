
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptClassDecReference : ALittleScriptReferenceTemplate<ALittleScriptClassDecElement>
    {
        string m_namespace_name;
        public ALittleScriptClassDecReference(ABnfElement element) : base(element)
        {
            m_namespace_name = ALittleScriptUtility.GetNamespaceName(m_element);
        }

        public override ABnfGuessError CheckError()
        {
            var name_dec = m_element.GetClassNameDec();
            if (name_dec == null)
                return new ABnfGuessError(m_element, "没有定义类名");

            var body_dec = m_element.GetClassBodyDec();
            if (body_dec == null)
                return new ABnfGuessError(m_element, "没有定义类体");

            return null;
        }

        public override ABnfGuessError GuessTypes(out List<ABnfGuess> guess_list)
        {
            guess_list = null;

            var name_dec = m_element.GetClassNameDec();
            if (name_dec == null)
                return new ABnfGuessError(m_element, "没有定义类名");

            var body_dec = m_element.GetClassBodyDec();
            if (body_dec == null)
                return new ABnfGuessError(m_element, "没有定义类体");

            var namespace_element_dec = m_element.GetParent() as ALittleScriptNamespaceElementDecElement;
            if (namespace_element_dec == null)
                return new ABnfGuessError(m_element, "ALittleScriptClassDecReference的父节点不是ALittleScriptNamespaceElementDecElement");

            bool is_native = ALittleScriptUtility.IsNative(namespace_element_dec.GetModifierList());
            var info = new ALittleScriptGuessClass(m_namespace_name, name_dec.GetElementText(), m_element, null, false, is_native);
            var template_dec = m_element.GetTemplateDec();
            if (template_dec != null)
            {
                var error = template_dec.GuessTypes(out info.template_list);
                if (error != null) return error;
            }
            info.UpdateValue();

            guess_list = new List<ABnfGuess>() { info };
            return null;
        }
    }
}

