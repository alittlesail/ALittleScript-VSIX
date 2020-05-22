
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptClassNameDecReference : ALittleScriptReferenceTemplate<ALittleScriptClassNameDecElement>
    {
        string m_namespace_name;
        string m_key;
        public ALittleScriptClassNameDecReference(ABnfElement element) : base(element)
        {
            m_namespace_name = ALittleScriptUtility.GetNamespaceName(m_element);

            // 如果父节点是extends，那么就获取指定的命名域
            var parent = element.GetParent();
            if (parent is ALittleScriptClassExtendsDecElement)
            {
                var namespace_name_dec = ((ALittleScriptClassExtendsDecElement)parent).GetNamespaceNameDec();
                if (namespace_name_dec != null)
                    m_namespace_name = namespace_name_dec.GetElementText();
            }

            m_key = m_element.GetElementText();
        }

        public override string QueryClassificationTag(out bool blur)
        {
            blur = false;
            return "ALittleScriptDefineName";
        }

        public override ABnfGuessError GuessTypes(out List<ABnfGuess> guess_list)
        {
            guess_list = new List<ABnfGuess>();
            var parent = m_element.GetParent();

            // 如果直接就是定义，那么直接获取
            if (parent is ALittleScriptClassDecElement)
            {
                var error = parent.GuessType(out ABnfGuess guess);
                if (error != null) return error;
                guess_list.Add(guess);
            }
            // 如果是继承那么就从继承那边获取
            else if (parent is ALittleScriptClassExtendsDecElement)
            {
                if (m_key.Length == 0)
                    return new ABnfGuessError(m_element, "找不到类, namespace:" + m_namespace_name + ", key:" + m_key);

                // 查找继承
                var class_name_dec_list = ALittleScriptIndex.inst.FindALittleNameDecList(
                        ALittleScriptUtility.ABnfElementType.CLASS_NAME, m_element.GetFile(), m_namespace_name, m_key, true);
                if (class_name_dec_list.Count == 0)
                    return new ABnfGuessError(m_element, "找不到类, namespace:" + m_namespace_name + ", key:" + m_key);
                
                foreach (var class_name_dec in class_name_dec_list)
                {
                    var error = class_name_dec.GuessType(out ABnfGuess guess);
                    if (error != null) return error;
                    if (!(guess is ALittleScriptGuessClass))
                        return new ABnfGuessError(m_element, "继承的不是一个类, namespace:" + m_namespace_name + ", key:" + m_key);

                    var guess_class = guess as ALittleScriptGuessClass;
                    if (guess_class.template_list.Count > 0)
                    {
                        var sub_class = parent.GetParent() as ALittleScriptClassDecElement;
                        if (sub_class == null)
                            return new ABnfGuessError(parent, "定义不完整");

                        var sub_template_dec = sub_class.GetTemplateDec();
                        if (sub_template_dec == null)
                            return new ABnfGuessError(parent, "子类的模板参数列表必须涵盖父类的模板参数列表");

                        var sub_template_pair_list = sub_template_dec.GetTemplatePairDecList();
                        if (sub_template_pair_list.Count < guess_class.template_list.Count)
                            return new ABnfGuessError(parent, "子类的模板参数列表必须涵盖父类的模板参数列表");

                        for (int i = 0; i < guess_class.template_list.Count; ++i)
                        {
                            error = sub_template_pair_list[i].GuessType(out ABnfGuess sub_template);
                            if (error != null) return error;
                            error = ALittleScriptOp.GuessTypeEqual(guess_class.template_list[i], sub_template_pair_list[i], sub_template, false, false);
                            if (error != null)
                                return new ABnfGuessError(sub_template_pair_list[i], "子类的模板参数和父类的模板参数不一致:" + error.GetError());
                        }
                    }
                    guess_list.Add(guess);
                }
            }
            else
            {
                return new ABnfGuessError(m_element, "ALittleClassNameDec出现未知的父节点");
            }

            return null;
        }

        public override ABnfGuessError CheckError()
        {
            if (m_element.GetElementText().StartsWith("___"))
                return new ABnfGuessError(m_element, "类名不能以3个下划线开头");

            var error = m_element.GuessTypes(out List<ABnfGuess> guess_list);
            if (error != null) return error;
            if (guess_list.Count == 0)
                return new ABnfGuessError(m_element, "未知类型");
            else if (guess_list.Count != 1)
                return new ABnfGuessError(m_element, "重复定义");
            return null;
        }

        public override ABnfElement GotoDefinition()
        {
            var dec = ALittleScriptIndex.inst.FindALittleNameDec(
               ALittleScriptUtility.ABnfElementType.CLASS_NAME, m_element.GetFile(), m_namespace_name, m_key, true);
            if (dec != null) return dec;
            return null;
        }

        // 输入智能补全
        public override bool QueryCompletion(int offset, List<ALanguageCompletionInfo> list)
        {
            {
                var dec_list = ALittleScriptIndex.inst.FindALittleNameDecList(
                    ALittleScriptUtility.ABnfElementType.CLASS_NAME, m_element.GetFile(), m_namespace_name, "", true);

                foreach (var dec in dec_list)
                    list.Add(new ALanguageCompletionInfo(dec.GetElementText(), ALittleScriptIndex.inst.sClassIcon));
            }

            if (m_element.GetParent() is ALittleScriptClassExtendsDecElement)
            {
                var dec_list = ALittleScriptIndex.inst.FindNamespaceNameDecList("");
                foreach (var pair in dec_list)
                    list.Add(new ALanguageCompletionInfo(pair.Key, ALittleScriptIndex.inst.sNamespaceIcon));
            }
            return true;
        }
    }
}

