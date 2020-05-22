
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptMethodReturnDecReference : ALittleScriptReferenceTemplate<ALittleScriptMethodReturnDecElement>
    {
        public ALittleScriptMethodReturnDecReference(ABnfElement element) : base(element)
        {

        }

        public override ABnfGuessError CheckError()
        {
            if (m_element.GetMethodReturnOneDecList().Count == 0)
                return new ABnfGuessError(m_element, "没有定义返回值类型");
            return null;
        }
    }
}

