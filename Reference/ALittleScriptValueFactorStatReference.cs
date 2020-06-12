
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptValueFactorStatReference : ALittleScriptReferenceTemplate<ALittleScriptValueFactorStatElement>
    {
        public ALittleScriptValueFactorStatReference(ABnfElement element) : base(element)
        {

        }

        public override ABnfGuessError GuessTypes(out List<ABnfGuess> guess_list)
        {
            if (m_element.GetPropertyValue() != null)
                return m_element.GetPropertyValue().GuessTypes(out guess_list);
            else if (m_element.GetReflectValue() != null)
                return m_element.GetReflectValue().GuessTypes(out guess_list);
            else if (m_element.GetConstValue() != null)
                return m_element.GetConstValue().GuessTypes(out guess_list);
            else if (m_element.GetWrapValueStat() != null)
                return m_element.GetWrapValueStat().GuessTypes(out guess_list);
            else if (m_element.GetMethodParamTailDec() != null)
                return m_element.GetMethodParamTailDec().GuessTypes(out guess_list);
            else if (m_element.GetCoroutineStat() != null)
                return m_element.GetCoroutineStat().GuessTypes(out guess_list);
            else if (m_element.GetPathsValue() != null)
                return m_element.GetPathsValue().GuessTypes(out guess_list);
            guess_list = new List<ABnfGuess>();
            return null;
        }
    }
}

