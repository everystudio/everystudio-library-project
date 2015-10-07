using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public enum AUDIO_TYPE {
	NONE	= 0,
	BGM		,
	SE		,
	VOICE	,
	JINGLE	,
	MAX		,
}

[System.Serializable]
public class SOUND {
	public enum BGM : int {
		MAIN 			= 0,		//メダルゲーム画面のBGM
		REACH 			,			//スロットリーチ時のBGM
		BATTLE 			,			//バトル時のBGM
		RADAR	 		,			//１次抽選時のBGM
		JACKPOT 		,			//２次抽選時のBGM

		VS_BATTLE		,			// 対戦中のBGM
        BATTLE_RESULT   ,           // 対戦結果表示中のBGM

		GOLD_RUSH		,
	}


	public enum SE : int {
		BUTTON_PUSH			= 0,	//決定音　※パズキンサウンド
		BUTTON_BACK 		,		//キャンセル音　※パズキンサウンド
		MEDAL_FALL			,		//メダルが落下した際の効果音
		MEDAL_CHECKERIN 	,		//メダルがチェッカーに入った際の効果音
		SLOT_ROTATION	 	,		//スロットが回転中の効果音
		SLOT_STOP 			,		//スロットの回転が止まった時の効果音
		SLOT_WIN 			,		//スロットで出目が揃った、及び、フィールドWIN時の効果音
		BATTLE_ENCOUNT 		,		//バトル開始前、敵が出てくるときの効果音
		RADAR_START 		,		//１次抽選開始時の効果音
		JP_CHALLENGE 		,		//１次抽選で２次抽選が確定した時の効果音
		JP_GET 				,		//２次抽選でジャックポットをゲットした際の効果音
		JP_POCKETIN 		,		//抽選機のポケットにボールが入る際の効果音

		SKILL_START			,		//カードの『スキル発動』エフェクトの効果音			
		SKILL_BOMB			,		//スキル『ボム』の爆弾が爆発した時の効果音			
		SKILL_FIREBALL		,		//スキル『火の玉』が発動した時の効果音			
		SKILL_MEDAL_TOWER	,		//スキル『メダルタワー』が発動した時の効果音			
		SKILL_THUNDER		,		//スキル『雷』が発動した時の効果音			
		SKILL_TORNADE		,		//スキル『竜巻』が発動した時の効果音			
		SKILL_KOBITO_APPEAR	,		//スキル『チビキャラ操作』でチビキャラが登場した時の効果音			
		SKILL_WAVE			,		//スキル『ウェイブ』が発動した時の効果音			
		MEDAL_BOUND			,		//小人でメダル弾いた時の音

		MEDAL_IN_SELF		,
		MEDAL_IN_OTHER		,

		MAX 				,
	}

	// TODO:Voiceのファイルが決まり次第書き換えてください（テスト用名称）.
	public enum VOICE : int{
		NO_NAME_0 = 0,
		NO_NAME_1,
		NO_NAME_2,
		NO_NAME_3,
	}
}

public class AudioClipData {
	public AUDIO_TYPE m_eAudioType;
	public string m_strPath;
	public string m_strName;
	public AudioClip m_AudioClip;
	public AudioClipData(){
	}
	public AudioClipData (AUDIO_TYPE _eAudioType, string _strPath , string _strName, AudioClip _audioClip){
		m_eAudioType = _eAudioType;
		m_strPath = _strPath;
		m_strName = _strName;
		m_AudioClip = _audioClip;
		return;
	}
}

public class AudioChannelData {
	public int iChannelIndex;
	public enum STATUS {
		NONE		= 0,
		IDLE		,		// 待機状態
		REQUEST		,		// 受付中
		WAITING		,		// 読み込み or delay待ち
		PLAYING		,		// 再生中
		MAX			,
	}
	public STATUS m_eStatus;
	public bool m_bLoop;
	public bool m_bForce;
	public AudioSource m_tAudioSource;
	public string m_strFilename;

	public AUDIO_TYPE m_eAudioType{
		get{ return m_tAudioClipData.m_eAudioType; }
		set{ m_tAudioClipData.m_eAudioType = value; }
	}
	public AudioClipData m_tAudioClipData;

	public void Play( AudioClipData _clipData ){

		//Debug.Log ("sound play!!!!");
		m_tAudioSource.clip = _clipData.m_AudioClip;
		m_tAudioSource.loop = m_bLoop;
		m_eStatus = STATUS.PLAYING;
		m_tAudioSource.Play ();
		return;
	}

	public AudioChannelData(AUDIO_TYPE _eAudioType , AudioSource _tAudioSource){
		m_tAudioClipData = new AudioClipData ();

		m_eStatus = STATUS.IDLE;
		m_tAudioSource = _tAudioSource;
		m_eAudioType = _eAudioType;
	}
	public AudioChannelData(){
		;// 空で返す用
	}

}


// 音管理クラス
[RequireComponent(typeof(SoundPlayerSupport))]
public class SoundManager : MonoBehaviour {

	protected static SoundManager instance = null;
	public SoundPlayerSupport m_csSoundPlayerSupport;

	public void Add( AUDIO_TYPE _eAudioType , AudioClipData _tData ){
		m_tAudioClipDataList [(int)_eAudioType].Add (_tData);
		return;
	}
	public void Add( AudioClipData _tData ){
		Add (_tData.m_eAudioType, _tData);
		return;
	}

	public static SoundManager Instance {
		get {
			if (instance == null) {
				GameObject obj = GameObject.Find ("SoundManager");
				if (obj == null) {
					obj = new GameObject("SoundManager");
					//Debug.LogError ("Not Exist AtlasManager!!");
				}
				instance = obj.GetComponent<SoundManager> ();
				if (instance == null) {
					//Debug.LogError ("Not Exist AtlasManager Script!!");
					instance = obj.AddComponent<SoundManager>() as SoundManager;
				}
				instance.initialize ();
			}
			return instance;
		}
	}




	// 音量
	public SoundVolume volume = new SoundVolume();

	// === AudioSource ===
 	// BGM
	private AudioSource BGMsource;

	private int[] CHANNNEL_NUM_LIST = new int[(int)AUDIO_TYPE.MAX]{
		0,		// NONE
		2,		// BGM
		16,		// SE
		16,		// VOICE
		16,		// JINGLE
	};

	public List<AudioClipData> [] m_tAudioClipDataList = new List<AudioClipData>[] { //new List<AudioClipData>();
		new List<AudioClipData>(),
		new List<AudioClipData>(),
		new List<AudioClipData>(),
		new List<AudioClipData>(),
		new List<AudioClipData>(),
	};
	public List<AudioChannelData> [] m_tAudioChannnelDataList =  new List<AudioChannelData>[(int)AUDIO_TYPE.MAX] {//  aa new List<AudioChannelData>[](new List<AudioChannelData> () [(int)AUDIO_TYPE.MAX];
		new List<AudioChannelData>(),
		new List<AudioChannelData>(),
		new List<AudioChannelData>(),
		new List<AudioChannelData>(),
		new List<AudioChannelData>(),
	};
	public static string BGM_VOLUME_KEY = "BGMVolume";
	public static string SE_VOLUME_KEY = "SEVolume";
	public static string VOICE_VOLUME_KEY = "VoiceVolume";
	public static string JINGLE_VOLUME_KEY = "JingleVolume";

	void initialize (){
		DontDestroyOnLoad(gameObject);

		m_csSoundPlayerSupport = GetComponent<SoundPlayerSupport> ();

		// 全てのAudioSourceコンポーネントを追加する
		BGMsource = gameObject.AddComponent<AudioSource>();
		BGMsource.loop = true;

		for (int i = 0; i < (int)AUDIO_TYPE.MAX; i++) {
			//Debug.Log ((AUDIO_TYPE)i);
			for (int num = 0; num < CHANNNEL_NUM_LIST [i]; num++) {
				//Debug.Log ("channel:" + num.ToString() );
				AudioSource source = gameObject.AddComponent<AudioSource>();
				AudioChannelData channnelData = new AudioChannelData ((AUDIO_TYPE)i , source );
				m_tAudioChannnelDataList [i].Add (channnelData);
			}
		}

	}

	void Start(){
		// TODO:場所を変更しなければエラー吐いてしまうため一時コメントアウト.
//		InitSoundSettingData ();
	}

	void Update () {
		// ボリューム設定
		BGMsource.volume = volume.BGM;

		// ここ、変更があった時だけでいな
		for (int i = 0; i < (int)AUDIO_TYPE.MAX; i++) {
			float fVolume = 1.0f;
			switch ((AUDIO_TYPE)i) {

			case AUDIO_TYPE.BGM:
				fVolume = volume.BGM;
				break;
			case AUDIO_TYPE.SE:
				fVolume = volume.SE;
				break;
			case AUDIO_TYPE.VOICE:
				fVolume = volume.VOICE;
				break;
			case AUDIO_TYPE.JINGLE:
				fVolume = volume.JINGLE;
				break;
			default:
				fVolume = 1.0f;
				break;
			}
			foreach (AudioChannelData channnel in m_tAudioChannnelDataList[i]) {
				channnel.m_tAudioSource.volume = fVolume;
			}
		}
		bgm_ctrl();

		return;
	}

	public int m_intBgmStatus = 0;
	public int m_intBgmStatusPre = 0;
	public float m_fBgmSave;
	public void BgmFadeout(){
		m_fBgmSave = volume.BGM;
		m_intBgmStatus = 1;
		return;
	}

	public void BgmFadein(){
		m_intBgmStatus = 2;
		return;
	}

	private void bgm_ctrl(){

		switch( m_intBgmStatus )
		{
		case 0:
			break;

		case 1:
			if( 0.0f < volume.BGM ){
				volume.BGM -= Time.deltaTime;
			}
			else {
				volume.BGM = 0.0f;
				m_intBgmStatus = 0;
			}
			break;
			
		case 2:
			if( volume.BGM < m_fBgmSave){
				volume.BGM += Time.deltaTime;
			}
			else {
				volume.BGM = m_fBgmSave;
				m_intBgmStatus = 0;
			}
			break;

		default:
			break;

		}
	}

	public void PlaySound( AudioChannelData _channnel ){

		AudioClipData audioClipData = new AudioClipData();
		//AudioClipData audioClipData;
		if ( false == IsAudioClipData (_channnel.m_strFilename, _channnel.m_eAudioType , ref audioClipData)) {
			return;
		}
		//Debug.Log ("PlaySound:" + _channnel.m_strFilename);
		_channnel.Play (audioClipData);

		return;
	}

	// サウンドを鳴らす
	AudioChannelData playSound( AUDIO_TYPE _eAudioType , string _strName , bool _bLoop = false ){

		_strName = _strName.ToLower ();

		bool bHit = false;
		AudioChannelData data = new AudioChannelData ();

		bHit = getEnableChannnel (_eAudioType, ref data);

		if (bHit == true) {
			data.m_strFilename = _strName;
			data.m_bLoop = _bLoop;
			data.m_eStatus = AudioChannelData.STATUS.REQUEST;
		} else {
			Debug.LogError ("no hit");
			return data;
		}

		AudioClipData audioClipData = new AudioClipData();
		if ( false == IsAudioClipData (_strName, _eAudioType, ref audioClipData)) {
			//Debug.LogError ( "audio_type=" +_eAudioType + " " + "playSound:" + _strName);
			return m_csSoundPlayerSupport.PlaySound ( data );
		}

		//Debug.Log ("audio_type:" + _eAudioType + " playSound:" + data.m_strFilename);

		data.Play (audioClipData);
		return data;
	}

	/// <summary>
	/// Determines whether this instance is audio data the specified _strName _dataList _dataRet.
	/// </summary>
	/// <returns><c>true</c> if this instance is audio data the specified _strName _dataList _dataRet; otherwise, <c>false</c>.</returns>
	/// <param name="_strName">_str name.</param>
	/// <param name="_dataList">検索する_data list.</param>
	/// <param name="_dataRet">_data ret.</param>
	private bool IsAudioClipData( string _strName , List<AudioClipData> _dataList , ref AudioClipData _dataRet ){

		_strName = _strName.ToLower ();
		Debug.Log (_strName ); 

		bool bRet = false;
		foreach (AudioClipData data in _dataList) {
			//Debug.Log (data.m_strName);
			if (data.m_strName.Contains (_strName) == true) {
				Debug.Log (_strName + " == " + data.m_strName); 
				_dataRet = data;
				bRet = true;
				break;
			} else {
				Debug.Log (_strName + " != " + data.m_strName); 
			}
		}
		return bRet;
	}
	public bool IsAudioClipData( string _strName , AUDIO_TYPE _eAudioType , ref AudioClipData _dataRet ){
		return IsAudioClipData (_strName, m_tAudioClipDataList [(int)_eAudioType], ref _dataRet);
	}

	public AudioChannelData PlayBGM( string _strName ){
		_strName = _strName.ToLower ();
		//Debug.LogError ( _strName );

		AudioChannelData channelData = m_tAudioChannnelDataList [(int)AUDIO_TYPE.BGM] [0];

		//Debug.Log ("channelData:"+channelData);
		//Debug.Log ("channelData:"+channelData.m_strFilename);
		if (channelData.m_strFilename != null && channelData.m_strFilename.ToLower ().Equals (_strName.ToLower ()) == true) {
			return channelData;
		}

		channelData.m_strFilename = _strName;
		channelData.m_bLoop = true;
		AudioClipData playData = new AudioClipData();
		if( false == IsAudioClipData( _strName , AUDIO_TYPE.BGM , ref playData )){
			m_csSoundPlayerSupport.PlaySound (channelData) ;
			return channelData;
		}
		PlaySound (channelData);
		return channelData;
	}

// 一旦今のやつをそのまま動かす
	public void PlayBGM( SOUND.BGM _eBGM ){
		string strName = "";

		switch (_eBGM) {
		case SOUND.BGM.MAIN:
			strName = "01_mainBGM";
			break;

		case SOUND.BGM.GOLD_RUSH:
			strName = "bgm_battle2";
			break;

		default:
			break;
		}
		PlayBGM (strName);
		return;
	}


	// BGM停止
	public void StopBGM(){
		AudioChannelData channelData = m_tAudioChannnelDataList [(int)AUDIO_TYPE.BGM] [0];
		channelData.m_tAudioSource.Stop ();
		channelData.m_tAudioSource.clip = null;
		channelData.m_eStatus = AudioChannelData.STATUS.IDLE;
		channelData.m_strFilename = "";
		return;
	}

	// こいつは絶対鳴らすマン
	public int play_sound( AudioSource _audioSource , AudioChannelData _audioChannnelData ){
		_audioSource.clip = _audioChannnelData.m_tAudioClipData.m_AudioClip;
		_audioSource.loop = _audioChannnelData.m_bLoop;
		_audioSource.Play ();
		return 0;
	}

	public AudioChannelData PlayVoice( string _strName , bool _bLoop = false ){
		return playSound (AUDIO_TYPE.VOICE, _strName, _bLoop);
	}

	// 空いているチャンネルを返す
	private bool getEnableChannnel( AUDIO_TYPE _eAudioType , ref AudioChannelData _channelData ){
		bool bRet = false;

		Debug.Log ("m_tAudioChannnelDataList.Length=" + m_tAudioChannnelDataList.Length.ToString () + " _eAudioType=" + _eAudioType.ToString ());

		foreach (AudioChannelData data in m_tAudioChannnelDataList[(int)_eAudioType]) {

			Debug.Log (data.m_eStatus.ToString() + ":" + data.m_strFilename );

			if (data.m_eStatus == AudioChannelData.STATUS.IDLE) {
				bRet = true;
			} else if (data.m_eStatus == AudioChannelData.STATUS.PLAYING && !data.m_tAudioSource.isPlaying) {
				bRet = true;
			} else {
			}

			if (bRet == true) {
				_channelData = data;
				break;
			}

		}
		return bRet;
	}

	public AudioChannelData PlaySE(string _strName , bool _bLoop = false , bool _bForce = false ){
		return playSound (AUDIO_TYPE.SE, _strName, _bLoop); 
	}
	// 一旦今のやつをそのまま動かす
	public AudioChannelData PlaySE( SOUND.SE _eSe , bool _bIsLoop = false , bool _bForce = false){
		string strName = "notfound";
		switch (_eSe) {
		case SOUND.SE.MEDAL_FALL:
			strName = "testname";
			break;
		}
		return new AudioChannelData ();
		return PlaySE (strName, _bIsLoop, _bForce);
	}

	/// <summary>
	/// ok se
	/// </summary>
	public void PlaySeOK()
	{
		PlaySE ("se_decide_00");
	}
	/// <summary>
	/// no se
	/// </summary>
	public void PlaySeNo()
	{
		PlaySE ("se_fail_00");
	}

	public void InitSoundSettingData( ){

		volume.BGM = 1.0f;
		volume.SE = 1.0f;
		volume.VOICE = 1.0f;
	}

	/// <summary> BGM・SE・VOICEの音量を現在設定で保存する </summary>
	public void SaveSoundVolume()
	{
		SaveBGMVolume (volume.BGM);
		SaveSEVolume (volume.SE);
		SaveVoiceVolume (volume.VOICE);
	}


	/// <summary>
	/// Sets the BGM volume.
	/// </summary>
	/// <param name="bgmVolume">Bgm volume.</param>
	public void SaveBGMVolume(float bgmVolume){
	}

	/// <summary>
	/// Resets the BGM volume.
	/// </summary>
	public void ResetBGMVolume(){
	}

	/// <summary>
	/// Sets the SE volume.
	/// </summary>
	/// <param name="seVolume">Se volume.</param>
	public void SaveSEVolume(float seVolume){
	}

	/// <summary>
	/// Resets the SE volume.
	/// </summary>
	public void ResetSEVolume(){
	}

	/// <summary>
	/// Sets the voice volume.
	/// </summary>
	/// <param name="voiceVolume">Voice volume.</param>
	public void SaveVoiceVolume(float voiceVolume){
	}

	/// <summary>
	/// Resets the voice volume.
	/// </summary>
	public void ResetVoiceVolume(){
	}

}

// 音量クラス
[Serializable]
public class SoundVolume{
	public float BGM = 1.0f;
	public float VOICE = 1.0f;
	public float SE = 1.0f;
	public float JINGLE = 1.0f;

	public bool MUTE;
	  
	public void Init(){
		BGM = 1.0f;
		VOICE = 1.0f;
		SE = 1.0f;
		JINGLE = 1.0f;

		MUTE = true;
	}
}
