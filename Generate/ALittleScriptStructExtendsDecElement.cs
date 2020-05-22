
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptStructExtendsDecElement : ABnfNodeElement
	{
		public ALittleScriptStructExtendsDecElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
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
        private bool m_flag_StructNameDec = false;
        private ALittleScriptStructNameDecElement m_cache_StructNameDec = null;
        public ALittleScriptStructNameDecElement GetStructNameDec()
        {
            if (m_flag_StructNameDec) return m_cache_StructNameDec;
            m_flag_StructNameDec = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptStructNameDecElement)
                {
                    m_cache_StructNameDec = child as ALittleScriptStructNameDecElement;
                    break;
                }
            }
            return m_cache_StructNameDec;
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