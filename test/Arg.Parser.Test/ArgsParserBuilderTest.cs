using System;
using Xunit;

namespace Arg.Parser.Test
{
    public class ArgsParserBuilderTest
    {
        [Fact]
        public void should_create_parser_when_get_valid_command(){
            Parser parser = new ArgsParserBuilder()
                .AddFlagOption("Fl-a1g","f","this is description")
                .Build();
        }

        [Fact]
        public void should_create_parser_when_only_get_valid_full_name()
        {
            Parser parser = new ArgsParserBuilder()
                .AddFlagOption("flag", null, "this is description")
                .Build();
        }

        [Fact]
        public void should_create_parser_when_only_get_valid_abbr_name()
        {
            Parser parser = new ArgsParserBuilder()
                .AddFlagOption(null, "f", "this is description")
                .Build();
        }

        //[Fact]
        //public void should_not_create_parser_when_both_full_name_and_abbr_name_are_null()
        //{
        //    Parser parser = new ArgsParserBuilder()
        //        .AddFlagOption(null, null, "this is description")
        //        .Build();
        //}

        //[Fact]
        //public void should_not_create_parser_when_full_name_is_invalid(){
        //    Parser parser = new ArgsParserBuilder()
        //        .AddFlagOption("-flag", "f", "this is description")
        //        .Build();
        //}
    }
}
