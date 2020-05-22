
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptGenericTypeElement : ABnfNodeElement
	{
		public ALittleScriptGenericTypeElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_GenericListType = false;
        private ALittleScriptGenericListTypeElement m_cache_GenericListType = null;
        public ALittleScriptGenericListTypeElement GetGenericListType()
        {
            if (m_flag_GenericListType) return m_cache_GenericListType;
            m_flag_GenericListType = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptGenericListTypeElement)
                {
                    m_cache_GenericListType = child as ALittleScriptGenericListTypeElement;
                    break;
                }
            }
            return m_cache_GenericListType;
        }
        private bool m_flag_GenericMapType = false;
        private ALittleScriptGenericMapTypeElement m_cache_GenericMapType = null;
        public ALittleScriptGenericMapTypeElement GetGenericMapType()
        {
            if (m_flag_GenericMapType) return m_cache_GenericMapType;
            m_flag_GenericMapType = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptGenericMapTypeElement)
                {
                    m_cache_GenericMapType = child as ALittleScriptGenericMapTypeElement;
                    break;
                }
            }
            return m_cache_GenericMapType;
        }
        private bool m_flag_GenericFunctorType = false;
        private ALittleScriptGenericFunctorTypeElement m_cache_GenericFunctorType = null;
        public ALittleScriptGenericFunctorTypeElement GetGenericFunctorType()
        {
            if (m_flag_GenericFunctorType) return m_cache_GenericFunctorType;
            m_flag_GenericFunctorType = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptGenericFunctorTypeElement)
                {
                    m_cache_GenericFunctorType = child as ALittleScriptGenericFunctorTypeElement;
                    break;
                }
            }
            return m_cache_GenericFunctorType;
        }

	}
}