
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptClassGetterDecElement : ABnfNodeElement
	{
		public ALittleScriptClassGetterDecElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
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
        private bool m_flag_MethodGetterParamDec = false;
        private ALittleScriptMethodGetterParamDecElement m_cache_MethodGetterParamDec = null;
        public ALittleScriptMethodGetterParamDecElement GetMethodGetterParamDec()
        {
            if (m_flag_MethodGetterParamDec) return m_cache_MethodGetterParamDec;
            m_flag_MethodGetterParamDec = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptMethodGetterParamDecElement)
                {
                    m_cache_MethodGetterParamDec = child as ALittleScriptMethodGetterParamDecElement;
                    break;
                }
            }
            return m_cache_MethodGetterParamDec;
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