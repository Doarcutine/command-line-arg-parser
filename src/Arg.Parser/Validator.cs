using System;
using System.Text.RegularExpressions;

namespace Arg.Parser
{
    public class Validator
    {
        public static void ValidateOptionFullNameFormat(string optionFullName)
        {
            if(optionFullName != null || optionFullName.Length <= 0){
                throw new FormatException("Option length must more than 1");
            }
            if(optionFullName[0] == '-'){
                throw new FormatException("First character can not be -");
            }
            Regex regex = new Regex(@"^[a-zA-Z0-9_-]+$");
            if(!regex.IsMatch(optionFullName)){
                throw new FormatException("Option full name can only contain uppercase and lowercase letters,numbers,_,-");
            }
        }

        public static void ValidateOptionAbbrNameFormat(string optionAbbrName)
        {
            Regex regex = new Regex(@"^[a-zA-Z]+$");
            if (!regex.IsMatch(optionAbbrName))
            {
                throw new FormatException("Option abbr name can only contain uppercase and lowercase letters");
            }
        }

        public static void ValidateOptionAtLeastHasOneName(Command command)
        {
            if(command.OptionFullName == null && command.OptionAbbrName == null){
                throw new FormatException("Option at least has one name(full name or abbreviation form name)");
            }
        }
    }
}
