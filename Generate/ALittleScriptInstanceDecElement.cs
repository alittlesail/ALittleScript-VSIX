
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptInstanceDecElement : ABnfNodeElement
	{
		public ALittleScriptInstanceDecElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_VarAssignExpr = false;
        private ALittleScriptVarAssignExprElement m_cache_VarAssignExpr = null;
        public ALittleScriptVarAssignExprElement GetVarAssignExpr()
        {
            if (m_flag_VarAssignExpr) return m_cache_VarAssignExpr;
            m_flag_VarAssignExpr = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptVarAssignExprElement)
                {
                    m_cache_VarAssignExpr = child as ALittleScriptVarAssignExprElement;
                    break;
                }
            }
            return m_cache_VarAssignExpr;
        }

	}
}