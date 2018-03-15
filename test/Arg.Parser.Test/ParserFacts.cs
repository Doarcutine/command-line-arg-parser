using System;
using Xunit;

namespace Arg.Parser.Test
{
    public class ParserFacts
    {
        [Theory]
        [InlineData("flag","flag","flag")]
        [InlineData("flag","Flag","flag")]
        [InlineData("flag","Flag","Flag")]
        [InlineData("Flag","Flag","Flag")]
        public void should_match_and_get_by_long_name(string supportLongName, string inputLongName, string getFlagLongName)
        {
            var parser = new ArgsParserBuilder()
                .AddFlagOption(supportLongName)
                .Build();
            
            ArgsParsingResult result = parser.Parse(new [] { "--" + inputLongName });
            Assert.True(result.IsSuccess);
            Assert.True(result.GetFlagValue("--" + getFlagLongName));
        }
        
        [Theory]
        [InlineData('f','f','f')]
        [InlineData('f','F','f')]
        [InlineData('f','F','F')]
        [InlineData('F','f','f')]
        public void should_match_and_get_by_short_name(char supportShortName, char inputShortName, char getFlagShortName)
        {
            var parser = new ArgsParserBuilder()
                .AddFlagOption(supportShortName)
                .Build();
            
            ArgsParsingResult result = parser.Parse(new [] { "-" + inputShortName.ToString() });
            Assert.True(result.IsSuccess);
            Assert.True(result.GetFlagValue("-" + getFlagShortName.ToString()));
        }
        
        [Theory]
        [InlineData('f',"flag","flag",'f')]
        [InlineData('f',"flag","Flag",'F')]
        [InlineData('F',"flag","flag",'f')]
        [InlineData('f',"Flag","flag",'f')]
        public void should_build_long_and_short_name_then_match_long_name_then_get_by_short_name(char supportShortName, string supportLongName, string inputLongName, char getFlagShortName)
        {
            var parser = new ArgsParserBuilder()
                .AddFlagOption(supportShortName, supportLongName, "it is a flag")
                .Build();
            
            ArgsParsingResult result = parser.Parse(new [] { "--" + inputLongName });
            Assert.True(result.IsSuccess);
            Assert.True(result.GetFlagValue("-" + getFlagShortName));
        }
        
        [Fact]
        public void not_support_mixed_short_name()
        {
            var parser = new ArgsParserBuilder()
                .AddFlagOption('f')
                .Build();
            
            Assert.Throws<ApplicationException>(() => parser.Parse(new [] { "--fr" }));
        }
        
        [Fact]
        public void should_get_false_when_input_argument_not_exist()
        {
            var parser = new ArgsParserBuilder()
                .AddFlagOption('f', "flag", "it is a flag")
                .Build();
            
            ArgsParsingResult result = parser.Parse(new string[]{});
            Assert.True(result.IsSuccess);
            Assert.False(result.GetFlagValue("-f"));
        }
        
        [Fact]
        public void should_throw_exception_when_input_arg_not_support_by_flag_option()
        {
            var parser = new ArgsParserBuilder()
                .AddFlagOption('f', "flag", "it is a flag")
                .Build();
            
            var e = Assert.Throws<ApplicationException>(() => parser.Parse(new string[]{"-a"}));
            
            Assert.Equal("input argument are not supported: -a", e.Message);
        }
        
        [Fact]
        public void should_throw_exception_when_input_arg_too_short()
        {
            var parser = new ArgsParserBuilder()
                .AddFlagOption('f', "flag", "it is a flag")
                .Build();
            
            var e = Assert.Throws<ApplicationException>(() => parser.Parse(new string[]{"-"}));
            
            Assert.Equal("argument too short", e.Message);
        }
        
        [Fact]
        public void should_throw_exception_when_input_arg_not_start_with_under_score()
        {
            var parser = new ArgsParserBuilder()
                .AddFlagOption('f', "flag", "it is a flag")
                .Build();
            
            var e = Assert.Throws<ApplicationException>(() => parser.Parse(new string[]{"abc"}));
            
            Assert.Equal("argument must start with - or --", e.Message);
        }
        
        [Theory]
        [InlineData("+")]
        [InlineData("-abc")]
        public void input_long_name_contain_invalid_character_or_start_with_dash_should_throw_exception(string input)
        {
            var parser = new ArgsParserBuilder()
                .AddFlagOption('f', "flag", "it is a flag")
                .Build();

            var e = Assert.Throws<ApplicationException>(() => parser.Parse(new string[]{"--" + input}));
            
            Assert.Equal($"long name should only contain lower or upper letter," +
                         $" number, dash and underscore, but get '{input}'", e.Message);
        }
        
        [Theory]
        [InlineData('+')]
        [InlineData('_')]
        [InlineData('3')]
        public void short_name_contain_invalid_character_should_throw_exception(char input)
        {
            var parser = new ArgsParserBuilder()
                .AddFlagOption('f', "flag", "it is a flag")
                .Build();

            var e = Assert.Throws<ApplicationException>(() => parser.Parse(new string[]{"-" + input}));
            
            Assert.Equal(
                $"short argument must and only have one lower or upper letter, but get: '{input}'",
                e.Message);
        }
    }
}