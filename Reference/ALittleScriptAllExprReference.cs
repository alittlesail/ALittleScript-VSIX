
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptAllExprReference : ALittleScriptReferenceTemplate<ALittleScriptAllExprElement>
    {
        public ALittleScriptAllExprReference(ABnfElement element) : base(element)
        {

        }

        public override ABnfGuessError CheckError()
        {
            return ALittleScriptUtility.CheckError(m_element, m_element.GetModifierList());
        }

        public override string QueryClassificationTag(out bool blur)
        {
            blur = !ALittleScriptUtility.IsLanguageEnable(m_element.GetModifierList());
            return null;
        }
    }
}

