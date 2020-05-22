
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptCustomTypeDotIdElement : ABnfNodeElement
	{
		public ALittleScriptCustomTypeDotIdElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_CustomTypeDotIdName = false;
        private ALittleScriptCustomTypeDotIdNameElement m_cache_CustomTypeDotIdName = null;
        public ALittleScriptCustomTypeDotIdNameElement GetCustomTypeDotIdName()
        {
            if (m_flag_CustomTypeDotIdName) return m_cache_CustomTypeDotIdName;
            m_flag_CustomTypeDotIdName = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptCustomTypeDotIdNameElement)
                {
                    m_cache_CustomTypeDotIdName = child as ALittleScriptCustomTypeDotIdNameElement;
                    break;
                }
            }
            return m_cache_CustomTypeDotIdName;
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