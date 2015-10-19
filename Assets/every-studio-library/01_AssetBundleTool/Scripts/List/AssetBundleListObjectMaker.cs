#if UNITY_EDITOR
using System;
using System.Text;
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//public class AssetBundleListObjectMaker : EditorWindow
public class AssetBundleListObjectMaker : ScriptableWizard
{
	static string m_strLoadURL = "Assets/01_AssetBundleTool/";
	static string source_path = "TestTestAAA.asset";

	/*

	[MenuItem ("Tools/DataMaker/Test")]
	static void Open ()
	{
		DisplayWizard<AssetBundleListObjectMaker> ("test");
	}

	void OnWizardUpdate ()
	{
		//isValid = !string.IsNullOrEmpty(asssetBundleLoadURL);
	}


	void OnWizardCreate ()
	{
		PrizeOfferSettingData loadSettingData = CreateInstance<PrizeOfferSettingData> ();

		loadSettingData.loadURL = "Test";

		//Directory.CreateDirectory("Assets/AssetExcelDataResources/Resources");
		//string path = "Assets/AssetExcelDataResources/Resources/PrizeOfferSettingData.asset";

		//Directory.CreateDirectory("Assets/AssetExcelDataResources/Resources");
		string path = m_strLoadURL + source_path;


		AssetDatabase.CreateAsset (loadSettingData, path);
		AssetDatabase.ImportAsset (path);
		EditorUtility.UnloadUnusedAssets ();
	}
	*/

	public static void MakeObject ()
	{
		Debug.LogWarning("MakeObject :");
		AssetBundleList assetBundleListData = CreateInstance<AssetBundleList> ();

		string path = m_strLoadURL + source_path;//
		
		AssetDatabase.CreateAsset ( assetBundleListData, path);
		AssetDatabase.ImportAsset (path);
		EditorUtility.UnloadUnusedAssets ();

		AssetDatabase.Refresh (ImportAssetOptions.ImportRecursive);

	}


	public static void LoadData ()
	{


		Debug.LogWarning("LoadData :");
		//LoadExcel.TestCode ();


	}


}
#endif