using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using mylib.CLI;

namespace CLIUtilities.Tests
{
    class MainClass
    {
        /*
        static void Main(string[] args)
        {
            Console.WriteLine(Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName));
            Console.WriteLine();

            if (args.Length > 1 && args[0] == "-t" && args[1] == "0")
            {
                WriteArgs(args);
            }
            else
            {
                // コマンドライン オプションのシンタックス。
                OptionSyntaxDictionary syntaxes = new OptionSyntaxDictionary {
                    new OptionSyntax("Type",    "コマンドの実行タイプ。\"-t 0\" で args をそのまま表示。"
                            , 1, new char[] { 't' }),
                    new OptionSyntax("Default", "既定のオプション。"
                            , 0, new char[] { }, false, false, true),
                    new OptionSyntax("Test0",   "1 個目のオプション。オプション値 2 個。"
                            , 2, new char[] { '0' }),
                    new OptionSyntax("Test1",   "2 個目のオプション。オプション値 1 個。"
                            , 1, new char[] { '1' }),
                    new OptionSyntax("Test2",   "3 個目のオプション。オプション値なし。"
                            , 0, new char[] { '2' }),
                    new OptionSyntax("Test3",   "4 個目のオプション。オプション名が case sensitive。(エイリアスは常に case sensitive)"
                            , 1, new char[] { '3', 'a' }, true),
                    new OptionSyntax("Test4",   "5 個目のオプション。必須オプション。"
                            , 1, new char[] { '4' }, false, true)
                };

                if (args.Length < 1)
                {
                    Console.Write(syntaxes.GetHelp());
                    Environment.Exit(0);
                }

                // シンタックスに基づいてパースしたコマンドライン オプションを格納するディクショナリ。
                IDictionary<string, IList<string>> options = null;

                try
                {
                    // パース処理。
                    options = new ArgsParser(syntaxes).Parse(args);
                }
                catch (Exception e)
                {
                    // パースに失敗したらヘルプを表示
                    Console.Error.Write(syntaxes.GetHelp());
                    Console.Error.WriteLine("--------");
                    Console.Error.WriteLine(e);
                    Environment.Exit(-1);
                }

                if (options.ContainsKey("type") && options["type"] != null && options["type"].Count > 0 && options["type"][0] == "0")
                {
                    WriteArgs(args);
                }
                else
                {
                    // コマンドライン オプションの表示
                    foreach (KeyValuePair<string, IList<string>> option in options)
                    {
                        Console.Write(option.Key + ": ");
                        foreach (string value in option.Value)
                        {
                            Console.Write(value + ", ");
                        }
                        Console.WriteLine();

                    }
                }
            }
        }
        //*/

        private static void WriteArgs(string[] args)
        {
            foreach (string arg in args)
            {
                Console.WriteLine("--------");
                Console.WriteLine(arg);
            }
        }
    }
}
