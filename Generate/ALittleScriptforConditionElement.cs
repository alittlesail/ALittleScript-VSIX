
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptForConditionElement : ABnfNodeElement
	{
		public ALittleScriptForConditionElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_ForPairDec = false;
        private ALittleScriptForPairDecElement m_cache_ForPairDec = null;
        public ALittleScriptForPairDecElement GetForPairDec()
        {
            if (m_flag_ForPairDec) return m_cache_ForPairDec;
            m_flag_ForPairDec = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptForPairDecElement)
                {
                    m_cache_ForPairDec = child as ALittleScriptForPairDecElement;
                    break;
                }
            }
            return m_cache_ForPairDec;
        }
        private bool m_flag_ForStepCondition = false;
        private ALittleScriptForStepConditionElement m_cache_ForStepCondition = null;
        public ALittleScriptForStepConditionElement GetForStepCondition()
        {
            if (m_flag_ForStepCondition) return m_cache_ForStepCondition;
            m_flag_ForStepCondition = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptForStepConditionElement)
                {
                    m_cache_ForStepCondition = child as ALittleScriptForStepConditionElement;
                    break;
                }
            }
            return m_cache_ForStepCondition;
        }
        private bool m_flag_ForInCondition = false;
        private ALittleScriptForInConditionElement m_cache_ForInCondition = null;
        public ALittleScriptForInConditionElement GetForInCondition()
        {
            if (m_flag_ForInCondition) return m_cache_ForInCondition;
            m_flag_ForInCondition = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptForInConditionElement)
                {
                    m_cache_ForInCondition = child as ALittleScriptForInConditionElement;
                    break;
                }
            }
            return m_cache_ForInCondition;
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