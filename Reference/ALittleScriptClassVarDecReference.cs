
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptClassVarDecReference : ALittleScriptReferenceTemplate<ALittleScriptClassVarDecElement>
    {
        public ALittleScriptClassVarDecReference(ABnfElement element) : base(element)
        {

        }

        public override ABnfGuessError GuessTypes(out List<ABnfGuess> guess_list)
        {
            var all_type = m_element.GetAllType();
            if (all_type != null)
            {
                var error = all_type.GuessTypes(out guess_list);
                if (error != null) return error;
                var class_element_dec = m_element.GetParent() as ALittleScriptClassElementDecElement;
                if (class_element_dec == null) return new ABnfGuessError(m_element, "父节点不是ALittleScriptClassElementDecElement类型");

                bool is_native = ALittleScriptUtility.IsNative(class_element_dec.GetModifierList());
                for (int i = 0; i < guess_list.Count; ++i)
                {
                    var guess = guess_list[i] as ALittleScriptGuessList;
                    if (guess != null && guess.is_native != is_native)
                    {
                        guess.is_native = is_native;
                        guess.UpdateValue();
                    }
                }
                return null;
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

            // 检查赋值表达式
            var value_dec = m_element.GetClassVarValueDec();
            if (value_dec != null)
			{
                var const_value = value_dec.GetConstValue();
                if (const_value != null)
				{
                    error = const_value.GuessType(out var guess);
                    if (error != null) return error;

                    error = ALittleScriptOp.GuessTypeEqual(guess_list[0], const_value, guess, true, false);
                    if (error != null)
                        return new ABnfGuessError(error.GetElement(), "等号左边的变量和表达式的类型不同:" + error.GetError());
                }

                var op_new_stat = value_dec.GetOpNewStat();
                if (op_new_stat != null)
				{
                    error = op_new_stat.GuessType(out var guess);
                    if (error != null) return error;

                    if (!(guess is ALittleScriptGuessList) && !(guess is ALittleScriptGuessMap))
                        return new ABnfGuessError(error.GetElement(), "成员变量初始化只能赋值List或者Map或者常量");

                    error = ALittleScriptOp.GuessTypeEqual(guess_list[0], op_new_stat, guess, true, false);
                    if (error != null)
                        return new ABnfGuessError(error.GetElement(), "等号左边的变量和表达式的类型不同:" + error.GetError());
                }
			}

            return null;
        }
    }
}

