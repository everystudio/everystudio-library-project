using UnityEngine;
using System.Collections;

public class UtilAssetBundleAudio : UtilAssetBundleBase {

	public AudioClip m_AudioClip;

	public override void afterLoaded( AssetBundle _assetBundle , string _strAssetName ){

		if (_strAssetName == "") {
			m_goLoadObject = (GameObject)Instantiate (_assetBundle.mainAsset);
		} else {

			m_AudioClip = Instantiate (_assetBundle.LoadAsset (_strAssetName, typeof(AudioClip))) as AudioClip;

			foreach( CsvAudioData data in DataManager.master_audio_list ){
			//foreach (MasterLoadAudio.Data data in DataContainer.Instance.MasterLoadAudioList) {
				if (data.filename.Equals (_strAssetName) == true) {

					//Debug.Log ("insert " + _strAssetName);

					AudioClipData dataAudio = new AudioClipData( (AUDIO_TYPE)data.audio_type , data.path , data.filename , m_AudioClip );
					SoundManager.Instance.Add (dataAudio);
				}
			}


		}


	}


}
