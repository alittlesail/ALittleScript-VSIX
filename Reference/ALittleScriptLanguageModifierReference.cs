
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptLanguageModifierReference : ALittleScriptReferenceTemplate<ALittleScriptLanguageModifierElement>
    {
        private HashSet<string> m_name_set = null;

        public ALittleScriptLanguageModifierReference(ABnfElement element) : base(element) { }

        public override ABnfGuessError CheckError()
        {
            var body_dec = m_element.GetLanguageBodyDec();
            if (body_dec == null)
                return new ABnfGuessError(m_element, "请定义你限定的语言范围");

            if (body_dec.GetLanguageNameDecList().Count == 0)
                return new ABnfGuessError(m_element, "请定义你限定的语言范围");

            return null;
        }

        public bool IsLanguageEnable()
        {
            if (m_name_set == null)
            {
                m_name_set = new HashSet<string>();

                var body_dec = m_element.GetLanguageBodyDec();
                if (body_dec == null) return false;

                var name_list = body_dec.GetLanguageNameDecList();
                foreach (var name in name_list)
                {
                    var text = name.GetElementText();
                    if (!m_name_set.Contains(text))
                        m_name_set.Add(text);
                }
            }

            return m_name_set.Contains(GeneralOptions.Instance.GetTargetLanguageString());
        }
    }
}

