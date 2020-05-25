
using Microsoft.VisualStudio.Text.Classification;

namespace ALittle
{
    public class ALittleScriptLineCommentReference : ALittleScriptReferenceTemplate<ALittleScriptLineCommentElement>
    {
        public ALittleScriptLineCommentReference(ABnfElement element) : base(element)
        {

        }

        public override string QueryClassificationTag(out bool blur)
        {
            blur = false;
            return "ALittleScriptComment";
        }

        public override bool CanGotoDefinition()
        {
            return false;
        }
    }
}

