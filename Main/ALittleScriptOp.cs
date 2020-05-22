
using System.Collections.Generic;

namespace ALittle
{
    public class ALittleScriptOp
    {
        private static ABnfGuessError GuessTypeForOp8Impl(string op_string
                , ABnfElement left_src, ABnfGuess left_guess_type
                , ABnfElement right_src, ABnfGuess right_guess_type
                , ALittleScriptOp8SuffixElement op_8_suffix
                , out ABnfGuess guess)
        {
            guess = null;

            if (!(left_guess_type is ALittleScriptGuessBool))
                return new ABnfGuessError(left_src, op_string + "运算符左边必须是bool类型.不能是:" + left_guess_type.GetValue());

            if (!(right_guess_type is ALittleScriptGuessBool))
                return new ABnfGuessError(right_src, op_string + "运算符右边边必须是bool类型.不能是:" + right_guess_type.GetValue());

            guess = ALittleScriptIndex.inst.sBoolGuess;
            return null;
        }


        private static ABnfGuessError GuessTypeForOp8(ABnfElement left_src
                , ABnfGuess left_guess_type
                , ALittleScriptOp8SuffixElement op_8_suffix
                , out ABnfGuess guess)
        {
            guess = null;
            string op_string = op_8_suffix.GetOp8().GetElementText();

            ABnfGuess suffix_guess_type = null;
            ABnfElement last_src = null;

            var value_factor_stat = op_8_suffix.GetValueFactorStat();
            var op_2_value = op_8_suffix.GetOp2Value();
            if (value_factor_stat != null)
            {
                var error = value_factor_stat.GuessType(out suffix_guess_type);
                if (error != null) return error;
                last_src = value_factor_stat;
            }
            else if (op_2_value != null)
            {
                var error = GuessType(op_2_value, out suffix_guess_type);
                if (error != null) return error;
                last_src = op_2_value;
            }
            else
            {
                return new ABnfGuessError(op_8_suffix, "未知的表达式");
            }

            var suffix_ee_list = op_8_suffix.GetOp8SuffixEeList();
            foreach (var suffix_ee in suffix_ee_list)
            {
                if (suffix_ee.GetOp3Suffix() != null)
                {
                    var error = GuessTypeForOp3(last_src, suffix_guess_type, suffix_ee.GetOp3Suffix(), out suffix_guess_type);
                    if (error != null) return error;
                    last_src = suffix_ee.GetOp3Suffix();
                }
                else if (suffix_ee.GetOp4Suffix() != null)
                {
                    var error = GuessTypeForOp4(last_src, suffix_guess_type, suffix_ee.GetOp4Suffix(), out suffix_guess_type);
                    if (error != null) return error;
                    last_src = suffix_ee.GetOp4Suffix();
                }
                else if (suffix_ee.GetOp5Suffix() != null)
                {
                    var error = GuessTypeForOp5(last_src, suffix_guess_type, suffix_ee.GetOp5Suffix(), out suffix_guess_type);
                    if (error != null) return error;
                    last_src = suffix_ee.GetOp5Suffix();
                }
                else if (suffix_ee.GetOp6Suffix() != null)
                {
                    var error = GuessTypeForOp6(last_src, suffix_guess_type, suffix_ee.GetOp6Suffix(), out suffix_guess_type);
                    if (error != null) return error;
                    last_src = suffix_ee.GetOp6Suffix();
                }
                else if (suffix_ee.GetOp7Suffix() != null)
                {
                    var error = GuessTypeForOp7(last_src, suffix_guess_type, suffix_ee.GetOp7Suffix(), out suffix_guess_type);
                    if (error != null) return error;
                    last_src = suffix_ee.GetOp7Suffix();
                }
                else
                {
                    return new ABnfGuessError(suffix_ee, "未知的表达式");
                }
            }

            return GuessTypeForOp8Impl(op_string, left_src, left_guess_type, last_src, suffix_guess_type, op_8_suffix, out guess);
        }


        private static ABnfGuessError GuessTypeForOp7Impl(string op_string
                , ABnfElement left_src, ABnfGuess left_guess_type
                , ABnfElement right_src, ABnfGuess right_guess_type
                , ALittleScriptOp7SuffixElement op_7_suffix
                , out ABnfGuess guess)
        {
            guess = null;
            if (!(left_guess_type is ALittleScriptGuessBool))
                return new ABnfGuessError(left_src, op_string + "运算符左边必须是bool类型.不能是:" + left_guess_type.GetValue());

            if (!(right_guess_type is ALittleScriptGuessBool))
                return new ABnfGuessError(right_src, op_string + "运算符右边边必须是bool类型.不能是:" + right_guess_type.GetValue());

            guess = ALittleScriptIndex.inst.sBoolGuess;
            return null;
        }


        private static ABnfGuessError GuessTypeForOp7(ABnfElement left_src, ABnfGuess left_guess_type, ALittleScriptOp7SuffixElement op_7_suffix, out ABnfGuess guess)
        {
            guess = null;
            string op_string = op_7_suffix.GetOp7().GetElementText();

            ABnfGuess suffix_guess_type = null;
            ABnfElement last_src = null;

            var value_factor_stat = op_7_suffix.GetValueFactorStat();
            var op_2_value = op_7_suffix.GetOp2Value();
            if (value_factor_stat != null)
            {
                var error = value_factor_stat.GuessType(out suffix_guess_type);
                if (error != null) return error;
                last_src = value_factor_stat;
            }
            else if (op_2_value != null)
            {
                var error = GuessType(op_2_value, out suffix_guess_type);
                if (error != null) return error;
                last_src = op_2_value;
            }
            else
            {
                return new ABnfGuessError(op_7_suffix, "未知的表达式");
            }

            var suffix_ee_list = op_7_suffix.GetOp7SuffixEeList();
            foreach (var suffix_ee in suffix_ee_list)
            {
                if (suffix_ee.GetOp3Suffix() != null)
                {
                    var error = GuessTypeForOp3(last_src, suffix_guess_type, suffix_ee.GetOp3Suffix(), out suffix_guess_type);
                    if (error != null) return error;
                    last_src = suffix_ee.GetOp3Suffix();
                }
                else if (suffix_ee.GetOp4Suffix() != null)
                {
                    var error = GuessTypeForOp4(last_src, suffix_guess_type, suffix_ee.GetOp4Suffix(), out suffix_guess_type);
                    if (error != null) return error;
                    last_src = suffix_ee.GetOp4Suffix();
                }
                else if (suffix_ee.GetOp5Suffix() != null)
                {
                    var error = GuessTypeForOp5(last_src, suffix_guess_type, suffix_ee.GetOp5Suffix(), out suffix_guess_type);
                    if (error != null) return error;
                    last_src = suffix_ee.GetOp5Suffix();
                }
                else if (suffix_ee.GetOp6Suffix() != null)
                {
                    var error = GuessTypeForOp6(last_src, suffix_guess_type, suffix_ee.GetOp6Suffix(), out suffix_guess_type);
                    if (error != null) return error;
                    last_src = suffix_ee.GetOp6Suffix();
                }
                else
                {
                    return new ABnfGuessError(suffix_ee, "未知的表达式");
                }
            }

            return GuessTypeForOp7Impl(op_string, left_src, left_guess_type, last_src, suffix_guess_type, op_7_suffix, out guess);
        }


        private static ABnfGuessError GuessTypeForOp6Impl(string op_string
                , ABnfElement left_src, ABnfGuess left_guess_type
                , ABnfElement right_src, ABnfGuess right_guess_type
                , ALittleScriptOp6SuffixElement op_6_suffix
                , out ABnfGuess guess)
        {
            guess = null;
            if (op_string == "==" || op_string == "!=")
            {
                if (left_guess_type is ALittleScriptGuessAny || left_guess_type.GetValue() == "null"
                    || right_guess_type is ALittleScriptGuessAny || right_guess_type.GetValue() == "null")
                {
                    guess = ALittleScriptIndex.inst.sBoolGuess;
                    return null;
                }

                if (left_guess_type is ALittleScriptGuessInt || left_guess_type is ALittleScriptGuessLong || left_guess_type is ALittleScriptGuessDouble)
                {
                    if (right_guess_type is ALittleScriptGuessInt
                        || right_guess_type is ALittleScriptGuessLong
                        || right_guess_type is ALittleScriptGuessDouble)
                    {
                        guess = ALittleScriptIndex.inst.sBoolGuess;
                        return null;
                    }
                    return new ABnfGuessError(right_src, op_string + "运算符左边是数字，那么右边必须是int,long,double,any,null类型.不能是:" + right_guess_type.GetValue());
                }

                if (left_guess_type is ALittleScriptGuessString)
                {
                    if (right_guess_type is ALittleScriptGuessString)
                    {
                        guess = ALittleScriptIndex.inst.sBoolGuess;
                        return null;
                    }
                    return new ABnfGuessError(right_src, op_string + "运算符左边是字符串，那么右边必须是string,any,null类型.不能是:" + right_guess_type.GetValue());
                }

                guess = ALittleScriptIndex.inst.sBoolGuess;
                return null;
            }
            else
            {
                if (left_guess_type is ALittleScriptGuessInt || left_guess_type is ALittleScriptGuessLong || left_guess_type is ALittleScriptGuessDouble)
                {
                    if (right_guess_type is ALittleScriptGuessInt || right_guess_type is ALittleScriptGuessLong || right_guess_type is ALittleScriptGuessDouble)
                    {
                        guess = ALittleScriptIndex.inst.sBoolGuess;
                        return null;
                    }
                    return new ABnfGuessError(right_src, op_string + "运算符左边是数字，那么右边必须是int,long,double类型.不能是:" + right_guess_type.GetValue());
                }

                if (left_guess_type is ALittleScriptGuessString)
                {
                    if (right_guess_type is ALittleScriptGuessString)
                    {
                        guess = ALittleScriptIndex.inst.sBoolGuess;
                        return null;
                    }
                    return new ABnfGuessError(right_src, op_string + "运算符左边是字符串，那么右边必须是string类型.不能是:" + right_guess_type.GetValue());
                }

                return new ABnfGuessError(left_src, op_string + "运算符左边必须是int,long,double,string类型.不能是:" + left_guess_type.GetValue());
            }
        }

        private static ABnfGuessError GuessTypeForOp6(ABnfElement left_src, ABnfGuess left_guess_type, ALittleScriptOp6SuffixElement op_6_suffix, out ABnfGuess guess)
        {
            guess = null;
            string op_string = op_6_suffix.GetOp6().GetElementText();

            ABnfGuess suffix_guess_type = null;
            ABnfElement last_src = null;

            var value_factor_stat = op_6_suffix.GetValueFactorStat();
            var op_2_value = op_6_suffix.GetOp2Value();
            if (value_factor_stat != null)
            {
                var error = value_factor_stat.GuessType(out suffix_guess_type);
                if (error != null) return error;
                last_src = value_factor_stat;
            }
            else if (op_2_value != null)
            {
                var error = GuessType(op_2_value, out suffix_guess_type);
                if (error != null) return error;
                last_src = op_2_value;
            }
            else
            {
                return new ABnfGuessError(op_6_suffix, "未知的表达式");
            }

            var suffix_ee_list = op_6_suffix.GetOp6SuffixEeList();
            foreach (var suffix_ee in suffix_ee_list)
            {
                if (suffix_ee.GetOp3Suffix() != null)
                {
                    var error = GuessTypeForOp3(last_src, suffix_guess_type, suffix_ee.GetOp3Suffix(), out suffix_guess_type);
                    if (error != null) return error;
                    last_src = suffix_ee.GetOp3Suffix();
                }
                else if (suffix_ee.GetOp4Suffix() != null)
                {
                    var error = GuessTypeForOp4(last_src, suffix_guess_type, suffix_ee.GetOp4Suffix(), out suffix_guess_type);
                    if (error != null) return error;
                    last_src = suffix_ee.GetOp4Suffix();
                }
                else if (suffix_ee.GetOp5Suffix() != null)
                {
                    var error = GuessTypeForOp5(last_src, suffix_guess_type, suffix_ee.GetOp5Suffix(), out suffix_guess_type);
                    if (error != null) return error;
                    last_src = suffix_ee.GetOp5Suffix();
                }
                else
                {
                    return new ABnfGuessError(suffix_ee, "未知的表达式");
                }
            }
            return GuessTypeForOp6Impl(op_string, left_src, left_guess_type, last_src, suffix_guess_type, op_6_suffix, out guess);
        }


        private static ABnfGuessError GuessTypeForOp5Impl(string op_string
                , ABnfElement left_src, ABnfGuess left_guess_type
                , ABnfElement right_src, ABnfGuess right_guess_type
                , ALittleScriptOp5SuffixElement op_5_suffix
                , out ABnfGuess guess)
        {
            guess = null;
            bool left_check = left_guess_type is ALittleScriptGuessInt || left_guess_type is ALittleScriptGuessLong || left_guess_type is ALittleScriptGuessDouble || left_guess_type is ALittleScriptGuessString;
            if (!left_check)
                return new ABnfGuessError(left_src, op_string + "运算符左边必须是int,long,double,string类型.不能是:" + left_guess_type.GetValue());

            bool right_check = right_guess_type is ALittleScriptGuessInt || right_guess_type is ALittleScriptGuessLong || right_guess_type is ALittleScriptGuessDouble || right_guess_type is ALittleScriptGuessString;
            if (!right_check)
                return new ABnfGuessError(right_src, op_string + "运算符右边必须是int,long,double,string类型.不能是:" + right_guess_type.GetValue());

            if (!(left_guess_type is ALittleScriptGuessString || right_guess_type is ALittleScriptGuessString))
                return new ABnfGuessError(left_src, op_string + "运算符左边和右边至少一个是string类型.不能是:" + left_guess_type.GetValue() + "和" + right_guess_type.GetValue());

            guess = ALittleScriptIndex.inst.sStringGuess;
            return null;
        }


        private static ABnfGuessError GuessTypeForOp5(ABnfElement left_src, ABnfGuess left_guess_type, ALittleScriptOp5SuffixElement op_5_suffix, out ABnfGuess guess)
        {
            guess = null;
            string op_string = op_5_suffix.GetOp5().GetElementText();

            ABnfGuess suffix_guess_type = null;
            ABnfElement last_src = null;

            var value_factor_stat = op_5_suffix.GetValueFactorStat();
            var op_2_value = op_5_suffix.GetOp2Value();
            if (value_factor_stat != null)
            {
                var error = value_factor_stat.GuessType(out suffix_guess_type);
                if (error != null) return error;
                last_src = value_factor_stat;
            }
            else if (op_2_value != null)
            {
                var error = GuessType(op_2_value, out suffix_guess_type);
                if (error != null) return error;
                last_src = op_2_value;
            }
            else
            {
                return new ABnfGuessError(op_5_suffix, "未知的表达式");
            }

            var suffix_ee_list = op_5_suffix.GetOp5SuffixEeList();
            foreach (var suffix_ee in suffix_ee_list)
            {
                if (suffix_ee.GetOp3Suffix() != null)
                {
                    var error = GuessTypeForOp3(last_src, suffix_guess_type, suffix_ee.GetOp3Suffix(), out suffix_guess_type);
                    if (error != null) return error;
                    last_src = suffix_ee.GetOp3Suffix();
                }
                else if (suffix_ee.GetOp4Suffix() != null)
                {
                    var error = GuessTypeForOp4(last_src, suffix_guess_type, suffix_ee.GetOp4Suffix(), out suffix_guess_type);
                    if (error != null) return error;
                    last_src = suffix_ee.GetOp4Suffix();
                }
                else
                {
                    return new ABnfGuessError(suffix_ee, "未知的表达式");
                }
            }

            return GuessTypeForOp5Impl(op_string, left_src, left_guess_type, last_src, suffix_guess_type, op_5_suffix, out guess);
        }


        private static ABnfGuessError GuessTypeForOp4Impl(string op_string
                , ABnfElement left_src, ABnfGuess left_guess_type
                , ABnfElement right_src, ABnfGuess right_guess_type
                , ALittleScriptOp4SuffixElement op_4_suffix
                , out ABnfGuess guess)
        {
            guess = null;
            if (left_guess_type is ALittleScriptGuessInt || left_guess_type is ALittleScriptGuessLong)
            {
                 if (right_guess_type is ALittleScriptGuessInt || right_guess_type is ALittleScriptGuessLong)
                {
                    guess = left_guess_type;
                    return null;
                }
                else if (right_guess_type is ALittleScriptGuessDouble)
                {
                    guess = right_guess_type;
                    return null;
                }
                else
                    return new ABnfGuessError(right_src, op_string + "运算符右边必须是int,long,double类型.不能是:" + right_guess_type.GetValue());
            }

            if (left_guess_type is ALittleScriptGuessDouble)
            {
                if (right_guess_type is ALittleScriptGuessInt || right_guess_type is ALittleScriptGuessLong)
                {
                    guess = left_guess_type;
                    return null;
                }
                else if (right_guess_type is ALittleScriptGuessDouble)
                {
                    guess = right_guess_type;
                    return null;
                }

                else
                    return new ABnfGuessError(right_src, op_string + "运算符右边必须是int,long,double类型.不能是:" + right_guess_type.GetValue());
            }

            return new ABnfGuessError(left_src, op_string + "运算符左边必须是int,long,double类型.不能是:" + left_guess_type.GetValue());
        }


        private static ABnfGuessError GuessTypeForOp4(ABnfElement left_src, ABnfGuess left_guess_type, ALittleScriptOp4SuffixElement op_4_suffix, out ABnfGuess guess)
        {
            guess = null;
            string op_string = op_4_suffix.GetOp4().GetElementText();

            ABnfGuess suffix_guess_type = null;
            ABnfElement last_src = null;

            var value_factor_stat = op_4_suffix.GetValueFactorStat();
            var op_2_value = op_4_suffix.GetOp2Value();
            if (value_factor_stat != null)
            {
                var error = value_factor_stat.GuessType(out suffix_guess_type);
                if (error != null) return error;
                last_src = value_factor_stat;
            }
            else if (op_2_value != null)
            {
                var error = GuessType(op_2_value, out suffix_guess_type);
                if (error != null) return error;
                last_src = op_2_value;
            }
            else
            {
                return new ABnfGuessError(op_4_suffix, "未知的表达式");
            }

            var suffix_ee_list = op_4_suffix.GetOp4SuffixEeList();
            foreach (var suffix_ee in suffix_ee_list)
            {
                if (suffix_ee.GetOp3Suffix() != null)
                {
                    var error = GuessTypeForOp3(last_src, suffix_guess_type, suffix_ee.GetOp3Suffix(), out suffix_guess_type);
                    if (error != null) return error;
                    last_src = suffix_ee.GetOp3Suffix();
                }
                else
                {
                    return new ABnfGuessError(suffix_ee, "未知的表达式");
                }
            }

            return GuessTypeForOp4Impl(op_string, left_src, left_guess_type, last_src, suffix_guess_type, op_4_suffix, out guess);
        }


        private static ABnfGuessError GuessTypeForOp3(ABnfElement left_src, ABnfGuess left_guess_type, ALittleScriptOp3SuffixElement op_3_suffix, out ABnfGuess guess)
        {
            guess = null;
            string op_string = op_3_suffix.GetOp3().GetElementText();

            ABnfGuess right_guess_type = null;
            ABnfElement right_src = null;

            var value_factor_stat = op_3_suffix.GetValueFactorStat();
            var op_2_value = op_3_suffix.GetOp2Value();
            if (value_factor_stat != null)
            {
                var error = value_factor_stat.GuessType(out right_guess_type);
                if (error != null) return error;
                right_src = value_factor_stat;
            }
            else if (op_2_value != null)
            {
                var error = GuessType(op_2_value, out right_guess_type);
                if (error != null) return error;
                right_src = op_2_value;
            }
            else
            {
                return new ABnfGuessError(op_3_suffix, "未知的表达式");
            }

            if (left_guess_type is ALittleScriptGuessInt)
            {
                if (right_guess_type is ALittleScriptGuessInt)
                {
                    // 这个是特殊的
                    if (op_string == "/")
                    {
                        guess = ALittleScriptIndex.inst.sDoubleGuess;
                        return null;
                    }
                    guess = left_guess_type;
                    return null;
                }
                else if (right_guess_type is ALittleScriptGuessLong)
                {
                    // 这个是特殊的
                    if (op_string == "/")
                    {
                        guess = ALittleScriptIndex.inst.sDoubleGuess;
                        return null;
                    }
                    guess = right_guess_type;
                    return null;
                }
                else if (right_guess_type is ALittleScriptGuessDouble)
                {
                    guess = right_guess_type;
                    return null;
                }
                else
                {
                    return new ABnfGuessError(right_src, op_string + "运算符右边必须是int,long,double类型.不能是:" + right_guess_type.GetValue());
                }
            }

            if (left_guess_type is ALittleScriptGuessLong)
            {
                if (right_guess_type is ALittleScriptGuessInt || right_guess_type is ALittleScriptGuessLong)
                {
                    // 这个是特殊的
                    if (op_string == "/")
                    {
                        guess = ALittleScriptIndex.inst.sDoubleGuess;
                        return null;
                    }
                    guess = left_guess_type;
                    return null;
                }
                else if (right_guess_type is ALittleScriptGuessDouble)
                {
                    guess = right_guess_type;
                    return null;
                }
                else
                {
                    return new ABnfGuessError(right_src, op_string + "运算符右边必须是int,long,double类型.不能是:" + right_guess_type.GetValue());
                }
            }

            if (left_guess_type is ALittleScriptGuessDouble)
            {
                if (right_guess_type is ALittleScriptGuessInt || right_guess_type is ALittleScriptGuessLong)
                {
                    guess = left_guess_type;
                    return null;
                }
                else if (right_guess_type is ALittleScriptGuessDouble)
                {
                    guess = right_guess_type;
                    return null;
                }
                else
                    return new ABnfGuessError(right_src, op_string + "运算符右边必须是int,long,double类型.不能是:" + right_guess_type.GetValue());
            }

            return new ABnfGuessError(left_src, op_string + "运算符左边必须是int,long,double类型.不能是:" + left_guess_type.GetValue());
        }

        public static ABnfGuessError GuessType(ALittleScriptValueFactorStatElement value_factor_stat, ALittleScriptOp8StatElement op_8_stat, out ABnfGuess guess)
        {
            guess = null;
            var error = value_factor_stat.GuessType(out ABnfGuess factor_guess_type);
            if (error != null) return error;

            error = GuessTypeForOp8(value_factor_stat, factor_guess_type, op_8_stat.GetOp8Suffix(), out ABnfGuess suffix_guess_type);
            if (error != null) return error;

            ABnfElement last_src = op_8_stat.GetOp8Suffix();
            var suffix_ex_list = op_8_stat.GetOp8SuffixExList();
            foreach (var suffix_ex in suffix_ex_list)
            {
                if (suffix_ex.GetOp8Suffix() != null)
                {
                    error = GuessTypeForOp8(last_src, suffix_guess_type, suffix_ex.GetOp8Suffix(), out suffix_guess_type);
                    if (error != null) return error;
                    if (suffix_guess_type == null) return null;
                    last_src = suffix_ex.GetOp8Suffix();
                }
                else
                {
                    return new ABnfGuessError(suffix_ex, "未知的表达式");
                }
            }

            guess = suffix_guess_type;
            return null;
        }


        public static ABnfGuessError GuessType(ALittleScriptValueFactorStatElement value_factor_stat, ALittleScriptOp7StatElement op_7_stat, out ABnfGuess guess)
        {
            guess = null;
            var error = value_factor_stat.GuessType(out ABnfGuess factor_guess_type);
            if (error != null) return error;

            error = GuessTypeForOp7(value_factor_stat, factor_guess_type, op_7_stat.GetOp7Suffix(), out ABnfGuess suffix_guess_type);
            if (error != null) return error;

            ABnfElement last_src = op_7_stat.GetOp7Suffix();
            var suffix_ex_list = op_7_stat.GetOp7SuffixExList();
            foreach (var suffix_ex in suffix_ex_list)
            {
                if (suffix_ex.GetOp7Suffix() != null)
                {
                    error = GuessTypeForOp7(last_src, suffix_guess_type, suffix_ex.GetOp7Suffix(), out suffix_guess_type);
                    if (error != null) return error;
                    last_src = suffix_ex.GetOp7Suffix();
                }
                else if (suffix_ex.GetOp8Suffix() != null)
                {
                    error = GuessTypeForOp8(last_src, suffix_guess_type, suffix_ex.GetOp8Suffix(), out suffix_guess_type);
                    if (error != null) return error;
                    last_src = suffix_ex.GetOp8Suffix();
                }
                else
                {
                    return new ABnfGuessError(suffix_ex, "未知的表达式");
                }
            }

            guess = suffix_guess_type;
            return null;
        }


        public static ABnfGuessError GuessType(ALittleScriptValueFactorStatElement value_factor_stat, ALittleScriptOp6StatElement op_6_stat, out ABnfGuess guess)
        {
            guess = null;
            var error = value_factor_stat.GuessType(out ABnfGuess factor_guess_type);
            if (error != null) return error;

            error = GuessTypeForOp6(value_factor_stat, factor_guess_type, op_6_stat.GetOp6Suffix(), out ABnfGuess suffix_guess_type);
            if (error != null) return error;

            ABnfElement last_src = op_6_stat.GetOp6Suffix();
            var suffix_ex_list = op_6_stat.GetOp6SuffixExList();
            foreach (var suffix_ex in suffix_ex_list)
            {
                if (suffix_ex.GetOp6Suffix() != null)
                {
                    error = GuessTypeForOp6(last_src, suffix_guess_type, suffix_ex.GetOp6Suffix(), out suffix_guess_type);
                    if (error != null) return error;
                    last_src = suffix_ex.GetOp6Suffix();
                }
                else if (suffix_ex.GetOp7Suffix() != null)
                {
                    error = GuessTypeForOp7(last_src, suffix_guess_type, suffix_ex.GetOp7Suffix(), out suffix_guess_type);
                    if (error != null) return error;
                    last_src = suffix_ex.GetOp7Suffix();
                }
                else if (suffix_ex.GetOp8Suffix() != null)
                {
                    error = GuessTypeForOp8(last_src, suffix_guess_type, suffix_ex.GetOp8Suffix(), out suffix_guess_type);
                    if (error != null) return error;
                    last_src = suffix_ex.GetOp8Suffix();
                }
                else
                {
                    return new ABnfGuessError(suffix_ex, "未知的表达式");
                }
            }

            guess = suffix_guess_type;
            return null;
        }

        public static ABnfGuessError GuessType(ALittleScriptValueFactorStatElement value_factor_stat, ALittleScriptOp5StatElement op_5_stat, out ABnfGuess guess)
        {
            guess = null;
            var error = value_factor_stat.GuessType(out ABnfGuess factor_guess_type);
            if (error != null) return error;

            error = GuessTypeForOp5(value_factor_stat, factor_guess_type, op_5_stat.GetOp5Suffix(), out ABnfGuess suffix_guess_type);
            if (error != null) return error;

            ABnfElement last_src = op_5_stat.GetOp5Suffix();
            var suffix_ex_list = op_5_stat.GetOp5SuffixExList();
            foreach (var suffix_ex in suffix_ex_list)
            {
                if (suffix_ex.GetOp5Suffix() != null)
                {
                    error = GuessTypeForOp5(last_src, suffix_guess_type, suffix_ex.GetOp5Suffix(), out suffix_guess_type);
                    if (error != null) return error;
                    last_src = suffix_ex.GetOp5Suffix();
                }
                else if (suffix_ex.GetOp6Suffix() != null)
                {
                    error = GuessTypeForOp6(last_src, suffix_guess_type, suffix_ex.GetOp6Suffix(), out suffix_guess_type);
                    if (error != null) return error;
                    last_src = suffix_ex.GetOp6Suffix();
                }
                else if (suffix_ex.GetOp7Suffix() != null)
                {
                    error = GuessTypeForOp7(last_src, suffix_guess_type, suffix_ex.GetOp7Suffix(), out suffix_guess_type);
                    if (error != null) return error;
                    last_src = suffix_ex.GetOp7Suffix();
                }
                else if (suffix_ex.GetOp8Suffix() != null)
                {
                    error = GuessTypeForOp8(last_src, suffix_guess_type, suffix_ex.GetOp8Suffix(), out suffix_guess_type);
                    if (error != null) return error;
                    last_src = suffix_ex.GetOp8Suffix();
                }
                else
                {
                    return new ABnfGuessError(suffix_ex, "未知的表达式");
                }
            }

            guess = suffix_guess_type;
            return null;
        }


        public static ABnfGuessError GuessType(ALittleScriptValueFactorStatElement value_factor_stat, ALittleScriptOp4StatElement op_4_stat, out ABnfGuess guess)
        {
            guess = null;
            var error = value_factor_stat.GuessType(out ABnfGuess factor_guess_type);
            if (error != null) return error;

            error = GuessTypeForOp4(value_factor_stat, factor_guess_type, op_4_stat.GetOp4Suffix(), out ABnfGuess suffix_guess_type);
            if (error != null) return error;

            ABnfElement last_src = op_4_stat.GetOp4Suffix();
            var suffix_ex_list = op_4_stat.GetOp4SuffixExList();
            foreach (var suffix_ex in suffix_ex_list)
            {
                if (suffix_ex.GetOp4Suffix() != null)
                {
                    error = GuessTypeForOp4(last_src, suffix_guess_type, suffix_ex.GetOp4Suffix(), out suffix_guess_type);
                    if (error != null) return error;
                    last_src = suffix_ex.GetOp4Suffix();
                }
                else if (suffix_ex.GetOp5Suffix() != null)
                {
                    error = GuessTypeForOp5(last_src, suffix_guess_type, suffix_ex.GetOp5Suffix(), out suffix_guess_type);
                    if (error != null) return error;
                    last_src = suffix_ex.GetOp5Suffix();
                }
                else if (suffix_ex.GetOp6Suffix() != null)
                {
                    error = GuessTypeForOp6(last_src, suffix_guess_type, suffix_ex.GetOp6Suffix(), out suffix_guess_type);
                    if (error != null) return error;
                    last_src = suffix_ex.GetOp6Suffix();
                }
                else if (suffix_ex.GetOp7Suffix() != null)
                {
                    error = GuessTypeForOp7(last_src, suffix_guess_type, suffix_ex.GetOp7Suffix(), out suffix_guess_type);
                    if (error != null) return error;
                    last_src = suffix_ex.GetOp7Suffix();
                }
                else if (suffix_ex.GetOp8Suffix() != null)
                {
                    error = GuessTypeForOp8(last_src, suffix_guess_type, suffix_ex.GetOp8Suffix(), out suffix_guess_type);
                    if (error != null) return error;
                    last_src = suffix_ex.GetOp8Suffix();
                }
                else
                {
                    return new ABnfGuessError(suffix_ex, "未知的表达式");
                }
            }

            guess = suffix_guess_type;
            return null;
        }

        public static ABnfGuessError GuessType(ALittleScriptValueFactorStatElement value_factor_stat, ALittleScriptOp3StatElement op_3_stat, out ABnfGuess guess)
        {
            guess = null;
            var error = value_factor_stat.GuessType(out ABnfGuess factor_guess_type);
            if (error != null) return error;

            error = GuessTypeForOp3(value_factor_stat, factor_guess_type, op_3_stat.GetOp3Suffix(), out ABnfGuess suffix_guess_type);
            if (error != null) return error;

            ABnfElement last_src = op_3_stat.GetOp3Suffix();
            var suffix_ex_list = op_3_stat.GetOp3SuffixExList();
            foreach (var suffix_ex in suffix_ex_list)
            {
                if (suffix_ex.GetOp3Suffix() != null)
                {
                    error = GuessTypeForOp3(last_src, suffix_guess_type, suffix_ex.GetOp3Suffix(), out suffix_guess_type);
                    if (error != null) return error;
                    last_src = suffix_ex.GetOp3Suffix();
                }
                else if (suffix_ex.GetOp4Suffix() != null)
                {
                    error = GuessTypeForOp4(last_src, suffix_guess_type, suffix_ex.GetOp4Suffix(), out suffix_guess_type);
                    if (error != null) return error;
                    last_src = suffix_ex.GetOp4Suffix();
                }
                else if (suffix_ex.GetOp5Suffix() != null)
                {
                    error = GuessTypeForOp5(last_src, suffix_guess_type, suffix_ex.GetOp5Suffix(), out suffix_guess_type);
                    if (error != null) return error;
                    last_src = suffix_ex.GetOp5Suffix();
                }
                else if (suffix_ex.GetOp6Suffix() != null)
                {
                    error = GuessTypeForOp6(last_src, suffix_guess_type, suffix_ex.GetOp6Suffix(), out suffix_guess_type);
                    if (error != null) return error;
                    last_src = suffix_ex.GetOp6Suffix();
                }
                else if (suffix_ex.GetOp7Suffix() != null)
                {
                    error = GuessTypeForOp7(last_src, suffix_guess_type, suffix_ex.GetOp7Suffix(), out suffix_guess_type);
                    if (error != null) return error;
                    last_src = suffix_ex.GetOp7Suffix();
                }
                else if (suffix_ex.GetOp8Suffix() != null)
                {
                    error = GuessTypeForOp8(last_src, suffix_guess_type, suffix_ex.GetOp8Suffix(), out suffix_guess_type);
                    if (error != null) return error;
                    last_src = suffix_ex.GetOp8Suffix();
                }
                else
                {
                    return new ABnfGuessError(suffix_ex, "未知的表达式");
                }
            }

            guess = suffix_guess_type;
            return null;
        }

        public static ABnfGuessError GuessTypes(ALittleScriptValueOpStatElement value_op_stat, out List<ABnfGuess> guess_list)
        {
            guess_list = null;
            var value_factor_stat = value_op_stat.GetValueFactorStat();
            
            if (value_op_stat.GetOp3Stat() != null)
            {
                guess_list = new List<ABnfGuess>();
                var error = GuessType(value_factor_stat, value_op_stat.GetOp3Stat(), out ABnfGuess guess);
                if (error != null) return error;
                guess_list.Add(guess);
                return null;
            }
            else if (value_op_stat.GetOp4Stat() != null)
            {
                guess_list = new List<ABnfGuess>();
                var error = GuessType(value_factor_stat, value_op_stat.GetOp4Stat(), out ABnfGuess guess);
                if (error != null) return error;
                guess_list.Add(guess);
                return null;
            }
            else if (value_op_stat.GetOp5Stat() != null)
            {
                guess_list = new List<ABnfGuess>();
                var error = GuessType(value_factor_stat, value_op_stat.GetOp5Stat(), out ABnfGuess guess);
                if (error != null) return error;
                guess_list.Add(guess);
                return null;
            }
            else if (value_op_stat.GetOp6Stat() != null)
            {
                guess_list = new List<ABnfGuess>();
                var error = GuessType(value_factor_stat, value_op_stat.GetOp6Stat(), out ABnfGuess guess);
                if (error != null) return error;
                guess_list.Add(guess);
                return null;
            }
            else if (value_op_stat.GetOp7Stat() != null)
            {
                guess_list = new List<ABnfGuess>();
                var error = GuessType(value_factor_stat, value_op_stat.GetOp7Stat(), out ABnfGuess guess);
                if (error != null) return error;
                guess_list.Add(guess);
                return null;
            }
            else if (value_op_stat.GetOp8Stat() != null)
            {
                guess_list = new List<ABnfGuess>();
                var error = GuessType(value_factor_stat, value_op_stat.GetOp8Stat(), out ABnfGuess guess);
                if (error != null) return error;
                guess_list.Add(guess);
                return null;
            }
            return value_factor_stat.GuessTypes(out guess_list);
        }

        public static ABnfGuessError GuessType(ALittleScriptOp2ValueElement op_2_value, out ABnfGuess guess)
        {
            guess = null;

            var value_factor_stat = op_2_value.GetValueFactorStat();
            if (value_factor_stat == null)
                return new ABnfGuessError(value_factor_stat, "单目运算没有目标表达式");

            var error = value_factor_stat.GuessType(out ABnfGuess guess_info);
            if (error != null) return error;

            string op_2 = op_2_value.GetOp2().GetElementText();
            // guess_type必须是逻辑运算符
            if (op_2 == "!")
            {
                if (!(guess_info is ALittleScriptGuessBool))
                    return new ABnfGuessError(value_factor_stat, "!运算符右边必须是bool类型.不能是:" + guess_info.GetValue());
                // guess_type必须是数字
            }
            else if (op_2 == "-")
            {
                if (!(guess_info is ALittleScriptGuessInt) && !(guess_info is ALittleScriptGuessLong) && !(guess_info is ALittleScriptGuessDouble))
                    return new ABnfGuessError(value_factor_stat, "-运算符右边必须是int,double类型.不能是:" + guess_info.GetValue());
            }
            else
            {
                return new ABnfGuessError(op_2_value.GetOp2(), "未知的运算符:" + op_2);
            }

            guess = guess_info;
            return null;
        }

        public static ABnfGuessError GuessType(ALittleScriptOp2StatElement op_2_stat, out ABnfGuess guess)
        {
            guess = null;
            var op_2_value = op_2_stat.GetOp2Value();
            var error = GuessType(op_2_value, out ABnfGuess suffix_guess_type);
            if (error != null) return error;

            ABnfElement last_src = op_2_value;
            var suffix_ex_list = op_2_stat.GetOp2SuffixExList();
            foreach (var suffix_ex in suffix_ex_list)
            {
                if (suffix_ex.GetOp3Suffix() != null)
                {
                    error = GuessTypeForOp3(last_src, suffix_guess_type, suffix_ex.GetOp3Suffix(), out suffix_guess_type);
                    if (error != null) return error;
                    last_src = suffix_ex.GetOp3Suffix();
                }
                else if (suffix_ex.GetOp4Suffix() != null)
                {
                    error = GuessTypeForOp4(last_src, suffix_guess_type, suffix_ex.GetOp4Suffix(), out suffix_guess_type);
                    if (error != null) return error;
                    last_src = suffix_ex.GetOp4Suffix();
                }
                else if (suffix_ex.GetOp5Suffix() != null)
                {
                    error = GuessTypeForOp5(last_src, suffix_guess_type, suffix_ex.GetOp5Suffix(), out suffix_guess_type);
                    if (error != null) return error;
                    last_src = suffix_ex.GetOp5Suffix();
                }
                else if (suffix_ex.GetOp6Suffix() != null)
                {
                    error = GuessTypeForOp6(last_src, suffix_guess_type, suffix_ex.GetOp6Suffix(), out suffix_guess_type);
                    if (error != null) return error;
                    last_src = suffix_ex.GetOp6Suffix();
                }
                else if (suffix_ex.GetOp7Suffix() != null)
                {
                    error = GuessTypeForOp7(last_src, suffix_guess_type, suffix_ex.GetOp7Suffix(), out suffix_guess_type);
                    if (error != null) return error;
                    last_src = suffix_ex.GetOp7Suffix();
                }
                else if (suffix_ex.GetOp8Suffix() != null)
                {
                    error = GuessTypeForOp8(last_src, suffix_guess_type, suffix_ex.GetOp8Suffix(), out suffix_guess_type);
                    if (error != null) return error;
                    last_src = suffix_ex.GetOp8Suffix();
                }
                else
                {
                    return new ABnfGuessError(suffix_ex, "未知的表达式");
                }
            }

            guess = suffix_guess_type;
            return null;
        }

        // assign_or_call 填true表示赋值，否则是函数调用的参数传递
        public static ABnfGuessError GuessTypeEqual(ABnfGuess left_guess, ABnfElement right_src, ABnfGuess right_guess, bool assign_or_call, bool is_return)
        {
            // 如果值等于null，那么可以赋值
            if (right_guess.GetValue() == "null") return null;

            // 如果字符串直接相等，那么直接返回成功
            if (!(left_guess is ALittleScriptGuessTemplate) && !(right_guess is ALittleScriptGuessTemplate)
                && left_guess.GetValue() == right_guess.GetValue()) return null;

            // const是否可以赋值给非const
            if (assign_or_call)
            {
                if (left_guess.is_const && !right_guess.is_const)
                    return new ABnfGuessError(right_src, "要求是" + left_guess.GetValue() + ", 不能是:" + right_guess.GetValue());
            }
            else
            {
                // 如果不是基本变量类型（排除any），基本都是值传递，函数调用时就不用检查const
                if (!(left_guess is ALittleScriptGuessPrimitive) || left_guess.GetValueWithoutConst() == "any")
                {
                    if (!left_guess.is_const && right_guess.is_const)
                        return new ABnfGuessError(right_src, "要求是" + left_guess.GetValue() + ", 不能是:" + right_guess.GetValue());
                }
            }

            // 如果字符串直接相等，那么直接返回成功
            if (!(left_guess is ALittleScriptGuessTemplate) && !(right_guess is ALittleScriptGuessTemplate)
                && left_guess.GetValueWithoutConst() == right_guess.GetValueWithoutConst()) return null;

            // 如果任何一方是any，那么就认为是相等
            if (left_guess is ALittleScriptGuessAny || right_guess is ALittleScriptGuessAny) return null;
            
            // 基本变量类型检查
            if (left_guess is ALittleScriptGuessBool)
                return new ABnfGuessError(right_src, "要求是bool,不能是:" + right_guess.GetValue());

            if (left_guess is ALittleScriptGuessInt)
            {
                if (right_guess is ALittleScriptGuessLong)
                    return new ABnfGuessError(right_src, "long赋值给int，需要使用cast<int>()做强制类型转换");
                if (right_guess is ALittleScriptGuessDouble)
                    return new ABnfGuessError(right_src, "double赋值给int，需要使用cast<int>()做强制类型转换");
                return new ABnfGuessError(right_src, "要求是int, 不能是:" + right_guess.GetValue());
            }

            if (left_guess is ALittleScriptGuessLong)
            {
                if (right_guess is ALittleScriptGuessInt) return null;

                if (right_guess is ALittleScriptGuessDouble)
                    return new ABnfGuessError(right_src, "double赋值给long，需要使用cast<long>()做强制类型转换");
                return new ABnfGuessError(right_src, "要求是long, 不能是:" + right_guess.GetValue());
            }

            if (left_guess is ALittleScriptGuessDouble)
            {
                if (right_guess is ALittleScriptGuessInt || right_guess is ALittleScriptGuessLong) return null;
                return new ABnfGuessError(right_src, "要求是double, 不能是:" + right_guess.GetValue());
            }

            if (left_guess is ALittleScriptGuessString)
                return new ABnfGuessError(right_src, "要求是string,不能是:" + right_guess.GetValue());

            if (left_guess is ALittleScriptGuessMap)
            {
                if (!(right_guess is ALittleScriptGuessMap))
                    return new ABnfGuessError(right_src, "要求是" + left_guess.GetValue() + ",不能是:" + right_guess.GetValue());
                
                var error = GuessTypeEqual(((ALittleScriptGuessMap)left_guess).key_type, right_src, ((ALittleScriptGuessMap)right_guess).key_type, assign_or_call, is_return);
                if (error != null) return new ABnfGuessError(right_src, "要求是" + left_guess.GetValue() + ",不能是:" + right_guess.GetValue());
                error = GuessTypeEqual(((ALittleScriptGuessMap)left_guess).value_type, right_src, ((ALittleScriptGuessMap)right_guess).value_type, assign_or_call, is_return);
                if (error != null) return new ABnfGuessError(right_src, "要求是" + left_guess.GetValue() + ",不能是:" + right_guess.GetValue());
                return null;
            }

            if (left_guess is ALittleScriptGuessList)
            {
                if (!(right_guess is ALittleScriptGuessList))
                    return new ABnfGuessError(right_src, "要求是" + left_guess.GetValue() + ",不能是:" + right_guess.GetValue());

                var error = GuessTypeEqual(((ALittleScriptGuessList)left_guess).sub_type, right_src, ((ALittleScriptGuessList)right_guess).sub_type, assign_or_call, is_return);
                if (error != null) return new ABnfGuessError(right_src, "要求是" + left_guess.GetValue() + ",不能是:" + right_guess.GetValue());
                return null;
            }

            if (left_guess is ALittleScriptGuessFunctor)
            {
                if (!(right_guess is ALittleScriptGuessFunctor))
                    return new ABnfGuessError(right_src, "要求是" + left_guess.GetValue() + ",不能是:" + right_guess.GetValue());

                ALittleScriptGuessFunctor left_guess_functor = (ALittleScriptGuessFunctor)left_guess;
                ALittleScriptGuessFunctor right_guess_functor = (ALittleScriptGuessFunctor)right_guess;

                if (left_guess_functor.param_list.Count != right_guess_functor.param_list.Count
                    || left_guess_functor.param_nullable_list.Count != right_guess_functor.param_nullable_list.Count
                    || left_guess_functor.return_list.Count != right_guess_functor.return_list.Count
                    || left_guess_functor.template_param_list.Count != right_guess_functor.template_param_list.Count
                    || left_guess_functor.await_modifier != right_guess_functor.await_modifier
                    || left_guess_functor.proto == null && right_guess_functor.proto != null
                    || left_guess_functor.proto != null && right_guess_functor.proto == null
                    || (left_guess_functor.proto != null && left_guess_functor.proto != right_guess_functor.proto)
                    || left_guess_functor.param_tail == null && right_guess_functor.param_tail != null
                    || left_guess_functor.param_tail != null && right_guess_functor.param_tail == null
                    || left_guess_functor.return_tail == null && right_guess_functor.return_tail != null
                    || left_guess_functor.return_tail != null && right_guess_functor.return_tail == null
                )
                {
                    return new ABnfGuessError(right_src, "要求是" + left_guess.GetValue() + ",不能是:" + right_guess.GetValue());
                }

                for (int i = 0; i < left_guess_functor.template_param_list.Count; ++i)
                {
                    var error = GuessTypeEqual(left_guess_functor.template_param_list[i], right_src, right_guess_functor.template_param_list[i], assign_or_call, is_return);
                    if (error != null) return error;
                }

                for (int i = 0; i < left_guess_functor.param_list.Count; ++i)
                {
                    var error = GuessTypeEqual(left_guess_functor.param_list[i], right_src, right_guess_functor.param_list[i], assign_or_call, is_return);
                    if (error != null) return error;
                }

                for (int i = 0; i < left_guess_functor.param_nullable_list.Count; ++i)
                {
                    if (left_guess_functor.param_nullable_list[i] != right_guess_functor.param_nullable_list[i])
                        return new ABnfGuessError(right_src, "要求是" + left_guess.GetValue() + ",不能是:" + right_guess.GetValue());
                }

                for (int i = 0; i < left_guess_functor.return_list.Count; ++i)
                {
                    var error = GuessTypeEqual(left_guess_functor.return_list[i], right_src, right_guess_functor.return_list[i], assign_or_call, is_return);
                    if (error != null) return error;
                }
                return null;
            }

            if (left_guess is ALittleScriptGuessClass)
            {
                if (right_guess is ALittleScriptGuessTemplate)
                {
                    var right_guess_template = right_guess as ALittleScriptGuessTemplate;
                    if (right_guess_template.template_extends != null)
                        right_guess = right_guess_template.template_extends;
                }

                if (!(right_guess is ALittleScriptGuessClass))
                    return new ABnfGuessError(right_src, "要求是" + left_guess.GetValue() + ",不能是:" + right_guess.GetValue());

                if (left_guess.GetValueWithoutConst() == right_guess.GetValueWithoutConst()) return null;

                var error = ALittleScriptUtility.IsClassSuper(((ALittleScriptGuessClass)right_guess).class_dec, left_guess.GetValue(), out bool result);
                if (error != null) return error;
                if (result) return null;

                return new ABnfGuessError(right_src, "要求是" + left_guess.GetValue() + ",不能是:" + right_guess.GetValue());
            }

            if (left_guess is ALittleScriptGuessStruct)
            {
                if (right_guess is ALittleScriptGuessTemplate)
                {
                    var right_guess_template = right_guess as ALittleScriptGuessTemplate;
                    if (right_guess_template.template_extends != null)
                        right_guess = right_guess_template.template_extends;
                }

                if (!(right_guess is ALittleScriptGuessStruct))
                {
                    return new ABnfGuessError(right_src, "要求是" + left_guess.GetValue() + ",不能是:" + right_guess.GetValue());
                }

                if (left_guess.GetValueWithoutConst() == right_guess.GetValueWithoutConst()) return null;

                var error = ALittleScriptUtility.IsStructSuper(((ALittleScriptGuessStruct)right_guess).struct_dec, left_guess.GetValue(), out bool result);
                if (error != null) return error;
                if (result) return null;

                return new ABnfGuessError(right_src, "要求是" + left_guess.GetValue() + ",不能是:" + right_guess.GetValue());
            }

            if (left_guess is ALittleScriptGuessTemplate)
            {
                var left_guess_template = left_guess as ALittleScriptGuessTemplate;
                if (left_guess_template.template_extends != null)
                {
                    var error = GuessTypeEqual(left_guess_template.template_extends, right_src, right_guess, assign_or_call, is_return);
                    if (error != null) return error;
                    return null;
                }
                else if (left_guess_template.is_class)
                {
                    if (right_guess is ALittleScriptGuessClass)
                    {
                        return null;
                    }
                    else if (right_guess is ALittleScriptGuessTemplate)
                    {
                        var right_guess_template = right_guess as ALittleScriptGuessTemplate;
                        if (right_guess_template.template_extends is ALittleScriptGuessClass || right_guess_template.is_class)
                            return null;
                    }
                    return new ABnfGuessError(right_src, "要求是" + left_guess.GetValue() + ",不能是:" + right_guess.GetValue());
                }
                else if (left_guess_template.is_struct)
                {
                    if (right_guess is ALittleScriptGuessStruct)
                    {
                        return null;
                    }
                    else if (right_guess is ALittleScriptGuessTemplate)
                    {
                        var right_guess_template = right_guess as ALittleScriptGuessTemplate;
                        if (right_guess_template.template_extends is ALittleScriptGuessStruct || right_guess_template.is_struct)
                            return null;
                    }
                    return new ABnfGuessError(right_src, "要求是" + left_guess.GetValue() + ",不能是:" + right_guess.GetValue());
                }
                else
                {
                    if (!assign_or_call && !is_return) return null;
                    
                    if (right_guess is ALittleScriptGuessTemplate)
                    {
                        if (left_guess.GetValue() == right_guess.GetValue())
                            return null;
                        if (left_guess.GetValueWithoutConst() == right_guess.GetValue())
                            return null;
                    }
                    return new ABnfGuessError(right_src, "要求是" + left_guess.GetValue() + ",不能是:" + right_guess.GetValue());
                }
            }

            return new ABnfGuessError(right_src, "要求是" + left_guess.GetValue() + ",不能是:" + right_guess.GetValue());
        }
    }

}
