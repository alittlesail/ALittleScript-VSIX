
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptEnumVarDecReference : ALittleScriptReferenceTemplate<ALittleScriptEnumVarDecElement>
    {
        public ALittleScriptEnumVarDecReference(ABnfElement element) : base(element)
        {

        }

        public override ABnfGuessError GuessTypes(out List<ABnfGuess> guess_list)
        {
            if (m_element.GetText() != null)
                ALittleScriptIndex.inst.sPrimitiveGuessListMap.TryGetValue("string", out guess_list);
            else
                ALittleScriptIndex.inst.sPrimitiveGuessListMap.TryGetValue("int", out guess_list);
            if (guess_list == null) guess_list = new List<ABnfGuess>();
            return null;
        }

        public override ABnfGuessError CheckError()
        {
            if (m_element.GetNumber() == null) return null;

            if (!ALittleScriptUtility.IsInt(m_element.GetNumber()))
                return new ABnfGuessError(m_element.GetNumber(), "枚举值必须是整数");
            return null;
        }
    }
}

