
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptStructVarDecReference : ALittleScriptReferenceTemplate<ALittleScriptStructVarDecElement>
    {
        public ALittleScriptStructVarDecReference(ABnfElement element) : base(element)
        {

        }

        public override ABnfGuessError GuessTypes(out List<ABnfGuess> guess_list)
        {
            var all_type = m_element.GetAllType();
            if (all_type != null) return all_type.GuessTypes(out guess_list);
            guess_list = new List<ABnfGuess>();
            return null;
        }
    }
}

