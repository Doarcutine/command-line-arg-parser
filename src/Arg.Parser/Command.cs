namespace Arg.Parser
{
    public class Command
    {
        string optionFullName;
        string optionAbbrName;

        public string CommandName { get; set; }
        public string OptionFullName
        {
            get
            {
                return optionFullName;
            }
            set
            {
                Validator.ValidateOptionFullNameFormat(value);
                optionFullName = value;
            }
        }
        public string OptionAbbrName { 
            get
            {
                return optionAbbrName;
            }
            set
            {
                Validator.ValidateOptionAbbrNameFormat(value);
                optionAbbrName = value;
            } 
        }
        public string Description { get; set; }

        public Command(string commandName, string optionFullName, string optionAbbrName, string description)
        {
            CommandName = commandName;
            this.optionFullName = optionFullName;
            this.optionAbbrName = optionAbbrName;
            Description = description;
        }

        public Command(string optionFullName, string optionAbbrName, string description)
        {
            this.optionFullName = optionFullName;
            this.optionAbbrName = optionAbbrName;
            Description = description;

            Validator.ValidateOptionAtLeastHasOneName(this);
        }
    }
}