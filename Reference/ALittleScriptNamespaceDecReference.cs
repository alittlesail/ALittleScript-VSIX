
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptNamespaceDecReference : ALittleScriptReferenceTemplate<ALittleScriptNamespaceDecElement>
    {
        public ALittleScriptNamespaceDecReference(ABnfElement element) : base(element)
        {

        }

        public override ABnfGuessError CheckError()
        {
            return ALittleScriptUtility.CheckError(m_element, m_element.GetModifierList());
        }

        public override ABnfGuessError GuessTypes(out List<ABnfGuess> guess_list)
        {
            guess_list = null;
            var name_dec = m_element.GetNamespaceNameDec();
            if (name_dec == null)
                return new ABnfGuessError(m_element, "没有定义命名域");

            var info = new ALittleScriptGuessNamespace(name_dec.GetElementText(), m_element);
            info.UpdateValue();

            guess_list = new List<ABnfGuess>() { info };
            return null;
        }

        public override string QueryClassificationTag(out bool blur)
        {
            blur = !ALittleScriptUtility.IsLanguageEnable(m_element.GetModifierList());
            return null;
        }
    }
}

