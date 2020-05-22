
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptNamespaceDecElement : ABnfNodeElement
	{
		public ALittleScriptNamespaceDecElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        List<ALittleScriptModifierElement> m_list_Modifier = null;
        public List<ALittleScriptModifierElement> GetModifierList()
        {
            var list = new List<ALittleScriptModifierElement>();
            if (m_list_Modifier == null)
            {
                m_list_Modifier = new List<ALittleScriptModifierElement>();
                foreach (var child in m_childs)
                {
                    if (child is ALittleScriptModifierElement)
                        m_list_Modifier.Add(child as ALittleScriptModifierElement);
                }   
            }
            list.AddRange(m_list_Modifier);
            return list;
        }
        private bool m_flag_NamespaceNameDec = false;
        private ALittleScriptNamespaceNameDecElement m_cache_NamespaceNameDec = null;
        public ALittleScriptNamespaceNameDecElement GetNamespaceNameDec()
        {
            if (m_flag_NamespaceNameDec) return m_cache_NamespaceNameDec;
            m_flag_NamespaceNameDec = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptNamespaceNameDecElement)
                {
                    m_cache_NamespaceNameDec = child as ALittleScriptNamespaceNameDecElement;
                    break;
                }
            }
            return m_cache_NamespaceNameDec;
        }
        List<ALittleScriptNamespaceElementDecElement> m_list_NamespaceElementDec = null;
        public List<ALittleScriptNamespaceElementDecElement> GetNamespaceElementDecList()
        {
            var list = new List<ALittleScriptNamespaceElementDecElement>();
            if (m_list_NamespaceElementDec == null)
            {
                m_list_NamespaceElementDec = new List<ALittleScriptNamespaceElementDecElement>();
                foreach (var child in m_childs)
                {
                    if (child is ALittleScriptNamespaceElementDecElement)
                        m_list_NamespaceElementDec.Add(child as ALittleScriptNamespaceElementDecElement);
                }   
            }
            list.AddRange(m_list_NamespaceElementDec);
            return list;
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
        private bool m_flag_String = false;
        private ALittleScriptStringElement m_cache_String = null;
        public ALittleScriptStringElement GetString()
        {
            if (m_flag_String) return m_cache_String;
            m_flag_String = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptStringElement)
                {
                    m_cache_String = child as ALittleScriptStringElement;
                    break;
                }
            }
            return m_cache_String;
        }

	}
}