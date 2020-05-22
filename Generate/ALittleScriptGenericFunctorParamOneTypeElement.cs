
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptGenericFunctorParamOneTypeElement : ABnfNodeElement
	{
		public ALittleScriptGenericFunctorParamOneTypeElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        private bool m_flag_GenericFunctorParamTail = false;
        private ALittleScriptGenericFunctorParamTailElement m_cache_GenericFunctorParamTail = null;
        public ALittleScriptGenericFunctorParamTailElement GetGenericFunctorParamTail()
        {
            if (m_flag_GenericFunctorParamTail) return m_cache_GenericFunctorParamTail;
            m_flag_GenericFunctorParamTail = true;
            foreach (var child in m_childs)
            {
                if (child is ALittleScriptGenericFunctorParamTailElement)
                {
                    m_cache_GenericFunctorParamTail = child as ALittleScriptGenericFunctorParamTailElement;
                    break;
                }
            }
            return m_cache_GenericFunctorParamTail;
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