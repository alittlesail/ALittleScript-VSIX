
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptClassDecElement : ABnfNodeElement
	{
		public ALittleScriptClassDecElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_ClassNameDec = false;
        private ALittleScriptClassNameDecElement m_cache_ClassNameDec = null;
        public ALittleScriptClassNameDecElement GetClassNameDec()
        {
            if (m_flag_ClassNameDec) return m_cache_ClassNameDec;
            m_flag_ClassNameDec = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptClassNameDecElement)
                {
                    m_cache_ClassNameDec = child as ALittleScriptClassNameDecElement;
                    break;
                }
            }
            return m_cache_ClassNameDec;
        }
        private bool m_flag_TemplateDec = false;
        private ALittleScriptTemplateDecElement m_cache_TemplateDec = null;
        public ALittleScriptTemplateDecElement GetTemplateDec()
        {
            if (m_flag_TemplateDec) return m_cache_TemplateDec;
            m_flag_TemplateDec = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptTemplateDecElement)
                {
                    m_cache_TemplateDec = child as ALittleScriptTemplateDecElement;
                    break;
                }
            }
            return m_cache_TemplateDec;
        }
        private bool m_flag_ClassExtendsDec = false;
        private ALittleScriptClassExtendsDecElement m_cache_ClassExtendsDec = null;
        public ALittleScriptClassExtendsDecElement GetClassExtendsDec()
        {
            if (m_flag_ClassExtendsDec) return m_cache_ClassExtendsDec;
            m_flag_ClassExtendsDec = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptClassExtendsDecElement)
                {
                    m_cache_ClassExtendsDec = child as ALittleScriptClassExtendsDecElement;
                    break;
                }
            }
            return m_cache_ClassExtendsDec;
        }
        private bool m_flag_ClassBodyDec = false;
        private ALittleScriptClassBodyDecElement m_cache_ClassBodyDec = null;
        public ALittleScriptClassBodyDecElement GetClassBodyDec()
        {
            if (m_flag_ClassBodyDec) return m_cache_ClassBodyDec;
            m_flag_ClassBodyDec = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptClassBodyDecElement)
                {
                    m_cache_ClassBodyDec = child as ALittleScriptClassBodyDecElement;
                    break;
                }
            }
            return m_cache_ClassBodyDec;
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