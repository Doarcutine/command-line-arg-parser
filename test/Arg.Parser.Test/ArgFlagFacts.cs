using System;
using System.Linq;
using Xunit;

namespace Arg.Parser.Test
{
    public class ArgFlagFacts
    {
        [Fact]
        public void throw_exception_when_full_form_and_abbreviation_form_both_null()
        {
            var e = Assert.Throws<ArgumentException>(() => new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption(null,null,null)
                .EndCommand()
                .Build());
            Assert.Equal("arg opt must have at least one form, full form or abbreviation form", e.Message);
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void throw_exception_when_not_supply_either_abbreviation_form_or_full_form(string fullForm)
        {
            var e = Assert.Throws<ArgumentException>(() => new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption(fullForm)
                .EndCommand()
                .Build());
            Assert.Equal("arg opt must have at least one form, full form or abbreviation form", e.Message);
        }
        
        [Theory]
        [InlineData("+")]
        [InlineData("-abc")]
        public void full_form_contain_invalid_character_or_start_with_dash_should_throw_exception(string fullForm)
        {
            var e = Assert.Throws<ArgumentException>(() => new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption(fullForm)
                .EndCommand()
                .Build());
            Assert.Equal("full form should only contain lower or upper letter," +
                         $" number, dash and underscore, but get '{fullForm}'", e.Message);
        }
        
        [Theory]
        [InlineData("a")]
        [InlineData("acAZ7_-")]
        [InlineData("_b")]
        public void full_form_should_only_contain_number_alpha_dash_underscore_and_without_dash_start(string fullForm)
        {
            var parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption(fullForm)
                .EndCommand()
                .Build();
            Assert.NotNull(parser);
        }
        
        [Theory]
        [InlineData('+')]
        [InlineData('_')]
        [InlineData('-')]
        [InlineData('3')]
        public void abbreviation_form_contain_invalid_character_should_throw_exception(char abbreviationForm)
        {
            var e = Assert.Throws<ArgumentException>(() => new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption(abbreviationForm)
                .EndCommand()
                .Build());
            Assert.Equal(
                $"abbreviation argument must and only have one lower or upper letter, but get: '{abbreviationForm}'",
                e.Message);
        }
        
        [Theory]
        [InlineData('a')]
        [InlineData('Z')]
        public void abbreviation_form_should_only_contain_alpha(char abbreviationForm)
        {
            var parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption(abbreviationForm)
                .EndCommand()
                .Build();
            Assert.NotNull(parser);
        }

        [Fact]
        public void should_save_and_replace_new_line_in_arg_flag_description()
        {
            var parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption('v', "version", "this is version info\nnew line")
                .EndCommand()
                .Build();
            var helpInfo = parser.HelpInfo().ToList();
            Assert.Single(helpInfo);
            Assert.Equal("v    version    this is version info new line", helpInfo.First());
        }

        [Fact]
        public void should_throw_exception_when_full_form_duplicate()
        {
            var fullForm = "abc";
            var e = Assert.Throws<ArgumentException>(() => new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption(fullForm)
                .AddFlagOption(fullForm.ToUpper())
                .EndCommand()
                .Build());
            Assert.Equal("duplicate full form",e.Message);
        }
        
        [Fact]
        public void should_throw_exception_when_abbreviation_form_duplicate()
        {
            var abbreviationForm = 'a';
            var e = Assert.Throws<ArgumentException>(() => new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption(abbreviationForm)
                .AddFlagOption(char.ToUpper(abbreviationForm))
                .EndCommand()
                .Build());
            Assert.Equal("duplicate abbreviation name",e.Message);
        }

        [Fact]
        public void should_throw_invalid_operation_exception_when_begin_default_command_more_than_once()
        {
            Assert.Throws<InvalidOperationException>(() => new ArgsParserBuilder()
                .BeginDefaultCommand()
                .EndCommand()
                .BeginDefaultCommand()
                .EndCommand()
                .Build());
        }
    }
}
