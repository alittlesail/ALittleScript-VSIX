
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptOpAssignExprReference : ALittleScriptReferenceTemplate<ALittleScriptOpAssignExprElement>
    {
        public ALittleScriptOpAssignExprReference(ABnfElement element) : base(element)
        {
        }

        public override ABnfGuessError CheckError()
        {
            var property_value_list = m_element.GetPropertyValueList();
            var value_stat = m_element.GetValueStat();
            if (value_stat == null)
            {
                if (property_value_list.Count != 1)
                    return new ABnfGuessError(m_element, "没有赋值表达式时，只能是一个函数调用");
                var property_value = property_value_list[0];
                var suffix_list = property_value.GetPropertyValueSuffixList();
                if (suffix_list.Count == 0)
                    return new ABnfGuessError(m_element, "没有赋值表达式时，只能是一个函数调用");
                var suffix = suffix_list[suffix_list.Count - 1];
                if (suffix.GetPropertyValueMethodCall() == null)
                    return new ABnfGuessError(m_element, "没有赋值表达式时，只能是一个函数调用");
                return null;
            }

            if (property_value_list.Count == 0) return null;

            // 如果返回值只有一个函数调用
            if (property_value_list.Count > 1)
            {
                // 获取右边表达式的
                var guess_error = value_stat.GuessTypes(out List<ABnfGuess> method_call_guess_list);
                if (guess_error != null) return guess_error;
                if (method_call_guess_list.Count == 0)
                    return new ABnfGuessError(value_stat, "调用的函数没有返回值");

                bool hasTail = method_call_guess_list[method_call_guess_list.Count - 1] is ALittleScriptGuessReturnTail;
                if (hasTail)
                {
                    // 不做检查
                }
                else
                {
                    if (method_call_guess_list.Count < property_value_list.Count)
                        return new ABnfGuessError(value_stat, "调用的函数返回值数量少于定义的变量数量");
                }

                for (int i = 0; i < property_value_list.Count; ++i)
                {
                    var pair_dec = property_value_list[i];
                    if (i >= method_call_guess_list.Count) break;
                    if (method_call_guess_list[i] is ALittleScriptGuessReturnTail) break;
                    guess_error = pair_dec.GuessType(out ABnfGuess pair_dec_guess);
                    if (guess_error != null) return guess_error;
                    guess_error = ALittleScriptOp.GuessTypeEqual(pair_dec_guess, value_stat, method_call_guess_list[i], true, false);
                    if (guess_error != null)
                        return new ABnfGuessError(value_stat, "等号左边的第" + (i + 1) + "个变量数量和函数定义的返回值类型不相等:" + guess_error.GetError());
                }

                return null;
            }

            var op_assign = m_element.GetOpAssign();
            if (op_assign == null)
                return new ABnfGuessError(m_element, "没有赋值符号");
            string op_string = op_assign.GetElementText();

            var error = property_value_list[0].GuessType(out ABnfGuess pair_guess);
            if (error != null) return error;
            error = value_stat.GuessType(out ABnfGuess value_guess);
            if (error != null) return error;
            if (pair_guess is ALittleScriptGuessTemplate)
            {
                if (pair_guess.GetValue() != value_guess.GetValue() && value_guess.GetValue() != "null")
                    return new ABnfGuessError(value_stat, "等号左边的变量和表达式的类型不同");
            }

            if (op_string == "=")
            {
                error = ALittleScriptOp.GuessTypeEqual(pair_guess, value_stat, value_guess, true, false);
                if (error != null)
                    return new ABnfGuessError(error.GetElement(), "等号左边的变量和表达式的类型不同:" + error.GetError());
            }
            else
            {
                error = ALittleScriptUtility.CalcReturnCount(value_stat, out int return_count, out _);
                if (error != null) return error;

                if (return_count != 1)
                    return new ABnfGuessError(value_stat, op_string + "右边必须只能是一个返回值");

                if (pair_guess.is_const)
                    return new ABnfGuessError(property_value_list[0], "const类型不能使用" + op_string + "运算符");

                if (!(pair_guess is ALittleScriptGuessInt) && !(pair_guess is ALittleScriptGuessDouble) && !(pair_guess is ALittleScriptGuessLong))
                    return new ABnfGuessError(property_value_list[0], op_string + "左边必须是int, double, long");

                if (!(value_guess is ALittleScriptGuessInt) && !(value_guess is ALittleScriptGuessDouble) && !(value_guess is ALittleScriptGuessLong))
                    return new ABnfGuessError(value_stat, op_string + "右边必须是int, double, long");
            }
            return null;
        }
    }
}

