
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptElseBodyElement : ABnfNodeElement
	{
		public ALittleScriptElseBodyElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        List<ALittleScriptAllExprElement> m_list_AllExpr = null;
        public List<ALittleScriptAllExprElement> GetAllExprList()
        {
            var list = new List<ALittleScriptAllExprElement>();
            if (m_list_AllExpr == null)
            {
                m_list_AllExpr = new List<ALittleScriptAllExprElement>();
                foreach (var child in m_childs)
                {
                    if (child is ALittleScriptAllExprElement)
                        m_list_AllExpr.Add(child as ALittleScriptAllExprElement);
                }   
            }
            list.AddRange(m_list_AllExpr);
            return list;
        }
        List<ALittleScriptStringElement> m_list_String = null;
        public List<ALittleScriptStringElement> GetStringList()
        {
            var list = new List<ALittleScriptStringElement>();
            if (m_list_String == null)
            {
                m_list_String = new List<ALittleScriptStringElement>();
                foreach (var child in m_childs)
                {
                    if (child is ALittleScriptStringElement)
                        m_list_String.Add(child as ALittleScriptStringElement);
                }   
            }
            list.AddRange(m_list_String);
            return list;
        }

	}
}