
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptPropertyValueSuffixElement : ABnfNodeElement
	{
		public ALittleScriptPropertyValueSuffixElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_PropertyValueDotId = false;
        private ALittleScriptPropertyValueDotIdElement m_cache_PropertyValueDotId = null;
        public ALittleScriptPropertyValueDotIdElement GetPropertyValueDotId()
        {
            if (m_flag_PropertyValueDotId) return m_cache_PropertyValueDotId;
            m_flag_PropertyValueDotId = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptPropertyValueDotIdElement)
                {
                    m_cache_PropertyValueDotId = child as ALittleScriptPropertyValueDotIdElement;
                    break;
                }
            }
            return m_cache_PropertyValueDotId;
        }
        private bool m_flag_PropertyValueBracketValue = false;
        private ALittleScriptPropertyValueBracketValueElement m_cache_PropertyValueBracketValue = null;
        public ALittleScriptPropertyValueBracketValueElement GetPropertyValueBracketValue()
        {
            if (m_flag_PropertyValueBracketValue) return m_cache_PropertyValueBracketValue;
            m_flag_PropertyValueBracketValue = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptPropertyValueBracketValueElement)
                {
                    m_cache_PropertyValueBracketValue = child as ALittleScriptPropertyValueBracketValueElement;
                    break;
                }
            }
            return m_cache_PropertyValueBracketValue;
        }
        private bool m_flag_PropertyValueMethodCall = false;
        private ALittleScriptPropertyValueMethodCallElement m_cache_PropertyValueMethodCall = null;
        public ALittleScriptPropertyValueMethodCallElement GetPropertyValueMethodCall()
        {
            if (m_flag_PropertyValueMethodCall) return m_cache_PropertyValueMethodCall;
            m_flag_PropertyValueMethodCall = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptPropertyValueMethodCallElement)
                {
                    m_cache_PropertyValueMethodCall = child as ALittleScriptPropertyValueMethodCallElement;
                    break;
                }
            }
            return m_cache_PropertyValueMethodCall;
        }

	}
}