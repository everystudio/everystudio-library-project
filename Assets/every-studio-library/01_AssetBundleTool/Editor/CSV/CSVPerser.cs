using System;
using System.Text;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class CSVPerser : EditorWindow
{

	public static string dataTitle;
	public static List<string> memberList;
	public static List<string> typeList;


	public static void MakeCSV ()
	{
		List<Type> addList = CSMaker.ReadClass ();

		foreach (Type item in addList) {
			MakeData (item);
		}
	}


	private static void MakeData (Type _dataType)
	{
		MakeFormat (_dataType);
		string filePath = SystemSetting.GetCSVFilePath () + StringExtensions.UpperCamelToSnake(_dataType.Name) + ".csv";
		using (Workaholism.IO.CsvWriter writer = new Workaholism.IO.CsvWriter (filePath, Encoding.GetEncoding ("utf-8"))) {
			string[] values = memberList.ToArray ();
			writer.WriteLine (values);
		}
	}


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

				tmpTypeString = "public int" + "\t\t"; 

			} else if (memberFieldInfo.FieldType.ToString () == "System.Single") {

				tmpTypeString = "public float" + "\t\t"; 

			} else if (memberFieldInfo.FieldType.ToString () == "System.String") {

				tmpTypeString = "public string" + "\t\t";
			} else if (memberFieldInfo.FieldType.ToString () == "System.Boolean") {
				tmpTypeString = "public bool" + "\t\t";
			} else {
				Debug.LogError ("データ定義に異常があります!!!!!!!" + memberFieldInfo.FieldType);

				//異常なデータは出力してはいけないので強制停止
				break;
			}
			typeList.Add (tmpTypeString);
		}

		for (int i = 0; i < memberList.Count; i++) {
			exString += "\t" + typeList [i] + " " + memberList [i] + ";\n";
		}
	}



	//アセットバンドル管理リストを生成
	public static void MakeAssetBundleList ()
	{
		string filePath = SystemSetting.GetAssetBundleListPath ()+".csv";
		List<Type> typeNameList = CSMaker.ReadClass ();
		List<string> memberNameList = CSMaker.GetMemberList (typeof(AssetBundleData.Param));
		//Type curType = typeof(AssetBundleData.Param);

        /*
        //アトラス未対応のため一旦コメントアウト
		using (Workaholism.IO.CsvWriter writer = new Workaholism.IO.CsvWriter (filePath, Encoding.GetEncoding ("utf-8"))) {
			List<string> headData = new List<string> ();

			foreach (string fieldName in memberNameList) {
				headData.Add ("\"" + fieldName + "\"");
			}
			writer.WriteLine (headData);

			foreach (Type curData in typeNameList) {
                Debug.LogWarning("Type :" + curData);
				List<string> lineData = new List<string> ();
				int columnCount = 0;

				foreach (string fieldName in memberNameList) {
					string addString = "";

					if (fieldName == "assetbundleName") {
						addString = "\"" + curData.Name + "\"";
					} else if (fieldName == "version") {
						addString = "999"; 
					} else if (fieldName == "type") {
						addString = "0"; 
					} else if (fieldName == "update") {
						addString = "0";
					} else if (fieldName == "arr_index") {
						addString = "0";
					} else if (fieldName == "folderName") {
						addString = "\"" + SystemSetting.GetassetBundlePathPlane () + "\"";
					} else {
						Debug.LogError ("データ定義に異常があります!!!!!!!" + fieldName);
						//異常なデータは出力してはいけないので強制停止
						break;
					}
					lineData.Add (addString);
					columnCount++;
				}
				writer.WriteLine (lineData);
			}
		}
        */

		//SetScriptableData(typeof( AssetBundleData));
		ObjectCreate ();
	}

	//スクリプタブルオブジェクトを生成
	public static void ObjectCreate ()
	{
		AssetBundleData loadSettingData = CreateInstance<AssetBundleData> ();
		string path = SystemSetting.GetAssetBundleListPath () + "Asset.asset";

		AssetDatabase.CreateAsset (loadSettingData, path);
		AssetDatabase.ImportAsset (path);
		EditorUtility.UnloadUnusedAssets ();

		CreateComponents ( typeof(AssetBundleData));
	}

	//プレハブを作ってスクリプタブルオブジェクトをアタッチするところまで.
	public static void CreateComponents (Type _type)
	{
		string name = "target";
		string outputPath =  SystemSetting.GetAssetBundleListPath () + "Prefab.prefab";


		GameObject gameObject = EditorUtility.CreateGameObjectWithHideFlags (
			                          name,
			                          HideFlags.HideInHierarchy,
			typeof(AssetBundleDataHolder)
		);

		//プレハブにスクリプタブルオブジェクトを設置
		UnityEngine.Object[] assets;

		string assetName = SystemSetting.GetAssetBundleDataFolder () + "AssetBundleDataAsset";

		assets = Resources.LoadAll (assetName);
		Debug.LogWarning ("GetObj :" + assetName.ToString ());


		AssetBundleData tmp = new AssetBundleData();

		foreach (UnityEngine.Object asset in assets) {
			if (asset is AssetBundleData) {
				tmp = (AssetBundleData)asset;
			}
		}

		AssetBundleDataHolder holder = gameObject.GetComponent<AssetBundleDataHolder> ();
		holder.assetBundleData = tmp;

		SetScriptableData (typeof(AssetBundleData));

		PrefabUtility.CreatePrefab (outputPath, gameObject, ReplacePrefabOptions.ReplaceNameBased);
		Editor.DestroyImmediate (gameObject);

	}


	public static void SetScriptableData (Type _type)
	{
	//	string filePath = SystemSetting.GetAssetBundleListPath ()+"CSV.csv";
	
		//プレハブにスクリプタブルオブジェクトを設置
		UnityEngine.Object[] assets;
		string assetName = SystemSetting.GetAssetBundleDataFolder () + "AssetBundleDataAsset";
		assets = Resources.LoadAll (assetName);

		AssetBundleData tmp = new AssetBundleData();
		ScriptableObject sObject = null;

		foreach (UnityEngine.Object asset in assets) {
			if (asset is AssetBundleData) {
				tmp = (AssetBundleData)asset;
				sObject = (ScriptableObject)asset;
			}
		}
		List<AssetBundleData.Param> listData = GetListDataFromCSV(_type);
		tmp.list = listData;

		//最後に保存して変更を確定
		EditorUtility.SetDirty (sObject);
	}



	public static List<AssetBundleData.Param> GetListDataFromCSV(Type _dataType){


		string filePath = SystemSetting.GetAssetBundleDataFolder ()+_dataType.Name+"";
		//string filePath = SystemSetting.GetResourcesCSVFilePath() + _dataType.Name + "CSV";


		TextAsset csv = (TextAsset)Resources.Load(filePath, typeof(TextAsset)) as TextAsset;

		FieldInfo[] fieldInfoList = CSMaker.GetFieldInfo (typeof(AssetBundleData.Param));
		List<string> memberNameList = CSMaker.GetMemberList ( typeof(AssetBundleData.Param));
		List<AssetBundleData.Param> listData = new List<AssetBundleData.Param>();

        StringReader reader = new StringReader(csv.text);

		Type curType = typeof(AssetBundleData.Param);


		Debug.LogWarning("DataSet :" + csv);

		int rowNum = -1;

        while (reader.Peek() > -1) {

			AssetBundleData.Param lineData = new AssetBundleData.Param();
			int columnCount = 0;

            string line = reader.ReadLine();
            string[] values = line.Split(',');

			rowNum++;
			//0行はタイトルなので飛ばす
			if(rowNum == 0){ continue;}

			foreach (string item in values) {
				string columnType = memberNameList[columnCount];
				FieldInfo memberFieldInfo = fieldInfoList[columnCount];

				Debug.LogWarning("columnType :" + columnType.ToString ());
				Debug.LogWarning("Field :" + memberFieldInfo.FieldType.ToString ());

				if (memberFieldInfo.FieldType.ToString () == "System.Int32") {
					int setNumber = int.Parse(item);
					curType.InvokeMember(columnType,BindingFlags.SetField,null,lineData,new object[] {setNumber});
				} else if (memberFieldInfo.FieldType.ToString () == "System.Single") {
					float setNumber = float.Parse(item);
					curType.InvokeMember(columnType,BindingFlags.SetField,null,lineData,new object[] {setNumber});
				} else if (memberFieldInfo.FieldType.ToString () == "System.String") {
					string setString = item.Substring(1, item.Length-2); 
					curType.InvokeMember(columnType,BindingFlags.SetField,null,lineData,new object[] {setString});
				} else if (memberFieldInfo.FieldType.ToString () == "System.Boolean") {
					if(item == "1"){
						curType.InvokeMember(columnType,BindingFlags.SetField,null,lineData,new object[] {true});
					}else{
						curType.InvokeMember(columnType,BindingFlags.SetField,null,lineData,new object[] {false});
					}


				} else {
					Debug.LogError ("データ定義に異常があります!!!!!!!" + memberFieldInfo.FieldType);
											//異常なデータは出力してはいけないので強制停止
					break;
				}

				columnCount++;
			}
			listData.Add(lineData);
        }
        return listData;
	}


}
