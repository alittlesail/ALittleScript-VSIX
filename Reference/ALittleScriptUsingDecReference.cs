
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptUsingDecReference : ALittleScriptReferenceTemplate<ALittleScriptUsingDecElement>
    {
        private string m_namespace_name;

        public ALittleScriptUsingDecReference(ABnfElement element) : base(element)
        {
            m_namespace_name = ALittleScriptUtility.GetNamespaceName(m_element);
        }

        public override ABnfGuessError GuessTypes(out List<ABnfGuess> guess_list)
        {
            guess_list = new List<ABnfGuess>();
            var name_dec = m_element.GetUsingNameDec();
            if (name_dec == null) return new ABnfGuessError(m_element, "没有定义using的名称");

            if (m_element.GetAllType() != null)
            {
                var error = m_element.GetAllType().GuessTypes(out guess_list);
                if (error != null) return error;

                bool has_template = false;
                foreach (var guess in guess_list)
                {
                    if (guess is ALittleScriptGuessClass)
                    {
                        var guess_class = guess as ALittleScriptGuessClass;
                        if (guess_class.template_list.Count > 0)
                        {
                            has_template = true;
                            break;
                        }
                    }
                }
                if (!has_template) return null;

                var new_guess_list = new List<ABnfGuess>();
                foreach (var guess in guess_list)
                {
                    if (guess is ALittleScriptGuessClass)
                    {
                        var guess_class = guess as ALittleScriptGuessClass;
                        if (guess_class.template_list.Count == 0)
                        {
                            new_guess_list.Add(guess);
                        }
                        else
                        {
                            guess_class = guess_class.Clone() as ALittleScriptGuessClass;
                            guess_class.using_name = m_namespace_name + "." + name_dec.GetElementText();
                            new_guess_list.Add(guess_class);
                        }
                    }
                    else
                    {
                        new_guess_list.Add(guess);
                    }
                }

                guess_list = new_guess_list;
            }
            return null;
        }
    }
}

