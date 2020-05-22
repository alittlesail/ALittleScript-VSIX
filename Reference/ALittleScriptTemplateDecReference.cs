
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptTemplateDecReference : ALittleScriptReferenceTemplate<ALittleScriptTemplateDecElement>
    {
        public ALittleScriptTemplateDecReference(ABnfElement element) : base(element)
        {

        }

        public override bool MultiGuessTypes()
        {
            return true;
        }

        public override ABnfGuessError GuessTypes(out List<ABnfGuess> guess_list)
        {
            guess_list = new List<ABnfGuess>();
            var pair_dec_list = m_element.GetTemplatePairDecList();
            foreach (var pair_dec in pair_dec_list)
            {
                var error = pair_dec.GuessType(out ABnfGuess guess);
                if (error != null) return error;
                guess_list.Add(guess);
            }
                
            return null;
        }
    }
}

