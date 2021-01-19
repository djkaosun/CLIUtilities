using System;

namespace mylib.CLI
{
    /// <summary>
    /// フルネームやエイリアスが重複している事を示す例外。
    /// </summary>
    public class ConflictionException : Exception
    {
        /// <summary>
        /// コンストラクター。
        /// </summary>
        public ConflictionException() : base() { }

        /// <summary>
        /// コンストラクター。
        /// </summary>
        /// <param name="message">メッセージ。</param>
        public ConflictionException(string message) : base(message) { }
    }

    /// <summary>
    /// 単一のシンタックス内で整合性が取れていない事を示す例外。
    /// </summary>
    public class ValidationException : Exception
    {
        /// <summary>
        /// コンストラクター。
        /// </summary>
        public ValidationException() : base() { }

        /// <summary>
        /// コンストラクター。
        /// </summary>
        /// <param name="message">メッセージ。</param>
        public ValidationException(string message) : base(message) { }
    }
}
