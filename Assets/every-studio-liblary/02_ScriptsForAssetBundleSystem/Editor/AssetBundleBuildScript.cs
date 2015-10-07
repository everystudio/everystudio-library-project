using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class AssetBundleBuildScript
{
    const string kAssetBundlesOutputPath = "AssetBundles";

    // アセットバンドル化するフォルダの設置場所
    private static string assetTopDir = "Assets/Data/";     // ※大文字小文字は区別される

//    [MenuItem( "AssetBundles/Build AssetBundles" )]
    public static void BuildAssetBundles()
    {
        // Choose the output path according to the build target.
        string outputPath = Path .Combine(kAssetBundlesOutputPath, "");
        if (!Directory .Exists(outputPath))
            Directory.CreateDirectory(outputPath);

        BuildPipeline.BuildAssetBundles(outputPath, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
    }

    private static void SetAssetName(Object obj)
    {
        var path = AssetDatabase.GetAssetPath(obj);
        // AssetImporterも取得
        AssetImporter importer = AssetImporter.GetAtPath(path);

        if (path.IndexOf("Resources/" ) >= 0)
            return;

        string abname = path.Replace(assetTopDir, "");
        int idx = abname.LastIndexOf('.' );
        if (idx != -1)
        {
            abname = abname.Substring(0, idx) + ".unity3d";
        }
        else
        {
            abname = path;
        }
        importer.assetBundleName = abname;
        importer.assetBundleVariant = "";

    }

    [MenuItem( "AssetBundles/Set Assetbundle name" )]
    public static void SelectionAsset()
    {
        // Build the resource file from the active selection.
        Object[] selection = Selection.GetFiltered(typeof( Object), SelectionMode .DeepAssets);

        foreach (var obj in selection)
        {
            SetAssetName(obj);
        }

    }
}
