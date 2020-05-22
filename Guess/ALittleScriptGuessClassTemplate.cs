
namespace ALittle
{
    public class ALittleScriptGuessClassTemplate : ALittleScriptGuessTemplate
    {
        public ALittleScriptGuessClassTemplate(ALittleScriptTemplatePairDecElement p_template_pair_dec
            , ABnfGuess p_template_extends
            , bool p_is_class, bool p_is_struct)
            : base(p_template_pair_dec, p_template_extends, p_is_class, p_is_struct)
        {}


        public override ABnfGuess Clone()
        {
            var guess = new ALittleScriptGuessClassTemplate(template_pair_dec, template_extends, is_class, is_struct);
            guess.UpdateValue();
            return guess;
        }
    }
}
