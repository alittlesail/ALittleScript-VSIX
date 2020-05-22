
using Microsoft.VisualStudio.Text.Classification;

namespace ALittle
{
    public class ALittleScriptTextReference : ALittleScriptReferenceTemplate<ALittleScriptTextElement>
    {
        public ALittleScriptTextReference(ABnfElement element) : base(element) { }

        public override string QueryClassificationTag(out bool blur)
        {
            blur = false;
            return "ALittleScriptText";
        }
    }
}

