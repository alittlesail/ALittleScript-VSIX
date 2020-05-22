
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptPropertyValueCustomTypeReference : ALittleScriptReferenceTemplate<ALittleScriptPropertyValueCustomTypeElement>
    {
        private string m_namespace_name;
        private string m_key;
        private ABnfElement m_method_dec;
        private ALittleScriptMethodBodyDecElement m_method_body_dec;

        public ALittleScriptPropertyValueCustomTypeReference(ABnfElement element) : base(element)
        {
            m_namespace_name = ALittleScriptUtility.GetNamespaceName(m_element);
            m_key = m_element.GetElementText();
            ReloadInfo();
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
                else if (parent is ALittleScriptClassDecElement) {
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
                else if (parent is ALittleScriptGlobalMethodDecElement) {
                    m_method_dec = parent;
                    m_method_body_dec = (parent as ALittleScriptGlobalMethodDecElement).GetMethodBodyDec();
                    break;
                }

                parent = parent.GetParent();
            }
        }

        // 计算命名域前缀
        public ABnfGuessError CalcNamespaceName(out string namespace_name)
        {
            namespace_name = "";
            var result_list = CalcResolve();
            foreach (var result in result_list)
            {
                if (result is ALittleScriptNamespaceNameDecElement)
                {
                    return null;
                }
                else if (result is ALittleScriptClassNameDecElement)
                {
                    var error = result.GuessType(out ABnfGuess class_guess);
                    if (error != null) return error;
                    if (!(class_guess is ALittleScriptGuessClass))
                        return new ABnfGuessError(m_element, "ALittleClassNameDec.guessType()的结果不是ALittleScriptGuessClass");

                    var class_guess_class = class_guess as ALittleScriptGuessClass;
                    namespace_name = class_guess_class.namespace_name;
                    return null;
                }
                else if (result is ALittleScriptStructNameDecElement)
                {
                    var error = result.GuessType(out ABnfGuess struct_guess);
                    if (error != null) return error;
                    if (!(struct_guess is ALittleScriptGuessStruct))
                        return new ABnfGuessError(m_element, "ALittleStructNameDec.guessType()的结果不是ALittleScriptGuessStruct");
                    var struct_guess_struct = struct_guess as ALittleScriptGuessStruct;
                    namespace_name = struct_guess_struct.namespace_name;
                    return null;
                }
                else if (result is ALittleScriptEnumNameDecElement)
                {
                    var error = result.GuessType(out ABnfGuess enum_guess);
                    if (error != null) return error;
                    if (!(enum_guess is ALittleScriptGuessEnum))
                        return new ABnfGuessError(m_element, "ALittleEnumNameDec.guessType()的结果不是ALittleScriptGuessEnum");
                    var enum_guess_enum = enum_guess as ALittleScriptGuessEnum;
                    namespace_name = enum_guess_enum.namespace_name;
                    return null;
                }
                else if (result is ALittleScriptMethodParamNameDecElement)
                {
                    return null;
                }
                else if (result is ALittleScriptVarAssignNameDecElement)
                {
                    var assign_dec = result.GetParent() as ALittleScriptVarAssignDecElement;
                    if (assign_dec == null) return null;
                    var expr_dec = assign_dec.GetParent() as ALittleScriptVarAssignExprElement;
                    if (expr_dec == null)
                        return new ABnfGuessError(m_element, "ALittleScriptVarAssignDecElement的父节点不是ALittleScriptVarAssignExprElement");

                    // 如果父节点是instance
                    var instance_dec = expr_dec.GetParent() as ALittleScriptInstanceDecElement;
                    if (instance_dec == null) return null;

                    var element_dec = instance_dec.GetParent() as ALittleScriptNamespaceElementDecElement;
                    if (element_dec == null)
                        return new ABnfGuessError(m_element, "ALittleScriptInstanceDecElement的父节点不是ALittleScriptNamespaceElementDecElement");
                    var access_type = ALittleScriptUtility.CalcAccessType(element_dec.GetModifierList());
                    if (access_type == ALittleScriptUtility.ClassAccessType.PROTECTED)
                        namespace_name = ALittleScriptUtility.GetNamespaceName(result);
                    return null;
                }
                else if (result is ALittleScriptMethodNameDecElement)
                {
                    var error = result.GuessType(out ABnfGuess method_guess);
                    if (error != null) return error;
                    if (!(method_guess is ALittleScriptGuessFunctor))
                        return new ABnfGuessError(m_element, "ALittleScriptMethodNameDecElement.guessType()的结果不是ALittleScriptGuessFunctor");
                    var method_guess_functor = method_guess as ALittleScriptGuessFunctor;
                    if (method_guess_functor.element is ALittleScriptGlobalMethodDecElement)
                    {
                        var element_dec = method_guess_functor.element.GetParent() as ALittleScriptNamespaceElementDecElement;
                        if (element_dec == null)
                            return new ABnfGuessError(m_element, "ALittleScriptGlobalMethodDecElement的父节点不是ALittleScriptNamespaceElementDecElement");
                        var access_type = ALittleScriptUtility.CalcAccessType(element_dec.GetModifierList());
                        if (access_type != ALittleScriptUtility.ClassAccessType.PRIVATE)
                            namespace_name = ALittleScriptUtility.GetNamespaceName(method_guess_functor.element);
                    }
                    return null;

                }
                else if (result is ALittleScriptUsingNameDecElement)
                {
                    var using_dec = result.GetParent() as ALittleScriptUsingDecElement;
                    if (using_dec == null)
                        return new ABnfGuessError(m_element, "ALittleScriptUsingNameDecElement的父节点不是ALittleScriptUsingDecElement");
                    var element_dec = using_dec.GetParent() as ALittleScriptNamespaceElementDecElement;
                    if (element_dec == null)
                        return new ABnfGuessError(m_element, "ALittleScriptUsingDecElement的父节点不是ALittleScriptNamespaceElementDecElement");
                    var access_type = ALittleScriptUtility.CalcAccessType(element_dec.GetModifierList());
                    if (access_type != ALittleScriptUtility.ClassAccessType.PRIVATE)
                        namespace_name = ALittleScriptUtility.GetNamespaceName(result);
                    return null;
                }
            }

            return null;
        }

        private List<ABnfElement> CalcResolve()
        {
            var result_list = new List<ABnfElement>();
            if (m_key.Length == 0) return result_list;

            // 处理命名域
            {
                var dec_map = ALittleScriptIndex.inst.FindNamespaceNameDecList(m_key);
                if (dec_map.Count > 0) result_list.Clear();
                foreach (var pair in dec_map)
                    result_list.Add(pair.Value);
            }
            // 处理全局函数
            {
                var dec_list = ALittleScriptIndex.inst.FindALittleNameDecList(
                        ALittleScriptUtility.ABnfElementType.GLOBAL_METHOD, m_element.GetFile(), m_namespace_name, m_key, true);
                if (dec_list.Count > 0) result_list.Clear();
                foreach (var dec in dec_list)
                    result_list.Add(dec);
            }
            // 处理类名
            {
                var dec_list = ALittleScriptIndex.inst.FindALittleNameDecList(
                        ALittleScriptUtility.ABnfElementType.CLASS_NAME, m_element.GetFile(), m_namespace_name, m_key, true);
                if (dec_list.Count > 0) result_list.Clear();
                foreach (var dec in dec_list)
                    result_list.Add(dec);
            }
            // 处理结构体名
            {
                var dec_list = ALittleScriptIndex.inst.FindALittleNameDecList(
                        ALittleScriptUtility.ABnfElementType.STRUCT_NAME, m_element.GetFile(), m_namespace_name, m_key, true);
                if (dec_list.Count > 0) result_list.Clear();
                foreach (var dec in dec_list)
                    result_list.Add(dec);
            }
            // 处理using
            {
                var dec_list = ALittleScriptIndex.inst.FindALittleNameDecList(
                        ALittleScriptUtility.ABnfElementType.USING_NAME, m_element.GetFile(), m_namespace_name, m_key, true);
                foreach (var dec in dec_list)
                    result_list.Add(dec);
            }
            // 处理枚举名
            {
                var dec_list = ALittleScriptIndex.inst.FindALittleNameDecList(
                        ALittleScriptUtility.ABnfElementType.ENUM_NAME, m_element.GetFile(), m_namespace_name, m_key, true);
                if (dec_list.Count > 0) result_list.Clear();
                foreach (var dec in dec_list)
                    result_list.Add(dec);
            }
            // 处理单例
            {
                var dec_list = ALittleScriptIndex.inst.FindALittleNameDecList(
                        ALittleScriptUtility.ABnfElementType.INSTANCE_NAME, m_element.GetFile(), m_namespace_name, m_key, true);
                if (dec_list.Count > 0) result_list.Clear();
                foreach (var dec in dec_list)
                    result_list.Add(dec);
            }
            // 处理参数
            if (m_method_dec != null)
            {
                var dec_list = ALittleScriptUtility.FindMethodParamNameDecList(m_method_dec, m_key);
                if (dec_list.Count > 0) result_list.Clear();
                foreach (var dec in dec_list)
                    result_list.Add(dec);
            }
            // 处理表达式定义
            {
                var dec_list = ALittleScriptUtility.FindVarAssignNameDecList(m_element, m_key);
                if (dec_list.Count > 0) result_list.Clear();
                foreach (var dec in dec_list)
                    result_list.Add(dec);
            }
            return result_list;
        }

        public override ABnfGuessError GuessTypes(out List<ABnfGuess> guess_list)
        {
            guess_list = new List<ABnfGuess>();
            // 标记是否已经包含了命名域，命名域的guess不要重复
            bool has_namespace = false;

            var result_list = CalcResolve();
            foreach (var result in result_list)
            {
                ABnfGuess guess = null;
                if (result is ALittleScriptNamespaceNameDecElement)
                {
                    if (!has_namespace)
                    {
                        var error = result.GuessType(out ABnfGuess result_guess);
                        if (error != null) return error;
                        has_namespace = true;
                        var guess_namespace_name = new ALittleScriptGuessNamespaceName(
                                result_guess.GetValue(),
                                (ALittleScriptNamespaceNameDecElement)result
                        );
                        guess_namespace_name.UpdateValue();
                        guess = guess_namespace_name;
                    }
                }
                else if (result is ALittleScriptClassNameDecElement)
                {
                    var error = result.GuessType(out ABnfGuess class_guess);
                    if (error != null) return error;
                    if (!(class_guess is ALittleScriptGuessClass))
                        return new ABnfGuessError(m_element, "ALittleClassNameDec.guessType()的结果不是ALittleScriptGuessClass");
                    
                    var class_guess_class = class_guess as ALittleScriptGuessClass;
                    if (class_guess_class.template_list.Count > 0)
                        return new ABnfGuessError(m_element, "模板类" + class_guess_class.GetValue() + "不能直接使用");

                    var guess_class_name = new ALittleScriptGuessClassName(class_guess_class.namespace_name,
                            class_guess_class.class_name, result as ALittleScriptClassNameDecElement);
                    guess_class_name.UpdateValue();
                    guess = guess_class_name;
                }
                else if (result is ALittleScriptStructNameDecElement)
                {
                    var error = result.GuessType(out ABnfGuess struct_guess);
                    if (error != null) return error;
                    if (!(struct_guess is ALittleScriptGuessStruct))
                        return new ABnfGuessError(m_element, "ALittleStructNameDec.guessType()的结果不是ALittleScriptGuessStruct");
                    var struct_guess_struct = struct_guess as ALittleScriptGuessStruct;

                    var guess_struct_name = new ALittleScriptGuessStructName(struct_guess_struct.namespace_name,
                            struct_guess_struct.struct_name, result as ALittleScriptStructNameDecElement);
                    guess_struct_name.UpdateValue();
                    guess = guess_struct_name;
                }
                else if (result is ALittleScriptEnumNameDecElement)
                {
                    var error = result.GuessType(out ABnfGuess enum_guess);
                    if (error != null) return error;
                    if (!(enum_guess is ALittleScriptGuessEnum))
                        return new ABnfGuessError(m_element, "ALittleEnumNameDec.guessType()的结果不是ALittleScriptGuessEnum");
                    var enum_guess_enum = enum_guess as ALittleScriptGuessEnum;

                    var guess_enum_name = new ALittleScriptGuessEnumName(enum_guess_enum.namespace_name,
                            enum_guess_enum.enum_name, result as ALittleScriptEnumNameDecElement);
                    guess_enum_name.UpdateValue();
                    guess = guess_enum_name;
                }
                else if (result is ALittleScriptMethodParamNameDecElement)
                {
                    var error = result.GuessType(out guess);
                    if (error != null) return error;
                }
                else if (result is ALittleScriptVarAssignNameDecElement)
                {
                    var error = result.GuessType(out guess);
                    if (error != null) return error;
                }
                else if (result is ALittleScriptMethodNameDecElement)
                {
                    var error = result.GuessType(out guess);
                    if (error != null) return error;
                }
                else if (result is ALittleScriptUsingNameDecElement)
                {
                    var error = result.GuessType(out guess);
                    if (error != null) return error;
                }

                if (guess != null) guess_list.Add(guess);
            }

            return null;
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
            // 处理命名域
            {
                var dec_map = ALittleScriptIndex.inst.FindNamespaceNameDecList("");
                foreach (var pair in dec_map)
                    list.Add(new ALanguageCompletionInfo(pair.Value.GetElementText(), ALittleScriptIndex.inst.sNamespaceIcon));
            }
            // 处理全局函数
            {
                var dec_list = ALittleScriptIndex.inst.FindALittleNameDecList(ALittleScriptUtility.ABnfElementType.GLOBAL_METHOD, m_element.GetFile(), m_namespace_name, "", true);
                foreach (var dec in dec_list)
                    list.Add(new ALanguageCompletionInfo(dec.GetElementText(), ALittleScriptIndex.inst.sGlobalMethodIcon));
            }
            // 处理类名
            {
                var dec_list = ALittleScriptIndex.inst.FindALittleNameDecList(ALittleScriptUtility.ABnfElementType.CLASS_NAME, m_element.GetFile(), m_namespace_name, "", true);
                foreach (var dec in dec_list)
                    list.Add(new ALanguageCompletionInfo(dec.GetElementText(), ALittleScriptIndex.inst.sClassIcon));
            }
            // 处理结构体名
            {
                var dec_list = ALittleScriptIndex.inst.FindALittleNameDecList(ALittleScriptUtility.ABnfElementType.STRUCT_NAME, m_element.GetFile(), m_namespace_name, "", true);
                foreach (var dec in dec_list)
                    list.Add(new ALanguageCompletionInfo(dec.GetElementText(), ALittleScriptIndex.inst.sStructIcon));
            }
            // 处理using
            {
                var dec_list = ALittleScriptIndex.inst.FindALittleNameDecList(ALittleScriptUtility.ABnfElementType.USING_NAME, m_element.GetFile(), m_namespace_name, "", true);
                foreach (var dec in dec_list)
                {
                    var error = dec.GuessType(out ABnfGuess guess);
                    if (error != null)
                        list.Add(new ALanguageCompletionInfo(dec.GetElementText(), ALittleScriptIndex.inst.sClassIcon));
                    else
                    {
                        if (guess is ALittleScriptGuessClass)
                            list.Add(new ALanguageCompletionInfo(dec.GetElementText(), ALittleScriptIndex.inst.sClassIcon));
                        else if (guess is ALittleScriptGuessStruct)
                            list.Add(new ALanguageCompletionInfo(dec.GetElementText(), ALittleScriptIndex.inst.sStructIcon));
                        else
                            list.Add(new ALanguageCompletionInfo(dec.GetElementText(), ALittleScriptIndex.inst.sPropertyIcon));
                    }
                }
            }
            // 处理枚举名
            {
                var dec_list = ALittleScriptIndex.inst.FindALittleNameDecList(ALittleScriptUtility.ABnfElementType.ENUM_NAME, m_element.GetFile(), m_namespace_name, "", true);
                foreach (var dec in dec_list)
                    list.Add(new ALanguageCompletionInfo(dec.GetElementText(), ALittleScriptIndex.inst.sEnumIcon));
            }
            // 处理单例
            {
                var dec_list = ALittleScriptIndex.inst.FindALittleNameDecList(ALittleScriptUtility.ABnfElementType.INSTANCE_NAME, m_element.GetFile(), m_namespace_name, "", true);
                foreach (var dec in dec_list)
                    list.Add(new ALanguageCompletionInfo(dec.GetElementText(), ALittleScriptIndex.inst.sInstanceIcon));
            }

            // 处理参数
            if (m_method_dec != null)
            {
                var dec_list = ALittleScriptUtility.FindMethodParamNameDecList(m_method_dec, "");
                foreach (var dec in dec_list)
                    list.Add(new ALanguageCompletionInfo(dec.GetElementText(), ALittleScriptIndex.inst.sParamIcon));
            }

            // 处理表达式
            {
                var dec_list = ALittleScriptUtility.FindVarAssignNameDecList(m_element, "");
                foreach (var dec in dec_list)
                    list.Add(new ALanguageCompletionInfo(dec.GetElementText(), ALittleScriptIndex.inst.sVariableIcon));
            }

            return true;
        }

        public override ABnfElement GotoDefinition()
        {
            var result_list = CalcResolve();
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
            var error = m_element.GuessType(out ABnfGuess guess);
            if (error != null) return;
            CollectHighlight(guess, m_method_body_dec, list);

            // 处理参数
            if (m_method_dec != null)
            {
                var dec_list = ALittleScriptUtility.FindMethodParamNameDecList(m_method_dec, m_key);
                foreach (var dec in dec_list)
                {
                    var info = new ALanguageHighlightWordInfo();
                    info.start = dec.GetStart();
                    info.end = dec.GetEnd();
                    list.Add(info);
                }
            }
        }

        private void CollectHighlight(ABnfGuess target_guess, ABnfElement element, List<ALanguageHighlightWordInfo> list)
        {
            if (element is ALittleScriptPropertyValueCustomTypeElement
                || element is ALittleScriptVarAssignNameDecElement)
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

