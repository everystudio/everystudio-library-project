using System;
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using System.Xml.Serialization;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.Reflection;


public class ExcelParser : EditorWindow {

	

	public static string dataTitle;
	public static List<string> memberList;
	public static List<string> typeList;
	static int makeCellLow;

	static HSSFCellStyle headStyle;
	static ICellStyle blackBorder;
	static HSSFCellStyle headStylePrefab;
	static HSSFCellStyle headStyleBundle;

	public static void MakeDataSeet ()
	{
		List<Type> addList = CSMaker.ReadClass ();

		foreach (Type item in addList) {
			MakeData (item);
		}
	}

	//Excelスタイル定義
	private static void MakeSeetStyle (HSSFWorkbook workbook)
	{

		// 本体のスタイル（黒線）
		blackBorder = workbook.CreateCellStyle ();
		blackBorder.BorderBottom = BorderStyle.THIN;
		blackBorder.BorderLeft = BorderStyle.THIN;
		blackBorder.BorderRight = BorderStyle.THIN;
		blackBorder.BorderTop = BorderStyle.THIN;
		blackBorder.BottomBorderColor = HSSFColor.BLACK.index;
		blackBorder.LeftBorderColor = HSSFColor.BLACK.index;
		blackBorder.RightBorderColor = HSSFColor.BLACK.index;
		blackBorder.TopBorderColor = HSSFColor.BLACK.index;


		// ヘッダのスタイル
		headStyle = (HSSFCellStyle)workbook.CreateCellStyle ();

		headStyle.FillForegroundColor = IndexedColors.BRIGHT_GREEN.Index;
		headStyle.FillPattern = FillPatternType.SOLID_FOREGROUND;
 

		headStylePrefab = (HSSFCellStyle)workbook.CreateCellStyle ();
		headStylePrefab.FillForegroundColor = IndexedColors.LEMON_CHIFFON.Index;
		headStylePrefab.FillPattern = FillPatternType.SOLID_FOREGROUND;

		headStyleBundle = (HSSFCellStyle)workbook.CreateCellStyle ();
		headStyleBundle.FillForegroundColor = IndexedColors.GOLD.Index;
		headStyleBundle.FillPattern = FillPatternType.SOLID_FOREGROUND;

		// フォントスタイル
		IFont font = workbook.CreateFont ();
		font.FontHeightInPoints = 12;
		font.FontName = "Arial";
		font.Boldweight = (short)FontBoldWeight.BOLD;


		headStyle.SetFont (font);


	}


	static void MakeData (Type _dataType)
	{
		dataTitle = _dataType.Name;

		MakeFormat( _dataType);

		//const Int32 MAX_COL = 19;
		//int maxRow = memberList.Count;

		//Int32 iCol = 0;
		Int32 iRow = 0;
		IRow row;
		ICell cell;
 
 		
		int generateCellCount = SystemSetting.GetInitGenerateCell();


		// ワークブックオブジェクト生成
		HSSFWorkbook workbook = new HSSFWorkbook ();
 
		// シートオブジェクト生成
		ISheet sheet1 = workbook.CreateSheet ("MainSeet");
		MakeSeetStyle(workbook);
		makeCellLow = 0;

		List<string> titleTextB = memberList;

		DataCellMake (sheet1,titleTextB);



		// セルを作成する（垂直方向）
		for (iRow = (makeCellLow); iRow <( generateCellCount + makeCellLow); iRow++) {
			MakeCell( sheet1, _dataType, iRow);
		}

		row = sheet1.CreateRow (generateCellCount + makeCellLow);

		//アセットバンドル名
		cell = row.CreateCell (0);
		cell.SetCellValue ("end");
		cell.CellStyle = blackBorder;



		string dataURL = SystemSetting.GetExcelSeetPath() + dataTitle + "Sheet.xls";
		// Excelファイル出力
		OutputExcelFile (dataURL, workbook);
		AssetDatabase.Refresh (ImportAssetOptions.ImportRecursive);
	}


	static void DataCellMake ( ISheet _sheet1, List<string> _dataList)
	{
		IRow row = _sheet1.CreateRow (makeCellLow);
		makeCellLow++;

		row.HeightInPoints = 24;

		for (int i = 0; i < _dataList.Count; i++) {
			Debug.LogWarning ("Data :" + i + "  :" + _dataList [i]);
	
			ICell cell = row.CreateCell (i);
			cell.SetCellValue (_dataList [i]);

			cell.CellStyle = headStyle;
			_sheet1.SetColumnWidth (i, 255 * 20);
		}
	}


	static void CellMake (ISheet _sheet1,string[] _titleText, HSSFCellStyle[] _hssfCellStyle, int[] _columnWidth)
	{
		IRow row = _sheet1.CreateRow (makeCellLow);
		makeCellLow++;

		row.HeightInPoints = 24;

		for (int i = 0; i < _titleText.Length; i++) {
			Debug.LogWarning ("Data :" + i + "  :" + _titleText [i]);
	
			ICell cell = row.CreateCell (i);
			cell.SetCellValue (_titleText [i]);

			cell.CellStyle = _hssfCellStyle [i];
			_sheet1.SetColumnWidth (i, 255 * _columnWidth [i]);
		}
	}


	static void OutputExcelFile (String strFileName, HSSFWorkbook workbook)
	{

		Debug.LogWarning ("strFileName :" + strFileName);

		FileStream file = new FileStream (strFileName, FileMode.Create);
		workbook.Write (file);
		file.Close ();
	}


	public static void MakeCell (ISheet _sheet1, Type _dataType, int _iRow)
	{
		IRow row;
		ICell cell;

		int cellIndex = 0;

		row = _sheet1.CreateRow (_iRow);

		//メンバを取得する
		MemberInfo[] members = _dataType.GetMembers (
			                       BindingFlags.Public |
			                       BindingFlags.Instance);

		//string exString = "";

		memberList = new List<string> ();

		foreach (MemberInfo m in members) {
			if (m.MemberType == MemberTypes.Field) {
				memberList.Add (m.Name.ToString ());
			}
		}

		string objName = "None :";

		typeList = new List<string> ();

		int countNum = -1;
		foreach (string item in memberList) {
			countNum++;

			cell = row.CreateCell (cellIndex++);

			FieldInfo memberFieldInfo = _dataType.GetField (item, BindingFlags.Public |
			                            BindingFlags.Instance);

			if (memberFieldInfo.FieldType.ToString () == "System.Int32") {

				int setInt; 
				if (memberList [countNum] == "index") {
					setInt = _iRow-1;
				} else {
					setInt = 0;
				}
				cell.SetCellValue (setInt);

			} else if (memberFieldInfo.FieldType.ToString () == "System.Single") {

				float setFloat = 0.0f;
				cell.SetCellValue (setFloat);

			} else if (memberFieldInfo.FieldType.ToString() == "System.String") {
				objName = "null";
				cell.SetCellValue (objName);

			} else if (memberFieldInfo.FieldType.ToString() == "System.Boolean") {
				objName = "false";
				cell.SetCellValue (objName);

			} else {
				Debug.LogError("データ定義に異常があります!!!!!!!" + memberFieldInfo.FieldType);

				//異常なデータは出力してはいけないので強制停止
				break;
			}
			cell.CellStyle = blackBorder;
		}
	}


	//TODO 後で削除
	public static void MakeFormat (Type _dataType)
	{
		//メンバを取得する
		MemberInfo[] members = _dataType.GetMembers (
			                       BindingFlags.Public |
			                       BindingFlags.Instance);

		string exString = "";

		memberList = new List<string> ();

		foreach (MemberInfo m in members) {
			if (m.MemberType == MemberTypes.Field) {
				Debug.LogWarning ("MemberInfo   :" + m.Name);

				memberList.Add (m.Name.ToString ());
			}
		}


		typeList = new List<string> ();


		foreach (string item in memberList) {
			FieldInfo memberFieldInfo = _dataType.GetField (item, BindingFlags.Public |
			                            BindingFlags.Instance);

			Debug.LogWarning ("memberFieldInfo   :" + memberFieldInfo.FieldType);

			string tmpTypeString = "";


			if (memberFieldInfo.FieldType.ToString () == "System.Int32") {

				tmpTypeString = "public int"+"\t"; 

			} else if (memberFieldInfo.FieldType.ToString () == "System.Single") {

				tmpTypeString = "public float"; 

			} else if (memberFieldInfo.FieldType.ToString() == "System.String") {

				tmpTypeString = "public string"+"\t";
			} else if (memberFieldInfo.FieldType.ToString() == "System.Boolean") {
				tmpTypeString = "public bool"+"\t";
			} else {
				Debug.LogError("データ定義に異常があります!!!!!!!" + memberFieldInfo.FieldType);

				//異常なデータは出力してはいけないので強制停止
				break;

			}

			typeList.Add (tmpTypeString);

		}

		for (int i = 0; i < memberList.Count; i++) {
			exString += "\t" + typeList[i] + " " + memberList[i] + ";\n";
		}

	}

}
