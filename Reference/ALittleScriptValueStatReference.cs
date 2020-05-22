
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptValueStatReference : ALittleScriptReferenceTemplate<ALittleScriptValueStatElement>
    {
        public ALittleScriptValueStatReference(ABnfElement element) : base(element)
        {

        }

        public override ABnfGuessError GuessTypes(out List<ABnfGuess> guess_list)
        {
            if (m_element.GetOpNewStat() != null)
                return m_element.GetOpNewStat().GuessTypes(out guess_list);
            else if (m_element.GetOpNewListStat() != null)
                return m_element.GetOpNewListStat().GuessTypes(out guess_list);
            else if (m_element.GetValueOpStat() != null)
                return ALittleScriptOp.GuessTypes(m_element.GetValueOpStat(), out guess_list);
            else if (m_element.GetBindStat() != null)
                return m_element.GetBindStat().GuessTypes(out guess_list);
            else if (m_element.GetTcallStat() != null)
                return m_element.GetTcallStat().GuessTypes(out guess_list);
            else if (m_element.GetOp2Stat() != null)
            {
                guess_list = new List<ABnfGuess>();
                var error = ALittleScriptOp.GuessType(m_element.GetOp2Stat(), out ABnfGuess guess);
                if (error != null) return error;
                guess_list.Add(guess);
                return null;
            }

            guess_list = new List<ABnfGuess>();
            return null;
        }

        public override ABnfGuessError CheckError()
        {
            ABnfElement parent = m_element.GetParent();
            if (parent is ALittleScriptIfExprElement
                || parent is ALittleScriptElseIfExprElement
                || parent is ALittleScriptWhileExprElement
                || parent is ALittleScriptDoWhileExprElement)
            {
                var error = m_element.GuessTypes(out List<ABnfGuess> guess_list);
                if (error != null) return error;
                if (guess_list.Count == 0) return null;

                if (!(guess_list[0] is ALittleScriptGuessBool) && guess_list[0].GetValue() != "null")
                    return new ABnfGuessError(m_element, "条件语句中的表达式的类型必须是bool或者null");
            }

            return null;
        }
    }
}

