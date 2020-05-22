
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptGenericFunctorReturnOneTypeElement : ABnfNodeElement
	{
		public ALittleScriptGenericFunctorReturnOneTypeElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_GenericFunctorReturnTail = false;
        private ALittleScriptGenericFunctorReturnTailElement m_cache_GenericFunctorReturnTail = null;
        public ALittleScriptGenericFunctorReturnTailElement GetGenericFunctorReturnTail()
        {
            if (m_flag_GenericFunctorReturnTail) return m_cache_GenericFunctorReturnTail;
            m_flag_GenericFunctorReturnTail = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptGenericFunctorReturnTailElement)
                {
                    m_cache_GenericFunctorReturnTail = child as ALittleScriptGenericFunctorReturnTailElement;
                    break;
                }
            }
            return m_cache_GenericFunctorReturnTail;
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