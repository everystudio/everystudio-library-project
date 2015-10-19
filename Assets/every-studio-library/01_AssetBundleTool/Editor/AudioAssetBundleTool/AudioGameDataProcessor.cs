using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;


public class AudioGameDataProcessor : AssetPostprocessor
{

	//存在するAudioデータシートをここで定義
    private static readonly string[] audioDataList = {"MedalGame"};//増設の余地を作っておく

	//Audioデータの入っているフォルダ
    private static readonly string dataFolder = "Assets/00_AssetBundleData/Resources/AudioResources/";

	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			
			bool isSkip = false;
			string curName = "";

			//変更したシートにAudioシートがあるかチェッック
			foreach (string itemName in audioDataList) {
				string dataPath = dataFolder + "AudioSheet_" +  itemName + ".xls";
				if (dataPath == asset) {
					curName = itemName;
					isSkip = true;
				}
			}

			if (!isSkip) {
				continue;
			}

			string filePathSub = dataFolder + "AudioSheet_" + curName + ".xls";


			string exportPathSub = dataFolder + "Audio_" + curName + ".asset";

			//Debug.LogWarning ("Path :" + exportPathSub);


			AudioSettingData dataSub = (AudioSettingData)AssetDatabase.LoadAssetAtPath (exportPathSub, typeof(AudioSettingData));
			
			if (dataSub == null) {
				//data = ScriptableObject.CreateInstance<AudioSettingData> ();

				dataSub = ScriptableObject.CreateInstance<AudioSettingData> ();
				AssetDatabase.CreateAsset ((ScriptableObject)dataSub, exportPathSub);
				//dataSub.hideFlags = HideFlags.NotEditable;
			}

			dataSub.audioData = new AudioSettingData.BGMSoundList();
			dataSub.audioSrcData = new AudioSettingData.BGMSoundSrcList();

			using (FileStream stream = File.Open (filePathSub, FileMode.Open, FileAccess.Read)) {
				IWorkbook book = new HSSFWorkbook (stream);
					
				ISheet sheet = book.GetSheet ("BGM");
					
				////Debug.Log (sheet.SheetName);
					
				for (int i = 1; i < sheet.LastRowNum; i++) {
						
					IRow row = sheet.GetRow (i);
						
					AudioSettingData.AudioParam p = new AudioSettingData.AudioParam ();
						
                    p.index = (int)row.GetCell (0).NumericCellValue;
					p.soundID = (int)row.GetCell (1).NumericCellValue;
					p.audioName = (string)row.GetCell (2).StringCellValue;

					dataSub.audioData.bgmParamList.Add (p);
				}
			}

			using (FileStream stream = File.Open (filePathSub, FileMode.Open, FileAccess.Read)) {
				IWorkbook book = new HSSFWorkbook (stream);
					
				ISheet sheet = book.GetSheet ("SOUND");
					
				////Debug.Log (sheet.SheetName);
					
				for (int i = 1; i < sheet.LastRowNum; i++) {
						
					IRow row = sheet.GetRow (i);
						
					AudioSettingData.AudioParam p = new AudioSettingData.AudioParam ();
						
					p.index 		= (int)row.GetCell (0).NumericCellValue;
					p.soundID = (int)row.GetCell (1).NumericCellValue;
					p.audioName = (string)row.GetCell (2).StringCellValue;

					dataSub.audioData.soundParamList.Add (p);
				}
			}

			//オーディオファイルのセット
			foreach (AudioSettingData.AudioParam item in dataSub.audioData.bgmParamList) {

                string dataname = "AudioResources/" + curName + "/" + item.audioName;

//				Debug.LogWarning ("audioClip NameSrc :" + dataname);

				AudioClip audioClip = (AudioClip)Resources.Load (dataname, typeof(AudioClip));

				AudioSettingData.AudioSrcParam audioSrcParam = new AudioSettingData.AudioSrcParam ();
					 
				audioSrcParam.audioClip = audioClip;

//				Debug.LogWarning ("Add AudioClip Name :" + audioSrcParam.audioClip.name);

				dataSub.audioSrcData.bgmSrcList.Add (audioSrcParam);
			}

			foreach (AudioSettingData.AudioParam item in dataSub.audioData.soundParamList) {

                string dataname = "AudioResources/" + curName + "/" + item.audioName;

//				Debug.LogWarning ("audioClip NameSrc :" + dataname);

				AudioClip audioClip = (AudioClip)Resources.Load (dataname, typeof(AudioClip));

				AudioSettingData.AudioSrcParam audioSrcParam = new AudioSettingData.AudioSrcParam ();
					 
				audioSrcParam.audioClip = audioClip;

//				Debug.LogWarning ("Add AudioClip Name :" + audioSrcParam.audioClip.name);

				dataSub.audioSrcData.soundSrcList.Add (audioSrcParam);
			}



			ScriptableObject subobj = AssetDatabase.LoadAssetAtPath (exportPathSub, typeof(ScriptableObject)) as ScriptableObject;
			EditorUtility.SetDirty (subobj);

		}
	}

	
	static bool BoolSetting (int _val)
	{
		if (_val == 0) {
			return false;
		}
		return  true;
	}
	
	/*
	static MiniGameA.Enemy.MoveType MoveTypeSetting(string type){
		
		MiniGameA.Enemy.MoveType returnType = (MiniGameA.Enemy.MoveType)System.Enum.Parse(typeof(MiniGameA.Enemy.MoveType), type);
		
		return  returnType;
	}
	*/



}
