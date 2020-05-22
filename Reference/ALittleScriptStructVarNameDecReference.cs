
using Microsoft.VisualStudio.Text.Classification;

namespace ALittle
{
    public class ALittleScriptStructVarNameDecReference : ALittleScriptReferenceTemplate<ALittleScriptStructVarNameDecElement>
    {
        public ALittleScriptStructVarNameDecReference(ABnfElement element) : base(element)
        {

        }

        public override string QueryClassificationTag(out bool blur)
        {
            blur = false;
            return "ALittleScriptVarName";
        }
    }
}

