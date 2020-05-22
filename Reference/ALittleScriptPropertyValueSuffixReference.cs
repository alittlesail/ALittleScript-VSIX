
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptPropertyValueSuffixReference : ALittleScriptReferenceTemplate<ALittleScriptPropertyValueSuffixElement>
    {
        public ALittleScriptPropertyValueSuffixReference(ABnfElement element) : base(element)
        {
        }

        public override ABnfGuessError GuessTypes(out List<ABnfGuess> guess_list)
        {
            if (m_element.GetPropertyValueMethodCall() != null)
                return m_element.GetPropertyValueMethodCall().GuessTypes(out guess_list);
            else if (m_element.GetPropertyValueDotId() != null)
                return m_element.GetPropertyValueDotId().GuessTypes(out guess_list);
            else if (m_element.GetPropertyValueBracketValue() != null)
                return m_element.GetPropertyValueBracketValue().GuessTypes(out guess_list);
            guess_list = new List<ABnfGuess>();
            return null;
        }
    }
}

