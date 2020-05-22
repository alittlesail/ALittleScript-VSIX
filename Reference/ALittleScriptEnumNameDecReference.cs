
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptEnumNameDecReference : ALittleScriptReferenceTemplate<ALittleScriptEnumNameDecElement>
    {
        private string m_namespace_name;
        private string m_key;

        public ALittleScriptEnumNameDecReference(ABnfElement element) : base(element)
        {
            m_namespace_name = ALittleScriptUtility.GetNamespaceName(m_element);
            m_key = m_element.GetElementText();
        }

        public override string QueryClassificationTag(out bool blur)
        {
            blur = false;
            return "ALittleScriptDefineName";
        }

        public override ABnfGuessError GuessTypes(out List<ABnfGuess> guess_list)
        {
            return m_element.GetParent().GuessTypes(out guess_list);
        }

        public override ABnfElement GotoDefinition()
        {
            var dec_list = ALittleScriptIndex.inst.FindALittleNameDecList(
                    ALittleScriptUtility.ABnfElementType.ENUM_NAME, m_element.GetFile(), m_namespace_name, m_key, true);
            foreach (var dec in dec_list) return dec;
            return null;
        }

        public override bool QueryCompletion(int offset, List<ALanguageCompletionInfo> list)
        {
            var dec_list = ALittleScriptIndex.inst.FindALittleNameDecList(
                    ALittleScriptUtility.ABnfElementType.ENUM_NAME, m_element.GetFile(), m_namespace_name, m_key, true);

            foreach (var dec in dec_list)
                list.Add(new ALanguageCompletionInfo(dec.GetElementText(), ALittleScriptIndex.inst.sEnumIcon));

            return true;
        }

        public override ABnfGuessError CheckError()
        {
            if (m_element.GetElementText().StartsWith("___"))
                return new ABnfGuessError(m_element, "枚举名不能以3个下划线开头");

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

