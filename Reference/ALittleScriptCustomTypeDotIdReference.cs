
using Microsoft.VisualStudio.Text.Classification;

namespace ALittle
{
    public class ALittleScriptCustomTypeDotIdReference : ALittleScriptCustomTypeCommonReference<ALittleScriptCustomTypeDotIdElement>
    {
        public ALittleScriptCustomTypeDotIdReference(ABnfElement element) : base(element.GetParent() as ALittleScriptCustomTypeElement, element)
        {
            var custom_type = element.GetParent() as ALittleScriptCustomTypeElement;
            var custom_type_name = custom_type.GetCustomTypeName();
            if (custom_type_name != null)
                m_namespace_name = custom_type_name.GetElementText();
            else
                m_namespace_name = "";
            m_key = m_element.GetElementText();
        }
    }
}

