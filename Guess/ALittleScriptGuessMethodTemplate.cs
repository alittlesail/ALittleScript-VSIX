
namespace ALittle
{
    public class ALittleScriptGuessMethodTemplate : ALittleScriptGuessTemplate
    {
        public ALittleScriptGuessMethodTemplate(ALittleScriptTemplatePairDecElement p_template_pair_dec
            , ABnfGuess p_template_extends
            , bool p_is_class, bool p_is_struct, bool p_is_const)
            : base(p_template_pair_dec, p_template_extends, p_is_class, p_is_struct, p_is_const)
        { }


        public override ABnfGuess Clone()
        {
            var guess = new ALittleScriptGuessMethodTemplate(template_pair_dec, template_extends, is_class, is_struct, is_const);
            guess.UpdateValue();
            return guess;
        }
    }
}
