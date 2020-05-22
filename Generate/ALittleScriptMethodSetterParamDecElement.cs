
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptMethodSetterParamDecElement : ABnfNodeElement
	{
		public ALittleScriptMethodSetterParamDecElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_MethodParamOneDec = false;
        private ALittleScriptMethodParamOneDecElement m_cache_MethodParamOneDec = null;
        public ALittleScriptMethodParamOneDecElement GetMethodParamOneDec()
        {
            if (m_flag_MethodParamOneDec) return m_cache_MethodParamOneDec;
            m_flag_MethodParamOneDec = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptMethodParamOneDecElement)
                {
                    m_cache_MethodParamOneDec = child as ALittleScriptMethodParamOneDecElement;
                    break;
                }
            }
            return m_cache_MethodParamOneDec;
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