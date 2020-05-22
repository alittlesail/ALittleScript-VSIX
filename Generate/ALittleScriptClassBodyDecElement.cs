
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptClassBodyDecElement : ABnfNodeElement
	{
		public ALittleScriptClassBodyDecElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        List<ALittleScriptClassElementDecElement> m_list_ClassElementDec = null;
        public List<ALittleScriptClassElementDecElement> GetClassElementDecList()
        {
            var list = new List<ALittleScriptClassElementDecElement>();
            if (m_list_ClassElementDec == null)
            {
                m_list_ClassElementDec = new List<ALittleScriptClassElementDecElement>();
                foreach (var child in m_childs)
                {
                    if (child is ALittleScriptClassElementDecElement)
                        m_list_ClassElementDec.Add(child as ALittleScriptClassElementDecElement);
                }   
            }
            list.AddRange(m_list_ClassElementDec);
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