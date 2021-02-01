using System;
using System.Collections.Generic;
using mylib.CLI;
using Xunit;

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
    }
}
