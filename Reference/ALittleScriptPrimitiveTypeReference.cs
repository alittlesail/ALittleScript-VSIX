
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptPrimitiveTypeReference : ALittleScriptReferenceTemplate<ALittleScriptPrimitiveTypeElement>
    {
        public ALittleScriptPrimitiveTypeReference(ABnfElement element) : base(element)
        {
        }

        public override ABnfGuessError GuessTypes(out List<ABnfGuess> guess_list)
        {
            if (ALittleScriptIndex.inst.sPrimitiveGuessListMap.TryGetValue(m_element.GetElementText(), out guess_list))
                return null;
            guess_list = new List<ABnfGuess>();
            return null;
        }
    }
}

