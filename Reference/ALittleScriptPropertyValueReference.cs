
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptPropertyValueReference : ALittleScriptReferenceTemplate<ALittleScriptPropertyValueElement>
    {
        public ALittleScriptPropertyValueReference(ABnfElement element) : base(element)
        {
        }

        public override ABnfGuessError GuessTypes(out List<ABnfGuess> guess_list)
        {
            var suffixList = m_element.GetPropertyValueSuffixList();
            if (suffixList.Count == 0)
            {
                var first_type = m_element.GetPropertyValueFirstType();
                if (first_type != null)
                    return first_type.GuessTypes(out guess_list);
            }
            else
            {
                return suffixList[suffixList.Count - 1].GuessTypes(out guess_list);
            }
            guess_list = new List<ABnfGuess>();
            return null;
        }
    }
}

