
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptForPairDecReference : ALittleScriptReferenceTemplate<ALittleScriptForPairDecElement>
    {
        public ALittleScriptForPairDecReference(ABnfElement element) : base(element)
        {

        }

        public override ABnfGuessError GuessTypes(out List<ABnfGuess> guess_list)
        {
            // 如果有定义类型
            var all_type = m_element.GetAllType();
            if (all_type != null) return all_type.GuessTypes(out guess_list);

            // 如果没有定义类型
            var parent = m_element.GetParent();
            if (parent is ALittleScriptForConditionElement)
            {
                var for_condition = parent as ALittleScriptForConditionElement;
                var step_condition = for_condition.GetForStepCondition();
                if (step_condition != null)
                {
                    var start_stat = step_condition.GetForStartStat();
                    if (start_stat != null)
                    {
                        var value_stat = start_stat.GetValueStat();
                        if (value_stat != null)
                            return value_stat.GuessTypes(out guess_list);
                    }
                }
                else
                {
                    parent = for_condition.GetForInCondition();
                }
            }

            guess_list = new List<ABnfGuess>();
            if (parent is ALittleScriptForInConditionElement)
            {
                var in_condition = parent as ALittleScriptForInConditionElement;
                // 取出遍历的对象
                var value_stat = in_condition.GetValueStat();
                if (value_stat == null)
                    return new ABnfGuessError(m_element, "For没有遍历对象，无法推导类型");

                // 获取定义列表
                var pair_dec_list = in_condition.GetForPairDecList();
                // 查找是第几个，如果没有找到，那么就是第0个，如果有找到那就+1
                int index = pair_dec_list.IndexOf(m_element);
                if (index < 0)
                    index = 0;
                else
                    index += 1;
                // 获取循环对象的类型
                var error = value_stat.GuessTypes(out List<ABnfGuess> value_guess_list);
                if (error != null) return error;
                // 处理List
                if (value_guess_list.Count == 1 && value_guess_list[0] is ALittleScriptGuessList)
                {
                    // 对于List的key使用auto，那么就默认是int类型
                    if (index == 0)
                    {
                        if (value_guess_list[0].is_const)
                        {
                            if (ALittleScriptIndex.inst.sPrimitiveGuessListMap.TryGetValue("const int", out List<ABnfGuess> temp_guess_list))
                                guess_list = temp_guess_list;
                        }
                        else
                        {
                            if (ALittleScriptIndex.inst.sPrimitiveGuessListMap.TryGetValue("int", out List<ABnfGuess> temp_guess_list))
                                guess_list = temp_guess_list;
                        }
                        return null;
                    }
                    else if (index == 1)
                    {
                        var sub_type = ((ALittleScriptGuessList)value_guess_list[0]).sub_type;
                        if (value_guess_list[0].is_const && !sub_type.is_const)
                        {
                            sub_type = sub_type.Clone();
                            sub_type.is_const = true;
                            sub_type.UpdateValue();
                        }
                        guess_list.Add(sub_type);
                    }
                }
                // 处理Map
                else if (value_guess_list.Count == 1 && value_guess_list[0]  is ALittleScriptGuessMap)
                {
                    // 如果是key，那么就取key的类型
                    if (index == 0)
                    {
                        var key_type = ((ALittleScriptGuessMap)value_guess_list[0]).key_type;
                        if (value_guess_list[0].is_const && !key_type.is_const)
                        {
                            key_type = key_type.Clone();
                            key_type.is_const = true;
                            key_type.UpdateValue();
                        }
                        guess_list.Add(key_type);
                    }
                    // 如果是value，那么就取value的类型
                    else if (index == 1)
                    {
                        var value_type = ((ALittleScriptGuessMap)value_guess_list[0]).value_type;
                        if (value_guess_list[0].is_const && !value_type.is_const)
                        {
                            value_type = value_type.Clone();
                            value_type.is_const = true;
                            value_type.UpdateValue();
                        }
                        guess_list.Add(value_type);
                    }
                }
                // 如果是pairs函数
                else if (ALittleScriptUtility.IsPairsFunction(value_guess_list))
                {
                    guess_list.Add(value_guess_list[2]);
                }
            }

            return null;
        }
    }
}

