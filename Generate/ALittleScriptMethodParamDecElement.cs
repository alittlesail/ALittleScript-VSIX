
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptMethodParamDecElement : ABnfNodeElement
	{
		public ALittleScriptMethodParamDecElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        List<ALittleScriptMethodParamOneDecElement> m_list_MethodParamOneDec = null;
        public List<ALittleScriptMethodParamOneDecElement> GetMethodParamOneDecList()
        {
            var list = new List<ALittleScriptMethodParamOneDecElement>();
            if (m_list_MethodParamOneDec == null)
            {
                m_list_MethodParamOneDec = new List<ALittleScriptMethodParamOneDecElement>();
                foreach (var child in m_childs)
                {
                    if (child is ALittleScriptMethodParamOneDecElement)
                        m_list_MethodParamOneDec.Add(child as ALittleScriptMethodParamOneDecElement);
                }   
            }
            list.AddRange(m_list_MethodParamOneDec);
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