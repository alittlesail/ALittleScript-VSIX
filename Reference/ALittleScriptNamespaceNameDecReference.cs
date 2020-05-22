
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptNamespaceNameDecReference : ALittleScriptReferenceTemplate<ALittleScriptNamespaceNameDecElement>
    {
        private string m_key;
        public ALittleScriptNamespaceNameDecReference(ABnfElement element) : base(element)
        {
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
            if (m_key.Length == 0) return null;

            var name_dec_map = ALittleScriptIndex.inst.FindNamespaceNameDecList(m_key);
            foreach (var pair in name_dec_map)
            {
                var error = pair.Value.GetParent().GuessType(out ABnfGuess guess);
                if (error != null) return error;
                guess_list.Add(guess);
            }
            return null;
        }

        public override ABnfGuessError CheckError()
        {
            if (m_element.GetElementText().StartsWith("___"))
                return new ABnfGuessError(m_element, "命名域不能以3个下划线开头");

            var error = m_element.GuessTypes(out List<ABnfGuess> guess_list);
            if (error != null) return error;
            if (guess_list.Count == 0)
                return new ABnfGuessError(m_element, "未知类型");
            return null;
        }

        public override ABnfElement GotoDefinition()
        {
            var dec_map = ALittleScriptIndex.inst.FindNamespaceNameDecList(m_key);
            foreach (var pair in dec_map) return pair.Value;
            return null;
        }

        public override bool QueryCompletion(int offset, List<ALanguageCompletionInfo> list)
        {
            var dec_map = ALittleScriptIndex.inst.FindNamespaceNameDecList(m_key);
            foreach (var pair in dec_map)
                list.Add(new ALanguageCompletionInfo(pair.Value.GetElementText(), ALittleScriptIndex.inst.sNamespaceIcon));
            return true;
        }
    }
}

