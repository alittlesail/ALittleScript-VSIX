
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptClassCtorDecReference : ALittleScriptReferenceTemplate<ALittleScriptClassCtorDecElement>
    {
        public ALittleScriptClassCtorDecReference(ABnfElement element) : base(element) { }

        public override ABnfElement GotoDefinition()
        {
            var class_dec = ALittleScriptUtility.FindClassDecFromParent(m_element);
            if (class_dec == null) return null;

            var class_extends_dec = ALittleScriptUtility.FindClassExtends(class_dec);
            if (class_extends_dec == null) return null;

            var extends_ctor_dec = ALittleScriptUtility.FindFirstCtorDecFromExtends(class_extends_dec, 100);
            if (extends_ctor_dec == null) return null;

            return extends_ctor_dec.GetKey();
        }

        public override ABnfGuessError CheckError()
        {
            var class_dec = ALittleScriptUtility.FindClassDecFromParent(m_element);
            if (class_dec == null) return null;

            var class_extends_dec = ALittleScriptUtility.FindClassExtends(class_dec);
            if (class_extends_dec == null) return null;

            var extends_ctor_dec = ALittleScriptUtility.FindFirstCtorDecFromExtends(class_extends_dec, 100);
            if (extends_ctor_dec == null) return null;

            // 参数必须一致并且可转化
            var extends_method_param_dec = extends_ctor_dec.GetMethodParamDec();
            var my_method_param_dec = m_element.GetMethodParamDec();
            if (extends_method_param_dec == null && my_method_param_dec == null) return null;
            if (extends_method_param_dec == null || my_method_param_dec == null)
                return new ABnfGuessError(m_element, "该函数是从父类继承下来，但是参数数量不一致");

            var extends_param_one_dec_list = extends_method_param_dec.GetMethodParamOneDecList();
            var my_param_one_dec_list = my_method_param_dec.GetMethodParamOneDecList();
            if (extends_param_one_dec_list.Count > my_param_one_dec_list.Count)
                return new ABnfGuessError(my_method_param_dec, "该函数是从父类继承下来，但是子类的参数数量少于父类的构造函数");

            for (int i = 0; i < extends_param_one_dec_list.Count; ++i)
            {
                var extends_one_dec = extends_param_one_dec_list[i];
                var extends_name_dec = extends_one_dec.GetMethodParamNameDec();
                if (extends_name_dec == null) return new ABnfGuessError(my_method_param_dec, "该函数是从父类继承下来，但是定义不一致");
                var my_one_dec = my_param_one_dec_list[i];
                var my_name_dec = my_one_dec.GetMethodParamNameDec();
                if (my_name_dec == null) return new ABnfGuessError(my_method_param_dec, "该函数是从父类继承下来，但是定义不一致");

                var error = extends_name_dec.GuessType(out ABnfGuess extends_name_dec_guess);
                if (error != null) return error;
                error = my_name_dec.GuessType(out ABnfGuess my_name_dec_guess);
                if (error != null) return error;
                if (extends_name_dec_guess.GetValue() != my_name_dec_guess.GetValue())
                    return new ABnfGuessError(my_method_param_dec, "该函数是从父类继承下来，但是子类参数和父类参数类型不一致");
            }

            return null;
        }
    }
}

