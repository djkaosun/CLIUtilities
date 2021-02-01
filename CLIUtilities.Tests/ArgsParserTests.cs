using System;
using System.Collections.Generic;
using mylib.CLI;
using Xunit;
using CLIUtilities.Tests.TestDatas;

namespace CLIUtilities.Tests
{
    public class ArgsParserTests
    {
        [Theory]
        [ClassData(typeof(ValidOptionSyntaxesAndArgs))]
        public void ParseTest_Valid(string[] args, IDictionary<string, IList<string>> expected, params OptionSyntax[] syntaxes)
        {
            mylib.CLI.OptionSyntaxDictionary syntaxDictionary = new OptionSyntaxDictionary();
            foreach (OptionSyntax syntax in syntaxes)
            {
                syntaxDictionary[syntax.FullName] = syntax;
            }

            ArgsParser argsParser = new ArgsParser(syntaxDictionary);
            Assert.Equal(expected, argsParser.Parse(args));
        }

        [Theory]
        [ClassData(typeof(ValidOptionSyntaxesAndInvalidArgs))]
        public void ParseTest_Invalid(string[] args, params OptionSyntax[] syntaxes)
        {
            OptionSyntaxDictionary syntaxDictionary = new OptionSyntaxDictionary();
            foreach (OptionSyntax syntax in syntaxes)
            {
                syntaxDictionary[syntax.FullName] = syntax;
            }

            ArgsParser argsParser = new ArgsParser(syntaxDictionary);
            Assert.Throws<ArgumentException>(() => { argsParser.Parse(args); });
        }

        [Fact]
        public void Parse_IfNotContainsOptionInArgsAndContainsInConfig_ComplementsByConfig()
        {
            // arrange
            var syntaxDictionary = new OptionSyntaxDictionary();
            syntaxDictionary.Add(new OptionSyntax("option","description",1));
            var configs = new Dictionary<string, string[]>();
            configs["option"] = new string[] { "configValue" };
            var argsParser = new ArgsParser() {
                OptionSyntaxDictionary = syntaxDictionary,
                ConfigOptionDictionary = configs
            };

            var args = new string[] { };

            // act
            var options = argsParser.Parse(args);
            var actual = options["option"][0];

            // assert
            Assert.Equal("configValue", actual);
        }

        [Fact]
        public void Parse_IfNotContainsOptionInBothArgsAndConfig_PreemptsByArgs()
        {
            // arrange
            var syntaxDictionary = new OptionSyntaxDictionary();
            syntaxDictionary.Add(new OptionSyntax("option", "description", 1));
            var configs = new Dictionary<string, string[]>();
            configs["option"] = new string[] { "configValue" };
            var argsParser = new ArgsParser()
            {
                OptionSyntaxDictionary = syntaxDictionary,
                ConfigOptionDictionary = configs
            };

            var args = new string[] { "--option", "cmdValue" };

            // act
            var options = argsParser.Parse(args);
            var actual = options["option"][0];

            // assert
            Assert.Equal("cmdValue", actual);

        }

        [Fact]
        public void Parse_IfUndefinedConfigOption_ThrowsInvalidOperationException()
        {
            // arrange
            var syntaxDictionary = new OptionSyntaxDictionary();
            var configs = new Dictionary<string, string[]>();
            configs["option"] = new string[] { "configValue" };
            var argsParser = new ArgsParser()
            {
                OptionSyntaxDictionary = syntaxDictionary,
                ConfigOptionDictionary = configs
            };

            var args = new string[] { };

            // act
            // assert
            Assert.Throws<InvalidOperationException>(()=> { argsParser.Parse(args); });
        }

        [Fact]
        public void Parse_IfConfigOptionValueIsTooMatch_ThrowsInvalidOperationException()
        {
            // arrange
            var syntaxDictionary = new OptionSyntaxDictionary();
            syntaxDictionary.Add(new OptionSyntax("option", "description", 1));
            var configs = new Dictionary<string, string[]>();
            configs["option"] = new string[] { "configValue1", "configValue2" };
            var argsParser = new ArgsParser()
            {
                OptionSyntaxDictionary = syntaxDictionary,
                ConfigOptionDictionary = configs
            };

            var args = new string[] { };

            // act
            // assert
            Assert.Throws<InvalidOperationException>(() => { argsParser.Parse(args); });
        }

        [Fact]
        public void Parse_IfConfigOptionValueIsLess_ThrowsInvalidOperationException()
        {
            // arrange
            var syntaxDictionary = new OptionSyntaxDictionary();
            syntaxDictionary.Add(new OptionSyntax("option", "description", 2));
            var configs = new Dictionary<string, string[]>();
            configs["option"] = new string[] { "configValue" };
            var argsParser = new ArgsParser()
            {
                OptionSyntaxDictionary = syntaxDictionary,
                ConfigOptionDictionary = configs
            };

            var args = new string[] { };

            // act
            // assert
            Assert.Throws<InvalidOperationException>(() => { argsParser.Parse(args); });
        }

        [Fact]
        public void Parse_IfConfigDefaultOptionAndValueCountIsZero_ThrowsNoException()
        {
            // arrange
            var syntaxDictionary = new OptionSyntaxDictionary();
            syntaxDictionary.Add(new OptionSyntax("option", "description", 0, null, false, false, true));
            var configs = new Dictionary<string, string[]>();
            configs["option"] = new string[] { "configValue" };
            var argsParser = new ArgsParser()
            {
                OptionSyntaxDictionary = syntaxDictionary,
                ConfigOptionDictionary = configs
            };

            var args = new string[] { };

            // act
            // assert
            argsParser.Parse(args); // no exception
        }
    }
}
