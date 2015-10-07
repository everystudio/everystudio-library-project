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


public class AssetBudleDataChecker : EditorWindow
{

	static AssetBudleDataChecker w;
	private AssetBundle bundle;

	/*
	/// <summary]] > 
	/// UIウインドウ初期化.
	/// </summary]] > 
	[MenuItem ("Tools/アセットバンドル生成/アセットバンドルチェック")]
	static void Init ()
	{
		w = (AssetBudleDataChecker)EditorWindow.GetWindow (typeof(AssetBudleDataChecker));
		w.minSize = new Vector2 (200.0f, 500.0f);
	}


	void OnGUI ()
	{
		GUILayout.Label ("Check AssetBundle", EditorStyles.boldLabel);
		GUILayout.Label ("アセットバンドルのチェックを行います", EditorStyles.boldLabel);
		GUILayout.Space (20f);

		GUILayout.Label ("アトラスデータのアセットバンドルをチェック", EditorStyles.boldLabel);
		//foreach (string buildURL in buildURLArray) {
		if (GUILayout.Button ("Check")) {
			DataSeetMake ();
		}

		if (GUILayout.Button ("LoadCheckData")) {
			LoadCheckData ();
		}


	}
	*/

	public static void DataSeetMake ()
	{
		MakeData ();
	}

	static int makeCellLow;

	static void MakeData ()
	{
		AssetBundleData data = LoadCheckData ();

		//const Int32 MAX_COL = 9;
		int maxRow = data.list.Count;
		//Int32 iCol = 0;
		Int32 iRow = 0;
		IRow row;
		ICell cell;
 
		// ワークブックオブジェクト生成
		HSSFWorkbook workbook = new HSSFWorkbook ();
 
		// シートオブジェクト生成
		ISheet sheet1 = workbook.CreateSheet ("DataChecker");
 
		makeCellLow = 0;

		// セルスタイル（黒線）
		ICellStyle blackBorder = workbook.CreateCellStyle ();
		blackBorder.BorderBottom = BorderStyle.THIN;
		blackBorder.BorderLeft = BorderStyle.THIN;
		blackBorder.BorderRight = BorderStyle.THIN;
		blackBorder.BorderTop = BorderStyle.THIN;
		blackBorder.BottomBorderColor = HSSFColor.BLACK.index;
		blackBorder.LeftBorderColor = HSSFColor.BLACK.index;
		blackBorder.RightBorderColor = HSSFColor.BLACK.index;
		blackBorder.TopBorderColor = HSSFColor.BLACK.index;
 

		// セルスタイル（黒線）
		HSSFCellStyle hStyle = (HSSFCellStyle)workbook.CreateCellStyle ();

		hStyle.FillForegroundColor = IndexedColors.AQUA.Index;
		hStyle.FillPattern = FillPatternType.SOLID_FOREGROUND;

		hStyle.BorderBottom = BorderStyle.THIN;
		hStyle.BorderLeft = BorderStyle.THIN;
		hStyle.BorderRight = BorderStyle.THIN;
		hStyle.BorderTop = BorderStyle.THIN;
		hStyle.BottomBorderColor = HSSFColor.BLACK.index;
		hStyle.LeftBorderColor = HSSFColor.BLACK.index;
		hStyle.RightBorderColor = HSSFColor.BLACK.index;
		hStyle.TopBorderColor = HSSFColor.BLACK.index;


		// セルスタイル（黒線）
		HSSFCellStyle hStyleRed = (HSSFCellStyle)workbook.CreateCellStyle ();

		hStyleRed.FillForegroundColor = IndexedColors.DARK_RED.Index;
		hStyleRed.FillPattern = FillPatternType.SOLID_FOREGROUND;

		hStyleRed.BorderBottom = BorderStyle.THIN;
		hStyleRed.BorderLeft = BorderStyle.THIN;
		hStyleRed.BorderRight = BorderStyle.THIN;
		hStyleRed.BorderTop = BorderStyle.THIN;
		hStyleRed.BottomBorderColor = HSSFColor.BLACK.index;
		hStyleRed.LeftBorderColor = HSSFColor.BLACK.index;
		hStyleRed.RightBorderColor = HSSFColor.BLACK.index;
		hStyleRed.TopBorderColor = HSSFColor.BLACK.index;



		HSSFCellStyle headStyle = (HSSFCellStyle)workbook.CreateCellStyle ();

		headStyle.FillForegroundColor = IndexedColors.BRIGHT_GREEN.Index;
		headStyle.FillPattern = FillPatternType.SOLID_FOREGROUND;
 

		HSSFCellStyle headStylePrefab = (HSSFCellStyle)workbook.CreateCellStyle ();
		headStylePrefab.FillForegroundColor = IndexedColors.LEMON_CHIFFON.Index;
		headStylePrefab.FillPattern = FillPatternType.SOLID_FOREGROUND;

		HSSFCellStyle headStyleBundle = (HSSFCellStyle)workbook.CreateCellStyle ();
		headStyleBundle.FillForegroundColor = IndexedColors.GOLD.Index;
		headStyleBundle.FillPattern = FillPatternType.SOLID_FOREGROUND;


		// IWorkbook doc
		IFont font = workbook.CreateFont ();
		font.FontHeightInPoints = 12;
		font.FontName = "Arial";
		font.Boldweight = (short)FontBoldWeight.BOLD;


		headStyle.SetFont (font);

		string[] titleText = { "■■■ EXCELシート ■■■", "", "", "■■■ プレハブ ■■■", "", "■■■ アセットバンドル ■■■", "" };
		int[] columnWidth = { 35, 12, 20, 30, 12, 30, 12 };
		HSSFCellStyle[] hssfCellStyle = {
			headStyle,
			headStyle,
			headStyle,
			headStylePrefab,
			headStylePrefab,
			headStyleBundle,
			headStyleBundle
		};

		CellMake (sheet1, titleText, hssfCellStyle, columnWidth);


		string[] titleTextB = { "アセットバンドル名:", "バージョン", "フォルダ名", "プレハブ最終出力時間", "プレハブVer", "アセットバンドル最終出力時間", "アセットVer" };
		int[] columnWidthB = { 35, 12, 20, 30, 12, 30, 12 };
		HSSFCellStyle[] hssfCellStyleB = {
			headStyle,
			headStyle,
			headStyle,
			headStylePrefab,
			headStylePrefab,
			headStyleBundle,
			headStyleBundle
		};

		CellMake (sheet1, titleTextB, hssfCellStyleB, columnWidthB);

		int countDataListIndex = 0;

		// セルを作成する（垂直方向）
		for (iRow = (makeCellLow); iRow < (maxRow + makeCellLow); iRow++) {

			int cellIndex = 0;

			//	Debug.Log ("Cell :" + cellIndex);
			Debug.Log ("iRow :" + iRow);

			string assebundlename = data.list [countDataListIndex].assetbundleName;
			string folderName = data.list [countDataListIndex].folderName;

			Debug.Log ("読み込みアセットバンドル数 :" + data.list.Count);
			Debug.Log ("読み込みアセットバンドル名 :" + assebundlename);

			row = sheet1.CreateRow (iRow);



			//アセットバンドル名
			cell = row.CreateCell (cellIndex++);
			string objName = assebundlename;
			cell.SetCellValue (objName);
			cell.CellStyle = hStyle;

			//バージョン名
			cell = row.CreateCell (cellIndex++);
			int version = data.list [countDataListIndex].version;
			cell.SetCellValue (version);
			cell.CellStyle = hStyle;


			//フォルダ名 
			cell = row.CreateCell (cellIndex++);
			cell.SetCellValue (folderName);
			cell.CellStyle = hStyle;


			//プレハブ最終出力時間 
			string returnString = GetFileInfo (assebundlename, folderName, AssetBundleInfo.InfoType.MAKETIME);

			string prefTime = returnString;

			cell = row.CreateCell (cellIndex++);
			cell.SetCellValue (returnString);
			cell.CellStyle = hStyle;

			//プレハブバージョン
			returnString = GetFileInfo (assebundlename, folderName, AssetBundleInfo.InfoType.VERID);
			cell = row.CreateCell (cellIndex++);
			cell.SetCellValue (returnString);
			cell.CellStyle = hStyle;


			//アセットバンドル最終出力時間
			returnString = GetAssetBundleFileInfo (assebundlename, folderName, AssetBundleInfo.InfoType.MAKETIME);

			cell = row.CreateCell (cellIndex++);
			cell.SetCellValue (returnString);

			if (prefTime != returnString) {
				cell.CellStyle = hStyleRed;
			}else{
				cell.CellStyle = hStyle;
			}

			returnString = GetAssetBundleFileInfo (assebundlename, folderName, AssetBundleInfo.InfoType.VERID);

			cell = row.CreateCell (cellIndex++);
			cell.SetCellValue (returnString);
			cell.CellStyle = hStyle;

			countDataListIndex++;
		}


		string dataURL = SystemSetting.GetBundleVerUrl() + SystemSetting.GetBundleVerName();


		// Excelファイル出力
		OutputExcelFile (dataURL, workbook);
		AssetDatabase.Refresh (ImportAssetOptions.ImportRecursive);

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


	static string GetAssetBundleFileInfo (string _assetbundleName, string _folderName, AssetBundleInfo.InfoType _infoType)
	{
		GameObject obj = GetAssetBundleInfo (_assetbundleName, _folderName);
		AssetBundleInfo info = obj.GetComponent<AssetBundleInfo> ();

		string returnString = "";

		if (info != null) {

			switch (_infoType) {
			case AssetBundleInfo.InfoType.MAKETIME:
				returnString = info.makeTime;
				break;
			case AssetBundleInfo.InfoType.LASTAUTHER:
				returnString = info.lastAuthor;
				break;
			case AssetBundleInfo.InfoType.VERID:
				returnString = info.verID.ToString ();
				break;
			default:
				Debug.LogError ("データ設定に異常があります");
				break;
			}
		}

		DestroyImmediate (obj);
		return returnString;
	}


	//アセットバンドルからGameObjectを取り出し
	static GameObject GetAssetBundleInfo (string _assetbundleName, string _folderName)
	{
        //string source_path = "AssetbundleList/AssetBundleHolder."+DEFINE.ASSET_BUNDLE_PREFIX+".unity3d";
		//int assetVersion = DataManager.Instance.asset_version;

		//TODO 後でバージョン処理追加
		//int assetVersion = 0;


		//"file:///Users/kanada/Documents/WorkE/New Unity Project 6/Assets/StreamingAssets/";

		// ここのみ
        EditPlayerSettingsData data = ConfigManager.instance.GetEditPlayerSettingsData();
        string resultUrl = data.m_strS3Url + _folderName + "/" + _assetbundleName + "Prefab."+".unity3d";

		Debug.Log ("Loading :" + resultUrl);

		// 同じバージョンが存在する場合はアセットバンドルをキャッシュからロードするか、またはダウンロードしてキャッシュに格納します。
		WWW www = new WWW (resultUrl);

		Debug.LogWarning ("Load :" + resultUrl);

		if (www.error != null) {
			throw new Exception ("WWWダウンロードにエラーがありました:" + www.error);
		}
		AssetBundle bundle = www.assetBundle;
		Debug.Log ("ロードアセットバンドルファイル名 :" + _assetbundleName);


        GameObject tmpObj = Instantiate (bundle.LoadAsset (_assetbundleName + "Prefab", typeof(GameObject))) as GameObject;

		if (tmpObj == null) {
			Debug.LogError ("アセットバンドルList内容に異常があります");
		}

		// メモリ節約のため圧縮されたアセットバンドルのコンテンツをアンロード
		bundle.Unload (false);

		return tmpObj;
	}


	//プレハブからの生成情報を取得
	static string GetFileInfo (string _assetbundleName, string _folderName, AssetBundleInfo.InfoType _infoType)
	{
		string buildURL = SystemSetting.GetResourcesAssetSrcPath () + _assetbundleName + "Prefab";
		UnityEngine.Object[] gameObjectArray = Resources.LoadAll (buildURL, typeof(GameObject));

		Debug.LogWarning("生成情報" + buildURL);

		AssetBundleInfo info;
		string returnString = "";

		foreach (var item in gameObjectArray) {
	
			GameObject obj = (GameObject)item as GameObject;
			info = obj.GetComponent<AssetBundleInfo> ();

			if (info != null) {
				switch (_infoType) {
				case AssetBundleInfo.InfoType.MAKETIME:
					returnString = info.makeTime;
					break;
				case AssetBundleInfo.InfoType.LASTAUTHER:
					returnString = info.lastAuthor;
					break;
				case AssetBundleInfo.InfoType.VERID:
					returnString = info.verID.ToString ();
					break;
				default:
					Debug.LogError ("データ設定に異常があります");
					break;
				}
			}
		}

		return returnString;
	}
 
	//-------------------------------------------------
	// Excelファイル出力
	//-------------------------------------------------
	static void OutputExcelFile (String strFileName, HSSFWorkbook workbook)
	{

		Debug.LogWarning ("strFileName :" + strFileName);

		FileStream file = new FileStream (strFileName, FileMode.Create);
		workbook.Write (file);
		file.Close ();
	}
 
	//-------------------------------------------------
	// セルの位置を取得する
	//-------------------------------------------------
	static String GetCellPos (Int32 iRow, Int32 iCol)
	{
		iCol = Convert.ToInt32 ('A') + iCol;
		iRow = iRow + 1;
		return ((char)iCol) + iRow.ToString ();
	}




	/*

	static void Main (string[] args)
	{
		// Excelのブックを作成
		var book = new HSSFWorkbook ();

		// シートを作成
		var sheet = book.CreateSheet ("何とかレポート");

		// ヘッダーにあたる行を作成
		CreateHeaderRow (book, sheet);


		//`AssetBudleDataChecker.CreateHeaderRow(NPOI.HSSF.UserModel.HSSFWorkbook, NPOI.HSSF.UserModel.HSSFSheet)' has some invalid arguments


		// とりあえず１０行くらいデータ作成
		foreach (var index in Enumerable.Range(1, 10)) {
			CreateRow (book, sheet, index);
		}

		// 2列目と3列目は、そのままだと幅が足りないので広げる(256で1文字ぶんの幅らしい）
		sheet.SetColumnWidth (1, 256 * 12);
		sheet.SetColumnWidth (2, 256 * 15);

		// output.xlsに保存
		using (var fs = new FileStream ("otuput.xls", FileMode.OpenOrCreate, FileAccess.Write)) {
			book.Write (fs);
		}
	}


	// ヘッダー行を作成する
	private static void CreateHeaderRow (HSSFWorkbook book, HSSFSheet sheet)
	{
		var row = sheet.CreateRow (0);

		// 0列目はIDの列
		var idCell = row.CreateCell (0);
		idCell.SetCellValue ("ID");

		// 1列目は名前の列
		var nameCell = row.CreateCell (1);
		nameCell.SetCellValue ("名前");

		// 3列目は誕生日の列
		var birthdayCell = row.CreateCell (2);
		birthdayCell.SetCellValue ("誕生日");

		// 4方に罫線
		var style = book.CreateCellStyle ();
		style.BorderTop = HSSFBorderFormatting.BORDER_THIN;
		style.BorderLeft = HSSFBorderFormatting.BORDER_THIN;
		style.BorderBottom = HSSFBorderFormatting.BORDER_THIN;
		style.BorderRight = HSSFBorderFormatting.BORDER_THIN;

		// 薄いグリーンの背景色で塗りつぶす
		style.FillForegroundColor = HSSFColor.LIGHT_GREEN.index;
		style.FillPattern = HSSFCellStyle.SOLID_FOREGROUND;
		// テキストはセンタリング
		style.Alignment = HSSFCellStyle.ALIGN_CENTER;

		// 太字
		var font = book.CreateFont ();
		font.Boldweight = HSSFFont.BOLDWEIGHT_BOLD;
		style.SetFont (font);

		// 全てのヘッダー用のセルに、上で作ったスタイルを適用する
		foreach (var cell in new[] { idCell, nameCell, birthdayCell }) {
			cell.CellStyle = style;
		}

	}

	private static System.Random r = new System.Random ();

	// index行目のデータを作る
	private static void CreateRow (HSSFWorkbook book, HSSFSheet sheet, int index)
	{
		// 行を作って
		var row = sheet.CreateRow (index);

		// id列を作る
		var idCell = row.CreateCell (0);
		idCell.SetCellValue (index);

		// 名前も適当に入れて
		var nameCell = row.CreateCell (1);
		nameCell.SetCellValue ("田中　太郎" + index);

		// 誕生日も適当に
		var birthdayCell = row.CreateCell (2);
		birthdayCell.SetCellValue (DateTime.Now.AddYears (r.Next (10)));

		// 全ての列に4方に罫線のあるスタイルを作って適用する
		// あえて別々のスタイルを設定してるのは、誕生日セルにフォーマットを入れるため
		foreach (var cell in new[] { idCell, nameCell, birthdayCell }) {
			var style = book.CreateCellStyle ();
			style.BorderTop = HSSFBorderFormatting.BORDER_THIN;
			style.BorderRight = HSSFBorderFormatting.BORDER_THIN;
			style.BorderLeft = HSSFBorderFormatting.BORDER_THIN;
			style.BorderBottom = HSSFBorderFormatting.BORDER_THIN;

			cell.CellStyle = style;
		}
		// 日付用yyyy年mm月dd日のフォーマットで誕生日は表示するようにする
		var format = book.CreateDataFormat ();
		birthdayCell.CellStyle.DataFormat = format.GetFormat ("yyyy年mm月dd日");
	}


	*/


	//##################################################
	// Loading
	//##################################################

	//private static readonly string filePath = "Assets/00_AssetBundleData/Resources/Admini/AssetBundleData.xls";
	//private static readonly string exportPath = "Assets/00_AssetBundleData/Resources/Admini/AssetBundleData.asset";


	static AssetBundleData LoadCheckData ()
	{
		//	if (!filePath.Equals (asset))
		//		continue;

		AssetBundleData data = (AssetBundleData)AssetDatabase.LoadAssetAtPath (SystemSetting.GetAdminAssetPath(), typeof(AssetBundleData));

		Debug.LogWarning("###### SystemSetting.GetAdminAssetPath() :"+SystemSetting.GetAdminAssetPath());

		return data;


		//AssetBundleData data = (AssetBundleData)AssetDatabase.LoadAssetAtPath (SystemSetting.GetAdminAssetPath(), typeof(AssetBundleData));

		if (data == null) {
			data = ScriptableObject.CreateInstance<AssetBundleData> ();
			AssetDatabase.CreateAsset ((ScriptableObject)data, SystemSetting.GetAdminAssetPath());
			data.hideFlags = HideFlags.NotEditable;
		}

		data.list.Clear ();

		using (FileStream stream = File.Open (SystemSetting.GetAdminExcelPath(), FileMode.Open, FileAccess.Read)) {
			IWorkbook book = new HSSFWorkbook (stream);
				
			ISheet sheet = book.GetSheet ("AssetList");
				
			////Debug.Log (sheet.SheetName);
				
			for (int i = 1; i < sheet.LastRowNum; i++) {
					
				IRow row = sheet.GetRow (i);

				AssetBundleData.Param p = new AssetBundleData.Param ();




				p.assetbundleName = row.GetCell (0).StringCellValue;
				p.version = (int)row.GetCell (1).NumericCellValue;
				p.type = (int)row.GetCell (2).NumericCellValue;


				int tmpUpdate = (int)row.GetCell (3).NumericCellValue;

				if (tmpUpdate == 1) {
					p.update = true;
				} else {
					p.update = false;
				}

				p.folderName = row.GetCell (5).StringCellValue;
				data.list.Add (p);


				//data.waveParamlist.Add (p);
			}


			return data;
		}


			
//		ScriptableObject obj = AssetDatabase.LoadAssetAtPath (exportPath, typeof(ScriptableObject)) as ScriptableObject;
//		EditorUtility.SetDirty (obj);//
		//	}
	}

	
	static bool BoolSetting (int _val)
	{
		if (_val == 0) {
			return false;
		}
		return  true;
	}

	/*
	static MiniGameA.Enemy.MoveType MoveTypeSetting (string type)
	{
		
		MiniGameA.Enemy.MoveType returnType = (MiniGameA.Enemy.MoveType)System.Enum.Parse (typeof(MiniGameA.Enemy.MoveType), type);
		
		return  returnType;
	}

	static MiniGameA.TerrainType TerrainTypeSetting (string type)
	{
		
		MiniGameA.TerrainType returnType = (MiniGameA.TerrainType)System.Enum.Parse (typeof(MiniGameA.TerrainType), type);
		
		return  returnType;
	}


	static MiniGameA.ArrivalPos ArrivalPosSetting (string type)
	{
		
		MiniGameA.ArrivalPos returnType = (MiniGameA.ArrivalPos)System.Enum.Parse (typeof(MiniGameA.ArrivalPos), type);
		
		return  returnType;
	}
	*/


}
