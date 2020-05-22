
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptEnumDecElement : ABnfNodeElement
	{
		public ALittleScriptEnumDecElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_EnumNameDec = false;
        private ALittleScriptEnumNameDecElement m_cache_EnumNameDec = null;
        public ALittleScriptEnumNameDecElement GetEnumNameDec()
        {
            if (m_flag_EnumNameDec) return m_cache_EnumNameDec;
            m_flag_EnumNameDec = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptEnumNameDecElement)
                {
                    m_cache_EnumNameDec = child as ALittleScriptEnumNameDecElement;
                    break;
                }
            }
            return m_cache_EnumNameDec;
        }
        private bool m_flag_EnumBodyDec = false;
        private ALittleScriptEnumBodyDecElement m_cache_EnumBodyDec = null;
        public ALittleScriptEnumBodyDecElement GetEnumBodyDec()
        {
            if (m_flag_EnumBodyDec) return m_cache_EnumBodyDec;
            m_flag_EnumBodyDec = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptEnumBodyDecElement)
                {
                    m_cache_EnumBodyDec = child as ALittleScriptEnumBodyDecElement;
                    break;
                }
            }
            return m_cache_EnumBodyDec;
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