
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptIdReference : ALittleScriptReferenceTemplate<ALittleScriptIdElement>
    {
        public ALittleScriptIdReference(ABnfElement element) : base(element) { }

        public override string QueryQuickInfo()
        {
            var parent = m_element.GetParent();
            if (parent == null) return null;
            return parent.GetReference().QueryQuickInfo();
        }

        public override ABnfElement GotoDefinition()
        {
            var parent = m_element.GetParent();
            if (parent == null) return null;
            return parent.GetReference().GotoDefinition();
        }

        public override bool QueryCompletion(int offset, List<ALanguageCompletionInfo> list)
        {
            var parent = m_element.GetParent();
            if (parent == null) return false;
            return parent.GetReference().QueryCompletion(offset, list);
        }

        public override bool PeekHighlightWord()
        {
            var parent = m_element.GetParent();
            if (parent == null) return false;
            return parent.GetReference().PeekHighlightWord();
        }

        public override void QueryHighlightWordTag(List<ALanguageHighlightWordInfo> list)
        {
            var parent = m_element.GetParent();
            if (parent == null) return;
            parent.GetReference().QueryHighlightWordTag(list);
        }
    }
}

