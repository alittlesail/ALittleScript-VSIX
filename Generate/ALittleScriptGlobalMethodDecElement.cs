
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptGlobalMethodDecElement : ABnfNodeElement
	{
		public ALittleScriptGlobalMethodDecElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_MethodNameDec = false;
        private ALittleScriptMethodNameDecElement m_cache_MethodNameDec = null;
        public ALittleScriptMethodNameDecElement GetMethodNameDec()
        {
            if (m_flag_MethodNameDec) return m_cache_MethodNameDec;
            m_flag_MethodNameDec = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptMethodNameDecElement)
                {
                    m_cache_MethodNameDec = child as ALittleScriptMethodNameDecElement;
                    break;
                }
            }
            return m_cache_MethodNameDec;
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
        private bool m_flag_MethodParamDec = false;
        private ALittleScriptMethodParamDecElement m_cache_MethodParamDec = null;
        public ALittleScriptMethodParamDecElement GetMethodParamDec()
        {
            if (m_flag_MethodParamDec) return m_cache_MethodParamDec;
            m_flag_MethodParamDec = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptMethodParamDecElement)
                {
                    m_cache_MethodParamDec = child as ALittleScriptMethodParamDecElement;
                    break;
                }
            }
            return m_cache_MethodParamDec;
        }
        private bool m_flag_MethodReturnDec = false;
        private ALittleScriptMethodReturnDecElement m_cache_MethodReturnDec = null;
        public ALittleScriptMethodReturnDecElement GetMethodReturnDec()
        {
            if (m_flag_MethodReturnDec) return m_cache_MethodReturnDec;
            m_flag_MethodReturnDec = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptMethodReturnDecElement)
                {
                    m_cache_MethodReturnDec = child as ALittleScriptMethodReturnDecElement;
                    break;
                }
            }
            return m_cache_MethodReturnDec;
        }
        private bool m_flag_MethodBodyDec = false;
        private ALittleScriptMethodBodyDecElement m_cache_MethodBodyDec = null;
        public ALittleScriptMethodBodyDecElement GetMethodBodyDec()
        {
            if (m_flag_MethodBodyDec) return m_cache_MethodBodyDec;
            m_flag_MethodBodyDec = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptMethodBodyDecElement)
                {
                    m_cache_MethodBodyDec = child as ALittleScriptMethodBodyDecElement;
                    break;
                }
            }
            return m_cache_MethodBodyDec;
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