
namespace ALittle
{
    public class ALittleScriptDoWhileConditionReference : ALittleScriptReferenceTemplate<ALittleScriptDoWhileConditionElement>
    {
        public ALittleScriptDoWhileConditionReference(ABnfElement element) : base(element) { }

        public override ABnfGuessError CheckError()
        {
            var value_stat = m_element.GetValueStat();
            if (value_stat == null)
                return new ABnfGuessError(m_element, "没有条件表达式");

            var error = ALittleScriptUtility.CalcReturnCount(value_stat, out int return_count, out _);
            if (error != null) return error;
            if (return_count != 1) return new ABnfGuessError(value_stat, "表达式必须只能是一个返回值");

            error = value_stat.GuessType(out ABnfGuess guess);
            if (error != null) return error;
            if (guess is ALittleScriptGuessBool) return null;

            return new ABnfGuessError(m_element, "这里必须是一个bool表达式");
        }
    }
}

