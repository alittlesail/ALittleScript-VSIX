
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptAllTypeReference : ALittleScriptReferenceTemplate<ALittleScriptAllTypeElement>
    {
        public ALittleScriptAllTypeReference(ABnfElement element) : base(element) { }

        public override ABnfGuessError GuessTypes(out List<ABnfGuess> guess_list)
        {
            guess_list = null;
            ABnfGuessError error = null;
            bool is_const = m_element.GetAllTypeConst() != null;

            if (m_element.GetCustomType() != null)
                error = m_element.GetCustomType().GuessTypes(out guess_list);
            else if (m_element.GetGenericType() != null)
                error = m_element.GetGenericType().GuessTypes(out guess_list);
            else if (m_element.GetPrimitiveType() != null)
                error = m_element.GetPrimitiveType().GuessTypes(out guess_list);

            if (error != null) return error;

            if (guess_list != null)
            {
                if (!is_const) return null;

                for (int i = 0; i < guess_list.Count; ++i)
                {
                    var guess = guess_list[i];
                    if (guess.is_const) continue;

                    if (guess is ALittleScriptGuessPrimitive)
                    {
                        ALittleScriptIndex.inst.sPrimitiveGuessListMap.TryGetValue("const " + guess.GetValue(), out guess_list);
                        if (guess_list == null) return new ABnfGuessError(m_element, "找不到const " + guess.GetValue());
                        break;
                    }
                    else
                    {
                        guess = guess.Clone();
                        guess.is_const = true;
                        guess.UpdateValue();
                    }

                    guess_list[i] = guess;
                }
                return null;
            }

            return new ABnfGuessError(m_element, "AllType出现未知的子节点");
        }
    }
}

