
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptClassCtorDecElement : ABnfNodeElement
	{
		public ALittleScriptClassCtorDecElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
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