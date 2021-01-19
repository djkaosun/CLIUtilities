using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace mylib.CLI
{
    /// <summary>
    /// オプション名とその値リストを格納するディクショナリ。
    /// </summary>
    public class OptionDictionary : IDictionary<string, IReadOnlyList<string>>
    {
        private readonly Dictionary<string, IReadOnlyList<string>> argsDictionary;

        /// <summary>
        /// コンストラクター。シンタックス ディクショナリに格納されたシンタックスに基づいて、コマンド ライン引数をパースします。
        /// </summary>
        /// <param name="args">コマンド ライン引数。</param>
        /// <param name="optionSyntaxDictionary">オプションのシンタックスを示す、シンタックス ディクショナリ。</param>
        public OptionDictionary(string[] args, OptionSyntaxDictionary optionSyntaxDictionary)
        {
            argsDictionary = new Dictionary<string, IReadOnlyList<string>>();
            for (uint i = 0; i < args.Length;)
            {
                if (args[i].StartsWith("--"))
                {
                    // フルネーム
                    string optionName = args[i].Substring(2);
                    i++;
                    List<string> valueCollection = new List<string>();
                    for (uint valueIdx = 0; valueIdx < optionSyntaxDictionary[optionName].ValueCount; valueIdx++)
                    {
                        valueCollection.Add(args[i]);
                        i++;
                    }
                    argsDictionary.Add(optionName, valueCollection.AsReadOnly());
                }
                else if (args[i].StartsWith("-"))
                {
                    // エイリアス
                    char[] aliases = args[i].Substring(1).ToCharArray();
                    i++;
                    for (uint optionIdx = 0; optionIdx < aliases.Length; optionIdx++)
                    {
                        OptionSyntax syntax = optionSyntaxDictionary[aliases[optionIdx].ToString()];
                        List<string> valueCollection = new List<string>();
                        for (uint valueIdx = 0; valueIdx < syntax.ValueCount; valueIdx++)
                        {
                            valueCollection.Add(args[i]);
                            i++;
                        }
                        argsDictionary.Add(syntax.FullName, valueCollection.AsReadOnly());
                    }
                }
                else
                {
                    // デフォルト
                    OptionSyntax defaultSyntax = optionSyntaxDictionary.DefaultOptionSyntax;
                    List<string> valueCollection = new List<string>();
                    for (uint valueIdx = 0; valueIdx < defaultSyntax.ValueCount; valueIdx++)
                    {
                        valueCollection.Add(args[i]);
                        i++;
                    }
                    argsDictionary.Add(defaultSyntax.FullName, valueCollection.AsReadOnly());
                }
            }
        }

        /// <summary>
        /// オプション名を指定して、関連付けられている値リストを取得または設定します。
        /// </summary>
        /// <param name="key">オプション名。</param>
        /// <returns>オプションの値リスト。</returns>
        public IReadOnlyList<string> this[string key] {
            get => ((IDictionary<string, IReadOnlyList<string>>)argsDictionary)[key];
            set => ((IDictionary<string, IReadOnlyList<string>>)argsDictionary)[key] = value;
        }

        /// <summary>
        /// ディクショナリのキー (オプション名) のコレクション。
        /// </summary>
        public IEnumerable<string> Keys
        {
            get
            {
                return argsDictionary.Keys;
            }
        }

        /// <summary>
        /// ディクショナリの値 (オプション値リスト) のコレクション。
        /// </summary>
        public IEnumerable<IReadOnlyList<string>> Values
        {
            get
            {
                return argsDictionary.Values;
            }
        }

        /// <summary>
        /// ディクショナリに格納されているフルネーム/シンタックス ペアの数。
        /// </summary>
        public int Count
        {
            get
            {
                return argsDictionary.Count;
            }
        }

        /// <summary>
        /// ディクショナリが読み取り可能かどうかを示す。
        /// </summary>
        public bool IsReadOnly => ((ICollection<KeyValuePair<string, IReadOnlyList<string>>>)argsDictionary).IsReadOnly;

        ICollection<string> IDictionary<string, IReadOnlyList<string>>.Keys => ((IDictionary<string, IReadOnlyList<string>>)argsDictionary).Keys;

        ICollection<IReadOnlyList<string>> IDictionary<string, IReadOnlyList<string>>.Values => ((IDictionary<string, IReadOnlyList<string>>)argsDictionary).Values;

        /// <summary>
        /// オプション名を指定して、関連付ける値リストを追加します。
        /// </summary>
        /// <param name="key">オプション名</param>
        /// <param name="value">値リスト</param>
        public void Add(string key, IReadOnlyList<string> value)
        {
            ((IDictionary<string, IReadOnlyList<string>>)argsDictionary).Add(key, value);
        }

        /// <summary>
        /// オプション名と値リストが関連づいたキー/値ペアを追加します。
        /// </summary>
        /// <param name="item">オプション名と値リストが関連づいたキー/値ペア</param>
        public void Add(KeyValuePair<string, IReadOnlyList<string>> item)
        {
            ((ICollection<KeyValuePair<string, IReadOnlyList<string>>>)argsDictionary).Add(item);
        }

        /// <summary>
        /// すべてのキーと値を削除します。
        /// </summary>
        public void Clear()
        {
            ((ICollection<KeyValuePair<string, IReadOnlyList<string>>>)argsDictionary).Clear();
        }

        /// <summary>
        /// キー/値ペアがコレクションに格納されているか判断します。
        /// </summary>
        /// <param name="item">オプション名と値リストが関連づいたキー/値ペア</param>
        public bool Contains(KeyValuePair<string, IReadOnlyList<string>> item)
        {
            return ((ICollection<KeyValuePair<string, IReadOnlyList<string>>>)argsDictionary).Contains(item);
        }

        /// <summary>
        /// オプション名がディクショナリに格納されているか判断します。
        /// </summary>
        /// <param name="key">オプション名</param>
        public bool ContainsKey(string key)
        {
            return ((IDictionary<string, IReadOnlyList<string>>)argsDictionary).ContainsKey(key);
        }

        /// <summary>
        /// 要素を Array にコピーします。Array の特定のインデックスからコピーが開始されます。
        /// </summary>
        /// <param name="array">要素が格納される Array。</param>
        /// <param name="arrayIndex">開始インデックス。</param>
        public void CopyTo(KeyValuePair<string, IReadOnlyList<string>>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<string, IReadOnlyList<string>>>)argsDictionary).CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// 指定したオプション名を持つ要素をディクショナリから削除します。
        /// </summary>
        /// <param name="key">オプション名</param>
        /// <returns>要素が正常に削除された場合は true。それ以外の場合は false。</returns>
        public bool Remove(string key)
        {
            return ((IDictionary<string, IReadOnlyList<string>>)argsDictionary).Remove(key);
        }

        /// <summary>
        /// 指定したキー/値ペアをディクショナリから削除します。
        /// </summary>
        /// <param name="item">キー/値ペア</param>
        /// <returns>要素が正常に削除された場合は true。それ以外の場合は false。</returns>
        public bool Remove(KeyValuePair<string, IReadOnlyList<string>> item)
        {
            return ((ICollection<KeyValuePair<string, IReadOnlyList<string>>>)argsDictionary).Remove(item);
        }

        /// <summary>
        /// 指定したオプション名に関連付けられている値リストを取得します。
        /// </summary>
        /// <param name="key">オプション名。</param>
        /// <param name="value">オプション名が見つかった場合は、指定したオプション名に関連付けられている値リスト。</param>
        /// <returns>要素が見つかった場合は true。それ以外の場合は false。</returns>
        public bool TryGetValue(string key, out IReadOnlyList<string> value)
        {
            return ((IDictionary<string, IReadOnlyList<string>>)argsDictionary).TryGetValue(key, out value);
        }

        /// <summary>
        /// コレクションを反復処理する列挙子を返します。
        /// </summary>
        /// <returns>コレクションの反復処理に使用できる列挙子。</returns>
        public IEnumerator<KeyValuePair<string, IReadOnlyList<string>>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, IReadOnlyList<string>>>)argsDictionary).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)argsDictionary).GetEnumerator();
        }
    }
}