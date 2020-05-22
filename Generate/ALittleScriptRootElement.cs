
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptRootElement : ABnfNodeElement
	{
		public ALittleScriptRootElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_NamespaceDec = false;
        private ALittleScriptNamespaceDecElement m_cache_NamespaceDec = null;
        public ALittleScriptNamespaceDecElement GetNamespaceDec()
        {
            if (m_flag_NamespaceDec) return m_cache_NamespaceDec;
            m_flag_NamespaceDec = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptNamespaceDecElement)
                {
                    m_cache_NamespaceDec = child as ALittleScriptNamespaceDecElement;
                    break;
                }
            }
            return m_cache_NamespaceDec;
        }

	}
}