
using Microsoft.VisualStudio.Text.Classification;

namespace ALittle
{
    public class ALittleScriptBlockCommentReference : ALittleScriptReferenceTemplate<ALittleScriptBlockCommentElement>
    {
        public ALittleScriptBlockCommentReference(ABnfElement element) : base(element) { }

        public override string QueryClassificationTag(out bool blur)
        {
            blur = false;
            return "ALittleScriptComment";
        }
    }
}

