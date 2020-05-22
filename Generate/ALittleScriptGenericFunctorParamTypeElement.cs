
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptGenericFunctorParamTypeElement : ABnfNodeElement
	{
		public ALittleScriptGenericFunctorParamTypeElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        List<ALittleScriptGenericFunctorParamOneTypeElement> m_list_GenericFunctorParamOneType = null;
        public List<ALittleScriptGenericFunctorParamOneTypeElement> GetGenericFunctorParamOneTypeList()
        {
            var list = new List<ALittleScriptGenericFunctorParamOneTypeElement>();
            if (m_list_GenericFunctorParamOneType == null)
            {
                m_list_GenericFunctorParamOneType = new List<ALittleScriptGenericFunctorParamOneTypeElement>();
                foreach (var child in m_childs)
                {
                    if (child is ALittleScriptGenericFunctorParamOneTypeElement)
                        m_list_GenericFunctorParamOneType.Add(child as ALittleScriptGenericFunctorParamOneTypeElement);
                }   
            }
            list.AddRange(m_list_GenericFunctorParamOneType);
            return list;
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