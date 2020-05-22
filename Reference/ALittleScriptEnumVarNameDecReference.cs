
using Microsoft.VisualStudio.Text.Classification;

namespace ALittle
{
    public class ALittleScriptEnumVarNameDecReference : ALittleScriptReferenceTemplate<ALittleScriptEnumVarNameDecElement>
    {
        public ALittleScriptEnumVarNameDecReference(ABnfElement element) : base(element)
        {

        }

        public override string QueryClassificationTag(out bool blur)
        {
            blur = false;
            return "ALittleScriptVarName";
        }
    }
}

