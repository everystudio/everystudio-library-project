using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEditor;


// 参考(一部修正) http://workaholist.hatenablog.com/entry/2013/02/18/104345
namespace Workaholism.IO
{
	/// <summary>
	/// CSVファイル出力クラス。
	/// </summary>
	public class CsvWriter : IDisposable
	{
		#region "  フィールド  "

		/// <summary>
		/// ライタ。
		/// </summary>
		private StreamWriter _writer = null;

		#endregion

		#region "  コンストラクタ / デストラクタ  "

		/// <summary>
		/// コンストラクタ。
		/// </summary>
		/// <param name="filePath">ファイルパス。</param>
		/// <remarks>エンコードはUTF-8。</remarks>
		public CsvWriter (string filePath)
		{
			// ライタオープン
			Open (filePath, Encoding.UTF8);
		}

		/// <summary>
		/// コンストラクタ。
		/// </summary>
		/// <param name="filePath">ファイルパス。</param>
		/// <param name="encoding">文字エンコーディング。</param>
		public CsvWriter (string filePath, Encoding encoding)
		{
			// ライタオープン
			Open (filePath, encoding);
		}

		/// <summary>
		/// デストラクタ。
		/// </summary>
		~CsvWriter ()
		{
			// リソースの解放
			Dispose (false);
		}

		#endregion

		#region "  メソッド  "

		/// <summary>
		/// 行を出力する。
		/// </summary>
		/// <param name="values">出力項目コレクション。</param>
		public void WriteLine (IEnumerable<string> values)
		{
			StringBuilder line = new StringBuilder ();

			foreach (string item in values) {
				if (0 < line.Length)
					//line.Append (",");
					line.Append (",");

				line.Append (item);
				//line.Append (ToCsvValue (item));
			}

			_writer.WriteLine (line.ToString ());
		}

		/// <summary>
		/// CSV 出力用の値に変換する。
		/// </summary>
		/// <param name="value">変換前の値。</param>
		/// <returns>変換後の値。</returns>
		private static string ToCsvValue (string value)
		{
			if (string.IsNullOrEmpty (value))
				value = "";

			return String.Format ("\"{0}\"", value.Replace ("\"", "\"\""));
		}

		/// <summary>
		/// ライタを開く。
		/// </summary>
		/// <param name="filePath">ファイルパス。</param>
		/// <param name="encoding">エンコーディング。</param>
		private void Open (string filePath, Encoding encoding)
		{
			if (_writer != null)
				return;

			// ライタの生成
			_writer = new StreamWriter (filePath, false, encoding);

		}

		/// <summary>
		/// ライタを閉じる。
		/// </summary>
		private void Close ()
		{
			if (_writer == null)
				return;

			_writer.Close ();
			_writer.Dispose ();
			_writer = null;

			AssetDatabase.Refresh (ImportAssetOptions.ImportRecursive);

		}

		#endregion

		#region IDisposable メンバ

		/// <summary>
		/// 解放済みか。
		/// </summary>
		private bool _disposed = false;

		/// <summary>
		/// リソースを解放する。
		/// </summary>
		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		/// <summary>
		/// リソースを解放する。
		/// </summary>
		/// <param name="disposing">
		/// マネージリソースとアンマネージリソースの両方を解放する場合は <c>true</c>。<br/>
		/// アンマネージリソースだけを解放する場合は <c>false</c>。
		/// </param>
		protected virtual void Dispose (bool disposing)
		{
			if (!_disposed)
			if (disposing)
				Close ();

			_disposed = true;
		}

		#endregion
	}
}