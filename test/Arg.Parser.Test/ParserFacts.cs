﻿using System;
using System.Linq;
using Xunit;

namespace Arg.Parser.Test
{
    public class ParserFacts
    {
        [Fact]
        public void should_throw_exception_when_argument_is_null()
        {
            var parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption('f')
                .EndCommand()
                .Build();

            Assert.Throws<ArgumentNullException>(() => parser.Parse(null));
        }

        [Fact]
        public void should_throw_exception_when_argument_contains_null()
        {
            var parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption('f')
                .EndCommand()
                .Build();

            Assert.Throws<ArgumentException>(() => parser.Parse(new string[] {null}));
        }

        [Theory]
        [InlineData("flag", "flag", "flag")]
        [InlineData("flag", "Flag", "flag")]
        [InlineData("flag", "Flag", "Flag")]
        [InlineData("Flag", "Flag", "Flag")]
        public void should_match_and_get_by_full_form(string supportFullForm, string inputFullForm,
            string getFlagFullForm)
        {
            var parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption(supportFullForm)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new[] {"--" + inputFullForm});
            Assert.True(result.IsSuccess);
            Assert.True(result.GetFlagValue("--" + getFlagFullForm));
        }

        [Theory]
        [InlineData('f', 'f', 'f')]
        [InlineData('f', 'F', 'f')]
        [InlineData('f', 'F', 'F')]
        [InlineData('F', 'f', 'f')]
        public void should_match_and_get_by_abbreviation_form(char supportAbbreviationForm, char inputAbbreviationForm,
            char getFlagAbbreviationForm)
        {
            var parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption(supportAbbreviationForm)
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new[] {"-" + inputAbbreviationForm.ToString()});
            Assert.True(result.IsSuccess);
            Assert.True(result.GetFlagValue("-" + getFlagAbbreviationForm.ToString()));
        }

        [Theory]
        [InlineData('f', "flag", "flag", 'f')]
        [InlineData('f', "flag", "Flag", 'F')]
        [InlineData('F', "flag", "flag", 'f')]
        [InlineData('f', "Flag", "flag", 'f')]
        public void should_build_full_and_abbreviation_form_then_match_full_form_then_get_by_abbreviation_form(
            char supportAbbreviationForm, string supportFullForm, string inputFullForm, char getFlagAbbreviationForm)
        {
            var parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption(supportAbbreviationForm, supportFullForm, "it is a flag")
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new[] {"--" + inputFullForm});
            Assert.True(result.IsSuccess);
            Assert.True(result.GetFlagValue("-" + getFlagAbbreviationForm));
        }

        [Theory]
        [InlineData("--no-edit", "--amend")]
        [InlineData("--amend", "--no-edit")]
        public void should_parse_mutiple_args_ignore_order(string input1, string input2)
        {
            var parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption("no-edit")
                .AddFlagOption("amend")
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new[] {input1, input2});
            Assert.True(result.IsSuccess);
            Assert.True(result.GetFlagValue(input1));
            Assert.True(result.GetFlagValue(input2));
        }

        [Fact]
        public void should_get_false_when_input_argument_not_exist()
        {
            var parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption('f', "flag", "it is a flag")
                .EndCommand()
                .Build();

            ArgsParsingResult result = parser.Parse(new string[] { });
            Assert.True(result.IsSuccess);
            Assert.False(result.GetFlagValue("-f"));
        }

        [Fact]
        public void should_get_error_when_input_arg_not_support_by_flag_option()
        {
            var parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption('f', "flag", "it is a flag")
                .EndCommand()
                .Build();

            var parseResult = parser.Parse(new[] {"-a"});

            Assert.False(parseResult.IsSuccess);
            var error = parseResult.Error;
            Assert.Equal(ParsingErrorCode.FreeValueNotSupported, error.Code);
            Assert.Equal("input argument is not supported", error.Detail);
            Assert.Equal("-a", error.Trigger);
        }

        [Fact]
        public void should_get_error_when_input_arg_too_short()
        {
            var parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption('f', "flag", "it is a flag")
                .EndCommand()
                .Build();

            var parseResult = parser.Parse(new[] {"-"});

            Assert.False(parseResult.IsSuccess);
            var error = parseResult.Error;
            Assert.Equal(ParsingErrorCode.FreeValueNotSupported, error.Code);
            Assert.Equal("argument too short", error.Detail);
            Assert.Equal("-", error.Trigger);
        }

        [Fact]
        public void should_get_error_when_input_arg_not_start_with_under_score()
        {
            var parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption('f', "flag", "it is a flag")
                .EndCommand()
                .Build();

            var parseResult = parser.Parse(new[] {"abc"});

            Assert.False(parseResult.IsSuccess);
            var error = parseResult.Error;
            Assert.Equal(ParsingErrorCode.FreeValueNotSupported, error.Code);
            Assert.Equal("argument must start with - or --", error.Detail);
            Assert.Equal("abc", error.Trigger);
        }

        [Theory]
        [InlineData("+")]
        [InlineData("-abc")]
        public void input_full_form_contain_invalid_character_or_start_with_dash_should_get_error(string input)
        {
            var parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption('f', "flag", "it is a flag")
                .EndCommand()
                .Build();

            var inputFlag = "--" + input;
            var parseResult = parser.Parse(new[] {inputFlag});

            Assert.False(parseResult.IsSuccess);
            var error = parseResult.Error;
            Assert.Equal(ParsingErrorCode.FreeValueNotSupported, error.Code);
            Assert.Equal("full form should only contain lower or upper letter, number, dash and underscore",
                error.Detail);
            Assert.Equal(inputFlag, error.Trigger);
        }

        [Theory]
        [InlineData('+')]
        [InlineData('_')]
        [InlineData('3')]
        public void abbreviation_form_contain_invalid_character_should_get_error(char input)
        {
            var parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption('f', "flag", "it is a flag")
                .EndCommand()
                .Build();

            var inputFlag = "-" + input;
            var parseResult = parser.Parse(new[] {inputFlag});

            Assert.False(parseResult.IsSuccess);
            var error = parseResult.Error;
            Assert.Equal(ParsingErrorCode.FreeValueNotSupported, error.Code);
            Assert.Equal("abbreviation form argument must only contains lower or upper letter", error.Detail);
            Assert.Equal(inputFlag, error.Trigger);
        }

        [Theory]
        [InlineData("-F", "--flag")]
        [InlineData("--flag", "--flAg")]
        [InlineData("-f", "-F")]
        public void should_get_duplicate_flag_when_input_argument_duplicate(string flag1, string flag2)
        {
            var parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption('f', "flag", "it is a flag")
                .EndCommand()
                .Build();

            var parseResult = parser.Parse(new[] {flag1, flag2});

            Assert.False(parseResult.IsSuccess);
            var error = parseResult.Error;
            Assert.Equal(ParsingErrorCode.DuplicateFlagsInArgs, error.Code);
            Assert.Equal("duplicate flag option in input arguments", error.Detail);
            Assert.Equal(flag1 + " " + flag2, error.Trigger);
        }

        [Fact]
        public void should_throw_argument_null_exception_when_get_flag_argument_is_null()
        {
            var parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption('f', "flag", "it is a flag")
                .EndCommand()
                .Build();

            var parseResult = parser.Parse(new[] {"-f"});

            Assert.True(parseResult.IsSuccess);
            Assert.Null(parseResult.Error);

            Assert.Throws<ArgumentNullException>(() => parseResult.GetFlagValue(null));
        }


        [Fact]
        public void should_throw_invalid_operation_exception_when_parse_error()
        {
            var parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption('f', "flag", "it is a flag")
                .EndCommand()
                .Build();

            var parseResult = parser.Parse(new[] {"--a"});

            Assert.False(parseResult.IsSuccess);

            Assert.Throws<InvalidOperationException>(() => parseResult.GetFlagValue("--a"));
        }

        [Fact]
        public void should_throw_argument_exception_when_parse_success_but_get_flag_syntax_error()
        {
            var parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption('f', "flag", "it is a flag")
                .EndCommand()
                .Build();

            var parseResult = parser.Parse(new[] {"-f"});

            Assert.True(parseResult.IsSuccess);

            Assert.Throws<ArgumentException>(() => parseResult.GetFlagValue("-3"));
        }

        [Theory]
        [InlineData("-a")]
        [InlineData("--version")]
        public void should_throw_argument_exception_when_parse_success_but_get_not_support_flag(string flag)
        {
            var parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption('f', "flag", "it is a flag")
                .EndCommand()
                .Build();

            var parseResult = parser.Parse(new[] {"-f"});

            Assert.True(parseResult.IsSuccess);

            Assert.Throws<ArgumentException>(() => parseResult.GetFlagValue(flag));
        }

        [Fact]
        public void should_get_free_value_not_supported_when_combine_arg_include_not_support_flag_option()
        {
            var parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption('r', "recursive", string.Empty)
                .AddFlagOption('f', "force", string.Empty)
                .EndCommand()
                .Build();

            string[] args = {"-rfa"};
            ArgsParsingResult result = parser.Parse(args);
            Assert.False(result.IsSuccess);
            Assert.Equal(ParsingErrorCode.FreeValueNotSupported,
                result.Error.Code);
            Assert.Equal("-rfa", result.Error.Trigger);
        }

        [Theory]
        [InlineData("-r")]
        [InlineData("-f")]
        [InlineData("-rf")]
        [InlineData("--force")]
        [InlineData("--recursive")]
        public void should_get_duplicate_flags_when_combine_arg_duplicate(string input)
        {
            var parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption('r', "recursive", string.Empty)
                .AddFlagOption('f', "force", string.Empty)
                .EndCommand()
                .Build();
            string[] args = {"-rf", input};
            ArgsParsingResult result = parser.Parse(args);
            Assert.False(result.IsSuccess);
            Assert.Equal(ParsingErrorCode.DuplicateFlagsInArgs,
                result.Error.Code);
            Assert.Equal($"-rf {input}", result.Error.Trigger);
        }

        [Fact]
        public void should_throw_argument_exception_when_parse_success_but_get_combine_flag()
        {
            var parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption('f', "flag", "it is a flag")
                .AddFlagOption('r', "recursive", "it is recursive")
                .EndCommand()
                .Build();

            var parseResult = parser.Parse(new[] {"-fr"});

            Assert.True(parseResult.IsSuccess);

            Assert.Throws<ArgumentException>(() => parseResult.GetFlagValue("-fr"));
        }

        [Fact]
        public void should_get_both_value_when_parse_combine_flag_success()
        {
            var parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption('f', "flag", "it is a flag")
                .AddFlagOption('r', "recursive", "it is recursive")
                .EndCommand()
                .Build();

            var parseResult = parser.Parse(new[] {"-fr"});

            Assert.True(parseResult.IsSuccess);

            Assert.True(parseResult.GetFlagValue("-f"));
            Assert.True(parseResult.GetFlagValue("-r"));
        }

        [Fact]
        public void should_get_value_when_parse_combine_flag_and_single_flag_mixed_success()
        {
            var parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption('f', "flag", "it is a flag")
                .AddFlagOption('r', "recursive", "it is recursive")
                .AddFlagOption('v')
                .EndCommand()
                .Build();

            var parseResult = parser.Parse(new[] {"-fr"});

            Assert.True(parseResult.IsSuccess);

            Assert.True(parseResult.GetFlagValue("-f"));
            Assert.True(parseResult.GetFlagValue("-r"));
            Assert.False(parseResult.GetFlagValue("-v"));
        }
        
        [Fact]
        public void command_symbol_should_be_null_when_parse_default_command_success()
        {
            var parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .EndCommand()
                .Build();

            var parseResult = parser.Parse(Array.Empty<string>());

            Assert.True(parseResult.IsSuccess);

            Assert.NotNull(parseResult.Command);
            Assert.Null(parseResult.Command.Symbol);
        }
        
        [Fact]
        public void command_should_be_null_when_parse_default_command_failure()
        {
            var parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .EndCommand()
                .Build();

            var parseResult = parser.Parse(new [] {"-f"});

            Assert.False(parseResult.IsSuccess);

            Assert.Null(parseResult.Command);
        }

        [Fact]
        public void should_get_metadata_when_parse_success()
        {
            var parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption('f', "flag", "flag description")
                .EndCommand()
                .Build();
            ArgsParsingResult result = parser.Parse(new [] {"--flag"});
            Assert.True(result.IsSuccess);
            IOptionDefinitionMetadata[] optionDefinitionMetadatas =
                result.Command.GetRegisteredOptionsMetadata().ToArray();
            IOptionDefinitionMetadata flagMetadata =
                optionDefinitionMetadatas.Single(
                    d => d.SymbolMetadata.FullForm.Equals("flag",
                        StringComparison.OrdinalIgnoreCase));
            Assert.Equal("flag", flagMetadata.SymbolMetadata.FullForm);
            Assert.Equal('f', flagMetadata.SymbolMetadata.Abbreviation);
            Assert.Equal("flag description", flagMetadata.Description);
        }

        [Fact]
        public void command_description_should_be_empty_string_when_command_is_default()
        {
            var parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption('f', "flag", "flag description")
                .EndCommand()
                .Build();
            
            ArgsParsingResult result = parser.Parse(new [] {"--flag"});
            Assert.True(result.IsSuccess);
            Assert.Equal(string.Empty, result.Command.Description);
        }
        
        [Fact]
        public void registered_options_should_be_empty_collection_when_command_no_flag_option()
        {
            var parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .EndCommand()
                .Build();
            
            ArgsParsingResult result = parser.Parse(new string[]{});
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Command.GetRegisteredOptionsMetadata());
        }
        
        [Fact]
        public void flag_description_should_be_empty_string_when_not_set_flag_description()
        {
            var parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption('f', "flag", null)
                .EndCommand()
                .Build();
            
            ArgsParsingResult result = parser.Parse(new [] {"--flag"});
            Assert.True(result.IsSuccess);
            IOptionDefinitionMetadata flagMetadata =
                result.Command.GetRegisteredOptionsMetadata().Single(
                    d => d.SymbolMetadata.FullForm.Equals("flag",
                        StringComparison.OrdinalIgnoreCase));
            Assert.Equal(string.Empty, flagMetadata.Description);
        }
    }
}