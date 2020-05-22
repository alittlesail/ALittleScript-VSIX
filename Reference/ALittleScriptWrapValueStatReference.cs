
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptWrapValueStatReference : ALittleScriptReferenceTemplate<ALittleScriptWrapValueStatElement>
    {
        public ALittleScriptWrapValueStatReference(ABnfElement element) : base(element)
        {

        }

        public override ABnfGuessError GuessTypes(out List<ABnfGuess> guess_list)
        {
            var value_stat = m_element.GetValueStat();
            if (value_stat != null)
            {
                var error = ALittleScriptUtility.CalcReturnCount(value_stat, out int return_count, out guess_list);
                if (error != null) return error;
                if (return_count != 1) return new ABnfGuessError(value_stat, "表达式必须只能是一个返回值");
                return null;
            }
            guess_list = new List<ABnfGuess>();
            return null;
        }
    }
}

