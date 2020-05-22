
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptVarAssignExprReference : ALittleScriptReferenceTemplate<ALittleScriptVarAssignExprElement>
    {
        public ALittleScriptVarAssignExprReference(ABnfElement element) : base(element)
        {

        }

        public override ABnfGuessError CheckError()
        {
            var value_stat = m_element.GetValueStat();
            if (value_stat == null) return null;

            var pair_dec_list = m_element.GetVarAssignDecList();
            if (pair_dec_list.Count == 0) return null;

            // 如果返回值只有一个函数调用
            if (pair_dec_list.Count > 1)
            {
                // 获取右边表达式的
                var guess_error = value_stat.GuessTypes(out List<ABnfGuess> method_call_guess_list);
                if (guess_error != null) return guess_error;
                if (method_call_guess_list.Count == 0)
                    return new ABnfGuessError(value_stat, "调用的函数没有返回值");
                bool has_tail = method_call_guess_list[method_call_guess_list.Count - 1] is ALittleScriptGuessReturnTail;
                if (has_tail)
                {
                    // 不需要检查
                }
                else
                {
                    if (method_call_guess_list.Count < pair_dec_list.Count)
                        return new ABnfGuessError(value_stat, "调用的函数返回值数量少于定义的变量数量");
                }

                for (int i = 0; i < pair_dec_list.Count; ++i)
                {
                    var pair_dec = pair_dec_list[i];
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

            var error = pair_dec_list[0].GuessType(out ABnfGuess pair_guess);
            if (error != null) return error;
            error = value_stat.GuessType(out ABnfGuess value_guess);
            if (error != null) return error;

            error = ALittleScriptOp.GuessTypeEqual(pair_guess, value_stat, value_guess, true, false);
            if (error != null)
                return new ABnfGuessError(error.GetElement(), "等号左边的变量和表达式的类型不同:" + error.GetError());

            return null;
        }
    }
}

