
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptGenericTypeReference : ALittleScriptReferenceTemplate<ALittleScriptGenericTypeElement>
    {
        public ALittleScriptGenericTypeReference(ABnfElement element) : base(element)
        {

        }

        public override ABnfGuessError GuessTypes(out List<ABnfGuess> guess_list)
        {
            guess_list = new List<ABnfGuess>();

            // 处理List
            if (m_element.GetGenericListType() != null)
            {
                var dec = m_element.GetGenericListType();
                var all_type = dec.GetAllType();
                if (all_type == null) return null;

                var error = all_type.GuessType(out ABnfGuess guess);
                if (error != null) return error;

                var info = new ALittleScriptGuessList(guess, false, false);
                info.UpdateValue();
                guess_list.Add(info);
            }
            // 处理Map
            else if (m_element.GetGenericMapType() != null)
            {
                var dec = m_element.GetGenericMapType();
                var all_type_list = dec.GetAllTypeList();
                if (all_type_list.Count != 2) return null;

                var error = all_type_list[0].GuessType(out ABnfGuess key_guess);
                if (error != null) return error;
                error = all_type_list[1].GuessType(out ABnfGuess value_guess);
                if (error != null) return error;

                var info = new ALittleScriptGuessMap(key_guess, value_guess, false);
                info.UpdateValue();
                guess_list.Add(info);
            }
            // 处理函数
            else if (m_element.GetGenericFunctorType() != null)
            {
                var dec = m_element.GetGenericFunctorType();
                if (dec != null)
                {
                    var info = new ALittleScriptGuessFunctor(m_element);
                    // 处理是不是const
                    info.const_modifier = dec.GetAllTypeConst() != null;
                    // 处理是不是await
                    info.await_modifier = (dec.GetCoroutineModifier() != null && dec.GetCoroutineModifier().GetElementText() == "await");
                    
                    // 处理参数
                    var param_type = dec.GetGenericFunctorParamType();
                    if (param_type != null)
                    {
                        var param_one_list = param_type.GetGenericFunctorParamOneTypeList();
                        for (int i = 0; i < param_one_list.Count; ++i)
                        {
                            var param_one = param_one_list[i];
                            var all_type = param_one.GetAllType();
                            if (all_type != null)
                            {
                                var error = all_type.GuessType(out ABnfGuess guess);
                                if (error != null) return error;
                                info.param_list.Add(guess);
                                info.param_nullable_list.Add(false);
                                info.param_name_list.Add(guess.GetValue());
                            }
                            else
                            {
                                var param_tail = param_one.GetGenericFunctorParamTail();
                                if (param_tail == null)
                                    return new ABnfGuessError(param_one, "未知类型");
                                if (i + 1 != param_one_list.Count)
                                    return new ABnfGuessError(param_one, "参数占位符必须定义在最后");
                                info.param_tail = new ALittleScriptGuessParamTail(param_tail.GetElementText());
                            }
                        }
                    }

                    // 处理返回值
                    var return_type = dec.GetGenericFunctorReturnType();
                    if (return_type != null)
                    {
                        var return_one_list = return_type.GetGenericFunctorReturnOneTypeList();
                        for (int i = 0; i < return_one_list.Count; ++i)
                        {
                            var return_one = return_one_list[i];
                            var all_type = return_one.GetAllType();
                            if (all_type != null)
                            {
                                var error = all_type.GuessType(out ABnfGuess guess);
                                if (error != null) return error;
                                info.return_list.Add(guess);
                            }
                            else
                            {
                                var return_tail = return_one.GetGenericFunctorReturnTail();
                                if (return_tail == null)
                                    return new ABnfGuessError(return_one, "未知类型");
                                if (i + 1 != return_one_list.Count)
                                    return new ABnfGuessError(return_one, "返回值占位符必须定义在最后");
                                info.param_tail = new ALittleScriptGuessParamTail(return_tail.GetElementText());
                            }
                        }
                    }
                    info.UpdateValue();
                    guess_list.Add(info);
                }
            }

            return null;
        }
    }
}

