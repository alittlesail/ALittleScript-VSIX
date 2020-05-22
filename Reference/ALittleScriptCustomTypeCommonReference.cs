
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptCustomTypeCommonReference<T> : ALittleScriptReferenceTemplate<T> where T : ABnfElement
    {
        protected string m_namespace_name;
        protected string m_key;
        private ALittleScriptClassDecElement m_class_dec;
        private ALittleScriptTemplateDecElement m_template_param_dec;
        private ALittleScriptCustomTypeElement m_custom_type;

        public ALittleScriptCustomTypeCommonReference(ALittleScriptCustomTypeElement custom_type, ABnfElement element) : base(element)
        {
            m_namespace_name = ALittleScriptUtility.GetNamespaceName(m_element);
            m_key = m_element.GetElementText();
            m_custom_type = custom_type;
        }

        public ALittleScriptClassDecElement GetClassDec()
        {
            if (m_class_dec != null) return m_class_dec;
            m_class_dec = ALittleScriptUtility.FindClassDecFromParent(m_element);
            return m_class_dec;
        }

        // 获取函数模板
        public ALittleScriptTemplateDecElement GetMethodTemplateDec()
        {
            if (m_template_param_dec != null) return m_template_param_dec;
            m_template_param_dec = ALittleScriptUtility.FindMethodTemplateDecFromParent(m_element);
            return m_template_param_dec;
        }

        public override ABnfGuessError GuessTypes(out List<ABnfGuess> guess_list)
        {
            guess_list = new List<ABnfGuess>();
            if (m_key.Length == 0) return new ABnfGuessError(m_element, "找不到指定类型, namespace:" + m_namespace_name + ", key:" + m_key);

            var custom_type_template = m_custom_type.GetCustomTypeTemplate();
            {
                var dec_list = ALittleScriptIndex.inst.FindALittleNameDecList(
                        ALittleScriptUtility.ABnfElementType.USING_NAME, m_element.GetFile(), m_namespace_name, m_key, true);
                foreach (var dec in dec_list)
                {
                    var error = dec.GuessType(out ABnfGuess guess);
                    if (error != null) return error;
                    guess_list.Add(guess);
                }
                    
                if (dec_list.Count > 0 && custom_type_template != null && custom_type_template.GetAllTypeList().Count > 0)
                    return new ABnfGuessError(m_element, "使用using定义的类不能再使用模板参数, namespace:" + m_namespace_name + ", key:" + m_key);
            }
            {
                // 根据名字获取对应的类
                var dec_list = ALittleScriptIndex.inst.FindALittleNameDecList(
                        ALittleScriptUtility.ABnfElementType.CLASS_NAME, m_element.GetFile(), m_namespace_name, m_key, true);

                // 获取模板的填充对象，并计算类型
                var src_guess_list = new List<ABnfGuess>();
                List<ALittleScriptAllTypeElement> template_list;
                if (custom_type_template != null)
                {
                    template_list = custom_type_template.GetAllTypeList();
                    foreach (var all_type in template_list)
                    {
                        var error = all_type.GuessType(out ABnfGuess all_type_guess);
                        if (error != null) return error;
                        src_guess_list.Add(all_type_guess);
                    }
                }
                else
                {
                    template_list = new List<ALittleScriptAllTypeElement>();
                }

                // 遍历所有的类
                foreach (var dec in dec_list)
                {
                    // 获取dec的类型
                    var error = dec.GuessType(out ABnfGuess guess);
                    if (error != null) return error;
                    var guess_class = guess as ALittleScriptGuessClass;
                    if (guess_class == null)
                        return new ABnfGuessError(m_element, "模板参数数量和类定义的不一致, namespace:" + m_namespace_name + ", key:" + m_key);
                    // 类模板列表的参数数量必须和填充的一致
                    if (template_list.Count != guess_class.template_list.Count)
                        return new ABnfGuessError(m_element, "模板参数数量和类定义的不一致, namespace:" + m_namespace_name + ", key:" + m_key);

                    // 对比两种
                    for (int i = 0; i < template_list.Count; ++i)
                    {
                        error = ALittleScriptOp.GuessTypeEqual(guess_class.template_list[i], template_list[i], src_guess_list[i], false, false);
                        if (error != null) return error;
                    }

                    if (guess_class.template_list.Count > 0)
                    {
                        var src_class_dec = guess_class.class_dec;
                        var src_class_name_dec = src_class_dec.GetClassNameDec();
                        if (src_class_name_dec == null)
                            return new ABnfGuessError(m_custom_type, "类模板没有定义类名");

                        var info = new ALittleScriptGuessClass(ALittleScriptUtility.GetNamespaceName(src_class_dec),
                                src_class_name_dec.GetElementText(),
                                guess_class.class_dec, guess_class.using_name, guess_class.is_const, guess_class.is_native);
                        info.template_list.AddRange(guess_class.template_list);
                        for (int i = 0; i < guess_class.template_list.Count; ++i)
                        {
                            if (info.template_map.ContainsKey(guess_class.template_list[i].GetValueWithoutConst()))
                                info.template_map.Remove(guess_class.template_list[i].GetValueWithoutConst());
                            info.template_map.Add(guess_class.template_list[i].GetValueWithoutConst(), src_guess_list[i]);
                        }
                        info.UpdateValue();
                        guess = info;
                    }

                    guess_list.Add(guess);
                }
            }
            {
                var class_dec = GetClassDec();
                if (class_dec != null)
                {
                    var dec_list = new List<ABnfElement>();
                    ALittleScriptIndex.inst.FindClassAttrList(class_dec, ALittleScriptUtility.sAccessPrivateAndProtectedAndPublic, ALittleScriptUtility.ClassAttrType.TEMPLATE, m_key, dec_list);
                    // 不能再静态函数中使用模板定义
                    if (dec_list.Count > 0 && ALittleScriptUtility.IsInClassStaticMethod(m_element))
                        return new ABnfGuessError(m_element, "类静态函数不能使用模板符号");
                    foreach (var dec in dec_list)
                    {
                        var error = dec.GuessType(out ABnfGuess guess);
                        if (error != null) return error;
                        guess_list.Add(guess);
                    }
                }
            }
            {
                var template_dec = GetMethodTemplateDec();
                if (template_dec != null)
                {
                    var pair_dec_list = template_dec.GetTemplatePairDecList();
                    foreach (var dec in pair_dec_list)
                    {
                        var name_dec = dec.GetTemplateNameDec();
                        if (name_dec == null) continue;

                        if (name_dec.GetElementText() == m_key)
                        {
                            var error = dec.GuessType(out ABnfGuess guess);
                            if (error != null) return error;
                            guess_list.Add(guess);
                        }
                    }
                }
            }
            {
                var dec_list = ALittleScriptIndex.inst.FindALittleNameDecList(
                        ALittleScriptUtility.ABnfElementType.STRUCT_NAME, m_element.GetFile(), m_namespace_name, m_key, true);
                foreach (var dec in dec_list)
                {
                    var error = dec.GuessType(out ABnfGuess guess);
                    if (error != null) return error;
                    guess_list.Add(guess);
                }
            }
            {
                var dec_list = ALittleScriptIndex.inst.FindALittleNameDecList(
                        ALittleScriptUtility.ABnfElementType.ENUM_NAME, m_element.GetFile(), m_namespace_name, m_key, true);
                foreach (var dec in dec_list)
                {
                    var error = dec.GuessType(out ABnfGuess guess);
                    if (error != null) return error;
                    guess_list.Add(guess);
                }
            }
            if (m_element is ALittleScriptCustomTypeElement)
            {
                var dec_list = ALittleScriptIndex.inst.FindNamespaceNameDecList(m_key);
                foreach (var dec in dec_list)
                {
                    var error = dec.Value.GuessType(out ABnfGuess guess);
                    if (error != null) return error;
                    guess_list.Add(guess);
                }
            }

            if (guess_list.Count == 0) return new ABnfGuessError(m_element, "找不到指定类型, namespace:" + m_namespace_name + ", key:" + m_key);

            return null;
        }

        public override ABnfElement GotoDefinition()
        {
            {
                var dec_list = ALittleScriptIndex.inst.FindALittleNameDecList(
                        ALittleScriptUtility.ABnfElementType.USING_NAME,  m_element.GetFile(), m_namespace_name, m_key, true);
                foreach (var dec in dec_list) return dec;
            }
            {
                var dec_list = ALittleScriptIndex.inst.FindALittleNameDecList(
                        ALittleScriptUtility.ABnfElementType.CLASS_NAME,  m_element.GetFile(), m_namespace_name, m_key, true);
                foreach (var dec in dec_list) return dec;
            }
            {
                var class_dec = GetClassDec();
                if (class_dec != null)
                {
                    var dec_list = new List<ABnfElement>();
                    ALittleScriptIndex.inst.FindClassAttrList(class_dec, ALittleScriptUtility.sAccessPrivateAndProtectedAndPublic, ALittleScriptUtility.ClassAttrType.TEMPLATE, m_key, dec_list);
                    foreach (var dec in dec_list) return dec;
                }
            }
            {
                var template_dec = GetMethodTemplateDec();
                if (template_dec != null)
                {
                    var pair_dec_list = template_dec.GetTemplatePairDecList();
                    foreach (var pair_dec in pair_dec_list)
                    {
                        var pair_name_dec = pair_dec.GetTemplateNameDec();
                        if (pair_name_dec != null && pair_name_dec.GetElementText() == m_key)
                            return pair_name_dec;
                    }
                }
            }
            {
                var dec_list = ALittleScriptIndex.inst.FindALittleNameDecList(
                        ALittleScriptUtility.ABnfElementType.STRUCT_NAME,  m_element.GetFile(), m_namespace_name, m_key, true);
                foreach (var dec in dec_list) return dec;
            }
            {
                var dec_list = ALittleScriptIndex.inst.FindALittleNameDecList(
                        ALittleScriptUtility.ABnfElementType.STRUCT_NAME,  m_element.GetFile(), m_namespace_name, m_key, true);
                foreach (var dec in dec_list) return dec;
            }

            return null;
        }

        public override bool QueryCompletion(int offset, List<ALanguageCompletionInfo> list)
        {
            // 查找该命名域下的
            {
                var dec_list = ALittleScriptIndex.inst.FindALittleNameDecList(
                        ALittleScriptUtility.ABnfElementType.USING_NAME, m_element.GetFile(), m_namespace_name, "", true);
                foreach (var dec in dec_list)
                {
                    var error = dec.GuessType(out ABnfGuess guess);
                    if (error != null)
                    {
                        list.Add(new ALanguageCompletionInfo(dec.GetElementText(), ALittleScriptIndex.inst.sClassIcon));
                    }
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

            // 查找对应命名域下的类名
            {
                var dec_list = ALittleScriptIndex.inst.FindALittleNameDecList(
                        ALittleScriptUtility.ABnfElementType.CLASS_NAME, m_element.GetFile(), m_namespace_name, "", true);
                foreach (var dec in dec_list)
                    list.Add(new ALanguageCompletionInfo(dec.GetElementText(), ALittleScriptIndex.inst.sClassIcon));
            }
            // 查找类模板
            {
                var class_dec = GetClassDec();
                if (class_dec != null)
                {
                    var dec_list = new List<ABnfElement>();
                    ALittleScriptIndex.inst.FindClassAttrList(class_dec, ALittleScriptUtility.sAccessPrivateAndProtectedAndPublic
                        , ALittleScriptUtility.ClassAttrType.TEMPLATE, "", dec_list);
                    foreach (var dec in dec_list)
                    {
                        var pair_dec = dec as ALittleScriptTemplatePairDecElement;
                        var pair_name_dec = pair_dec.GetTemplateNameDec();
                        if (pair_name_dec != null)
                            list.Add(new ALanguageCompletionInfo(pair_name_dec.GetElementText(), ALittleScriptIndex.inst.sTemplateIcon));
                    }
                }
            }
            // 查找函数模板
            {
                var template_dec = GetMethodTemplateDec();
                if (template_dec != null)
                {
                    var pair_dec_list = template_dec.GetTemplatePairDecList();
                    foreach (var pair_dec in pair_dec_list)
                    {
                        var pair_name_dec = pair_dec.GetTemplateNameDec();
                        if (pair_name_dec != null)
                            list.Add(new ALanguageCompletionInfo(pair_name_dec.GetElementText(), ALittleScriptIndex.inst.sTemplateIcon));
                    }
                }
            }
            // 结构体名
            {
                var dec_list = ALittleScriptIndex.inst.FindALittleNameDecList(
                        ALittleScriptUtility.ABnfElementType.STRUCT_NAME, m_element.GetFile(), m_namespace_name, "", true);
                foreach (var dec in dec_list)
                    list.Add(new ALanguageCompletionInfo(dec.GetElementText(), ALittleScriptIndex.inst.sStructIcon));
            }
            // 枚举名
            {
                var dec_list = ALittleScriptIndex.inst.FindALittleNameDecList(
                        ALittleScriptUtility.ABnfElementType.ENUM_NAME, m_element.GetFile(), m_namespace_name, "", true);
                foreach (var dec in dec_list)
                    list.Add(new ALanguageCompletionInfo(dec.GetElementText(), ALittleScriptIndex.inst.sEnumIcon));
            }
            // 查找全局函数
            {
                var dec_list = ALittleScriptIndex.inst.FindALittleNameDecList(
                        ALittleScriptUtility.ABnfElementType.GLOBAL_METHOD, m_element.GetFile(), m_namespace_name, "", true);
                foreach (var dec in dec_list)
                    list.Add(new ALanguageCompletionInfo(dec.GetElementText(), ALittleScriptIndex.inst.sGlobalMethodIcon));
            }
            // 查找所有命名域
            {
                var dec_map = ALittleScriptIndex.inst.FindNamespaceNameDecList("");
                foreach (var pair in dec_map)
                    list.Add(new ALanguageCompletionInfo(pair.Value.GetElementText(), ALittleScriptIndex.inst.sNamespaceIcon));
            }

            return true;
        }
    }
}

