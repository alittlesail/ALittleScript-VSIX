
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptGlobalMethodDecReference : ALittleScriptReferenceTemplate<ALittleScriptGlobalMethodDecElement>
    {
        public ALittleScriptGlobalMethodDecReference(ABnfElement element) : base(element)
        {

        }

        private ABnfGuessError CheckCmdError()
        {
            var parent = m_element.GetParent() as ALittleScriptNamespaceElementDecElement;
            string command_type = ALittleScriptUtility.GetCommandDetail(parent.GetModifierList(), out string desc);
            if (command_type == null) return null;

            if (m_element.GetTemplateDec() != null)
                return new ABnfGuessError(m_element, "带Cmd的全局函数，不能使用模板");
            return null;
        }

        private ABnfGuessError CheckProtoError()
        {
            var parent = m_element.GetParent() as ALittleScriptNamespaceElementDecElement;
            var proto_type = ALittleScriptUtility.GetProtocolType(parent.GetModifierList());
            if (proto_type == null) return null;

            var co_text = ALittleScriptUtility.GetCoroutineType(parent.GetModifierList());

            var param_dec = m_element.GetMethodParamDec();
            var return_dec = m_element.GetMethodReturnDec();

            string text = proto_type;

            if (m_element.GetTemplateDec() != null)
                return new ABnfGuessError(m_element, "带" + text + "的全局函数，不能使用模板");

            if (param_dec == null) return new ABnfGuessError(m_element, "带" + text + "的全局函数，必须有参数");
            var return_list = new List<ALittleScriptAllTypeElement>();
            if (return_dec != null)
            {
                var return_one_list = return_dec.GetMethodReturnOneDecList();
                foreach (var return_one in return_one_list)
                {
                    if (return_one.GetMethodReturnTailDec() != null)
                        return new ABnfGuessError(m_element, "带" + text + "的全局函数，不能使用返回值占位符");
                    var all_type = return_one.GetAllType();
                    if (all_type == null)
                        return new ABnfGuessError(m_element, "带" + text + "的全局函数，返回值没有定义类型");
                    return_list.Add(all_type);
                }
            }
            
            var param_one_dec_list = param_dec.GetMethodParamOneDecList();
            var param_guess_list = new List<ABnfGuess>();
            foreach (var param_one_dec in param_one_dec_list)
            {
                if (param_one_dec.GetMethodParamTailDec() != null)
                    return new ABnfGuessError(m_element, "带" + text + "的全局函数，不能使用参数占位符");
                var all_type = param_one_dec.GetAllType();
                if (all_type == null)
                    return new ABnfGuessError(m_element, "带" + text + "的全局函数，参数没有定义类型");
                var guess_error = all_type.GuessType(out ABnfGuess all_type_guess);
                if (guess_error != null) return guess_error;
                param_guess_list.Add(all_type_guess);
            }

            var return_guess_list = new List<ABnfGuess>();
            foreach (var all_type in return_list)
            {
                var guess_error = all_type.GuessType(out ABnfGuess all_type_guess);
                if (guess_error != null) return guess_error;
                return_guess_list.Add(all_type_guess);
            }

            if (co_text == "async" && return_guess_list.Count > 0)
                return new ABnfGuessError(return_dec, "带async修饰的函数，不能有返回值");

            // 检查参数个数
            if (param_guess_list.Count != 2) return new ABnfGuessError(param_dec, "带" + text + "的全局函数，必须有两个参数");

            // 第二个参数
            if (!(param_guess_list[1] is ALittleScriptGuessStruct))
                return new ABnfGuessError(m_element, "带" + text + "的全局函数，第二个参数必须是struct");
            ABnfGuessError error = null;
            if (text == "Http")
            {
                if (co_text != "await")
                    return new ABnfGuessError(m_element, "带" + text + "的全局函数，必须使用await修饰");

                if (return_guess_list.Count != 1) return new ABnfGuessError(m_element, "带" + text + "的全局函数，必须有一个返回值");
                // 第一个参数
                if (param_guess_list[0].GetValue() != "ALittle.IHttpReceiver")
                    return new ABnfGuessError(param_one_dec_list[0], "带" + text + "的全局函数，第一个参数必须是ALittle.IHttpReceiver");

                // 返回值
                if (!(return_guess_list[0] is ALittleScriptGuessStruct))
                    return new ABnfGuessError(m_element, "带" + text + "的全局函数，返回值必须是struct");

                error = CheckJsonStruct(param_one_dec_list[1], param_guess_list[1], new HashSet<string>());
                if (error != null) return error;

                error = CheckJsonStruct(return_list[0], return_guess_list[0], new HashSet<string>());
                if (error != null) return error;
            }
            else if (text == "HttpDownload")
            {
                if (co_text != "await")
                    return new ABnfGuessError(m_element, "带" + text + "的全局函数，必须使用await修饰");

                if (return_guess_list.Count != 2) return new ABnfGuessError(m_element, "带" + text + "的全局函数，必须有两个返回值，一个是string，一个是int");
                if (return_guess_list[0].GetValue() != "string") return new ABnfGuessError(m_element, "带" + text + "的全局函数，第一个参数必须是string");
                if (return_guess_list[1].GetValue() != "int") return new ABnfGuessError(m_element, "带" + text + "的全局函数，第二个参数必须是int");
                // 第一个参数
                if (param_guess_list[0].GetValue() != "ALittle.IHttpReceiver")
                    return new ABnfGuessError(param_one_dec_list[0], "带" + text + "的全局函数，第一个参数必须是ALittle.IHttpReceiver");

                error = CheckJsonStruct(param_one_dec_list[1], param_guess_list[1], new HashSet<string>());
                if (error != null) return error;
            }
            else if (text == "HttpUpload")
            {
                if (co_text != "await")
                    return new ABnfGuessError(m_element, "带" + text + "的全局函数，必须使用await修饰");

                // 第一个参数
                if (param_guess_list[0].GetValue() != "ALittle.IHttpFileReceiver")
                    return new ABnfGuessError(param_one_dec_list[0], "带" + text + "的全局函数，第一个参数必须是ALittle.IHttpFileReceiver");

                error = CheckJsonStruct(param_one_dec_list[1], param_guess_list[1], new HashSet<string>());
                if (error != null) return error;

                // 返回值
                if (return_guess_list.Count != 0)
                    return new ABnfGuessError(m_element, "带" + text + "的全局函数，不能有返回值");
            }
            else if (text == "Msg")
            {
                if (return_guess_list.Count > 1) return new ABnfGuessError(m_element, "带" + text + "的全局函数，最多只能有一个返回值");
                // 第一个参数
                if (param_guess_list[0].GetValue() != "ALittle.IMsgCommon")
                    return new ABnfGuessError(param_one_dec_list[0], "带" + text + "的全局函数，第一个参数必须是ALittle.IMsgCommon");

                error = CheckMsgStruct(param_one_dec_list[1], param_guess_list[1], new HashSet<string>());
                if (error != null) return error;

                // 返回值
                if (return_guess_list.Count > 0)
                {
                    if (co_text != "await")
                        return new ABnfGuessError(m_element, "带" + text + "的全局函数，并且有返回值，必须使用await修饰");

                    if (!(return_guess_list[0] is ALittleScriptGuessStruct))
                        return new ABnfGuessError(m_element, "带" + text + "的全局函数，返回值必须是struct");
                    error = CheckMsgStruct(return_list[0], return_guess_list[0], new HashSet<string>());
                    if (error != null) return error;
                }
                else
                {
                    // 如果没有返回值，那么不能使用await，只能使用async，或者不使用
                    if (co_text == "await")
                        return new ABnfGuessError(m_element, "带" + text + "的全局函数，当没有返回值时，不能使用await，可以使用async");
                }
            }
            return null;
        }

        private ABnfGuessError CheckMsgStruct(ABnfElement element, ABnfGuess guess, HashSet<string> map)
        {
            if (guess is ALittleScriptGuessList)
            {
                var guess_list = guess as ALittleScriptGuessList;
                var error = CheckMsgStruct(element, guess_list.sub_type, map);
                if (error != null) return error;
            }
            else if (guess is ALittleScriptGuessMap)
            {
                var guess_map = guess as ALittleScriptGuessMap;
                if (!(guess_map.key_type is ALittleScriptGuessString)
                    && !(guess_map.key_type is ALittleScriptGuessInt)
                    && !(guess_map.key_type is ALittleScriptGuessLong))
                    return new ABnfGuessError(element, "Msg协议接口的参数使用二进制序列化，内部使用的Map的key必须是string,int,long类型");
                var error = CheckMsgStruct(element, guess_map.value_type, map);
                if (error != null) return error;
            }
            else if (guess is ALittleScriptGuessStruct)
            {
                var guess_struct = guess as ALittleScriptGuessStruct;
                // 如果有继承，那么就检查一下继承
                if (guess_struct.struct_dec.GetStructExtendsDec() != null)
                {
                    var extends_name = guess_struct.struct_dec.GetStructExtendsDec().GetStructNameDec();
                    if (extends_name != null)
                    {
                        var extends_error = extends_name.GuessType(out ABnfGuess extends_guess);
                        if (extends_error != null) return extends_error;
                        var extends_struct_guess = extends_guess as ALittleScriptGuessStruct;
                        extends_error = CheckMsgStruct(element, extends_struct_guess, map);
                        if (extends_error != null) return extends_error;
                    }
                }

                if (guess_struct.GetValueWithoutConst() == "ALittle.ProtocolAnyStruct") return null;

                // 如果已经识别了，那么就直接返回
                if (map.Contains(guess_struct.GetValueWithoutConst())) return null;
                map.Add(guess_struct.GetValueWithoutConst());

                var body_dec = guess_struct.struct_dec.GetStructBodyDec();
                if (body_dec == null)
                    return new ABnfGuessError(element, "struct不完整");

                var var_dec_list = body_dec.GetStructVarDecList();
                foreach (var var_dec in var_dec_list)
                {
                    var error = var_dec.GuessType(out guess);
                    if (error != null) return error;
                    error = CheckMsgStruct(element, guess, map);
                    if (error != null) return error;
                }
            }
            else if (guess is ALittleScriptGuessClass)
            {
                return new ABnfGuessError(element, "Msg协议接口的参数使用二进制序列化，内部不能使用类");
            }
            else if (guess is ALittleScriptGuessFunctor)
            {
                return new ABnfGuessError(element, "Msg协议接口的参数使用二进制序列化，内部不能使用函数");
            }
            else if (guess.HasAny())
            {
                return new ABnfGuessError(element, "Msg协议接口的参数使用二进制序列化，内部不能使用any");
            }

            return null;
        }

        private ABnfGuessError CheckJsonStruct(ABnfElement element, ABnfGuess guess, HashSet<string> map)
        {
            if (guess is ALittleScriptGuessList)
            {
                var guess_list = guess as ALittleScriptGuessList;
                var error = CheckJsonStruct(element, guess_list.sub_type, map);
                if (error != null) return error;
            }
            else if (guess is ALittleScriptGuessMap)
            {
                var guess_map = guess as ALittleScriptGuessMap;
                if (!(guess_map.key_type is ALittleScriptGuessString))
                    return new ABnfGuessError(element, "http协议接口的参数使用json序列化，内部使用的Map的key必须是string类型");
                var error = CheckJsonStruct(element, guess_map.value_type, map);
                if (error != null) return error;
            }
            else if (guess is ALittleScriptGuessStruct)
            {
                var guess_struct = guess as ALittleScriptGuessStruct;
                // 如果有继承，那么就检查一下继承
                if (guess_struct.struct_dec.GetStructExtendsDec() != null)
                {
                    var extends_name = guess_struct.struct_dec.GetStructExtendsDec().GetStructNameDec();
                    if (extends_name != null)
                    {
                        var extends_error = extends_name.GuessType(out ABnfGuess extends_guess);
                        if (extends_error != null) return extends_error;
                        var extends_struct_guess = extends_guess as ALittleScriptGuessStruct;
                        extends_error = CheckJsonStruct(element, extends_struct_guess, map);
                        if (extends_error != null) return extends_error;
                    }
                }

                // 如果已经识别了，那么就直接返回
                if (map.Contains(guess_struct.GetValueWithoutConst())) return null;
                map.Add(guess_struct.GetValueWithoutConst());

                var body_dec = guess_struct.struct_dec.GetStructBodyDec();
                if (body_dec == null)
                    return new ABnfGuessError(element, "struct不完整");

                var var_dec_list = body_dec.GetStructVarDecList();
                foreach (var var_dec in var_dec_list)
                {
                    var error = var_dec.GuessType(out guess);
                    if (error != null) return error;
                    error = CheckJsonStruct(element, guess, map);
                    if (error != null) return error;
                }
            }
            else if (guess is ALittleScriptGuessClass)
            {
                return new ABnfGuessError(element, "http协议接口的参数使用json序列化，内部不能使用类");
            }
            else if (guess is ALittleScriptGuessFunctor)
            {
                return new ABnfGuessError(element, "http协议接口的参数使用json序列化，内部不能使用函数");
            }
            else if (guess.HasAny())
            {
                return new ABnfGuessError(element, "http协议接口的参数使用json序列化，内部不能使用any");
            }

            return null;
        }

        public override ABnfGuessError CheckError()
        {
            if (m_element.GetMethodNameDec() == null)
                return new ABnfGuessError(m_element, "没有函数名");
            if (m_element.GetMethodBodyDec() == null)
                return new ABnfGuessError(m_element, "没有函数体");

            var error = CheckCmdError();
            if (error != null) return error;
            error = CheckProtoError();
            if (error != null) return error;
            return null;
        }
    }
}

