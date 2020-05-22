
using Microsoft.VisualStudio.Text.Classification;

namespace ALittle
{
    public class ALittleScriptCustomTypeDotIdNameReference : ALittleScriptCustomTypeCommonReference<ALittleScriptCustomTypeDotIdNameElement>
    {
        public ALittleScriptCustomTypeDotIdNameReference(ABnfElement element) : base(element.GetParent().GetParent() as ALittleScriptCustomTypeElement, element)
        {
            var custom_type = element.GetParent().GetParent() as ALittleScriptCustomTypeElement;
            var custom_type_name = custom_type.GetCustomTypeName();
            if (custom_type_name != null)
                m_namespace_name = custom_type_name.GetElementText();
            else
                m_namespace_name = "";
            m_key = m_element.GetElementText();
        }

        public override string QueryClassificationTag(out bool blur)
        {
            blur = false;
            return "ALittleScriptCustomName";
        }
    }
}

