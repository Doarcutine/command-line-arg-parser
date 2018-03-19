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
                .AddFlagOption(null,null,null)
                .Build());
            Assert.Equal("arg opt must have at least one form, full form or abbreviation form", e.Message);
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void throw_exception_when_not_supply_either_abbreviation_form_or_full_form(string fullForm)
        {
            var e = Assert.Throws<ArgumentException>(() => new ArgsParserBuilder()
                .AddFlagOption(fullForm)
                .Build());
            Assert.Equal("arg opt must have at least one form, full form or abbreviation form", e.Message);
        }
        
        [Theory]
        [InlineData("+")]
        [InlineData("-abc")]
        public void full_form_contain_invalid_character_or_start_with_dash_should_throw_exception(string fullForm)
        {
            var e = Assert.Throws<ArgumentException>(() => new ArgsParserBuilder()
                .AddFlagOption(fullForm)
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
                .AddFlagOption(fullForm)
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
                .AddFlagOption(abbreviationForm)
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
                .AddFlagOption(abbreviationForm)
                .Build();
            Assert.NotNull(parser);
        }

        [Fact]
        public void should_save_and_replace_new_line_in_arg_flag_description()
        {
            var parser = new ArgsParserBuilder()
                .AddFlagOption('v', "version", "this is version info\nnew line")
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
                .AddFlagOption(fullForm)
                .AddFlagOption(fullForm.ToUpper())
                .Build());
            Assert.Equal("duplicate full form",e.Message);
        }
        
        [Fact]
        public void should_throw_exception_when_abbreviation_form_duplicate()
        {
            var abbreviationForm = 'a';
            var e = Assert.Throws<ArgumentException>(() => new ArgsParserBuilder()
                .AddFlagOption(abbreviationForm)
                .AddFlagOption(char.ToUpper(abbreviationForm))
                .Build());
            Assert.Equal("duplicate abbreviation name",e.Message);
        }
    }
}
