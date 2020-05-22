
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptClassSetterDecElement : ABnfNodeElement
	{
		public ALittleScriptClassSetterDecElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
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
        private bool m_flag_MethodSetterParamDec = false;
        private ALittleScriptMethodSetterParamDecElement m_cache_MethodSetterParamDec = null;
        public ALittleScriptMethodSetterParamDecElement GetMethodSetterParamDec()
        {
            if (m_flag_MethodSetterParamDec) return m_cache_MethodSetterParamDec;
            m_flag_MethodSetterParamDec = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptMethodSetterParamDecElement)
                {
                    m_cache_MethodSetterParamDec = child as ALittleScriptMethodSetterParamDecElement;
                    break;
                }
            }
            return m_cache_MethodSetterParamDec;
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