
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptEnumBodyDecElement : ABnfNodeElement
	{
		public ALittleScriptEnumBodyDecElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        List<ALittleScriptEnumVarDecElement> m_list_EnumVarDec = null;
        public List<ALittleScriptEnumVarDecElement> GetEnumVarDecList()
        {
            var list = new List<ALittleScriptEnumVarDecElement>();
            if (m_list_EnumVarDec == null)
            {
                m_list_EnumVarDec = new List<ALittleScriptEnumVarDecElement>();
                foreach (var child in m_childs)
                {
                    if (child is ALittleScriptEnumVarDecElement)
                        m_list_EnumVarDec.Add(child as ALittleScriptEnumVarDecElement);
                }   
            }
            list.AddRange(m_list_EnumVarDec);
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