
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptOp5StatElement : ABnfNodeElement
	{
		public ALittleScriptOp5StatElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_Op5Suffix = false;
        private ALittleScriptOp5SuffixElement m_cache_Op5Suffix = null;
        public ALittleScriptOp5SuffixElement GetOp5Suffix()
        {
            if (m_flag_Op5Suffix) return m_cache_Op5Suffix;
            m_flag_Op5Suffix = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptOp5SuffixElement)
                {
                    m_cache_Op5Suffix = child as ALittleScriptOp5SuffixElement;
                    break;
                }
            }
            return m_cache_Op5Suffix;
        }
        List<ALittleScriptOp5SuffixExElement> m_list_Op5SuffixEx = null;
        public List<ALittleScriptOp5SuffixExElement> GetOp5SuffixExList()
        {
            var list = new List<ALittleScriptOp5SuffixExElement>();
            if (m_list_Op5SuffixEx == null)
            {
                m_list_Op5SuffixEx = new List<ALittleScriptOp5SuffixExElement>();
                foreach (var child in m_childs)
                {
                    if (child is ALittleScriptOp5SuffixExElement)
                        m_list_Op5SuffixEx.Add(child as ALittleScriptOp5SuffixExElement);
                }   
            }
            list.AddRange(m_list_Op5SuffixEx);
            return list;
        }

	}
}