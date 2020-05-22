
namespace ALittle
{
    public class ALittleScriptAssertExprReference : ALittleScriptReferenceTemplate<ALittleScriptAssertExprElement>
    {
        public ALittleScriptAssertExprReference(ABnfElement element) : base(element) { }

        public override ABnfGuessError CheckError()
        {
            var value_stat_list = m_element.GetValueStatList();
            if (value_stat_list.Count == 0)
                return new ABnfGuessError(m_element, "assert表达式不能没有参数");

            if (value_stat_list.Count != 2)
                return new ABnfGuessError(m_element, "assert有且仅有两个参数，第一个是任意类型，第二个是string");

            var value_stat = value_stat_list[0];

            var error = ALittleScriptUtility.CalcReturnCount(value_stat, out int return_count, out _);
            if (error != null) return error;
            if (return_count != 1) return new ABnfGuessError(value_stat, "表达式必须只能是一个返回值");

            error = value_stat.GuessType(out ABnfGuess guess);
            if (error != null) return error;

            value_stat = value_stat_list[1];

            error = ALittleScriptUtility.CalcReturnCount(value_stat, out return_count, out _);
            if (error != null) return error;
            if (return_count != 1) return new ABnfGuessError(value_stat, "表达式必须只能是一个返回值");

            error = value_stat.GuessType(out guess);
            if (error != null) return error;
            if (!(guess is ALittleScriptGuessString))
                return new ABnfGuessError(value_stat, "assert表达式第二个参数必须是string类型");
            return null;
        }
    }
}

