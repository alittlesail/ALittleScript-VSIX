
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptTemplateExtendsDecElement : ABnfNodeElement
	{
		public ALittleScriptTemplateExtendsDecElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_AllType = false;
        private ALittleScriptAllTypeElement m_cache_AllType = null;
        public ALittleScriptAllTypeElement GetAllType()
        {
            if (m_flag_AllType) return m_cache_AllType;
            m_flag_AllType = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptAllTypeElement)
                {
                    m_cache_AllType = child as ALittleScriptAllTypeElement;
                    break;
                }
            }
            return m_cache_AllType;
        }
        private bool m_flag_TemplateExtendsClassDec = false;
        private ALittleScriptTemplateExtendsClassDecElement m_cache_TemplateExtendsClassDec = null;
        public ALittleScriptTemplateExtendsClassDecElement GetTemplateExtendsClassDec()
        {
            if (m_flag_TemplateExtendsClassDec) return m_cache_TemplateExtendsClassDec;
            m_flag_TemplateExtendsClassDec = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptTemplateExtendsClassDecElement)
                {
                    m_cache_TemplateExtendsClassDec = child as ALittleScriptTemplateExtendsClassDecElement;
                    break;
                }
            }
            return m_cache_TemplateExtendsClassDec;
        }
        private bool m_flag_TemplateExtendsStructDec = false;
        private ALittleScriptTemplateExtendsStructDecElement m_cache_TemplateExtendsStructDec = null;
        public ALittleScriptTemplateExtendsStructDecElement GetTemplateExtendsStructDec()
        {
            if (m_flag_TemplateExtendsStructDec) return m_cache_TemplateExtendsStructDec;
            m_flag_TemplateExtendsStructDec = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptTemplateExtendsStructDecElement)
                {
                    m_cache_TemplateExtendsStructDec = child as ALittleScriptTemplateExtendsStructDecElement;
                    break;
                }
            }
            return m_cache_TemplateExtendsStructDec;
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