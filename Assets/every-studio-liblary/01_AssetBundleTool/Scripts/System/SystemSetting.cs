using UnityEngine;
using System.Collections;


public static class SystemSetting
{
	//各生成データの格納フォルダ
	static string rootPath = "Assets/00_AssetBundleData/Resources/";
	static string excelSeetPath = "Excel/";
	static string csvFilePath = "CSV/";
	static string scriptablePath = "Scriptable/";
	static string assetSrcPath = "assetSrcPrefab/";
	static string assetBundlePath = "Data/";
	static string assetBundlePathPlane = "Data";

	//パースしたC#プログラムのソース配置フォルダ
	static string templateFolder = "Template/";
	static string csFileSrcPath = "Assets/01_AssetBundleTool/Scripts/DataDefine/";
	static string csEditorFileSrcPath = "Assets/01_AssetBundleTool/Scripts/DataDefine/Editor/";
	static string containerFilePath = "Assets/01_AssetBundleTool/Scripts/DataContainer/";

	// アセットバンドル管理ファイルのパス.
	static string AdminExcelpath = "Assets/00_AssetBundleData/Resources/Admin/AssetBundleData.xls";
	static string AdminAssetPath = "Assets/00_AssetBundleData/Resources/Admin/AssetBundleDataAsset.asset";

	//Resources基点でのアセットバンドル管理ファイルフォルダのパス
	static string AssetBundleDataFolder = "Admin/";

	//アセットバンドルリストのパス
	static string AssetBundleListPath = "Assets/00_AssetBundleData/Resources/Admin/AssetBundleData";

	//アセットバンドルリストのアセットバンドルの配置パス
	static string AdminAssetPathPrefab = "Admin/AssetBundleDataPrefab";
	static string AdminAssetListPath = "AssetbundleList/";
	static string DataListName = "AssetBundleDataPrefab";

	//アセットバンドル配置パス 
    #if UNITY_EDITOR
    static string StreamingAssetspath = "file://"+Application.dataPath + "/StreamingAssets/";
    #elif UNITY_IPHONE
 	static string StreamingAssetspath = "file://" + Application.streamingAssetsPath + "/";

    #elif UNITY_ANDROID
    static string StreamingAssetspath = "jar:file://" + Application.dataPath + "!/assets/";
    #else
    static string StreamingAssetspath = "file://"+Application.dataPath + "/StreamingAssets/";
    #endif

	//アセットバージョンチェックファイル名
	static string BundleVerName = "AssetBundleVer.xls";

	//アセットバージョンチェックファイルの出力URL
	static string BundleVerUrl = "Assets/01_AssetBundleTool/Scripts/System/";

    //都度ダウンロード用のPATH
    static string AssetBundlesBasePath = "assets/assetbundleresource/";

    static string AssetBundlesSpritePath = AssetBundlesBasePath+"sprite/";

    static string AssetBundlesAtlasPath = AssetBundlesBasePath+"atlas/";

    static string AssetBundlesSoundPath = AssetBundlesBasePath+"sound/";

    static string AssetBundlesModelPath = AssetBundlesBasePath+"model/";

	//Excelの初期生成レコード数
	static int initExcelGenerateCell = 10;

	public static string GetAssetBundleDataFolder ()
	{
		return AssetBundleDataFolder;
	}

	public static string GetAssetBundleListPath ()
	{
		return AssetBundleListPath;
	}

	public static string GetContainerFilePath ()
	{
		return containerFilePath;
	}

	public static string GetDataListName ()
	{
		return DataListName;
	}


	public static string GetAdminAssetPathPrefab ()
	{
		return AdminAssetPathPrefab;
	}


	public static string GetAdminAssetListPath ()
	{
		return AdminAssetListPath;
	}


	public static string GetBundleVerName ()
	{
		return BundleVerName;
	}

	public static string GetBundleVerUrl ()
	{
		return BundleVerUrl;
	}


	public static string GetStreamingAssetspath ()
	{
		return StreamingAssetspath;
	}


	public static string GetAdminExcelPath ()
	{
		return AdminExcelpath;
	}

	public static string GetAdminAssetPath ()
	{
		return AdminAssetPath;
	}


	public static string GetEditorFileSrcPath ()
	{
		return csEditorFileSrcPath;
	}


	public static string GetCSFileSrcPath ()
	{
		return csFileSrcPath;
	}

	public static string GetTemplateFolder ()
	{
		return templateFolder;
	}






	public static string GetExcelSeetPath ()
	{
		return rootPath + excelSeetPath;
	}

	public static string GetCSVFilePath ()
	{
		return rootPath + csvFilePath;
	}




	public static string GetScriptableobjectPath ()
	{
		return rootPath + scriptablePath;
	}

	public static string GetAssetSrcPath ()
	{
		return rootPath + assetSrcPath;
	}


	//Resourcesからのパス
	public static string GetResourcesCSVFilePath ()
	{
		return csvFilePath;
	}

	//Resourcesからのパス
	public static string GetResourcesAssetSrcPath ()
	{
		return assetSrcPath;
	}

	//Resourcesからのパス
	public static string GetResourcesLoadPath ()
	{
		return scriptablePath;
	}

	//Resourcesからのパス
	public static string GetAssetBundlePath ()
	{
		return assetBundlePath;
	}

	public static string GetassetBundlePathPlane ()
	{
		return assetBundlePathPlane;
	}


	public static int GetInitGenerateCell(){
		return initExcelGenerateCell;
	}

    public static string GetAssetBundlesBasePath(){
        return AssetBundlesBasePath;
    }

    public static string GetAssetBundlesSpritePath(){
        return AssetBundlesSpritePath;
    }

    public static string GetAssetBundlesAtlasPath(){
        return AssetBundlesAtlasPath;
    }

    public static string GetAssetBundlesSoundPath(){
        return AssetBundlesSoundPath;
    }

    public static string GetAssetBundlesModelPath(){
        return AssetBundlesModelPath;
    }

}
