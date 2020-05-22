
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptMethodParamNameDecReference : ALittleScriptReferenceTemplate<ALittleScriptMethodParamNameDecElement>
    {
        public ALittleScriptMethodParamNameDecReference(ABnfElement element) : base(element)
        {

        }

        public override string QueryClassificationTag(out bool blur)
        {
            blur = false;
            return "ALittleScriptVarName";
        }

        public override ABnfGuessError GuessTypes(out List<ABnfGuess> guess_list)
        {
            var parent = m_element.GetParent();
            var one_dec = parent as ALittleScriptMethodParamOneDecElement;
            if (one_dec != null)
            {
                var all_type = one_dec.GetAllType();
                if (all_type != null)
                    return all_type.GuessTypes(out guess_list);
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

