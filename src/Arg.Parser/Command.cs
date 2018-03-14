namespace Arg.Parser
{
    public class Command
    {
        public string CommandName { get; set; }
        public string OptionFullName { get; set; }
        public string OptionAbbrName { get; set; }
        public string Description { get; set; }

        public Command(string commandName, string optionFullName, string optionAbbrName, string description)
        {
            CommandName = commandName;
            OptionFullName = optionFullName;
            OptionAbbrName = optionAbbrName;
            Description = description;
        }
    }
}