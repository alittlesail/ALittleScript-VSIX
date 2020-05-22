
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptPropertyValueElement : ABnfNodeElement
	{
		public ALittleScriptPropertyValueElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_PropertyValueFirstType = false;
        private ALittleScriptPropertyValueFirstTypeElement m_cache_PropertyValueFirstType = null;
        public ALittleScriptPropertyValueFirstTypeElement GetPropertyValueFirstType()
        {
            if (m_flag_PropertyValueFirstType) return m_cache_PropertyValueFirstType;
            m_flag_PropertyValueFirstType = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptPropertyValueFirstTypeElement)
                {
                    m_cache_PropertyValueFirstType = child as ALittleScriptPropertyValueFirstTypeElement;
                    break;
                }
            }
            return m_cache_PropertyValueFirstType;
        }
        List<ALittleScriptPropertyValueSuffixElement> m_list_PropertyValueSuffix = null;
        public List<ALittleScriptPropertyValueSuffixElement> GetPropertyValueSuffixList()
        {
            var list = new List<ALittleScriptPropertyValueSuffixElement>();
            if (m_list_PropertyValueSuffix == null)
            {
                m_list_PropertyValueSuffix = new List<ALittleScriptPropertyValueSuffixElement>();
                foreach (var child in m_childs)
                {
                    if (child is ALittleScriptPropertyValueSuffixElement)
                        m_list_PropertyValueSuffix.Add(child as ALittleScriptPropertyValueSuffixElement);
                }   
            }
            list.AddRange(m_list_PropertyValueSuffix);
            return list;
        }

	}
}