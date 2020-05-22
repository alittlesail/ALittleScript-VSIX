
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptVarAssignDecReference : ALittleScriptReferenceTemplate<ALittleScriptVarAssignDecElement>
    {
        private string m_namespace_name;
        private ALittleScriptClassDecElement m_class_dec;
        private ALittleScriptTemplateDecElement m_template_param_dec;

        public ALittleScriptVarAssignDecReference(ABnfElement element) : base(element)
        {
            m_namespace_name = ALittleScriptUtility.GetNamespaceName(m_element);
        }

        public ALittleScriptClassDecElement GetClassDec()
        {
            if (m_class_dec != null) return m_class_dec;
            m_class_dec = ALittleScriptUtility.FindClassDecFromParent(m_element);
            return m_class_dec;
        }

        public ALittleScriptTemplateDecElement GetTemplateDec()
        {
            if (m_template_param_dec != null) return m_template_param_dec;
            m_template_param_dec = ALittleScriptUtility.FindMethodTemplateDecFromParent(m_element);
            return m_template_param_dec;
        }

        public override ABnfGuessError GuessTypes(out List<ABnfGuess> guess_list)
        {
            var all_type = m_element.GetAllType();
            if (all_type != null) return all_type.GuessTypes(out guess_list);

            guess_list = new List<ABnfGuess>();
            var name_dec = m_element.GetVarAssignNameDec();
            if (name_dec == null) return null;

            var parent = m_element.GetParent() as ALittleScriptVarAssignExprElement;
            var value_stat = parent.GetValueStat();
            if (value_stat == null)
                return new ABnfGuessError(name_dec, "没有赋值对象，无法推导类型");

            // 获取等号左边的变量定义列表
            var pair_dec_list = parent.GetVarAssignDecList();
            // 计算当前是第几个参数
            int index = pair_dec_list.IndexOf(m_element);
            // 获取函数对应的那个返回值类型
            var error = value_stat.GuessTypes(out List<ABnfGuess> method_call_guess_list);
            if (error != null) return error;
            // 如果有"..."作为返回值结尾
            bool hasTail = method_call_guess_list.Count > 0 && method_call_guess_list[method_call_guess_list.Count - 1] is ALittleScriptGuessReturnTail;
            if (hasTail)
            {
                if (index >= method_call_guess_list.Count - 1)
                    guess_list.Add(ALittleScriptIndex.inst.sAnyGuess);
                else
                    guess_list.Add(method_call_guess_list[index]);
            }
            else
            {
                if (index >= method_call_guess_list.Count)
                    return new ABnfGuessError(m_element, "没有赋值对象，无法推导类型");
                guess_list.Add(method_call_guess_list[index]);
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
                            list.Add(new ALanguageCompletionInfo(dec.GetElementText(), ALittleScriptIndex.inst.sClassIcon));
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
                var template_dec = GetTemplateDec();
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

