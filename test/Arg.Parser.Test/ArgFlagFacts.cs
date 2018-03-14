using System;
using Xunit;

namespace Arg.Parser.Test
{
    public class ArgFlagFacts
    {
        [Theory]
        [InlineData(null)]
        [InlineData("+")]
        [InlineData("-abc")]
        public void long_name_contain_invalid_character_or_start_with_dash_should_throw_exception(string longName)
        {
            Assert.Throws<ApplicationException>(() => new ArgsParserBuilder()
                .AddFlagOption(longName)
                .Build());
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
            Assert.Throws<ApplicationException>(() => new ArgsParserBuilder()
                .AddFlagOption(shortName)
                .Build());
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
        public void should_build_and_match_long_name()
        {
            var flag = "version";
            var parser = new ArgsParserBuilder()
                .AddFlagOption(flag)
                .Build();
            
            ArgsParsingResult result = parser.Parse(new [] { "--" + flag });
            Assert.True(result.IsSuccess);
            Assert.True(result.GetFlagValue("-f"));
        }
    }
}
