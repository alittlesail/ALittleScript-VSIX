
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptLanguageNameDecReference : ALittleScriptReferenceTemplate<ALittleScriptLanguageNameDecElement>
    {
        public ALittleScriptLanguageNameDecReference(ABnfElement element) : base(element) { }

        public override ABnfGuessError CheckError()
        {
            var text = m_element.GetElementText();
            if (!GeneralOptions.Instance.TargetLanguageNameSet.Contains(text))
                return new ABnfGuessError(m_element, "不支持该目标语言");
            return null;
        }

        public override bool QueryCompletion(int offset, List<ALanguageCompletionInfo> list)
        {
            foreach (var name in GeneralOptions.Instance.TargetLanguageNameSet)
                list.Add(new ALanguageCompletionInfo(name, ALittleScriptIndex.inst.sLanguageIcon));
            return true;
        }

        public override string QueryClassificationTag(out bool blur)
        {
            blur = false;
            return "ALittleScriptCustomName";
        }
    }
}

