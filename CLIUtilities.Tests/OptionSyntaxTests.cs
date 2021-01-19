using System;
using mylib.CLI;
using Xunit;

namespace CLIUtilities.Tests
{
    public class OptionSyntaxTests
    {
        [Theory]
        [InlineData(new char[] { 'c', 'b', 'a' })]
        public void Aliases_PassUnsortedAliasArrayToConstructor_ReturnsSortedAliasArray(char[] stubArray)
        {
            // arrange
            var expectedArray = new char[stubArray.Length];
            Array.Copy(stubArray, expectedArray, stubArray.Length);
            Array.Sort(expectedArray);

            // act
            var syntax = new OptionSyntax("FullName", "Description", 0, stubArray);
            var actual = syntax.Aliases;

            // assert
            Assert.Equal(expectedArray, actual);
        }

        [Fact]
        public void Aliases_PassNullToConstructorAsAliasArray_ReturnsNull()
        {
            // arrange

            // act
            var syntax = new OptionSyntax("FullName", "Description", 0, null);
            var actual = syntax.Aliases;

            // assert
            Assert.Null(actual);
        }

        [Fact]
        public void Description_PassEmptyStringToConstructorAsDescription_ReturnsNull()
        {
            // arrange

            // act
            var syntax = new OptionSyntax("FullName", String.Empty, 0, null);
            var actual = syntax.Aliases;

            // assert
            Assert.Null(actual);
        }

        [Fact]
        public void Constructor_MinimumArguments_DoseNotError()
        {
            // arrange

            // act

            // assert
            //Assert.DoesNotThrow(() => { new OptionSyntax("a", null); });
            _ = new OptionSyntax("FullName", null);
        }

        [Fact]
        public void Constructor_MaxmumArguments_DoseNotError()
        {
            // arrange

            // act

            // assert
            _ = new OptionSyntax("FullName", "Description", 1, new char[] { 'b', 'c', 'a' }, true, true, true);
        }

        [Fact]
        public void Equality_NormalCase_ReturnValidResult()
        {
            // arrenge
            var syntax1 = new OptionSyntax();
            var syntax2 = new OptionSyntax();

            // act

            // assert
            // Assert ごとに別テストにしたほうが良い？
            Assert.True(syntax1 == syntax2);
            Assert.False(syntax1 != syntax2);
            Assert.True(syntax1.Equals(syntax2));
            Assert.True(OptionSyntax.Equals(syntax1, syntax2));
            Assert.Equal(syntax2.GetHashCode(), syntax1.GetHashCode());
        }

        [Fact]
        public void Equality_CastToBaseClass_ReturnValidResult()
        {
            // arrenge
            object object1 = new OptionSyntax();
            object object2 = new OptionSyntax();

            // act

            // assert
            Assert.False(object1 == object2);
            Assert.True(object1 != object2);
            Assert.True(object1.Equals(object2));
            Assert.True(OptionSyntax.Equals(object1,object2));
            Assert.True(OptionSyntax.Equals(null, null));
            Assert.True(Object.Equals(object1, object2));
            Assert.True(Object.Equals(null, null));
            Assert.Equal(object2.GetHashCode(), object1.GetHashCode());
        }

        [Theory]
        [InlineData(new char[] { 'c', 'b', 'a' })]
        public void Equality_PassUnsortedAliasArrayToConstructor_JudgedAsEqual(char[] stubArray)
        {
            // arrange
            var sortedArray = new char[stubArray.Length];
            Array.Copy(stubArray, sortedArray, stubArray.Length);
            Array.Sort(sortedArray);
            var syntax1 = new OptionSyntax("a", "a", 1, stubArray, true, true, true);
            var syntax2 = new OptionSyntax("a", "a", 1, sortedArray, true, true, true);

            // act

            // assert
            Assert.True(syntax1 == syntax2);
            Assert.False(syntax1 != syntax2);
            Assert.True(syntax1.Equals(syntax2));
            Assert.True(OptionSyntax.Equals(syntax1, syntax2));
            Assert.Equal(syntax2.GetHashCode(), syntax1.GetHashCode());
        }

        [Fact]
        public void Equality_DifferentFullName_JudgeAsNotEqual()
        {
            // arrenge
            var syntax1 = new OptionSyntax("a", "a", 1, new char[] { 'a', 'b', 'c' }, true, true, true);
            var syntax2 = new OptionSyntax("b", "a", 1, new char[] { 'a', 'b', 'c' }, true, true, true);

            // act

            // assert
            Assert.False(syntax1 == syntax2);
            Assert.True(syntax1 != syntax2);
            Assert.False(syntax1.Equals(syntax2));
            Assert.False(OptionSyntax.Equals(syntax1, syntax2));
            Assert.NotEqual(syntax2.GetHashCode(), syntax1.GetHashCode());
        }

        [Fact]
        public void Equality_DifferentDescription_JudgeAsNotEqual()
        {
            // arrenge
            var syntax1 = new OptionSyntax("a", "a", 1, new char[] { 'a', 'b', 'c' }, true, true, true);
            var syntax2 = new OptionSyntax("a", "b", 1, new char[] { 'a', 'b', 'c' }, true, true, true);

            // act

            // assert
            Assert.False(syntax1 == syntax2);
            Assert.True(syntax1 != syntax2);
            Assert.False(syntax1.Equals(syntax2));
            Assert.False(OptionSyntax.Equals(syntax1, syntax2));
            Assert.NotEqual(syntax2.GetHashCode(), syntax1.GetHashCode());

        }

        [Fact]
        public void Equality_DifferentValueCount_JudgeAsNotEqual()
        {
            // arrenge
            var syntax1 = new OptionSyntax("a", "a", 1, new char[] { 'a', 'b', 'c' }, true, true, true);
            var syntax2 = new OptionSyntax("a", "a", 2, new char[] { 'a', 'b', 'c' }, true, true, true);

            // act

            // assert
            Assert.False(syntax1 == syntax2);
            Assert.True(syntax1 != syntax2);
            Assert.False(syntax1.Equals(syntax2));
            Assert.False(OptionSyntax.Equals(syntax1, syntax2));
            Assert.NotEqual(syntax2.GetHashCode(), syntax1.GetHashCode());

        }

        [Fact]
        public void Equality_DifferentAliasChar_JudgeAsNotEqual()
        {
            // arrenge
            var syntax1 = new OptionSyntax("a", "a", 1, new char[] { 'a', 'b', 'c' }, true, true, true);
            var syntax2 = new OptionSyntax("a", "a", 1, new char[] { 'A', 'b', 'c' }, true, true, true);

            // act

            // assert
            Assert.False(syntax1 == syntax2);
            Assert.True(syntax1 != syntax2);
            Assert.False(syntax1.Equals(syntax2));
            Assert.False(OptionSyntax.Equals(syntax1, syntax2));
            Assert.NotEqual(syntax2.GetHashCode(), syntax1.GetHashCode());

        }

        [Fact]
        public void Equality_DifferentAliasCount_JudgeAsNotEqual()
        {
            // arrenge
            var syntax1 = new OptionSyntax("a", "a", 1, new char[] { 'a', 'b', 'c' }, true, true, true);
            var syntax2 = new OptionSyntax("a", "a", 1, new char[] { 'a', 'b' }, true, true, true);

            // act

            // assert
            Assert.False(syntax1 == syntax2);
            Assert.True(syntax1 != syntax2);
            Assert.False(syntax1.Equals(syntax2));
            Assert.False(OptionSyntax.Equals(syntax1, syntax2));
            Assert.NotEqual(syntax2.GetHashCode(), syntax1.GetHashCode());

        }

        [Fact]
        public void Equality_DifferentIsCaseSensitive_JudgeAsNotEqual()
        {
            // arrenge
            var syntax1 = new OptionSyntax("a", "a", 1, new char[] { 'a', 'b', 'c' }, true, true, true);
            var syntax2 = new OptionSyntax("a", "a", 1, new char[] { 'a', 'b', 'c' }, false, true, true);

            // act

            // assert
            Assert.False(syntax1 == syntax2);
            Assert.True(syntax1 != syntax2);
            Assert.False(syntax1.Equals(syntax2));
            Assert.False(OptionSyntax.Equals(syntax1, syntax2));
            Assert.NotEqual(syntax2.GetHashCode(), syntax1.GetHashCode());

        }

        [Fact]
        public void Equality_DifferentIsMandatory_JudgeAsNotEqual()
        {
            // arrenge
            var syntax1 = new OptionSyntax("a", "a", 1, new char[] { 'a', 'b', 'c' }, true, true, true);
            var syntax2 = new OptionSyntax("a", "a", 1, new char[] { 'a', 'b', 'c' }, true, false, true);

            // act

            // assert
            Assert.False(syntax1 == syntax2);
            Assert.True(syntax1 != syntax2);
            Assert.False(syntax1.Equals(syntax2));
            Assert.False(OptionSyntax.Equals(syntax1, syntax2));
            Assert.NotEqual(syntax2.GetHashCode(), syntax1.GetHashCode());

        }

        [Fact]
        public void Equality_DifferentIsDefault_JudgeAsNotEqual()
        {
            // arrenge
            var syntax1 = new OptionSyntax("a", "a", 1, new char[] { 'a', 'b', 'c' }, true, true, true);
            var syntax2 = new OptionSyntax("a", "a", 1, new char[] { 'a', 'b', 'c' }, true, true, false);

            // act

            // assert
            Assert.False(syntax1 == syntax2);
            Assert.True(syntax1 != syntax2);
            Assert.False(syntax1.Equals(syntax2));
            Assert.False(OptionSyntax.Equals(syntax1, syntax2));
            Assert.NotEqual(syntax2.GetHashCode(), syntax1.GetHashCode());
        }

        [Fact]
        public static void GetHelp_RunMethod_ReturnsValidHelpString()
        {
            OptionSyntax syntax;

            syntax = new OptionSyntax("", null);
            Assert.Contains("(Description is null.)", syntax.GetHelp());

            syntax = new OptionSyntax("a", "");
            Assert.Contains("(Description is null.)", syntax.GetHelp());

            syntax = new OptionSyntax("a", "a");
            Assert.DoesNotContain("(Description is null.)", syntax.GetHelp());

            syntax = new OptionSyntax("a", "a", 1, new char[] { 'a', 'b', 'c' }, false, false, false);
            Assert.DoesNotContain("[Mandatory]",syntax.GetHelp());

            syntax = new OptionSyntax("a", "a", 1, new char[] { 'a', 'b', 'c' }, false, true, false);
            Assert.Contains("[Mandatory]", syntax.GetHelp());
        }
    }
}
