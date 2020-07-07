
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptPropertyValueDotIdNameReference : ALittleScriptReferenceTemplate<ALittleScriptPropertyValueDotIdNameElement>
    {
        private string m_namespace_name;
        private string m_key;

        private List<ABnfElement> m_getter_list;
        private List<ABnfElement> m_setter_list;
        private ALittleScriptGuessClass m_class_guess;

        private ABnfElement m_method_dec;
        private ALittleScriptMethodBodyDecElement m_method_body_dec;

        public ALittleScriptPropertyValueDotIdNameReference(ABnfElement element) : base(element)
        {
            m_namespace_name = ALittleScriptUtility.GetNamespaceName(m_element);
            m_key = m_element.GetElementText();
        }

        private void ReloadInfo()
        {
            m_method_dec = null;
            ABnfElement parent = m_element;
            while (parent != null)
            {
                if (parent is ALittleScriptNamespaceDecElement)
                {
                    break;
                }
                else if (parent is ALittleScriptClassDecElement)
                {
                    break;
                }
                else if (parent is ALittleScriptClassCtorDecElement)
                {
                    m_method_dec = parent;
                    m_method_body_dec = (parent as ALittleScriptClassCtorDecElement).GetMethodBodyDec();
                    break;
                }
                else if (parent is ALittleScriptClassSetterDecElement)
                {
                    m_method_dec = parent;
                    m_method_body_dec = (parent as ALittleScriptClassSetterDecElement).GetMethodBodyDec();
                    break;
                }
                else if (parent is ALittleScriptClassGetterDecElement)
                {
                    m_method_dec = parent;
                    m_method_body_dec = (parent as ALittleScriptClassGetterDecElement).GetMethodBodyDec();
                    break;
                }
                else if (parent is ALittleScriptClassMethodDecElement)
                {
                    m_method_dec = parent;
                    m_method_body_dec = (parent as ALittleScriptClassMethodDecElement).GetMethodBodyDec();
                    break;

                }
                else if (parent is ALittleScriptClassStaticDecElement)
                {
                    m_method_dec = parent;
                    m_method_body_dec = (parent as ALittleScriptClassStaticDecElement).GetMethodBodyDec();
                    break;
                }
                else if (parent is ALittleScriptGlobalMethodDecElement)
                {
                    m_method_dec = parent;
                    m_method_body_dec = (parent as ALittleScriptGlobalMethodDecElement).GetMethodBodyDec();
                    break;
                }

                parent = parent.GetParent();
            }
        }


        private ABnfGuessError ReplaceTemplate(ABnfGuess guess, out ABnfGuess result)
        {
            result = null;
            if (m_class_guess == null)
            {
                result = guess;
                return null;
            }

            if (guess is ALittleScriptGuessTemplate && m_class_guess.template_map.Count > 0)
            {
                if (m_class_guess.template_map.TryGetValue(guess.GetValueWithoutConst(), out ABnfGuess guess_template))
                {
                    if (guess.is_const && !guess_template.is_const)
                    {
                        guess_template = guess_template.Clone();
                        guess_template.is_const = true;
                        guess_template.UpdateValue();
                    }
                    result = guess_template;
                }
                else
                    result = guess;
                return null;
            }

            if (guess is ALittleScriptGuessFunctor)
            {
                var guess_functor = guess as ALittleScriptGuessFunctor;
                var info = new ALittleScriptGuessFunctor(guess_functor.element);
                info.await_modifier = guess_functor.await_modifier;
                info.const_modifier = guess_functor.const_modifier;
                info.proto = guess_functor.proto;
                info.template_param_list.AddRange(guess_functor.template_param_list);
                info.param_tail = guess_functor.param_tail;
                info.param_name_list.AddRange(guess_functor.param_name_list);
                info.return_tail = guess_functor.return_tail;

                int start_index = 0;
                if (guess_functor.element is ALittleScriptClassMethodDecElement
                    || guess_functor.element is ALittleScriptClassSetterDecElement
                    || guess_functor.element is ALittleScriptClassGetterDecElement)
                {
                    info.param_list.Add(m_class_guess);
                    info.param_nullable_list.Add(false);
                    if (info.param_name_list.Count > 0)
                        info.param_name_list[0] = m_class_guess.GetValue();
                    start_index = 1;
                }
                for (int i = start_index; i < guess_functor.param_list.Count; ++i)
                {
                    var error = ReplaceTemplate(guess_functor.param_list[i], out ABnfGuess guess_info);
                    if (error != null) return error;
                    info.param_list.Add(guess_info);
                }
                for (int i = start_index; i < guess_functor.param_nullable_list.Count; ++i)
                {
                    info.param_nullable_list.Add(guess_functor.param_nullable_list[i]);
                }
                for (int i = 0; i < guess_functor.return_list.Count; ++i)
                {
                    var error = ReplaceTemplate(guess_functor.return_list[i], out ABnfGuess guess_info);
                    if (error != null) return error;
                    info.return_list.Add(guess_info);
                }
                info.UpdateValue();
                result = info;
                return null;
            }

            if (guess is ALittleScriptGuessList)
            {
                var guess_list = guess as ALittleScriptGuessList;
                var error = ReplaceTemplate(guess_list.sub_type, out ABnfGuess sub_info);
                if (error != null) return error;
                var info = new ALittleScriptGuessList(sub_info, guess_list.is_const, guess_list.is_native);
                info.UpdateValue();
                result = info;
                return null;
            }

            if (guess is ALittleScriptGuessMap)
            {
                var guess_map = guess as ALittleScriptGuessMap;
                var error = ReplaceTemplate(guess_map.key_type, out ABnfGuess key_info);
                if (error != null) return error;
                error = ReplaceTemplate(guess_map.value_type, out ABnfGuess value_info);
                if (error != null) return error;

                var info = new ALittleScriptGuessMap(key_info, value_info, guess.is_const);
                info.UpdateValue();
                result = info;
                return null;
            }

            if (guess is ALittleScriptGuessClass)
            {
                var guess_class = guess as ALittleScriptGuessClass;
                var info = new ALittleScriptGuessClass(guess_class.namespace_name,
                        guess_class.class_name, guess_class.class_dec, guess_class.using_name, guess_class.is_const, guess_class.is_native);
                info.template_list.AddRange(guess_class.template_list);
                foreach (var pair in guess_class.template_map)
                {
                    if (info.template_map.ContainsKey(pair.Key)) info.template_map.Remove(pair.Key);
                    var error = ReplaceTemplate(pair.Value, out ABnfGuess replace_guess);
                    if (error != null) return error;
                    info.template_map.Add(pair.Key, replace_guess);
                }

                var src_class_dec = guess_class.class_dec;
                var src_class_name_dec = src_class_dec.GetClassNameDec();
                if (src_class_name_dec == null)
                    return new ABnfGuessError(m_element, "类模板没有定义类名");
                info.UpdateValue();
                result = info;
                return null;
            }

            result = guess;
            return null;
        }

        private ABnfGuessError CalcResolve(out List<ABnfElement> result_list, out ABnfGuess pre_type)
        {
            pre_type = null;
            result_list = new List<ABnfElement>();
            if (m_key.Length == 0) return null;

            // 获取父节点
            var property_value_dot_id = m_element.GetParent() as ALittleScriptPropertyValueDotIdElement;
            var property_value_suffix = property_value_dot_id.GetParent() as ALittleScriptPropertyValueSuffixElement;
            var property_value = property_value_suffix.GetParent() as ALittleScriptPropertyValueElement;
            var property_value_first_type = property_value.GetPropertyValueFirstType();
            var suffix_list = property_value.GetPropertyValueSuffixList();

            // 获取所在位置
            int index = suffix_list.IndexOf(property_value_suffix);
            if (index == -1) return null;

            ABnfGuessError error = null;
            if (index == 0)
                error = property_value_first_type.GuessType(out pre_type);
            else
                error = suffix_list[index - 1].GuessType(out pre_type);
            if (error != null) return error;

            // 判断当前后缀是否是最后一个后缀
            ALittleScriptPropertyValueSuffixElement next_suffix = null;
            if (index + 1 < suffix_list.Count)
                next_suffix = suffix_list[index + 1];

            bool is_const = false;
            if (pre_type != null) is_const = pre_type.is_const;

            if (pre_type is ALittleScriptGuessTemplate)
            {
                pre_type = (pre_type as ALittleScriptGuessTemplate).template_extends;
                if (pre_type != null && is_const && !pre_type.is_const)
                {
                    pre_type = pre_type.Clone();
                    pre_type.is_const = true;
                    pre_type.UpdateValue();
                }
            }

            if (pre_type == null) return null;

            // 处理类的实例对象
            if (pre_type is ALittleScriptGuessClass)
            {
                m_class_guess = pre_type as ALittleScriptGuessClass;
                var class_dec = m_class_guess.class_dec;

                // 计算当前元素对这个类的访问权限
                int access_level = ALittleScriptUtility.CalcAccessLevelByTargetClassDecForElement(m_element, class_dec);

                // 所有成员变量
                var class_var_dec_list = new List<ABnfElement>();
                ALittleScriptUtility.FindClassAttrList(class_dec,
                        access_level, ALittleScriptUtility.ClassAttrType.VAR, m_key, class_var_dec_list, 100);
                foreach (var class_var_dec in class_var_dec_list)
                    result_list.Add(class_var_dec);

                var class_method_name_dec_list = new List<ABnfElement>();
                // 在当前情况下，只有当前property_value在等号的左边，并且是最后一个属性才是setter，否则都是getter
                if (next_suffix == null && property_value.GetParent() is ALittleScriptOpAssignExprElement) {
                    m_setter_list = new List<ABnfElement>();
                    ALittleScriptUtility.FindClassAttrList(class_dec, access_level, ALittleScriptUtility.ClassAttrType.SETTER, m_key, m_setter_list, 100);
                    class_method_name_dec_list.AddRange(m_setter_list);
                }
                else
                {
                    m_getter_list = new List<ABnfElement>();
                    ALittleScriptUtility.FindClassAttrList(class_dec, access_level, ALittleScriptUtility.ClassAttrType.GETTER, m_key, m_getter_list, 100);
                    class_method_name_dec_list.AddRange(m_getter_list);
                }
                // 所有成员函数
                ALittleScriptUtility.FindClassAttrList(class_dec, access_level, ALittleScriptUtility.ClassAttrType.FUN, m_key, class_method_name_dec_list, 100);
                // 添加函数名元素
                class_method_name_dec_list = ALittleScriptUtility.FilterSameName(class_method_name_dec_list);
                foreach (var class_method_name_dec in class_method_name_dec_list)
                    result_list.Add(class_method_name_dec);
                // 处理结构体的实例对象
            }
            else if (pre_type is ALittleScriptGuessStruct)
            {
                var struct_dec = ((ALittleScriptGuessStruct)pre_type).struct_dec;
                var struct_var_dec_list = new List<ALittleScriptStructVarDecElement>();
                // 所有成员变量
                ALittleScriptUtility.FindStructVarDecList(struct_dec, m_key, struct_var_dec_list, 100);
                foreach (var struct_var_dec in struct_var_dec_list)
                    result_list.Add(struct_var_dec);
                // 比如 ALittleName.XXX
            }
            else if (pre_type is ALittleScriptGuessNamespaceName)
            {
                var namespace_name_dec = ((ALittleScriptGuessNamespaceName)pre_type).namespace_name_dec;
                string namespace_name = namespace_name_dec.GetElementText();
                // 所有枚举名
                var enum_name_dec_list = ALittleScriptIndex.inst.FindALittleNameDecList(
                        ALittleScriptUtility.ABnfElementType.ENUM_NAME, m_element.GetFile(), namespace_name, m_key, true);
                foreach (var enum_name_dec in enum_name_dec_list)
                    result_list.Add(enum_name_dec);
                // 所有全局函数
                var method_name_dec_list = ALittleScriptIndex.inst.FindALittleNameDecList(
                        ALittleScriptUtility.ABnfElementType.GLOBAL_METHOD, m_element.GetFile(), namespace_name, m_key, true);
                foreach (var method_name_dec in method_name_dec_list)
                    result_list.Add(method_name_dec);
                // 所有类名
                var class_name_dec_list = ALittleScriptIndex.inst.FindALittleNameDecList(
                        ALittleScriptUtility.ABnfElementType.CLASS_NAME, m_element.GetFile(), namespace_name, m_key, true);
                foreach (var class_name_dec in class_name_dec_list)
                    result_list.Add(class_name_dec);
                // 所有结构体名
                var struct_name_dec_list = ALittleScriptIndex.inst.FindALittleNameDecList(
                        ALittleScriptUtility.ABnfElementType.STRUCT_NAME, m_element.GetFile(), namespace_name, m_key, true);
                foreach (var struct_name_dec in struct_name_dec_list)
                    result_list.Add(struct_name_dec);
                // 所有单例
                var instance_name_dec_list = ALittleScriptIndex.inst.FindALittleNameDecList(
                        ALittleScriptUtility.ABnfElementType.INSTANCE_NAME, m_element.GetFile(), namespace_name, m_key, false);
                foreach (var instance_name_dec in instance_name_dec_list)
                    result_list.Add(instance_name_dec);
                // 比如 AClassName.XXX
            }
            else if (pre_type is ALittleScriptGuessClassName)
            {
                var class_name_dec = ((ALittleScriptGuessClassName)pre_type).class_name_dec;
                var class_dec = class_name_dec.GetParent() as ALittleScriptClassDecElement;

                // 计算当前元素对这个类的访问权限
                int access_level = ALittleScriptUtility.CalcAccessLevelByTargetClassDecForElement(m_element, class_dec);

                // 所有静态函数
                var class_method_name_dec_list = new List<ABnfElement>();
                ALittleScriptUtility.FindClassAttrList(class_dec, access_level, ALittleScriptUtility.ClassAttrType.STATIC, m_key, class_method_name_dec_list, 100);

                // 如果后面那个是MethodCall，并且有两个参数的是setter，是一个参数的是getter，否则两个都不是
                if (next_suffix != null)
                {
                    var method_call_stat = next_suffix.GetPropertyValueMethodCall();
                    if (method_call_stat != null)
                    {
                        int paramCount = method_call_stat.GetValueStatList().Count;
                        if (paramCount == 1)
                        {
                            // 所有getter
                            ALittleScriptUtility.FindClassAttrList(class_dec, access_level, ALittleScriptUtility.ClassAttrType.GETTER, m_key, class_method_name_dec_list, 100);
                        }
                        else if (paramCount == 2)
                        {
                            // 所有setter
                            ALittleScriptUtility.FindClassAttrList(class_dec, access_level, ALittleScriptUtility.ClassAttrType.SETTER, m_key, class_method_name_dec_list, 100);
                        }
                    }
                }

                // 所有成员函数
                ALittleScriptUtility.FindClassAttrList(class_dec, access_level, ALittleScriptUtility.ClassAttrType.FUN, m_key, class_method_name_dec_list, 100);
                class_method_name_dec_list = ALittleScriptUtility.FilterSameName(class_method_name_dec_list);
                foreach (var class_method_name_dec in class_method_name_dec_list)
                    result_list.Add(class_method_name_dec);
                // 比如 AEnumName.XXX
            } else if (pre_type is ALittleScriptGuessEnumName) {
                // 所有枚举字段
                var enum_name_dec = ((ALittleScriptGuessEnumName)pre_type).enum_name_dec;
                var enum_dec = enum_name_dec.GetParent() as ALittleScriptEnumDecElement;
                var var_dec_list = new List<ALittleScriptEnumVarDecElement>();
                ALittleScriptUtility.FindEnumVarDecList(enum_dec, m_key, var_dec_list);
                foreach (var var_name_dec in var_dec_list)
                    result_list.Add(var_name_dec);
            }

            return null;
        }

        public override ABnfGuessError GuessTypes(out List<ABnfGuess> guess_list)
        {
            guess_list = new List<ABnfGuess>();

            m_getter_list = null;
            m_setter_list = null;
            m_class_guess = null;

            var error = CalcResolve(out List<ABnfElement> result_list, out ABnfGuess pre_type);
            if (error != null) return error;
            foreach (var result in result_list)
            {
                ABnfGuess guess = null;
                if (result is ALittleScriptClassVarDecElement)
                {
                    error = result.GuessType(out guess);
                    if (error != null) return error;

                    if (m_class_guess != null && guess is ALittleScriptGuessTemplate)
                    {
                        if (!m_class_guess.template_map.TryGetValue(guess.GetValueWithoutConst(), out ABnfGuess guess_template))
                        {
                            for (int i = 0; i < m_class_guess.template_list.Count; ++i)
                            {
                                guess_template = m_class_guess.template_list[i];

                                if (guess_template.GetValueWithoutConst() == guess.GetValueWithoutConst())
                                    break;
                            }
                        }

                        if (guess_template != null)
                        {
                            if (guess.is_const && !guess_template.is_const)
                            {
                                guess_template = guess_template.Clone();
                                guess_template.is_const = true;
                                guess_template.UpdateValue();
                            }

                            guess = guess_template;
                        }
                    }
                }
                else if (result is ALittleScriptStructVarDecElement)
                {
                    error = result.GuessType(out guess);
                    if (error != null) return error;
                }
                else if (result is ALittleScriptEnumVarDecElement)
                {
                    error = result.GuessType(out guess);
                    if (error != null) return error;
                }
                else if (result is ALittleScriptMethodNameDecElement)
                {
                    error = result.GuessType(out guess);
                    if (error != null) return error;

                    // 如果前一个数据是const，那么调用的函数也必须是const
                    if (pre_type != null && pre_type.is_const)
                    {
                        var guess_functor = guess as ALittleScriptGuessFunctor;
                        if (guess_functor != null && !guess_functor.const_modifier)
                            return new ABnfGuessError(m_element, "请使用带Const修饰的函数");
                    }

                    if (result.GetParent() is ALittleScriptClassGetterDecElement)
                    {
                        if (m_getter_list != null && m_getter_list.IndexOf(result) >= 0 && guess is ALittleScriptGuessFunctor)
                        {
                            guess = ((ALittleScriptGuessFunctor)guess).return_list[0];
                        }
                    }
                    else if (result.GetParent() is ALittleScriptClassSetterDecElement)
                    {
                        if (m_setter_list != null && m_setter_list.IndexOf(result) >= 0 && guess is ALittleScriptGuessFunctor)
                        {
                            guess = ((ALittleScriptGuessFunctor)guess).param_list[1];
                        }
                    }
                    error = ReplaceTemplate(guess, out guess);
                    if (error != null) return error;

                } else if (result is ALittleScriptVarAssignNameDecElement) {
                    error = result.GuessType(out guess);
                    if (error != null) return error;
                } else if (result is ALittleScriptEnumNameDecElement) {
                    error = result.GuessType(out ABnfGuess enum_guess);
                    if (error != null) return error;
                    if (!(enum_guess is ALittleScriptGuessEnum))
                        return new ABnfGuessError(m_element, "ALittleEnumNameDec.guessType的结果不是ALittleGuessEnum");
                    var enum_guess_enum = enum_guess as ALittleScriptGuessEnum;
                    var info = new ALittleScriptGuessEnumName(enum_guess_enum.namespace_name, enum_guess_enum.enum_name, result as ALittleScriptEnumNameDecElement);
                    info.UpdateValue();
                    guess = info;
                } else if (result is ALittleScriptStructNameDecElement) {
                    error = result.GuessType(out ABnfGuess struct_guess);
                    if (error != null) return error;
                    if (!(struct_guess is ALittleScriptGuessStruct))
                        return new ABnfGuessError(m_element, "ALittleStructNameDec.guessType的结果不是ALittleGuessStruct");
                    var struct_guess_struct = struct_guess as ALittleScriptGuessStruct;
                    var info = new ALittleScriptGuessStructName(struct_guess_struct.namespace_name, struct_guess_struct.struct_name, result as ALittleScriptStructNameDecElement);
                    info.UpdateValue();
                    guess = info;
                } else if (result is ALittleScriptClassNameDecElement) {
                    error = result.GuessType(out ABnfGuess class_guess);
                    if (error != null) return error;
                    if (!(class_guess is ALittleScriptGuessClass))
                        return new ABnfGuessError(m_element, "ALittleClassNameDec.guessType的结果不是ALittleGuessClass");
                    var class_guess_class = class_guess as ALittleScriptGuessClass;
                    if (class_guess_class.template_list.Count > 0)
                        return new ABnfGuessError(m_element, "模板类" + class_guess_class.GetValue() + "不能直接使用");
                    var info = new ALittleScriptGuessClassName(class_guess_class.namespace_name, class_guess_class.class_name, result as ALittleScriptClassNameDecElement);
                    info.UpdateValue();
                    guess = info;
                }

                if (guess != null)
                {
                    if (pre_type != null && pre_type.is_const && !guess.is_const)
                    {
                        if (guess is ALittleScriptGuessPrimitive)
                        {
                            var guess_value = guess.GetValue();
                            ALittleScriptIndex.inst.sPrimitiveGuessMap.TryGetValue("const " + guess.GetValue(), out guess);
                            if (guess == null) return new ABnfGuessError(m_element, "找不到const " + guess_value);
                        }
                        else
                        {
                            guess = guess.Clone();
                            guess.is_const = true;
                            guess.UpdateValue();
                        }
                    }
                    guess_list.Add(guess);
                }
            }

            m_getter_list = null;
            m_setter_list = null;
            m_class_guess = null;

            return null;
        }

        public override string QueryClassificationTag(out bool blur)
        {
            blur = false;
            var error = m_element.GuessType(out ABnfGuess guess);
            if (error != null) return null;
            if (guess is ALittleScriptGuessFunctor)
            {
                var guess_functor = guess as ALittleScriptGuessFunctor;
                if (guess_functor.element is ALittleScriptClassStaticDecElement
                    || guess_functor.element is ALittleScriptClassMethodDecElement
                    || guess_functor.element is ALittleScriptClassGetterDecElement
                    || guess_functor.element is ALittleScriptClassSetterDecElement
                    || guess_functor.element is ALittleScriptGlobalMethodDecElement)
                    return "ALittleScriptMethodName";
            }
            else if (guess is ALittleScriptGuessNamespaceName
                || guess is ALittleScriptGuessClassName
                || guess is ALittleScriptGuessStructName
                || guess is ALittleScriptGuessEnumName)
            {
                return "ALittleScriptCustomName";
            }

            return "ALittleScriptVarName";
        }

        public override ABnfGuessError CheckError()
        {
            var error = m_element.GuessTypes(out List<ABnfGuess> guess_list);
            if (error != null) return error;
            if (guess_list.Count == 0)
                return new ABnfGuessError(m_element, "未知类型");
            else if (guess_list.Count != 1)
                return new ABnfGuessError(m_element, "重复定义");
            return null;
        }

        public override bool QueryCompletion(int offset, List<ALanguageCompletionInfo> list)
        {
            var parent = m_element.GetParent();
            if (parent == null) return false;

            return parent.GetReference().QueryCompletion(offset, list);
        }

        public override ABnfElement GotoDefinition()
        {
            var error = CalcResolve(out List<ABnfElement> result_list, out ABnfGuess pre_type);
            if (error != null) return null;
            foreach (var result in result_list)
                return result;
            return null;
        }


        public override bool PeekHighlightWord()
        {
            return true;
        }

        public override void QueryHighlightWordTag(List<ALanguageHighlightWordInfo> list)
        {
            if (m_method_dec == null)
                ReloadInfo();

            var error = m_element.GuessType(out ABnfGuess guess);
            if (error != null) return;
            CollectHighlight(guess, m_method_body_dec, list);
        }

        private void CollectHighlight(ABnfGuess target_guess, ABnfElement element, List<ALanguageHighlightWordInfo> list)
        {
            if (element is ALittleScriptPropertyValueDotIdNameElement)
            {
                if (element.GetElementText() != m_key) return;

                var error = element.GuessType(out ABnfGuess guess);
                if (error != null) return;
                if (guess.GetValue() == target_guess.GetValue())
                {
                    var info = new ALanguageHighlightWordInfo();
                    info.start = element.GetStart();
                    info.end = element.GetEnd();
                    list.Add(info);
                }
                return;
            }

            if (element is ABnfNodeElement)
            {
                var childs = (element as ABnfNodeElement).GetChilds();
                foreach (var child in childs)
                    CollectHighlight(target_guess, child, list);
            }
        }
    }
}

