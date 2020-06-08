
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ALittle
{
    public class ALittleScriptUtility
    {
        // 删除文件夹，以及子文件和子文件夹
        public static void DeleteDirectory(DirectoryInfo info)
        {
            if (!info.Exists) return;

            var file_list = info.GetFiles();
            foreach (var file in file_list)
                file.Delete();

            var dir_list = info.GetDirectories();
            foreach (var dir in dir_list)
                DeleteDirectory(dir);

            info.Delete();
        }

        // 判断字符串是不是整型值
        public static bool IsInt(ALittleScriptNumberElement element)
        {
            string text = element.GetElementText();
            if (text.StartsWith("0x")) return true;
            return text.IndexOf('.') < 0;
        }

        // 计算哈希值
        public static int JSHash(string content)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(content);

            int l = bytes.Length;
            int h = l;
            int step = (l >> 5) + 1;

            for (int i = l; i >= step; i -= step)
            {
                h = h ^ ((h << 5) + bytes[i - 1] + (h >> 2));
            }
            return h;
        }

        // 计算结构体的哈希值
        public static int StructHash(ALittleScriptGuessStruct guess)
        {
            return JSHash(guess.namespace_name + "." + guess.struct_name) + JSHash(guess.namespace_name) + JSHash(guess.struct_name);
        }

        // 类的属性类型
        public enum ClassAttrType
        {
            VAR,            // 成员变量
            FUN,            // 成员函数
            GETTER,         // getter函数
            SETTER,         // setter函数
            STATIC,         // 静态函数
            TEMPLATE,       // 模板参数
        }

        // 访问权限类型
        public enum ClassAccessType
        {
            PUBLIC,         // 全局可访问
            PROTECTED,      // 本命名域可访问
            PRIVATE,        // 本类可访问
        }

        // 元素类型
        public enum ABnfElementType
        {
            CLASS_NAME,         // 类名
            ENUM_NAME,          // 枚举名
            STRUCT_NAME,        // 结构体名
            INSTANCE_NAME,      // 单例名
            GLOBAL_METHOD,      // 全局函数
            USING_NAME,         // using名
        }

        // 语言判定
        public static bool IsLanguageEnable(List<ALittleScriptModifierElement> element_list)
        {
            foreach (var element in element_list)
            {
                if (element.GetAttributeModifier() != null)
                {
                    var language = element.GetAttributeModifier().GetLanguageModifier();
                    if (language != null)
                    {
                        var modifier = language.GetReference() as ALittleScriptLanguageModifierReference;
                        if (modifier == null) return false;

                        return modifier.IsLanguageEnable();
                    }
                }
            }

            return true;
        }

        // 是否使用原生
        public static bool IsNative(List<ALittleScriptModifierElement> element_list)
        {
            foreach (var element in element_list)
            {
                if (element.GetAttributeModifier() != null)
                {
                    return element.GetAttributeModifier().GetNativeModifier() != null;
                }
            }

            return false;
        }

        // 检查await
        public static ABnfGuessError CheckInvokeAwait(ABnfElement element)
        {
            // 检查这次所在的函数必须要有await或者async修饰
            ABnfElement parent = element;
            while (parent != null)
            {
                if (parent is ALittleScriptNamespaceDecElement)
                {
                    return new ABnfGuessError(element, "全局表达式不能调用带有await的函数");
                }
                else if (parent is ALittleScriptClassCtorDecElement)
                {
                    return new ABnfGuessError(element, "构造函数内不能调用带有await的函数");
                }
                else if (parent is ALittleScriptClassGetterDecElement)
                {
                    return new ABnfGuessError(element, "getter函数内不能调用带有await的函数");
                }
                else if (parent is ALittleScriptClassSetterDecElement)
                {
                    return new ABnfGuessError(element, "setter函数内不能调用带有await的函数");
                }
                else if (parent is ALittleScriptClassMethodDecElement)
                {
                    var modifier = (parent.GetParent() as ALittleScriptClassElementDecElement).GetModifierList();
                    if (GetCoroutineType(modifier) == null)
                        return new ABnfGuessError(element, "所在函数没有async或await修饰");
                    break;
                }
                else if (parent is ALittleScriptClassStaticDecElement)
                {
                    var modifier = (parent.GetParent() as ALittleScriptClassElementDecElement).GetModifierList();
                    if (GetCoroutineType(modifier) == null)
                        return new ABnfGuessError(element, "所在函数没有async或await修饰");
                    break;
                }
                else if (parent is ALittleScriptGlobalMethodDecElement)
                {
                    var modifier = (parent.GetParent() as ALittleScriptNamespaceElementDecElement).GetModifierList();
                    if (GetCoroutineType(modifier) == null)
                        return new ABnfGuessError(element, "所在函数没有async或await修饰");
                    break;
                }
                parent = parent.GetParent();
            }

            return null;
        }

        // 判断是否存在
        public static ABnfGuessError CheckError(ABnfElement parent, List<ALittleScriptModifierElement> element_list)
        {
            int register_count = 0;
            int coroutine_count = 0;
            int access_count = 0;

            int language_count = 0;
            int const_count = 0;
            int nullable_count = 0;
            int proto_cmd_count = 0;
            int native_count = 0;

            foreach (var element in element_list)
            {
                if (element.GetRegisterModifier() != null)
                {
                    ++register_count;

                    if (register_count > 1)
                        return new ABnfGuessError(element.GetRegisterModifier(), "register修饰符只能定义一个");

                    // register只能修饰namespace
                    if (!(parent is ALittleScriptNamespaceDecElement))
                        return new ABnfGuessError(element.GetRegisterModifier(), "register只能修饰namespace");
                }
                else if (element.GetCoroutineModifier() != null)
                {
                    ++coroutine_count;

                    if (coroutine_count > 1)
                        return new ABnfGuessError(element.GetCoroutineModifier(), "协程修饰符只能定义一个");
                    
                    bool has_error = true;

                    if (parent is ALittleScriptNamespaceElementDecElement)
                    {
                        var namespace_element_dec = parent as ALittleScriptNamespaceElementDecElement;
                        has_error = namespace_element_dec.GetGlobalMethodDec() == null;
                    }
                    else if (parent is ALittleScriptClassElementDecElement)
                    {
                        var class_element_dec = parent as ALittleScriptClassElementDecElement;
                        has_error = class_element_dec.GetClassMethodDec() == null
                                    && class_element_dec.GetClassStaticDec() == null;
                    }

                    if (has_error)
                        return new ABnfGuessError(element.GetCoroutineModifier(), "协程修饰符修饰全局函数，类成员函数，类静态函数");
                }
                else if (element.GetAccessModifier() != null)
                {
                    ++access_count;

                    if (access_count > 1)
                        return new ABnfGuessError(element.GetAccessModifier(), "访问修饰符只能定义一个");

                    if (parent is ALittleScriptMethodParamOneDecElement)
                        return new ABnfGuessError(element.GetAccessModifier(), "访问修饰符不能修饰函数形参");

                    if (parent is ALittleScriptNamespaceDecElement)
                        return new ABnfGuessError(element.GetAccessModifier(), "访问修饰符不能修饰namespace");

                    if (parent is ALittleScriptAllExprElement)
                        return new ABnfGuessError(element.GetAccessModifier(), "访问修饰符不能修饰表达式列表");
                }
                else if (element.GetAttributeModifier() != null)
                {
                    var attribute = element.GetAttributeModifier();
                    if (attribute.GetLanguageModifier() != null)
                    {
                        ++language_count;
                        if (language_count > 1)
                            return new ABnfGuessError(attribute.GetLanguageModifier(), "Language修饰符最多只能有一个");

                        if (parent is ALittleScriptMethodParamOneDecElement)
                            return new ABnfGuessError(attribute, "Language修饰符不能修饰函数形参");
                    }
                    else if (attribute.GetConstModifier() != null)
                    {
                        ++const_count;
                        if (const_count > 1)
                            return new ABnfGuessError(attribute.GetConstModifier(), "Const修饰符最多只能有一个");

                        bool has_error = true;
                        if (parent is ALittleScriptClassElementDecElement)
                        {
                            var class_element_dec = parent as ALittleScriptClassElementDecElement;
                            has_error = class_element_dec.GetClassGetterDec() == null
                                        && class_element_dec.GetClassSetterDec() == null
                                        && class_element_dec.GetClassMethodDec() == null;
                        }

                        if (has_error)
                            return new ABnfGuessError(attribute, "Const修饰符修饰类getter函数，类setter函数，类成员函数");
                    }
                    else if (attribute.GetNullableModifier() != null)
                    {
                        ++nullable_count;
                        if (nullable_count > 1)
                            return new ABnfGuessError(attribute.GetNullableModifier(), "Nullable修饰符最多只能有一个");

                        if (!(parent is ALittleScriptMethodParamOneDecElement))
                            return new ABnfGuessError(attribute.GetNullableModifier(), "Nullable只能修饰函数形参");
                    }
                    else if (attribute.GetProtocolModifier() != null)
                    {
                        ++proto_cmd_count;
                        if (proto_cmd_count > 1)
                            return new ABnfGuessError(attribute.GetProtocolModifier(), "协议修饰符和命令修饰符最多只能有一个");

                        bool has_error = true;
                        if (parent is ALittleScriptNamespaceElementDecElement)
                        {
                            var namespace_element_dec = parent as ALittleScriptNamespaceElementDecElement;
                            has_error = namespace_element_dec.GetGlobalMethodDec() == null;
                        }

                        if (has_error)
                            return new ABnfGuessError(attribute, "协议修饰符只能修饰全局函数");
                    }
                    else if (attribute.GetCommandModifier() != null)
                    {
                        ++proto_cmd_count;
                        if (proto_cmd_count > 1)
                            return new ABnfGuessError(attribute.GetCommandModifier(), "协议修饰符和命令修饰符最多只能有一个");

                        bool has_error = true;
                        if (parent is ALittleScriptNamespaceElementDecElement)
                        {
                            var namespace_element_dec = parent as ALittleScriptNamespaceElementDecElement;
                            has_error = namespace_element_dec.GetGlobalMethodDec() == null;
                        }

                        if (has_error)
                            return new ABnfGuessError(attribute, "命令修饰符只能修饰全局函数");
                    }
                    else if (attribute.GetNativeModifier() != null)
                    {
                        ++native_count;
                        if (native_count > 1)
                            return new ABnfGuessError(attribute.GetCommandModifier(), "原生修饰符和命令修饰符最多只能有一个");

                        bool has_error = true;
                        if (parent is ALittleScriptClassElementDecElement)
                        {
                            var class_element_dec = parent as ALittleScriptClassElementDecElement;
                            if (class_element_dec.GetClassVarDec() != null)
                            {
                                var error = class_element_dec.GetClassVarDec().GuessType(out ABnfGuess guess);
                                if (error != null) return error;

                                if (guess is ALittleScriptGuessList)
                                    has_error = false;
                            }
                        }
                        else if (parent is ALittleScriptAllExprElement)
                        {
                            var all_expr = parent as ALittleScriptAllExprElement;
                            if (all_expr.GetForExpr() != null)
                                has_error = false;
                        }
                        else if (parent is ALittleScriptNamespaceElementDecElement)
                        {
                            var namespace_element_dec = parent as ALittleScriptNamespaceElementDecElement;
                            has_error = namespace_element_dec.GetClassDec() == null;
                        }

                        if (has_error)
                            return new ABnfGuessError(attribute, "Native修饰符只能修饰class、类成员List的变量、for表达式");
                    }
                }
            }

            return null;
        }

        // 获取是否是register
        public static bool IsRegister(List<ALittleScriptModifierElement> element_list)
        {
            foreach (var element in element_list)
            {
                if (element.GetRegisterModifier() != null)
                    return true;
            }
            return false;
        }

        // 获取是否是Const
        public static bool IsConst(List<ALittleScriptModifierElement> element_list)
        {
            foreach (var element in element_list)
            {
                if (element.GetAttributeModifier() != null
                    && element.GetAttributeModifier().GetConstModifier() != null)
                    return true;
            }
            return false;
        }

        // 获取是否是Nullable
        public static bool IsNullable(List<ALittleScriptModifierElement> element_list)
        {
            foreach (var element in element_list)
            {
                if (element.GetAttributeModifier() != null
                    && element.GetAttributeModifier().GetNullableModifier() != null)
                    return true;
            }
            return false;
        }

        // 获取协程类型
        public static string GetCoroutineType(List<ALittleScriptModifierElement> element_list)
        {
            foreach (var element in element_list)
            {
                if (element.GetCoroutineModifier() != null)
                    return element.GetCoroutineModifier().GetElementText();
            }
            return null;
        }

        // 获取协议类型
        public static string GetProtocolType(List<ALittleScriptModifierElement> element_list)
        {
            foreach (var element in element_list)
            {
                if (element.GetAttributeModifier() != null
                    && element.GetAttributeModifier().GetProtocolModifier() != null)
                    return element.GetAttributeModifier().GetProtocolModifier().GetElementText();
            }
            return null;
        }

        // 获取命令类型
        public static string GetCommandDetail(List<ALittleScriptModifierElement> element_list, out string desc)
        {
            desc = null;

            foreach (var element in element_list)
            {
                if (element.GetAttributeModifier() != null
                    && element.GetAttributeModifier().GetCommandModifier() != null)
                {
                    var body_dec = element.GetAttributeModifier().GetCommandModifier().GetCommandBodyDec();
                    if (body_dec != null && body_dec.GetText() != null)
                        desc = body_dec.GetText().GetElementString();
                    return element.GetAttributeModifier().GetCommandModifier().GetKey().GetElementText();
                }
            }
            return null;
        }

        // 获取访问权限类型
        public static ClassAccessType CalcAccessType(List<ALittleScriptModifierElement> element_list)
        {
            foreach (var element in element_list)
            {
                if (element.GetAccessModifier() != null)
                {
                    var text = element.GetAccessModifier().GetElementText();
                    if (text == "public")
                        return ClassAccessType.PUBLIC;
                    else if (text == "protected")
                        return ClassAccessType.PROTECTED;

                    return ClassAccessType.PRIVATE;
                }
            }

            return ClassAccessType.PRIVATE;
        }

        // 访问权限等级
        public static int sAccessOnlyPublic = 1;                    // 可以访问public的属性和方法
        public static int sAccessProtectedAndPublic = 2;            // 可以访问public protected的属性和方法
        public static int sAccessPrivateAndProtectedAndPublic = 3;  // 可以public protected private的属性和方法

        // 获取某个元素的命名域对象
        public static ALittleScriptNamespaceDecElement GetNamespaceDec(ABnfFile file)
        {
            if (file == null) return null;
            ALittleScriptRootElement root = file.GetRoot() as ALittleScriptRootElement;
            if (root == null) return null;

            return root.GetNamespaceDec();
        }

        // 获取某个元素的命名域名对象
        public static ALittleScriptNamespaceNameDecElement GetNamespaceNameDec(ABnfFile file)
        {
            var namesapce_dec = GetNamespaceDec(file);
            if (namesapce_dec == null) return null;

            return namesapce_dec.GetNamespaceNameDec();
        }

        // 判断某个是不是register
        public static bool IsRegister(ABnfElement element)
        {
            var namespace_dec = GetNamespaceDec(element.GetFile());
            if (namespace_dec == null) return false;

            return IsRegister(namespace_dec.GetModifierList());
        }

        // 获取某个元素的命名域名
        public static string GetNamespaceName(ABnfElement element)
        {
            if (element == null) return "";
            var namespace_name_dec = GetNamespaceNameDec(element.GetFile());
            if (namespace_name_dec == null) return "";
            return namespace_name_dec.GetElementText();
        }

        // 获取命名域名
        public static string GetNamespaceName(ABnfFile file)
        {
            var namespace_name_dec = GetNamespaceNameDec(file);
            if (namespace_name_dec == null) return "";
            return namespace_name_dec.GetElementText();
        }

        // 获取这个元素所在的类
        public static ALittleScriptClassDecElement FindClassDecFromParent(ABnfElement dec)
        {
            while (dec != null)
            {
                if (dec is ALittleScriptClassDecElement)
                    return dec as ALittleScriptClassDecElement;
                dec = dec.GetParent();
            }
            return null;
        }

        // 获取这个元素所在的函数模板
        public static ALittleScriptTemplateDecElement FindMethodTemplateDecFromParent(ABnfElement dec)
        {
            while (dec != null)
            {
                if (dec is ALittleScriptRootElement)
                    return null;
                else if (dec is ALittleScriptClassDecElement)
                    return null;
                else if (dec is ALittleScriptClassCtorDecElement)
                    return null;
                else if (dec is ALittleScriptClassSetterDecElement)
                    return null;
                else if (dec is ALittleScriptClassStaticDecElement)
                    return (dec as ALittleScriptClassStaticDecElement).GetTemplateDec();
                else if (dec is ALittleScriptClassMethodDecElement)
                    return (dec as ALittleScriptClassMethodDecElement).GetTemplateDec();
                else if (dec is ALittleScriptGlobalMethodDecElement)
                    return (dec as ALittleScriptGlobalMethodDecElement).GetTemplateDec();

                dec = dec.GetParent();
            }
            return null;
        }

        // 检查是否在静态函数中
        public static bool IsInClassStaticMethod(ABnfElement dec)
        {
            var parent = dec;
            while (true)
            {
                if (parent == null) break;

                if (parent is ALittleScriptRootElement)
                    return false;
                else if (parent is ALittleScriptClassDecElement)
                    return false;
                else if (parent is ALittleScriptClassSetterDecElement)
                    return false;
                else if (parent is ALittleScriptClassMethodDecElement)
                    return false;
                else if (parent is ALittleScriptClassStaticDecElement)
                    return true;
                else if (parent is ALittleScriptGlobalMethodDecElement)
                    return false;

                parent = parent.GetParent();
            }

            return false;
        }

        // 根据名称，获取这个结构体的成员列表
        public static void FindStructVarDecList(ALittleScriptStructDecElement dec, string name, List<ALittleScriptStructVarDecElement> result, int deep)
        {
            if (deep <= 0) return;

            // 处理当前
            var data = ALittleScriptIndex.inst.GetStructData(dec);
            if (data != null)
                data.FindVarDecList(name, result);

            // 处理继承
            var extends_dec = FindStructExtends(dec);
            if (extends_dec != null)
                FindStructVarDecList(extends_dec, name, result, deep - 1);
        }

        // 根据名称，获取这个枚举中的成员
        public static void FindEnumVarDecList(ALittleScriptEnumDecElement dec, string name, List<ALittleScriptEnumVarDecElement> result)
        {
            var data = ALittleScriptIndex.inst.GetEnumData(dec);
            if (data != null)
                data.FindVarDecList(name, result);
        }

        // 计算struct的父类
        public static ALittleScriptStructDecElement FindStructExtends(ALittleScriptStructDecElement dec)
        {
            // 获取继承
            var extends_dec = dec.GetStructExtendsDec();
            if (extends_dec == null) return null;

            // 获取结构体名
            var name_dec = extends_dec.GetStructNameDec();
            if (name_dec == null) return null;

            // 获取命名域
            string namespace_name;
            var extends_namespace_name_dec = extends_dec.GetNamespaceNameDec();
            if (extends_namespace_name_dec != null)
                namespace_name = extends_namespace_name_dec.GetElementText();
            else
                namespace_name = GetNamespaceName(dec);

            // 获取元素对象
            var result = ALittleScriptIndex.inst.FindALittleNameDec(
                    ABnfElementType.STRUCT_NAME, dec.GetFile()
                    , namespace_name, name_dec.GetElementText(), true);
            if (result is ALittleScriptStructNameDecElement)
                return result.GetParent() as ALittleScriptStructDecElement;

            return null;
        }

        // 计算class的父类
        public static ALittleScriptClassDecElement FindClassExtends(ALittleScriptClassDecElement dec)
        {
            // 获取继承
            var extends_dec = dec.GetClassExtendsDec();
            if (extends_dec == null) return null;

            // 获取类名
            var name_dec = extends_dec.GetClassNameDec();
            if (name_dec == null) return null;

            // 获取命名域
            string namespace_name;
            var extends_namespace_name_dec = extends_dec.GetNamespaceNameDec();
            if (extends_namespace_name_dec != null)
                namespace_name = extends_namespace_name_dec.GetElementText();
            else
                namespace_name = GetNamespaceName(dec);

            // 获取元素对象
            var result = ALittleScriptIndex.inst.FindALittleNameDec(
                    ABnfElementType.CLASS_NAME, dec.GetFile()
                    , namespace_name, name_dec.GetElementText(), true);
            if (result is ALittleScriptClassNameDecElement)
                return result.GetParent() as ALittleScriptClassDecElement;

            return null;
        }

        // 过滤名称相同的元素
        public static List<ABnfElement> FilterSameName(List<ABnfElement> list)
        {
            var map = new Dictionary<string, ABnfElement>();
            for (int i = list.Count - 1; i >= 0; --i)
            {
                string text = list[i].GetElementText();
                if (map.ContainsKey(text)) map.Remove(text);
                map.Add(text, list[i]);
            }
            if (map.Count == list.Count) return list;
            list = new List<ABnfElement>();
            list.AddRange(map.Values);
            return list;
        }

        // 计算在dec这个类中，对targetDec成员的访问权限
        public static int CalcAccessLevelByTargetClassDec(int access_level, ALittleScriptClassDecElement dec, ALittleScriptClassDecElement target_dec)
        {
            // 如果当前访问权限已经只剩下public，就直接返回
            if (access_level <= sAccessOnlyPublic)
                return access_level;

            // 如果dec和目标dec一致，那么直接返回
            if (dec == target_dec)
                return access_level;

            // 检查dec的父类，然后判断父类和targetDec的访问权限
            var extends_dec = FindClassExtends(dec);
            if (extends_dec != null)
                return CalcAccessLevelByTargetClassDec(access_level, extends_dec, target_dec);

            // 如果没有父类，检查是否是在相同命名域下，如果是那么可以访问public和protected
            if (GetNamespaceName(dec) == GetNamespaceName(target_dec))
                return sAccessProtectedAndPublic;

            // 否则只能访问public
            return sAccessOnlyPublic;
        }

        // 计算任意元素访问targetDec的访问权限
        public static int CalcAccessLevelByTargetClassDecForElement(ABnfElement element, ALittleScriptClassDecElement target_dec)
        {
            // 默认为public
            int access_level = sAccessOnlyPublic;

            // 如果这个元素在类中，那么可以通过类和targetDec访问权限直接计算
            var dec = FindClassDecFromParent(element);
            if (dec != null)
            {
                access_level = CalcAccessLevelByTargetClassDec(sAccessPrivateAndProtectedAndPublic, dec, target_dec);
            }
            // 如果元素不在类中，那么element在lua中，或者和targetDec相同，则返回sAccessProtectedAndPublic
            // 如果在同一个文件中，则返回sAccessPrivateAndProtectedAndPublic
            else
            {
                string namespace_name = GetNamespaceName(element);
                if (element.GetFullPath() == target_dec.GetFullPath())
                    access_level = sAccessPrivateAndProtectedAndPublic;
                else if (namespace_name == "alittle" || namespace_name == GetNamespaceName(target_dec))
                    access_level = sAccessProtectedAndPublic;
            }

            return access_level;
        }

        // 根据名称，获取函数列表
        public static void FindClassMethodNameDecList(ALittleScriptClassDecElement dec, int access_level, string name, List<ABnfElement> result, int deep)
        {
            // 这个用于跳出无限递归
            if (deep <= 0) return;

            // 查找类中的元素
            var data = ALittleScriptIndex.inst.GetClassData(dec);
            if (data != null)
            {
                data.FindClassAttrList(access_level, ClassAttrType.FUN, name, result);
                data.FindClassAttrList(access_level, ClassAttrType.GETTER, name, result);
                data.FindClassAttrList(access_level, ClassAttrType.SETTER, name, result);
                data.FindClassAttrList(access_level, ClassAttrType.STATIC, name, result);
            }

            // 处理继承
            var extends_dec = FindClassExtends(dec);
            if (extends_dec != null)
                FindClassMethodNameDecList(extends_dec, access_level, name, result, deep - 1);
        }

        // 根据名称，获取类的属性列表
        public static void FindClassAttrList(ALittleScriptClassDecElement dec
            , int access_level, ClassAttrType attr_type, string name, List<ABnfElement> result, int deep)
        {
            // 这个用于跳出无限递归
            if (deep <= 0) return;

            // 处理成员
            ALittleScriptIndex.inst.FindClassAttrList(dec, access_level, attr_type, name, result);

            // 处理继承
            var extends_dec = FindClassExtends(dec);
            if (extends_dec != null)
                FindClassAttrList(extends_dec, access_level, attr_type, name, result, deep - 1);
        }

        // 根据名称，获取继承的构造函数
        public static ALittleScriptClassCtorDecElement FindFirstCtorDecFromExtends(ALittleScriptClassDecElement dec, int deep)
        {
            // 这个用于跳出无限递归
            if (deep <= 0) return null;

            // 获取class体
            var body_dec = dec.GetClassBodyDec();
            if (body_dec == null) return null;

            // 处理成员函数
            var element_dec_list = body_dec.GetClassElementDecList();
            foreach (var element_dec in element_dec_list)
            {
                if (element_dec.GetClassCtorDec() != null)
                    return element_dec.GetClassCtorDec();
            }

            // 处理继承
            var extends_dec = FindClassExtends(dec);
            if (extends_dec != null)
                return FindFirstCtorDecFromExtends(extends_dec, deep - 1);

            return null;
        }

        // 根据名称，获取继承的属性
        public static ABnfElement FindFirstClassAttrFromExtends(ALittleScriptClassDecElement dec, ClassAttrType attr_type, string name, int deep)
        {
            // 这个用于跳出无限递归
            if (deep <= 0) return null;

            // 处理setter函数
            var result = ALittleScriptIndex.inst.FindClassAttr(dec,
                    sAccessPrivateAndProtectedAndPublic, attr_type, name);
            if (result != null) return result;

            // 处理继承
            var extends_dec = FindClassExtends(dec);
            if (extends_dec != null)
                return FindFirstClassAttrFromExtends(extends_dec, attr_type, name, deep - 1);

            return null;
        }

        // 根据名称，查找函数的参数列表
        public static List<ALittleScriptMethodParamNameDecElement> FindMethodParamNameDecList(ABnfElement method_dec, string name)
        {
            List<ALittleScriptMethodParamOneDecElement> param_one_dec_list = null;
            // 处理构造函数的参数列表
            if (method_dec is ALittleScriptClassCtorDecElement)
            {
                var method_param_dec = ((ALittleScriptClassCtorDecElement)method_dec).GetMethodParamDec();
                if (method_param_dec != null)
                    param_one_dec_list = method_param_dec.GetMethodParamOneDecList();
            }
            // 处理成员函数的参数列表
            else if (method_dec is ALittleScriptClassMethodDecElement)
            {
                var method_param_dec = ((ALittleScriptClassMethodDecElement)method_dec).GetMethodParamDec();
                if (method_param_dec != null)
                    param_one_dec_list = method_param_dec.GetMethodParamOneDecList();
            }
            // 处理静态函数的参数列表
            else if (method_dec is ALittleScriptClassStaticDecElement)
            {
                var method_param_dec = ((ALittleScriptClassStaticDecElement)method_dec).GetMethodParamDec();
                if (method_param_dec != null)
                    param_one_dec_list = method_param_dec.GetMethodParamOneDecList();
            }
            // 处理setter函数的参数列表
            else if (method_dec is ALittleScriptClassSetterDecElement)
            {
                var method_setter_param_dec = ((ALittleScriptClassSetterDecElement)method_dec).GetMethodSetterParamDec();
                if (method_setter_param_dec != null)
                {
                    var param_one_dec = method_setter_param_dec.GetMethodParamOneDec();
                    if (param_one_dec != null)
                    {
                        param_one_dec_list = new List<ALittleScriptMethodParamOneDecElement>();
                        param_one_dec_list.Add(param_one_dec);
                    }
                }
            }
            // 处理全局函数的参数列表
            else if (method_dec is ALittleScriptGlobalMethodDecElement)
            {
                var method_param_dec = ((ALittleScriptGlobalMethodDecElement)method_dec).GetMethodParamDec();
                if (method_param_dec != null)
                    param_one_dec_list = method_param_dec.GetMethodParamOneDecList();
            }

            // 收集所有的参数名元素
            var result = new List<ALittleScriptMethodParamNameDecElement>();
            if (param_one_dec_list != null)
            {
                foreach (var one_dec in param_one_dec_list)
                {
                    var name_dec = one_dec.GetMethodParamNameDec();
                    if (name_dec == null) continue;
                    if (name.Length == 0 || name_dec.GetElementText() == name)
                        result.Add(name_dec);
                }
            }
            return result;
        }

        // 根据名称，查找变量名所在的定义元素
        public static List<ALittleScriptVarAssignNameDecElement> FindVarAssignNameDecList(ABnfElement element, string name)
        {
            var var_dec_list = new List<ALittleScriptVarAssignNameDecElement>();

            // 计算出所在的表达式
            var parent = element;
            while (parent != null)
            {
                if (parent is ALittleScriptAllExprElement)
                {
                    FindVarAssignNameDecList(parent as ALittleScriptAllExprElement, var_dec_list, name);
                    break;
                }
                if (parent is ALittleScriptForStepConditionElement)
                {
                    var for_condition = parent.GetParent() as ALittleScriptForConditionElement;
                    if (for_condition == null) break;
                    var for_expr = for_condition.GetParent() as ALittleScriptForExprElement;
                    if (for_expr == null) break;
                    FindVarAssignNameDecList(for_expr, var_dec_list, name);
                    break;
                }
                parent = parent.GetParent();
            }

            return var_dec_list;
        }

        private static void FindVarAssignNameDecList(ALittleScriptForExprElement for_expr, List<ALittleScriptVarAssignNameDecElement> var_dec_list, string name)
        {
            FindVarAssignNameDecList(for_expr.GetParent() as ALittleScriptAllExprElement, var_dec_list, name);

            var for_condition = for_expr.GetForCondition();
            if (for_condition != null)
                FindVarAssignNameDecList(for_condition, var_dec_list, name);
        }
        private static void FindVarAssignNameDecList(ALittleScriptForConditionElement for_condition, List<ALittleScriptVarAssignNameDecElement> var_dec_list, string name)
        {
            var for_pair_dec = for_condition.GetForPairDec();
            if (for_pair_dec != null)
            {
                // 步进式的for有一个临时变量
                if (for_condition.GetForStepCondition() != null)
                {
                    var start_stat = for_condition.GetForStepCondition().GetForStartStat();
                    if (start_stat != null)
                    {
                        var var_assign_name_dec = for_pair_dec.GetVarAssignNameDec();
                        if (var_assign_name_dec != null)
                        {
                            if (name.Length == 0 || name == var_assign_name_dec.GetElementText())
                                var_dec_list.Add(var_assign_name_dec);
                        }
                    }
                }
                // 迭代式的for有多个临时变量
                else if (for_condition.GetForInCondition() != null)
                {
                    List<ALittleScriptForPairDecElement> pair_dec_list = for_condition.GetForInCondition().GetForPairDecList();
                    pair_dec_list.Insert(0, for_pair_dec);
                    foreach (var pair_dec in pair_dec_list)
                    {
                        var var_assign_name_dec = pair_dec.GetVarAssignNameDec();
                        if (var_assign_name_dec != null)
                        {
                            if (name.Length == 0 || name == var_assign_name_dec.GetElementText())
                                var_dec_list.Add(var_assign_name_dec);
                        }
                    }
                }
            }
        }

        // 根据名称，查找定义表达式名列表
        private static void FindVarAssignNameDecList(ALittleScriptAllExprElement all_expr, List<ALittleScriptVarAssignNameDecElement> var_dec_list, string name)
        {
            ABnfElement parent = all_expr.GetParent();
            List<ALittleScriptAllExprElement> all_expr_list = null;

            // 处理函数体
            if (parent is ALittleScriptMethodBodyDecElement)
            {
                var cur_expr = parent as ALittleScriptMethodBodyDecElement;
                all_expr_list = cur_expr.GetAllExprList();
            }
            // 处理for循环
            else if (parent is ALittleScriptForExprElement || parent is ALittleScriptForBodyElement)
            {
                if (parent is ALittleScriptForBodyElement) parent = parent.GetParent();
                FindVarAssignNameDecList(parent.GetParent() as ALittleScriptAllExprElement, var_dec_list, name);

                var cur_expr = parent as ALittleScriptForExprElement;

                // 获取for内部的表达式
                if (cur_expr.GetForBody() != null)
                    all_expr_list = cur_expr.GetForBody().GetAllExprList();
                if (cur_expr.GetAllExpr() != null)
                {
                    all_expr_list = new List<ALittleScriptAllExprElement>();
                    all_expr_list.Add(cur_expr.GetAllExpr());
                }

                var for_condition = cur_expr.GetForCondition();
                if (for_condition != null)
                    FindVarAssignNameDecList(for_condition, var_dec_list, name);
            }
            // 处理while循环
            else if (parent is ALittleScriptWhileExprElement || parent is ALittleScriptWhileBodyElement)
            {
                if (parent is ALittleScriptWhileBodyElement) parent = parent.GetParent();
                FindVarAssignNameDecList(parent.GetParent() as ALittleScriptAllExprElement, var_dec_list, name);
                var cur_expr = parent as ALittleScriptWhileExprElement;
                if (cur_expr.GetWhileBody() != null)
                    all_expr_list = cur_expr.GetWhileBody().GetAllExprList();
                else if (cur_expr.GetAllExpr() != null)
                {
                    all_expr_list = new List<ALittleScriptAllExprElement>();
                    all_expr_list.Add(cur_expr.GetAllExpr());
                }
            }
            // 处理do while
            else if (parent is ALittleScriptDoWhileExprElement || parent is ALittleScriptDoWhileBodyElement)
            {
                if (parent is ALittleScriptDoWhileBodyElement) parent = parent.GetParent();
                FindVarAssignNameDecList(parent.GetParent() as ALittleScriptAllExprElement, var_dec_list, name);
                var cur_expr = parent as ALittleScriptDoWhileExprElement;
                if (cur_expr.GetDoWhileBody() != null)
                    all_expr_list = cur_expr.GetDoWhileBody().GetAllExprList();
            }
            // 处理 if
            else if (parent is ALittleScriptIfExprElement || parent is ALittleScriptIfBodyElement)
            {
                if (parent is ALittleScriptIfBodyElement) parent = parent.GetParent();
                FindVarAssignNameDecList(parent.GetParent() as ALittleScriptAllExprElement, var_dec_list, name);
                var cur_expr = parent as ALittleScriptIfExprElement;
                if (cur_expr.GetIfBody() != null)
                    all_expr_list = cur_expr.GetIfBody().GetAllExprList();
                else if (cur_expr.GetAllExpr() != null)
                {
                    all_expr_list = new List<ALittleScriptAllExprElement>();
                    all_expr_list.Add(cur_expr.GetAllExpr());
                }
            }
            // 处理 else if
            else if (parent is ALittleScriptElseIfExprElement || parent is ALittleScriptElseIfBodyElement)
            {
                if (parent is ALittleScriptElseIfBodyElement) parent = parent.GetParent();
                FindVarAssignNameDecList(parent.GetParent().GetParent() as ALittleScriptAllExprElement, var_dec_list, name);
                var cur_expr = parent as ALittleScriptElseIfExprElement;
                if (cur_expr.GetElseIfBody() != null)
                    all_expr_list = cur_expr.GetElseIfBody().GetAllExprList();
                else if (cur_expr.GetAllExpr() != null)
                {
                    all_expr_list = new List<ALittleScriptAllExprElement>();
                    all_expr_list.Add(cur_expr.GetAllExpr());
                }
            }
            // 处理 else
            else if (parent is ALittleScriptElseExprElement || parent is ALittleScriptElseBodyElement)
            {
                if (parent is ALittleScriptElseBodyElement) parent = parent.GetParent();
                FindVarAssignNameDecList(parent.GetParent().GetParent() as ALittleScriptAllExprElement, var_dec_list, name);
                var cur_expr = parent as ALittleScriptElseExprElement;
                if (cur_expr.GetElseBody() != null)
                    all_expr_list = cur_expr.GetElseBody().GetAllExprList();
                else if (cur_expr.GetAllExpr() != null)
                {
                    all_expr_list = new List<ALittleScriptAllExprElement>();
                    all_expr_list.Add(cur_expr.GetAllExpr());
                }
            }
            // 处理 wrap
            else if (parent is ALittleScriptWrapExprElement)
            {
                FindVarAssignNameDecList(parent.GetParent() as ALittleScriptAllExprElement, var_dec_list, name);
                var cur_expr = parent as ALittleScriptWrapExprElement;
                all_expr_list = cur_expr.GetAllExprList();
            }

            if (all_expr_list == null) return;

            foreach (var expr in all_expr_list)
            {
                // 如果已经遍历到当前，那么就可以返回了
                if (expr == all_expr) return;

                // 获取定义表达式
                var var_assign_expr = expr.GetVarAssignExpr();
                if (var_assign_expr == null) continue;

                // 获取变量名列表
                var var_assign_dec_list = var_assign_expr.GetVarAssignDecList();
                foreach (var var_assign_dec in var_assign_dec_list)
                {
                    var var_assign_name_dec = var_assign_dec.GetVarAssignNameDec();
                    if (var_assign_name_dec == null) continue;
                    if (name.Length == 0 || name == var_assign_name_dec.GetElementText())
                        var_dec_list.Add(var_assign_name_dec);
                }
            }
        }

        // 检查迭代函数
        public static bool IsPairsFunction(List<ABnfGuess> guess_list)
        {
            // guess_list长度必须是3
            if (guess_list.Count != 3) return false;
            // 第一个必须是函数
            if (!(guess_list[0] is ALittleScriptGuessFunctor)) return false;
            var guess = guess_list[0] as ALittleScriptGuessFunctor;
            // 函数不能带await
            if (guess.await_modifier) return false;
            // 函数不能带proto
            if (guess.proto != null) return false;
            // 函数不能是模板函数
            if (guess.template_param_list.Count > 0) return false;
            // 函数参数必须是2个
            if (guess.param_list.Count != 2) return false;
            if (guess.param_nullable_list.Count != 2) return false;
            // 函数的参数不能带Nullable
            if (guess.param_nullable_list[0]) return false;
            if (guess.param_nullable_list[1]) return false;
            // 函数不能有参数占位符
            if (guess.param_tail != null) return false;
            // 函数必须有返回值，可以是任意个，这个也表示for的变量列表的数量
            if (guess.return_list.Count > 0) return false;
            // 函数不能有返回值占位符
            if (guess.return_tail != null) return false;
            // 函数的第一个参数必须和guess_list第二个参数一致
            if (guess.param_list[0].GetValue() != guess_list[1].GetValue()) return false;
            // 函数的第二个参数必须和guess_list第二个参数一致
            if (guess.param_list[1].GetValue() != guess_list[2].GetValue()) return false;
            return true;
        }

        // 计算表达式需要使用什么样的变量方式
        public static ABnfGuessError CalcPairsTypeForLua(ALittleScriptValueStatElement value_stat, out string result)
        {
            result = "";
            var error = value_stat.GuessTypes(out List<ABnfGuess> guess_list);
            if (error != null) return error;

            // 必出是模板容器
            if (guess_list.Count == 1 && guess_list[0] is ALittleScriptGuessList)
            {
                result = "___ipairs";
                return null;
            }
            else if (guess_list.Count == 1 && guess_list[0] is ALittleScriptGuessMap)
            {
                result = "___pairs";
                return null;
            }

            // 已经是迭代函数了，就不需要包围修饰
            if (IsPairsFunction(guess_list)) return null;

            return new ABnfGuessError(value_stat, "该表达式不能遍历");
        }

        // 计算表达式在for中使用in还是of
        public static ABnfGuessError CalcPairsTypeForJavaScript(ALittleScriptValueStatElement value_stat, out string result, out bool is_native)
        {
            result = "Other";
            is_native = false;
            var error = value_stat.GuessTypes(out List<ABnfGuess> guess_list);
            if (error != null) return error;

            // 必出是模板容器
            if (guess_list.Count == 1 && guess_list[0] is ALittleScriptGuessList)
            {
                result = "List";
                is_native = (guess_list[0] as ALittleScriptGuessList).is_native;
                return null;
            }
            else if (guess_list.Count == 1 && guess_list[0] is ALittleScriptGuessMap)
            {
                if ((guess_list[0] as ALittleScriptGuessMap).key_type is ALittleScriptGuessString)
                    result = "Object";
                else
                    result = "Map";
                return null;
            }

            // 已经是迭代函数了，就不需要包围修饰
            if (IsPairsFunction(guess_list)) return null;

            return new ABnfGuessError(value_stat, "该表达式不能遍历");
        }

        // 判断 parent是否是child的父类
        public static ABnfGuessError IsClassSuper(ALittleScriptClassDecElement child, string parent, out bool result)
        {
            result = false;
            // 获取继承
            var extends_dec = child.GetClassExtendsDec();
            if (extends_dec == null) return null;

            // 获取类名
            var name_dec = extends_dec.GetClassNameDec();
            if (name_dec == null) return null;

            // 获取类型
            var error = name_dec.GuessType(out ABnfGuess guess);
            if (error != null) return error;

            // 继续判断父类的父类
            var guess_class = guess as ALittleScriptGuessClass;
            if (guess_class == null) return null;

            // 检查是否一致
            if (guess_class.GetValueWithoutConst() == parent)
            {
                result = true;
                return null;
            }

            return IsClassSuper(guess_class.class_dec, parent, out result);
        }

        // 判断 parent是否是child的父类
        public static ABnfGuessError IsStructSuper(ABnfElement child, string parent, out bool result)
        {
            result = false;

            var struct_child = child as ALittleScriptStructDecElement;
            if (struct_child == null) return null;

            // 获取继承
            var extends_dec = struct_child.GetStructExtendsDec();
            if (extends_dec == null) return null;

            // 获取结构体名
            var name_dec = extends_dec.GetStructNameDec();
            if (name_dec == null) return null;

            // 获取类型
            var error = name_dec.GuessType(out ABnfGuess guess);
            if (error != null) return error;

            // 继续判断父结构体的父结构体
            var guess_struct = guess as ALittleScriptGuessStruct;
            if (guess_struct == null) return null;

            // 判断是否一致
            if (guess_struct.GetValueWithoutConst() == parent)
            {
                result = true;
                return null;
            }

            return IsStructSuper(guess_struct.struct_dec, parent, out result);
        }

        // 获取目标根路径
        public static string CalcRootFullPath(string project_path, string ext)
        {
            string out_pre = "";
            if (ext == "js") out_pre = "JS";
            return project_path + out_pre + "Script\\";
        }

        // 获取目标文件路径
        public static string CalcTargetFullPath(string project_path, string ali_full_path, string ext, out string error)
        {
            error = null;
            string ali_rel_path = Path.ChangeExtension(ali_full_path.Substring(project_path.Length), ext);
            if (!ali_rel_path.StartsWith("src\\"))
            {
                error = "请把代码文件工程目录下的src文件夹中:" + project_path + "src\\";
                return null;
            }

            return CalcRootFullPath(project_path, ext) + ali_rel_path.Substring("src\\".Length);
        }

        // 判断ValueStat
        public static ABnfGuessError CalcReturnCount(ALittleScriptValueStatElement value_stat, out int count, out List<ABnfGuess> guess_list)
        {
            count = 0;
            // 获取右边表达式的
            var guess_error = value_stat.GuessTypes(out guess_list);
            if (guess_error != null) return guess_error;

            count = guess_list.Count;
            if (guess_list.Count > 0 && guess_list[guess_list.Count - 1] is ALittleScriptGuessReturnTail)
                count = -1;

            return null;
        }
    }
}
