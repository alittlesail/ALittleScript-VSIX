
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptClassGetterDecReference : ALittleScriptReferenceTemplate<ALittleScriptClassGetterDecElement>
    {
        public ALittleScriptClassGetterDecReference(ABnfElement element) : base(element) { }

        public override ABnfGuessError CheckError()
        {
            if (m_element.GetMethodNameDec() == null)
                return new ABnfGuessError(m_element, "没有函数名");

            if (m_element.GetMethodBodyDec() == null)
                return new ABnfGuessError(m_element, "没有函数体");
            return null;
        }
    }
}

