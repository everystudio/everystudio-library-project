using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
//using MiniJSON;

public enum E_NETWORK{
	// 基本情報系
	GET_UID,
	REGISTER_ENTRY,
	LIMIT						//=256
}

public class TNetworkData {
	public int m_iSerial;
	public CommonNetwork.EConnect m_eConnect;
	public string m_strData;
	public IDictionary m_dictRecievedData;

	public bool m_bIsError;
	public int m_iErrorCode;

	public bool IsConnected(){
		if (m_eConnect == CommonNetwork.EConnect.CONNECTED) {
			return true;
		}
		return false;
	}

	public bool IsError(){
		bool bRet = false;

		switch (m_eConnect) {
		case CommonNetwork.EConnect.ERROR_REQUEST:
		case CommonNetwork.EConnect.ERROR_TIMEOUT:
		case CommonNetwork.EConnect.ERROR_DATA_EMPTY:
		case CommonNetwork.EConnect.ERROR_RECIEVED:
		case CommonNetwork.EConnect.ERROR_UNKNOWN:
			bRet = true;
		//Debug.Log("Network Error!" + m_eConnect);
			break;
		default:
			break;
		}
		return bRet;
	}


	public TNetworkData( int _iSerial ){
		m_iSerial = _iSerial;
		m_eConnect = CommonNetwork.EConnect.CONNECTING;
		m_bIsError = false;
		m_iErrorCode = 0;
		return;
	}

}

public class CommonNetwork : MonoBehaviour {

	const int TIMEOUT_TIME 				= 20;
	const int SESSION_ERROR_CODE 		=  9;
	const int MAINTENANCE_ERROR_CODE 	= 20;
	const int APP_VERSION_ERROR 		= 31;
	const int UNKNOWN_ERROR_CODE = 255;
	const string JSON_ERROR_CODE = "error_code";
	public const string APPLYING_URL = "http://production-puzzle-app-2026643382.ap-northeast-1.elb.amazonaws.com/";

	public Dictionary<int , TNetworkData> m_dictNetworkData = new Dictionary<int , TNetworkData>();

	private int m_intTimeoutTime;
	private bool IS_STAND_ALONE = false;
	public bool isStandAlone(){
		return IS_STAND_ALONE;
	}

	public bool IsMaintenance;
	public bool IsSessionError;

	public enum EConnect{
		DISCONNECT			= 1,
		CONNECTING			= 2,
		CONNECTED			= 3,

		ERROR_REQUEST		= 11,
		ERROR_TIMEOUT		,
		ERROR_DATA_EMPTY	,
		ERROR_RECIEVED 		,
		ERROR_APP_VERSION	,		// アプリのバージョンが違う
		ERROR_UNKNOWN		,

		MAX					= 99,
	};

	public enum EStatus {
		SLEEP		= 0,
		INITIALIZE	,
		OK			,
		MAX			,
	};

	public EConnect	m_eConnect;
	public EStatus	m_eStatus;					// this class controll status
	public int m_intErrorCode;
	public int ErrorCode {
		get{ return m_intErrorCode;}
	}

	public int m_iNetworkSerial;

	public IDictionary m_dictRecievedData;
	public bool m_bErrorLog;

	public bool SESSION_ERROR;

	// 受信データ
	public IDictionary GetData( int _iSerial ){

		if (m_dictNetworkData.ContainsKey (_iSerial)) {
			return m_dictNetworkData [_iSerial].m_dictRecievedData;
		}
		return new TNetworkData (_iSerial).m_dictRecievedData;
	}

	public string GetString( int _iSerial ){
		if (m_dictNetworkData.ContainsKey (_iSerial)) {
			return m_dictNetworkData [_iSerial].m_strData;
		}
		return new TNetworkData (_iSerial).m_strData;
	}

	// 接続できたかどうかの確認のみ（エラー状態は特に検出せず）
	public bool IsConnected( int _iSerial ){
		if (m_dictNetworkData.ContainsKey (_iSerial)) {
			return m_dictNetworkData[_iSerial].IsConnected();
		}
		return false;
	}

	// 現在の通信接続状態を取得
	public EConnect getConnectStatus(){
		return m_eConnect;
	}

	/// <summary>
	/// Determines whether this instance is error the specified _iSerial.
	/// エラー状態の真偽を返す.
	/// TODO:現状動いてません
	/// </summary>
	/// <returns><c>true</c> if this instance is error the specified _iSerial; otherwise, <c>false</c>.</returns>
	/// <param name="_iSerial">_i serial.</param>
	public bool IsError( int _iSerial ){
		bool bRet = false;
		if (m_dictNetworkData.ContainsKey (_iSerial)) {
			return m_dictNetworkData [_iSerial].IsError();
		}
		return bRet;
	}

	public void setErrorLog( bool _bSet ){
		m_bErrorLog = _bSet;
		return;
	}

	void Start(){
		initialize();
		return;
	}

	public void SetTimeoutTime( int _intTime ){
		m_intTimeoutTime = _intTime;
		return;
	}

	public int Recieve( E_NETWORK _eNetwork ){
		string strUrl = GetURL (_eNetwork);
		return Recieve (strUrl);
	}
	public int Recieve( string _strUrl ){
		return sendNetwork (_strUrl, "{\"dummy\":0}");
	}
	public int Request( E_NETWORK _eNetwork , string _strJson , int _intSerial = 0){
		string strUrl = GetURL (_eNetwork);
		return Request (strUrl, _strJson);
	}
	public int Request( string _strUrl , string _strJson , int _intSerial = 0){
		return sendNetwork (_strUrl, _strJson);
	}

	public int RecieveHtml( string _strUrl ){
		return sendGetHtml (_strUrl, "{\"dummy\":0}");
	}
	public int RequestHtml( string _strUrl , string _strJson ){
		//StartCoroutine (sendModule (_eNetwork , _strJson , _intSerial ));
		return sendGetHtml (_strUrl, _strJson);
	}


	public int RegisterEntry(string _strUUID ){
		string json_data = "{\"uuid\":\""+ _strUUID +"\"}";
		return Request (E_NETWORK.REGISTER_ENTRY, json_data);
	}

	public int RequestUid( string _strUUID ){
		string json_data = "{\"uuid\":\""+ _strUUID +"\"}";
		return Request (E_NETWORK.GET_UID, json_data);
	}

	public bool send( ){
		return true;

	}

	public bool send( string _strSendJson ){
		return true;
	}

	public GameObject m_goConnectingDialog;
	private bool m_bAutoConnectingDialog = true;
	public bool AutoConnectingDialog {
		get{ return m_bAutoConnectingDialog;}
		set{ m_bAutoConnectingDialog = value;}
	}

	private STEP m_eConnectingDialogStatus;
	private bool m_bShowConnectingDialog = false;
	private float m_fConnectingTimer;
	private GameObject m_goConnectingDialogPos;
	public void setConnectiongDialogPos (GameObject _pos) {
		m_goConnectingDialogPos = _pos;
	}
	public void showConnectingDialog() {
		m_bAutoConnectingDialog = false;
		m_bShowConnectingDialog = true;
		m_eConnectingDialogStatus = STEP.NONE;
	}
	/*
	public void closeConnectingDialog() {
		m_bAutoConnectingDialog = true;
		m_bShowConnectingDialog = false;
		m_eConnectingDialogStatus = STEP.MAX;
		if (m_goConnectingDialog) {
			TweenScale ts = null;
			ts = TweenScale.Begin (m_goConnectingDialog, 0.252f, Vector3.zero);
			if (ts != null) {
				EventDelegate.Set (ts.onFinished, destoryConnectiongDialog);
			} else {
				destoryConnectiongDialog ();
			}
		}
	}
	*/
	private void destoryConnectiongDialog () {
		if( m_goConnectingDialog ){
			Destroy( m_goConnectingDialog );
		}
		if (m_goConnectingDialogPos) {
			m_goConnectingDialogPos = null;
		}
	}

	/*
	private void chkConnectingDialog() {
		if (m_bShowConnectingDialog) {
//			//Debug.Log ("m_eConnectingDialogStatus="+m_eConnectingDialogStatus+" m_bAutoConnectingDialog="+m_bAutoConnectingDialog);
			switch (m_eConnectingDialogStatus) {
			case STEP.NONE:
				m_fConnectingTimer = 0.0f;
				if( m_goConnectingDialog ){
					Destroy( m_goConnectingDialog );
				}
				m_eConnectingDialogStatus = STEP.DELAY;

				break;
			case STEP.DELAY:
				if (m_fConnectingDialogTime <= m_fConnectingTimer) {
					m_eConnectingDialogStatus = STEP.BUSY;
				} else {
					m_fConnectingTimer += Time.deltaTime;
				}

				break;
			case STEP.BUSY:
				if( m_goConnectingDialog ){
					Destroy( m_goConnectingDialog );
				}
				GameObject PrefConnecting = Resources.Load("rPrefConnecting", typeof(GameObject)) as GameObject;
				if( m_goConnectingDialogPos ){
					m_goConnectingDialog = NGUITools.AddChild (m_goConnectingDialogPos, PrefConnecting) as GameObject;
				}
				else {
					m_goConnectingDialog = NGUITools.AddChild (this.gameObject, PrefConnecting) as GameObject;
				}
				m_csCtrlConnecting = (CtrlConnecting)m_goConnectingDialog.GetComponent(typeof(CtrlConnecting));
				m_csCtrlConnecting.Init(m_goConnectingDialog);

				m_eConnectingDialogStatus = STEP.IDLE;

				break;
			case STEP.IDLE:

				break;
			default:
				if( m_goConnectingDialog ){
					Destroy( m_goConnectingDialog );
				}
				break;
			}
		}
	}
	*/

	private int sendNetwork( string _strUrl  , string _strJson ){
		m_eConnect = EConnect.CONNECTING;
		// 通信に利用するシリアル番号を発行する
		m_iNetworkSerial += 1;
		StartCoroutine (sendModule (_strUrl , _strJson , m_iNetworkSerial ));
		return m_iNetworkSerial;
	}

	private int sendGetHtml( string _strUrl , string _strJson ){
		m_eConnect = EConnect.CONNECTING;
		// 通信に利用するシリアル番号を発行する
		m_iNetworkSerial += 1;
		StartCoroutine (getHtml (_strUrl , _strJson , m_iNetworkSerial ));
		return m_iNetworkSerial;
	}

	IEnumerator sendModule (string _strUrl , string _strJson , int _iNetworkSerial )
	{
		// 通信用に収納
		WWWForm form = new WWWForm ();
		form.AddField ("req_data", _strJson);

		Debug.Log (_strUrl + _strJson);

		string strOutput = "";

		WWW www = new WWW( _strUrl ,form); //何も返ってこない場合

		// リクエストを受け取る処理(別メソッドでタイムアウト判定) 
		yield return StartCoroutine(ResponseConnectJson(www, (float)m_intTimeoutTime));

		//  リクエストエラーの場合
		if(!string.IsNullOrEmpty(www.error)){

			m_eStep = STEP.IDLE;
			m_intErrorCode = UNKNOWN_ERROR_CODE;
			m_eConnect = EConnect.ERROR_UNKNOWN;

			Debug.Log(string.Format("Fail Whale!\n{0}", www.error));
			yield break; // コルーチンを終了
		
			//タイムアウトエラーだった場合
		} else if (m_eConnect == EConnect.ERROR_TIMEOUT){
			if( m_bErrorLog ){
				Debug.Log("timeout_error");
			}
			m_eStep = STEP.IDLE;
			yield break; // コルーチンを終了

			//リクエストしたのに空で返ってきた場合
		} else if(string.IsNullOrEmpty(www.text)){
			if( m_bErrorLog ){
				Debug.Log("no items");
			}
			m_eStep = STEP.IDLE;
			yield break; // コルーチンを終了
		}
		else {
            
            SafeDebutLog ("success:"+www.text);

			string decodedText = "";
			strOutput = www.text;
		}
		IDictionary dictRecieveData;
		#if USE_NETWORK_MINIJSON
		dictRecieveData = (IDictionary)Json.Deserialize(strOutput);
		#else
		dictRecieveData = null;
		#endif

		m_dictRecievedData = dictRecieveData;							// これは直接利用しない
		// 受信したデータを格納する
		TNetworkData tNetworkData = new TNetworkData (_iNetworkSerial);
		tNetworkData.m_iSerial = _iNetworkSerial;
		tNetworkData.m_dictRecievedData = dictRecieveData;
		tNetworkData.m_strData = strOutput;
		tNetworkData.m_bIsError = false;
		tNetworkData.m_iErrorCode = 0;

		bool bIsError = false;
		bool bErrorCode = false;
		if (dictRecieveData != null) {
			bErrorCode = dictRecieveData.Contains (JSON_ERROR_CODE);
		}
		int iErrorCode = 0;

		if( bErrorCode ){
			DataCopyUtil.copyLongToInt (ref iErrorCode, dictRecieveData, JSON_ERROR_CODE);
			if( 0 < iErrorCode ){
				bIsError = true;
				tNetworkData.m_bIsError = true;
				tNetworkData.m_iErrorCode = iErrorCode;
			}
		}
		m_intErrorCode = 0;

		EConnect eConnect = EConnect.CONNECTING;
		if( bIsError ){
			eConnect = EConnect.ERROR_RECIEVED;
			//Debug.Log( "m_intErrorCode=" + m_intErrorCode );
			if( m_intErrorCode == MAINTENANCE_ERROR_CODE ){
				// 状態は変更しません
				IsMaintenance = true;
			}
			else if( m_intErrorCode == SESSION_ERROR_CODE){
				IsSessionError = true;
			}
			else {
				eConnect = EConnect.ERROR_RECIEVED;
			}
		}
		else {
			eConnect = EConnect.CONNECTED;
		}
		m_eConnect = eConnect;
		tNetworkData.m_eConnect = eConnect;
		m_dictNetworkData.Remove (_iNetworkSerial);
		m_dictNetworkData.Add (_iNetworkSerial, tNetworkData);

		m_eStep = STEP.IDLE;

		yield break;		// コルーチン終了
	}

	// 共通部おいのでまとめれそう
	IEnumerator getHtml (string _strUrl , string _strJson , int _iNetworkSerial ){

		// 通信用に収納
		WWWForm form = new WWWForm ();
		form.AddField ("req_data", _strJson);

		setConnectingDialogTime ();

		string url = _strUrl;

	//		Debug.Log(url + _strJson);
		SafeDebutLog (url + _strJson);

		string strOutput = "";

		WWW www = new WWW( url ,form); //何も返ってこない場合

		// リクエストを受け取る処理(別メソッドでタイムアウト判定) 
		yield return StartCoroutine(ResponseConnectJson(www, (float)m_intTimeoutTime));

		//  リクエストエラーの場合
		if(!string.IsNullOrEmpty(www.error)){

			m_eStep = STEP.IDLE;
			m_intErrorCode = UNKNOWN_ERROR_CODE;
			m_eConnect = EConnect.ERROR_UNKNOWN;

			Debug.Log(string.Format("Fail Whale!\n{0}", www.error));
			yield break; // コルーチンを終了

			//タイムアウトエラーだった場合
		} else if (m_eConnect == EConnect.ERROR_TIMEOUT){
			if( m_bErrorLog ){
				Debug.Log("timeout_error");
			}
			m_eStep = STEP.IDLE;
			yield break; // コルーチンを終了

			//リクエストしたのに空で返ってきた場合
		} else if(string.IsNullOrEmpty(www.text)){
			if( m_bErrorLog ){
				Debug.Log("no items");
			}
			m_eStep = STEP.IDLE;
			yield break; // コルーチンを終了
		}
		else {

			SafeDebutLog ("success:"+www.text);

			string decodedText = "";
			strOutput = www.text;
		}
		IDictionary dictRecieveData;
		#if USE_NETWORK_MINIJSON
		dictRecieveData = (IDictionary)Json.Deserialize(strOutput);
		#else
		dictRecieveData = null;
		#endif
		m_dictRecievedData = dictRecieveData;							// これは直接利用しない

		// 受信したデータを格納する
		TNetworkData tNetworkData = new TNetworkData (_iNetworkSerial);
		tNetworkData.m_iSerial = _iNetworkSerial;
		tNetworkData.m_dictRecievedData = dictRecieveData;
		tNetworkData.m_strData = strOutput;
		tNetworkData.m_bIsError = false;
		tNetworkData.m_iErrorCode = 0;

		bool bIsError = false;
		bool bErrorCode = false;
		if (dictRecieveData != null) {
			bErrorCode = dictRecieveData.Contains (JSON_ERROR_CODE);
		}
		int iErrorCode = 0;

		if( bErrorCode ){
			DataCopyUtil.copyLongToInt (ref iErrorCode, dictRecieveData, JSON_ERROR_CODE);
			if( 0 < iErrorCode ){
				bIsError = true;
				tNetworkData.m_bIsError = true;
				tNetworkData.m_iErrorCode = iErrorCode;
			}
		}
		m_intErrorCode = 0;

		EConnect eConnect = EConnect.CONNECTING;
		if( bIsError ){
			eConnect = EConnect.ERROR_RECIEVED;
			//Debug.Log( "m_intErrorCode=" + m_intErrorCode );
			if( m_intErrorCode == MAINTENANCE_ERROR_CODE ){
				// 状態は変更しません
				IsMaintenance = true;
			}
			else if( m_intErrorCode == SESSION_ERROR_CODE){
				IsSessionError = true;
			}
			else {
				eConnect = EConnect.ERROR_RECIEVED;
			}
		}
		else {
			eConnect = EConnect.CONNECTED;
		}
		m_eConnect = eConnect;
		tNetworkData.m_eConnect = eConnect;
		m_dictNetworkData.Remove (_iNetworkSerial);
		m_dictNetworkData.Add (_iNetworkSerial, tNetworkData);

		m_eStep = STEP.IDLE;
		yield break;
	}
	private IEnumerator ResponseConnectJson(WWW www, float timeout){

		float requestTime = Time.time;
		while(!www.isDone)
		{
			if(Time.time - requestTime < timeout){
				yield return null;
			}
			else {
				//Debug.LogWarning("TimeOut"); //タイムアウト
				m_eConnect = EConnect.ERROR_TIMEOUT;		// エラー状態にする
				break;
			}
		}
		yield return www;
	}


//==================================================================================

    private void SafeDebutLog (string debug_json) {
        int limit_string = 5000;
        int count = Mathf.CeilToInt((float)debug_json.Length / limit_string);
        for (int i = 0;i < count;i++) {

			string strLogMessage = "";
            if (i == count-1) {
				strLogMessage = debug_json.Substring (i * limit_string);
            } else {
				strLogMessage = debug_json.Substring (i * limit_string, limit_string);
            }
			Debug.Log(strLogMessage);
			//Textreader.Append ( "/log_data.txt" , strLogMessage);
        }
    }

	public string GetURL(E_NETWORK _eNetwork )
	{
		string address = " ";
		string host = " ";

		host = ConfigManager.instance.GetEditPlayerSettingsData().m_strServerUrl;

		/*
		if( DEFINE.IsApplying ){
			host = APPLYING_URL;
		}
		*/

		switch( _eNetwork )
		{
		case E_NETWORK.REGISTER_ENTRY:		address="register/entry";break;
		case E_NETWORK.GET_UID:				address="register/confirm";break;

		default:
			address="error";
			break;
		}

		string url = host + address;
		return url;


	}
		
//==================================================================================



	public enum STEP{

		NONE 		= 0,
		IDLE 		,

		DELAY 		,
		BUSY 		,
		END 		,

		MAX 		,
	}
	public STEP m_eStep;
	public STEP m_eStepPre;
	public float m_fTimer;
	public float m_fConnectingDialogTime;
	public void setConnectingDialogTime (float _connectingDialogTime = 3.0f) {
		m_fConnectingDialogTime = _connectingDialogTime;
	}

	public GameObject m_goPosConnecting;

	public GameObject m_goConnecting;
	//public CtrlConnecting m_csCtrlConnecting;

	void Update (){
		/*
		bool bInit = false;
		if( m_eStepPre != m_eStep ){
			m_eStepPre  = m_eStep;
			bInit = true;
		}

		switch( m_eStep )
		{
		case STEP.NONE: break;

		case STEP.IDLE:
			if( bInit ){
				if( m_csCtrlConnecting != null ){
                    // From Here i think he want remove the indicator 
                    //    so i create a class in CtrlConnecting so he can erase it. 
                    //    the original script doest erase it created it
                    // The right way to do this is Close the Script, by Close()
                    // since if u do this the memory is still well alive in the system
					m_csCtrlConnecting.EraseIndicator();
					m_csCtrlConnecting = null;
				}
				if( m_goConnecting ){
					Destroy( m_goConnecting );
				}
			}
			break;

		case STEP.DELAY:
			if( bInit ){
				m_fTimer = 0.0f;
				if( m_goConnecting ){
					Destroy( m_goConnecting );
				}
			}
			if( m_fConnectingDialogTime <= m_fTimer && m_bAutoConnectingDialog){
				m_eStep = STEP.BUSY;
			}
			else {
				m_fTimer += Time.deltaTime;
			}
			break;

		case STEP.BUSY:
			if( bInit ){
				if( m_goConnecting ){
					Destroy( m_goConnecting );
				}
				GameObject PrefConnecting = Resources.Load("rPrefConnecting", typeof(GameObject)) as GameObject;
				if( m_goPosConnecting ){
					m_goConnecting = NGUITools.AddChild (m_goPosConnecting, PrefConnecting) as GameObject;
				}
				else {
					m_goConnecting = NGUITools.AddChild (this.gameObject, PrefConnecting) as GameObject;
				}
				m_csCtrlConnecting = (CtrlConnecting)m_goConnecting.GetComponent(typeof(CtrlConnecting));
				m_csCtrlConnecting.Init(m_goConnecting);
			}
			break;
		case STEP.END:
		default:
			break;

		}
		*/

		//chkConnectingDialog ();

		return;
	}


	// ====================================================================================
	// 以下シングルトン宣言
	private void initialize(){
		m_eConnect= EConnect.DISCONNECT;
		m_eStatus = EStatus.SLEEP;
		m_bErrorLog = false;
		m_fTimer = 0.0f;

		SESSION_ERROR = false;

		m_intTimeoutTime = TIMEOUT_TIME;

		IsMaintenance = false;
		m_dictNetworkData.Clear ();

		return;
	}
	private static CommonNetwork instance = null;
	public static CommonNetwork Instance {
		get{
			if( instance == null ){
				instance = (CommonNetwork) FindObjectOfType(typeof(CommonNetwork));
				if (instance == null)
				{
					GameObject obj = new GameObject();
					obj.name = "CommonNetwork";
					obj.AddComponent<CommonNetwork>();
					instance = obj.GetComponent<CommonNetwork>();
					instance.Init();
					//Debug.Log( "Create CommonNetwork");
				}
			}
			return instance;
		 }
	}

	protected void Init(){
		CommonNetwork[] obj = FindObjectsOfType(typeof(CommonNetwork)) as CommonNetwork[];
		if( obj.Length > 1 ){
			// 既に存在しているなら削除
			Destroy(gameObject);
		}else{
			// 音管理はシーン遷移では破棄させない
			DontDestroyOnLoad(gameObject);
		}
		initialize();
		return;
	}
	void Awake(){
		return;
	}

	/*
	public LoadSettingData m_LoadSettingData;	
	
	public void LoadSetting ()
	{
		UnityEngine.Object[] assets;
		assets = Resources.LoadAll("LoadSettingData");
		
		foreach (UnityEngine.Object asset in assets)
		{
			if (asset is LoadSettingData)
			{
			    m_LoadSettingData = (LoadSettingData)asset;
			}
		}
	}	
	*/
}

/**

エラー文字列一覧


*/









