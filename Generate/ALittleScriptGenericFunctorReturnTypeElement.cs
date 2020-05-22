
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System.Collections.Generic;

namespace ALittle
{
	public class ALittleScriptGenericFunctorReturnTypeElement : ABnfNodeElement
	{
		public ALittleScriptGenericFunctorReturnTypeElement(ABnfFactory factory, ABnfFile file, int line, int col, int offset, string type)
            : base(factory, file, line, col, offset, type)
        {
        }

        List<ALittleScriptGenericFunctorReturnOneTypeElement> m_list_GenericFunctorReturnOneType = null;
        public List<ALittleScriptGenericFunctorReturnOneTypeElement> GetGenericFunctorReturnOneTypeList()
        {
            var list = new List<ALittleScriptGenericFunctorReturnOneTypeElement>();
            if (m_list_GenericFunctorReturnOneType == null)
            {
                m_list_GenericFunctorReturnOneType = new List<ALittleScriptGenericFunctorReturnOneTypeElement>();
                foreach (var child in m_childs)
                {
                    if (child is ALittleScriptGenericFunctorReturnOneTypeElement)
                        m_list_GenericFunctorReturnOneType.Add(child as ALittleScriptGenericFunctorReturnOneTypeElement);
                }   
            }
            list.AddRange(m_list_GenericFunctorReturnOneType);
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