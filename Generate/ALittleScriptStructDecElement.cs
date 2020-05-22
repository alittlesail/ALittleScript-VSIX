
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptStructDecElement : ABnfNodeElement
	{
		public ALittleScriptStructDecElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_StructNameDec = false;
        private ALittleScriptStructNameDecElement m_cache_StructNameDec = null;
        public ALittleScriptStructNameDecElement GetStructNameDec()
        {
            if (m_flag_StructNameDec) return m_cache_StructNameDec;
            m_flag_StructNameDec = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptStructNameDecElement)
                {
                    m_cache_StructNameDec = child as ALittleScriptStructNameDecElement;
                    break;
                }
            }
            return m_cache_StructNameDec;
        }
        private bool m_flag_StructExtendsDec = false;
        private ALittleScriptStructExtendsDecElement m_cache_StructExtendsDec = null;
        public ALittleScriptStructExtendsDecElement GetStructExtendsDec()
        {
            if (m_flag_StructExtendsDec) return m_cache_StructExtendsDec;
            m_flag_StructExtendsDec = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptStructExtendsDecElement)
                {
                    m_cache_StructExtendsDec = child as ALittleScriptStructExtendsDecElement;
                    break;
                }
            }
            return m_cache_StructExtendsDec;
        }
        private bool m_flag_StructBodyDec = false;
        private ALittleScriptStructBodyDecElement m_cache_StructBodyDec = null;
        public ALittleScriptStructBodyDecElement GetStructBodyDec()
        {
            if (m_flag_StructBodyDec) return m_cache_StructBodyDec;
            m_flag_StructBodyDec = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptStructBodyDecElement)
                {
                    m_cache_StructBodyDec = child as ALittleScriptStructBodyDecElement;
                    break;
                }
            }
            return m_cache_StructBodyDec;
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