
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptOp2StatElement : ABnfNodeElement
	{
		public ALittleScriptOp2StatElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_Op2Value = false;
        private ALittleScriptOp2ValueElement m_cache_Op2Value = null;
        public ALittleScriptOp2ValueElement GetOp2Value()
        {
            if (m_flag_Op2Value) return m_cache_Op2Value;
            m_flag_Op2Value = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptOp2ValueElement)
                {
                    m_cache_Op2Value = child as ALittleScriptOp2ValueElement;
                    break;
                }
            }
            return m_cache_Op2Value;
        }
        List<ALittleScriptOp2SuffixExElement> m_list_Op2SuffixEx = null;
        public List<ALittleScriptOp2SuffixExElement> GetOp2SuffixExList()
        {
            var list = new List<ALittleScriptOp2SuffixExElement>();
            if (m_list_Op2SuffixEx == null)
            {
                m_list_Op2SuffixEx = new List<ALittleScriptOp2SuffixExElement>();
                foreach (var child in m_childs)
                {
                    if (child is ALittleScriptOp2SuffixExElement)
                        m_list_Op2SuffixEx.Add(child as ALittleScriptOp2SuffixExElement);
                }   
            }
            list.AddRange(m_list_Op2SuffixEx);
            return list;
        }

	}
}