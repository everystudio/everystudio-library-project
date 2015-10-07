using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;
using UnityEngine;

/// <summary>
/// string 型の拡張メソッドを管理するクラス
/// </summary>
public static class StringExtensions
{
	/// <summary>
	/// スネークケースをアッパーキャメル(パスカル)ケースに変換します
	/// 例) quoted_printable_encode → QuotedPrintableEncode
	/// </summary>
	public static string SnakeToUpperCamel(this string self)
	{
		if (string.IsNullOrEmpty(self))
		{
			return self;
		}

		return self
				.Split(new[] {'_'}, StringSplitOptions.RemoveEmptyEntries)
				.Select(s => char.ToUpperInvariant(s[0]) + s.Substring(1, s.Length - 1))
				.Aggregate(string.Empty, (s1, s2) => s1 + s2);
	}

	/// <summary>
	/// スネークケースをローワーキャメル(キャメル)ケースに変換します
	/// 例) quoted_printable_encode → quotedPrintableEncode
	/// </summary>
	public static string SnakeToLowerCamel(this string self)
	{
		if (string.IsNullOrEmpty(self))
		{
			return self;
		}

		return self.SnakeToUpperCamel().Insert(0, char.ToLowerInvariant(self[0]).ToString()).Remove(1, 1);
	}

	/// <summary>
	/// アッパーキャメル(パスカル)ケースをスネークケースに変換します
	/// 例) QuotedPrintableEncode → quoted_printable_encode
	/// </summary>
	public static string UpperCamelToSnake (string self) 
	{
		if (string.IsNullOrEmpty(self))
		{
			return self;
		}

		return System.Text.RegularExpressions.Regex.Replace(self, "(?<=.)([A-Z])", "_$0").ToLower();
	}
}