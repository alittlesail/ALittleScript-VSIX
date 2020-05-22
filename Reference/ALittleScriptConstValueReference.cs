
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptConstValueReference : ALittleScriptReferenceTemplate<ALittleScriptConstValueElement>
    {
        public ALittleScriptConstValueReference(ABnfElement element) : base(element)
        {

        }

        public override ABnfGuessError GuessTypes(out List<ABnfGuess> guess_list)
        {
            guess_list = null;

            string text = m_element.GetElementText();

            if (m_element.GetNumber() != null)
            {
                if (ALittleScriptUtility.IsInt(m_element.GetNumber()))
                    ALittleScriptIndex.inst.sPrimitiveGuessListMap.TryGetValue("int", out guess_list);
                else
                    ALittleScriptIndex.inst.sPrimitiveGuessListMap.TryGetValue("double", out guess_list);
            }
            else if (m_element.GetText() != null)
            {
                ALittleScriptIndex.inst.sPrimitiveGuessListMap.TryGetValue("string", out guess_list);
            }
            else if (text == "true" || text == "false")
            {
                ALittleScriptIndex.inst.sPrimitiveGuessListMap.TryGetValue("bool", out guess_list);
            }
            else if (text == "null")
            {
                guess_list = ALittleScriptIndex.inst.sConstNullGuess;
            }
            else
            {
                return new ABnfGuessError(m_element, "未知的常量类型:" + text);
            }

            if (guess_list == null) guess_list = new List<ABnfGuess>();
            return null;
        }
    }
}

