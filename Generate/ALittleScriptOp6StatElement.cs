
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptOp6StatElement : ABnfNodeElement
	{
		public ALittleScriptOp6StatElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_Op6Suffix = false;
        private ALittleScriptOp6SuffixElement m_cache_Op6Suffix = null;
        public ALittleScriptOp6SuffixElement GetOp6Suffix()
        {
            if (m_flag_Op6Suffix) return m_cache_Op6Suffix;
            m_flag_Op6Suffix = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptOp6SuffixElement)
                {
                    m_cache_Op6Suffix = child as ALittleScriptOp6SuffixElement;
                    break;
                }
            }
            return m_cache_Op6Suffix;
        }
        List<ALittleScriptOp6SuffixExElement> m_list_Op6SuffixEx = null;
        public List<ALittleScriptOp6SuffixExElement> GetOp6SuffixExList()
        {
            var list = new List<ALittleScriptOp6SuffixExElement>();
            if (m_list_Op6SuffixEx == null)
            {
                m_list_Op6SuffixEx = new List<ALittleScriptOp6SuffixExElement>();
                foreach (var child in m_childs)
                {
                    if (child is ALittleScriptOp6SuffixExElement)
                        m_list_Op6SuffixEx.Add(child as ALittleScriptOp6SuffixExElement);
                }   
            }
            list.AddRange(m_list_Op6SuffixEx);
            return list;
        }

	}
}