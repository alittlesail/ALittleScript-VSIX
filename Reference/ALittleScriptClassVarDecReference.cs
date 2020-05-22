
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptClassVarDecReference : ALittleScriptReferenceTemplate<ALittleScriptClassVarDecElement>
    {
        public ALittleScriptClassVarDecReference(ABnfElement element) : base(element)
        {

        }

        public override ABnfGuessError GuessTypes(out List<ABnfGuess> guess_list)
        {
            var all_type = m_element.GetAllType();
            if (all_type != null)
            {
                var error = all_type.GuessTypes(out guess_list);
                if (error != null) return error;
                var class_element_dec = m_element.GetParent() as ALittleScriptClassElementDecElement;
                if (class_element_dec == null) return new ABnfGuessError(m_element, "父节点不是ALittleScriptClassElementDecElement类型");

                bool is_native = ALittleScriptUtility.IsNative(class_element_dec.GetModifierList());
                for (int i = 0; i < guess_list.Count; ++i)
                {
                    var guess = guess_list[i] as ALittleScriptGuessList;
                    if (guess != null && guess.is_native != is_native)
                    {
                        guess.is_native = is_native;
                        guess.UpdateValue();
                    }
                }
                return null;
            }

            guess_list = new List<ABnfGuess>();
            return null;
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

