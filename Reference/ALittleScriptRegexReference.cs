
using Microsoft.VisualStudio.Text.Classification;

namespace ALittle
{
    public class ALittleScriptRegexReference : ALittleScriptReferenceTemplate<ALittleScriptRegexElement>
    {
        public ALittleScriptRegexReference(ABnfElement element) : base(element)
        {

        }

        public override bool CanGotoDefinition()
        {
            var parent = m_element.GetParent();
            if (parent == null) return false;
            return parent.GetReference().CanGotoDefinition();
        }
    }
}

