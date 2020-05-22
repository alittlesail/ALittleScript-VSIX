
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptStructBodyDecElement : ABnfNodeElement
	{
		public ALittleScriptStructBodyDecElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        List<ALittleScriptStructOptionDecElement> m_list_StructOptionDec = null;
        public List<ALittleScriptStructOptionDecElement> GetStructOptionDecList()
        {
            var list = new List<ALittleScriptStructOptionDecElement>();
            if (m_list_StructOptionDec == null)
            {
                m_list_StructOptionDec = new List<ALittleScriptStructOptionDecElement>();
                foreach (var child in m_childs)
                {
                    if (child is ALittleScriptStructOptionDecElement)
                        m_list_StructOptionDec.Add(child as ALittleScriptStructOptionDecElement);
                }   
            }
            list.AddRange(m_list_StructOptionDec);
            return list;
        }
        List<ALittleScriptStructVarDecElement> m_list_StructVarDec = null;
        public List<ALittleScriptStructVarDecElement> GetStructVarDecList()
        {
            var list = new List<ALittleScriptStructVarDecElement>();
            if (m_list_StructVarDec == null)
            {
                m_list_StructVarDec = new List<ALittleScriptStructVarDecElement>();
                foreach (var child in m_childs)
                {
                    if (child is ALittleScriptStructVarDecElement)
                        m_list_StructVarDec.Add(child as ALittleScriptStructVarDecElement);
                }   
            }
            list.AddRange(m_list_StructVarDec);
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