
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptStructOptionDecElement : ABnfNodeElement
	{
		public ALittleScriptStructOptionDecElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_StructOptionNameDec = false;
        private ALittleScriptStructOptionNameDecElement m_cache_StructOptionNameDec = null;
        public ALittleScriptStructOptionNameDecElement GetStructOptionNameDec()
        {
            if (m_flag_StructOptionNameDec) return m_cache_StructOptionNameDec;
            m_flag_StructOptionNameDec = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptStructOptionNameDecElement)
                {
                    m_cache_StructOptionNameDec = child as ALittleScriptStructOptionNameDecElement;
                    break;
                }
            }
            return m_cache_StructOptionNameDec;
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
        private bool m_flag_Key = false;
        private ALittleScriptKeyElement m_cache_Key = null;
        public ALittleScriptKeyElement GetKey()
        {
            if (m_flag_Key) return m_cache_Key;
            m_flag_Key = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptKeyElement)
                {
                    m_cache_Key = child as ALittleScriptKeyElement;
                    break;
                }
            }
            return m_cache_Key;
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