
using Microsoft.VisualStudio.Text.Classification;

namespace ALittle
{
    public class ALittleScriptNumberReference : ALittleScriptReferenceTemplate<ALittleScriptNumberElement>
    {
        public ALittleScriptNumberReference(ABnfElement element) : base(element)
        {

        }

        public override string QueryClassificationTag(out bool blur)
        {
            blur = false;
            return "ALittleScriptDefault";
        }

    }
}

