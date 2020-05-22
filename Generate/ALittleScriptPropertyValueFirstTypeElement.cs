
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptPropertyValueFirstTypeElement : ABnfNodeElement
	{
		public ALittleScriptPropertyValueFirstTypeElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_PropertyValueThisType = false;
        private ALittleScriptPropertyValueThisTypeElement m_cache_PropertyValueThisType = null;
        public ALittleScriptPropertyValueThisTypeElement GetPropertyValueThisType()
        {
            if (m_flag_PropertyValueThisType) return m_cache_PropertyValueThisType;
            m_flag_PropertyValueThisType = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptPropertyValueThisTypeElement)
                {
                    m_cache_PropertyValueThisType = child as ALittleScriptPropertyValueThisTypeElement;
                    break;
                }
            }
            return m_cache_PropertyValueThisType;
        }
        private bool m_flag_PropertyValueCastType = false;
        private ALittleScriptPropertyValueCastTypeElement m_cache_PropertyValueCastType = null;
        public ALittleScriptPropertyValueCastTypeElement GetPropertyValueCastType()
        {
            if (m_flag_PropertyValueCastType) return m_cache_PropertyValueCastType;
            m_flag_PropertyValueCastType = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptPropertyValueCastTypeElement)
                {
                    m_cache_PropertyValueCastType = child as ALittleScriptPropertyValueCastTypeElement;
                    break;
                }
            }
            return m_cache_PropertyValueCastType;
        }
        private bool m_flag_PropertyValueCustomType = false;
        private ALittleScriptPropertyValueCustomTypeElement m_cache_PropertyValueCustomType = null;
        public ALittleScriptPropertyValueCustomTypeElement GetPropertyValueCustomType()
        {
            if (m_flag_PropertyValueCustomType) return m_cache_PropertyValueCustomType;
            m_flag_PropertyValueCustomType = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptPropertyValueCustomTypeElement)
                {
                    m_cache_PropertyValueCustomType = child as ALittleScriptPropertyValueCustomTypeElement;
                    break;
                }
            }
            return m_cache_PropertyValueCustomType;
        }

	}
}