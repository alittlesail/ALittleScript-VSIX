
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptOpNewStatReference : ALittleScriptReferenceTemplate<ALittleScriptOpNewStatElement>
    {
        public ALittleScriptOpNewStatReference(ABnfElement element) : base(element)
        {
        }

        public override ABnfGuessError GuessTypes(out List<ABnfGuess> guess_list)
        {
            if (m_element.GetCustomType() != null)
                return m_element.GetCustomType().GuessTypes(out guess_list);
            else if (m_element.GetGenericType() != null)
                return m_element.GetGenericType().GuessTypes(out guess_list);
            guess_list = null;
            return new ABnfGuessError(m_element, "ALittleOpNewStat出现未知的子节点");
        }

        public override ABnfGuessError CheckError()
        {
            var value_stat_list = m_element.GetValueStatList();

            if (m_element.GetGenericType() != null)
            {
                if (value_stat_list.Count > 0)
                    return new ABnfGuessError(m_element, "创建容器实例对象不能有参数");
                return null;
            }

            if (m_element.GetCustomType() != null)
            {
                var custom_type = m_element.GetCustomType();
                var error = custom_type.GuessType(out ABnfGuess guess);
                if (error != null) return error;
                if (guess is ALittleScriptGuessStruct)
                {
                    if (value_stat_list.Count > 0)
                        return new ABnfGuessError(m_element, "new的结构体不能有参数");
                    return null;
                }

                if (guess is ALittleScriptGuessMap)
                {
                    if (value_stat_list.Count > 0)
                        return new ABnfGuessError(m_element, "new的Map不能有参数");
                    return null;
                }

                if (guess is ALittleScriptGuessList)
                {
                    if (value_stat_list.Count > 0)
                        return new ABnfGuessError(m_element, "new的List不能有参数");
                    return null;
                }

                if (guess is ALittleScriptGuessTemplate)
                {
                    var guess_template = guess as ALittleScriptGuessTemplate;
                    if (guess_template.template_extends != null)
                        guess = guess_template.template_extends;
                    else if (guess_template.is_struct)
                    {
                        if (value_stat_list.Count > 0)  return new ABnfGuessError(m_element, "new的结构体不能有参数");
                        return null;
                    }
                    else if (guess_template.is_class)
                    {
                        return new ABnfGuessError(m_element, "如果要new改模板类型，请不要使用class，无法确定它的构造函数参数");
                    }
                }

                if (guess is ALittleScriptGuessStruct)
                {
                    if (value_stat_list.Count > 0) return new ABnfGuessError(m_element, "new的结构体不能有参数");
                    return null;
                }

                if (guess is ALittleScriptGuessClass)
                {
                    var class_dec = (guess as ALittleScriptGuessClass).class_dec;
                    var ctor_dec = ALittleScriptUtility.FindFirstCtorDecFromExtends(class_dec, 100);
                    if (ctor_dec == null)
                    {
                        if (value_stat_list.Count > 0)
                            return new ABnfGuessError(m_element, "new的类的构造函数没有参数");
                        return null;
                    }

                    var param_dec = ctor_dec.GetMethodParamDec();
                    if (param_dec == null)
                    {
                        if (value_stat_list.Count > 0)
                            return new ABnfGuessError(m_element, "new的类的构造函数没有参数");
                        return null;
                    }

                    var param_one_dec_list = param_dec.GetMethodParamOneDecList();
                    var param_guess_list = new List<ABnfGuess>();
                    var param_nullable_list = new List<bool>();
                    bool has_param_tail = false;
                    foreach (var param_one_dec in param_one_dec_list)
                    {
                        var all_type = param_one_dec.GetAllType();
                        var param_tail = param_one_dec.GetMethodParamTailDec();
                        if (all_type != null)
                        {
                            error = all_type.GuessType(out ABnfGuess all_type_guess);
                            if (error != null) return error;
                            param_guess_list.Add(all_type_guess);
                            param_nullable_list.Add(ALittleScriptUtility.IsNullable(param_one_dec.GetModifierList()));
                        }
                        else if (param_tail != null)
                        {
                            has_param_tail = true;
                        }   
                    }

                    // 如果参数数量不足以填充
                    if (value_stat_list.Count < param_guess_list.Count)
                    {
                        // 不足的部分，参数必须都是nullable
                        for (int i = value_stat_list.Count; i < param_nullable_list.Count; ++i)
                        {
                            if (!param_nullable_list[i])
                            {
                                // 计算至少需要的参数个数
                                int count = param_nullable_list.Count;
                                for (int j = param_nullable_list.Count - 1; j >= 0; --j)
                                {
                                    if (param_nullable_list[j])
                                        --count;
                                    else
                                        break;
                                }
                                return new ABnfGuessError(m_element, "new的类的构造函数调用需要" + count + "个参数,不能是:" + value_stat_list.Count + "个");
                            }
                        }
                    }

                    for (int i = 0; i < value_stat_list.Count; ++i)
                    {
                        var value_stat = value_stat_list[i];

                        error = ALittleScriptUtility.CalcReturnCount(value_stat, out int return_count, out _);
                        if (error != null) return error;
                        if (return_count != 1) return new ABnfGuessError(value_stat, "表达式必须只能是一个返回值");

                        error = value_stat.GuessType(out ABnfGuess value_stat_guess);
                        if (error != null) return error;
                        // 如果参数返回的类型是tail，那么就可以不用检查
                        if (value_stat_guess is ALittleScriptGuessReturnTail) continue;

                        if (i >= param_guess_list.Count)
                        {
                            // 如果有参数占位符，那么就直接跳出，不检查了
                            // 如果没有，就表示超过参数数量了
                            if (has_param_tail)
                                break;
                            else
                                return new ABnfGuessError(m_element, "该构造函数调用需要" + param_guess_list.Count + "个参数，而不是" + value_stat_list.Count + "个");
                        }
                        
                        error = ALittleScriptOp.GuessTypeEqual(param_guess_list[i], value_stat, value_stat_guess, false, false);
                        if (error != null)
                            return new ABnfGuessError(value_stat, "第" + (i + 1) + "个参数类型和函数定义的参数类型不同:" + error.GetError());
                    }
                    return null;
                }

                return new ABnfGuessError(m_element, "只能new结构体和类");
            }

            return null;
        }
    }
}

