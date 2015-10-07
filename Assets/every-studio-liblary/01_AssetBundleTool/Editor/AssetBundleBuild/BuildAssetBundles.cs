using System;
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;


public class BuildAssetBundles : EditorWindow {


    public static void StartMakeAssetBundleList (BuildTarget target = BuildTarget.iOS)
	{
		Debug.LogWarning("MakeAssetBundleList :");
        BuildStartDataList(target);
		AssetDatabase.Refresh( ImportAssetOptions.ImportRecursive);
	}

    public static void StartMakeAssetBundle(BuildTarget target = BuildTarget.iOS){
		Debug.LogWarning("MakeAssetBundle :");
		CSMaker.ReadClass();
		List<Type> addList = CSMaker.ReadClass ();

		foreach (Type item in addList) {
            BuildStartGameData (item.Name + "Prefab",target);
		}

		AssetDatabase.Refresh( ImportAssetOptions.ImportRecursive);
	}

	/// <summary>
	/// Makes the asset bundle.
	/// </summary>
	/// <param name="_strpath">データパス.</param>
	/// <param name="_strDataName">データ名.</param>
	/// <param name="_strBuildUrl">データ保存パス.</param>
    public static void MakeAssetBundle(string _strpath,  string _strDataName , string _strBuildUrl ,BuildTarget target){
		UnityEngine.Object dataObject;

		if(_strpath == null){
            Debug.LogWarning("############ LoadPath :" + _strDataName);
            dataObject = Resources.Load( _strDataName , typeof(GameObject));
		}
		else
		{
            Debug.LogWarning("############ LoadPath :" + _strpath+_strDataName);
			dataObject = Resources.Load( string.Concat(_strpath , _strDataName) , typeof(GameObject));
		}
		Debug.Log (dataObject);
		string basename = dataObject.name;
		string path = Application.streamingAssetsPath;
		path = Application.dataPath + "/../" + Define.ASSET_BUNDLES_ROOT ;

		Debug.Log (path);

		UnityEngine.Object[] selection = new UnityEngine.Object[1];
		selection[0] = dataObject;

		BuildPipeline.BuildAssetBundle(Selection.activeObject,
            selection, path +_strBuildUrl + basename +".unity3d",
			BuildAssetBundleOptions.CollectDependencies |
			BuildAssetBundleOptions.CompleteAssets,
            target);

		return;
	}

    static void BuildStartGameData (string _dataClassName,BuildTarget target){
		string dataName = SystemSetting.GetResourcesAssetSrcPath () + _dataClassName;
		string buildURL = SystemSetting.GetAssetBundlePath();

        MakeAssetBundle (null, dataName, buildURL,target);
	}

    static void BuildStartDataList (BuildTarget target){
		string dataName = SystemSetting.GetAdminAssetPathPrefab ();
		string buildURL = SystemSetting.GetAdminAssetListPath () ;

        MakeAssetBundle (null, dataName, buildURL,target);
	}

	public static void BuildStartAtlasSingle (string _strBuildPath , string _strFileName , string _strOutputPath , BuildTarget target ){
		UnityEngine.Object loadObject = Resources.Load (_strBuildPath + _strFileName );
		//UnityEngine.Object[] selection = Selection.GetFiltered (typeof(UnityEngine.Object), SelectionMode.DeepAssets);
		UnityEngine.Object[] selection = new UnityEngine.Object[1];
		selection[0] = loadObject;

		//string path = Application.dataPath + "/" + _strOutputPath;
		string path = Application.dataPath + "/../" + _strOutputPath;

		Debug.Log (path);
		Debug.Log (_strOutputPath);

		// for Android
		BuildPipeline.BuildAssetBundle(Selection.activeObject,
			selection,
			path  + _strFileName +".unity3d",
			BuildAssetBundleOptions.CollectDependencies |
			BuildAssetBundleOptions.CompleteAssets,
			target);

	}


    public static void BuildStartAtlas (string buildURL,BuildTarget target = BuildTarget.iOS)
    {
        UnityEngine.Object[] gameObjectArray = Resources.LoadAll("AssetBundle/" + buildURL,typeof(GameObject));
        foreach (var item in gameObjectArray) {
            Debug.LogWarning("MakeObjectName :" + item.name);
        }
        BuildBase (gameObjectArray,buildURL,target);
        AssetDatabase.Refresh( ImportAssetOptions.ImportRecursive);
    }


    static void BuildBase (UnityEngine.Object[] gameObjectArray,string buildURL,BuildTarget target)
    {
        int buildCount = 0;

        foreach (var item in gameObjectArray) {
            FileExport(item,buildURL,target);
            buildCount++;
        }
    }

    static void FileExport(UnityEngine.Object obj,string buildURL,BuildTarget target) {
        string basename = obj.name;

        string path = Application.streamingAssetsPath;
		//path = Application.dataPath + "AssetBundles/" + DEFINE.ASSET_BUNDLES_ROOT + "/assets/assetbundleresource";

        //TODO 複数読みがあるかもしれないので配列を残しておく
        UnityEngine.Object[] selection = new UnityEngine.Object[1];
        selection[0] = obj;

        //Debug.LogWarning("buildURL :" + path + "/" + buildURL + "/" + basename + ".iphone.unity3d");

        BuildPipeline.BuildAssetBundle(Selection.activeObject,
            selection, path + "/" + buildURL + "/" + basename +".unity3d",
            BuildAssetBundleOptions.CollectDependencies |
            BuildAssetBundleOptions.CompleteAssets,
            target);
    }

    public static void BuildStartAudio (string buildItem,BuildTarget target = BuildTarget.iOS){

        string dataname = "AudioDataHolder_" + buildItem;

        string dataPath = "AudioResources/" + dataname + "Prefab";

        Debug.Log ("dataPath="+dataPath);

        UnityEngine.Object dataObject = Resources.Load( dataPath,typeof(GameObject));

        string path = Application.streamingAssetsPath;
		//path = Application.dataPath + "AssetBundles/" + DEFINE.ASSET_BUNDLES_ROOT + "/assets/assetbundleresource";
		Debug.Log (path);

        string buildURL = "AudioDataAsset";

        UnityEngine.Object[] selection = new UnityEngine.Object[1];
        selection[0] = dataObject;

        BuildPipeline.BuildAssetBundle(Selection.activeObject,
            selection, path + "/" + buildURL + "/" + dataname + "Prefab"+".unity3d",
            BuildAssetBundleOptions.CollectDependencies |
            BuildAssetBundleOptions.CompleteAssets,
            target);

        AssetDatabase.Refresh( ImportAssetOptions.ImportRecursive);
    }
}
