
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptConstValueElement : ABnfNodeElement
	{
		public ALittleScriptConstValueElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_Text = false;
        private ALittleScriptTextElement m_cache_Text = null;
        public ALittleScriptTextElement GetText()
        {
            if (m_flag_Text) return m_cache_Text;
            m_flag_Text = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptTextElement)
                {
                    m_cache_Text = child as ALittleScriptTextElement;
                    break;
                }
            }
            return m_cache_Text;
        }
        private bool m_flag_Number = false;
        private ALittleScriptNumberElement m_cache_Number = null;
        public ALittleScriptNumberElement GetNumber()
        {
            if (m_flag_Number) return m_cache_Number;
            m_flag_Number = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptNumberElement)
                {
                    m_cache_Number = child as ALittleScriptNumberElement;
                    break;
                }
            }
            return m_cache_Number;
        }
        private bool m_flag_Key = false;
        private ALittleScriptKeyElement m_cache_Key = null;
        public ALittleScriptKeyElement GetKey()
        {
            if (m_flag_Key) return m_cache_Key;
            m_flag_Key = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptKeyElement)
                {
                    m_cache_Key = child as ALittleScriptKeyElement;
                    break;
                }
            }
            return m_cache_Key;
        }

	}
}