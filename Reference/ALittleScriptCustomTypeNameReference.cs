
using Microsoft.VisualStudio.Text.Classification;

namespace ALittle
{
    public class ALittleScriptCustomTypeNameReference : ALittleScriptCustomTypeCommonReference<ALittleScriptCustomTypeNameElement>
    {
        public ALittleScriptCustomTypeNameReference(ABnfElement element) : base(element.GetParent() as ALittleScriptCustomTypeElement, element)
        {
        }

        public override string QueryClassificationTag(out bool blur)
        {
            blur = false;
            return "ALittleScriptCustomName";
        }
    }
}

