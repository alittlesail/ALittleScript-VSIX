
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptAllTypeElement : ABnfNodeElement
	{
		public ALittleScriptAllTypeElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
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
        private bool m_flag_PrimitiveType = false;
        private ALittleScriptPrimitiveTypeElement m_cache_PrimitiveType = null;
        public ALittleScriptPrimitiveTypeElement GetPrimitiveType()
        {
            if (m_flag_PrimitiveType) return m_cache_PrimitiveType;
            m_flag_PrimitiveType = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptPrimitiveTypeElement)
                {
                    m_cache_PrimitiveType = child as ALittleScriptPrimitiveTypeElement;
                    break;
                }
            }
            return m_cache_PrimitiveType;
        }
        private bool m_flag_GenericType = false;
        private ALittleScriptGenericTypeElement m_cache_GenericType = null;
        public ALittleScriptGenericTypeElement GetGenericType()
        {
            if (m_flag_GenericType) return m_cache_GenericType;
            m_flag_GenericType = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptGenericTypeElement)
                {
                    m_cache_GenericType = child as ALittleScriptGenericTypeElement;
                    break;
                }
            }
            return m_cache_GenericType;
        }
        private bool m_flag_CustomType = false;
        private ALittleScriptCustomTypeElement m_cache_CustomType = null;
        public ALittleScriptCustomTypeElement GetCustomType()
        {
            if (m_flag_CustomType) return m_cache_CustomType;
            m_flag_CustomType = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptCustomTypeElement)
                {
                    m_cache_CustomType = child as ALittleScriptCustomTypeElement;
                    break;
                }
            }
            return m_cache_CustomType;
        }

	}
}