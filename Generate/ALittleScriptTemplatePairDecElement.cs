
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptTemplatePairDecElement : ABnfNodeElement
	{
		public ALittleScriptTemplatePairDecElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_TemplateConst = false;
        private ALittleScriptTemplateConstElement m_cache_TemplateConst = null;
        public ALittleScriptTemplateConstElement GetTemplateConst()
        {
            if (m_flag_TemplateConst) return m_cache_TemplateConst;
            m_flag_TemplateConst = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptTemplateConstElement)
                {
                    m_cache_TemplateConst = child as ALittleScriptTemplateConstElement;
                    break;
                }
            }
            return m_cache_TemplateConst;
        }
        private bool m_flag_TemplateNameDec = false;
        private ALittleScriptTemplateNameDecElement m_cache_TemplateNameDec = null;
        public ALittleScriptTemplateNameDecElement GetTemplateNameDec()
        {
            if (m_flag_TemplateNameDec) return m_cache_TemplateNameDec;
            m_flag_TemplateNameDec = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptTemplateNameDecElement)
                {
                    m_cache_TemplateNameDec = child as ALittleScriptTemplateNameDecElement;
                    break;
                }
            }
            return m_cache_TemplateNameDec;
        }
        private bool m_flag_TemplateExtendsDec = false;
        private ALittleScriptTemplateExtendsDecElement m_cache_TemplateExtendsDec = null;
        public ALittleScriptTemplateExtendsDecElement GetTemplateExtendsDec()
        {
            if (m_flag_TemplateExtendsDec) return m_cache_TemplateExtendsDec;
            m_flag_TemplateExtendsDec = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptTemplateExtendsDecElement)
                {
                    m_cache_TemplateExtendsDec = child as ALittleScriptTemplateExtendsDecElement;
                    break;
                }
            }
            return m_cache_TemplateExtendsDec;
        }

	}
}