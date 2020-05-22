
using Microsoft.VisualStudio.Text.Classification;

namespace ALittle
{
    public class ALittleScriptStructOptionNameDecReference : ALittleScriptReferenceTemplate<ALittleScriptStructOptionNameDecElement>
    {
        public ALittleScriptStructOptionNameDecReference(ABnfElement element) : base(element)
        {

        }

        public override string QueryClassificationTag(out bool blur)
        {
            blur = false;
            var text = m_element.GetElementText();
            if (text == "primary" || text == "unique" || text == "index")
                return "ALittleScriptCtrlKeyWord";
            return "ALittleScriptVarName";
        }
    }
}

