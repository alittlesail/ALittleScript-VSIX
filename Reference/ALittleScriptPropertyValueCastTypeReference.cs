
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptPropertyValueCastTypeReference : ALittleScriptReferenceTemplate<ALittleScriptPropertyValueCastTypeElement>
    {
        public ALittleScriptPropertyValueCastTypeReference(ABnfElement element) : base(element)
        {
        }

        public override ABnfGuessError GuessTypes(out List<ABnfGuess> guess_list)
        {
            var all_type = m_element.GetAllType();
            if (all_type != null)
                return all_type.GuessTypes(out guess_list);

            guess_list = null;
            return new ABnfGuessError(m_element, "ALittlePropertyValueCastType出现未知的子节点");
        }

        public override ABnfGuessError CheckError()
        {
            var error = m_element.GuessTypes(out List<ABnfGuess> guess_list);
            if (error != null) return error;
            if (guess_list.Count == 0)
                return new ABnfGuessError(m_element, "未知类型");
            else if (guess_list.Count != 1)
                return new ABnfGuessError(m_element, "重复定义");
            return null;
        }
    }
}

