
using Microsoft.VisualStudio.Text.Classification;

namespace ALittle
{
    public class ALittleScriptTemplateNameDecReference : ALittleScriptReferenceTemplate<ALittleScriptTemplateNameDecElement>
    {
        public ALittleScriptTemplateNameDecReference(ABnfElement element) : base(element)
        {

        }

        public override string QueryClassificationTag(out bool blur)
        {
            blur = false;
            return "ALittleScriptCustomName";
        }

    }
}

