using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace mylib.CLI
{
    /// <summary>
    /// コマンド ライン オプションのシンタックスを格納するディクショナリ。
    /// </summary>
    public class OptionSyntaxDictionary : IDictionary<string, OptionSyntax>
    {
        private readonly Dictionary<char, string> aliasDictionary;
        private readonly Dictionary<string, string> caseInsensitiveDictionary;
        private readonly List<string> mandatoryList;
        private readonly Dictionary<string, OptionSyntax> optionSyntaxDictionary;

        /// <summary>
        /// コンストラクター。
        /// </summary>
        public OptionSyntaxDictionary()
        {
            aliasDictionary = new Dictionary<char, string>();
            caseInsensitiveDictionary = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
            mandatoryList = new List<string>();
            optionSyntaxDictionary = new Dictionary<string, OptionSyntax>();
            HasDefault = false;
            DefaultFullName = null;
        }

        /// <summary>
        /// フルネームもしくはエイリアスを指定して、関連付けられているシンタックスを取得または設定します。
        /// </summary>
        /// <param name="key">フルネームもしくはエイリアス。</param>
        /// <returns>シンタックス</returns>
        /// <exception cref="ValidationException">キーと、シンタックスのフルネームが一致しない場合。</exception>
        /// <exception cref="ConflictionException">代入しようとしたシンタックスが、登録済みのシンタックスと重複する場合。</exception>
        public OptionSyntax this[string key] {
            get
            {
                key = _GetFullName(key);
                return optionSyntaxDictionary[key];
            }
            set
            {
                CheckValidity(value);
                key = _GetFullName(key);

                if (key != value.FullName) throw new ValidationException("key is not equal syntax.FullName or duplicate alias");
                if (optionSyntaxDictionary.ContainsKey(key))
                {
                    OptionSyntax backup = optionSyntaxDictionary[key];
                    _Remove(key);
                    if (IsAddable(value))
                    {
                        _Add(value);
                    }
                    else
                    {
                        _Add(backup);
                        throw new ConflictionException(
                                "FullName: " + key
                                + " / OptionSyntaxDictionary.ContainsKey: " + optionSyntaxDictionary.ContainsKey(key)
                                + " / aliasDictionary.Count: " + aliasDictionary.Count
                                + " / mandatoryList.Contains: " + mandatoryList.Contains(key)
                                + " / caseInsensitiveDictionary.ContainsKey: " + caseInsensitiveDictionary.ContainsKey(key)
                        );
                    }
                }
                else
                {
                    if (IsAddable(value))
                    {
                        _Add(value);
                    }
                    else
                    {
                        throw new ConflictionException(
                                "FullName: " + key
                                + " / OptionSyntaxDictionary.ContainsKey: " + optionSyntaxDictionary.ContainsKey(key)
                                + " / aliasDictionary.Count: " + aliasDictionary.Count
                                + " / mandatoryList.Contains: " + mandatoryList.Contains(key)
                                + " / caseInsensitiveDictionary.ContainsKey: " + caseInsensitiveDictionary.ContainsKey(key)
                        );
                    }
                }
            }
        }

        /// <summary>
        /// ディクショナリのキー (フルネーム) のコレクション。
        /// </summary>
        public ICollection<string> Keys {
            get
            {
                return optionSyntaxDictionary.Keys;
            }
        }

        /// <summary>
        /// ディクショナリの値 (シンタックス) のコレクション。
        /// </summary>
        public ICollection<OptionSyntax> Values
        {
            get
            {
                return optionSyntaxDictionary.Values;
            }
        }

        /// <summary>
        /// ディクショナリに格納されているフルネーム/シンタックス ペアの数。
        /// </summary>
        public int Count
        {
            get
            {
                return optionSyntaxDictionary.Count;
            }
        }

        /// <summary>
        /// ディクショナリが読み取り専用かどうかを示す。常に false。
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// キーがエイリアス、値がフルネームのディクショナリ。
        /// </summary>
        public IReadOnlyDictionary<char, string> Aliases
        {
            get
            {
                return new ReadOnlyDictionary<char, string>(aliasDictionary);
            }
        }

        /// <summary>
        /// 必須オプションのフルネームのリスト。
        /// </summary>
        public IReadOnlyList<string> Mandatories
        {
            get
            {
                return mandatoryList.AsReadOnly();
            }
        }

        /// <summary>
        /// 既定のフルネームがある場合 true。それ以外の場合は false。
        /// </summary>
        public bool HasDefault
        {
            get;
            private set;
        }

        /// <summary>
        /// 既定のフルネーム。
        /// </summary>
        public string DefaultFullName
        {
            get;
            private set;
        }

        /// <summary>
        /// 既定のシンタックス。
        /// </summary>
        public OptionSyntax DefaultOptionSyntax
        {
            get { return optionSyntaxDictionary[DefaultFullName]; }
        }



        /// <summary>
        /// 指定したキー (フルネームまたはエイリアス) と値 (シンタックス) をディクショナリに追加します。
        /// </summary>
        /// <param name="key">フルネームまたはエイリアス。</param>
        /// <param name="syntax">シンタックス。</param>
        /// <exception cref="ValidationException">キーと、シンタックスのフルネームが一致しない場合。</exception>
        public void Add(string key, OptionSyntax syntax)
        {
            key = _GetFullName(key);
            if (key != syntax.FullName) throw new ValidationException("key not equal syntax.FullName or duplicate alias.");
            Add(syntax);
        }

        /// <summary>
        /// シンタックスをディクショナリに追加します。
        /// </summary>
        /// <param name="syntax">シンタックス。</param>
        /// <exception cref="ConflictionException">追加しようとしたシンタックスが、登録済みのシンタックスと重複する場合。</exception>
        public void Add(OptionSyntax syntax)
        {
            CheckValidity(syntax);
            if (IsAddable(syntax))
            {
                _Add(syntax);
            }
            else
            {
                
                 throw new ConflictionException(
                         "FullName: " + syntax.FullName
                         + " / OptionSyntaxDictionary.ContainsKey: " + optionSyntaxDictionary.ContainsKey(syntax.FullName)
                         + " / aliasDictionary.Count: " + aliasDictionary.Count
                         + " / mandatoryList.Contains: " + mandatoryList.Contains(syntax.FullName)
                         + " / caseInsensitiveDictionary.ContainsKey: " + caseInsensitiveDictionary.ContainsKey(syntax.FullName)
                 );
            }
        }

        /// <summary>
        /// キー/値ペアをディクショナリに追加します。
        /// </summary>
        /// <param name="item">キー/値ペア。</param>
        public void Add(KeyValuePair<string, OptionSyntax> item)
        {
            Add(item.Key, item.Value);
        }

        private void _Add(OptionSyntax syntax)
        {
            optionSyntaxDictionary.Add(syntax.FullName, syntax);
            if (syntax.Aliases != null)
            {
                foreach (char alias in syntax.Aliases)
                {
                    aliasDictionary.Add(alias, syntax.FullName);
                }
            }

            if (syntax.FullName.Length == 1)
            {
                aliasDictionary.Add(syntax.FullName.ToCharArray()[0], syntax.FullName);
            }

            if (!syntax.IsCaseSensitive)
            {
                caseInsensitiveDictionary.Add(syntax.FullName.ToUpper(), syntax.FullName);
            }
            if (syntax.IsDefault)
            {
                HasDefault = true;
                DefaultFullName = syntax.FullName;
            }
            if (syntax.IsMandatory)
            {
                mandatoryList.Add(syntax.FullName);
            }
        }

        /// <summary>
        /// シンタックスが追加可能かどうかを判断する。
        /// </summary>
        /// <param name="syntax">シンタックス。</param>
        /// <returns>追加可能な場合 true。それ以外の場合は false。</returns>
        protected bool IsAddable(OptionSyntax syntax)
        {
            if (syntax.IsDefault && HasDefault) return false; // 既定のオプションが登録済み
            if (ContainsKey(syntax.FullName)) return false; // 既に登録済み
            if (caseInsensitiveDictionary.ContainsKey(syntax.FullName)) return false; // CaseInsensitive として登録済み

            if (syntax.IsCaseSensitive)
            {
                char fullNameChar = syntax.FullName.ToCharArray()[0];
                foreach (char alias in aliasDictionary.Keys)
                {
                    if (alias == fullNameChar) return false; // 引数フルネームが既存エイリアスと重複している
                }
            }
            else
            {
                foreach (char alias in aliasDictionary.Keys)
                {
                    string aliasString = alias.ToString();
                    if (syntax.FullName.Equals(aliasString, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return false; // 引数フルネームが既存エイリアスと重複している (CaseInsensitive)
                    }

                }
            }

            if (syntax.Aliases != null)
            {
                foreach (char alias in syntax.Aliases)
                {
                    if (aliasDictionary.ContainsKey(alias)) return false; // 引数エイリアスが既存エイリアスと重複している
                }
            }
            return true;
        }

        /// <summary>
        /// シンタックスの整合性をチェックする。
        /// </summary>
        /// <param name="syntax">シンタックス</param>
        /// <exception cref="ValidationException"/>
        protected void CheckValidity(OptionSyntax syntax)
        {
            if (syntax.FullName == null) throw new ValidationException("FullName is null.");
            if (syntax.FullName.Length < 1) throw new ValidationException("FullName length is 0.");
            if (syntax.FullName.Length == 1)
            {
                // 一文字フルネームかつ Case-Insensitive の場合、不整合。
                if (!syntax.IsCaseSensitive)
                {
                    throw new ValidationException("FullName is Single-Char and Syntax is Case-Insensitive.");
                }

                // 一文字フルネームとエイリアスの競合チェック
                if (syntax.Aliases != null && syntax.Aliases.Length > 0)
                {
                    if (syntax.IsCaseSensitive)
                    {
                        char fullNameChar = syntax.FullName.ToCharArray()[0];
                        foreach (char alias in syntax.Aliases)
                        {
                            if (alias == fullNameChar) throw new ValidationException("FullName and Alias is conflict");
                        }
                    }
                    else
                    {
                        foreach (char alias in syntax.Aliases)
                        {
                            string aliasString = alias.ToString();
                            if (syntax.FullName.Equals(aliasString, StringComparison.CurrentCultureIgnoreCase))
                            {
                                throw new ValidationException("FullName and Alias is conflict");
                            }
                        }
                    }
                }
            }
            
            if (syntax.IsMandatory && !syntax.IsDefault && syntax.ValueCount < 1) throw new ValidationException("ValueCount of IsMandatory is 0.");
        }

        /// <summary>
        /// すべてのキーと値を削除します。
        /// </summary>
        public void Clear()
        {
            optionSyntaxDictionary.Clear();
            aliasDictionary.Clear();
            mandatoryList.Clear();
            caseInsensitiveDictionary.Clear();
            HasDefault = false;
            DefaultFullName = null;
        }

        /// <summary>
        /// 指定したキー/値ペアがディクショナリに格納されているかを判断します。
        /// </summary>
        /// <param name="item">キー/値ペア</param>
        /// <returns>格納されている場合は true。それ以外の場合は false。</returns>
        public bool Contains(KeyValuePair<string, OptionSyntax> item)
        {
            string fullName = _GetFullName(item.Key);
            return optionSyntaxDictionary.ContainsKey(fullName)
                    && optionSyntaxDictionary[fullName].Equals(item.Value);
        }

        /// <summary>
        /// 指定したキー (フルネームまたはエイリアス) がディクショナリに格納されているかを判断します。
        /// </summary>
        /// <param name="key">フルネームまたはエイリアス</param>
        /// <returns>格納されている場合は true。それ以外の場合は false。</returns>
        public bool ContainsKey(string key)
        {
            // エイリアスに含まれていたら true
            if (key.Length == 1 && aliasDictionary.ContainsKey(key.ToCharArray()[0]))
            {
                return true;
            }

            // CaseInsensitive に含まれていたら true
            if (caseInsensitiveDictionary.ContainsKey(key))
            {
                return true;
            }

            return optionSyntaxDictionary.ContainsKey(key);
        }

        /// <summary>
        /// 要素を Array にコピーします。Array の特定のインデックスからコピーが開始されます。
        /// </summary>
        /// <param name="array">要素が格納される Array。</param>
        /// <param name="arrayIndex">開始インデックス。</param>
        public void CopyTo(KeyValuePair<string, OptionSyntax>[] array, int arrayIndex)
        {
            int index = arrayIndex;
            foreach(KeyValuePair<string, OptionSyntax> item in optionSyntaxDictionary)
            {
                array[index] = item;
                index++;
            }
        }

        /// <summary>
        /// 指定したキー (フルネームまたはエイリアス) を持つ要素をディクショナリから削除します。
        /// </summary>
        /// <param name="key">フルネームまたはエイリアス</param>
        /// <returns>要素が正常に削除された場合は true。それ以外の場合は false。</returns>
        public bool Remove(string key)
        {
            bool isRemoved = false;

            key = _GetFullName(key);

            if (optionSyntaxDictionary.ContainsKey(key))
            {
                _Remove(key);
                isRemoved = true;
            }
            return isRemoved;
        }

        /// <summary>
        /// 指定したキー/値ペアをディクショナリから削除します。
        /// </summary>
        /// <param name="item">キー/値ペア</param>
        /// <returns>要素が正常に削除された場合は true。それ以外の場合は false。</returns>
        public bool Remove(KeyValuePair<string, OptionSyntax> item)
        {
            if (Contains(item))
            {
                return Remove(item.Key);
            }
            else
            {
                return false;
            }
        }

        private void _Remove(string key)
        {
            OptionSyntax syntax = optionSyntaxDictionary[key];
            if (syntax.FullName.Length == 1)
            {
                aliasDictionary.Remove(syntax.FullName.ToCharArray()[0]);
            }

            if (syntax.Aliases != null)
            {
                foreach (char alias in optionSyntaxDictionary[key].Aliases)
                {
                    aliasDictionary.Remove(alias);
                }
            }

            if (!syntax.IsCaseSensitive)
            {
                caseInsensitiveDictionary.Remove(key);
            }

            if (syntax.IsDefault)
            {
                HasDefault = false;
                DefaultFullName = null;
            }

            if (syntax.IsMandatory)
            {
                mandatoryList.Remove(key);
            }

            optionSyntaxDictionary.Remove(key);
        }

        /// <summary>
        /// 指定したキー (フルネームまたはエイリアス) に関連付けられている値を取得します。
        /// </summary>
        /// <param name="key">フルネームまたはエイリアス。</param>
        /// <param name="value">キーが見つかった場合は、指定したキーに関連付けられているシンタックス。それ以外の場合は既定値のシンタックス。</param>
        /// <returns>要素が見つかった場合は true。それ以外の場合は false。</returns>
        public bool TryGetValue(string key, out OptionSyntax value)
        {
            if (ContainsKey(key))
            {
                value = this[key];
                return true;
            }
            else
            {
                value = new OptionSyntax();
                return false;
            }
        }

        /// <summary>
        /// 指定したエイリアスに対応するフルネームを取得します。引数としてフルネームを渡された場合は、そのフルネームを返します。
        /// </summary>
        /// <param name="name">フルネームまたはエイリアス。</param>
        /// <returns>フルネーム。</returns>
        /// <exception cref="KeyNotFoundException" />
        public string GetFullName(string name)
        {
            name = _GetFullName(name);
            return optionSyntaxDictionary[name].FullName;
        }

        /// <summary>
        /// 引数の名前がエイリアスや CaseInsensitive に含まれている場合、対応するフルネーム、含まれていない場合は引数がそのまま返ります。
        /// </summary>
        /// <param name="name">名前。</param>
        /// <returns>引数の名前がエイリアスや CaseInsensitive に含まれている場合、対応するフルネーム、含まれていない場合は引数そのまま。</returns>
        private string _GetFullName(string name)
        {
            if (name == null) throw new ValidationException("FullName is null.");

            string result = name;
            // エイリアスをフルネームに読み替え
            if (name.Length == 1 && aliasDictionary.ContainsKey(name.ToCharArray()[0]))
            {
                result = aliasDictionary[name.ToCharArray()[0]];
            }

            // CaseInsensitive からフルネームに読み替え
            if (caseInsensitiveDictionary.ContainsKey(name))
            {
                result = caseInsensitiveDictionary[name];
            }
            return result;
        }

        /// <summary>
        /// ヘルプを返します。
        /// </summary>
        /// <returns>ヘルプ。</returns>
        public string GetHelp()
        {
            string result = String.Empty;
            foreach (OptionSyntax syntax in optionSyntaxDictionary.Values)
            {
                result += syntax.GetHelp();
            }
            return result;
        }

        /// <summary>
        /// コレクションを反復処理する列挙子を返します。
        /// </summary>
        /// <returns>コレクションの反復処理に使用できる列挙子。</returns>
        public IEnumerator<KeyValuePair<string, OptionSyntax>> GetEnumerator()
        {
            return (new ReadOnlyDictionary<string,OptionSyntax>(optionSyntaxDictionary)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
