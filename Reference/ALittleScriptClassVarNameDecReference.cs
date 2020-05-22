
using Microsoft.VisualStudio.Text.Classification;

namespace ALittle
{
    public class ALittleScriptClassVarNameDecReference : ALittleScriptReferenceTemplate<ALittleScriptClassVarNameDecElement>
    {
        public ALittleScriptClassVarNameDecReference(ABnfElement element) : base(element)
        {

        }

        public override string QueryClassificationTag(out bool blur)
        {
            blur = false;
            return "ALittleScriptVarName";
        }
    }
}

