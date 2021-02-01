using System;
using System.Collections;
using System.Collections.Generic;
using mylib.CLI;

namespace CLIUtilities.Tests.TestDatas
{
    class ValidOptionSyntaxes : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            List<object[]> resultList = new List<object[]>();
            foreach (object[] obj in new ValidOptionSyntaxesMulti())
            {
                foreach (OptionSyntax syntax in obj)
                {
                    resultList.Add(new object[] { syntax });
                }
            }
            return resultList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    class ValidOptionSyntaxesMulti : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {
                new OptionSyntax("aa", null),
                new OptionSyntax("bb", ""),
                new OptionSyntax("cc", "a"),
                new OptionSyntax("dd", "a", 2),
                new OptionSyntax("ee", "a", 2, new char[0]),
                new OptionSyntax("ff", "a", 2, new char[] { 'a' }),
                new OptionSyntax("gg", "a", 2, new char[] { 'b' }),
                new OptionSyntax("h", "a", 2, new char[] { 'H' }, true),
                new OptionSyntax("i", "a", 2, new char[] { 'I' }, true, true),
                new OptionSyntax("j", "a", 0, new char[] { 'J' }, true, true, true)
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    class InvalidOptionSyntaxes : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new OptionSyntax() };
            yield return new object[] { new OptionSyntax("", "") };
            yield return new object[] { new OptionSyntax("a", "", 1, new char[] { 'A' }) };
            yield return new object[] { new OptionSyntax("a", "", 1, new char[] { 'a' }, true) };
            yield return new object[] { new OptionSyntax("a", "", 0, new char[] { 'b' }, false, true, true) };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    class ConflictOptionSyntaxes : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {
                new OptionSyntax("aa", null, 0, new char[] { 'c' }),
                new OptionSyntax("bb", null, 0, new char[] { 'c' })
            };
            yield return new object[] {
                new OptionSyntax("a", null, 0, null, true),
                new OptionSyntax("b", null, 0, new char[] { 'a' }, true)
            };
            yield return new object[] {
                new OptionSyntax("aa", null, 0, null, false, false, true),
                new OptionSyntax("bb", null, 0, null, false, false, true)
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    class ValidOptionSyntaxesAndArgs : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            List<object> testData = new List<object>();

            string[] args = new string[]
            {
                "-j", "value0",
                "-I", "value1", "value2",
                "-h", "value3", "value4",
                "value5",
                "--gg", "value6", "value7",
                "--cc"
            };

            IDictionary<string, IList<string>> expected = new Dictionary<string, IList<string>>();
            expected.Add("j", new List<string> { "value0", "value5" });
            expected.Add("i", new List<string> { "value1", "value2" });
            expected.Add("h", new List<string> { "value3", "value4" });
            expected.Add("gg", new List<string> { "value6", "value7" });
            expected.Add("cc", new List<string> {  });

            testData.Add(args);
            testData.Add(expected);
            foreach (object[] obj in new ValidOptionSyntaxesMulti())
            {
                foreach (OptionSyntax syntax in obj)
                {
                    testData.Add(syntax);
                }
            }

            object[] testDataArr = new object[testData.Count];
            testData.CopyTo(testDataArr);

            return new List<object[]> { testDataArr }.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    class ValidOptionSyntaxesAndInvalidArgs : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            List<object> testData;
            string[] args;
            object[] testDataArr;

            // 必須オプションの欠如
            testData = new List<object>();
            args = new string[]
            {
                "-h", "value3", "value4",
                "value5",
                "--gg", "value6", "value7",
                "--cc"
            };

            testData.Add(args);
            foreach (object[] obj in new ValidOptionSyntaxesMulti())
            {
                foreach (OptionSyntax syntax in obj)
                {
                    testData.Add(syntax);
                }
            }

            testDataArr = new object[testData.Count];
            testData.CopyTo(testDataArr);

            yield return testDataArr;

            // オプションの重複
            testData = new List<object>();
            args = new string[]
            {
                "-j", "value0",
                "-I", "value1", "value2",
                "-h", "value3", "value4",
                "value5",
                "--gg", "value6", "value7",
                "--cc",
                "--cc"
            };

            testData.Add(args);
            foreach (object[] obj in new ValidOptionSyntaxesMulti())
            {
                foreach (OptionSyntax syntax in obj)
                {
                    testData.Add(syntax);
                }
            }

            testDataArr = new object[testData.Count];
            testData.CopyTo(testDataArr);

            yield return testDataArr;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
