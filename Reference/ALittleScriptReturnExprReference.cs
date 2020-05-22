
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptReturnExprReference : ALittleScriptReferenceTemplate<ALittleScriptReturnExprElement>
    {
        public ALittleScriptReturnExprReference(ABnfElement element) : base(element)
        {
        }

        public override ABnfGuessError CheckError()
        {
            ABnfElement parent = null;
            if (m_element.GetReturnYield() != null)
            {
                // 对于ReturnYield就不需要做返回值检查
                // 对所在函数进行检查，必须要有async和await表示
                // 获取对应的函数对象
                ABnfElement element = null;

                parent = m_element;
                while (parent != null)
                {
                    if (parent is ALittleScriptClassMethodDecElement)
                    {
                        var method_dec = parent as ALittleScriptClassMethodDecElement;
                        var modifier = (method_dec.GetParent() as ALittleScriptClassElementDecElement).GetModifierList();
                        if (ALittleScriptUtility.GetCoroutineType(modifier) == null)
                        {
                            element = method_dec.GetMethodNameDec();
                            if (element == null) element = method_dec;
                        }
                        break;
                    }
                    else if (parent is ALittleScriptClassStaticDecElement)
                    {
                        var method_dec = parent as ALittleScriptClassStaticDecElement;
                        var modifier = (method_dec.GetParent() as ALittleScriptClassElementDecElement).GetModifierList();
                        if (ALittleScriptUtility.GetCoroutineType(modifier) == null)
                        {
                            element = method_dec.GetMethodNameDec();
                            if (element == null) element = method_dec;
                        }
                        break;
                    }
                    else if (parent is ALittleScriptGlobalMethodDecElement)
                    {
                        var method_dec = parent as ALittleScriptGlobalMethodDecElement;
                        var modifier = (method_dec.GetParent() as ALittleScriptNamespaceElementDecElement).GetModifierList();
                        if (ALittleScriptUtility.GetCoroutineType(modifier) == null)
                        {
                            element = method_dec.GetMethodNameDec();
                            if (element == null) element = method_dec;
                        }
                        break;
                    }

                    parent = parent.GetParent();
                }

                if (element != null)
                    return new ABnfGuessError(element, "函数内部使用了return yield表达式，所以必须使用async或await修饰");
                return null;
            }

            var value_stat_list = m_element.GetValueStatList();
            var return_type_list = new List<ALittleScriptAllTypeElement>();
            ALittleScriptMethodReturnTailDecElement return_tail_dec = null;

            // 获取对应的函数对象
            parent = m_element;
            while (parent != null)
            {
                if (parent is ALittleScriptClassGetterDecElement)
                {
                    var getterDec = parent as ALittleScriptClassGetterDecElement;
                    return_type_list.Clear();
                    var return_type_dec = getterDec.GetAllType();
                    if (return_type_dec != null)
                        return_type_list.Add(return_type_dec);
                    break;
                }
                else if (parent is ALittleScriptClassSetterDecElement)
                {
                    break;
                }
                else if (parent is ALittleScriptClassMethodDecElement)
                {
                    var method_dec = parent as ALittleScriptClassMethodDecElement;
                    var return_dec = method_dec.GetMethodReturnDec();
                    if (return_dec != null)
                    {
                        var return_one_list = return_dec.GetMethodReturnOneDecList();
                        foreach (var return_one in return_one_list)
                        {
                            var all_type = return_one.GetAllType();
                            if (all_type != null) return_type_list.Add(all_type);

                            var return_tail = return_one.GetMethodReturnTailDec();
                            if (return_tail != null) return_tail_dec = return_tail;
                        }
                    }
                    break;
                }
                else if (parent is ALittleScriptClassStaticDecElement)
                {
                    var method_dec = parent as ALittleScriptClassStaticDecElement;
                    var return_dec = method_dec.GetMethodReturnDec();
                    if (return_dec != null)
                    {
                        var return_one_list = return_dec.GetMethodReturnOneDecList();
                        foreach (var return_one in return_one_list)
                        {
                            var all_type = return_one.GetAllType();
                            if (all_type != null) return_type_list.Add(all_type);

                            var return_tail = return_one.GetMethodReturnTailDec();
                            if (return_tail != null) return_tail_dec = return_tail;
                        }
                    }
                    break;
                }
                else if (parent is ALittleScriptGlobalMethodDecElement)
                {
                    var method_dec = parent as ALittleScriptGlobalMethodDecElement;
                    var return_dec = method_dec.GetMethodReturnDec();
                    if (return_dec != null)
                    {
                        var return_one_list = return_dec.GetMethodReturnOneDecList();
                        foreach (var return_one in return_one_list)
                        {
                            var all_type = return_one.GetAllType();
                            if (all_type != null) return_type_list.Add(all_type);

                            var return_tail = return_one.GetMethodReturnTailDec();
                            if (return_tail != null) return_tail_dec = return_tail;
                        }
                    }
                    break;
                }

                parent = parent.GetParent();
            }

            // 参数的类型
            List<ABnfGuess> guess_list = null;
            // 如果返回值只有一个函数调用
            if (value_stat_list.Count == 1 && (return_type_list.Count > 1 || return_tail_dec != null))
            {
                var value_stat = value_stat_list[0];
                var error = value_stat.GuessTypes(out guess_list);
                if (error != null) return error;
                bool has_value_tail = guess_list.Count > 0
                        && guess_list[guess_list.Count - 1] is ALittleScriptGuessReturnTail;

                if (return_tail_dec == null)
                {
                    if (has_value_tail)
                    {
                        if (guess_list.Count < return_type_list.Count - 1)
                            return new ABnfGuessError(m_element, "return的函数调用的返回值数量超过函数定义的返回值数量");
                    }
                    else
                    {
                        if (guess_list.Count != return_type_list.Count)
                            return new ABnfGuessError(m_element, "return的函数调用的返回值数量和函数定义的返回值数量不相等");
                    }
                }
                else
                {
                    if (has_value_tail)
                    {
                        // 不用检查
                    }
                    else
                    {
                        if (guess_list.Count < return_type_list.Count)
                            return new ABnfGuessError(m_element, "return的函数调用的返回值数量少于函数定义的返回值数量");
                    }
                }
            }
            else
            {
                if (return_tail_dec == null)
                {
                    if (value_stat_list.Count != return_type_list.Count)
                        return new ABnfGuessError(m_element, "return的返回值数量和函数定义的返回值数量不相等");
                }
                else
                {
                    if (value_stat_list.Count < return_type_list.Count)
                        return new ABnfGuessError(m_element, "return的返回值数量少于函数定义的返回值数量");
                }
                guess_list = new List<ABnfGuess>();
                foreach (var value_stat in value_stat_list)
                {
                    var error = ALittleScriptUtility.CalcReturnCount(value_stat, out int return_count, out _);
                    if (error != null) return error;
                    if (return_count != 1) return new ABnfGuessError(value_stat, "表达式必须只能是一个返回值");

                    error = value_stat.GuessType(out ABnfGuess guess);
                    if (error != null) return error;
                    if (guess is ALittleScriptGuessParamTail)
                        return new ABnfGuessError(value_stat, "return表达式不能返回\"...\"");
                    error = value_stat.GuessType(out ABnfGuess value_stat_guess);
                    if (error != null) return error;
                    guess_list.Add(value_stat_guess);
                }
            }

            // 每个类型依次检查
            for (int i = 0; i < guess_list.Count; ++i)
            {
                ALittleScriptValueStatElement target_value_stat = null;
                if (i < value_stat_list.Count)
                    target_value_stat = value_stat_list[i];
                else
                    target_value_stat = value_stat_list[0];

                if (guess_list[i] is ALittleScriptGuessReturnTail) break;
                if (i >= return_type_list.Count) break;
                var error = return_type_list[i].GuessType(out ABnfGuess return_type_guess);
                if (error != null) return error;
                if (return_type_guess is ALittleScriptGuessReturnTail) break;

                error = ALittleScriptOp.GuessTypeEqual(return_type_guess, target_value_stat, guess_list[i], false, true);
                if (error != null)
                    return new ABnfGuessError(target_value_stat, "return的第" + (i + 1) + "个返回值数量和函数定义的返回值类型不同:" + error.GetError());
            }

            return null;
        }
    }
}

