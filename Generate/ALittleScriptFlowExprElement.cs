
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptFlowExprElement : ABnfNodeElement
	{
		public ALittleScriptFlowExprElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
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
        private bool m_flag_String = false;
        private ALittleScriptStringElement m_cache_String = null;
        public ALittleScriptStringElement GetString()
        {
            if (m_flag_String) return m_cache_String;
            m_flag_String = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptStringElement)
                {
                    m_cache_String = child as ALittleScriptStringElement;
                    break;
                }
            }
            return m_cache_String;
        }

	}
}