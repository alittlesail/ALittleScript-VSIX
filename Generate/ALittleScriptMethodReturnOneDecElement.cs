
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptMethodReturnOneDecElement : ABnfNodeElement
	{
		public ALittleScriptMethodReturnOneDecElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_MethodReturnTailDec = false;
        private ALittleScriptMethodReturnTailDecElement m_cache_MethodReturnTailDec = null;
        public ALittleScriptMethodReturnTailDecElement GetMethodReturnTailDec()
        {
            if (m_flag_MethodReturnTailDec) return m_cache_MethodReturnTailDec;
            m_flag_MethodReturnTailDec = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptMethodReturnTailDecElement)
                {
                    m_cache_MethodReturnTailDec = child as ALittleScriptMethodReturnTailDecElement;
                    break;
                }
            }
            return m_cache_MethodReturnTailDec;
        }
        private bool m_flag_AllType = false;
        private ALittleScriptAllTypeElement m_cache_AllType = null;
        public ALittleScriptAllTypeElement GetAllType()
        {
            if (m_flag_AllType) return m_cache_AllType;
            m_flag_AllType = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptAllTypeElement)
                {
                    m_cache_AllType = child as ALittleScriptAllTypeElement;
                    break;
                }
            }
            return m_cache_AllType;
        }

	}
}