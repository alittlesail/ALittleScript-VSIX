
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptEnumVarDecElement : ABnfNodeElement
	{
		public ALittleScriptEnumVarDecElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_EnumVarNameDec = false;
        private ALittleScriptEnumVarNameDecElement m_cache_EnumVarNameDec = null;
        public ALittleScriptEnumVarNameDecElement GetEnumVarNameDec()
        {
            if (m_flag_EnumVarNameDec) return m_cache_EnumVarNameDec;
            m_flag_EnumVarNameDec = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptEnumVarNameDecElement)
                {
                    m_cache_EnumVarNameDec = child as ALittleScriptEnumVarNameDecElement;
                    break;
                }
            }
            return m_cache_EnumVarNameDec;
        }
        private bool m_flag_Number = false;
        private ALittleScriptNumberElement m_cache_Number = null;
        public ALittleScriptNumberElement GetNumber()
        {
            if (m_flag_Number) return m_cache_Number;
            m_flag_Number = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptNumberElement)
                {
                    m_cache_Number = child as ALittleScriptNumberElement;
                    break;
                }
            }
            return m_cache_Number;
        }
        private bool m_flag_Text = false;
        private ALittleScriptTextElement m_cache_Text = null;
        public ALittleScriptTextElement GetText()
        {
            if (m_flag_Text) return m_cache_Text;
            m_flag_Text = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptTextElement)
                {
                    m_cache_Text = child as ALittleScriptTextElement;
                    break;
                }
            }
            return m_cache_Text;
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