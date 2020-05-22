
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptMethodBodyDecReference : ALittleScriptReferenceTemplate<ALittleScriptMethodBodyDecElement>
    {
        public ALittleScriptMethodBodyDecReference(ABnfElement element) : base(element)
        {

        }

        // 检查表达式是否有return
        public static ABnfGuessError CheckAllExpr(List<ABnfGuess> return_list, ALittleScriptAllExprElement all_expr, out bool result)
        {
            result = false;
            if (all_expr.GetIfExpr() != null)
            {
                var if_expr = all_expr.GetIfExpr();
                var sub_all_expr = if_expr.GetAllExpr();
                var if_body = if_expr.GetIfBody();
                if (sub_all_expr != null)
                {
                    var error = CheckAllExpr(return_list, sub_all_expr, out bool sub_result);
                    if (error != null) return error;
                    if (!sub_result) return null;
                }
                else if (if_body != null)
                {
                    var error = CheckAllExprList(return_list, if_body.GetAllExprList(), out bool sub_result);
                    if (error != null) return error;
                    if (!sub_result) return null;
                }
                else
                {
                    return null;
                }

                var else_if_expr_list = if_expr.GetElseIfExprList();
                foreach (var else_if_expr in else_if_expr_list)
                {
                    sub_all_expr = else_if_expr.GetAllExpr();
                    var else_if_body = else_if_expr.GetElseIfBody();
                    if (sub_all_expr != null)
                    {
                        var error = CheckAllExpr(return_list, sub_all_expr, out bool sub_result);
                        if (error != null) return error;
                        if (!sub_result) return null;
                    }
                    else if (else_if_body != null)
                    {
                        var error = CheckAllExprList(return_list, if_body.GetAllExprList(), out bool sub_result);
                        if (error != null) return error;
                        if (!sub_result) return null;
                    }
                    else
                    {
                        return null;
                    }
                }

                var else_expr = if_expr.GetElseExpr();
                if (else_expr == null) return null;

                sub_all_expr = else_expr.GetAllExpr();
                var else_body = else_expr.GetElseBody();
                if (sub_all_expr != null)
                    return CheckAllExpr(return_list, sub_all_expr, out result);
                else if (else_body != null)
                    return CheckAllExprList(return_list, else_body.GetAllExprList(), out result);
                else
                    return null;
            }
/*
            if (all_expr.GetForExpr() != null)
            {
                var for_expr = all_expr.GetForExpr();
                var sub_all_expr = for_expr.GetAllExpr();
                var for_body = for_expr.GetForBody();
                if (sub_all_expr != null)
                    return CheckAllExpr(return_list, sub_all_expr, out result);
                else if (for_body != null)
                    return CheckAllExprList(return_list, for_body.GetAllExprList(), out result);
                else
                    return null;
            }

            if (all_expr.GetWhileExpr() != null)
            {
                var while_expr = all_expr.GetWhileExpr();
                var sub_all_expr = while_expr.GetAllExpr();
                var while_body = while_expr.GetWhileBody();
                if (sub_all_expr != null)
                    return CheckAllExpr(return_list, sub_all_expr, out result);
                else if (while_body != null)
                    return CheckAllExprList(return_list, while_body.GetAllExprList(), out result);
                else
                    return null;
            }
*/
            if (all_expr.GetDoWhileExpr() != null)
            {
                var do_while_expr = all_expr.GetDoWhileExpr();
                var do_while_body = do_while_expr.GetDoWhileBody();
                if (do_while_body != null)
                    return CheckAllExprList(return_list, do_while_body.GetAllExprList(), out result);
                else
                    return null;
            }

            if (all_expr.GetReturnExpr() != null)
            {
                result = true;
                // 这里检查return
                return null;
            }

            if (all_expr.GetWrapExpr() != null)
            {
                var wrap_expr = all_expr.GetWrapExpr();
                return CheckAllExprList(return_list, wrap_expr.GetAllExprList(), out result);
            }

            if (all_expr.GetThrowExpr() != null)
            {
                result = true;
                return null;
            }

            return null;
        }

        // 检查表达式是否有return
        public static ABnfGuessError CheckAllExprList(List<ABnfGuess> return_list, List<ALittleScriptAllExprElement> all_expr_list, out bool result)
        {
            result = false;
            // 如果没有就检查子表达式
            int index = -1;
            for (int i = 0; i < all_expr_list.Count; ++i)
            {
                var all_expr = all_expr_list[i];
                if (!ALittleScriptUtility.IsLanguageEnable(all_expr.GetModifierList()))
                    continue;

                var error = CheckAllExpr(return_list, all_expr_list[i], out bool sub_result);
                if (error != null) return error;
                if (sub_result)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
                return null;

            for (int i = index + 1; i < all_expr_list.Count; ++i)
            {
                var all_expr = all_expr_list[i];
                if (!ALittleScriptUtility.IsLanguageEnable(all_expr.GetModifierList()))
                    continue;
                return new ABnfGuessError(all_expr, "当前分支内，从这里开始之后所有语句永远都不会执行到");
            }
            
            result = true;
            return null;
        }

        // 检查函数体
        private static ABnfGuessError CheckMethodBody(List<ABnfGuess> return_list
            , ALittleScriptMethodNameDecElement method_name_dec
            , ALittleScriptMethodBodyDecElement method_body_dec)
        {
            // 检查return
            if (return_list.Count > 0 && !ALittleScriptUtility.IsRegister(method_name_dec))
            {
                var all_expr_list = method_body_dec.GetAllExprList();
                var error = CheckAllExprList(return_list, all_expr_list, out bool result);
                if (error != null) return error;
                if (!result)
                    return new ABnfGuessError(method_name_dec, "不是所有分支都有return");
            }

            return null;
        }


        public override ABnfGuessError CheckError()
        {
            var parent = m_element.GetParent();

            if (parent is ALittleScriptClassCtorDecElement) return null;
            if (parent is ALittleScriptClassSetterDecElement) return null;

            var return_list = new List<ABnfGuess>();
            ALittleScriptMethodReturnDecElement return_dec = null;
            ALittleScriptMethodNameDecElement name_dec = null;

            if (parent is ALittleScriptClassGetterDecElement)
            {
                var getter_dec = parent as ALittleScriptClassGetterDecElement;
                name_dec = getter_dec.GetMethodNameDec();
                if (name_dec == null) return null;

                var all_type = getter_dec.GetAllType();
                if (all_type == null) return null;
                var error = all_type.GuessType(out ABnfGuess all_type_guess);
                if (error != null) return error;
                return_list.Add(all_type_guess);
                return CheckMethodBody(return_list, name_dec, m_element);
            }

            if (parent is ALittleScriptClassMethodDecElement)
            {
                var method_dec = parent as ALittleScriptClassMethodDecElement;
                name_dec = method_dec.GetMethodNameDec();
                if (name_dec == null) return null;
                return_dec = method_dec.GetMethodReturnDec();
            }

            if (parent is ALittleScriptClassStaticDecElement)
            {
                var static_dec = parent as ALittleScriptClassStaticDecElement;
                name_dec = static_dec.GetMethodNameDec();
                if (name_dec == null) return null;
                return_dec = static_dec.GetMethodReturnDec();
            }

            if (parent is ALittleScriptGlobalMethodDecElement) {
                var global_method_dec = parent as ALittleScriptGlobalMethodDecElement;
                name_dec = global_method_dec.GetMethodNameDec();
                if (name_dec == null) return null;
                return_dec = global_method_dec.GetMethodReturnDec();
            }

            if (name_dec == null) return null;

            if (return_dec != null)
            {
                var return_one_list = return_dec.GetMethodReturnOneDecList();
                for (int i = 0; i < return_one_list.Count; ++i)
                {
                    var return_one = return_one_list[i];
                    var all_type = return_one.GetAllType();
                    var return_tail = return_one.GetMethodReturnTailDec();
                    if (all_type != null)
                    {
                        var error = all_type.GuessType(out ABnfGuess all_type_guess);
                        if (error != null) return error;
                        return_list.Add(all_type_guess);
                    }
                    else if (return_tail != null)
                    {
                        if (i + 1 != return_one_list.Count)
                            return new ABnfGuessError(return_one, "返回值占位符必须定义在最后");
                        var error = return_tail.GuessType(out ABnfGuess return_tail_guess);
                        if (error != null) return error;
                        return_list.Add(return_tail_guess);
                    }
                }
            }

            return CheckMethodBody(return_list, name_dec, m_element);
        }
    }
}

