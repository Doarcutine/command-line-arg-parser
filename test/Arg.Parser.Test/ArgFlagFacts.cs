using System;
using System.Linq;
using Xunit;

namespace Arg.Parser.Test
{
    public class ArgFlagFacts
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void throw_exception_when_not_supply_either_short_name_or_long_name(string longName)
        {
            var e = Assert.Throws<ApplicationException>(() => new ArgsParserBuilder()
                .AddFlagOption(longName)
                .Build());
            Assert.Equal("arg opt must have at least one name, long or short", e.Message);
        }
        
        [Theory]
        [InlineData("+")]
        [InlineData("-abc")]
        public void long_name_contain_invalid_character_or_start_with_dash_should_throw_exception(string longName)
        {
            var e = Assert.Throws<ApplicationException>(() => new ArgsParserBuilder()
                .AddFlagOption(longName)
                .Build());
            Assert.Equal($"long name should only contain lower or upper letter," +
                         $" number, dash and underscore, but get '{longName}'", e.Message);
        }
        
        [Theory]
        [InlineData("a")]
        [InlineData("acAZ7_-")]
        [InlineData("_b")]
        public void long_name_should_only_contain_number_alpha_dash_underscore_and_without_dash_start(string longName)
        {
            var parser = new ArgsParserBuilder()
                .AddFlagOption(longName)
                .Build();
            Assert.NotNull(parser);
        }
        
        [Theory]
        [InlineData('+')]
        [InlineData('_')]
        [InlineData('-')]
        [InlineData('3')]
        public void short_name_contain_invalid_character_should_throw_exception(char shortName)
        {
            var e = Assert.Throws<ApplicationException>(() => new ArgsParserBuilder()
                .AddFlagOption(shortName)
                .Build());
            Assert.Equal(
                $"short argument must and only have one lower or upper letter, but get: '{shortName}'",
                e.Message);
        }
        
        [Theory]
        [InlineData('a')]
        [InlineData('Z')]
        public void short_name_should_only_contain_alpha(char shortName)
        {
            var parser = new ArgsParserBuilder()
                .AddFlagOption(shortName)
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
        public void should_throw_exception_when_add_more_than_one_flag_option()
        {
            var e = Assert.Throws<ApplicationException>(() => new ArgsParserBuilder()
                .AddFlagOption('v')
                .AddFlagOption('f')
                .Build());
            Assert.Equal("only support one flag for now", e.Message);
        }
    }
}
