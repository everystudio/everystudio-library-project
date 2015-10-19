using System.Reflection;
using System;
using System.Text;
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using MakeDataNamespace;
using System.Linq;


public class CSMaker : EditorWindow
{

	public class DataProperty
	{
		string DataName;
	}

	public static List<DataProperty> dataList;

	public static List<Type> makeTypeList;

	public static List<Type> dataSeetTypeList;

    public class MyToggle
    {
        public bool Enable;
        public bool Validate;
        public Type srcType;
    }


	public static void SetSTODataFromCSV()
	{
		Debug.LogWarning ("CSVデータをスクリプタブルオブジェクトへ転送 :");

		List<Type> addList = ReadClass ();

		foreach (Type item in addList) {
			Type exeType = Type.GetType (item.Name + "CSVLoader");
			exeType.InvokeMember ("SetScriptableData", BindingFlags.InvokeMethod, null, null, new object[] {item});
		}
	}


    public static Dictionary<string, MyToggle> dataSeetMyToggleDictionary = new Dictionary<string, MyToggle>();

    public static Dictionary<string, MyToggle>  ReadMyToggle ()
    {

        string dllPath = 
            System.IO.Path.Combine (System.IO.Directory.GetCurrentDirectory (),
                "Library/ScriptAssemblies/Assembly-CSharp.dll");

        IList<string> list = new List<string>(dataSeetMyToggleDictionary.Keys);
        foreach (string str in list)
        {
            dataSeetMyToggleDictionary[str].Validate = false;
        }

        Assembly asm = Assembly.LoadFile (dllPath);
        Type[] types = asm.GetTypes ();
        foreach (Type t in types) {
            if (t.Namespace == "MakeDataNamespace") {
                if (t.Name != "DataMakeSheet") {
                    if (dataSeetMyToggleDictionary.ContainsKey(t.Name))
                    {
                        dataSeetMyToggleDictionary[t.Name].Validate = true;
                    }
                    else
                    {
                        MyToggle toggle = new MyToggle();
                        toggle.Enable = true;
                        toggle.Validate = true;
						toggle.srcType = t;
                        dataSeetMyToggleDictionary.Add(t.Name, toggle);
                    }
                }
            }
        }
        return dataSeetMyToggleDictionary;
    }


	//定義シートから設置クラスを読み取る
	public static List<Type> ReadClass ()
	{
		dataSeetTypeList = new List<Type> ();

		IList<string> list = new List<string>(dataSeetMyToggleDictionary.Keys);
		foreach (string str in list)
		{
			if (CSMaker.dataSeetMyToggleDictionary[str].Validate)
			{
				if(CSMaker.dataSeetMyToggleDictionary[str].Enable){
					dataSeetTypeList.Add(CSMaker.dataSeetMyToggleDictionary[str].srcType);
				}
			}
		}
		return dataSeetTypeList;
	}

	//特定のクラスを名前で取得
	public static Type GetClassByName (String _name)
	{
		string dllPath = 
			System.IO.Path.Combine (System.IO.Directory.GetCurrentDirectory (),
				"Library/ScriptAssemblies/Assembly-CSharp.dll");

		dataSeetTypeList = new List<Type> ();

		Assembly asm = Assembly.LoadFile (dllPath);
		Type[] types = asm.GetTypes ();
		foreach (Type t in types) {
			if (t.Name == _name) {
				dataSeetTypeList.Add (t);
			}
		}

		if (dataSeetTypeList.Count > 1) {
			Debug.LogError ("同名定義ファイルが複数あります" + dataSeetTypeList.Count);
		}else if (dataSeetTypeList.Count == 0) {
			Debug.LogError ("同名定義ファイルがありません" + dataSeetTypeList.Count);
		}
		return dataSeetTypeList [0];
	}


	public static void GenerateScriptableObject ()
	{
		Debug.LogWarning ("スクリプタブルオブジェクト生成 :");

		List<Type> addList = ReadClass ();
		List<String> exeClassName = new List<string> ();

		foreach (Type item in addList) {
			string className = item.Name + "ObjectMaker";
			exeClassName.Add (className);
		}

		foreach (String item in exeClassName) {
			Type exeType = GetClassByName (item);
			exeType.InvokeMember ("MakeScriptableWizard", BindingFlags.InvokeMethod, null, null, null);
		}
	}


	public static void AtachObject ()
	{
		List<Type> addList = ReadClass ();
		List<String> exeClassName = new List<string> ();

		foreach (Type item in addList) {
			string className = item.Name + "Atach";
			exeClassName.Add (className);
		}

		foreach (String item in exeClassName) {
			Type exeType = GetClassByName (item);
			exeType.InvokeMember ("Makeholder", BindingFlags.InvokeMethod, null, null, null);
		}
	}

	//CSVのローダをパース
	public static void MakeCSVLoader ()
	{
		List<Type> addList = ReadClass ();

		foreach (Type item in addList) {
			MakeCSVLoaderFormat (item);
		}
	}

	//Excelのローダをパース
	public static void MakeExcelLoader ()
	{

		List<Type> addList = ReadClass ();

		foreach (Type item in addList) {
			MakeExcelLoaderFormat (item);
		}

	}

	//ExcelLoder生成ファイルの自動生成
	public static void MakeExcelLoaderFormat (Type _dataType)
	{
		string result = ReplaceTemplate (_dataType, "ExcelLoaderTemplate");
		FieldInfo[] fieldInfoList = CSMaker.GetFieldInfo (_dataType);
		List<string> memberNameList = CSMaker.GetMemberList (_dataType);

		int dataCount = 0;

		string tmpTypeString = "";

		foreach (string memberName in memberNameList) {
			tmpTypeString += "\n\t\t\t";
			string fieldType = fieldInfoList [dataCount].FieldType.ToString ();

			switch (fieldType) {
			case "System.Single":
				tmpTypeString += "\t" + "tmpData." + memberName + " = (float)row.GetCell (iColumn++).NumericCellValue;";
				break;
			case "System.String":
				tmpTypeString += "\t" + "tmpData." + memberName + " = row.GetCell (iColumn++).StringCellValue;";
				break;
			case "System.Int32":
				tmpTypeString += "\t" + "tmpData." + memberName + " = (int)row.GetCell (iColumn++).NumericCellValue;";
				break;
			case "System.Boolean":
				tmpTypeString += "\t" + "tmpData." + memberName + " = BoolSetting(row.GetCell (iColumn++).StringCellValue);";
				break;
			default:
				break;
			}
			dataCount++;
		}
		result = Regex.Replace (result, "{DataSwitch}", "//DataSwitch :" + tmpTypeString);
		string fileURL = SystemSetting.GetEditorFileSrcPath () + _dataType.Name + "ExcelLoader.cs";
		CreateFormatFile (fileURL, result);
	}


	//ExcelLoder生成ファイルの自動生成
	public static void MakeCSVLoaderFormat (Type _dataType)
	{
		string result = ReplaceTemplate (_dataType, "CSVLoaderTemplate");

		string tmpTypeString = "";
		result = Regex.Replace (result, "{DataSwitch}", "//DataSwitch :" + tmpTypeString);
		string fileURL = SystemSetting.GetEditorFileSrcPath () + _dataType.Name + "CSVLoader.cs";
		CreateFormatFile (fileURL, result);
	}



	//DataContainerファイルの自動生成
	public static void MakeDataContainer ()
	{
		List<Type> classList = ReadClass();

		int dataCount = 0;
		string tmpTypeString = "";

		foreach (Type item in classList) {
			string typeName = item.Name;

			if(typeName == "DataMakeSheet"){
				continue;
			}
			tmpTypeString += "\n";
			tmpTypeString += "\t" + "public List<" + typeName + ".Data>  " + typeName + "List;";
			dataCount++;
		}

		string datasetString = "";

		foreach (Type item in classList) {
			string typeName = item.Name;

			if(typeName == "DataMakeSheet"){
				continue;
			}
			datasetString += "\n\t\t";

			datasetString += "case \"" + typeName + "Prefab\":\n\t\t\tDataContainer.Instance." + 
				typeName + "List = obj.GetComponent<" + typeName +
			"DataHolder> ().assetBundleData.DataList;\n\t\t\tbreak;";
		}
		string tmplateURL = SystemSetting.GetTemplateFolder () + "DataContainerTemplate";
		TextAsset csv = (TextAsset)Resources.Load (tmplateURL, typeof(TextAsset)) as TextAsset;
		string result = Regex.Replace (csv.text, "{mainString}", tmpTypeString);
		result = Regex.Replace (result, "{datasetString}", datasetString);
		string fileURL = SystemSetting.GetContainerFilePath () + "DataContainer.cs";

		CreateFormatFile (fileURL, result);
		Debug.LogWarning ("MakeEnd :");
	}

	public static void CSParser ()
	{
		List<Type> addList = ReadClass ();

		foreach (Type item in addList) {
			MakeFormat (item);
		}
	}

	//スクリプタブルオブジェクトを生成するコードを書くクラス
	public static void CSParserObjectMaker ()
	{
		List<Type> addList = ReadClass ();

		foreach (Type item in addList) {
			MakeDataObjectMakerFormat (item);
			MakeDataObjectHolderFormat (item);
		}
	}


	//スクリプタブルオブジェクトをHolderファイルへアタッチするスクリプトの自動生成
	public static void MakeAtachHolderFormat ()
	{
		List<Type> addList = ReadClass ();

		foreach (Type item in addList) {
			string result = ReplaceTemplate (item, "AtachDataHolderTemplate");
			string fileURL = SystemSetting.GetCSFileSrcPath () + item.Name + "Atach.cs";

			CreateFormatFile (fileURL, result);
		}
	}


	//スクリプタブルオブジェクトHolderファイルの自動生成
	public static void MakeDataObjectHolderFormat (Type _dataType)
	{
		string result = ReplaceTemplate (_dataType, "DataHolderTemplate");
		string fileURL = SystemSetting.GetCSFileSrcPath () + _dataType.Name + "DataHolder.cs";
		CreateFormatFile (fileURL, result);
	}


	//スクリプタブルオブジェクトの生成ファイルの自動生成
	public static void MakeDataObjectMakerFormat (Type _dataType)
	{
		string result = ReplaceTemplate (_dataType, "ScriptableWizardTemplate");
		string fileURL = SystemSetting.GetCSFileSrcPath () + _dataType.Name + "ObjectMaker.cs";
		CreateFormatFile (fileURL, result);

	}

	//データ基本定義ファイルの自動生成
	public static void MakeCS (Type _type, string _infoString = "")
	{
		string result = ReplaceTemplate (_type, "DataTemplate");
		result = Regex.Replace (result, "{infoString}", _infoString);

		string fileURL = SystemSetting.GetCSFileSrcPath () + _type.Name + ".cs";
		CreateFormatFile (fileURL, result);
	}


	private static string ReplaceTemplate (Type _dataType, string _templateName)
	{
		string tmplateURL = SystemSetting.GetTemplateFolder () + _templateName;
		TextAsset csv = (TextAsset)Resources.Load (tmplateURL, typeof(TextAsset)) as TextAsset;
		string result = Regex.Replace (csv.text, "{className}", _dataType.Name);

		return result;
	}

	//データ生成書き込みクラス
	private static void CreateFormatFile (string _fileURL, string _fileBody)
	{
		System.IO.StreamWriter sw = new System.IO.StreamWriter (
			                            _fileURL,
			                            false,
			System.Text.Encoding.GetEncoding ("utf-8"));

		sw.WriteLine (_fileBody);
		sw.Close ();

		AssetDatabase.Refresh (ImportAssetOptions.ImportRecursive);
	}


	public static MemberInfo[] GetMemberInfo (Type _dataType)
	{
		MemberInfo[] members = _dataType.GetMembers (
			                       BindingFlags.Public |
			                       BindingFlags.Instance);
		return members;
	}


	public static FieldInfo[] GetFieldInfo (Type _dataType)
	{
		List<FieldInfo> fieldInfoList = new List<FieldInfo> ();

		foreach (MemberInfo m in GetMemberInfo (_dataType)) {
			if (m.MemberType == MemberTypes.Field) {
				FieldInfo memberFieldInfo = _dataType.GetField (m.Name.ToString (), BindingFlags.Public |
				                            BindingFlags.Instance);

				fieldInfoList.Add (memberFieldInfo);
			}
		}
		return fieldInfoList.ToArray ();
	}


	public static List<string> GetMemberList (Type _dataType)
	{
		List<string> memberList = new List<string> ();

		foreach (MemberInfo m in GetMemberInfo (_dataType)) {
			if (m.MemberType == MemberTypes.Field) {
				memberList.Add (m.Name.ToString ());
			}
		}
		return memberList;
	}

	public static void MakeFormat (Type _dataType)
	{
		FieldInfo[] fieldInfoList = GetFieldInfo (_dataType);
		List<string> memberList = GetMemberList (_dataType);

		string exString = "";
		List<string> typeList = new List<string> ();

		foreach (FieldInfo memberFieldInfo in fieldInfoList) {
			string tmpTypeString = "";

			if (memberFieldInfo.FieldType.ToString () == "System.Int32") {
				tmpTypeString = "\t" + "public int" + "\t\t"; 
			} else if (memberFieldInfo.FieldType.ToString () == "System.Single") {
				tmpTypeString = "\t" + "public float" + "\t"; 
			} else if (memberFieldInfo.FieldType.ToString () == "System.String") {
				tmpTypeString = "\t" + "public string" + "\t";
			} else if (memberFieldInfo.FieldType.ToString () == "System.Boolean") {
				tmpTypeString = "\t" + "public bool" + "\t\t";
			} else {
				Debug.LogError ("データ定義に異常があります!!!!!!!" + memberFieldInfo.FieldType);
				//異常なデータは出力してはいけないので強制停止
				break;
			}
			typeList.Add (tmpTypeString);
		}

		for (int i = 0; i < fieldInfoList.Length; i++) {
			exString += "\t" + typeList [i] + " " + memberList [i] + ";\n";
		}
		MakeCS (_dataType, exString);
	}

	public static void MakeDataList ()
	{
		dataList = new List<DataProperty> ();
	}

	public void DataReset ()
	{



	}

}
