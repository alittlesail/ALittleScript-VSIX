
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptStructNameDecReference : ALittleScriptReferenceTemplate<ALittleScriptStructNameDecElement>
    {
        private string m_namespace_name;
        private string m_key;

        public ALittleScriptStructNameDecReference(ABnfElement element) : base(element)
        {
            m_key = m_element.GetElementText();
            m_namespace_name = ALittleScriptUtility.GetNamespaceName(m_element);

            // 如果父节点是extends，那么就获取指定的命名域
            var parent = element.GetParent();
            if (parent is ALittleScriptStructExtendsDecElement)
            {
                var namespace_name_dec = ((ALittleScriptStructExtendsDecElement)parent).GetNamespaceNameDec();
                if (namespace_name_dec != null)
                    m_namespace_name = namespace_name_dec.GetElementText();
            }
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
            if (parent is ALittleScriptStructDecElement)
            {
                var error = parent.GuessType(out ABnfGuess guess);
                if (error != null) return error;
                guess_list.Add(guess);
                // 如果是继承那么就从继承那边获取
            }
            else if (parent is ALittleScriptStructExtendsDecElement)
            {
                if (m_key.Length == 0) return null;
                var struct_name_dec_list = ALittleScriptIndex.inst.FindALittleNameDecList(
                        ALittleScriptUtility.ABnfElementType.STRUCT_NAME, m_element.GetFile(), m_namespace_name, m_key, true);
                foreach (var struct_name_dec in struct_name_dec_list)
                {
                    var error = struct_name_dec.GuessType(out ABnfGuess guess);
                    if (error != null) return error;
                    guess_list.Add(guess);
                }
            }
            return null;
        }

        public override ABnfElement GotoDefinition()
        {   
            var dec_list = ALittleScriptIndex.inst.FindALittleNameDecList(
                ALittleScriptUtility.ABnfElementType.STRUCT_NAME, m_element.GetFile(), m_namespace_name, m_key, true);
            foreach (var dec in dec_list) return dec;
            return null;
        }

        public override bool QueryCompletion(int offset, List<ALanguageCompletionInfo> list)
        {
            var dec_list = ALittleScriptIndex.inst.FindALittleNameDecList(
                ALittleScriptUtility.ABnfElementType.STRUCT_NAME, m_element.GetFile(), m_namespace_name, "", true);

            foreach (var dec in dec_list)
                list.Add(new ALanguageCompletionInfo(dec.GetElementText(), ALittleScriptIndex.inst.sStructIcon));

            return true;
        }

        public override ABnfGuessError CheckError()
        {
            if (m_element.GetElementText().StartsWith("___"))
                return new ABnfGuessError(m_element, "结构体名不能以3个下划线开头");

            var error = m_element.GuessTypes(out List<ABnfGuess> guess_list);
            if (error != null) return error;
            if (guess_list.Count == 0)
                return new ABnfGuessError(m_element, "未知类型");
            else if (guess_list.Count != 1)
                return new ABnfGuessError(m_element, "重复定义");
            return null;
        }
    }
}

