
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptMethodParamOneDecReference : ALittleScriptReferenceTemplate<ALittleScriptMethodParamOneDecElement>
    {
        public ALittleScriptMethodParamOneDecReference(ABnfElement element) : base(element)
        {

        }

        public override ABnfGuessError CheckError()
        {
            return ALittleScriptUtility.CheckError(m_element, m_element.GetModifierList());
        }
    }
}

