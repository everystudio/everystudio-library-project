using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;


[CustomEditor (typeof(AssetBundleTool))]
public class AssetBundleToolEditor : Editor
{
	public List<GameObject> m_goAtlasList = new List<GameObject>();

	string[] buildAtlasPathArray = {
		/*
		"SystemAtlas",
		"Atlas/Battle",
		"Atlas/Card",
		"Atlas/CardIcon",
		"Atlas/Collection",
		"Atlas/CollectionCardIcon",
		"Atlas/CollectionIcon",
		"Atlas/Friend",
		"Atlas/Gacha",
		"Atlas/Icon",
		"Atlas/MonCard",
		"Atlas/Numbers",
		"Atlas/Pusher",
		"Atlas/Shop",
		"Atlas/SkillIcon",
		"Atlas/SymbolAtlas",
		"Atlas/Tips",
		"Atlas/UIAtlas",
		"Atlas/World",
		*/

		/*		
        ,
        "MonsterIconAtlas",
        "MonsterAtlas",
        "EventAtlas"*/};

	string[] buildAtlasNameArray = {
		/*
		"BattlePrefab",
		"Card",
		"CardIcon",
		"Collection",
		"CollectionCardIcon",
		"CollectionIcon",
		"Friend",
		"Gacha",
		"Icon",
		"MonCard",
		"Numbers",
		"Pusher",
		"Shop",
		"SkillIcon",
		"SymbolAtlas",
		"Tips",
		"UIAtlas",
		"World",
		*/

		/*		
        ,
        "MonsterIconAtlas",
        "MonsterAtlas",
        "EventAtlas"*/};

    private static readonly string[] buildAudioDataArray = {"MedalGame"};

	public override void OnInspectorGUI ()
	{
		serializedObject.Update ();
		DrawProperties ();
		serializedObject.ApplyModifiedProperties ();
	}

	public string testtest;

	//各要素の描画
	void DrawProperties ()
	{
		//AssetBundleTool debugViewer = target as AssetBundleTool;

		/*
		EditorGUILayout.LabelField ("名前", "test");
		EditorGUILayout.LabelField ("コメント", "test");

		EditorGUILayout.Separator ();
		*/


		EditorGUILayout.Separator();
		/*
		if (GUILayout.Button ("定義ファイル読み取り", GUILayout.Width (250f))) {
			CSMaker.ReadClass();
		}
		*/

		EditorGUILayout.HelpBox ("アセットバンドル照合用のEXCELファイルを生成します.", MessageType.Info, true);

		if (GUILayout.Button ("データチェッカーファイルを生成", GUILayout.Width (250f))) {
			AssetBudleDataChecker.DataSeetMake ();
		}

		EditorGUILayout.Separator();

		EditorGUILayout.HelpBox ("↓オブジェクト初期生成", MessageType.Info, true);

		if (GUILayout.Button ("C#ファイル生成", GUILayout.Width (250f))) {

			CSMaker.CSParser();

			//スクリプタブルオブジェクト生成コードを生成
			CSMaker.CSParserObjectMaker ();

			//Atach生成コードを設置
			CSMaker.MakeAtachHolderFormat();

			//CSV出入力系を生成
			CSMaker.MakeCSVLoader ();

			//EXCEL出入力系を生成
			CSMaker.MakeExcelLoader ();

			Debug.LogWarning ("MakeEnd :");

		}


		EditorGUILayout.Separator();

		if (GUILayout.Button ("スクリプタブルオブジェクト生成", GUILayout.Width (250f))) {

			//スクリプタブルオブジェクト生成
			CSMaker.GenerateScriptableObject();

			//スクリプタブルオブジェクトをアタッチ
			CSMaker.AtachObject();
			Debug.LogWarning ("Holder生成とスクリプタブルオブジェクト設置");
		}


		EditorGUILayout.Separator();
		#if UNITY_IOS
        if (GUILayout.Button ("データのアセットバンドル生成(iOS)", GUILayout.Width (250f))) {
            BuildAssetBundles.StartMakeAssetBundle(BuildTarget.iOS);
		}
		EditorGUILayout.Separator();
		if (GUILayout.Button ("利用不可:データのアセットバンドル生成(Android)", GUILayout.Width (250f))) {
			Debug.LogError( "Please SwitchPlatform for Android" );
		}
		#elif UNITY_ANDROID
		if (GUILayout.Button ("利用不可:データのアセットバンドル生成(iOS)", GUILayout.Width (250f))) {
			Debug.LogError( "Please SwitchPlatform for iOS" );
		}
        EditorGUILayout.Separator();
        if (GUILayout.Button ("データのアセットバンドル生成(Android)", GUILayout.Width (250f))) {
            BuildAssetBundles.StartMakeAssetBundle(BuildTarget.Android);
        }
		#else

		#endif

//		EditorGUILayout.Separator();
//
//		if (GUILayout.Button ("データのアセットバンドル生成2", GUILayout.Width (250f))) {
//			BuildPipeline.BuildAssetBundles ("AssetBundles");
//		}

		EditorGUILayout.Separator();

		if (GUILayout.Button ("Excelひな形生成(上書き注意)", GUILayout.Width (250f))) {
			ExcelParser.MakeDataSeet ();
		}


		if (GUILayout.Button ("CSVひな形生成(上書き注意)", GUILayout.Width (250f))) {
			CSVPerser.MakeCSV ();
		}

		EditorGUILayout.HelpBox ("↓オブジェクト初期生成", MessageType.Info, true);

		EditorGUILayout.Separator();

		if (GUILayout.Button ("EXCEL→CSV変換(上書き注意)", GUILayout.Width (250f))) {
			ExcelLoader.LoadExcelAndMakeCSV();
			Debug.LogWarning ("変換終了 :");
		}

		if (GUILayout.Button ("CSV→EXCEL変換(上書き注意)", GUILayout.Width (250f))) {
			CSVLoader.LoadCSVAndMakeExcel ();
			Debug.LogWarning ("変換終了 :");
		}

		if (GUILayout.Button ("CSV→ScriptableObject変換", GUILayout.Width (250f))) {
			CSMaker.SetSTODataFromCSV();
		}

		EditorGUILayout.Separator();

		EditorGUILayout.HelpBox ("データローダシステム側を更新します", MessageType.Info, true);

		if (GUILayout.Button ("データロードコンテナを更新", GUILayout.Width (250f))) {
			CSMaker.MakeDataContainer();
		}

        EditorGUILayout.Separator();

        EditorGUILayout.HelpBox ("フォントのアセットバンドルを生成します", MessageType.Info, true);

		#if UNITY_IOS
		if (GUILayout.Button ( "BitmapFont(iOS)", GUILayout.Width (250f))) {
			BuildAssetBundles.MakeAssetBundle ("AssetBundle/BitmapFont/","BitmapFontPrefab","BitmapFont/",BuildTarget.iOS);
		}
		if (GUILayout.Button ( "利用不可:BitmapFont(Android)", GUILayout.Width (250f))) {
			Debug.LogError( "Please SwitchPlatform for Android" );
		}

		#elif UNITY_ANDROID
		if (GUILayout.Button ( "利用不可:BitmapFont(iOS)", GUILayout.Width (250f))) {
			Debug.LogError( "Please SwitchPlatform for iOS" );
		}
		if (GUILayout.Button ( "BitmapFont(Android)", GUILayout.Width (250f))) {
			BuildAssetBundles.MakeAssetBundle ("AssetBundle/BitmapFont/","BitmapFontPrefab","BitmapFont/",BuildTarget.Android);
		}
		#endif

		EditorGUILayout.Separator();

		EditorGUILayout.HelpBox ("アトラスのアセットバンドルを生成します", MessageType.Info, true);

		#if UNITY_IOS
		foreach (string buildURL in buildAtlasPathArray) {
			if (GUILayout.Button ( buildURL+"(iOS)", GUILayout.Width (250f))) {
				BuildAssetBundles.BuildStartAtlas(buildURL,BuildTarget.iOS);
			}
		}

		EditorGUILayout.Separator();

		foreach (string filename in buildAtlasNameArray) {
			if (GUILayout.Button ("filename:"+ filename+"(iOS)", GUILayout.Width (250f))) {
				BuildAssetBundles.BuildStartAtlasSingle("Assetbundle/Atlas/" + filename + "/" , filename , "AssetBundles/Atlas/" ,BuildTarget.iOS);
			}
		}

		#elif UNITY_ANDROID
		foreach (string buildURL in buildAtlasPathArray) {
            if (GUILayout.Button ( buildURL+"(Android)", GUILayout.Width (250f))) {
                BuildAssetBundles.BuildStartAtlas(buildURL,BuildTarget.Android);
            }
        }

		EditorGUILayout.Separator();

		foreach (string filename in buildAtlasNameArray) {
			if (GUILayout.Button ("filename:"+ filename+"(Android)", GUILayout.Width (250f))) {
				BuildAssetBundles.BuildStartAtlasSingle("Assetbundle/Atlas/"+filename , filename , "AssetBundles/Atlas/" ,BuildTarget.Android);
			}
		}

		#endif

        EditorGUILayout.Separator();

        EditorGUILayout.HelpBox ("Audioのアセットバンドルを生成します", MessageType.Info, true);

		#if UNITY_IOS

		foreach (string buildURL in buildAudioDataArray) {
			if (GUILayout.Button ( buildURL+"(iOS)", GUILayout.Width (250f))) {
				BuildAssetBundles.BuildStartAudio(buildURL,BuildTarget.iOS);
			}
		}


		#elif UNITY_ANDROID

        foreach (string buildURL in buildAudioDataArray) {
            if (GUILayout.Button ( buildURL+"(Android)", GUILayout.Width (250f))) {
                BuildAssetBundles.BuildStartAudio(buildURL,BuildTarget.Android);
            }
        }
		#endif

        EditorGUILayout.Separator();

		EditorGUILayout.HelpBox ("管理ファイルの生成を行います", MessageType.Info, true);

		if (GUILayout.Button ("管理ファイルを更新", GUILayout.Width (250f))) {
			CSVPerser.MakeAssetBundleList ();
		}

		#if UNITY_IOS
        if (GUILayout.Button ("管理ファイルのアセットバンドル生成(iOS)", GUILayout.Width (250f))) {
            BuildAssetBundles.StartMakeAssetBundleList(BuildTarget.iOS);
		}
		#elif UNITY_ANDROID

        if (GUILayout.Button ("管理ファイルのアセットバンドル生成(Android)", GUILayout.Width (250f))) {
            BuildAssetBundles.StartMakeAssetBundleList(BuildTarget.Android);
        }
		#endif

		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		EditorGUILayout.HelpBox ("更新クラス。チェックをはずすとデータ生成の影響外になります。アセットバンドル生成だけ対象外", MessageType.Info, true);

        CSMaker.ReadMyToggle(); // 更新
        IList<string> list = new List<string>(CSMaker.dataSeetMyToggleDictionary.Keys);
        foreach (string str in list)
        {
            if (CSMaker.dataSeetMyToggleDictionary[str].Validate)
            {
                bool _flag = CSMaker.dataSeetMyToggleDictionary[str].Enable;
                CSMaker.dataSeetMyToggleDictionary[str].Enable = EditorGUILayout.Toggle(str, _flag);
            }
        }

        /*
        foreach (string str in list)
        {
            CSMaker.MyToggle toggle = CSMaker.dataSeetMyToggleDictionary[str];

            // 有効であれば表示
            if (toggle.b_validate)
            {
                toggle.b_enable = EditorGUILayout.Toggle(str, toggle.b_enable);
                CSMaker.dataSeetMyToggleDictionary[str].b_enable = toggle.b_enable;
            }
        }
        */

		EditorGUILayout.Separator();
		EditorGUILayout.Separator();

		EditorGUILayout.HelpBox ("自動生成したデータやC#ファイルを全部削除します。取り扱いに注意!!!", MessageType.Info, true);

		/*
		if (GUILayout.Button ("全データリセット。取り扱い注意!!", GUILayout.Width (250f))) {
			//BuildAssetBundles.StartMakeAssetBundleList();
		}
		*/

	}


}
