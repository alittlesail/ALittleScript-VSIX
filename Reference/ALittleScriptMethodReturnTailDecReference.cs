
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptMethodReturnTailDecReference : ALittleScriptReferenceTemplate<ALittleScriptMethodReturnTailDecElement>
    {
        public ALittleScriptMethodReturnTailDecReference(ABnfElement element) : base(element)
        {

        }

        public override ABnfGuessError GuessTypes(out List<ABnfGuess> guess_list)
        {
            var info = new ALittleScriptGuessReturnTail(m_element.GetElementText());
            info.UpdateValue();
            guess_list = new List<ABnfGuess>() { info };
            return null;
        }
    }
}

