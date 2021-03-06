﻿
using Microsoft.VisualStudio.Text.Classification;
using System;
using System.Collections.Generic;
using System.IO;

namespace ALittle
{
    public class ALittleScriptTranslationLua : ALittleScriptTranslation
    {
        // 命名域生成前缀
        private string m_alittle_gen_namespace_pre;
        
        // 标记是否使用rawset
        private bool m_open_rawset = false;
        // 使用rawset的数量
        private int m_rawset_usecount = 0;

        // 当前文件需要处理的反射信息
        private Dictionary<string, StructReflectInfo> m_reflect_map;
        // 是否需要all_struct
        private bool m_need_all_struct = false;

        protected override string GetExt()
        {
            return "lua";
        }

        // 生成bind命令
        private ABnfGuessError GenerateBindStat(ALittleScriptBindStatElement bind_stat, out string content)
        {
            var value_stat_list = bind_stat.GetValueStatList();

            content = "Lua.Bind(";
            var param_list = new List<string>();
            foreach (var value_stat in value_stat_list)
            {
                var error = GenerateValueStat(value_stat, out string sub_content);
                if (error != null) return error;
                param_list.Add(sub_content);
            }
            content += string.Join(", ", param_list);
            content += ")";
            return null;
        }

        // 生成tcall命令
        private ABnfGuessError GenerateTcallStat(ALittleScriptTcallStatElement tcall_stat, out string content)
        {
            var value_stat_list = tcall_stat.GetValueStatList();

            content = "Lua.TCall(";
            var param_list = new List<string>();
            foreach (var value_stat in value_stat_list)
            {
                var error = GenerateValueStat(value_stat, out string sub_content);
                if (error != null) return error;
                param_list.Add(sub_content);
            }
            content += string.Join(", ", param_list);
            content += ")";
            return null;
        }

        // 生成new List
        private ABnfGuessError GenerateOpNewListStat(ALittleScriptOpNewListStatElement op_new_list, out string content)
        {
            var value_stat_list = op_new_list.GetValueStatList();

            content = "{";
            var param_list = new List<string>();
            foreach (var value_stat in value_stat_list)
            {
                var error = GenerateValueStat(value_stat, out string sub_content);
                if (error != null) return error;
                param_list.Add(sub_content);
            }
            content += string.Join(", ", param_list);
            content += "}";
            return null;
        }

        // 生成new
        private ABnfGuessError GenerateOpNewStat(ALittleScriptOpNewStatElement op_new_stat, out string content)
        {
            content = "";
            // 如果是通用类型
            var generic_type = op_new_stat.GetGenericType();
            if (generic_type != null)
            {
                // 如果是Map，那么直接返回{}
                var map_type = generic_type.GetGenericMapType();
                if (map_type != null)
                {
                    content = "{}";
                    return null;
                }

                // 如果是List，那么直接返回{}
                var list_type = generic_type.GetGenericListType();
                if (list_type != null)
                {
                    content = "{}";
                    return null;
                }

                var functor_type = generic_type.GetGenericFunctorType();
                if (functor_type != null)
                    return new ABnfGuessError(generic_type, "Functor不能使用new来创建");
            }

            // 自定义类型
            var custom_type = op_new_stat.GetCustomType();
            if (custom_type != null)
            {
                var error = custom_type.GuessType(out ABnfGuess guess);
                if (error != null) return error;
                
                // 如果是Map，那么直接返回{}
                if (guess is ALittleScriptGuessMap)
                {
                    content = "{}";
                    return null;
                }

                // 如果是List，那么直接返回{}
                if (guess is ALittleScriptGuessList)
                {
                    content = "{}";
                    return null;
                }

                // 如果是结构体
                if (guess is ALittleScriptGuessStruct)
                {
                    content = "{}";
                    return null;
                }

                // 如果是类
                if (guess is ALittleScriptGuessClass)
                {
                    // 生成custom_type名
                    error = GenerateCustomType(custom_type, out content);
                    if (error != null) return error;
                    content += "(";
                    var param_list = new List<string>();
                    var value_stat_list = op_new_stat.GetValueStatList();
                    foreach (var value_stat in value_stat_list)
                    {
                        error = GenerateValueStat(value_stat, out string sub_content);
                        if (error != null) return error;
                        param_list.Add(sub_content);
                    }
                    content += string.Join(", ", param_list);
                    content += ")";
                    return null;
                }
                
                // 如果是函数模板参数
                if (guess is ALittleScriptGuessMethodTemplate)
                {
                    var guess_template = guess as ALittleScriptGuessMethodTemplate;
                    if (guess_template.template_extends is ALittleScriptGuessStruct || guess_template.is_struct)
                    {
                        content = "{}";
                        return null;
                    }
                    
                    if (guess_template.template_extends is ALittleScriptGuessClass || guess_template.is_class)
                    {
                        content = guess_template.GetValue() + "()";
                        return null;
                    }
                }

                // 如果是类模板参数
                if (guess is ALittleScriptGuessClassTemplate)
                {
                    var guess_template = guess as ALittleScriptGuessClassTemplate;
                    if (guess_template.template_extends is ALittleScriptGuessStruct || guess_template.is_struct)
                    {
                        content = "{}";
                        return null;
                    }

                    if (guess_template.template_extends is ALittleScriptGuessClass)
                    {
                        // 生成custom_type名
                        error = GenerateCustomType(custom_type, out content);
                        if (error != null) return error;
                        content += "(";
                        var param_list = new List<string>();
                        var value_stat_list = op_new_stat.GetValueStatList();
                        foreach (var value_stat in value_stat_list)
                        {
                            error = GenerateValueStat(value_stat, out string sub_content);
                            if (error != null) return error;
                            param_list.Add(sub_content);
                        }
                        content += string.Join(", ", param_list);
                        content += ")";
                        return null;
                    }

                    if (guess_template.is_class)
                    {
                        return new ABnfGuessError(null, "该模板只是class，不能确定它的构造参数参数");
                    }
                }
            }

            return new ABnfGuessError(null, "new 未知类型");
        }

        // 生成custom_type定义中的模板参数列表
        private ABnfGuessError GenerateCustomTypeTemplateList(List<ABnfGuess> guess_list,
                                            List<string> template_param_list,
                                            List<string> template_param_name_list)
        {
            for (int index = 0; index < guess_list.Count; ++index)
            {
                var guess = guess_list[index];
                if (guess is ALittleScriptGuessClass)
                {
                    var guess_class = guess as ALittleScriptGuessClass;
                    // 添加依赖
                    AddRelay(guess_class.class_dec);
                    // 如果没有模板参数
                    if (guess_class.template_list.Count == 0)
                    {
                        // 获取类名
                        string name = guess_class.GetValue();
                        // 如果是有using定义而来，就使用using_name
                        if (guess_class.using_name != null) name = guess_class.using_name;
                        // 拆分名称，检查命名域，如果与当前相同，或者是lua，那么就去掉
                        string[] split = name.Split('.');
                        if (split.Length == 2 && split[0] == "lua")
                            template_param_list.Add(split[1]);
                        else
                            template_param_list.Add(name);
                        template_param_name_list.Add(name);
                    }
                    // 有模板参数
                    else
                    {
                        // 检查模板参数
                        var sub_guess_list = new List<ABnfGuess>();
                        foreach (var sub_guess in guess_class.template_list)
                        {
                            if (!guess_class.template_map.TryGetValue(sub_guess.GetValueWithoutConst(), out ABnfGuess value_guess))
                                return new ABnfGuessError(null, "参数模板没有填充完毕");
                            if (sub_guess.is_const && !value_guess.is_const)
                            {
                                value_guess = value_guess.Clone();
                                value_guess.is_const = true;
                                value_guess.UpdateValue();
                            }
                            sub_guess_list.Add(value_guess);
                        }
                        // 获取子模板参数
                        var sub_template_param_list = new List<string>();
                        var sub_template_param_name_list = new List<string>();
                        var error = GenerateCustomTypeTemplateList(sub_guess_list, sub_template_param_list, sub_template_param_name_list);
                        if (error != null) return error;

                        // 带命名域的类名
                        string full_class_name = guess_class.namespace_name + "." + guess_class.class_name;

                        // 计算实际类名
                        string class_name = full_class_name;
                        if (guess_class.namespace_name == "lua")
                            class_name = guess_class.class_name;

                        // 计算模板名
                        string template_name = full_class_name + "<" + string.Join(", ", sub_template_param_name_list) + ">";

                        string content = "Lua.Template(" + class_name;
                        content += ", \"" + template_name + "\"";
                        if (sub_template_param_list.Count > 0)
                            content += ", " + string.Join(", ", sub_template_param_list);

                        content += ")";
                        template_param_name_list.Add(template_name);
                        template_param_list.Add(content);
                    }
                }
                // 如果是结构体
                else if (guess is ALittleScriptGuessStruct)
                {
                    template_param_name_list.Add(guess.GetValue());
                    m_need_all_struct = true;
                    template_param_list.Add("___all_struct[" + ALittleScriptUtility.StructHash(guess as ALittleScriptGuessStruct) + "]");
                    var error = GenerateReflectStructInfo(guess as ALittleScriptGuessStruct);
                    if (error != null) return error;
                }
                // 如果是函数模板参数
                else if (guess is ALittleScriptGuessMethodTemplate)
                {
                    var guess_template = guess as ALittleScriptGuessMethodTemplate;
                    template_param_list.Add(guess_template.GetValue());
                    // 如果不是结构体就是类
                    if (guess_template.template_extends is ALittleScriptGuessStruct || guess_template.is_struct)
                        template_param_name_list.Add("\".." + guess_template.GetValue() + ".name..\"");
                    else
                        template_param_name_list.Add("\".." + guess_template.GetValue() + ".__name..\"");
                }
                // 如果是类模板参数
                else if (guess is ALittleScriptGuessClassTemplate)
                {
                    var guess_template = guess as ALittleScriptGuessClassTemplate;
                    template_param_list.Add("self.__class.__element[" + (index + 1) + "]");
                    // 如果不是结构体就是类
                    if (guess_template.template_extends is ALittleScriptGuessStruct || guess_template.is_struct)
                        template_param_name_list.Add("\"..self.__class.__element[" + (index + 1) + "].name..\"");
                    else
                        template_param_name_list.Add("\"..self.__class.__element[" + (index + 1) + "].__name..\"");
                }
                // 其他类型，直接填nil
                else
                {
                    template_param_name_list.Add(guess.GetValue());
                    template_param_list.Add("nil");
                }
            }
            return null;
        }

        // 生成custom_type
        private ABnfGuessError GenerateCustomType(ALittleScriptCustomTypeElement custom_type, out string content)
        {
            content = "";

            var error = custom_type.GuessType(out ABnfGuess guess);
            if (error != null) return error;

            // 如果是结构体名，那么就当表来处理
            if (guess is ALittleScriptGuessStruct)
            {
                content = "{}";
                return null;
            }
            // 如果是类
            else if (guess is ALittleScriptGuessClass)
            {
                var guess_class = guess as ALittleScriptGuessClass;
                // 添加依赖
                AddRelay(guess_class.class_dec);

                // 计算custom_type的类名，如果和当前文件命名与一致，或者是在lua命名域下，取消命名域前缀
                var name_dec = custom_type.GetCustomTypeName();
                if (name_dec == null) return new ABnfGuessError(custom_type, "表达式不完整");
                string class_name = name_dec.GetElementText();

                var dot_id = custom_type.GetCustomTypeDotId();
                if (dot_id != null)
                {
                    var dot_id_name = dot_id.GetCustomTypeDotIdName();
                    if (dot_id_name != null)
                    {
                        if (class_name == "lua")
                            class_name = dot_id_name.GetElementText();
                        else
                            class_name += "." + dot_id_name.GetElementText();
                    }
                    else
                    {
                        // 判断custom_type的来源
                        string pre_namespace_name;
                        error = ((ALittleScriptCustomTypeReference)custom_type.GetReference()).CalcNamespaceName(out pre_namespace_name);
                        if (error != null) return error;

                        if (pre_namespace_name == "alittle" || pre_namespace_name == "lua") pre_namespace_name = "";
                        if (pre_namespace_name.Length > 0) pre_namespace_name += ".";

                        class_name = pre_namespace_name + class_name;
                    }
                }
                else
                {
                    // 判断custom_type的来源
                    string pre_namespace_name;
                    error = ((ALittleScriptCustomTypeReference)custom_type.GetReference()).CalcNamespaceName(out pre_namespace_name);
                    if (error != null) return error;

                    if (pre_namespace_name == "alittle" || pre_namespace_name == "lua") pre_namespace_name = "";
                    if (pre_namespace_name.Length > 0) pre_namespace_name += ".";

                    class_name = pre_namespace_name + class_name;
                }

                // 如果有填充模板参数，那么就模板模板
                List<ALittleScriptAllTypeElement> all_type_list = null;
                if (custom_type.GetCustomTypeTemplate() != null) all_type_list = custom_type.GetCustomTypeTemplate().GetAllTypeList();

                if (all_type_list != null && all_type_list.Count > 0)
                {
                    // 获取所有模板参数
                    var guess_list = new List<ABnfGuess>();
                    foreach (var all_type in all_type_list)
                    {
                        error = all_type.GuessType(out ABnfGuess sub_guess);
                        if (error != null) return error;
                        guess_list.Add(sub_guess);
                    }
                    // 生成模板信息
                    var template_param_list = new List<string>();
                    var template_param_name_list = new List<string>();
                    error = GenerateCustomTypeTemplateList(guess_list, template_param_list, template_param_name_list);
                    if (error != null) return error;

                    string template_name = guess_class.namespace_name + "." + guess_class.class_name;
                    template_name += "<" + string.Join(", ", template_param_name_list) + ">";

                    content = "Lua.Template(" + class_name;
                    content += ", \"" + template_name + "\"";
                    if (template_param_list.Count > 0)
                        content += ", " + string.Join(", ", template_param_list);
                    content += ")";

                    return null;
                }
                else
                {
                    content = class_name;
                    return null;
                }
            }
            // 如果是函数模板参数
            else if (guess is ALittleScriptGuessMethodTemplate)
            {
                content = guess.GetValue();
                return null;
            }
            // 如果是类模板元素
            else if (guess is ALittleScriptGuessClassTemplate)
            {
                var guess_template = guess as ALittleScriptGuessClassTemplate;
                // 检查下标
                var template_pair_dec = guess_template.template_pair_dec;
                var template_dec = template_pair_dec.GetParent() as ALittleScriptTemplateDecElement;
                int index = template_dec.GetTemplatePairDecList().IndexOf(template_pair_dec);
                // 模板元素
                content = "self.__class.__element[" + (index + 1) + "]";
                return null;
            }

            return new ABnfGuessError(null, "未知的表达式类型");
        }

        // 生成8级运算符
        private ABnfGuessError GenerateOp8Suffix(ALittleScriptOp8SuffixElement suffix, out string content)
        {
            content = "";
            string op_string = suffix.GetOp8().GetElementText();
            if (op_string == "||") op_string = "or";

            string value_functor_result = null;
            if (suffix.GetValueFactorStat() != null)
            {
                var error = GenerateValueFactorStat(suffix.GetValueFactorStat(), out value_functor_result);
                if (error != null) return error;
            }
            else if (suffix.GetOp2Value() != null)
            {
                var error = GenerateOp2Value(suffix.GetOp2Value(), out value_functor_result);
                if (error != null) return error;
            }

            var suffix_content_list = new List<string>();
            var suffix_ee_list = suffix.GetOp8SuffixEeList();
            foreach (var suffix_ee in suffix_ee_list)
            {
                var error = GenerateOp8SuffixEe(suffix_ee, out string suffix_ee_result);
                if (error != null) return error;
                suffix_content_list.Add(suffix_ee_result);
            }
            content = op_string + " " + value_functor_result;
            if (suffix_content_list.Count > 0) content += " " + string.Join(" ", suffix_content_list);
            return null;
        }


        private ABnfGuessError GenerateOp8SuffixEe(ALittleScriptOp8SuffixEeElement suffix, out string content)
        {
            if (suffix.GetOp3Suffix() != null)
                return GenerateOp3Suffix(suffix.GetOp3Suffix(), out content);
            else if (suffix.GetOp4Suffix() != null)
                return GenerateOp4Suffix(suffix.GetOp4Suffix(), out content);
            else if (suffix.GetOp5Suffix() != null)
                return GenerateOp5Suffix(suffix.GetOp5Suffix(), out content);
            else if (suffix.GetOp6Suffix() != null)
                return GenerateOp6Suffix(suffix.GetOp6Suffix(), out content);
            else if (suffix.GetOp7Suffix() != null)
                return GenerateOp7Suffix(suffix.GetOp7Suffix(), out content);
            else
            {
                content = "";
                return new ABnfGuessError(null, "GenerateOp8SuffixEe出现未知的表达式");
            }
        }


        private ABnfGuessError GenerateOp8SuffixEx(ALittleScriptOp8SuffixExElement suffix, out string content)
        {
            if (suffix.GetOp8Suffix() != null)
                return GenerateOp8Suffix(suffix.GetOp8Suffix(), out content);
            else
            {
                content = "";
                return new ABnfGuessError(null, "GenerateOp8SuffixEx出现未知的表达式");
            }
        }


        private ABnfGuessError GenerateOp8Stat(ALittleScriptValueFactorStatElement value_factor_stat, ALittleScriptOp8StatElement op_8_stat, out string content)
        {
            content = "";
            var error = GenerateValueFactorStat(value_factor_stat, out string value_functor_result);
            if (error != null) return error;

            var suffix = op_8_stat.GetOp8Suffix();
            error = GenerateOp8Suffix(suffix, out string suffix_result);
            if (error != null) return error;

            var suffix_content_list = new List<string>();
            var suffix_ex_list = op_8_stat.GetOp8SuffixExList();
            foreach (var suffix_ex in suffix_ex_list)
            {
                error = GenerateOp8SuffixEx(suffix_ex, out string sub_content);
                if (error != null) return error;
                suffix_content_list.Add(sub_content);
            }
            content = value_functor_result + " " + suffix_result;
            if (suffix_content_list.Count > 0) content += " " + string.Join(" ", suffix_content_list);
            return null;
        }

        // 生成7级运算符
        private ABnfGuessError GenerateOp7Suffix(ALittleScriptOp7SuffixElement suffix, out string content)
        {
            content = "";
            string op_string = suffix.GetOp7().GetElementText();
            if (op_string == "&&")
                op_string = "and";

            string value_functor_result = null;
            if (suffix.GetValueFactorStat() != null)
            {
                var error = GenerateValueFactorStat(suffix.GetValueFactorStat(), out value_functor_result);
                if (error != null) return error;
            }
            else if (suffix.GetOp2Value() != null)
            {
                var error = GenerateOp2Value(suffix.GetOp2Value(), out value_functor_result);
                if (error != null) return error;
            }

            var suffix_content_list = new List<string>();
            var suffix_ee_list = suffix.GetOp7SuffixEeList();
            foreach (var suffix_ee in suffix_ee_list)
            {
                var error = GenerateOp7SuffixEe(suffix_ee, out string sub_content);
                if (error != null) return error;
                suffix_content_list.Add(sub_content);
            }
            content = op_string + " " + value_functor_result;
            if (suffix_content_list.Count > 0) content += " " + string.Join(" ", suffix_content_list);
            return null;
        }


        private ABnfGuessError GenerateOp7SuffixEe(ALittleScriptOp7SuffixEeElement suffix, out string content)
        {
            if (suffix.GetOp3Suffix() != null)
                return GenerateOp3Suffix(suffix.GetOp3Suffix(), out content);
            else if (suffix.GetOp4Suffix() != null)
                return GenerateOp4Suffix(suffix.GetOp4Suffix(), out content);
            else if (suffix.GetOp5Suffix() != null)
                return GenerateOp5Suffix(suffix.GetOp5Suffix(), out content);
            else if (suffix.GetOp6Suffix() != null)
                return GenerateOp6Suffix(suffix.GetOp6Suffix(), out content);
            else
            {
                content = "";
                return new ABnfGuessError(null, "GenerateOp7SuffixEe出现未知的表达式");
            }
        }


        private ABnfGuessError GenerateOp7SuffixEx(ALittleScriptOp7SuffixExElement suffix, out string content)
        {
            if (suffix.GetOp7Suffix() != null)
                return GenerateOp7Suffix(suffix.GetOp7Suffix(), out content);
            else if (suffix.GetOp8Suffix() != null)
                return GenerateOp8Suffix(suffix.GetOp8Suffix(), out content);
            else
            {
                content = "";
                return new ABnfGuessError(null, "GenerateOp7SuffixEx出现未知的表达式");
            }
        }


        private ABnfGuessError GenerateOp7Stat(ALittleScriptValueFactorStatElement value_factor_stat, ALittleScriptOp7StatElement op_7_stat, out string content)
        {
            content = "";
            var error = GenerateValueFactorStat(value_factor_stat, out string value_functor_result);
            if (error != null) return error;

            var suffix = op_7_stat.GetOp7Suffix();
            error = GenerateOp7Suffix(suffix, out string suffix_result);
            if (error != null) return error;

            var suffix_content_list = new List<string>();
            var suffix_ex_list = op_7_stat.GetOp7SuffixExList();
            foreach (var suffix_ex in suffix_ex_list)
            {
                error = GenerateOp7SuffixEx(suffix_ex, out string sub_content);
                if (error != null) return error;
                suffix_content_list.Add(sub_content);
            }
            content = value_functor_result + " " + suffix_result;
            if (suffix_content_list.Count > 0) content += " " + string.Join(" ", suffix_content_list);
            return null;
        }

        // 生成6级运算符
        private ABnfGuessError GenerateOp6Suffix(ALittleScriptOp6SuffixElement suffix, out string content)
        {
            content = "";
            string op_string = suffix.GetOp6().GetElementText();
            if (op_string == "!=")
                op_string = "~=";

            string value_functor_result = null;
            if (suffix.GetValueFactorStat() != null)
            {
                var error = GenerateValueFactorStat(suffix.GetValueFactorStat(), out value_functor_result);
                if (error != null) return error;
            }
            else if (suffix.GetOp2Value() != null)
            {
                var error = GenerateOp2Value(suffix.GetOp2Value(), out value_functor_result);
                if (error != null) return error;
            }

            var suffix_content_list = new List<string>();
            var suffix_ee_list = suffix.GetOp6SuffixEeList();
            foreach (var suffix_ee in suffix_ee_list)
            {
                var error = GenerateOp6SuffixEe(suffix_ee, out string sub_content);
                if (error != null) return error;
                suffix_content_list.Add(sub_content);
            }
            content = op_string + " " + value_functor_result;
            if (suffix_content_list.Count > 0) content += " " + string.Join(" ", suffix_content_list);
            return null;
        }


        private ABnfGuessError GenerateOp6SuffixEe(ALittleScriptOp6SuffixEeElement suffix, out string content)
        {
            if (suffix.GetOp3Suffix() != null)
                return GenerateOp3Suffix(suffix.GetOp3Suffix(), out content);
            else if (suffix.GetOp4Suffix() != null)
                return GenerateOp4Suffix(suffix.GetOp4Suffix(), out content);
            else if (suffix.GetOp5Suffix() != null)
                return GenerateOp5Suffix(suffix.GetOp5Suffix(), out content);
            else
            {
                content = "";
                return new ABnfGuessError(null, "GenerateOp6SuffixEe出现未知的表达式");
            }
        }


        private ABnfGuessError GenerateOp6SuffixEx(ALittleScriptOp6SuffixExElement suffix, out string content)
        {
            if (suffix.GetOp6Suffix() != null)
                return GenerateOp6Suffix(suffix.GetOp6Suffix(), out content);
            else if (suffix.GetOp7Suffix() != null)
                return GenerateOp7Suffix(suffix.GetOp7Suffix(), out content);
            else if (suffix.GetOp8Suffix() != null)
                return GenerateOp8Suffix(suffix.GetOp8Suffix(), out content);
            else
            {
                content = "";
                return new ABnfGuessError(null, "GenerateOp6SuffixEx出现未知的表达式");
            }
        }


        private ABnfGuessError GenerateOp6Stat(ALittleScriptValueFactorStatElement value_factor_stat, ALittleScriptOp6StatElement op_6_tat, out string content)
        {
            content = "";
            var error = GenerateValueFactorStat(value_factor_stat, out string value_functor_result);
            if (error != null) return error;

            var suffix = op_6_tat.GetOp6Suffix();
            error = GenerateOp6Suffix(suffix, out string suffix_result);
            if (error != null) return error;

            var suffix_content_list = new List<string>();
            var suffix_ex_list = op_6_tat.GetOp6SuffixExList();
            foreach (var suffix_ex in suffix_ex_list)
            {
                error = GenerateOp6SuffixEx(suffix_ex, out string sub_content);
                if (error != null) return error;
                suffix_content_list.Add(sub_content);
            }
            content = value_functor_result + " " + suffix_result;
            if (suffix_content_list.Count > 0) content += " " + string.Join(" ", suffix_content_list);
            return null;
        }

        // 生成5级运算符

        private ABnfGuessError GenerateOp5Suffix(ALittleScriptOp5SuffixElement suffix, out string content)
        {
            content = "";
            string op_string = suffix.GetOp5().GetElementText();

            string value_functor_result = null;
            if (suffix.GetValueFactorStat() != null)
            {
                var error = GenerateValueFactorStat(suffix.GetValueFactorStat(), out value_functor_result);
                if (error != null) return error;
            }
            else if (suffix.GetOp2Value() != null)
            {
                var error = GenerateOp2Value(suffix.GetOp2Value(), out value_functor_result);
                if (error != null) return error;
            }

            var suffix_content_list = new List<string>();
            var suffix_ee_list = suffix.GetOp5SuffixEeList();
            foreach (var suffix_ee in suffix_ee_list)
            {
                var error = GenerateOp5SuffixEe(suffix_ee, out string sub_content);
                if (error != null) return error;
                suffix_content_list.Add(sub_content);
            }
            content = op_string + " " + value_functor_result;
            if (suffix_content_list.Count > 0) content += " " + string.Join(" ", suffix_content_list);
            return null;
        }


        private ABnfGuessError GenerateOp5SuffixEe(ALittleScriptOp5SuffixEeElement suffix, out string content)
        {
            if (suffix.GetOp3Suffix() != null)
                return GenerateOp3Suffix(suffix.GetOp3Suffix(), out content);
            else if (suffix.GetOp4Suffix() != null)
                return GenerateOp4Suffix(suffix.GetOp4Suffix(), out content);
            else
            {
                content = "";
                return new ABnfGuessError(null, "GenerateOp5SuffixEe出现未知的表达式");
            }
        }


        private ABnfGuessError GenerateOp5SuffixEx(ALittleScriptOp5SuffixExElement suffix, out string content)
        {
            if (suffix.GetOp5Suffix() != null)
                return GenerateOp5Suffix(suffix.GetOp5Suffix(), out content);
            else if (suffix.GetOp6Suffix() != null)
                return GenerateOp6Suffix(suffix.GetOp6Suffix(), out content);
            else if (suffix.GetOp7Suffix() != null)
                return GenerateOp7Suffix(suffix.GetOp7Suffix(), out content);
            else if (suffix.GetOp8Suffix() != null)
                return GenerateOp8Suffix(suffix.GetOp8Suffix(), out content);
            else
            {
                content = "";
                return new ABnfGuessError(null, "GenerateOp5SuffixEx出现未知的表达式");
            }
        }
        
        private ABnfGuessError GenerateOp5Stat(ALittleScriptValueFactorStatElement value_factor_stat, ALittleScriptOp5StatElement op_5_stat, out string content)
        {
            content = "";
            var error = GenerateValueFactorStat(value_factor_stat, out string value_functor_result);
            if (error != null) return error;

            var suffix = op_5_stat.GetOp5Suffix();
            error = GenerateOp5Suffix(suffix, out string suffix_result);
            if (error != null) return error;

            var suffix_content_list = new List<string>();
            var suffix_ex_list = op_5_stat.GetOp5SuffixExList();
            foreach (var suffix_ex in suffix_ex_list)
            {
                error = GenerateOp5SuffixEx(suffix_ex, out string sub_content);
                if (error != null) return error;
                suffix_content_list.Add(sub_content);
            }
            content = value_functor_result + " " + suffix_result;
            if (suffix_content_list.Count > 0) content += " " + string.Join(" ", suffix_content_list);
            return null;
        }

        // 生成4级运算符
        private ABnfGuessError GenerateOp4Suffix(ALittleScriptOp4SuffixElement suffix, out string content)
        {
            content = "";
            string op_string = suffix.GetOp4().GetElementText();

            string value_functor_result = null;
            if (suffix.GetValueFactorStat() != null)
            {
                var error = GenerateValueFactorStat(suffix.GetValueFactorStat(), out value_functor_result);
                if (error != null) return error;
            }
            else if (suffix.GetOp2Value() != null)
            {
                var error = GenerateOp2Value(suffix.GetOp2Value(), out value_functor_result);
                if (error != null) return error;
            }

            var suffix_content_list = new List<string>();
            var suffix_ee_list = suffix.GetOp4SuffixEeList();
            foreach (var suffix_ee in suffix_ee_list)
            {
                var error = GenerateOp4SuffixEe(suffix_ee, out string sub_content);
                if (error != null) return error;
                suffix_content_list.Add(sub_content);
            }
            content = op_string + " " + value_functor_result;
            if (suffix_content_list.Count > 0) content += " " + string.Join(" ", suffix_content_list);
            return null;
        }

        private ABnfGuessError GenerateOp4SuffixEe(ALittleScriptOp4SuffixEeElement suffix, out string content)
        {
            if (suffix.GetOp3Suffix() != null)
                return GenerateOp3Suffix(suffix.GetOp3Suffix(), out content);
            else
            {
                content = "";
                return new ABnfGuessError(null, "GenerateOp4SuffixEe出现未知的表达式");
            }
        }


        private ABnfGuessError GenerateOp4SuffixEx(ALittleScriptOp4SuffixExElement suffix, out string content)
        {
            if (suffix.GetOp4Suffix() != null)
                return GenerateOp4Suffix(suffix.GetOp4Suffix(), out content);
            else if (suffix.GetOp5Suffix() != null)
                return GenerateOp5Suffix(suffix.GetOp5Suffix(), out content);
            else if (suffix.GetOp6Suffix() != null)
                return GenerateOp6Suffix(suffix.GetOp6Suffix(), out content);
            else if (suffix.GetOp7Suffix() != null)
                return GenerateOp7Suffix(suffix.GetOp7Suffix(), out content);
            else if (suffix.GetOp8Suffix() != null)
                return GenerateOp8Suffix(suffix.GetOp8Suffix(), out content);
            else
            {
                content = "";
                return new ABnfGuessError(null, "GenerateOp4SuffixEx出现未知的表达式");
            }
        }

        private ABnfGuessError GenerateOp4Stat(ALittleScriptValueFactorStatElement value_factor_stat, ALittleScriptOp4StatElement op_4_stat, out string content)
        {
            content = "";
            var error = GenerateValueFactorStat(value_factor_stat, out string value_functor_result);
            if (error != null) return error;

            ALittleScriptOp4SuffixElement suffix = op_4_stat.GetOp4Suffix();
            error = GenerateOp4Suffix(suffix, out string suffix_result);
            if (error != null) return error;

            List<string> suffix_content_list = new List<string>();
            var suffix_ex_list = op_4_stat.GetOp4SuffixExList();
            foreach (var suffix_ex in suffix_ex_list)
            {
                error = GenerateOp4SuffixEx(suffix_ex, out string sub_content);
                if (error != null) return error;
                suffix_content_list.Add(sub_content);
            }
            content = value_functor_result + " " + suffix_result;
            if (suffix_content_list.Count > 0) content += " " + string.Join(" ", suffix_content_list);
            return null;
        }

        // 生成3级运算符
        private ABnfGuessError GenerateOp3Suffix(ALittleScriptOp3SuffixElement suffix, out string content)
        {
            content = "";
            string op_string = suffix.GetOp3().GetElementText();

            string value_result;
            if (suffix.GetValueFactorStat() != null)
            {
                var error = GenerateValueFactorStat(suffix.GetValueFactorStat(), out value_result);
                if (error != null) return error;
            }
            else if (suffix.GetOp2Value() != null)
            {
                var error = GenerateOp2Value(suffix.GetOp2Value(), out value_result);
                if (error != null) return error;
            }
            else
            {
                return new ABnfGuessError(null, "GenerateOp3Suffix出现未知的表达式");
            }

            content = op_string + " " + value_result;
            return null;
        }

        private ABnfGuessError GenerateOp3SuffixEx(ALittleScriptOp3SuffixExElement suffix, out string content)
        {
            if (suffix.GetOp3Suffix() != null)
                return GenerateOp3Suffix(suffix.GetOp3Suffix(), out content);
            else if (suffix.GetOp4Suffix() != null)
                return GenerateOp4Suffix(suffix.GetOp4Suffix(), out content);
            else if (suffix.GetOp5Suffix() != null)
                return GenerateOp5Suffix(suffix.GetOp5Suffix(), out content);
            else if (suffix.GetOp6Suffix() != null)
                return GenerateOp6Suffix(suffix.GetOp6Suffix(), out content);
            else if (suffix.GetOp7Suffix() != null)
                return GenerateOp7Suffix(suffix.GetOp7Suffix(), out content);
            else if (suffix.GetOp8Suffix() != null)
                return GenerateOp8Suffix(suffix.GetOp8Suffix(), out content);
            else
            {
                content = "";
                return new ABnfGuessError(null, "GenerateOp3SuffixEx出现未知的表达式");
            }
        }

        private ABnfGuessError GenerateValueOpStat(ALittleScriptValueOpStatElement value_op_stat, out string content)
        {
            content = "";

            var value_factor_stat = value_op_stat.GetValueFactorStat();
            if (value_factor_stat == null) return new ABnfGuessError(null, "表达式不完整");

            if (value_op_stat.GetOp3Stat() != null)
                return GenerateOp3Stat(value_factor_stat, value_op_stat.GetOp3Stat(), out content);

            if (value_op_stat.GetOp4Stat() != null)
                return GenerateOp4Stat(value_factor_stat, value_op_stat.GetOp4Stat(), out content);

            if (value_op_stat.GetOp5Stat() != null)
                return GenerateOp5Stat(value_factor_stat, value_op_stat.GetOp5Stat(), out content);

            if (value_op_stat.GetOp6Stat() != null)
                return GenerateOp6Stat(value_factor_stat, value_op_stat.GetOp6Stat(), out content);

            if (value_op_stat.GetOp7Stat() != null)
                return GenerateOp7Stat(value_factor_stat, value_op_stat.GetOp7Stat(), out content);

            if (value_op_stat.GetOp8Stat() != null)
                return GenerateOp8Stat(value_factor_stat, value_op_stat.GetOp8Stat(), out content);

            return GenerateValueFactorStat(value_factor_stat, out content);
        }

        private ABnfGuessError GenerateOp3Stat(ALittleScriptValueFactorStatElement value_factor_stat, ALittleScriptOp3StatElement op_3_stat, out string content)
        {
            content = "";
            var error = GenerateValueFactorStat(value_factor_stat, out string value_functor_result);
            if (error != null) return error;

            ALittleScriptOp3SuffixElement suffix = op_3_stat.GetOp3Suffix();
            error = GenerateOp3Suffix(suffix, out string suffix_result);
            if (error != null) return error;

            var suffix_content_list = new List<string>();
            var suffix_ex_list = op_3_stat.GetOp3SuffixExList();
            foreach (var suffix_ex in suffix_ex_list)
            {
                error = GenerateOp3SuffixEx(suffix_ex, out string sub_content);
                if (error != null) return error;
                suffix_content_list.Add(sub_content);
            }
            content = value_functor_result + " " + suffix_result;
            if (suffix_content_list.Count > 0) content += " " + string.Join(" ", suffix_content_list);
            return null;
        }

        // 生成2级运算符
        private ABnfGuessError GenerateOp2SuffixEx(ALittleScriptOp2SuffixExElement suffix, out string content)
        {
            if (suffix.GetOp3Suffix() != null)
                return GenerateOp3Suffix(suffix.GetOp3Suffix(), out content);
            else if (suffix.GetOp4Suffix() != null)
                return GenerateOp4Suffix(suffix.GetOp4Suffix(), out content);
            else if (suffix.GetOp5Suffix() != null)
                return GenerateOp5Suffix(suffix.GetOp5Suffix(), out content);
            else if (suffix.GetOp6Suffix() != null)
                return GenerateOp6Suffix(suffix.GetOp6Suffix(), out content);
            else if (suffix.GetOp7Suffix() != null)
                return GenerateOp7Suffix(suffix.GetOp7Suffix(), out content);
            else if (suffix.GetOp8Suffix() != null)
                return GenerateOp8Suffix(suffix.GetOp8Suffix(), out content);
            else
            {
                content = "";
                return new ABnfGuessError(null, "GenerateOp2SuffixEx出现未知的表达式");
            }
        }

        private ABnfGuessError GenerateOp2Value(ALittleScriptOp2ValueElement op_2_value, out string content)
        {
            content = "";

            var value_factor = op_2_value.GetValueFactorStat();
            if (value_factor == null)
                return new ABnfGuessError(null, "GenerateOp2Stat单目运算没有操作对象");

            var error = GenerateValueFactorStat(value_factor, out string value_stat_result);
            if (error != null) return error;
            string op_string = op_2_value.GetOp2().GetElementText();
            if (op_string == "!")
                content += "not " + value_stat_result;
            else if (op_string == "-")
                content += "-" + value_stat_result;
            else
                return new ABnfGuessError(null, "GenerateOp2Stat出现未知类型");

            return null;
        }

        private ABnfGuessError GenerateOp2Stat(ALittleScriptOp2StatElement op_2_stat, out string content)
        {
            var error = GenerateOp2Value(op_2_stat.GetOp2Value(), out content);
            if (error != null) return error;

            var suffix_content_list = new List<string>();
            var suffix_ex_list = op_2_stat.GetOp2SuffixExList();
            foreach (var suffix_ex in suffix_ex_list)
            {
                error = GenerateOp2SuffixEx(suffix_ex, out string sub_content);
                if (error != null) return error;
                suffix_content_list.Add(sub_content);
            }
            if (suffix_content_list.Count > 0)
                content += " " + string.Join(" ", suffix_content_list);
            return null;
        }

        // 生成值表达式
        private ABnfGuessError GenerateValueStat(ALittleScriptValueStatElement root, out string content)
        {
            if (root.GetValueOpStat() != null)
                return GenerateValueOpStat(root.GetValueOpStat(), out content);

            if (root.GetOp2Stat() != null)
                return GenerateOp2Stat(root.GetOp2Stat(), out content);

            if (root.GetOpNewStat() != null)
                return GenerateOpNewStat(root.GetOpNewStat(), out content);

            if (root.GetOpNewListStat() != null)
                return GenerateOpNewListStat(root.GetOpNewListStat(), out content);

            if (root.GetBindStat() != null)
                return GenerateBindStat(root.GetBindStat(), out content);

            if (root.GetTcallStat() != null)
                return GenerateTcallStat(root.GetTcallStat(), out content);

            content = "";
            return null;
        }
        
        // 生成ValueFactorStat
        private ABnfGuessError GenerateValueFactorStat(ALittleScriptValueFactorStatElement value_factor, out string content)
        {
            if (value_factor.GetConstValue() != null)
                return GenerateConstValue(value_factor.GetConstValue(), out content);

            if (value_factor.GetReflectValue() != null)
                return GenerateReflectValue(value_factor.GetReflectValue(), out content);

            if (value_factor.GetPathsValue() != null)
                return GeneratePathsValue(value_factor.GetPathsValue(), out content);

            if (value_factor.GetPropertyValue() != null)
                return GeneratePropertyValue(value_factor.GetPropertyValue(), out content);

            if (value_factor.GetCoroutineStat() != null)
                return GenerateCoroutineStat(value_factor.GetCoroutineStat(), out content);

            if (value_factor.GetWrapValueStat() != null)
            {
                content = "";
                var error = GenerateValueStat(value_factor.GetWrapValueStat().GetValueStat(), out string sub_content);
                if (error != null) return error;
                content = "(" + sub_content + ")";
                return null;
            }

            if (value_factor.GetMethodParamTailDec() != null)
            {
                content = value_factor.GetMethodParamTailDec().GetElementText();
                return null;
            }

            content = "";
            return new ABnfGuessError(null, "GenerateValueFactor出现未知类型");
        }

        // 生成常量

        private ABnfGuessError GenerateConstValue(ALittleScriptConstValueElement const_value, out string content)
        {
            content = "";
            string const_value_string = const_value.GetElementText();
            if (const_value_string == "null")
                content += "nil";
            else
                content += const_value_string;
            return null;
        }

        // 生成路径信息
        private ABnfGuessError GeneratePathsValue(ALittleScriptPathsValueElement paths_value, out string content)
		{
            content = "";
            var text = paths_value.GetText();
            if (text == null) return new ABnfGuessError(paths_value, "请输入路径");

            var path = text.GetElementString().Trim();
            var info = new DirectoryInfo(paths_value.GetProjectPath() + path);
            if (!info.Exists) return new ABnfGuessError(paths_value, "路径不存在:" + path);
            var path_list = new List<string>();
            var error = ALittleScriptIndex.GetDeepFilePaths(paths_value.GetFile().GetProjectInfo(), info, "", path_list);
            if (error != null) return error;

            content = "{";
            for (int i = 0; i < path_list.Count; ++i)
			{
                if (i != 0) content += ", ";
                content += "\"" + path_list[i] + "\"";
			}
            content += "}";
            return null;
        }

        // 生成反射
        private ABnfGuessError GenerateReflectValue(ALittleScriptReflectValueElement reflect_value, out string content)
        {
            content = "";
            if (reflect_value.GetReflectCustomType() != null)
            {
                var custom_type = reflect_value.GetReflectCustomType().GetCustomType();
                if (custom_type == null) return new ABnfGuessError(null, "表达式不完整");

                var error = custom_type.GuessType(out ABnfGuess guess);
                if (error != null) return error;
                if (guess is ALittleScriptGuessStruct)
                {
                    var guess_struct = guess as ALittleScriptGuessStruct;
                    m_need_all_struct = true;
                    content = "___all_struct[" + ALittleScriptUtility.StructHash(guess_struct) + "]";
                    return GenerateReflectStructInfo(guess_struct);
                }
                else if (guess is ALittleScriptGuessClass)
                {
                    var guess_class = guess as ALittleScriptGuessClass;
                    string name = guess_class.GetValue();
                    // 如果是using定义而来，那么就使用using_name
                    if (guess_class.using_name != null) name = guess_class.using_name;
                    string[] split = name.Split('.');
                    if (split.Length == 2 && split[0] == "lua")
                        content = split[1];
                    else
                        content = name;
                    return null;
                }
                else if (guess is ALittleScriptGuessMethodTemplate)
                {
                    content = guess.GetValue();
                    return null;
                }
                else if (guess is ALittleScriptGuessClassTemplate)
                {
                    var guess_template = guess as ALittleScriptGuessClassTemplate;
                    var template_dec = guess_template.template_pair_dec.GetParent() as ALittleScriptTemplateDecElement;
                    int index = template_dec.GetTemplatePairDecList().IndexOf(guess_template.template_pair_dec);
                    content = "self.__class.__element[" + (index + 1) + "]";
                    return null;
                }
            }
            else if (reflect_value.GetReflectValueStat() != null)
            {
                var value_stat = reflect_value.GetReflectValueStat().GetValueStat();
                if (value_stat == null) return new ABnfGuessError(null, "表达式不完整");

                var error = value_stat.GuessType(out ABnfGuess guess);
                if (error != null) return error;
                if (guess is ALittleScriptGuessStruct)
                {
                    var guess_struct = guess as ALittleScriptGuessStruct;
                    m_need_all_struct = true;
                    content = "___all_struct[" + ALittleScriptUtility.StructHash(guess_struct) + "]";
                    return GenerateReflectStructInfo(guess_struct);
                }
                else if (guess is ALittleScriptGuessClass)
                {
                    error = GenerateValueStat(value_stat, out string sub_content);
                    if (error != null) return error;
                    content = "(" + sub_content + ").__class";
                    return null;
                }
                else if (guess is ALittleScriptGuessTemplate)
                {
                    var guess_template = guess as ALittleScriptGuessTemplate;
                    if (guess_template.template_extends is ALittleScriptGuessClass || guess_template.is_class)
                    {
                        error = GenerateValueStat(value_stat, out string sub_content);
                        if (error != null) return error;
                        content = "(" + sub_content + ").__class";
                        return null;
                    }

                    return new ABnfGuessError(null, "reflect不能反射struct类型模板对象");
                }
            }

            return new ABnfGuessError(null, "reflect只能反射struct或者class以及class对象");
        }

        // 生成struct的反射信息
        private ABnfGuessError GenerateReflectStructInfo(ABnfGuess guess)
        {
            if (guess is ALittleScriptGuessList)
            {
                var guess_list = guess as ALittleScriptGuessList;
                var error = GenerateReflectStructInfo(guess_list.sub_type);
                if (error != null) return error;
            }
            else if (guess is ALittleScriptGuessMap)
            {
                var guess_map = guess as ALittleScriptGuessMap;
                var error = GenerateReflectStructInfo(guess_map.key_type);
                if (error != null) return error;

                error = GenerateReflectStructInfo(guess_map.value_type);
                if (error != null) return error;
            }
            else if (guess is ALittleScriptGuessStruct)
            {
                var guess_struct = guess as ALittleScriptGuessStruct;

                if (m_reflect_map.ContainsKey(guess_struct.GetValue())) return null;

                var generate = false;
                // 如果是本文件的，那么就生成
                if (guess_struct.struct_dec.GetFullPath() == m_file_path)
                    generate = true;
                // 如果不在同一个工程，那么就生成
                if (guess_struct.struct_dec.GetProjectPath() != m_project_path)
                    generate = true;
                //  如果是同一个工程，并且是register，那么也要生成
                else if (guess_struct.is_register)
                    generate = true;

                var struct_dec = guess_struct.struct_dec;
                var body_dec = struct_dec.GetStructBodyDec();
                if (body_dec == null) return null;

                var info = new StructReflectInfo();
                info.generate = generate;

                // 如果有继承，那么就获取继承
                var extends_dec = struct_dec.GetStructExtendsDec();
                if (extends_dec != null && extends_dec.GetStructNameDec() != null)
                {
                    var error = extends_dec.GetStructNameDec().GuessType(out ABnfGuess extends_guess);
                    if (error != null) return error;
                    if (!(extends_guess is ALittleScriptGuessStruct))
                        return new ABnfGuessError(null, extends_guess.GetValue() + "不是结构体");
                    error = GenerateReflectStructInfo(extends_guess as ALittleScriptGuessStruct);
                    if (error != null) return error;

                    if (!m_reflect_map.TryGetValue(extends_guess.GetValue(), out StructReflectInfo extends_info))
                        return new ABnfGuessError(null, extends_guess.GetValue() + "反射信息生成失败");
                    info.name_list.AddRange(extends_info.name_list);
                    info.type_list.AddRange(extends_info.type_list);
                    foreach (var pair in extends_info.option_map)
                    {
                        if (info.option_map.ContainsKey(pair.Key))
                            info.option_map.Remove(pair.Key);
                        info.option_map.Add(pair.Key, pair.Value);
                    }
                }

                var next_list = new List<ABnfGuess>();
                var var_dec_list = body_dec.GetStructVarDecList();
                foreach (var var_dec in var_dec_list)
                {
                    var error = var_dec.GuessType(out ABnfGuess var_guess);
                    if (error != null) return error;
                    var name_dec = var_dec.GetStructVarNameDec();
                    if (name_dec == null) return new ABnfGuessError(null, guess_struct.GetValue() + "没有定义变量名");
                    info.name_list.Add("\"" + name_dec.GetElementText() + "\"");
                    info.type_list.Add("\"" + var_guess.GetValue() + "\"");
                    
                    next_list.Add(var_guess);
                }
                var option_dec_list = body_dec.GetStructOptionDecList();
                foreach (var option_dec in option_dec_list)
                {
                    var name = option_dec.GetStructOptionNameDec();
                    if (name == null) return new ABnfGuessError(null, guess_struct.GetValue() + "option定义不完整");
                    var value = option_dec.GetText();
                    if (value == null) return new ABnfGuessError(null, guess_struct.GetValue() + "option定义不完整");
                    if (info.option_map.ContainsKey(name.GetElementText()))
                        info.option_map.Remove(name.GetElementText());
                    info.option_map.Add(name.GetElementText(), value.GetElementText());
                }

                string[] split_list = guess_struct.GetValue().Split('.');
                if (split_list.Length != 2) return null;

                info.name = guess_struct.GetValue();
                info.ns_name = split_list[0];
                info.rl_name = split_list[1];
                info.hash_code = ALittleScriptUtility.StructHash(guess_struct);

                info.content = "{\n";
                info.content += "name = \"" + info.name + "\", ";         // 全称
                info.content += "ns_name = \"" + info.ns_name + "\", ";          // 命名域名
                info.content += "rl_name = \"" + info.rl_name + "\", ";          // struct名
                info.content += "hash_code = " + info.hash_code + ",\n";        // 哈希值
                info.content += "name_list = {" + string.Join(",", info.name_list) + "},\n";      // 成员名列表
                info.content += "type_list = {" + string.Join(",", info.type_list) + "},\n";      // 类型名列表
                info.content += "option_map = {";
                int cur_count = 0;
                foreach (var pair in info.option_map)
                {
                    info.content += pair.Key + "=" + pair.Value;
                    ++cur_count;
                    if (info.option_map.Count != cur_count)
                        info.content += ",";
                }
                info.content += "}\n";      // 类型名列表
                info.content += "}";
                m_reflect_map.Add(guess_struct.GetValue(), info);

                foreach (var guess_info in next_list)
                {
                    var error = GenerateReflectStructInfo(guess_info);
                    if (error != null) return error;
                }
            }

            return null;
        }

        // 对其他工程的枚举值进行优化处理，直接生成对应的值
        private ABnfGuessError GenerateEnumValue(ALittleScriptPropertyValueElement prop_value, out bool handle, out string content)
        {
            handle = false;
            content = "";

            var first_type = prop_value.GetPropertyValueFirstType();
            if (first_type == null) return null;

            var error = first_type.GuessType(out ABnfGuess custom_guess);
            if (error != null) return error;

            ALittleScriptGuessEnumName enum_name_guess = null;
            ALittleScriptPropertyValueSuffixElement suffix = null;

            var suffix_list = prop_value.GetPropertyValueSuffixList();
            if (custom_guess is ALittleScriptGuessNamespaceName)
            {
                if (suffix_list.Count != 2) return null;
                suffix = suffix_list[1];

                error = suffix_list[0].GuessType(out ABnfGuess guess);
                if (error != null) return error;
                enum_name_guess = guess as ALittleScriptGuessEnumName;
            }
            else if (custom_guess is ALittleScriptGuessEnumName)
            {
                if (suffix_list.Count != 1) return null;
                suffix = suffix_list[0];

                enum_name_guess = custom_guess as ALittleScriptGuessEnumName;
            }

            if (enum_name_guess == null) return null;
            if (suffix == null) return null;

            if (enum_name_guess.enum_name_dec.GetProjectPath() == m_project_path) return null;

            var dot_id = suffix.GetPropertyValueDotId();
            if (dot_id == null) return null;
            var dot_id_name = dot_id.GetPropertyValueDotIdName();
            if (dot_id_name == null) return null;

            var enum_dec = enum_name_guess.enum_name_dec.GetParent() as ALittleScriptEnumDecElement;
            if (enum_dec == null) return null;

            var body_dec = enum_dec.GetEnumBodyDec();
            if (body_dec == null) return null;

            var var_dec_list = body_dec.GetEnumVarDecList();
            foreach (var var_dec in var_dec_list)
            {
                var name_dec = var_dec.GetEnumVarNameDec();
                if (name_dec == null) continue;
                if (name_dec.GetElementText() != dot_id_name.GetElementText())
                    continue;

                if (var_dec.GetText() != null)
                    content = var_dec.GetText().GetElementText();
                else if (var_dec.GetNumber() != null)
                    content = var_dec.GetNumber().GetElementText();

                handle = content.Length != 0;
            }

            return null;
        }

        // 生成属性值表达式
        private ABnfGuessError GeneratePropertyValue(ALittleScriptPropertyValueElement prop_value, out string content)
        {
            // 对于枚举值进行特殊处理
            var error = GenerateEnumValue(prop_value, out bool handle, out content);
            if (error != null) return error;
            if (handle) return null;

            content = "";
            
            // 用来标记第一个变量是不是lua命名域
            bool is_lua_namespace = false;

            // 获取开头的属性信息
            var first_type = prop_value.GetPropertyValueFirstType();
            var custom_type = first_type.GetPropertyValueCustomType();
            var this_type = first_type.GetPropertyValueThisType();
            var cast_type = first_type.GetPropertyValueCastType();

            error = first_type.GuessType(out ABnfGuess custom_guess);
            if (error != null) return error;
            if (custom_type != null)
            {
                if ((custom_guess is ALittleScriptGuessFunctor && (custom_guess as ALittleScriptGuessFunctor).element is ALittleScriptGlobalMethodDecElement)
                    || custom_guess is ALittleScriptGuessClassName
                    || custom_guess is ALittleScriptGuessEnumName)
                    AddRelay((custom_guess as ALittleScriptGuess).GetElement());

                if (custom_guess is ALittleScriptGuessNamespaceName && (custom_guess.GetValue() == "lua" || custom_guess.GetValue() == "alittle"))
                    is_lua_namespace = true;

                // 如果是lua命名域，那么就忽略
                if (!is_lua_namespace)
                {
                    // 如果custom_type不是命名域，那么就自动补上命名域
                    if (!(custom_guess is ALittleScriptGuessNamespaceName) && custom_guess is ALittleScriptGuess)
                    {
                        // 判断custom_type的来源
                        string pre_namespace_name;
                        error = ((ALittleScriptPropertyValueCustomTypeReference)custom_type.GetReference()).CalcNamespaceName(out pre_namespace_name);
                        if (error != null) return error;

                        if (pre_namespace_name == "alittle" || pre_namespace_name == "lua") pre_namespace_name = "";
                        if (pre_namespace_name.Length > 0)
                            content += pre_namespace_name + ".";
                    }

                    content += custom_type.GetElementText();
                }
            }
            // 如果是this，那么就变为self
            else if (this_type != null)
            {
                content += "self";
            }
            else if (cast_type != null)
            {
                var value_factor_stat = cast_type.GetValueFactorStat();
                if (value_factor_stat == null) return new ABnfGuessError(null, "cast没有填写转换对象");
                error = GenerateValueFactorStat(value_factor_stat, out string sub_content);
                if (error != null) return error;
                content += sub_content;
            }

            string split = ".";
            // 后面跟着后缀属性
            var suffix_list = prop_value.GetPropertyValueSuffixList();
            for (int index = 0; index < suffix_list.Count; ++index)
            {
                // 获取当前后缀
                var suffix = suffix_list[index];
                // 获取上一个后缀
                ALittleScriptPropertyValueSuffixElement pre_suffix = null;
                if (index - 1 >= 0) pre_suffix = suffix_list[index - 1];
                // 获取下一个后缀
                ALittleScriptPropertyValueSuffixElement next_suffix = null;
                if (index + 1 < suffix_list.Count) next_suffix = suffix_list[index + 1];

                // 如果当前是点
                var dot_id = suffix.GetPropertyValueDotId();
                if (dot_id != null)
                {
                    var dot_id_name = dot_id.GetPropertyValueDotIdName();
                    if (dot_id_name == null) return new ABnfGuessError(null, "点后面没有定义属性对象");
                    // 获取类型
                    error = dot_id_name.GuessType(out ABnfGuess guess);
                    if (error != null) return error;

                    split = ".";
                    // 如果是函数名
                    if (guess is ALittleScriptGuessFunctor)
                    {
                        var guess_functor = guess as ALittleScriptGuessFunctor;
                        // 1. 是成员函数
                        // 2. 使用的是调用
                        // 3. 前一个后缀是类实例对象
                        // 那么就要改成使用语法糖
                        if (guess_functor.element is ALittleScriptClassMethodDecElement)
                        {
                            if (next_suffix != null && next_suffix.GetPropertyValueMethodCall() != null)
                            {
                                // 获取前一个后缀的类型
                                ABnfGuess pre_guess = custom_guess;
                                if (pre_suffix != null)
                                {
                                    error = pre_suffix.GuessType(out pre_guess);
                                    if (error != null) return error;
                                }

                                // 只要不是类名，那么肯定就是类实例对象，就是用语法糖
                                if (!(pre_guess is ALittleScriptGuessClassName))
                                    split = ":";
                            }
                        }
                        // setter和getter需要特殊处理
                        else if (guess_functor.element is ALittleScriptClassSetterDecElement
                                    || guess_functor.element is ALittleScriptClassGetterDecElement)
                        {
                            if (next_suffix != null && next_suffix.GetPropertyValueMethodCall() != null)
                            {
                                ABnfGuess pre_guess = custom_guess;
                                if (pre_suffix != null)
                                {
                                    error = pre_suffix.GuessType(out pre_guess);
                                    if (error != null) return error;
                                }

                                // 如果前一个后缀是类名，那么那么就需要获取setter或者getter来获取
                                if (pre_guess is ALittleScriptGuessClassName)
                                {
                                    // 如果是getter，那么一定是一个参数，比如ClassName.disabled(self)
                                    // 如果是setter，那么一定是两个参数，比如ClassName.width(self, 100)
                                    if (next_suffix.GetPropertyValueMethodCall().GetValueStatList().Count == 1)
                                        split = ".__getter.";
                                    else
                                        split = ".__setter.";
                                }
                            }
                        }
                        // 全局函数
                        else if (guess_functor.element is ALittleScriptGlobalMethodDecElement)
                        {
                            AddRelay(guess_functor.element);
                        }
                    }
                    else if (guess is ALittleScriptGuessClassName || guess is ALittleScriptGuessEnumName)
                    {
                        AddRelay((guess as ALittleScriptGuess).GetElement());
                    }

                    if (!is_lua_namespace)
                        content += split;

                    if (dot_id.GetPropertyValueDotIdName() == null)
                        return new ABnfGuessError(null, "点后面没有内容");

                    string name_content = dot_id.GetPropertyValueDotIdName().GetElementText();
                    // 因为lua中自带的string模块名和关键字string一样，所以把lua自动的改成string（大些开头）
                    // 然后再翻译的时候，把string改成string
                    if (is_lua_namespace && name_content == "String")
                        name_content = "string";
                    content += name_content;

                    // 置为false，表示不是命名域
                    is_lua_namespace = false;
                    continue;
                }

                var bracket_value = suffix.GetPropertyValueBracketValue();
                if (bracket_value != null)
                {
                    var value_stat = bracket_value.GetValueStat();
                    if (value_stat != null)
                    {
                        error = GenerateValueStat(value_stat, out string sub_content);
                        if (error != null) return error;
                        content += "[" + sub_content + "]";
                    }
                    continue;
                }

                var method_call = suffix.GetPropertyValueMethodCall();
                if (method_call != null)
                {
                    // 是否是调用了带注解函数，要进行特殊处理
                    var refe = method_call.GetReference();
                    if (!(refe is ALittleScriptPropertyValueMethodCallReference))
                        return new ABnfGuessError(null, "ALittlePropertyValueMethodCall.GetReference()得到的不是ALittlePropertyValueMethodCallReference");

                    var reference = (ALittleScriptPropertyValueMethodCallReference)refe;
                    error = reference.GuessPreType(out ABnfGuess pre_type);
                    if (error != null) return error;
                    if (!(pre_type is ALittleScriptGuessFunctor pre_type_functor))
                        return new ABnfGuessError(null, "ALittlePropertyValueMethodCallReference.guessPreType()得到的不是ALittleScriptGuessFunctor");

                    if (pre_type_functor.proto != null)
                    {
                        if (pre_type_functor.proto == "Http")
                            content = "ALittle.IHttpSender.Invoke";
                        else if (pre_type_functor.proto == "HttpDownload")
                            content = "ALittle.IHttpFileSender.InvokeDownload";
                        else if (pre_type_functor.proto == "HttpUpload")
                            content = "ALittle.IHttpFileSender.InvokeUpload";
                        else if (pre_type_functor.proto == "Msg")
                        {
                            if (pre_type_functor.return_list.Count == 0)
                                content = "ALittle.IMsgCommon.Invoke";
                            else
                                content = "ALittle.IMsgCommon.InvokeRPC";
                        }

                        if (pre_type_functor.param_list.Count != 2)
                            return new ABnfGuessError(null, "GeneratePropertyValue:处理到MethodCall时发现带注解的函数参数数量不是2");
                        if (!(pre_type_functor.param_list[1] is ALittleScriptGuessStruct))
                            return new ABnfGuessError(null, "GeneratePropertyValue:处理到MethodCall时发现带注解的函数第二个参数不是struct");
                        var param_struct = pre_type_functor.param_list[1] as ALittleScriptGuessStruct;
                        int msg_id = ALittleScriptUtility.StructHash(param_struct);

                        var param_list = new List<string>();
                        if (pre_type_functor.proto == "Msg")
                        {
                            param_list.Add("" + msg_id);
                            // 注册协议
                            error = GenerateReflectStructInfo(param_struct);
                            if (error != null) return error;
                            
                            // 如果有返回值，那么也要注册返回值
                            if (pre_type_functor.return_list.Count == 2)
                            {
                                if (!(pre_type_functor.return_list[1] is ALittleScriptGuessStruct))
                                    return new ABnfGuessError(null, "GeneratePropertyValue:处理到MethodCall时发现带注解的函数返回值不是struct");
                                error = GenerateReflectStructInfo(pre_type_functor.return_list[1] as ALittleScriptGuessStruct);
                                if (error != null) return error;
                            }
                        }
                        else
                        {
                            param_list.Add("\"" + param_struct.GetValue() + "\"");
                        }

                        var value_stat_list = method_call.GetValueStatList();
                        foreach (var value_stat in value_stat_list)
                        {
                            error = GenerateValueStat(value_stat, out string sub_content);
                            if (error != null) return error;
                            param_list.Add(sub_content);
                        }

                        content += "(" + string.Join(", ", param_list) + ")";
                    }
                    else
                    {
                        var param_list = new List<string>();

                        // 生成模板参数
                        error = reference.GenerateTemplateParamList(out List<ABnfGuess> template_list);
                        if (error != null) return error;
                        foreach (var guess in template_list)
                        {
                            if (guess is ALittleScriptGuessClass)
                            {
                                var guess_class = guess as ALittleScriptGuessClass;
                                if (guess_class.namespace_name == "lua")
                                    param_list.Add(guess_class.class_name);
                                else
                                    param_list.Add(guess_class.GetValue());
                            }
                            else if (guess is ALittleScriptGuessStruct)
                            {
                                m_need_all_struct = true;
                                param_list.Add("___all_struct[" + ALittleScriptUtility.StructHash(guess as ALittleScriptGuessStruct) + "]");
                                error = GenerateReflectStructInfo(guess as ALittleScriptGuessStruct);
                                if (error != null) return error;
                            }
                            else if (guess is ALittleScriptGuessMethodTemplate)
                            {
                                param_list.Add(guess.GetValue());
                            }
                            else if (guess is ALittleScriptGuessClassTemplate)
                            {
                                var guess_template = guess as ALittleScriptGuessClassTemplate;
                                var template_dec = guess_template.template_pair_dec.GetParent() as ALittleScriptTemplateDecElement;
                                int template_index = template_dec.GetTemplatePairDecList().IndexOf(guess_template.template_pair_dec);
                                param_list.Add("self.__class.__element[" + (template_index + 1) + "]");
                            }
                            else
                            {
                                return new ABnfGuessError(null, "ALittlePropertyValueMethodCallReference.generateTemplateParamList()的返回列表中出现其他类型的ALittleScriptGuess:" + guess.GetValue());
                            }
                        }

                        // 生成实际参数
                        var value_stat_list = method_call.GetValueStatList();
                        for (int i = 0; i < value_stat_list.Count; ++i)
                        {
                            var value_stat = value_stat_list[i];
                            // 如果是成员、setter、getter函数，第一个参数要放在最前面
                            if (i == 0 && split != ":" && (pre_type_functor.element is ALittleScriptClassMethodDecElement
                                        || pre_type_functor.element is ALittleScriptClassGetterDecElement
                                    || pre_type_functor.element is ALittleScriptClassSetterDecElement))
                            {
                                error = GenerateValueStat(value_stat, out string sub_content);
                                if (error != null) return error;
                                param_list.Insert(0, sub_content);
                            }
                            else
                            {
                                error = GenerateValueStat(value_stat, out string sub_content);
                                if (error != null) return error;
                                param_list.Add(sub_content);
                            }
                        }
                        content += "(" + string.Join(", ", param_list) + ")";
                    }
                    continue;
                }

                return new ABnfGuessError(null, "GeneratePropertyValue出现未知类型");
            }

            return null;
        }

        // 生成co
        private ABnfGuessError GenerateCoroutineStat(ALittleScriptCoroutineStatElement root, out string content)
        {
            content = "___COROUTINE";
            return null;
        }

        // 生成using
        private ABnfGuessError GenerateUsingDec(List<ALittleScriptModifierElement> modifier, ALittleScriptUsingDecElement root, string pre_tab, out string content)
        {
            content = "";
            var name_dec = root.GetUsingNameDec();
            if (name_dec == null) return new ABnfGuessError(null, "using 没有定义名称");

            var all_type = root.GetAllType();
            if (all_type == null) return null;

            var custom_type = all_type.GetCustomType();
            if (custom_type == null) return null;

            var error = custom_type.GuessType(out ABnfGuess guess);
            if (error != null) return error;
            if (!(guess is ALittleScriptGuessClass)) return null;

            content = pre_tab;

            if (ALittleScriptUtility.CalcAccessType(modifier) == ALittleScriptUtility.ClassAccessType.PRIVATE)
                content += "local ";
            else
                content += m_alittle_gen_namespace_pre;

            error = GenerateCustomType(custom_type, out string sub_content);
            if (error != null) return error;
            content += name_dec.GetElementText() + " = " + sub_content + ";\n";
            return null;
        }

        // 生成异常表达式
        private ABnfGuessError GenerateThrowExpr(ALittleScriptThrowExprElement return_expr, string pre_tab, out string content)
        {
            content = "";

            var value_stat_list = return_expr.GetValueStatList();
            if (value_stat_list.Count == 0) return new ABnfGuessError(null, "throw第一个参数必须是string类型");

            var error = value_stat_list[0].GuessType(out ABnfGuess guess_info);
            if (error != null) return error;

            if (!(guess_info is ALittleScriptGuessString))
                return new ABnfGuessError(null, "throw第一个参数必须是string类型");
            if (value_stat_list.Count != 1)
                return new ABnfGuessError(null, "throw只有一个参数");

            content = pre_tab + "Lua.Throw(";
            var param_list = new List<string>();
            for (int i = 0; i < value_stat_list.Count; ++i)
            {
                error = GenerateValueStat(value_stat_list[i], out string sub_content);
                if (error != null) return error;
                param_list.Add(sub_content);
            }
            content += string.Join(", ", param_list);
            content += ")\n";
            return null;
        }

        // 生成断言表达式
        private ABnfGuessError GenerateAssertExpr(ALittleScriptAssertExprElement assert_expr, string pre_tab, out string content)
        {
            content = "";

            var value_stat_list = assert_expr.GetValueStatList();
            if (value_stat_list.Count != 2) return new ABnfGuessError(null, "assert有且仅有两个参数，第一个是任意类型，第二个是string类型");

            var error = value_stat_list[1].GuessType(out ABnfGuess guess_info);
            if (error != null) return error;
            if (!(guess_info is ALittleScriptGuessString))
                return new ABnfGuessError(null, "assert第二个参数必须是string类型");

            content = pre_tab + "Lua.Assert(";
            var param_list = new List<string>();
            for (int i = 0; i < value_stat_list.Count; ++i)
            {
                error = GenerateValueStat(value_stat_list[i], out string sub_content);
                if (error != null) return error;
                param_list.Add(sub_content);
            }
            content += string.Join(", ", param_list);
            content += ")\n";
            return null;
        }

        // 生成1级运算符
        private ABnfGuessError GenerateOp1Expr(ALittleScriptOp1ExprElement root, string pre_tab, out string content)
        {
            content = "";
            var value_stat = root.GetValueStat();
            if (value_stat == null)
                return new ABnfGuessError(null, "GenerateOp1Expr 没有操作值:" + root.GetElementText());

            var op_1 = root.GetOp1();

            var error = GenerateValueStat(value_stat, out string value_stat_result);
            if (error != null) return error;

            string op_1_string = op_1.GetElementText();
            if (op_1_string == "++")
            {
                content = pre_tab + value_stat_result + " = " + value_stat_result + " + 1\n";
                return null;
            }

            if (op_1_string == "--")
            {
                content = pre_tab + value_stat_result + " = " + value_stat_result + " - 1\n";
                return null;
            }

            return new ABnfGuessError(null, "GenerateOp1Expr未知类型:" + op_1_string);
        }

        // 生成变量定义以及赋值表达式
        private ABnfGuessError GenerateVarAssignExpr(ALittleScriptVarAssignExprElement root, string pre_tab, string pre_string, out string content)
        {
            content = "";

            var pair_dec_list = root.GetVarAssignDecList();
            if (pair_dec_list.Count == 0)
                return new ABnfGuessError(null, "局部变量没有变量名:" + root.GetElementText());

            content = pre_tab + pre_string;

            var name_list = new List<string>();
            foreach (var pair_dec in pair_dec_list)
                name_list.Add(pair_dec.GetVarAssignNameDec().GetElementText());
            content += string.Join(", ", name_list);

            var value_stat = root.GetValueStat();
            if (value_stat == null)
            {
                content += "\n";
                return null;
            }

            var error = GenerateValueStat(value_stat, out string sub_content);
            if (error != null) return error;
            content += " = " + sub_content + "\n";
            return null;
        }

        // 生成赋值表达式
        private ABnfGuessError GenerateOpAssignExpr(ALittleScriptOpAssignExprElement root, string pre_tab, out string content)
        {
            content = "";
            var prop_value_list = root.GetPropertyValueList();

            // 变量列表
            var content_list = new List<string>();
            foreach (var prop_value in prop_value_list)
            {
                var guess_error = GeneratePropertyValue(prop_value, out string sub_content);
                if (guess_error != null) return guess_error;
                content_list.Add(sub_content);
            }
            string prop_value_result = string.Join(", ", content_list);

            // 如果没有赋值，可以直接返回定义
            var op_assign = root.GetOpAssign();
            var value_stat = root.GetValueStat();
            if (op_assign == null || value_stat == null)
            {
                content = pre_tab + prop_value_result + "\n";
                return null;
            }

            // 获取赋值表达式
            var error = GenerateValueStat(value_stat, out string value_stat_result);
            if (error != null) return error;

            // 处理等号
            if (op_assign.GetElementText() == "=")
            {
                // 这里做优化
                // 把 self._attr = value 优化为  rawset(self, "_attr", value)
                if (m_open_rawset && prop_value_list.Count == 1)
                {
                    var prop_value = prop_value_list[0];
                    var this_type = prop_value.GetPropertyValueFirstType().GetPropertyValueThisType();
                    if (this_type != null && prop_value.GetPropertyValueSuffixList().Count == 1)
                    {
                        var suffix = prop_value.GetPropertyValueSuffixList()[0];
                        if (suffix.GetPropertyValueDotId() != null)
                        {
                            var dot_id = suffix.GetPropertyValueDotId();
                            if (dot_id != null && dot_id.GetPropertyValueDotIdName() != null)
                            {
                                string attr_name = dot_id.GetPropertyValueDotIdName().GetElementText();
                                error = this_type.GuessType(out ABnfGuess this_guess);
                                if (error != null) return error;

                                if (this_guess is ALittleScriptGuessClass)
                                {
                                    var this_guess_class = this_guess as ALittleScriptGuessClass;
                                    var var_name_list = new List<ABnfElement>();
                                    ALittleScriptUtility.FindClassAttrList(this_guess_class.class_dec
                                            , ALittleScriptUtility.sAccessPrivateAndProtectedAndPublic
                                            , ALittleScriptUtility.ClassAttrType.VAR
                                            , attr_name
                                            , var_name_list, 100);
                                    if (var_name_list.Count > 0)
                                    {
                                        ++m_rawset_usecount;
                                        content = pre_tab + "___rawset(self, \"" + attr_name + "\", " + value_stat_result + ")\n";
                                        return null;
                                    }
                                }
                            }
                        }
                    }
                }

                content = pre_tab + prop_value_result + " = " + value_stat_result + "\n";
                return null;
            }

            string op_assign_string = op_assign.GetElementText();

            // 如果出现多个前缀赋值，那么只能是=号
            if (content_list.Count > 1)
                return new ABnfGuessError(null, "等号左边出现多个值的时候，只能使用=赋值符号:" + root.GetElementText());

            content = "";
            switch (op_assign_string)
            {
                case "+=":
                case "-=":
                case "*=":
                case "/=":
                case "%=":
                    string op_string = op_assign_string.Substring(0, 1);
                    content = pre_tab + prop_value_result + " = " + prop_value_result + " " + op_string + " (" + value_stat_result + ")\n";
                    break;
                default:
                    return new ABnfGuessError(null, "未知的赋值操作类型:" + op_assign_string);
            }
            return null;
        }

        // 生成else表达式
        private ABnfGuessError GenerateElseExpr(ALittleScriptElseExprElement root, int continue_num, out bool has_continue, string pre_tab, out string content)
        {
            has_continue = false;

            content = pre_tab;
            content += "else\n";
            List<ALittleScriptAllExprElement> all_expr_list;
            if (root.GetAllExpr() != null)
            {
                all_expr_list = new List<ALittleScriptAllExprElement>
                {
                    root.GetAllExpr()
                };
            }
            else if (root.GetElseBody() != null)
            {
                all_expr_list = root.GetElseBody().GetAllExprList();
            }
            else
            {
                content = "";
                return new ABnfGuessError(null, "表达式不完整");
            }
            foreach (var all_expr in all_expr_list)
            {
                if (!ALittleScriptUtility.IsLanguageEnable(all_expr.GetModifierList()))
                    continue;
                var error = GenerateAllExpr(all_expr, continue_num, out has_continue, pre_tab + "\t", out string sub_content);
                if (error != null) return error;
                content += sub_content;
            }
            return null;
        }

        // 生成elseif表达式
        private ABnfGuessError GenerateElseIfExpr(ALittleScriptElseIfExprElement root, int continue_num, out bool has_continue, string pre_tab, out string content)
        {
            has_continue = false;
            content = "";
            var condition = root.GetElseIfCondition();
            if (condition == null || condition.GetValueStat() == null)
                return new ABnfGuessError(null, "elseif (?) elseif没有条件值:" + root.GetElementText());

            var error = GenerateValueStat(condition.GetValueStat(), out string value_stat_result);
            if (error != null) return error;

            content = pre_tab;
            content += "elseif " + value_stat_result + " then\n";

            List<ALittleScriptAllExprElement> all_expr_list;
            if (root.GetAllExpr() != null)
            {
                all_expr_list = new List<ALittleScriptAllExprElement>
                {
                    root.GetAllExpr()
                };
            }
            else if (root.GetElseIfBody() != null)
            {
                all_expr_list = root.GetElseIfBody().GetAllExprList();
            }
            else
            {
                content = "";
                return new ABnfGuessError(null, "表达式不完整");
            }
            foreach (var all_expr in all_expr_list)
            {
                if (!ALittleScriptUtility.IsLanguageEnable(all_expr.GetModifierList()))
                    continue;
                error = GenerateAllExpr(all_expr, continue_num, out has_continue, pre_tab + "\t", out string sub_content);
                if (error != null) return error;
                content += sub_content;
            }
            return null;
        }

        // 生成if表达式
        private ABnfGuessError GenerateIfExpr(ALittleScriptIfExprElement root, int continue_num, out bool has_continue, string pre_tab, out string content)
        {
            has_continue = false;
            content = "";

            var condition = root.GetIfCondition();
            if (condition == null || condition.GetValueStat() == null)
                return new ABnfGuessError(null, "if (?) if没有条件值:" + root.GetElementText());

            var error = GenerateValueStat(condition.GetValueStat(), out string value_stat_result);
            if (error != null) return error;

            content = pre_tab;
            content += "if " + value_stat_result + " then\n";

            List<ALittleScriptAllExprElement> all_expr_list;
            if (root.GetAllExpr() != null)
            {
                all_expr_list = new List<ALittleScriptAllExprElement>
                {
                    root.GetAllExpr()
                };
            }
            else if (root.GetIfBody() != null)
            {
                all_expr_list = root.GetIfBody().GetAllExprList();
            }
            else
            {
                content = "";
                return new ABnfGuessError(null, "表达式不完整");
            }
            foreach (var all_expr in all_expr_list)
            {
                if (!ALittleScriptUtility.IsLanguageEnable(all_expr.GetModifierList()))
                    continue;
                error = GenerateAllExpr(all_expr, continue_num, out has_continue, pre_tab + "\t", out string sub_content);
                if (error != null) return error;
                content += sub_content;
            }

            var elseIfExprList = root.GetElseIfExprList();
            foreach (var elseIfExpr in elseIfExprList)
            {
                error = GenerateElseIfExpr(elseIfExpr, continue_num, out has_continue, pre_tab, out string sub_content);
                if (error != null) return error;
                content += sub_content;
            }

            var elseExpr = root.GetElseExpr();
            if (elseExpr != null)
            {
                error = GenerateElseExpr(elseExpr, continue_num, out has_continue, pre_tab, out string sub_content);
                if (error != null) return error;
                content += sub_content;
            }
            content += pre_tab + "end\n";
            return null;
        }

        // 生成for表达式
        private ABnfGuessError GenerateForExpr(ALittleScriptForExprElement root, int continue_num, string pre_tab, out string content)
        {
            content = "";

            var for_condition = root.GetForCondition();
            if (for_condition == null) return new ABnfGuessError(null, "表达式不完整");

            var for_pair_dec = for_condition.GetForPairDec();
            if (for_pair_dec == null) return new ABnfGuessError(null, "表达式不完整");

            content = pre_tab;

            string for_step_content = null;

            var for_step_condition = for_condition.GetForStepCondition();
            var for_in_condition = for_condition.GetForInCondition();
            if (for_step_condition != null)
            {
                var for_start_stat = for_step_condition.GetForStartStat();

                var start_value_stat = for_start_stat.GetValueStat();
                if (start_value_stat == null)
                    return new ABnfGuessError(null, "for 没有初始表达式:" + root.GetElementText());

                var error = GenerateValueStat(start_value_stat, out string start_value_stat_result);
                if (error != null) return error;

                var name_dec = for_pair_dec.GetVarAssignNameDec();
                if (name_dec == null)
                    return new ABnfGuessError(null, "for 初始表达式没有变量名:" + root.GetElementText());

                string start_var_name = name_dec.GetElementText();

                content += "local " + start_var_name + " = " + start_value_stat_result + "\n";
                content += pre_tab + "while true do\n";

                var for_end_stat = for_step_condition.GetForEndStat();
                if (for_end_stat == null)
                    return new ABnfGuessError(null, "for 没有结束表达式:" + root.GetElementText());

                var end_value_stat = for_end_stat.GetValueStat();
                error = GenerateValueStat(end_value_stat, out string sub_content);
                if (error != null) return error;
                content += pre_tab + "\tif not(" + sub_content + ") then break end\n";

                var for_step_stat = for_step_condition.GetForStepStat();
                if (for_step_stat == null)
                    return new ABnfGuessError(null, "for 没有步长表达式");

                var step_value_stat = for_step_stat.GetValueStat();
                error = GenerateValueStat(step_value_stat, out sub_content);
                if (error != null) return error;

                for_step_content = pre_tab + "\t" + start_var_name + " = " + start_var_name + "+(" + sub_content + ")\n";
            }
            else if (for_in_condition != null)
            {
                ALittleScriptValueStatElement value_stat = for_in_condition.GetValueStat();
                if (value_stat == null)
                    return new ABnfGuessError(null, "for in 没有遍历的对象:" + root.GetElementText());

                var error = GenerateValueStat(value_stat, out string value_stat_result);
                if (error != null) return error;

                var pair_list = for_in_condition.GetForPairDecList();
                pair_list.Insert(0, for_pair_dec);
                var pair_string_list = new List<string>();
                foreach (var pair in pair_list)
                {
                    var name_dec = pair.GetVarAssignNameDec();
                    if (name_dec == null)
                        return new ABnfGuessError(null, "for in 没有变量名");
                    pair_string_list.Add(name_dec.GetElementText());
                }

                error = ALittleScriptUtility.CalcPairsTypeForLua(value_stat, out string pair_type);
                if (error != null) return error;

                // 如果for in 遇到迭代函数，那么就不用pair_type
                if (pair_type.Length == 0)
                    content += "for " + string.Join(", ", pair_string_list) + " in " + value_stat_result + " do\n";
                else
                    content += "for " + string.Join(", ", pair_string_list) + " in " + pair_type + "(" + value_stat_result + ") do\n";
            }
            else
            {
                return new ABnfGuessError(null, "for(?) 无效的for语句:" + root.GetElementText());
            }

            List<ALittleScriptAllExprElement> all_expr_list;
            if (root.GetAllExpr() != null)
            {
                all_expr_list = new List<ALittleScriptAllExprElement>
                {
                    root.GetAllExpr()
                };
            }
            else if (root.GetForBody() != null)
            {
                all_expr_list = root.GetForBody().GetAllExprList();
            }
            else
            {
                content = "";
                return new ABnfGuessError(null, "表达式不完整");
            }

            bool has_continue_expr = false;
            foreach (var all_expr in all_expr_list)
            {
                if (!ALittleScriptUtility.IsLanguageEnable(all_expr.GetModifierList()))
                    continue;
                var error = GenerateAllExpr(all_expr, continue_num, out bool has_continue, pre_tab + "\t", out string sub_content);
                if (error != null) return error;
                content += sub_content;
                if (has_continue) has_continue_expr = true;
            }

            if (has_continue_expr)
                content += pre_tab + "\t::continue_" + continue_num + "::\n";

            if (for_step_content != null)
                content += for_step_content;
            content += pre_tab + "end\n";
            return null;
        }

        // 生成while表达式
        private ABnfGuessError GenerateWhileExpr(ALittleScriptWhileExprElement root, int continue_num, string pre_tab, out string content)
        {
            content = "";
            var condition = root.GetWhileCondition();
            if (condition == null || condition.GetValueStat() == null)
                return new ABnfGuessError(null, "while (?) { ... } while中没有条件值");

            var error = GenerateValueStat(condition.GetValueStat(), out string value_stat_result);
            if (error != null) return error;

            content = pre_tab + "while " + value_stat_result + " do\n";

            List<ALittleScriptAllExprElement> all_expr_list;
            if (root.GetAllExpr() != null)
            {
                all_expr_list = new List<ALittleScriptAllExprElement>
                {
                    root.GetAllExpr()
                };
            }
            else if (root.GetWhileBody() != null)
            {
                all_expr_list = root.GetWhileBody().GetAllExprList();
            }
            else
            {
                content = "";
                return new ABnfGuessError(null, "表达式不完整");
            }

            bool has_continue_expr = false;
            foreach (var all_expr in all_expr_list)
            {
                if (!ALittleScriptUtility.IsLanguageEnable(all_expr.GetModifierList()))
                    continue;
                error = GenerateAllExpr(all_expr, continue_num, out bool has_continue, pre_tab + "\t", out string sub_content);
                if (error != null) return error;
                content += sub_content;
                if (has_continue) has_continue_expr = true;
            }

            if (has_continue_expr)
                content += pre_tab + "::continue_" + continue_num + "::\n";
            content += pre_tab + "end\n";
            return null;
        }

        // 生成do while表达式
        private ABnfGuessError GenerateDoWhileExpr(ALittleScriptDoWhileExprElement root, int continue_num, string pre_tab, out string content)
        {
            content = "";
            var condition = root.GetDoWhileCondition();
            if (condition == null || condition.GetValueStat() == null)
                return new ABnfGuessError(null, "do { ... } while(?) while中没有条件值");

            var error = GenerateValueStat(condition.GetValueStat(), out string value_stat_result);
            if (error != null) return error;

            content = pre_tab + "repeat\n";

            List<ALittleScriptAllExprElement> all_expr_list;
            if (root.GetDoWhileBody() != null)
            {
                all_expr_list = root.GetDoWhileBody().GetAllExprList();
            }
            else
            {
                content = "";
                return new ABnfGuessError(null, "表达式不完整");
            }

            bool has_continue_expr = false;
            foreach (var all_expr in all_expr_list)
            {
                if (!ALittleScriptUtility.IsLanguageEnable(all_expr.GetModifierList()))
                    continue;
                error = GenerateAllExpr(all_expr, continue_num, out bool has_continue, pre_tab + "\t", out string sub_content);
                if (error != null) return error;
                content += sub_content;
                if (has_continue) has_continue_expr = true;
            }

            if (has_continue_expr)
                content += pre_tab + "::continue_" + continue_num + "::\n";
            content += pre_tab + "until not(" + value_stat_result + ")\n";
            return null;
        }

        // 生成子表达式组
        private ABnfGuessError GenerateWrapExpr(ALittleScriptWrapExprElement root, int continue_num, out bool has_continue_expr, string pre_tab, out string content)
        {
            content = pre_tab + "do\n";

            has_continue_expr = false;
            var all_expr_list = root.GetAllExprList();
            foreach (var all_expr in all_expr_list)
            {
                if (!ALittleScriptUtility.IsLanguageEnable(all_expr.GetModifierList()))
                    continue;
                var error = GenerateAllExpr(all_expr, continue_num, out bool has_continue, pre_tab + "\t", out string sub_content);
                if (error != null) return error;
                content += sub_content;
                if (has_continue) has_continue_expr = true;
            }

            content += pre_tab + "end\n";
            return null;
        }

        // 生成return表达式
        private ABnfGuessError GenerateReturnExpr(ALittleScriptReturnExprElement root, string pre_tab, out string content)
        {
            if (root.GetReturnYield() != null)
            {
                content = pre_tab + "return coroutine.yield()\n";
                return null;
            }

            content = "";

            var value_stat_list = root.GetValueStatList();
            var content_list = new List<string>();
            foreach (var value_stat in value_stat_list)
            {
                var error = GenerateValueStat(value_stat, out string sub_content);
                if (error != null) return error;
                content_list.Add(sub_content);
            }

            string value_stat_result = "";
            if (content_list.Count > 0)
                value_stat_result = " " + string.Join(", ", content_list);

            content = pre_tab + "return" + value_stat_result + "\n";
            return null;
        }

        // 生成break表达式
        private ABnfGuessError GenerateFlowExpr(ALittleScriptFlowExprElement root, int continue_num, out bool has_continue, string pre_tab, out string content)
        {
            has_continue = false;

            content = root.GetElementText();
            if (content.StartsWith("break"))
            {
                content = pre_tab + "break\n";
                return null;
            }
            else if (content.StartsWith("continue"))
            {
                content = pre_tab + "goto continue_" + continue_num + "\n";
                has_continue = true;
                return null;
            }

            return new ABnfGuessError(null, "未知的操作语句:" + content);
        }

        // 生成任意表达式
        private ABnfGuessError GenerateAllExpr(ALittleScriptAllExprElement root, int continue_num, out bool has_continue, string pre_tab, out string content)
        {
            has_continue = false;

            content = "";
            var expr_list = new List<string>();
            foreach (var child in root.GetChilds())
            {
                if (child is ALittleScriptFlowExprElement)
                {
                    var error = GenerateFlowExpr(child as ALittleScriptFlowExprElement, continue_num, out has_continue, pre_tab, out string sub_content);
                    if (error != null) return error;
                    expr_list.Add(sub_content);
                }
                else if (child is ALittleScriptReturnExprElement)
                {
                    var error = GenerateReturnExpr(child as ALittleScriptReturnExprElement, pre_tab, out string sub_content);
                    if (error != null) return error;
                    expr_list.Add(sub_content);
                }
                else if (child is ALittleScriptDoWhileExprElement)
                {
                    var error = GenerateDoWhileExpr(child as ALittleScriptDoWhileExprElement, continue_num + 1, pre_tab, out string sub_content);
                    if (error != null) return error;
                    expr_list.Add(sub_content);
                }
                else if (child is ALittleScriptWhileExprElement)
                {
                    var error = GenerateWhileExpr(child as ALittleScriptWhileExprElement, continue_num + 1, pre_tab, out string sub_content);
                    if (error != null) return error;
                    expr_list.Add(sub_content);
                }
                else if (child is ALittleScriptForExprElement)
                {
                    var error = GenerateForExpr(child as ALittleScriptForExprElement, continue_num + 1, pre_tab, out string sub_content);
                    if (error != null) return error;
                    expr_list.Add(sub_content);
                }
                else if (child is ALittleScriptIfExprElement)
                {
                    var error = GenerateIfExpr(child as ALittleScriptIfExprElement, continue_num, out has_continue, pre_tab, out string sub_content);
                    if (error != null) return error;
                    expr_list.Add(sub_content);
                }
                else if (child is ALittleScriptOpAssignExprElement)
                {
                    var error = GenerateOpAssignExpr(child as ALittleScriptOpAssignExprElement, pre_tab, out string sub_content);
                    if (error != null) return error;
                    expr_list.Add(sub_content);
                }
                else if (child is ALittleScriptVarAssignExprElement)
                {
                    var error = GenerateVarAssignExpr(child as ALittleScriptVarAssignExprElement, pre_tab, "local ", out string sub_content);
                    if (error != null) return error;
                    expr_list.Add(sub_content);
                }
                else if (child is ALittleScriptOp1ExprElement)
                {
                    var error = GenerateOp1Expr(child as ALittleScriptOp1ExprElement, pre_tab, out string sub_content);
                    if (error != null) return error;
                    expr_list.Add(sub_content);
                }
                else if (child is ALittleScriptWrapExprElement)
                {
                    var error = GenerateWrapExpr(child as ALittleScriptWrapExprElement, continue_num, out has_continue, pre_tab, out string sub_content);
                    if (error != null) return error;
                    expr_list.Add(sub_content);
                }
                else if (child is ALittleScriptThrowExprElement)
                {
                    var error = GenerateThrowExpr(child as ALittleScriptThrowExprElement, pre_tab, out string sub_content);
                    if (error != null) return error;
                    expr_list.Add(sub_content);
                }
                else if (child is ALittleScriptAssertExprElement)
                {
                    var error = GenerateAssertExpr(child as ALittleScriptAssertExprElement, pre_tab, out string sub_content);
                    if (error != null) return error;
                    expr_list.Add(sub_content);
                }
            }

            content = string.Join("\n", expr_list);
            return null;
        }

        // 生成枚举
        private ABnfGuessError GenerateEnum(ALittleScriptEnumDecElement root, string pre_tab, out string content)
        {
            content = "";
            var name_dec = root.GetEnumNameDec();
            if (name_dec == null) return new ABnfGuessError(null, root.GetElementText() + "没有定义枚举名");

            content += pre_tab + m_alittle_gen_namespace_pre + name_dec.GetElementText() + " = {\n";

            int enum_value = -1;
            string enum_string;

            var body_dec = root.GetEnumBodyDec();
            if (body_dec == null) return new ABnfGuessError(null, "表达式不完整");

            var var_dec_list = body_dec.GetEnumVarDecList();
            foreach (var var_dec in var_dec_list)
            {
                if (var_dec.GetNumber() != null)
                {
                    string value = var_dec.GetNumber().GetElementText();
                    if (!ALittleScriptUtility.IsInt(var_dec.GetNumber()))
                        return new ABnfGuessError(null, var_dec.GetNumber().GetElementText() + "对应的枚举值必须是整数");

                    if (value.StartsWith("0x"))
                    {
                        if (!int.TryParse(value.Substring(2), System.Globalization.NumberStyles.HexNumber, null, out int result))
                            return new ABnfGuessError(null, "枚举值的十六进制数解析失败");
                        else
                            enum_value = result;
                    }
                    else
                    {
                        if (!int.TryParse(value, out int result))
                            return new ABnfGuessError(null, "枚举值的十进制数解析失败");
                        else
                            enum_value = result;
                    }
                    enum_string = value;
                }
                else if (var_dec.GetText() != null)
                {
                    enum_string = var_dec.GetText().GetElementText();
                }
                else
                {
                    ++enum_value;
                    enum_string = "" + enum_value;
                }

                content += pre_tab + "\t" + var_dec.GetEnumVarNameDec().GetElementText()
                    + " = " + enum_string + ",\n";
            }

            content += pre_tab + "}\n\n";

            return null;
        }

        // 生成类
        private ABnfGuessError GenerateClass(ALittleScriptClassDecElement root, string pre_tab, out string content)
        {
            content = "";

            var name_dec = root.GetClassNameDec();
            if (name_dec == null) return new ABnfGuessError(null, "类没有定义类名");

            //类声明//////////////////////////////////////////////////////////////////////////////////////////
            string class_name = name_dec.GetElementText();

            var extends_dec = root.GetClassExtendsDec();
            string extends_name = "";
            if (extends_dec != null && extends_dec.GetClassNameDec() != null)
            {
                var error = extends_dec.GetClassNameDec().GuessType(out ABnfGuess guess);
                if (error != null) return error;

                var guess_class = guess as ALittleScriptGuessClass;
                if (guess_class == null)
                    return new ABnfGuessError(extends_dec, "extends_dec.GetClassNameDec().GuessType 得到的不是ALittleScriptGuessClass");
                extends_name = guess_class.namespace_name + "." + guess_class.class_name;

                // 继承属于定义依赖
                m_is_define_relay = true;
                AddRelay(guess_class.class_dec);
                m_is_define_relay = false;
            }
            if (extends_name == "")
                extends_name = "nil";
            else
                content += pre_tab + "assert(" + extends_name + ", \" extends class:" + extends_name + " is nil\")\n";

            content += pre_tab + m_alittle_gen_namespace_pre + class_name + " = "
                + "Lua.Class(" + extends_name + ", \""
                + ALittleScriptUtility.GetNamespaceName(root) + "." + class_name + "\")\n\n";

            var class_body_dec = root.GetClassBodyDec();
            if (class_body_dec == null) return new ABnfGuessError(null, "表达式不完整");

            var class_element_list = class_body_dec.GetClassElementDecList();

            // 获取所有成员变量初始化
            var var_init = "";
            bool has_ctor = false;
            foreach (var class_element_dec in class_element_list)
            {
                if (class_element_dec.GetClassCtorDec() != null)
				{
                    has_ctor = true;
                    continue;
                }

                var var_dec = class_element_dec.GetClassVarDec();
                if (var_dec == null) continue;

                var var_name_dec = var_dec.GetClassVarNameDec();
                if (var_name_dec == null) continue;
                var var_name = var_name_dec.GetElementText();

                var var_value_dec = var_dec.GetClassVarValueDec();
                if (var_value_dec == null) continue;
				{
                    if (var_value_dec.GetConstValue() != null)
					{
                        var error = GenerateConstValue(var_value_dec.GetConstValue(), out var var_value_content);
                        if (error != null) return null;
                        ++m_rawset_usecount;
                        var_init += pre_tab + "\t" + "___rawset(self, \"" + var_name + "\", " + var_value_content + ")\n";
                    }
                    else if (var_value_dec.GetOpNewStat() != null)
                    {
                        var error = GenerateOpNewStat(var_value_dec.GetOpNewStat(), out var op_new_stat_content);
                        if (error != null) return null;
                        ++m_rawset_usecount;
                        var_init += pre_tab + "\t" + "___rawset(self, \"" + var_name + "\", " + op_new_stat_content + ")\n";
                    }
                }
			}

            // 如果没有ctor，并且有初始化函数
            if (!has_ctor && var_init.Length > 0)
			{
                content += pre_tab + "function " + m_alittle_gen_namespace_pre + class_name + ":Ctor()\n";
                content += var_init;
                content += pre_tab + "end\n";
                content += "\n";
            }

            int ctor_count = 0;
            foreach (var class_element_dec in class_element_list)
            {
                if (!ALittleScriptUtility.IsLanguageEnable(class_element_dec.GetModifierList()))
                    continue;

                if (class_element_dec.GetClassCtorDec() != null)
                {
                    ++ctor_count;
                    if (ctor_count > 1)
                        return new ABnfGuessError(null, "class " + class_name + " 最多只能有一个构造函数");
                    //构建构造函数//////////////////////////////////////////////////////////////////////////////////////////
                    string ctor_param_list;

                    var ctor_dec = class_element_dec.GetClassCtorDec();
                    var param_name_list = new List<string>();

                    var param_dec = ctor_dec.GetMethodParamDec();
                    if (param_dec != null)
                    {
                        var param_one_dec_list = param_dec.GetMethodParamOneDecList();
                        foreach (var param_one_dec in param_one_dec_list)
                        {
                            var param_name_dec = param_one_dec.GetMethodParamNameDec();
                            if (param_name_dec == null)
                                return new ABnfGuessError(null, "class " + class_name + " 的构造函数没有参数名");
                            param_name_list.Add(param_name_dec.GetElementText());
                        }
                    }
                    ctor_param_list = string.Join(", ", param_name_list);
                    content += pre_tab + "function " + m_alittle_gen_namespace_pre + class_name + ":Ctor(" + ctor_param_list + ")\n";

                    m_open_rawset = true;

                    var body_dec = ctor_dec.GetMethodBodyDec();
                    var all_expr_content = "";

                    // 初始化成员变量
                    if (var_init.Length > 0)
                    {
                        all_expr_content += var_init;
                        var_init = "";
                    }

                    if (body_dec != null)
                    {
                        var all_expr_list = body_dec.GetAllExprList();
                        foreach (var all_expr in all_expr_list)
                        {
                            if (!ALittleScriptUtility.IsLanguageEnable(all_expr.GetModifierList()))
                                continue;
                            var error = GenerateAllExpr(all_expr, 0, out bool has_continue, pre_tab + "\t", out string sub_content);
                            if (error != null) return error;
                            all_expr_content += sub_content;
                        }
                    }

                    m_open_rawset = false;

                    content += all_expr_content;
                    content += pre_tab + "end\n";

                    content += "\n";
                }
                else if (class_element_dec.GetClassGetterDec() != null)
                {
                    //构建getter函数///////////////////////////////////////////////////////////////////////////////////////
                    var class_getter_dec = class_element_dec.GetClassGetterDec();
                    var class_method_name_dec = class_getter_dec.GetMethodNameDec();
                    if (class_method_name_dec == null)
                        return new ABnfGuessError(null, "class " + class_name + " getter函数没有函数名");

                    content += pre_tab + "function " + m_alittle_gen_namespace_pre + class_name + ".__getter:" + class_method_name_dec.GetElementText() + "()\n";

                    var class_method_body_dec = class_getter_dec.GetMethodBodyDec();
                    if (class_method_body_dec == null)
                        return new ABnfGuessError(null, "class " + class_name + " getter函数没有函数体");
                    var all_expr_list = class_method_body_dec.GetAllExprList();
                    foreach (var all_expr in all_expr_list)
                    {
                        if (!ALittleScriptUtility.IsLanguageEnable(all_expr.GetModifierList()))
                            continue;
                        var error = GenerateAllExpr(all_expr, 0, out bool has_continue, pre_tab + "\t", out string sub_content);
                        if (error != null) return error;
                        content += sub_content;
                    }
                    content += pre_tab + "end\n";

                    content += "\n";
                }
                else if (class_element_dec.GetClassSetterDec() != null)
                {
                    //构建setter函数///////////////////////////////////////////////////////////////////////////////////////
                    var class_setter_dec = class_element_dec.GetClassSetterDec();
                    var class_method_name_dec = class_setter_dec.GetMethodNameDec();
                    if (class_method_name_dec == null)
                        return new ABnfGuessError(null, "class " + class_name + " setter函数没有函数名");
                    var param_dec = class_setter_dec.GetMethodSetterParamDec();
                    if (param_dec == null)
                        return new ABnfGuessError(null, "class " + class_name + " setter函数必须要有一个参数");

                    var param_one_dec = param_dec.GetMethodParamOneDec();
                    if (param_one_dec == null)
                        return new ABnfGuessError(null, "class " + class_name + " setter函数必须要有一个参数");

                    var param_name_dec = param_one_dec.GetMethodParamNameDec();
                    if (param_name_dec == null)
                        return new ABnfGuessError(null, "class " + class_name + " 函数没有定义函数名");

                    content += pre_tab + "function " + m_alittle_gen_namespace_pre + class_name + ".__setter:"
                        + class_method_name_dec.GetElementText() + "("
                        + param_name_dec.GetElementText() + ")\n";

                    var class_method_body_dec = class_setter_dec.GetMethodBodyDec();
                    if (class_method_body_dec == null)
                        return new ABnfGuessError(null, "class " + class_name + " setter函数没有函数体");

                    var all_expr_list = class_method_body_dec.GetAllExprList();
                    foreach (var all_expr in all_expr_list)
                    {
                        if (!ALittleScriptUtility.IsLanguageEnable(all_expr.GetModifierList()))
                            continue;
                        var error = GenerateAllExpr(all_expr, 0, out bool has_continue, pre_tab + "\t", out string sub_content);
                        if (error != null) return error;
                        content += sub_content;
                    }
                    content += pre_tab + "end\n";

                    content += "\n";
                }
                else if (class_element_dec.GetClassMethodDec() != null)
                {
                    //构建成员函数//////////////////////////////////////////////////////////////////////////////////////////
                    var class_method_dec = class_element_dec.GetClassMethodDec();
                    var class_method_name_dec = class_method_dec.GetMethodNameDec();
                    if (class_method_name_dec == null)
                        return new ABnfGuessError(null, "class " + class_name + " 成员函数没有函数名");

                    var param_name_list = new List<string>();

                    var template_dec = class_method_dec.GetTemplateDec();
                    if (template_dec != null)
                    {
                        var pair_dec_list = template_dec.GetTemplatePairDecList();
                        foreach (var pair_dec in pair_dec_list)
                        {
                            var error = pair_dec.GuessType(out ABnfGuess guess);
                            if (error != null) return error;

                            if (guess is ALittleScriptGuessTemplate)
                            {
                                var guess_template = guess as ALittleScriptGuessTemplate;
                                if (guess_template.template_extends != null || guess_template.is_class || guess_template.is_struct)
                                    param_name_list.Add(guess_template.GetValue());
                            }
                        }
                    }

                    var param_dec = class_method_dec.GetMethodParamDec();
                    if (param_dec != null)
                    {
                        var param_one_dec_list = param_dec.GetMethodParamOneDecList();
                        foreach (var param_one_dec in param_one_dec_list)
                        {
                            if (param_one_dec.GetMethodParamTailDec() != null)
                            {
                                param_name_list.Add(param_one_dec.GetMethodParamTailDec().GetElementText());
                                continue;
                            }
                            var param_name_dec = param_one_dec.GetMethodParamNameDec();
                            if (param_name_dec == null)
                                return new ABnfGuessError(null, "class " + class_name + " 成员函数没有参数名");
                            param_name_list.Add(param_name_dec.GetElementText());
                        }
                    }
                    string method_param_list = string.Join(", ", param_name_list);
                    content += pre_tab + "function " + m_alittle_gen_namespace_pre + class_name + ":"
                                + class_method_name_dec.GetElementText()
                                + "(" + method_param_list + ")\n";

                    var coroutine_type = ALittleScriptUtility.GetCoroutineType(class_element_dec.GetModifierList());
                    if (coroutine_type == "await")
                        content += pre_tab + "\tlocal ___COROUTINE = coroutine.running()\n";

                    var class_method_body_dec = class_method_dec.GetMethodBodyDec();
                    if (class_method_body_dec == null)
                        return new ABnfGuessError(null, "class " + class_name + " 成员函数没有函数体");
                    var all_expr_list = class_method_body_dec.GetAllExprList();
                    foreach (var all_expr in all_expr_list)
                    {
                        if (!ALittleScriptUtility.IsLanguageEnable(all_expr.GetModifierList()))
                            continue;
                        var error = GenerateAllExpr(all_expr, 0, out bool has_continue, pre_tab + "\t", out string sub_content);
                        if (error != null) return error;
                        content += sub_content;
                    }
                    content += pre_tab + "end\n";

                    if (coroutine_type == "async")
                    {
                        content += pre_tab
                            + m_alittle_gen_namespace_pre + class_name + "." + class_method_name_dec.GetElementText()
                            + " = " + "Lua.CoWrap("
                            + m_alittle_gen_namespace_pre + class_name + "." + class_method_name_dec.GetElementText()
                            + ")\n";
                    }

                    content += "\n";
                }
                else if (class_element_dec.GetClassStaticDec() != null)
                {
                    //构建静态函数//////////////////////////////////////////////////////////////////////////////////////////
                    var class_static_dec = class_element_dec.GetClassStaticDec();
                    var class_method_name_dec = class_static_dec.GetMethodNameDec();
                    if (class_method_name_dec == null)
                        return new ABnfGuessError(null, "class " + class_name + " 静态函数没有函数名");
                    var param_name_list = new List<string>();

                    var template_dec = class_static_dec.GetTemplateDec();
                    if (template_dec != null)
                    {
                        var pair_dec_list = template_dec.GetTemplatePairDecList();
                        foreach (var pair_dec in pair_dec_list)
                        {
                            var error = pair_dec.GuessType(out ABnfGuess guess);
                            if (error != null) return error;
                            if (guess is ALittleScriptGuessTemplate)
                            {
                                var guess_template = guess as ALittleScriptGuessTemplate;
                                if (guess_template.template_extends != null || guess_template.is_class || guess_template.is_struct)
                                    param_name_list.Add(guess_template.GetValue());
                            }
                        }
                    }

                    var param_dec = class_static_dec.GetMethodParamDec();
                    if (param_dec != null)
                    {
                        var param_one_dec_list = param_dec.GetMethodParamOneDecList();
                        foreach (var param_one_dec in param_one_dec_list)
                        {
                            if (param_one_dec.GetMethodParamTailDec() != null)
                            {
                                param_name_list.Add(param_one_dec.GetMethodParamTailDec().GetElementText());
                                continue;
                            }
                            var param_name_dec = param_one_dec.GetMethodParamNameDec();
                            if (param_name_dec == null)
                                return new ABnfGuessError(null, "class " + class_name + " 静态函数没有参数名");
                            param_name_list.Add(param_name_dec.GetElementText());
                        }
                    }

                    string method_param_list = string.Join(", ", param_name_list);
                    content += pre_tab + "function " + m_alittle_gen_namespace_pre + class_name + "."
                        + class_method_name_dec.GetElementText()
                        + "(" + method_param_list + ")\n";

                    var coroutine_type = ALittleScriptUtility.GetCoroutineType(class_element_dec.GetModifierList());
                    if (coroutine_type == "await")
                        content += pre_tab + "\tlocal ___COROUTINE = coroutine.running()\n";

                    var class_method_body_dec = class_static_dec.GetMethodBodyDec();
                    if (class_method_body_dec == null)
                        return new ABnfGuessError(null, "class " + class_name + " 静态函数没有函数体");

                    var all_expr_list = class_method_body_dec.GetAllExprList();
                    foreach (var all_expr in all_expr_list)
                    {
                        if (!ALittleScriptUtility.IsLanguageEnable(all_expr.GetModifierList()))
                            continue;
                        var error = GenerateAllExpr(all_expr, 0, out bool has_continue, pre_tab + "\t", out string sub_content);
                        if (error != null) return error;
                        content += sub_content;
                    }
                    content += pre_tab + "end\n";

                    if (coroutine_type == "async")
                    {
                        content += pre_tab
                            + m_alittle_gen_namespace_pre + class_name + "." + class_method_name_dec.GetElementText()
                            + " = " + "Lua.CoWrap("
                            + m_alittle_gen_namespace_pre + class_name + "." + class_method_name_dec.GetElementText()
                            + ")\n";
                    }
                    content += "\n";
                }
            }
            ////////////////////////////////////////////////////////////////////////////////////////////

            return null;
        }

        // 生成单例
        private ABnfGuessError GenerateInstance(List<ALittleScriptModifierElement> modifier, ALittleScriptInstanceDecElement root, string pre_tab, out string content)
        {
            content = "";
            var var_assign_expr = root.GetVarAssignExpr();
            var pair_dec_list = var_assign_expr.GetVarAssignDecList();
            if (pair_dec_list.Count == 0)
                return new ABnfGuessError(null, "局部变量没有变量名:" + root.GetElementText());

            var name_list = new List<string>();
            foreach (var pair_dec in pair_dec_list)
                name_list.Add(pair_dec.GetVarAssignNameDec().GetElementText());

            content = pre_tab;

            var access_type = ALittleScriptUtility.CalcAccessType(modifier);
            if (access_type == ALittleScriptUtility.ClassAccessType.PRIVATE)
            {
                content += "local ";
                content += string.Join(", ", name_list);
            }
            else if (access_type == ALittleScriptUtility.ClassAccessType.PROTECTED)
            {
                content += m_alittle_gen_namespace_pre;
                content += string.Join(", " + m_alittle_gen_namespace_pre, name_list);
            }
            else if (access_type == ALittleScriptUtility.ClassAccessType.PUBLIC)
            {
                content += "_G.";
                content += string.Join(", _G.", name_list);
            }

            var value_stat = var_assign_expr.GetValueStat();
            if (value_stat == null)
            {
                content += " = nil\n";
                return null;
            }

            var error = GenerateValueStat(value_stat, out string sub_content);
            if (error != null) return error;
            content += " = " + sub_content + "\n";
            return null;
        }

        // 生成全局函数
        private ABnfGuessError GenerateGlobalMethod(List<ALittleScriptModifierElement> modifier, ALittleScriptGlobalMethodDecElement root, string pre_tab, out string content)
        {
            content = "";

            var global_method_name_dec = root.GetMethodNameDec();
            if (global_method_name_dec == null)
                return new ABnfGuessError(null, "全局函数没有函数名");

            // 函数名
            string method_name = global_method_name_dec.GetElementText();

            // 参数名列表
            var param_name_list = new List<string>();

            // 模板列表
            var template_dec = root.GetTemplateDec();
            if (template_dec != null)
            {
                var pair_dec_list = template_dec.GetTemplatePairDecList();
                foreach (var pair_dec in pair_dec_list)
                {
                    var error = pair_dec.GuessType(out ABnfGuess guess);
                    if (error != null) return error;

                    // 把模板名作为参数名
                    if (guess is ALittleScriptGuessTemplate)
                    {
                        var guess_template = guess as ALittleScriptGuessTemplate;
                        if (guess_template.template_extends != null || guess_template.is_class || guess_template.is_struct)
                            param_name_list.Add(guess_template.GetValue());
                    }
                }
            }

            // 遍历参数列表
            var param_dec = root.GetMethodParamDec();
            if (param_dec != null)
            {
                var param_one_dec_list = param_dec.GetMethodParamOneDecList();
                foreach (var param_one_dec in param_one_dec_list)
                {
                    if (param_one_dec.GetMethodParamTailDec() != null)
                    {
                        param_name_list.Add(param_one_dec.GetMethodParamTailDec().GetElementText());
                        continue;
                    }
                    var param_name_dec = param_one_dec.GetMethodParamNameDec();
                    if (param_name_dec == null)
                        return new ABnfGuessError(null, "全局函数" + method_name + "没有参数名");
                    param_name_list.Add(param_name_dec.GetElementText());
                }
            }

            // 私有判定
            bool isPrivate = ALittleScriptUtility.CalcAccessType(modifier) == ALittleScriptUtility.ClassAccessType.PRIVATE;

            string method_param_list = string.Join(", ", param_name_list);
            if (isPrivate)
            {
                content += pre_tab + "local " + method_name + "\n"
                    + pre_tab + method_name + " = " + "function(" + method_param_list + ")\n";
            }
            else
            {
                content += pre_tab + "function " + m_alittle_gen_namespace_pre + method_name + "(" + method_param_list + ")\n";
            }

            var coroutine_type = ALittleScriptUtility.GetCoroutineType(modifier);
            if (coroutine_type == "await")
                content += pre_tab + "\tlocal ___COROUTINE = coroutine.running()\n";

            var class_method_body_dec = root.GetMethodBodyDec();
            if (class_method_body_dec == null)
                return new ABnfGuessError(null, "全局函数 " + method_name + " 没有函数体");

            var all_expr_list = class_method_body_dec.GetAllExprList();
            foreach (var all_expr in all_expr_list)
            {
                if (!ALittleScriptUtility.IsLanguageEnable(all_expr.GetModifierList()))
                    continue;
                var error = GenerateAllExpr(all_expr, 0, out bool has_continue, pre_tab + "\t", out string sub_content);
                if (error != null) return error;
                content += sub_content;
            }
            content += pre_tab + "end\n";

            // 协程判定
            if (coroutine_type == "async")
            {
                content += pre_tab + m_alittle_gen_namespace_pre + method_name
                    + " = " + "Lua.CoWrap("
                    + m_alittle_gen_namespace_pre + method_name + ")\n";
            }

            content += "\n";

            // 注解判定
            string proto_type = ALittleScriptUtility.GetProtocolType(modifier);
            string command_text = null;
            string command_type = ALittleScriptUtility.GetCommandDetail(modifier, out command_text);
            if (proto_type != null)
            {
                if (param_dec == null) return new ABnfGuessError(null, "带" + proto_type + "的全局函数，必须有两个参数");
                var one_dec_list = param_dec.GetMethodParamOneDecList();
                if (one_dec_list.Count != 2 || one_dec_list[1].GetAllType() == null) return new ABnfGuessError(null, "带" + proto_type + "的全局函数，必须有两个参数");
                
                var error = one_dec_list[1].GetAllType().GuessType(out ABnfGuess guess_param);
                if (error != null) return error;

                if (!(guess_param is ALittleScriptGuessStruct guess_param_struct)) return new ABnfGuessError(null, "带" + proto_type + "的全局函数，第二个参数必须是struct");

                var return_list = new List<ALittleScriptAllTypeElement>();
                var return_dec = root.GetMethodReturnDec();
                if (return_dec != null)
                {
                    var return_one_dec_list = return_dec.GetMethodReturnOneDecList();
                    foreach (var return_one_dec in return_one_dec_list)
                    {
                        if (return_one_dec.GetAllType() != null)
                            return_list.Add(return_one_dec.GetAllType());
                    }
                }

                ABnfGuess guess_return = null;
                if (return_list.Count == 1)
                {
                    error = return_list[0].GuessType(out guess_return);
                    if (error != null) return error;
                }

                if (proto_type == "Http")
                {
                    if (return_list.Count != 1) return new ABnfGuessError(null, "带" + proto_type + "的全局函数，有且仅有一个返回值");
                    content += pre_tab + "ALittle.RegHttpCallback(\"" + guess_param_struct.GetValue() + "\", " + method_name + ")\n";
                }
                else if (proto_type == "HttpDownload")
                {
                    if (return_list.Count != 2) return new ABnfGuessError(null, "带" + proto_type + "的全局函数，有且仅有两个返回值");
                    content += pre_tab + "ALittle.RegHttpDownloadCallback(\"" + guess_param_struct.GetValue() + "\", " + method_name + ")\n";
                }
                else if (proto_type == "HttpUpload")
                {
                    if (return_list.Count != 0) return new ABnfGuessError(null, "带" + proto_type + "的全局函数，不能有返回值");
                    content += pre_tab + "ALittle.RegHttpFileCallback(\"" + guess_param_struct.GetValue() + "\", " + method_name + ")\n";
                }
                else if (proto_type == "Msg")
                {
                    if (return_list.Count > 1) return new ABnfGuessError(null, "带" + proto_type + "的全局函数，最多只有一个返回值");
                    error = GenerateReflectStructInfo(guess_param_struct);
                    if (error != null) return error;
                    
                    if (guess_return == null)
                    {
                        content += pre_tab
                            + "ALittle.RegMsgCallback(" + ALittleScriptUtility.StructHash(guess_param_struct)
                            + ", " + method_name + ")\n";
                    }
                    else
                    {
                        if (!(guess_return is ALittleScriptGuessStruct guess_return_struct)) return new ABnfGuessError(null, "带" + proto_type + "的全局函数，返回值必须是struct");

                        content += pre_tab
                            + "ALittle.RegMsgRpcCallback(" + ALittleScriptUtility.StructHash(guess_param_struct)
                            + ", " + method_name + ", " + ALittleScriptUtility.StructHash(guess_return_struct)
                            + ")\n";

                        error = GenerateReflectStructInfo(guess_return_struct);
                        if (error != null) return error;
                    }
                }
            }
            else if (command_type != null)
            {
                if (command_text == null) command_text = "";

                var var_list = new List<string>();
                var name_list = new List<string>();
                foreach (string param_name in param_name_list)
                    name_list.Add("\"" + param_name + "\"");

                if (param_dec != null)
                {
                    var one_dec_list = param_dec.GetMethodParamOneDecList();
                    foreach (var one_dec in one_dec_list)
                    {
                        var error = one_dec.GetAllType().GuessType(out ABnfGuess all_type_guess);
                        if (error != null) return error;
                        var_list.Add("\"" + all_type_guess.GetValue() + "\"");
                    }
                }

                content += pre_tab
                    + "ALittle.RegCmdCallback(\"" + method_name + "\", " + method_name
                    + ", {" + string.Join(",", var_list) + "}, {" + string.Join(",", name_list)
                    + "}, \"" + command_text + "\")\n";
            }

            return null;
        }

        // 生成根节点
        protected override ABnfGuessError GenerateRoot(List<ALittleScriptNamespaceElementDecElement> element_dec_list, out string content)
        {
            content = "-- ALittle Generate Lua And Do Not Edit This Line!\n";
            m_reflect_map = new Dictionary<string, StructReflectInfo>();

            m_alittle_gen_namespace_pre = "";
            if (m_namespace_name == "lua" || m_namespace_name == "alittle")
                m_alittle_gen_namespace_pre = "_G.";
            else
                m_alittle_gen_namespace_pre = m_namespace_name + ".";

            string other_content = "";
            foreach (var child in element_dec_list)
            {
                if (!ALittleScriptUtility.IsLanguageEnable(child.GetModifierList()))
                    continue;

                // 处理结构体
                if (child.GetStructDec() != null)
                {
                    var error = child.GetStructDec().GuessType(out ABnfGuess guess);
                    if (error != null) return error;
                    error = GenerateReflectStructInfo(guess as ALittleScriptGuessStruct);
                    if (error != null) return error;
                }
                // 处理enum
                else if (child.GetEnumDec() != null)
                {
                    var error = GenerateEnum(child.GetEnumDec(), "", out string sub_content);
                    if (error != null) return error;
                    other_content += sub_content;
                }
                // 处理class
                else if (child.GetClassDec() != null)
                {
                    var error = GenerateClass(child.GetClassDec(), "", out string sub_content);
                    if (error != null) return error;
                    other_content += sub_content;
                }
                // 处理instance
                else if (child.GetInstanceDec() != null)
                {
                    m_is_define_relay = true;
                    var error = GenerateInstance(child.GetModifierList(), child.GetInstanceDec(), "", out string sub_content);
                    m_is_define_relay = false;
                    if (error != null) return error;
                    other_content += sub_content;
                }
                // 处理全局函数
                else if (child.GetGlobalMethodDec() != null)
                {
                    m_is_define_relay = false;
                    var error = GenerateGlobalMethod(child.GetModifierList(), child.GetGlobalMethodDec(), "", out string sub_content);
                    if (error != null) return error;
                    other_content += sub_content;
                }
                // 处理全局操作表达式
                else if (child.GetOpAssignExpr() != null)
                {
                    m_is_define_relay = true;
                    var error = GenerateOpAssignExpr(child.GetOpAssignExpr(), "", out string sub_content);
                    m_is_define_relay = false;
                    if (error != null) return error;
                    other_content += sub_content;
                }
                // 处理using
                else if (child.GetUsingDec() != null)
                {
                    m_is_define_relay = true;
                    var error = GenerateUsingDec(child.GetModifierList(), child.GetUsingDec(), "", out string sub_content);
                    m_is_define_relay = false;
                    if (error != null) return error;
                    other_content += sub_content;
                }
            }

            if (m_namespace_name == "lua" || m_namespace_name == "alittle")
                content += "do\n";
            else
                content += "do\nif _G." + m_namespace_name + " == nil then _G." + m_namespace_name + " = {} end\n";

            if (m_rawset_usecount > 0) content += "local ___rawset = rawset\n";
            content += "local ___pairs = pairs\n";
            content += "local ___ipairs = ipairs\n";
            if (m_need_all_struct) content += "local ___all_struct = ALittle.GetAllStruct()\n";
            content += "\n";

            var info_list = new List<StructReflectInfo>();
            foreach (var pair in m_reflect_map) info_list.Add(pair.Value);
            info_list.Sort(StructReflectSort);
            foreach (var info in info_list)
            {
                if (!info.generate) continue;
                content += "ALittle.RegStruct(" + info.hash_code + ", \"" + info.name + "\", " + info.content + ")\n";
            }
            content += "\n";

            content += other_content;

            content += "end";

            return null;
        }
    }
}

