
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptCustomTypeElement : ABnfNodeElement
	{
		public ALittleScriptCustomTypeElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_CustomTypeName = false;
        private ALittleScriptCustomTypeNameElement m_cache_CustomTypeName = null;
        public ALittleScriptCustomTypeNameElement GetCustomTypeName()
        {
            if (m_flag_CustomTypeName) return m_cache_CustomTypeName;
            m_flag_CustomTypeName = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptCustomTypeNameElement)
                {
                    m_cache_CustomTypeName = child as ALittleScriptCustomTypeNameElement;
                    break;
                }
            }
            return m_cache_CustomTypeName;
        }
        private bool m_flag_CustomTypeDotId = false;
        private ALittleScriptCustomTypeDotIdElement m_cache_CustomTypeDotId = null;
        public ALittleScriptCustomTypeDotIdElement GetCustomTypeDotId()
        {
            if (m_flag_CustomTypeDotId) return m_cache_CustomTypeDotId;
            m_flag_CustomTypeDotId = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptCustomTypeDotIdElement)
                {
                    m_cache_CustomTypeDotId = child as ALittleScriptCustomTypeDotIdElement;
                    break;
                }
            }
            return m_cache_CustomTypeDotId;
        }
        private bool m_flag_CustomTypeTemplate = false;
        private ALittleScriptCustomTypeTemplateElement m_cache_CustomTypeTemplate = null;
        public ALittleScriptCustomTypeTemplateElement GetCustomTypeTemplate()
        {
            if (m_flag_CustomTypeTemplate) return m_cache_CustomTypeTemplate;
            m_flag_CustomTypeTemplate = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptCustomTypeTemplateElement)
                {
                    m_cache_CustomTypeTemplate = child as ALittleScriptCustomTypeTemplateElement;
                    break;
                }
            }
            return m_cache_CustomTypeTemplate;
        }

	}
}