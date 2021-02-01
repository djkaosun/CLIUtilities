using System;
using System.Collections.Generic;
using mylib.CLI;
using Xunit;
using CLIUtilities.Tests.TestDatas;

namespace CLIUtilities.Tests
{
    public class OptionSyntaxDictionaryTests
    {
        #region tests for indexer
        //******************************************************************************
        // 
        // tests for indexer
        // 
        //******************************************************************************
        [Theory]
        [ClassData(typeof(ValidOptionSyntaxesMulti))]
        public void IndexerTest_ValidData(params OptionSyntax[] syntaxes)
        {
            OptionSyntaxDictionary syntaxDictionary = new OptionSyntaxDictionary();
            foreach (OptionSyntax syntax in syntaxes)
            {
                syntaxDictionary[syntax.FullName] = syntax;
                Assert.Equal(syntax, syntaxDictionary[syntax.FullName]);
                if (!syntax.IsCaseSensitive)
                {
                    Assert.Equal(syntax, syntaxDictionary[syntax.FullName.ToUpper()]);
                    Assert.Equal(syntax, syntaxDictionary[syntax.FullName.ToLower()]);
                }
                if (syntax.Aliases != null)
                {
                    foreach (char alias in syntax.Aliases)
                    {
                        Assert.Equal(syntax, syntaxDictionary[alias.ToString()]);
                    }
                }
            }

            foreach (OptionSyntax syntax in syntaxes)
            {
                syntaxDictionary[syntax.FullName] = syntax;
                Assert.Equal(syntax, syntaxDictionary[syntax.FullName]);
                if (!syntax.IsCaseSensitive)
                {
                    Assert.Equal(syntax, syntaxDictionary[syntax.FullName.ToUpper()]);
                    Assert.Equal(syntax, syntaxDictionary[syntax.FullName.ToLower()]);
                }
                if (syntax.Aliases != null)
                {
                    foreach (char alias in syntax.Aliases)
                    {
                        Assert.Equal(syntax, syntaxDictionary[alias.ToString()]);
                    }
                }
            }
        }

        [Theory]
        [ClassData(typeof(ValidOptionSyntaxes))]
        public void IndexerTest_InvalidIndex(OptionSyntax syntax)
        {
            OptionSyntaxDictionary syntaxDictionary = new OptionSyntaxDictionary();
            syntaxDictionary[syntax.FullName] = syntax;
            Assert.Throws<ValidationException>(() => { syntaxDictionary[syntax.FullName + "a"] = syntax; });
        }

        [Theory]
        [ClassData(typeof(InvalidOptionSyntaxes))]
        public void IndexerTest_InvalidData(OptionSyntax syntax)
        {
            OptionSyntaxDictionary syntaxDictionary = new OptionSyntaxDictionary();
            Assert.Throws<ValidationException>(() => { syntaxDictionary[syntax.FullName] = syntax; });
        }

        [Theory]
        [ClassData(typeof(ConflictOptionSyntaxes))]
        public void IndexerTest_ConflictData(OptionSyntax syntax1, OptionSyntax syntax2)
        {
            OptionSyntaxDictionary syntaxDictionary = new OptionSyntaxDictionary
            {
                [syntax1.FullName] = syntax1
            };
            Assert.Throws<ConflictionException>(() => { syntaxDictionary[syntax2.FullName] = syntax2; });
        }
        #endregion

        #region tests for Add methods
        //******************************************************************************
        // 
        // tests for Add methods
        // 
        //******************************************************************************
        [Theory]
        [ClassData(typeof(ValidOptionSyntaxesMulti))]
        public void AddTest_ValidData(params OptionSyntax[] syntaxes)
        {
            OptionSyntaxDictionary syntaxDictionary = new OptionSyntaxDictionary();
            foreach (OptionSyntax syntax in syntaxes)
            {
                syntaxDictionary.Add(syntax);
                Assert.Equal(syntax, syntaxDictionary[syntax.FullName]);
                if (!syntax.IsCaseSensitive)
                {
                    Assert.Equal(syntax, syntaxDictionary[syntax.FullName.ToUpper()]);
                    Assert.Equal(syntax, syntaxDictionary[syntax.FullName.ToLower()]);
                }
                if (syntax.Aliases != null)
                {
                    foreach (char alias in syntax.Aliases)
                    {
                        Assert.Equal(syntax, syntaxDictionary[alias.ToString()]);
                    }
                }
            }

            foreach (OptionSyntax syntax in syntaxes)
            {
                Assert.Throws<ConflictionException>(() => { syntaxDictionary.Add(syntax.FullName, syntax); });
                if (!syntax.IsCaseSensitive)
                {
                    Assert.Throws<ConflictionException>(() => { syntaxDictionary.Add(syntax.FullName.ToUpper(), syntax); });
                    Assert.Throws<ConflictionException>(() => { syntaxDictionary.Add(syntax.FullName.ToLower(), syntax); });
                }
                if (syntax.Aliases != null)
                {
                    foreach (char alias in syntax.Aliases)
                    {
                        Assert.Throws<ConflictionException>(() => { syntaxDictionary.Add(alias.ToString(), syntax); });
                    }
                }

            }
        }

        [Theory]
        [ClassData(typeof(ValidOptionSyntaxes))]
        public void AddTest_InvalidIndex(OptionSyntax syntax)
        {
            OptionSyntaxDictionary syntaxDictionary = new OptionSyntaxDictionary();

            Assert.Throws<ValidationException>(() => { syntaxDictionary.Add(syntax.FullName + "a", syntax); });
        }

        [Theory]
        [ClassData(typeof(InvalidOptionSyntaxes))]
        public void AddTest_InvalidData(OptionSyntax syntax)
        {
            OptionSyntaxDictionary syntaxDictionary = new OptionSyntaxDictionary();
            Assert.Throws<ValidationException>(() => { syntaxDictionary.Add(syntax.FullName, syntax); });
        }

        [Theory]
        [ClassData(typeof(ConflictOptionSyntaxes))]
        public void AddTest_ConflictData(OptionSyntax syntax1, OptionSyntax syntax2)
        {
            OptionSyntaxDictionary syntaxDictionary = new OptionSyntaxDictionary
            {
                [syntax1.FullName] = syntax1
            };
            Assert.Throws<ConflictionException>(() => { syntaxDictionary.Add(syntax2.FullName, syntax2); });
        }
        #endregion

        #region test for Clear method
        //******************************************************************************
        // 
        // test for Clear method
        // 
        //******************************************************************************
        [Theory]
        [ClassData(typeof(ValidOptionSyntaxesMulti))]
        public void ClearTest(params OptionSyntax[] syntaxes)
        {
            OptionSyntaxDictionary syntaxDictionary = new OptionSyntaxDictionary();
            foreach (OptionSyntax syntax in syntaxes)
            {
                syntaxDictionary.Add(syntax);
                Assert.Equal(syntax, syntaxDictionary[syntax.FullName]);
            }

            syntaxDictionary.Clear();
            Assert.Empty(syntaxDictionary);

            foreach (OptionSyntax syntax in syntaxes)
            {
                syntaxDictionary.Add(syntax);
                Assert.Equal(syntax, syntaxDictionary[syntax.FullName]);
                if (!syntax.IsCaseSensitive)
                {
                    Assert.Equal(syntax, syntaxDictionary[syntax.FullName.ToUpper()]);
                    Assert.Equal(syntax, syntaxDictionary[syntax.FullName.ToLower()]);
                }
                if (syntax.Aliases != null)
                {
                    foreach (char alias in syntax.Aliases)
                    {
                        Assert.Equal(syntax, syntaxDictionary[alias.ToString()]);
                    }
                }
            }

            foreach (OptionSyntax syntax in syntaxes)
            {
                Assert.Throws<ConflictionException>(() => { syntaxDictionary.Add(syntax.FullName, syntax); });
                if (!syntax.IsCaseSensitive)
                {
                    Assert.Throws<ConflictionException>(() => { syntaxDictionary.Add(syntax.FullName.ToUpper(), syntax); });
                    Assert.Throws<ConflictionException>(() => { syntaxDictionary.Add(syntax.FullName.ToLower(), syntax); });
                }
                if (syntax.Aliases != null)
                {
                    foreach (char alias in syntax.Aliases)
                    {
                        Assert.Throws<ConflictionException>(() => { syntaxDictionary.Add(alias.ToString(), syntax); });
                    }
                }

            }

        }
        #endregion

        #region test for Contains methods
        //******************************************************************************
        // 
        // test for Contains methods
        // 
        //******************************************************************************
        [Theory]
        [ClassData(typeof(ValidOptionSyntaxes))]
        public void ContainsTest(OptionSyntax syntax)
        {
            OptionSyntaxDictionary syntaxDictionary = new OptionSyntaxDictionary();
            syntaxDictionary.Add(syntax);
            Assert.Contains(new KeyValuePair<string, OptionSyntax>(syntax.FullName, syntax), syntaxDictionary);
            Assert.True(syntaxDictionary.ContainsKey(syntax.FullName));
            Assert.DoesNotContain(new KeyValuePair<string, OptionSyntax>(syntax.FullName + "a", syntax), syntaxDictionary);
            Assert.False(syntaxDictionary.ContainsKey(syntax.FullName + "a"));

            if (!syntax.IsCaseSensitive)
            {
                Assert.Contains(new KeyValuePair<string, OptionSyntax>(syntax.FullName.ToUpper(), syntax), syntaxDictionary);
                Assert.True(syntaxDictionary.ContainsKey(syntax.FullName.ToUpper()));
                Assert.Contains(new KeyValuePair<string, OptionSyntax>(syntax.FullName.ToLower(), syntax), syntaxDictionary);
                Assert.True(syntaxDictionary.ContainsKey(syntax.FullName.ToLower()));
            }

            if (syntax.Aliases != null)
            {
                foreach (char alias in syntax.Aliases)
                {
                    Assert.Contains(new KeyValuePair<string, OptionSyntax>(alias.ToString(), syntax), syntaxDictionary);
                    Assert.True(syntaxDictionary.ContainsKey(alias.ToString()));
                }
            }
        }
        #endregion

        #region test for Remove methods
        //******************************************************************************
        // 
        // test for Remove methods
        // 
        //******************************************************************************
        [Theory]
        [ClassData(typeof(ValidOptionSyntaxes))]
        public void RemoveTest(OptionSyntax syntax)
        {
            OptionSyntaxDictionary syntaxDictionary = new OptionSyntaxDictionary();
            syntaxDictionary.Add(syntax);
            Assert.True(syntaxDictionary.Remove(new KeyValuePair<string, OptionSyntax>(syntax.FullName, syntax)));
            Assert.False(syntaxDictionary.Remove(syntax.FullName));
            syntaxDictionary.Add(syntax);
            Assert.True(syntaxDictionary.Remove(syntax.FullName));

            if (!syntax.IsCaseSensitive)
            {
                syntaxDictionary.Add(syntax);
                Assert.True(syntaxDictionary.Remove(syntax.FullName.ToUpper()));
                syntaxDictionary.Add(syntax);
                Assert.True(syntaxDictionary.Remove(syntax.FullName.ToLower()));
            }

            if (syntax.Aliases != null)
            {
                foreach (char alias in syntax.Aliases)
                {
                    syntaxDictionary.Add(syntax);
                    Assert.True(syntaxDictionary.Remove(alias.ToString()));
                }
            }
        }
        #endregion

        #region test for TryGetValue method
        //******************************************************************************
        // 
        // test for TryGetValue method
        // 
        //******************************************************************************
        [Theory]
        [ClassData(typeof(ValidOptionSyntaxes))]
        public void TryGetValueTest(OptionSyntax syntax)
        {
            OptionSyntaxDictionary syntaxDictionary = new OptionSyntaxDictionary();
            syntaxDictionary.Add(syntax);
            OptionSyntax result;

            Assert.True(syntaxDictionary.TryGetValue(syntax.FullName, out result));
            Assert.Equal(syntax, result);

            Assert.False(syntaxDictionary.TryGetValue(syntax.FullName + "a", out result));
            Assert.NotEqual(syntax, result);

            if (!syntax.IsCaseSensitive)
            {
                Assert.True(syntaxDictionary.TryGetValue(syntax.FullName.ToUpper(), out result));
                Assert.Equal(syntax, result);
                Assert.True(syntaxDictionary.TryGetValue(syntax.FullName.ToLower(), out result));
                Assert.Equal(syntax, result);
            }

            if (syntax.Aliases != null)
            {
                foreach (char alias in syntax.Aliases)
                {
                    Assert.True(syntaxDictionary.TryGetValue(alias.ToString(), out result));
                    Assert.Equal(syntax, result);
                }
            }
        }

        #endregion

        #region test for GetFullName method
        //******************************************************************************
        // 
        // test for GetFullName method
        // 
        //******************************************************************************
        [Theory]
        [ClassData(typeof(ValidOptionSyntaxes))]
        public void GetFullNameTest(OptionSyntax syntax)
        {
            OptionSyntaxDictionary syntaxDictionary = new OptionSyntaxDictionary();
            syntaxDictionary.Add(syntax);
            Assert.Equal(syntax.FullName, syntaxDictionary.GetFullName(syntax.FullName));
            Assert.Throws<KeyNotFoundException>(() => { syntaxDictionary.GetFullName(syntax.FullName + "a"); });

            if (!syntax.IsCaseSensitive)
            {
                Assert.Equal(syntax.FullName, syntaxDictionary.GetFullName(syntax.FullName.ToUpper()));
                Assert.Equal(syntax.FullName, syntaxDictionary.GetFullName(syntax.FullName.ToLower()));
            }

            if (syntax.Aliases != null)
            {
                foreach (char alias in syntax.Aliases)
                {
                    Assert.Equal(syntax.FullName, syntaxDictionary.GetFullName(alias.ToString()));
                }
            }
        }
        #endregion
    }
}
