
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptMethodNameDecReference : ALittleScriptReferenceTemplate<ALittleScriptMethodNameDecElement>
    {
        private string m_namespace_name;
        private string m_key;

        public ALittleScriptMethodNameDecReference(ABnfElement element) : base(element)
        {
            m_namespace_name = ALittleScriptUtility.GetNamespaceName(m_element);
            m_key = m_element.GetElementText();
        }

        public override string QueryClassificationTag(out bool blur)
        {
            blur = false;
            return "ALittleScriptMethodName";
        }

        public override ABnfGuessError GuessTypes(out List<ABnfGuess> guess_list)
        {
            guess_list = new List<ABnfGuess>();
            var parent = m_element.GetParent();
            // 处理getter
            if (parent is ALittleScriptClassGetterDecElement)
            {
                var class_getter_dec = parent as ALittleScriptClassGetterDecElement;
                var class_element_dec = class_getter_dec.GetParent() as ALittleScriptClassElementDecElement;
                var class_body_dec = class_element_dec.GetParent();
                var class_dec = class_body_dec.GetParent() as ALittleScriptClassDecElement;

                var info = new ALittleScriptGuessFunctor(class_getter_dec);
                info.const_modifier = ALittleScriptUtility.IsConst(class_element_dec.GetModifierList());

                // 第一个参数是类
                var error = class_dec.GuessType(out ABnfGuess class_guess);
                if (error != null) return error;
                info.param_list.Add(class_guess);
                info.param_nullable_list.Add(false);
                info.param_name_list.Add(class_guess.GetValue());

                // 添加返回值列表
                var all_type = class_getter_dec.GetAllType();
                if (all_type == null) return new ABnfGuessError(m_element, "指向的getter函数没有定义返回值");
                error = all_type.GuessType(out ABnfGuess all_type_guess);
                if (error != null) return error;
                info.return_list.Add(all_type_guess);

                info.UpdateValue();
                guess_list.Add(info);
            }
            else if (parent is ALittleScriptClassSetterDecElement)
            {
                var class_setter_dec = parent as ALittleScriptClassSetterDecElement;
                var class_element_dec = class_setter_dec.GetParent() as ALittleScriptClassElementDecElement;
                var class_body_dec = class_element_dec.GetParent();
                var class_dec = class_body_dec.GetParent() as ALittleScriptClassDecElement;

                var info = new ALittleScriptGuessFunctor(class_setter_dec);
                info.const_modifier = ALittleScriptUtility.IsConst(class_element_dec.GetModifierList());

                // 第一个参数是类
                var error = class_dec.GuessType(out ABnfGuess class_guess);
                if (error != null) return error;
                info.param_list.Add(class_guess);
                info.param_nullable_list.Add(false);
                info.param_name_list.Add(class_guess.GetValue());

                var param_dec = class_setter_dec.GetMethodSetterParamDec();
                if (param_dec == null)
                    return new ABnfGuessError(m_element, "指向的setter函数没有定义参数");

                // 添加参数列表
                var one_dec = param_dec.GetMethodParamOneDec();
                if (one_dec == null)
                    return new ABnfGuessError(m_element, "指向的setter函数没有定义参数");
                var all_type = one_dec.GetAllType();
                if (all_type == null)
                    return new ABnfGuessError(m_element, "指向的setter函数没有定义参数类型");
                error = all_type.GuessType(out ABnfGuess all_type_guess);
                if (error != null) return error;
                info.param_list.Add(all_type_guess);
                info.param_nullable_list.Add(ALittleScriptUtility.IsNullable(one_dec.GetModifierList()));
                if (one_dec.GetMethodParamNameDec() != null) {
                    info.param_name_list.Add(one_dec.GetMethodParamNameDec().GetElementText());
                } else {
                    info.param_name_list.Add("");
                }

                info.UpdateValue();
                guess_list.Add(info);
            } else if (parent is ALittleScriptClassMethodDecElement) {
                var class_method_dec = parent as ALittleScriptClassMethodDecElement;
                var class_element_dec = class_method_dec.GetParent() as ALittleScriptClassElementDecElement;
                var class_body_dec = class_element_dec.GetParent();
                var class_dec = class_body_dec.GetParent() as ALittleScriptClassDecElement;

                var info = new ALittleScriptGuessFunctor(class_method_dec);
                var modifier = class_element_dec.GetModifierList();
                info.const_modifier = ALittleScriptUtility.IsConst(class_element_dec.GetModifierList());
                info.await_modifier = ALittleScriptUtility.GetCoroutineType(modifier) == "await";

                // 第一个参数是类
                var error = class_dec.GuessType(out ABnfGuess class_guess);
                if (error != null) return error;
                info.param_list.Add(class_guess);
                info.param_nullable_list.Add(false);
                info.param_name_list.Add(class_guess.GetValue());

                // 添加模板参数列表
                var template_dec = class_method_dec.GetTemplateDec();
                if (template_dec != null)
                {
                    error = template_dec.GuessTypes(out List<ABnfGuess> template_guess_list);
                    if (error != null) return error;
                    foreach (var guess in template_guess_list)
                    {
                        if (!(guess is ALittleScriptGuessTemplate))
                            return new ABnfGuessError(m_element, "template_dec.GuessTypes()取到的不是ALittleScriptGuessTemplate");
                        info.template_param_list.Add(guess as ALittleScriptGuessTemplate);
                    }
                }

                // 添加参数列表
                var param_dec = class_method_dec.GetMethodParamDec();
                if (param_dec != null)
                {
                    var one_dec_list = param_dec.GetMethodParamOneDecList();
                    for (int i = 0; i < one_dec_list.Count; ++i)
                    {
                        var one_dec = one_dec_list[i];
                        var all_type = one_dec.GetAllType();
                        var param_tail = one_dec.GetMethodParamTailDec();
                        if (all_type != null)
                        {
                            error = all_type.GuessType(out ABnfGuess all_type_guess);
                            if (error != null) return error;
                            info.param_list.Add(all_type_guess);
                            info.param_nullable_list.Add(ALittleScriptUtility.IsNullable(one_dec.GetModifierList()));
                            if (one_dec.GetMethodParamNameDec() != null)
                                info.param_name_list.Add(one_dec.GetMethodParamNameDec().GetElementText());
                            else
                                info.param_name_list.Add("");
                        }
                        else if (param_tail != null)
                        {
                            if (i + 1 != one_dec_list.Count)
                                return new ABnfGuessError(one_dec, "参数占位符必须定义在最后");
                            error = param_tail.GuessType(out info.param_tail);
                            if (error != null) return error;
                        }
                    }
                }

                // 添加返回值列表
                var return_dec = class_method_dec.GetMethodReturnDec();
                if (return_dec != null)
                {
                    var one_dec_list = return_dec.GetMethodReturnOneDecList();
                    for (int i = 0; i < one_dec_list.Count; ++i)
                    {
                        var one_dec = one_dec_list[i];
                        var all_type = one_dec.GetAllType();
                        var return_tail = one_dec.GetMethodReturnTailDec();
                        if (all_type != null)
                        {
                            error = all_type.GuessType(out ABnfGuess all_type_guess);
                            if (error != null) return error;
                            info.return_list.Add(all_type_guess);
                        }
                        else if (return_tail != null)
                        {
                            if (i + 1 != one_dec_list.Count)
                                return new ABnfGuessError(one_dec, "返回值占位符必须定义在最后");
                            error = return_tail.GuessType(out info.return_tail);
                            if (error != null) return error;
                        }
                    }
                }
                info.UpdateValue();
                guess_list.Add(info);
            }
            else if (parent is ALittleScriptClassStaticDecElement)
            {
                var class_static_dec = parent as ALittleScriptClassStaticDecElement;
                var class_element_dec = class_static_dec.GetParent() as ALittleScriptClassElementDecElement;

                var info = new ALittleScriptGuessFunctor(class_static_dec);
                info.await_modifier = ALittleScriptUtility.GetCoroutineType(class_element_dec.GetModifierList()) == "await";

                // 添加模板参数列表
                var template_dec = class_static_dec.GetTemplateDec();
                if (template_dec != null)
                {
                    var error = template_dec.GuessTypes(out List<ABnfGuess> template_guess_list);
                    if (error != null) return error;
                    foreach (var guess in template_guess_list)
                    {
                        if (!(guess is ALittleScriptGuessTemplate))
                            return new ABnfGuessError(m_element, "template_dec.GuessTypes()取到的不是ALittleScriptGuessTemplate");
                        info.template_param_list.Add(guess as ALittleScriptGuessTemplate);
                    }
                }

                // 添加参数列表
                var param_dec = class_static_dec.GetMethodParamDec();
                if (param_dec != null)
                {
                    var one_dec_list = param_dec.GetMethodParamOneDecList();
                    for (int i = 0; i < one_dec_list.Count; ++i)
                    {
                        var one_dec = one_dec_list[i];
                        var all_type = one_dec.GetAllType();
                        var param_tail = one_dec.GetMethodParamTailDec();
                        if (all_type != null)
                        {
                            var error = all_type.GuessType(out ABnfGuess all_type_guess);
                            if (error != null) return error;
                            info.param_list.Add(all_type_guess);
                            info.param_nullable_list.Add(ALittleScriptUtility.IsNullable(one_dec.GetModifierList()));
                            if (one_dec.GetMethodParamNameDec() != null)
                                info.param_name_list.Add(one_dec.GetMethodParamNameDec().GetElementText());
                            else
                                info.param_name_list.Add("");
                        }
                        else if (param_tail != null)
                        {
                            if (i + 1 != one_dec_list.Count)
                                return new ABnfGuessError(one_dec, "参数占位符必须定义在最后");
                            var error = param_tail.GuessType(out info.param_tail);
                            if (error != null) return error;
                        }
                    }
                }

                // 添加返回值列表
                var return_dec = class_static_dec.GetMethodReturnDec();
                if (return_dec != null)
                {
                    var one_dec_list = return_dec.GetMethodReturnOneDecList();
                    for (int i = 0; i < one_dec_list.Count; ++i)
                    {
                        var one_dec = one_dec_list[i];
                        var all_type = one_dec.GetAllType();
                        var return_tail = one_dec.GetMethodReturnTailDec();
                        if (all_type != null)
                        {
                            var error = all_type.GuessType(out ABnfGuess all_type_guess);
                            if (error != null) return error;
                            info.return_list.Add(all_type_guess);
                        }
                        else if (return_tail != null)
                        {
                            if (i + 1 != one_dec_list.Count)
                                return new ABnfGuessError(one_dec, "返回值占位符必须定义在最后");
                            var error = return_tail.GuessType(out info.return_tail);
                            if (error != null) return error;
                        }
                    }
                }
                info.UpdateValue();
                guess_list.Add(info);
            } 
            else if (parent is ALittleScriptGlobalMethodDecElement) 
            {
                var global_method_dec = parent as ALittleScriptGlobalMethodDecElement;
                var namespace_element_dec = global_method_dec.GetParent() as ALittleScriptNamespaceElementDecElement;

                var info = new ALittleScriptGuessFunctor(global_method_dec);
                info.await_modifier = ALittleScriptUtility.GetCoroutineType(namespace_element_dec.GetModifierList()) == "await";

                var protocol_type = ALittleScriptUtility.GetProtocolType(namespace_element_dec.GetModifierList());
                if (protocol_type != null)
                {
                    ABnfElement error_element = global_method_dec.GetMethodNameDec();
                    if (error_element == null) error_element = global_method_dec;

                    if (global_method_dec.GetTemplateDec() != null)
                        return new ABnfGuessError(error_element, "带" + info.proto + "不能定义函数模板");

                    // 如果是带协议注解，那么一定是一个await
                    info.await_modifier = true;
                    info.proto = protocol_type;

                    var param_dec = global_method_dec.GetMethodParamDec();
                    if (param_dec == null) return new ABnfGuessError(error_element, "带" + info.proto + "注解的函数必须是两个参数");
                    var one_dec_list = param_dec.GetMethodParamOneDecList();
                    if (one_dec_list.Count != 2) return new ABnfGuessError(error_element, "带" + info.proto + "注解的函数必须是两个参数");
                    if (ALittleScriptUtility.IsNullable(one_dec_list[0].GetModifierList()) || ALittleScriptUtility.IsNullable(one_dec_list[1].GetModifierList()))
                        return new ABnfGuessError(error_element, "带" + info.proto + "注解的函数参数不能使用Nullable修饰");
                    var all_type = one_dec_list[1].GetAllType();
                    if (all_type == null) return new ABnfGuessError(error_element, "带" + info.proto + "注解的函数，第二个参数没有定义类型");
                    var error = all_type.GuessType(out ABnfGuess guess);
                    if (error != null) return error;
                    if (!(guess is ALittleScriptGuessStruct)) return new ABnfGuessError(error_element, "带" + info.proto + "注解的函数第二个参数必须是struct");

                    if (info.proto == "Http")
                    {
                        ABnfElement element = ALittleScriptIndex.inst.FindALittleNameDec(ALittleScriptUtility.ABnfElementType.CLASS_NAME, m_element.GetFile(), "ALittle", "IHttpSender", true);
                        if (!(element is ALittleScriptClassNameDecElement)) return new ABnfGuessError(error_element, "语言框架中找不到ALittle.IHttpSender");
                        var class_name_dec = element as ALittleScriptClassNameDecElement;
                        error = class_name_dec.GuessType(out ABnfGuess class_name_dec_guess);
                        if (error != null) return error;
                        info.param_list.Add(class_name_dec_guess);
                        info.param_nullable_list.Add(false);
                        info.param_name_list.Add("sender");
                        info.param_list.Add(guess);
                        info.param_nullable_list.Add(false);
                        info.param_name_list.Add("param");

                        var return_dec = global_method_dec.GetMethodReturnDec();
                        if (return_dec == null) return new ABnfGuessError(error_element, "带" + info.proto + "注解的函数返回值必须是struct");
                        var return_one_list = return_dec.GetMethodReturnOneDecList();
                        if (return_one_list.Count != 1) return new ABnfGuessError(error_element, "带" + info.proto + "注解的函数返回值有且仅有一个struct");
                        var return_one_all_type = return_one_list[0].GetAllType();
                        if (return_one_all_type == null) return new ABnfGuessError(error_element, "带" + info.proto + "注解的函数返回值有且仅有一个struct");
                        error = return_one_all_type.GuessType(out ABnfGuess return_guess);
                        if (error != null) return error;
                        if (!(return_guess is ALittleScriptGuessStruct)) return new ABnfGuessError(error_element, "带" + info.proto + "注解的函数返回值必须是struct");
                        info.return_list.Add(ALittleScriptIndex.inst.sStringGuess);
                        info.return_list.Add(return_guess);
                    }
                    else if (info.proto == "HttpDownload")
                    {
                        ABnfElement element = ALittleScriptIndex.inst.FindALittleNameDec(ALittleScriptUtility.ABnfElementType.CLASS_NAME, m_element.GetFile(), "ALittle", "IHttpFileSender", true);
                        if (!(element is ALittleScriptClassNameDecElement)) return new ABnfGuessError(error_element, "语言框架中找不到ALittle.IHttpFileSender");
                        var class_name_dec = element as ALittleScriptClassNameDecElement;
                        error = class_name_dec.GuessType(out ABnfGuess class_name_dec_guess);
                        if (error != null) return error;
                        info.param_list.Add(class_name_dec_guess);
                        info.param_nullable_list.Add(false);
                        info.param_name_list.Add("sender");
                        info.param_list.Add(guess);
                        info.param_nullable_list.Add(false);
                        info.param_name_list.Add("param");

                        info.return_list.Add(ALittleScriptIndex.inst.sStringGuess);
                        error = element.GuessType(out ABnfGuess sender_guess);
                        if (error != null) return error;
                        info.return_list.Add(sender_guess);
                    }
                    else if (info.proto == "HttpUpload")
                    {
                        ABnfElement element = ALittleScriptIndex.inst.FindALittleNameDec(ALittleScriptUtility.ABnfElementType.CLASS_NAME, m_element.GetFile(), "ALittle", "IHttpFileSender", true);
                        if (!(element is ALittleScriptClassNameDecElement)) return new ABnfGuessError(error_element, "语言框架中找不到ALittle.IHttpFileSender");
                        var class_name_dec = element as ALittleScriptClassNameDecElement;
                        error = class_name_dec.GuessType(out ABnfGuess class_name_dec_guess);
                        if (error != null) return error;
                        info.param_list.Add(class_name_dec_guess);
                        info.param_nullable_list.Add(false);
                        info.param_name_list.Add("sender");
                        info.param_list.Add(guess);
                        info.param_nullable_list.Add(false);
                        info.param_name_list.Add("param");

                        info.return_list.Add(ALittleScriptIndex.inst.sStringGuess);
                    }
                    else if (info.proto == "Msg")
                    {
                        ABnfElement element = ALittleScriptIndex.inst.FindALittleNameDec(ALittleScriptUtility.ABnfElementType.CLASS_NAME, m_element.GetFile(), "ALittle", "IMsgCommon", true);
                        if (!(element is ALittleScriptClassNameDecElement)) return new ABnfGuessError(error_element, "语言框架中找不到ALittle.IMsgCommon");
                        var class_name_dec = element as ALittleScriptClassNameDecElement;
                        error = class_name_dec.GuessType(out ABnfGuess class_name_dec_guess);
                        if (error != null) return error;
                        info.param_list.Add(class_name_dec_guess);
                        info.param_nullable_list.Add(false);
                        info.param_name_list.Add("sender");
                        info.param_list.Add(guess);
                        info.param_nullable_list.Add(false);
                        info.param_name_list.Add("param");

                        var return_dec = global_method_dec.GetMethodReturnDec();
                        if (return_dec != null)
                        {
                            var return_one_list = return_dec.GetMethodReturnOneDecList();
                            if (return_one_list.Count > 0)
                            {
                                var return_one_all_type = return_one_list[0].GetAllType();
                                if (return_one_all_type == null)
                                    return new ABnfGuessError(error_element, "带" + info.proto + "注解的函数返回值必须是struct");
                                error = return_one_all_type.GuessType(out ABnfGuess return_guess);
                                if (error != null) return error;
                                if (!(return_guess is ALittleScriptGuessStruct))
                                    return new ABnfGuessError(error_element, "带" + info.proto + "注解的函数返回值必须是struct");
                                info.return_list.Add(ALittleScriptIndex.inst.sStringGuess);
                                info.return_list.Add(return_guess);
                            }
                        }
                    }
                    else
                    {
                        return new ABnfGuessError(error_element, "未知的注解类型:" + info.proto);
                    }
                }
                else
                {
                    // 添加模板参数列表
                    var template_dec = global_method_dec.GetTemplateDec();
                    if (template_dec != null)
                    {
                        var error = template_dec.GuessTypes(out List<ABnfGuess> template_guess_list);
                        if (error != null) return error;
                        foreach (var guess in template_guess_list)
                        {
                            if (!(guess is ALittleScriptGuessTemplate))
                                return new ABnfGuessError(m_element, "template_dec.GuessTypes()取到的不是ALittleScriptGuessTemplate");
                            info.template_param_list.Add(guess as ALittleScriptGuessTemplate);
                        }
                    }

                    // 添加参数列表
                    var param_dec = global_method_dec.GetMethodParamDec();
                    if (param_dec != null)
                    {
                        var one_dec_list = param_dec.GetMethodParamOneDecList();
                        for (int i = 0; i < one_dec_list.Count; ++i)
                        {
                            var one_dec = one_dec_list[i];
                            var all_type = one_dec.GetAllType();
                            var param_tail = one_dec.GetMethodParamTailDec();
                            if (all_type != null)
                            {
                                var error = all_type.GuessType(out ABnfGuess all_type_guess);
                                if (error != null) return error;
                                info.param_list.Add(all_type_guess);
                                info.param_nullable_list.Add(ALittleScriptUtility.IsNullable(one_dec.GetModifierList()));
                                if (one_dec.GetMethodParamNameDec() != null)
                                    info.param_name_list.Add(one_dec.GetMethodParamNameDec().GetElementText());
                                else
                                    info.param_name_list.Add("");
                            }
                            else if (param_tail != null)
                            {
                                if (i + 1 != one_dec_list.Count)
                                    return new ABnfGuessError(one_dec, "参数占位符必须定义在最后");
                                var error = param_tail.GuessType(out info.param_tail);
                                if (error != null) return error;
                            }
                        }
                    }

                    // 添加返回值列表
                    var return_dec = global_method_dec.GetMethodReturnDec();
                    if (return_dec != null)
                    {
                        var one_dec_list = return_dec.GetMethodReturnOneDecList();
                        for (int i = 0; i < one_dec_list.Count; ++i)
                        {
                            var one_dec = one_dec_list[i];
                            var all_type = one_dec.GetAllType();
                            var return_tail = one_dec.GetMethodReturnTailDec();
                            if (all_type != null)
                            {
                                var error = all_type.GuessType(out ABnfGuess all_type_guess);
                                if (error != null) return error;
                                info.return_list.Add(all_type_guess);
                            }
                            else if (return_tail != null)
                            {
                                if (i + 1 != one_dec_list.Count)
                                    return new ABnfGuessError(one_dec, "返回值占位符必须定义在最后");
                                var error = return_tail.GuessType(out info.return_tail);
                                if (error != null) return error;
                            }
                        }
                    }
                }
                info.UpdateValue();
                guess_list.Add(info);
            }

            return null;
        }

        public override ABnfGuessError CheckError()
        {
            var method_dec = m_element.GetParent();
            if (method_dec == null) return null;
            var class_element_dec = method_dec.GetParent();
            if (class_element_dec == null) return null;
            var class_body = class_element_dec.GetParent();
            if (class_body == null) return null;
            var class_dec = class_body.GetParent() as ALittleScriptClassDecElement;
            if (class_dec == null) return null;

            // 计算父类
            var class_extends_dec = ALittleScriptUtility.FindClassExtends(class_dec);
            if (class_extends_dec == null) return null;

            ALittleScriptUtility.ClassAttrType attrType;
            if (method_dec is ALittleScriptClassMethodDecElement)
                attrType = ALittleScriptUtility.ClassAttrType.FUN;
            else if (method_dec is ALittleScriptClassStaticDecElement)
                attrType = ALittleScriptUtility.ClassAttrType.STATIC;
            else if (method_dec is ALittleScriptClassGetterDecElement)
                attrType = ALittleScriptUtility.ClassAttrType.GETTER;
            else if (method_dec is ALittleScriptClassSetterDecElement)
                attrType = ALittleScriptUtility.ClassAttrType.SETTER;
            else
                return null;

            var result = ALittleScriptUtility.FindFirstClassAttrFromExtends(class_extends_dec, attrType, m_key, 100);
            if (!(result is ALittleScriptMethodNameDecElement)) return null;
            var method_name_dec = result as ALittleScriptMethodNameDecElement;

            var error = m_element.GuessType(out ABnfGuess guess);
            if (error != null) return error;
            error = method_name_dec.GuessType(out ABnfGuess extends_guess);
            if (error != null) return error;
            error = ALittleScriptOp.GuessTypeEqual(extends_guess, m_element, guess, false, false);
            if (error != null)
                return new ABnfGuessError(m_element, "该函数是从父类继承下来，但是定义不一致:" + extends_guess.GetValue());
            return null;
        }

        public override bool QueryCompletion(int offset, List<ALanguageCompletionInfo> list)
        {
            var method_dec = m_element.GetParent();
            // 类内部的函数
            if (method_dec.GetParent() is ALittleScriptClassElementDecElement)
            {
                var class_dec = ALittleScriptUtility.FindClassDecFromParent(method_dec.GetParent());

                var dec_list = new List<ABnfElement>();
                ALittleScriptUtility.FindClassMethodNameDecList(class_dec, ALittleScriptUtility.sAccessPrivateAndProtectedAndPublic, "", dec_list, 100);
                for (int i = 0; i < dec_list.Count; ++i)
                    list.Add(new ALanguageCompletionInfo(dec_list[i].GetElementText(), ALittleScriptIndex.inst.sMemberMethodIcon));
            }
            // 全局函数
            else if (method_dec.GetParent() is ALittleScriptNamespaceElementDecElement)
            {
                var dec_list = ALittleScriptIndex.inst.FindALittleNameDecList(
                        ALittleScriptUtility.ABnfElementType.GLOBAL_METHOD, m_element.GetFile(), m_namespace_name, "", true);
                foreach (var dec in dec_list)
                    list.Add(new ALanguageCompletionInfo(dec.GetElementText(), ALittleScriptIndex.inst.sGlobalMethodIcon));
            }

            return true;
        }

        public override ABnfElement GotoDefinition()
        {
            var method_dec = m_element.GetParent();
            if (method_dec == null) return null;
            var class_element_dec = method_dec.GetParent();
            if (class_element_dec == null) return null;
            var class_body = method_dec.GetParent();
            if (class_body == null) return null;
            var class_dec = class_body.GetParent() as ALittleScriptClassDecElement;
            if (class_dec == null) return null;

            // 计算父类
            var class_extends_dec = ALittleScriptUtility.FindClassExtends(class_dec);
            if (class_extends_dec == null) return null;

            ALittleScriptUtility.ClassAttrType attrType;
            if (method_dec is ALittleScriptClassMethodDecElement)
                attrType = ALittleScriptUtility.ClassAttrType.FUN;
            else if (method_dec is ALittleScriptClassStaticDecElement)
                attrType = ALittleScriptUtility.ClassAttrType.STATIC;
            else if (method_dec is ALittleScriptClassGetterDecElement)
                attrType = ALittleScriptUtility.ClassAttrType.GETTER;
            else if (method_dec is ALittleScriptClassSetterDecElement)
                attrType = ALittleScriptUtility.ClassAttrType.SETTER;
            else
                return null;

            return ALittleScriptUtility.FindFirstClassAttrFromExtends(class_extends_dec, attrType, m_key, 100);
        }
    }
}

