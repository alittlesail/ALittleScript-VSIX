
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptPropertyValueFirstTypeReference : ALittleScriptReferenceTemplate<ALittleScriptPropertyValueFirstTypeElement>
    {
        public ALittleScriptPropertyValueFirstTypeReference(ABnfElement element) : base(element)
        {
        }

        public override ABnfGuessError GuessTypes(out List<ABnfGuess> guess_list)
        {
            if (m_element.GetPropertyValueCastType() != null)
                return m_element.GetPropertyValueCastType().GuessTypes(out guess_list);
            else if (m_element.GetPropertyValueCustomType() != null)
                return m_element.GetPropertyValueCustomType().GuessTypes(out guess_list);
            else if (m_element.GetPropertyValueThisType() != null)
                return m_element.GetPropertyValueThisType().GuessTypes(out guess_list);
            guess_list = new List<ABnfGuess>();
            return null;
        }
    }
}

