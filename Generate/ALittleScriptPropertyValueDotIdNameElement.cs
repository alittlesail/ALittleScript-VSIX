
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptPropertyValueDotIdNameElement : ABnfNodeElement
	{
		public ALittleScriptPropertyValueDotIdNameElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_Id = false;
        private ALittleScriptIdElement m_cache_Id = null;
        public ALittleScriptIdElement GetId()
        {
            if (m_flag_Id) return m_cache_Id;
            m_flag_Id = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptIdElement)
                {
                    m_cache_Id = child as ALittleScriptIdElement;
                    break;
                }
            }
            return m_cache_Id;
        }

	}
}