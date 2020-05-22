
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptDoWhileExprElement : ABnfNodeElement
	{
		public ALittleScriptDoWhileExprElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_DoWhileBody = false;
        private ALittleScriptDoWhileBodyElement m_cache_DoWhileBody = null;
        public ALittleScriptDoWhileBodyElement GetDoWhileBody()
        {
            if (m_flag_DoWhileBody) return m_cache_DoWhileBody;
            m_flag_DoWhileBody = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptDoWhileBodyElement)
                {
                    m_cache_DoWhileBody = child as ALittleScriptDoWhileBodyElement;
                    break;
                }
            }
            return m_cache_DoWhileBody;
        }
        private bool m_flag_DoWhileCondition = false;
        private ALittleScriptDoWhileConditionElement m_cache_DoWhileCondition = null;
        public ALittleScriptDoWhileConditionElement GetDoWhileCondition()
        {
            if (m_flag_DoWhileCondition) return m_cache_DoWhileCondition;
            m_flag_DoWhileCondition = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptDoWhileConditionElement)
                {
                    m_cache_DoWhileCondition = child as ALittleScriptDoWhileConditionElement;
                    break;
                }
            }
            return m_cache_DoWhileCondition;
        }
        List<ALittleScriptKeyElement> m_list_Key = null;
        public List<ALittleScriptKeyElement> GetKeyList()
        {
            var list = new List<ALittleScriptKeyElement>();
            if (m_list_Key == null)
            {
                m_list_Key = new List<ALittleScriptKeyElement>();
                foreach (var child in m_childs)
                {
                    if (child is ALittleScriptKeyElement)
                        m_list_Key.Add(child as ALittleScriptKeyElement);
                }   
            }
            list.AddRange(m_list_Key);
            return list;
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