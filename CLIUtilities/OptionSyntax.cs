using System;
using System.Collections.Generic;

namespace mylib.CLI
{
    /// <summary>
    /// オプション シンタックス構造体。
    /// </summary>
    public readonly struct OptionSyntax : IEquatable<OptionSyntax>
    {
        /// <summary>
        /// オプション名。オプションのフルネーム。
        /// </summary>
        public readonly string FullName;

        /// <summary>
        /// オプションの説明。
        /// </summary>
        public readonly string Description;

        /// <summary>
        /// そのオプションに関連づく値の数。
        /// </summary>
        public readonly uint ValueCount;

        /// <summary>
        /// オプション名のエイリアス。
        /// </summary>
        public char[] Aliases
        {
            get
            {
                return aliaseseString?.ToCharArray();
            }
        }
        private readonly string aliaseseString;

        /// <summary>
        /// このオプション名のフルネームの大文字小文字を区別する場合 true。それ以外の場合は false。
        /// </summary>
        public readonly bool IsCaseSensitive;

        /// <summary>
        /// このオプションが必須の場合 true。それ以外の場合は false。
        /// </summary>
        public readonly bool IsMandatory;

        /// <summary>
        /// このシンタックスが既定として扱われるかを示します。IsDefault が true、かつ、ValueCount が 0 の場合、オプションの値の数は可変になります。
        /// </summary>
        public readonly bool IsDefault;

        /// <summary>
        /// コンストラクター。
        /// </summary>
        /// <param name="fullName">フルネーム。</param>
        /// <param name="description">このオプションの説明。</param>
        /// <param name="valueCount">フルネームに対応する値の数。</param>
        /// <param name="aliases">フルネームに対応するエイリアス。配列は内部的にコピーされます。元の配列は変更されません。</param>
        /// <param name="isCaseSensitive">このオプション名のフルネームの大文字小文字を区別しない場合 true。それ以外の場合は false。</param>
        /// <param name="isMandatory">このオプションが必須の場合 true。それ以外の場合は false。</param>
        /// <param name="isDefault">このオプションが既定の場合 true。それ以外の場合は false。</param>
        public OptionSyntax(string fullName, string description, uint valueCount = 0, char[] aliases = null, bool isCaseSensitive = false, bool isMandatory = false, bool isDefault = false)
        {
            if (description?.Length == 0) description = null;
            FullName = fullName;
            Description = description;
            ValueCount = valueCount;
            IsCaseSensitive = isCaseSensitive;
            IsMandatory = isMandatory;
            IsDefault = isDefault;

            if (aliases?.Length > 0)
            {
                char[] copiedAliases = new char[aliases.Length];
                aliases.CopyTo(copiedAliases, 0);
                Array.Sort<char>(copiedAliases);
                aliaseseString = new string(copiedAliases);
            }
            else
            {
                aliaseseString = null;
            }
        }

        /// <summary>
        /// ヘルプを返します。
        /// </summary>
        /// <returns>ヘルプ。</returns>
        public string GetHelp()
        {
            string help;

            if (FullName.Length == 1)
            {
                help = "-" + FullName;
            }
            else
            {
                help = "--" + FullName;
            }

            if (aliaseseString != null)
            {
                foreach (char alias in Aliases)
                {
                    help += " | -" + alias;
                }
            }

            if (ValueCount > 0)
            {
                help += "  ";

                for (int i = 0; i < ValueCount; i++)
                {
                    help += " value" + i;
                }
            }

            help += Environment.NewLine;
            if (IsMandatory) help += "[Mandatory] ";
            help += Description ?? "(Description is null.)";

            help += Environment.NewLine + Environment.NewLine;
            return help;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="syntax1"></param>
        /// <param name="syntax2"></param>
        /// <returns></returns>
        public static bool operator ==(OptionSyntax syntax1, OptionSyntax syntax2)
        {
            // 構造体は null 非許容であるため、null チェックは不要。
            return syntax1.Equals(syntax2);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="syntax1"></param>
        /// <param name="syntax2"></param>
        /// <returns></returns>
        public static bool operator !=(OptionSyntax syntax1, OptionSyntax syntax2)
        {
            // 構造体は null 非許容であるため、null チェックは不要。
            return !syntax1.Equals(syntax2);
        }
        
        /// <summary>
        /// このオブジェクトと他のオブジェクトが等しいか比較する。
        /// </summary>
        /// <param name="obj">他のオブジェクト。</param>
        /// <returns>等しい場合 true。それ以外の場合は false。</returns>
        public override bool Equals(object obj)
        {
            return obj is OptionSyntax syntax && Equals(syntax);
            /*
             * 以下とほぼ同じ意味
             * 
             * if (obj is OptionSyntax)
             * {
             *     OptionSyntax syntax = (OptionSyntax)obj;
             *     return Equals(syntax);
             * }
             * else
             * {
             *     // obj が OptionSyntax にキャスト不可能な場合 (obj が null な場合を含む)
             *     return false;
             * }
             */
        }

        /// <summary>
        /// この構造体と他の構造体が等しいか比較する。
        /// </summary>
        /// <param name="other">他の構造体。</param>
        /// <returns>等しい場合 true。それ以外の場合は false。</returns>
        public bool Equals(OptionSyntax other)
        {
            // 同一のインスタンスを参照している場合は true
            if (System.Object.ReferenceEquals(this, other)) return true;

            // 構造体は null 非許容であるため、null チェックは不要。
            return _Equals(other);
        }

        private bool _Equals(OptionSyntax other)
        {
            if (this.ValueCount != other.ValueCount) return false;
            if (this.IsCaseSensitive != other.IsCaseSensitive) return false;
            if (this.IsMandatory != other.IsMandatory) return false;
            if (this.IsDefault != other.IsDefault) return false;
            if (this.FullName != other.FullName) return false;
            if (this.Description != other.Description) return false;

            return this.aliaseseString == other.aliaseseString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="syntax1"></param>
        /// <param name="syntax2"></param>
        /// <returns></returns>
        public static bool Equals(OptionSyntax syntax1, OptionSyntax syntax2)
        {
            // 同一のインスタンスを参照している場合は true
            if (System.Object.ReferenceEquals(syntax1, syntax2)) return true;

            return syntax1.Equals(syntax2);
        }

        /// <summary>
        /// この構造体のハッシュ値を取得する。
        /// </summary>
        /// <returns>ハッシュ値。</returns>
        public override int GetHashCode()
        {
            int hashCode = 615012876;
            unchecked
            {
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FullName);
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Description);
                hashCode = hashCode * -1521134295 + ValueCount.GetHashCode();
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(aliaseseString);
                hashCode = hashCode * -1521134295 + IsCaseSensitive.GetHashCode();
                hashCode = hashCode * -1521134295 + IsMandatory.GetHashCode();
                hashCode = hashCode * -1521134295 + IsDefault.GetHashCode();
            }
            return hashCode;
        }
    }
}