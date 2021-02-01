using System;
using System.Collections.Generic;

namespace mylib.CLI
{
    /// <summary>
    /// コマンドライン引数をシンタックスに基づいてパースします。
    /// </summary>
    public class ArgsParser
    {
        /// <summary>
        /// シンタックス ディクショナリ。
        /// </summary>
        public OptionSyntaxDictionary OptionSyntaxDictionary { get; set; }

        /// <summary>
        /// コンフィグから読み取ったオプションのディクショナリ。
        /// </summary>
        public IDictionary<string, string[]> ConfigOptionDictionary { get; set; }

        /// <summary>
        /// コンストラクター。
        /// </summary>
        /// <param name="optionSyntaxDictionary">シンタックス ディクショナリ。</param>
        public ArgsParser(OptionSyntaxDictionary optionSyntaxDictionary = null, IDictionary<string, string[]> configOption = null)
        {
            OptionSyntaxDictionary = optionSyntaxDictionary;
            ConfigOptionDictionary = configOption;
        }

        /// <summary>
        /// コマンドライン引数をシンタックスに基づいてパースします。
        /// </summary>
        /// <param name="args">コマンドライン引数。</param>
        /// <returns>パース結果を格納した、キーがオプション名、値がオプション値リストのディクショナリ。</returns>
        /// <exception cref="ArgumentException">引数に起因してパースに失敗する場合。</exception>
        public IDictionary<string, IList<string>> Parse(string[] args)
        {
            if (OptionSyntaxDictionary == null) throw new InvalidOperationException("OptionSyntaxDictionary is not set.");
            IDictionary<string, IList<string>> argsDictionary = new Dictionary<string, IList<string>>();
            for (uint i = 0; i < args.Length;)
            {
                if (args[i].StartsWith("--"))
                {
                    // フルネーム
                    string optionName = args[i].Substring(2);
                    i++;
                    List<string> valueCollection = new List<string>();
                    for (uint valueIdx = 0; valueIdx < OptionSyntaxDictionary[optionName].ValueCount; valueIdx++)
                    {
                        valueCollection.Add(args[i]);
                        i++;
                    }
                    argsDictionary.Add(OptionSyntaxDictionary.GetFullName(optionName), valueCollection);
                }
                else if (args[i].StartsWith("-"))
                {
                    // エイリアス
                    char[] aliases = args[i].Substring(1).ToCharArray();
                    i++;
                    for (uint optionIdx = 0; optionIdx < aliases.Length; optionIdx++)
                    {
                        OptionSyntax syntax = OptionSyntaxDictionary[aliases[optionIdx].ToString()];
                        List<string> valueCollection = new List<string>();
                        for (uint valueIdx = 0; valueIdx < syntax.ValueCount; valueIdx++)
                        {
                            valueCollection.Add(args[i]);
                            i++;
                        }
                        argsDictionary.Add(syntax.FullName, valueCollection);
                    }
                }
                else
                {
                    // デフォルト
                    OptionSyntax defaultSyntax = OptionSyntaxDictionary.DefaultOptionSyntax;

                    List<string> valueCollection;
                    if (argsDictionary.ContainsKey(defaultSyntax.FullName))
                    {
                        if (defaultSyntax.ValueCount == 0)
                        {
                            valueCollection = new List<string>(argsDictionary[defaultSyntax.FullName]);
                            argsDictionary.Remove(defaultSyntax.FullName);
                        }
                        else
                        {
                            throw new ArgumentException("An item with the same key has already been added. Key: " + defaultSyntax.FullName + " Dictionary: argsDictionary");
                        }
                    }
                    else
                    {
                        valueCollection = new List<string>();
                    }

                    if (defaultSyntax.ValueCount == 0)
                    {
                        while(i < args.Length && !args[i].StartsWith("-"))
                        {
                            valueCollection.Add(args[i]);
                            i++;
                        }
                    }
                    else
                    {
                        for (uint valueIdx = 0; valueIdx < defaultSyntax.ValueCount; valueIdx++)
                        {
                            valueCollection.Add(args[i]);
                            i++;
                        }
                    }

                    argsDictionary.Add(defaultSyntax.FullName, valueCollection);
                }
            }

            if (ConfigOptionDictionary != null)
            {
                foreach (var item in ConfigOptionDictionary)
                {
                    if (!OptionSyntaxDictionary.ContainsKey(item.Key)) throw new InvalidOperationException("A option in ConfigDictionary is not contains in OptionSintaxDictionary");

                    if (argsDictionary.ContainsKey(item.Key)) continue; // コンフィグよりコマンドライン引数優先

                    var syntax = OptionSyntaxDictionary[item.Key];

                    if (syntax.IsDefault && syntax.ValueCount == 0 || syntax.ValueCount == item.Value.Length)
                    {
                        argsDictionary.Add(syntax.FullName, item.Value);
                    }
                    else
                    {
                        throw new InvalidOperationException("The number of option's values in ConfigDictionary is too mach or less.");
                    }
                }
            }

            if (OptionSyntaxDictionary.Mandatories.Count > 0)
            {
                foreach (string optionname in OptionSyntaxDictionary.Mandatories)
                {
                    if (!argsDictionary.ContainsKey(optionname)) throw new ArgumentException("A mandatory option is not contains in args.");
                }
            }

            return argsDictionary;
        }
    }
}
