
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Media;

namespace ALittle
{
    public class ALittleScriptIndex
    {
        public static ALittleScriptIndex inst = new ALittleScriptIndex();

        // 保存关键的元素对象，用于快速语法树解析
        // Key1:文件对象，Key2:元素对象，Value:类型
        protected Dictionary<ABnfFile, Dictionary<ABnfElement, List<ABnfGuess>>> m_guess_type_map
            = new Dictionary<ABnfFile, Dictionary<ABnfElement, List<ABnfGuess>>>();
        protected Dictionary<ABnfFile, Dictionary<ABnfElement, ABnfGuessError>> m_guess_error_map
            = new Dictionary<ABnfFile, Dictionary<ABnfElement, ABnfGuessError>>();
        // Key1:文件对象，Key2:名称，Value:数据
        protected Dictionary<ABnfFile, Dictionary<string, ALittleScriptClassData>> m_class_data_map
            = new Dictionary<ABnfFile, Dictionary<string, ALittleScriptClassData>>();
        protected Dictionary<ABnfFile, Dictionary<string, ALittleScriptStructData>> m_struct_data_map
            = new Dictionary<ABnfFile, Dictionary<string, ALittleScriptStructData>>();
        protected Dictionary<ABnfFile, Dictionary<string, ALittleScriptEnumData>> m_enum_data_map
            = new Dictionary<ABnfFile, Dictionary<string, ALittleScriptEnumData>>();

        // 按命名域来划分
        // string 表示命名域
        protected Dictionary<string, Dictionary<ALittleScriptNamespaceNameDecElement, ALittleScriptAccessData>> m_all_data_map
            = new Dictionary<string, Dictionary<ALittleScriptNamespaceNameDecElement, ALittleScriptAccessData>>();
        // 根据开放权限来划分
        // string 表示命名域
        protected Dictionary<string, ALittleScriptAccessData> m_global_access_map
            = new Dictionary<string, ALittleScriptAccessData>();
        protected Dictionary<string, ALittleScriptAccessData> m_namespace_access_map
            = new Dictionary<string, ALittleScriptAccessData>();
        protected Dictionary<ABnfFile, ALittleScriptAccessData> m_file_access_map
            = new Dictionary<ABnfFile, ALittleScriptAccessData>();

        public ABnfGuess sIntGuess;
        public ABnfGuess sDoubleGuess;
        public ABnfGuess sStringGuess;
        public ABnfGuess sBoolGuess;
        public ABnfGuess sLongGuess;
        public ABnfGuess sAnyGuess;

        public ABnfGuess sConstIntGuess;
        public ABnfGuess sConstDoubleGuess;
        public ABnfGuess sConstStringGuess;
        public ABnfGuess sConstBoolGuess;
        public ABnfGuess sConstLongGuess;
        public ABnfGuess sConstAnyGuess;

        public Dictionary<string, List<ABnfGuess>> sPrimitiveGuessListMap;
        public Dictionary<string, ABnfGuess> sPrimitiveGuessMap;

        public List<ABnfGuess> sConstNullGuess;

        // 控制语句
        public HashSet<string> sCtrlKeyWord = new HashSet<string>();

        // 图标
        public ImageSource sClassIcon;
        public ImageSource sTemplateIcon;
        public ImageSource sNamespaceIcon;
        public ImageSource sStructIcon;
        public ImageSource sEnumIcon;
        public ImageSource sInstanceIcon;
        public ImageSource sStaticMethodIcon;
        public ImageSource sGlobalMethodIcon;
        public ImageSource sMemberMethodIcon;
        public ImageSource sFieldMethodIcon;
        public ImageSource sParamIcon;
        public ImageSource sVariableIcon;
        public ImageSource sPropertyIcon;
        public ImageSource sLanguageIcon;
        public Icon sFileIcon;

        public ALittleScriptIndex()
        {
            // 基础变量
            sPrimitiveGuessMap = new Dictionary<string, ABnfGuess>();
            sPrimitiveGuessListMap = new Dictionary<string, List<ABnfGuess>>();
            List<ABnfGuess> tmp;
            tmp = new List<ABnfGuess>(); sIntGuess = new ALittleScriptGuessInt(false); tmp.Add(sIntGuess); sPrimitiveGuessListMap.Add(sIntGuess.GetValue(), tmp); sPrimitiveGuessMap.Add(sIntGuess.GetValue(), sIntGuess);
            tmp = new List<ABnfGuess>(); sDoubleGuess = new ALittleScriptGuessDouble(false); tmp.Add(sDoubleGuess); sPrimitiveGuessListMap.Add(sDoubleGuess.GetValue(), tmp); sPrimitiveGuessMap.Add(sDoubleGuess.GetValue(), sDoubleGuess);
            tmp = new List<ABnfGuess>(); sStringGuess = new ALittleScriptGuessString(false); tmp.Add(sStringGuess); sPrimitiveGuessListMap.Add(sStringGuess.GetValue(), tmp); sPrimitiveGuessMap.Add(sStringGuess.GetValue(), sStringGuess);
            tmp = new List<ABnfGuess>(); sBoolGuess = new ALittleScriptGuessBool(false); tmp.Add(sBoolGuess); sPrimitiveGuessListMap.Add(sBoolGuess.GetValue(), tmp); sPrimitiveGuessMap.Add(sBoolGuess.GetValue(), sBoolGuess);
            tmp = new List<ABnfGuess>(); sLongGuess = new ALittleScriptGuessLong(false); tmp.Add(sLongGuess); sPrimitiveGuessListMap.Add(sLongGuess.GetValue(), tmp); sPrimitiveGuessMap.Add(sLongGuess.GetValue(), sLongGuess);
            tmp = new List<ABnfGuess>(); sAnyGuess = new ALittleScriptGuessAny(false); tmp.Add(sAnyGuess); sPrimitiveGuessListMap.Add(sAnyGuess.GetValue(), tmp); sPrimitiveGuessMap.Add(sAnyGuess.GetValue(), sAnyGuess);

            tmp = new List<ABnfGuess>(); sConstIntGuess = new ALittleScriptGuessInt(true); tmp.Add(sConstIntGuess); sPrimitiveGuessListMap.Add(sConstIntGuess.GetValue(), tmp); sPrimitiveGuessMap.Add(sConstIntGuess.GetValue(), sConstIntGuess);
            tmp = new List<ABnfGuess>(); sConstDoubleGuess = new ALittleScriptGuessDouble(true); tmp.Add(sConstDoubleGuess); sPrimitiveGuessListMap.Add(sConstDoubleGuess.GetValue(), tmp); sPrimitiveGuessMap.Add(sConstDoubleGuess.GetValue(), sConstDoubleGuess);
            tmp = new List<ABnfGuess>(); sConstStringGuess = new ALittleScriptGuessString(true); tmp.Add(sConstStringGuess); sPrimitiveGuessListMap.Add(sConstStringGuess.GetValue(), tmp); sPrimitiveGuessMap.Add(sConstStringGuess.GetValue(), sConstStringGuess);
            tmp = new List<ABnfGuess>(); sConstBoolGuess = new ALittleScriptGuessBool(true); tmp.Add(sConstBoolGuess); sPrimitiveGuessListMap.Add(sConstBoolGuess.GetValue(), tmp); sPrimitiveGuessMap.Add(sConstBoolGuess.GetValue(), sConstBoolGuess);
            tmp = new List<ABnfGuess>(); sConstLongGuess = new ALittleScriptGuessLong(true); tmp.Add(sConstLongGuess); sPrimitiveGuessListMap.Add(sConstLongGuess.GetValue(), tmp); sPrimitiveGuessMap.Add(sConstLongGuess.GetValue(), sConstLongGuess);
            tmp = new List<ABnfGuess>(); sConstAnyGuess = new ALittleScriptGuessAny(true); tmp.Add(sConstAnyGuess); sPrimitiveGuessListMap.Add(sConstAnyGuess.GetValue(), tmp); sPrimitiveGuessMap.Add(sConstAnyGuess.GetValue(), sConstAnyGuess);

            // null常量
            sConstNullGuess = new List<ABnfGuess>();
            sConstNullGuess.Add(new ALittleScriptGuessConst("null"));

            // 控制关键字
            sCtrlKeyWord.Add("if");
            sCtrlKeyWord.Add("elseif");
            sCtrlKeyWord.Add("else");
            sCtrlKeyWord.Add("do");
            sCtrlKeyWord.Add("while");
            sCtrlKeyWord.Add("return");
            sCtrlKeyWord.Add("break");
            sCtrlKeyWord.Add("continue");
            sCtrlKeyWord.Add("for");
            sCtrlKeyWord.Add("in");
            sCtrlKeyWord.Add("throw");
            sCtrlKeyWord.Add("assert");
        }

        public void Start()
        {
            sClassIcon = ALanguageUtility.ToImageSource(Properties.Resources.ALittleScriptClassIcon);
            sTemplateIcon = ALanguageUtility.ToImageSource(Properties.Resources.ALittleScriptTemplateIcon);
            sNamespaceIcon = ALanguageUtility.ToImageSource(Properties.Resources.ALittleScriptNamespaceIcon);
            sStructIcon = ALanguageUtility.ToImageSource(Properties.Resources.ALittleScriptStructIcon);
            sEnumIcon = ALanguageUtility.ToImageSource(Properties.Resources.ALittleScriptEnumIcon);
            sInstanceIcon = ALanguageUtility.ToImageSource(Properties.Resources.ALittleScriptInstanceIcon);
            sStaticMethodIcon = ALanguageUtility.ToImageSource(Properties.Resources.ALittleScriptStaticIcon);
            sGlobalMethodIcon = ALanguageUtility.ToImageSource(Properties.Resources.ALittleScriptStaticIcon);
            sMemberMethodIcon = ALanguageUtility.ToImageSource(Properties.Resources.ALittleScriptMethodIcon);
            sFieldMethodIcon = ALanguageUtility.ToImageSource(Properties.Resources.ALittleScriptFieldIcon);
            sParamIcon = ALanguageUtility.ToImageSource(Properties.Resources.ALittleScriptParamIcon);
            sVariableIcon = ALanguageUtility.ToImageSource(Properties.Resources.ALittleScriptVariableIcon);
            sPropertyIcon = ALanguageUtility.ToImageSource(Properties.Resources.ALittleScriptPropertyIcon);
            sLanguageIcon = ALanguageUtility.ToImageSource(Properties.Resources.ALittleScriptNamespaceIcon);
            sFileIcon = Properties.Resources.ALittleScriptFieldIcon;
        }

        public List<ABnfGuess> GetGuessTypeList(ABnfElement element)
        {
            if (!m_guess_type_map.TryGetValue(element.GetFile(), out Dictionary<ABnfElement, List<ABnfGuess>> map))
                return null;
            if (!map.TryGetValue(element, out List<ABnfGuess> list))
                return null;
            return list;
        }

        public void AddGuessTypeList(ABnfElement element, List<ABnfGuess> guess_type_list)
        {
            {
                if (!m_guess_type_map.TryGetValue(element.GetFile(), out Dictionary<ABnfElement, List<ABnfGuess>> map))
                {
                    map = new Dictionary<ABnfElement, List<ABnfGuess>>();
                    m_guess_type_map.Add(element.GetFile(), map);
                }
                if (map.ContainsKey(element)) map.Remove(element);
                map.Add(element, guess_type_list);
            }


            {
                if (m_guess_error_map.TryGetValue(element.GetFile(), out Dictionary<ABnfElement, ABnfGuessError> map))
                {
                    if (map.ContainsKey(element))
                        map.Remove(element);
                }
            }
        }

        public ABnfGuessError GetGuessError(ABnfElement element)
        {
            if (!m_guess_error_map.TryGetValue(element.GetFile(), out Dictionary<ABnfElement, ABnfGuessError> map))
                return null;
            if (!map.TryGetValue(element, out ABnfGuessError error))
                return null;
            return error;
        }

        public void AddGuessError(ABnfElement element, ABnfGuessError error)
        {
            {
                if (!m_guess_error_map.TryGetValue(element.GetFile(), out Dictionary<ABnfElement, ABnfGuessError> map))
                {
                    map = new Dictionary<ABnfElement, ABnfGuessError>();
                    m_guess_error_map.Add(element.GetFile(), map);
                }
                if (map.ContainsKey(element)) map.Remove(element);
                map.Add(element, error);
            }

            {
                if (m_guess_type_map.TryGetValue(element.GetFile(), out Dictionary<ABnfElement, List<ABnfGuess>> map))
                {
                    if (map.ContainsKey(element)) map.Remove(element);
                }
            }
        }

        public Dictionary<string, ALittleScriptNamespaceNameDecElement> FindNamespaceNameDecList(string namespace_name)
        {
            var result = new Dictionary<string, ALittleScriptNamespaceNameDecElement>();

            if (namespace_name.Length == 0)
            {
                foreach (var pair in m_all_data_map)
                {
                    foreach (var key in pair.Value.Keys)
                    {
                        var text = key.GetElementText();
                        if (!result.ContainsKey(text)) result.Add(text, key);
                    }
                }
            }
            else
            {
                if (m_all_data_map.TryGetValue(namespace_name, out Dictionary<ALittleScriptNamespaceNameDecElement, ALittleScriptAccessData> map))
                {
                    foreach (var key in map.Keys)
                    {
                        var text = key.GetElementText();
                        if (!result.ContainsKey(text)) result.Add(text, key);
                    }
                }
            }

            return result;
        }

        public List<ABnfElement> FindALittleNameDecList(ALittleScriptUtility.ABnfElementType type
            , ABnfFile file, string namespace_name, string name, bool find_in_global)
        {
            var result = new List<ABnfElement>();
            ALittleScriptAccessData data;

            // 查本文件的
            var file_namespace_name = ALittleScriptUtility.GetNamespaceName(file);
            if (file_namespace_name == namespace_name)
            {
                if (m_file_access_map.TryGetValue(file, out data))
                    data.FindNameDecList(type, name, result);
            }

            // 查本命名域的
            if (file_namespace_name == namespace_name)
            {
                if (m_namespace_access_map.TryGetValue(namespace_name, out data))
                    data.FindNameDecList(type, name, result);
            }

            // 查全局下
            if (find_in_global)
            {
                if (type == ALittleScriptUtility.ABnfElementType.INSTANCE_NAME)
                {
                    foreach (var pair in m_global_access_map)
                        pair.Value.FindNameDecList(type, name, result);
                }
                else
                {
                    if (m_global_access_map.TryGetValue(namespace_name, out data))
                        data.FindNameDecList(type, name, result);
                }
            }

            return result;
        }

        public ABnfElement FindALittleNameDec(ALittleScriptUtility.ABnfElementType type
            , ABnfFile file, string namespace_name, string name, bool find_in_global)
        {
            var result = FindALittleNameDecList(type, file, namespace_name, name, find_in_global);
            if (result == null || result.Count == 0) return null;
            return result[0];
        }

        public ABnfGuessError FindALittleStructGuessList(string namespace_name, string name, out List<ABnfGuess> guess_list)
        {
            var element = FindALittleNameDec(ALittleScriptUtility.ABnfElementType.STRUCT_NAME, null, namespace_name, name, true);
            if (element is ALittleScriptStructNameDecElement)
                return element.GuessTypes(out guess_list);
            guess_list = new List<ABnfGuess>();
            return null;
        }

        public ABnfGuessError FindALittleStructGuess(string namespace_name, string name, out ABnfGuess guess)
        {
            var element = FindALittleNameDec(ALittleScriptUtility.ABnfElementType.STRUCT_NAME, null, namespace_name, name, true);
            if (element is ALittleScriptStructNameDecElement)
                return element.GuessType(out guess);
            guess = null;
            return null;
        }

        public ABnfGuessError FindALittleClassGuessList(string namespace_name, string name, out List<ABnfGuess> guess_list)
        {
            var element = FindALittleNameDec(ALittleScriptUtility.ABnfElementType.CLASS_NAME, null, namespace_name, name, true);
            if (element is ALittleScriptClassNameDecElement)
                return element.GuessTypes(out guess_list);
            guess_list = new List<ABnfGuess>();
            return null;
        }

        public void FindClassAttrList(ALittleScriptClassDecElement dec
            , int access_level, ALittleScriptUtility.ClassAttrType attr_type, string name, List<ABnfElement> result)
        {
            var name_dec = dec.GetClassNameDec();
            if (name_dec == null) return;

            if (!m_class_data_map.TryGetValue(dec.GetFile(), out Dictionary<string, ALittleScriptClassData> map))
                return;
            if (!map.TryGetValue(name_dec.GetElementText(), out ALittleScriptClassData data))
                return;
            data.FindClassAttrList(access_level, attr_type, name, result);
        }

        public ABnfElement FindClassAttr(ALittleScriptClassDecElement dec
            , int access_level, ALittleScriptUtility.ClassAttrType attr_type, string name)
        {
            var result = new List<ABnfElement>();
            FindClassAttrList(dec, access_level, attr_type, name, result);
            if (result.Count == 0) return null;
            return result[0];
        }

        // 添加类索引数据
        private void AddClassData(ALittleScriptClassDecElement dec)
        {
            var name_dec = dec.GetClassNameDec();
            if (name_dec == null) return;

            if (!m_class_data_map.TryGetValue(dec.GetFile(), out Dictionary<string, ALittleScriptClassData> map))
            {
                map = new Dictionary<string, ALittleScriptClassData>();
                m_class_data_map.Add(dec.GetFile(), map);
            }
            var class_data = new ALittleScriptClassData();
            string name = name_dec.GetElementText();
            if (map.ContainsKey(name)) map.Remove(name);
            map.Add(name, class_data);

            var template_dec = dec.GetTemplateDec();
            if (template_dec != null)
                class_data.AddClassChildDec(template_dec);

            var body_dec = dec.GetClassBodyDec();
            if (body_dec == null) return;

            var element_dec_list = body_dec.GetClassElementDecList();
            foreach (var element_dec in element_dec_list)
                class_data.AddClassChildDec(element_dec);
        }

        // 获取类索引数据
        public ALittleScriptClassData GetClassData(ALittleScriptClassDecElement dec)
        {
            var name_dec = dec.GetClassNameDec();
            if (name_dec == null) return null;
            if (!m_class_data_map.TryGetValue(dec.GetFile(), out Dictionary<string, ALittleScriptClassData> map)) return null;
            if (!map.TryGetValue(name_dec.GetElementText(), out ALittleScriptClassData data)) return null;
            return data;
        }

        // 添加结构体数据
        private void AddStructData(ALittleScriptStructDecElement dec)
        {
            var name_dec = dec.GetStructNameDec();
            if (name_dec == null) return;

            if (!m_struct_data_map.TryGetValue(dec.GetFile(), out Dictionary<string, ALittleScriptStructData> map))
            {
                map = new Dictionary<string, ALittleScriptStructData>();
                m_struct_data_map.Add(dec.GetFile(), map);
            }
            var struct_data = new ALittleScriptStructData();
            string name = name_dec.GetElementText();
            if (map.ContainsKey(name)) map.Remove(name);
            map.Add(name, struct_data);

            var body_dec = dec.GetStructBodyDec();
            if (body_dec == null) return;

            var var_dec_list = body_dec.GetStructVarDecList();
            foreach (var var_dec in var_dec_list)
                struct_data.AddVarDec(var_dec);
        }

        // 获取结构体数据
        public ALittleScriptStructData GetStructData(ALittleScriptStructDecElement dec)
        {
            var name_dec = dec.GetStructNameDec();
            if (name_dec == null) return null;
            if (!m_struct_data_map.TryGetValue(dec.GetFile(), out Dictionary<string, ALittleScriptStructData> map)) return null;
            if (!map.TryGetValue(name_dec.GetElementText(), out ALittleScriptStructData data)) return null;
            return data;
        }

        // 添加枚举数据
        private void AddEnumData(ALittleScriptEnumDecElement dec)
        {
            var name_dec = dec.GetEnumNameDec();
            if (name_dec == null) return;

            if (!m_enum_data_map.TryGetValue(dec.GetFile(), out Dictionary<string, ALittleScriptEnumData> map))
            {
                map = new Dictionary<string, ALittleScriptEnumData>();
                m_enum_data_map.Add(dec.GetFile(), map);
            }
            var enum_data = new ALittleScriptEnumData();
            string name = name_dec.GetElementText();
            if (map.ContainsKey(name)) map.Remove(name);
            map.Add(name, enum_data);

            var body_dec = dec.GetEnumBodyDec();
            if (body_dec == null) return;

            var var_dec_list = body_dec.GetEnumVarDecList();
            foreach (var var_dec in var_dec_list)
                enum_data.AddVarDec(var_dec);
        }

        // 获取枚举数据
        public ALittleScriptEnumData GetEnumData(ALittleScriptEnumDecElement dec)
        {
            var name_dec = dec.GetEnumNameDec();
            if (name_dec == null) return null;
            if (!m_enum_data_map.TryGetValue(dec.GetFile(), out Dictionary<string, ALittleScriptEnumData> map)) return null;
            if (!map.TryGetValue(name_dec.GetElementText(), out ALittleScriptEnumData data)) return null;
            return data;
        }

        // 添加命名域
        public void AddRoot(ALittleScriptRootElement root)
        {
            // 清除标记
            m_guess_type_map.Remove(root.GetFile());
            m_guess_error_map.Remove(root.GetFile());
            m_class_data_map.Remove(root.GetFile());
            m_struct_data_map.Remove(root.GetFile());
            m_enum_data_map.Remove(root.GetFile());

            var namespace_dec = root.GetNamespaceDec();
            if (namespace_dec == null) return;
            var namespace_name_dec = namespace_dec.GetNamespaceNameDec();
            if (namespace_name_dec == null) return;

            // 获取命名域名
            string name = namespace_name_dec.GetElementText();

            if (!m_all_data_map.TryGetValue(name, out Dictionary<ALittleScriptNamespaceNameDecElement, ALittleScriptAccessData> all_data_map))
            {
                all_data_map = new Dictionary<ALittleScriptNamespaceNameDecElement, ALittleScriptAccessData>();
                m_all_data_map.Add(name, all_data_map);
            }
            ALittleScriptAccessData all_access_data = new ALittleScriptAccessData();
            if (all_data_map.ContainsKey(namespace_name_dec)) all_data_map.Remove(namespace_name_dec);
            all_data_map.Add(namespace_name_dec, all_access_data);

            if (!m_global_access_map.TryGetValue(name, out ALittleScriptAccessData global_access_data))
            {
                global_access_data = new ALittleScriptAccessData();
                m_global_access_map.Add(name, global_access_data);
            }
            if (!m_namespace_access_map.TryGetValue(name, out ALittleScriptAccessData namespace_access_data))
            {
                namespace_access_data = new ALittleScriptAccessData();
                m_namespace_access_map.Add(name, namespace_access_data);
            }
            if (!m_file_access_map.TryGetValue(root.GetFile(), out ALittleScriptAccessData file_access_data))
            {
                file_access_data = new ALittleScriptAccessData();
                m_file_access_map.Add(root.GetFile(), file_access_data);
            }

            var element_dec_list = namespace_dec.GetNamespaceElementDecList();
            foreach (var child in element_dec_list)
            {
                // 添加类
                if (child.GetClassDec() != null)
                {
                    var class_dec = child.GetClassDec();
                    var name_dec = class_dec.GetClassNameDec();
                    if (name_dec == null) continue;

                    // 添加类数据
                    AddClassData(class_dec);
                    // 添加到全权限
                    all_access_data.AddNameDec(name_dec);
                    // 按访问权限划分
                    var access_type = ALittleScriptUtility.CalcAccessType(child.GetModifierList());
                    if (access_type == ALittleScriptUtility.ClassAccessType.PUBLIC)
                        global_access_data.AddNameDec(name_dec);
                    else if (access_type == ALittleScriptUtility.ClassAccessType.PROTECTED)
                        namespace_access_data.AddNameDec(name_dec);
                    else if (access_type == ALittleScriptUtility.ClassAccessType.PRIVATE)
                        file_access_data.AddNameDec(name_dec);
                }
                // 添加枚举
                else if (child.GetEnumDec() != null)
                {
                    var enum_dec = child.GetEnumDec();
                    var name_dec = enum_dec.GetEnumNameDec();
                    if (name_dec == null) continue;

                    //  添加枚举数据
                    AddEnumData(enum_dec);
                    // 添加到全权限
                    all_access_data.AddNameDec(name_dec);
                    // 按访问权限划分
                    var access_type = ALittleScriptUtility.CalcAccessType(child.GetModifierList());
                    if (access_type == ALittleScriptUtility.ClassAccessType.PUBLIC)
                        global_access_data.AddNameDec(name_dec);
                    else if (access_type == ALittleScriptUtility.ClassAccessType.PROTECTED)
                        namespace_access_data.AddNameDec(name_dec);
                    else if (access_type == ALittleScriptUtility.ClassAccessType.PRIVATE)
                        file_access_data.AddNameDec(name_dec);
                }
                // 添加结构体
                else if (child.GetStructDec() != null)
                {
                    var struct_dec = child.GetStructDec();
                    var name_dec = struct_dec.GetStructNameDec();
                    if (name_dec == null) continue;

                    //  添加结构体数据
                    AddStructData(struct_dec);
                    // 添加到全权限
                    all_access_data.AddNameDec(name_dec);
                    // 按访问权限划分
                    var access_type = ALittleScriptUtility.CalcAccessType(child.GetModifierList());
                    if (access_type == ALittleScriptUtility.ClassAccessType.PUBLIC)
                        global_access_data.AddNameDec(name_dec);
                    else if (access_type == ALittleScriptUtility.ClassAccessType.PROTECTED)
                        namespace_access_data.AddNameDec(name_dec);
                    else if (access_type == ALittleScriptUtility.ClassAccessType.PRIVATE)
                        file_access_data.AddNameDec(name_dec);
                }
                // 添加全局函数
                else if (child.GetGlobalMethodDec() != null)
                {
                    var method_dec = child.GetGlobalMethodDec();
                    var name_dec = method_dec.GetMethodNameDec();
                    if (name_dec == null) continue;

                    // 添加到全权限
                    all_access_data.AddNameDec(name_dec);
                    // 按访问权限划分
                    var access_type = ALittleScriptUtility.CalcAccessType(child.GetModifierList());
                    if (access_type == ALittleScriptUtility.ClassAccessType.PUBLIC)
                        global_access_data.AddNameDec(name_dec);
                    else if (access_type == ALittleScriptUtility.ClassAccessType.PROTECTED)
                        namespace_access_data.AddNameDec(name_dec);
                    else if (access_type == ALittleScriptUtility.ClassAccessType.PRIVATE)
                        file_access_data.AddNameDec(name_dec);
                }
                // 添加单例
                else if (child.GetInstanceDec() != null)
                {
                    var instance_dec = child.GetInstanceDec();
                    var access_type = ALittleScriptUtility.CalcAccessType(child.GetModifierList());

                    var var_assign_expr = instance_dec.GetVarAssignExpr();
                    if (var_assign_expr == null) continue;
                    var var_assign_dec_list = var_assign_expr.GetVarAssignDecList();
                    foreach (var var_assign_dec in var_assign_dec_list)
                    {
                        var name_dec = var_assign_dec.GetVarAssignNameDec();
                        if (name_dec == null) continue;

                        // 添加到全权限
                        all_access_data.AddNameDec(name_dec);
                        // 按访问权限划分
                        if (access_type == ALittleScriptUtility.ClassAccessType.PUBLIC)
                            global_access_data.AddNameDec(name_dec);
                        else if (access_type == ALittleScriptUtility.ClassAccessType.PROTECTED)
                            namespace_access_data.AddNameDec(name_dec);
                        else if (access_type == ALittleScriptUtility.ClassAccessType.PRIVATE)
                            file_access_data.AddNameDec(name_dec);
                    }
                }
                // 添加using
                else if (child.GetUsingDec() != null)
                {
                    var using_dec = child.GetUsingDec();
                    var name_dec = using_dec.GetUsingNameDec();
                    if (name_dec == null) continue;

                    // 添加到全权限
                    all_access_data.AddNameDec(name_dec);
                    // 按访问权限划分
                    var access_type = ALittleScriptUtility.CalcAccessType(child.GetModifierList());
                    if (access_type == ALittleScriptUtility.ClassAccessType.PUBLIC)
                        global_access_data.AddNameDec(name_dec);
                    else if (access_type == ALittleScriptUtility.ClassAccessType.PROTECTED)
                        namespace_access_data.AddNameDec(name_dec);
                    else if (access_type == ALittleScriptUtility.ClassAccessType.PRIVATE)
                        file_access_data.AddNameDec(name_dec);
                }
            }
        }

        // 移除命名域
        public void RemoveRoot(ALittleScriptRootElement root)
        {
            // 清除标记
            m_guess_type_map.Remove(root.GetFile());
            m_guess_error_map.Remove(root.GetFile());
            m_class_data_map.Remove(root.GetFile());
            m_struct_data_map.Remove(root.GetFile());
            m_enum_data_map.Remove(root.GetFile());

            var namespace_dec = root.GetNamespaceDec();
            if (namespace_dec == null) return;
            var namespace_name_dec = namespace_dec.GetNamespaceNameDec();
            if (namespace_name_dec == null) return;

            var name = namespace_name_dec.GetElementText();

            if (!m_all_data_map.TryGetValue(name, out Dictionary<ALittleScriptNamespaceNameDecElement, ALittleScriptAccessData> all_data_map))
                return;
            if (!all_data_map.TryGetValue(namespace_name_dec, out ALittleScriptAccessData all_access_data))
                return;
            if (all_data_map.ContainsKey(namespace_name_dec)) all_data_map.Remove(namespace_name_dec);
            if (all_data_map.Count == 0) m_all_data_map.Remove(name);

            m_global_access_map.TryGetValue(name, out ALittleScriptAccessData global_access_data);
            m_namespace_access_map.TryGetValue(name, out ALittleScriptAccessData namespace_access_data);
            m_file_access_map.TryGetValue(root.GetFile(), out ALittleScriptAccessData file_access_data);

            foreach (var pair in all_access_data.GetElementMap())
            {
                foreach (var element_pair in pair.Value)
                {
                    foreach (var name_dec in element_pair.Value)
                    {
                        if (global_access_data != null) global_access_data.RemoveNameDec(name_dec);
                        if (namespace_access_data != null) namespace_access_data.RemoveNameDec(name_dec);
                        if (file_access_data != null) file_access_data.RemoveNameDec(name_dec);
                    }
                }
            }
        }
    }
}
