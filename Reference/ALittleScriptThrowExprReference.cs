
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptThrowExprReference : ALittleScriptReferenceTemplate<ALittleScriptThrowExprElement>
    {
        public ALittleScriptThrowExprReference(ABnfElement element) : base(element)
        {

        }

        public override ABnfGuessError GuessTypes(out List<ABnfGuess> guess_list)
        {
            guess_list = new List<ABnfGuess>();
            var value_stat_list = m_element.GetValueStatList();
            if (value_stat_list.Count == 0)
                return new ABnfGuessError(m_element, "throw表达式不能没有参数");

            if (value_stat_list.Count != 1)
                return new ABnfGuessError(m_element, "throw只有string一个参数");

            var value_stat = value_stat_list[0];
            var error = value_stat.GuessType(out ABnfGuess guess);
            if (error != null) return error;
            if (!(guess is ALittleScriptGuessString))
                return new ABnfGuessError(value_stat, "throw表达式第一个参数必须是string类型");

            return null;
        }

        public override ABnfGuessError CheckError()
        {
            var value_stat_list = m_element.GetValueStatList();
            if (value_stat_list.Count == 0)
                return new ABnfGuessError(m_element, "throw表达式不能没有参数");

            if (value_stat_list.Count != 1)
                return new ABnfGuessError(m_element, "throw只有string一个参数");

            var value_stat = value_stat_list[0];

            var error = ALittleScriptUtility.CalcReturnCount(value_stat, out int return_count, out _);
            if (error != null) return error;
            if (return_count != 1) return new ABnfGuessError(value_stat, "表达式必须只能是一个返回值");

            error = value_stat.GuessType(out ABnfGuess guess);
            if (error != null) return error;
            if (!(guess is ALittleScriptGuessString))
                return new ABnfGuessError(value_stat, "throw表达式第一个参数必须是string类型");

            return null;
        }
    }
}

