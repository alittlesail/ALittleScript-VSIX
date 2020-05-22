
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptForExprReference : ALittleScriptReferenceTemplate<ALittleScriptForExprElement>
    {
        public ALittleScriptForExprReference(ABnfElement element) : base(element)
        {

        }

        public override ABnfGuessError CheckError()
        {
            var for_condition = m_element.GetForCondition();
            if (for_condition == null) return null;

            var for_pair_dec = for_condition.GetForPairDec();
            if (for_pair_dec == null) return null;

            var step_condition = for_condition.GetForStepCondition();
            var in_condition = for_condition.GetForInCondition();
            if (step_condition != null)
            {
                var for_start_stat = step_condition.GetForStartStat();
                if (for_start_stat == null) return null;

                var error = for_pair_dec.GuessType(out ABnfGuess start_guess);
                if (error != null) return error;
                if (!(start_guess is ALittleScriptGuessInt) && !(start_guess is ALittleScriptGuessLong))
                    return new ABnfGuessError(for_pair_dec.GetVarAssignNameDec(), "这个变量必须是int或long类型");

                var value_stat = for_start_stat.GetValueStat();
                if (value_stat == null)
                    return new ABnfGuessError(for_pair_dec.GetVarAssignNameDec(), "没有初始化表达式");

                error = ALittleScriptUtility.CalcReturnCount(value_stat, out int return_count, out _);
                if (error != null) return error;
                if (return_count != 1) return new ABnfGuessError(value_stat, "等号右边的表达式类型必须只能是一个返回值");

                error = value_stat.GuessType(out ABnfGuess guess);
                if (error != null) return error;
                if (!(guess is ALittleScriptGuessInt) && !(guess is ALittleScriptGuessLong) && !(guess is ALittleScriptGuessDouble))
                    return new ABnfGuessError(value_stat, "等号右边的表达式类型必须是int,long,double 不能是:" + guess.GetValue());

                // 结束表达式
                var end_stat = step_condition.GetForEndStat();
                var step_stat = step_condition.GetForStepStat();

                if (end_stat == null || end_stat.GetValueStat() == null) return new ABnfGuessError(m_element, "必须有结束表达式");
                if (step_stat == null || step_stat.GetValueStat() == null) return new ABnfGuessError(m_element, "必须有步长表达式");

                error = end_stat.GetValueStat().GuessType(out ABnfGuess end_guess);
                if (error != null) return error;
                if (!(end_guess is ALittleScriptGuessBool))
                    return new ABnfGuessError(end_stat, "for的结束条件表达式类型必须是bool, 不能是:" + end_guess.GetValue());

                // 返回值
                error = ALittleScriptUtility.CalcReturnCount(step_stat.GetValueStat(), out return_count, out _);
                if (error != null) return error;
                if (return_count != 1) return new ABnfGuessError(value_stat, "for的步长条件表达式类型必须只能是一个返回值");

                error = step_stat.GetValueStat().GuessType(out ABnfGuess step_guess);
                if (error != null) return error;
                if (!(step_guess is ALittleScriptGuessInt) && !(step_guess is ALittleScriptGuessDouble) && !(step_guess is ALittleScriptGuessLong))
                    return new ABnfGuessError(step_stat, "for的步长条件表达式类型必须是int,double,long, 不能是:" + end_guess.GetValue());
            }
            else if (in_condition != null)
            {
                var value_stat = in_condition.GetValueStat();
                if (value_stat == null) return null;

                var error = ALittleScriptUtility.CalcReturnCount(value_stat, out int return_count, out List<ABnfGuess> return_guess_list);
                if (error != null) return error;
                if (ALittleScriptUtility.IsPairsFunction(return_guess_list))
                    return_count = 1;
                if (return_count != 1) return new ABnfGuessError(value_stat, "for的遍历对象必须只能是一个返回值");

                var pair_dec_list = in_condition.GetForPairDecList();
                pair_dec_list.Insert(0, for_pair_dec);
                error = value_stat.GuessTypes(out List<ABnfGuess> guess_list);
                if (error != null) return error;

                // 检查List
                if (guess_list.Count == 1 && guess_list[0] is ALittleScriptGuessList)
                {
                    var guess = guess_list[0] as ALittleScriptGuessList;

                    // for变量必须是2个
                    if (pair_dec_list.Count != 2)
                        return new ABnfGuessError(in_condition, "这里参数数量必须是2个");

                    // 第一个参数必须是 int或者long
                    error = pair_dec_list[0].GuessType(out ABnfGuess key_guess_type);
                    if (error != null) return error;
                    if (!(key_guess_type is ALittleScriptGuessInt) && !(key_guess_type is ALittleScriptGuessLong))
                        return new ABnfGuessError(pair_dec_list[0], "这个变量必须是int或long类型");

                    // 第二个参数必须和List元素相等
                    error = pair_dec_list[1].GuessType(out ABnfGuess value_guess_type);
                    if (error != null) return error;
                    var sub_type = guess.sub_type;
                    if (guess_list[0].is_const && !sub_type.is_const)
                    {
                        sub_type = sub_type.Clone();
                        sub_type.is_const = true;
                        sub_type.UpdateValue();
                    }
                    error = ALittleScriptOp.GuessTypeEqual(sub_type, pair_dec_list[1], value_guess_type, false, false);
                    if (error != null)
                        return new ABnfGuessError(error.GetElement(), "变量格式错误，不能是:" + value_guess_type.GetValue() + " :" + error.GetError());
                    return null;
                }

                // 检查Map
                if (guess_list.Count == 1 && guess_list[0] is ALittleScriptGuessMap)
                {
                    var guess_map = guess_list[0] as ALittleScriptGuessMap;

                    // for变量必须是2个
                    if (pair_dec_list.Count != 2)
                        return new ABnfGuessError(in_condition, "这里参数数量必须是2个");

                    // 第一个参数必须和Map的key元素相等
                    error = pair_dec_list[0].GuessType(out ABnfGuess key_guess_type);
                    if (error != null) return error;
                    var map_key_type = guess_map.key_type;
                    if (guess_list[0].is_const && !map_key_type.is_const)
                    {
                        map_key_type = map_key_type.Clone();
                        map_key_type.is_const = true;
                        map_key_type.UpdateValue();
                    }
                    error = ALittleScriptOp.GuessTypeEqual(map_key_type, pair_dec_list[0], key_guess_type, false, false);
                    if (error != null)
                        return new ABnfGuessError(error.GetElement(), "key变量格式错误，不能是:" + key_guess_type.GetValue() + " :" + error.GetError());

                    // 第二个参数必须和Map的value元素相等
                    error = pair_dec_list[1].GuessType(out ABnfGuess value_guess_type);
                    if (error != null) return error;
                    var map_value_type = guess_map.value_type;
                    if (guess_list[0].is_const && !map_value_type.is_const)
                    {
                        map_value_type = map_value_type.Clone();
                        map_value_type.is_const = true;
                        map_value_type.UpdateValue();
                    }
                    error = ALittleScriptOp.GuessTypeEqual(map_value_type, pair_dec_list[1], value_guess_type, false, false);
                    if (error != null)
                        return new ABnfGuessError(error.GetElement(), "value变量格式错误，不能是:" + value_guess_type.GetValue() + " :" + error.GetError());
                    return null;
                }

                // 检查迭代函数
                if (ALittleScriptUtility.IsPairsFunction(guess_list)) return null;

                return new ABnfGuessError(value_stat, "遍历对象类型必须是List,Map或者迭代函数");
            }
            return null;
        }
    }
}

