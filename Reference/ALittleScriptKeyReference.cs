
using Microsoft.VisualStudio.Text.Classification;

namespace ALittle
{
    public class ALittleScriptKeyReference : ALittleScriptReferenceTemplate<ALittleScriptKeyElement>
    {
        public ALittleScriptKeyReference(ABnfElement element) : base(element)
        {

        }

        public override string QueryClassificationTag(out bool blur)
        {
            blur = false;
            var text = m_element.GetElementText();
            if (ALittleScriptIndex.inst.sCtrlKeyWord.Contains(text))
                return "ALittleScriptCtrlKeyWord";
            return "ALittleScriptKeyWord";
        }

        public override ABnfElement GotoDefinition()
        {
            var parent = m_element.GetParent();
            if (parent == null) return null;
            return parent.GetReference().GotoDefinition();
        }

        public override bool CanGotoDefinition()
        {
            return false;
        }
    }
}

