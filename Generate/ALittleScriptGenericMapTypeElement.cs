
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptGenericMapTypeElement : ABnfNodeElement
	{
		public ALittleScriptGenericMapTypeElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        List<ALittleScriptAllTypeElement> m_list_AllType = null;
        public List<ALittleScriptAllTypeElement> GetAllTypeList()
        {
            var list = new List<ALittleScriptAllTypeElement>();
            if (m_list_AllType == null)
            {
                m_list_AllType = new List<ALittleScriptAllTypeElement>();
                foreach (var child in m_childs)
                {
                    if (child is ALittleScriptAllTypeElement)
                        m_list_AllType.Add(child as ALittleScriptAllTypeElement);
                }   
            }
            list.AddRange(m_list_AllType);
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