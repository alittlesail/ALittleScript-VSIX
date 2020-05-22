
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptPropertyValueMethodCallReference : ALittleScriptReferenceTemplate<ALittleScriptPropertyValueMethodCallElement>
    {
        public ALittleScriptPropertyValueMethodCallReference(ABnfElement element) : base(element)
        {
        }

        public override bool MultiGuessTypes()
        {
            return true;
        }

        public ABnfGuessError GuessPreType(out ABnfGuess guess)
        {
            guess = null;

            // 获取父节点
            var property_value_suffix = m_element.GetParent() as ALittleScriptPropertyValueSuffixElement;
            var property_value = property_value_suffix.GetParent() as ALittleScriptPropertyValueElement;
            var property_value_first_type = property_value.GetPropertyValueFirstType();
            var suffix_list = property_value.GetPropertyValueSuffixList();

            // 获取所在位置
            int index = suffix_list.IndexOf(property_value_suffix);
            if (index == -1) return null;

            // 获取前一个类型
            ABnfGuess pre_type = null;
            ABnfGuess pre_pre_type = null;
            ABnfGuessError error = null;
            if (index == 0)
            {
                error = property_value_first_type.GuessType(out pre_type);
                if (error != null) return error;
            }
            else if (index == 1)
            {
                error = suffix_list[index - 1].GuessType(out pre_type);
                if (error != null) return error;
                error = property_value_first_type.GuessType(out pre_pre_type);
                if (error != null) return error;
            }
            else
            {
                error = suffix_list[index - 1].GuessType(out pre_type);
                if (error != null) return error;
                error = suffix_list[index - 2].GuessType(out pre_pre_type);
                if (error != null) return error;
            }

            // 如果是Functor
            if (pre_type is ALittleScriptGuessFunctor)
            {
                var pre_type_functor = pre_type as ALittleScriptGuessFunctor;
                if (pre_pre_type is ALittleScriptGuessTemplate)
                    pre_pre_type = (pre_pre_type as ALittleScriptGuessTemplate).template_extends;

                // 如果再往前一个是一个Class实例对象，那么就要去掉第一个参数
                if (pre_pre_type is ALittleScriptGuessClass && pre_type_functor.param_list.Count > 0
                        && (pre_type_functor.element is ALittleScriptClassMethodDecElement
                        || pre_type_functor.element is ALittleScriptClassGetterDecElement
                        || pre_type_functor.element is ALittleScriptClassSetterDecElement)) {
                    var new_pre_type_functor = new ALittleScriptGuessFunctor(pre_type_functor.element);
                    pre_type = new_pre_type_functor;

                    new_pre_type_functor.await_modifier = pre_type_functor.await_modifier;
                    new_pre_type_functor.const_modifier = pre_type_functor.const_modifier;
                    new_pre_type_functor.proto = pre_type_functor.proto;
                    new_pre_type_functor.template_param_list.AddRange(pre_type_functor.template_param_list);
                    new_pre_type_functor.param_list.AddRange(pre_type_functor.param_list);
                    new_pre_type_functor.param_nullable_list.AddRange(pre_type_functor.param_nullable_list);
                    new_pre_type_functor.param_name_list.AddRange(pre_type_functor.param_name_list);
                    new_pre_type_functor.param_tail = pre_type_functor.param_tail;
                    new_pre_type_functor.return_list.AddRange(pre_type_functor.return_list);
                    new_pre_type_functor.return_tail = pre_type_functor.return_tail;

                    // 移除掉第一个参数
                    new_pre_type_functor.param_list.RemoveAt(0);
                    new_pre_type_functor.param_nullable_list.RemoveAt(0);
                    new_pre_type_functor.param_name_list.RemoveAt(0);

                    new_pre_type_functor.UpdateValue();
                }
            }

            guess = pre_type;
            return null;
        }

        public override ABnfGuessError GuessTypes(out List<ABnfGuess> guess_list)
        {
            guess_list = new List<ABnfGuess>();

            var src_map = new Dictionary<string, ALittleScriptGuessTemplate>();
            var fill_map = new Dictionary<string, ABnfGuess>();
            var error = CheckTemplateMap(src_map, fill_map, out ALittleScriptGuessFunctor pre_type_functor);
            if (error != null) return error;
            if (pre_type_functor == null) return null;

            foreach (var guess in pre_type_functor.return_list)
            {
                if (guess.NeedReplace())
                {
                    var replace = guess.ReplaceTemplate(fill_map);
                    if (replace == null) return new ABnfGuessError(m_element, "模板替换失败:" + guess.GetValue());
                    guess_list.Add(replace);
                }
                else
                    guess_list.Add(guess);
            }

            if (pre_type_functor.return_tail != null)
                guess_list.Add(pre_type_functor.return_tail);

            return null;
        }

        private ABnfGuessError AnalysisTemplate(Dictionary<string, ABnfGuess> fill_map, ABnfGuess left_guess, ABnfElement right_src, ABnfGuess right_guess, bool assign_or_call)
        {
            // 如果值等于null，那么可以赋值
            if (right_guess.GetValue() == "null") return null;

            // const是否可以赋值给非const
            if (assign_or_call)
            {
                if (left_guess.is_const && !right_guess.is_const)
                    return new ABnfGuessError(right_src, "要求是" + left_guess.GetValue() + ", 不能是:" + right_guess.GetValue());
            }
            else
            {
                // 如果不是基本变量类型（排除any），基本都是值传递，函数调用时就不用检查const
                if (!(left_guess is ALittleScriptGuessPrimitive) || left_guess.GetValueWithoutConst() == "any")
                {
                    if (!left_guess.is_const && right_guess.is_const)
                        return new ABnfGuessError(right_src, "要求是" + left_guess.GetValue() + ", 不能是:" + right_guess.GetValue());
                }
            }

            // 如果任何一方是any，那么就认为可以相等
            if (left_guess is ALittleScriptGuessAny) return null;

            if (left_guess is ALittleScriptGuessPrimitive
                || left_guess is ALittleScriptGuessStruct)
            {
                return ALittleScriptOp.GuessTypeEqual(left_guess, right_src, right_guess, assign_or_call, false);
            }

            if (left_guess is ALittleScriptGuessMap)
            {
                if (!(right_guess is ALittleScriptGuessMap))
                    return new ABnfGuessError(right_src, "要求是" + left_guess.GetValue() + ",不能是:" + right_guess.GetValue());
                

                var error = AnalysisTemplate(fill_map, ((ALittleScriptGuessMap)left_guess).key_type, right_src, ((ALittleScriptGuessMap)right_guess).key_type, false);
                if (error != null) return new ABnfGuessError(right_src, "要求是" + left_guess.GetValue() + ",不能是:" + right_guess.GetValue());
                error = AnalysisTemplate(fill_map, ((ALittleScriptGuessMap)left_guess).value_type, right_src, ((ALittleScriptGuessMap)right_guess).value_type, false);
                if (error != null) return new ABnfGuessError(right_src, "要求是" + left_guess.GetValue() + ",不能是:" + right_guess.GetValue());
                return null;
            }

            if (left_guess is ALittleScriptGuessList)
            {
                if (!(right_guess is ALittleScriptGuessList))
                    return new ABnfGuessError(right_src, "要求是" + left_guess.GetValue() + ",不能是:" + right_guess.GetValue());
                var error = AnalysisTemplate(fill_map, ((ALittleScriptGuessList)left_guess).sub_type, right_src, ((ALittleScriptGuessList)right_guess).sub_type, false);
                if (error != null) return new ABnfGuessError(right_src, "要求是" + left_guess.GetValue() + ",不能是:" + right_guess.GetValue());
                return null;
            }

            if (left_guess is ALittleScriptGuessFunctor)
            {
                if (!(right_guess is ALittleScriptGuessFunctor))
                    return new ABnfGuessError(right_src, "要求是" + left_guess.GetValue() + ",不能是:" + right_guess.GetValue());
                ALittleScriptGuessFunctor left_guess_functor = (ALittleScriptGuessFunctor)left_guess;
                ALittleScriptGuessFunctor right_guess_functor = (ALittleScriptGuessFunctor)right_guess;

                if (left_guess_functor.param_list.Count != right_guess_functor.param_list.Count
                    || left_guess_functor.param_nullable_list.Count != right_guess_functor.param_nullable_list.Count
                    || left_guess_functor.return_list.Count != right_guess_functor.return_list.Count
                    || left_guess_functor.template_param_list.Count != right_guess_functor.template_param_list.Count
                    || left_guess_functor.await_modifier != right_guess_functor.await_modifier
                    || left_guess_functor.proto == null && right_guess_functor.proto != null
                    || left_guess_functor.proto != null && right_guess_functor.proto == null
                    || (left_guess_functor.proto != null && left_guess_functor.proto != right_guess_functor.proto)
                    || left_guess_functor.param_tail == null && right_guess_functor.param_tail != null
                    || left_guess_functor.param_tail != null && right_guess_functor.param_tail == null
                    || left_guess_functor.return_tail == null && right_guess_functor.return_tail != null
                    || left_guess_functor.return_tail != null && right_guess_functor.return_tail == null
                )
                {
                    return new ABnfGuessError(right_src, "要求是" + left_guess.GetValue() + ",不能是:" + right_guess.GetValue());
                }

                for (int i = 0; i < left_guess_functor.template_param_list.Count; ++i)
                {
                    var error = AnalysisTemplate(fill_map, left_guess_functor.template_param_list[i], right_src, right_guess_functor.template_param_list[i], false);
                    if (error != null) return error;
                }

                for (int i = 0; i < left_guess_functor.param_list.Count; ++i)
                {
                    var error = AnalysisTemplate(fill_map, left_guess_functor.param_list[i], right_src, right_guess_functor.param_list[i], false);
                    if (error != null) return error;
                }

                for (int i = 0; i < left_guess_functor.param_nullable_list.Count; ++i)
                {
                    if (left_guess_functor.param_nullable_list[i] != right_guess_functor.param_nullable_list[i])
                        return new ABnfGuessError(right_src, "要求是" + left_guess.GetValue() + ",不能是:" + right_guess.GetValue());
                }

                for (int i = 0; i < left_guess_functor.return_list.Count; ++i)
                {
                    var error = AnalysisTemplate(fill_map, left_guess_functor.return_list[i], right_src, right_guess_functor.return_list[i], false);
                    if (error != null) return error;
                }
                return null;
            }

            if (left_guess is ALittleScriptGuessClass)
            {
                if (right_guess is ALittleScriptGuessTemplate)
                    right_guess = (right_guess as ALittleScriptGuessTemplate).template_extends;

                if (!(right_guess is ALittleScriptGuessClass))
                    return new ABnfGuessError(right_src, "要求是" + left_guess.GetValue() + ",不能是:" + right_guess.GetValue());

                if (left_guess.GetValue() == right_guess.GetValue()) return null;

                var error = ALittleScriptUtility.IsClassSuper((left_guess as ALittleScriptGuessClass).class_dec, right_guess.GetValue(), out bool result);
                if (error != null) return error;
                if (result) return null;
                error = ALittleScriptUtility.IsClassSuper((right_guess as ALittleScriptGuessClass).class_dec, left_guess.GetValue(), out result);
                if (error != null) return error;
                if (result) return null;

                return new ABnfGuessError(right_src, "要求是" + left_guess.GetValue() + ",不能是:" + right_guess.GetValue());
            }

            if (left_guess is ALittleScriptGuessTemplate) {
                var left_guess_template = left_guess as ALittleScriptGuessTemplate;

                // 查看模板是否已经被填充，那么就按填充的检查
                if (fill_map.TryGetValue(left_guess_template.GetValue(), out ABnfGuess fill_guess))
                    return ALittleScriptOp.GuessTypeEqual(fill_guess, right_src, right_guess, false, false);
                
                // 处理还未填充
                if (left_guess_template.template_extends != null)
                {
                    var error = AnalysisTemplate(fill_map, left_guess_template.template_extends, right_src, right_guess, false);
                    if (error != null) return error;
                    fill_map.Add(left_guess_template.GetValue(), right_guess);
                    return null;
                }
                else if (left_guess_template.is_class)
                {
                    if (right_guess is ALittleScriptGuessClass)
                    {
                        fill_map.Add(left_guess_template.GetValue(), right_guess);
                        return null;
                    }
                    else if (right_guess is ALittleScriptGuessTemplate)
                    {
                        var right_guess_template = right_guess as ALittleScriptGuessTemplate;
                        if (right_guess_template.template_extends is ALittleScriptGuessClass || right_guess_template.is_class)
                        {
                            fill_map.Add(right_guess_template.GetValue(), right_guess);
                            return null;
                        }
                    }
                    return new ABnfGuessError(right_src, "要求是" + left_guess.GetValue() + ",不能是:" + right_guess.GetValue());
                }
                else if (left_guess_template.is_struct)
                {
                    if (right_guess is ALittleScriptGuessStruct)
                    {
                        fill_map.Add(left_guess_template.GetValue(), right_guess);
                        return null;
                    }
                    else if (right_guess is ALittleScriptGuessTemplate)
                    {
                        var right_guess_template = right_guess as ALittleScriptGuessTemplate;
                        if (right_guess_template.template_extends is ALittleScriptGuessStruct || right_guess_template.is_struct)
                        {
                            fill_map.Add(left_guess_template.GetValue(), right_guess);
                            return null;
                        }
                    }
                    return new ABnfGuessError(right_src, "要求是" + left_guess.GetValue() + ",不能是:" + right_guess.GetValue());
                }

                fill_map.Add(left_guess_template.GetValue(), right_guess);
                return null;
            }

            return new ABnfGuessError(right_src, "要求是" + left_guess.GetValue() + ",不能是:" + right_guess.GetValue());
        }

        private ABnfGuessError CheckTemplateMap(Dictionary<string, ALittleScriptGuessTemplate> src_map, Dictionary<string, ABnfGuess> fill_map, out ALittleScriptGuessFunctor guess)
        {
            guess = null;
            var error = GuessPreType(out ABnfGuess pre_type);
            if (error != null) return error;
            if (pre_type == null) return null;

            // 如果需要处理
            if (!(pre_type is ALittleScriptGuessFunctor)) return null;
            ALittleScriptGuessFunctor pre_type_functor = (ALittleScriptGuessFunctor)pre_type;

            var value_stat_list = m_element.GetValueStatList();
            if (pre_type_functor.param_list.Count < value_stat_list.Count && pre_type_functor.param_tail == null)
                return new ABnfGuessError(m_element, "函数调用最多需要" + pre_type_functor.param_list.Count + "个参数,不能是:" + value_stat_list.Count + "个");

            // 检查模板参数
            if (pre_type_functor.template_param_list.Count > 0)
            {
                foreach (var template_param in pre_type_functor.template_param_list)
                {
                    if (src_map.ContainsKey(template_param.GetValue())) src_map.Remove(template_param.GetValue());
                    src_map.Add(template_param.GetValue(), template_param);
                }

                var method_template = m_element.GetPropertyValueMethodTemplate();
                if (method_template != null)
                {
                    var all_type_list = method_template.GetAllTypeList();
                    if (all_type_list.Count > pre_type_functor.template_param_list.Count)
                        return new ABnfGuessError(m_element, "函数调用最多需要" + pre_type_functor.template_param_list.Count + "个模板参数,不能是:" + all_type_list.Count + "个");

                    for (int i = 0; i < all_type_list.Count; ++i)
                    {
                        error = all_type_list[i].GuessType(out ABnfGuess all_type_guess);
                        if (error != null) return error;
                        error = ALittleScriptOp.GuessTypeEqual(pre_type_functor.template_param_list[i], all_type_list[i], all_type_guess, false, false);
                        if (error != null) return error;
                        var key = pre_type_functor.template_param_list[i].GetValue();
                        if (fill_map.ContainsKey(key)) fill_map.Remove(key);
                        fill_map.Add(key, all_type_guess);
                    }
                }

                // 根据填充的参数来分析以及判断
                for (int i = 0; i < value_stat_list.Count; ++i)
                {
                    var value_stat = value_stat_list[i];
                    error = value_stat.GuessType(out ABnfGuess value_stat_guess);
                    if (error != null) return error;
                    // 如果参数返回的类型是tail，那么就可以不用检查
                    if (value_stat_guess is ALittleScriptGuessReturnTail) continue;
                    if (i >= pre_type_functor.param_list.Count) break;

                    // 逐个分析，并且填充模板
                    error = AnalysisTemplate(fill_map, pre_type_functor.param_list[i], value_stat, value_stat_guess, false);
                    if (error != null) return error;
                }

                // 判断如果还未有模板解析，就报错
                foreach (var pair in src_map)
                {
                    if (!fill_map.ContainsKey(pair.Key))
                        return new ABnfGuessError(m_element, pair.Key + "模板无法解析");
                }
            }

            guess = pre_type_functor;
            return null;
        }

        public ABnfGuessError GenerateTemplateParamList(out List<ABnfGuess> param_list)
        {
            param_list = new List<ABnfGuess>();

            var src_map = new Dictionary<string, ALittleScriptGuessTemplate>();
            var fill_map = new Dictionary<string, ABnfGuess>();
            var error = CheckTemplateMap(src_map, fill_map, out ALittleScriptGuessFunctor pre_type_functor);
            if (error != null) return error;
            if (pre_type_functor == null) return null;

            for (int i = 0; i < pre_type_functor.template_param_list.Count; ++i)
            {
                var guess_template = pre_type_functor.template_param_list[i];
                if (guess_template.template_extends != null || guess_template.is_class || guess_template.is_struct)
                {
                    if (fill_map.TryGetValue(guess_template.GetValue(), out ABnfGuess value))
                        param_list.Add(value);
                }
            }

            return null;
        }

        public override ABnfGuessError CheckError()
        {
            var src_map = new Dictionary<string, ALittleScriptGuessTemplate>();
            var fill_map = new Dictionary<string, ABnfGuess>();
            var error = CheckTemplateMap(src_map, fill_map, out ALittleScriptGuessFunctor pre_type_functor);
            if (error != null) return error;
            if (pre_type_functor == null) return new ABnfGuessError(m_element, "括号前面必须是函数");
            
            // 检查填写的和函数定义的参数是否一致
            var value_stat_list = m_element.GetValueStatList();
            for (int i = 0; i < value_stat_list.Count; ++i)
            {
                var value_stat = value_stat_list[i];

                error = ALittleScriptUtility.CalcReturnCount(value_stat, out int return_count, out _);
                if (error != null) return error;
                if (return_count != 1) return new ABnfGuessError(value_stat, "表达式必须只能是一个返回值");

                error = value_stat.GuessType(out ABnfGuess guess);
                if (error != null) return error;
                // 如果参数返回的类型是tail，那么就可以不用检查
                if (guess is ALittleScriptGuessReturnTail) continue;

                if (i >= pre_type_functor.param_list.Count)
                {
                    // 如果有参数占位符，那么就直接跳出，不检查了
                    // 如果没有，就表示超过参数数量了
                    if (pre_type_functor.param_tail != null)
                        break;
                    else
                        return new ABnfGuessError(m_element, "该函数调用需要" + pre_type_functor.param_list.Count + "个参数，而不是" + value_stat_list.Count + "个");
                }

                error = ALittleScriptOp.GuessTypeEqual(pre_type_functor.param_list[i], value_stat, guess, false, false);
                if (error != null)
                    return new ABnfGuessError(value_stat, "第" + (i + 1) + "个参数类型和函数定义的参数类型不同:" + error.GetError());
            }

            // 如果参数数量不足以填充
            if (value_stat_list.Count < pre_type_functor.param_list.Count)
            {
                // 不足的部分，参数必须都是nullable
                for (int i = value_stat_list.Count; i < pre_type_functor.param_nullable_list.Count; ++i)
                {
                    if (!pre_type_functor.param_nullable_list[i])
                    {
                        // 计算至少需要的参数个数
                        int count = pre_type_functor.param_nullable_list.Count;
                        for (int j = pre_type_functor.param_nullable_list.Count - 1; j >= 0; --j)
                        {
                            if (pre_type_functor.param_nullable_list[j])
                                --count;
                            else
                                break;
                        }
                        return new ABnfGuessError(m_element, "该函数调用至少需要" + count + "个参数，而不是" + value_stat_list.Count + "个");
                    }
                }
            }

            // 检查这个函数是不是await
            if (pre_type_functor.await_modifier)
            {
                // 检查这次所在的函数必须要有await或者async修饰
                error = ALittleScriptUtility.CheckInvokeAwait(m_element);
                if (error != null) return error;
            }
            return null;
        }
    }
}

