using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(UtilAssetBundleAudio))]
public class SoundPlayerSupport : MonoBehaviourEx {

	public enum STEP{
		IDLE	= 0 ,
		LOAD  	,
		MAX 	,
	}
	public STEP m_eStep;
	protected STEP m_eStepPre;

	public UtilAssetBundleAudio m_csAssetBundleAudio;
	//public MasterLoadAudio.Data m_tempLoadAudioData = new MasterLoadAudio.Data();


	public Queue< AudioChannelData> m_tAudioChannelQueue = new Queue<AudioChannelData> ();

	//public Queue<string > m_strReserveQueue = new Queue<string > ();
	public AudioChannelData m_tChannnelData;

	/*
	 * 元
	public void recievePlaySound( AudioChannelData _channnel ){
		if( DataContainer.Instance.MasterLoadAudioList != null ){
			foreach (MasterLoadAudio.Data data in DataContainer.Instance.MasterLoadAudioList) {
				Debug.Log (data.filename + " : " + _channnel.m_strFilename);
				if (data.filename.Equals (_channnel.m_strFilename)) {

					m_tAudioChannelQueue.Enqueue (_channnel);
					return;
				}
			}
		}
		return;
	}
	*/

	public void recievePlaySound( AudioChannelData _channnel ){

		foreach( CsvAudioData data in DataManager.master_audio_list ){
			//Debug.Log (data.filename + " : " + _channnel.m_strFilename);
			if (data.filename.Equals (_channnel.m_strFilename)) {
				m_tAudioChannelQueue.Enqueue (_channnel);
				return;
			}
		}
		return;
	}
	public AudioChannelData PlaySound( AudioChannelData _channnel ){
		recievePlaySound (_channnel);
		return _channnel;
	}


	/*

	// なぜ分けたのか？
	public int PlaySe( string _strFilename ){
		_strFilename = _strFilename.ToLower ();
		return recievePlaySound(_strFilename);
	}
	public int PlayBgm( string _strFilename ){
		_strFilename = _strFilename.ToLower ();
		Debug.Log (_strFilename);
		return recievePlaySound(_strFilename);
	}
	*/

	// Update is called once per frame
	void Update () {

		bool bInit = false;
		if (m_eStepPre != m_eStep) {
			m_eStepPre  = m_eStep;
			bInit = true;
		}
		switch (m_eStep) {
		case STEP.IDLE:
			if (0 < m_tAudioChannelQueue.Count) {
				m_eStep = STEP.LOAD;
			}
			break;
		case STEP.LOAD:
			if (bInit) {
				m_tChannnelData = m_tAudioChannelQueue.Dequeue ();

				if (m_csAssetBundleAudio == null) {
					m_csAssetBundleAudio = GetComponent<UtilAssetBundleAudio> ();
				}

				foreach( CsvAudioData data in DataManager.master_audio_list ){
					if (data.filename.Equals (m_tChannnelData.m_strFilename)) {
						/*
						m_tempLoadAudioData.filename = data.filename;
						m_tempLoadAudioData.audio_type = data.audio_type;
						m_tempLoadAudioData.path = data.path;
						m_tempLoadAudioData.version = data.version;
						*/

						EditPlayerSettingsData epsData = ConfigManager.instance.GetEditPlayerSettingsData ();

						//Debug.Log (epsData.m_strS3Url);
						//Debug.Log (data.path);

						string resultUrl = epsData.m_strS3Url + "/" + data.path+"/" +  data.filename + ".unity3d";

						//Debug.LogError (resultUrl);
						m_csAssetBundleAudio.Load (data.filename, resultUrl, 1);
					}
				}
			}
			if (m_csAssetBundleAudio.IsLoaded ()) {

				//AudioClipData addData = new AudioClipData( m_tempLoadAudioData.audio_type , m_tempLoadAudioData.path , m_tempLoadAudioData.filename , m_cs
				//SoundManager.Instance.Add (m_tempAudioClipData);
				//SoundManager.Instance.PlaySE (m_strLoaingFile);

				SoundManager.Instance.PlaySound (m_tChannnelData);
				/*

				if (m_tempLoadAudioData.audio_type == (int)AUDIO_TYPE.SE) {
					SoundManager.Instance.PlaySE (m_strLoaingFile);
				} else if (m_tempLoadAudioData.audio_type == (int)AUDIO_TYPE.VOICE) {
					SoundManager.Instance.PlayVoice (m_strLoaingFile);
				}else if( m_tempLoadAudioData.audio_type == (int)AUDIO_TYPE.BGM ){
					SoundManager.Instance.PlayBGM (m_strLoaingFile);
				} else {
					Debug.Log ("unknown type");
				}
				*/
				m_eStep = STEP.IDLE;
			}
			break;
		case STEP.MAX:
		default:
			break;
		}
	
	}
}
