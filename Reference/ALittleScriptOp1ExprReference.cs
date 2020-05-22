
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptOp1ExprReference : ALittleScriptReferenceTemplate<ALittleScriptOp1ExprElement>
    {
        public ALittleScriptOp1ExprReference(ABnfElement element) : base(element)
        {

        }

        public override ABnfGuessError CheckError()
        {
            var value_stat = m_element.GetValueStat();
            if (value_stat == null) return null;

            var error = ALittleScriptUtility.CalcReturnCount(value_stat, out int return_count, out _);
            if (error != null) return error;
            if (return_count != 1) return new ABnfGuessError(value_stat, "表达式必须只能是一个返回值");

            error = value_stat.GuessType(out ABnfGuess guess);
            if (error != null) return error;

            if (guess.is_const)
                return new ABnfGuessError(m_element, "const类型不能使用--或者++运算符");
            return null;
        }
    }
}

