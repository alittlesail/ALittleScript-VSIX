
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptGenericFunctorTypeElement : ABnfNodeElement
	{
		public ALittleScriptGenericFunctorTypeElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_AllTypeConst = false;
        private ALittleScriptAllTypeConstElement m_cache_AllTypeConst = null;
        public ALittleScriptAllTypeConstElement GetAllTypeConst()
        {
            if (m_flag_AllTypeConst) return m_cache_AllTypeConst;
            m_flag_AllTypeConst = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptAllTypeConstElement)
                {
                    m_cache_AllTypeConst = child as ALittleScriptAllTypeConstElement;
                    break;
                }
            }
            return m_cache_AllTypeConst;
        }
        private bool m_flag_CoroutineModifier = false;
        private ALittleScriptCoroutineModifierElement m_cache_CoroutineModifier = null;
        public ALittleScriptCoroutineModifierElement GetCoroutineModifier()
        {
            if (m_flag_CoroutineModifier) return m_cache_CoroutineModifier;
            m_flag_CoroutineModifier = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptCoroutineModifierElement)
                {
                    m_cache_CoroutineModifier = child as ALittleScriptCoroutineModifierElement;
                    break;
                }
            }
            return m_cache_CoroutineModifier;
        }
        private bool m_flag_GenericFunctorParamType = false;
        private ALittleScriptGenericFunctorParamTypeElement m_cache_GenericFunctorParamType = null;
        public ALittleScriptGenericFunctorParamTypeElement GetGenericFunctorParamType()
        {
            if (m_flag_GenericFunctorParamType) return m_cache_GenericFunctorParamType;
            m_flag_GenericFunctorParamType = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptGenericFunctorParamTypeElement)
                {
                    m_cache_GenericFunctorParamType = child as ALittleScriptGenericFunctorParamTypeElement;
                    break;
                }
            }
            return m_cache_GenericFunctorParamType;
        }
        private bool m_flag_GenericFunctorReturnType = false;
        private ALittleScriptGenericFunctorReturnTypeElement m_cache_GenericFunctorReturnType = null;
        public ALittleScriptGenericFunctorReturnTypeElement GetGenericFunctorReturnType()
        {
            if (m_flag_GenericFunctorReturnType) return m_cache_GenericFunctorReturnType;
            m_flag_GenericFunctorReturnType = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptGenericFunctorReturnTypeElement)
                {
                    m_cache_GenericFunctorReturnType = child as ALittleScriptGenericFunctorReturnTypeElement;
                    break;
                }
            }
            return m_cache_GenericFunctorReturnType;
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
        List<ALittleScriptStringElement> m_list_String = null;
        public List<ALittleScriptStringElement> GetStringList()
        {
            var list = new List<ALittleScriptStringElement>();
            if (m_list_String == null)
            {
                m_list_String = new List<ALittleScriptStringElement>();
                foreach (var child in m_childs)
                {
                    if (child is ALittleScriptStringElement)
                        m_list_String.Add(child as ALittleScriptStringElement);
                }   
            }
            list.AddRange(m_list_String);
            return list;
        }

	}
}