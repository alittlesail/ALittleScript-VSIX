
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptMethodReturnDecElement : ABnfNodeElement
	{
		public ALittleScriptMethodReturnDecElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        List<ALittleScriptMethodReturnOneDecElement> m_list_MethodReturnOneDec = null;
        public List<ALittleScriptMethodReturnOneDecElement> GetMethodReturnOneDecList()
        {
            var list = new List<ALittleScriptMethodReturnOneDecElement>();
            if (m_list_MethodReturnOneDec == null)
            {
                m_list_MethodReturnOneDec = new List<ALittleScriptMethodReturnOneDecElement>();
                foreach (var child in m_childs)
                {
                    if (child is ALittleScriptMethodReturnOneDecElement)
                        m_list_MethodReturnOneDec.Add(child as ALittleScriptMethodReturnOneDecElement);
                }   
            }
            list.AddRange(m_list_MethodReturnOneDec);
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