
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptStringReference : ALittleScriptReferenceTemplate<ALittleScriptStringElement>
    {
        public ALittleScriptStringReference(ABnfElement element) : base(element)
        {

        }

        public override string QueryClassificationTag(out bool blur)
        {
            blur = false;
            return "ALittleScriptDefault";
        }
    }
}

