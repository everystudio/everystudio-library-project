using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(UtilAssetBundlePrefab))]
public class PrefabManager : MonoBehaviourEx {

	[System.Serializable]
	public class TPrefabPair {
		public string 		strPrefabName;
		public GameObject 	goPrefab;
	}
	public UtilAssetBundlePrefab m_csUtilAssetBundlePrefab;
	static PrefabManager instance = null;
	public static PrefabManager Instance
    {
        get
        {
			if(instance == null)
			{
				GameObject obj = GameObject.Find("PrefabManager");
				if( obj == null )
				{
					obj = new GameObject("PrefabManager");
				}
				
				instance = obj.GetComponent<PrefabManager>();
				
				if(instance == null)
				{
					instance = obj.AddComponent<PrefabManager>() as PrefabManager;
				}
				instance.initialize ();
			}
			return instance;
		}
	}
	private void initialize(){
		DontDestroyOnLoad(gameObject);

		m_csUtilAssetBundlePrefab = GetComponent<UtilAssetBundlePrefab> ();
		m_csUtilAssetBundlePrefab.SetParent (gameObject);
	}

	[SerializeField]
	private List<TPrefabPair> m_prefLoadedPrefabList = new List<TPrefabPair>();

	public bool IsLoadedPrefab( string _strPrefabName ){
		_strPrefabName = _strPrefabName.ToLower ();
		bool bRet = false;
		foreach( TPrefabPair data in m_prefLoadedPrefabList ){
			if( _strPrefabName.Equals( data.strPrefabName.ToLower()) == true ){
				bRet = true;
				break;
			}
		}
		return bRet;
	}

	private bool getPrefab( string _strPrefabName , ref GameObject _goPrefab ){
		bool bRet = false;
		//Debug.Log (_strPrefabName);
		_strPrefabName = _strPrefabName.ToLower ();
		foreach( TPrefabPair data in m_prefLoadedPrefabList ){

			//Debug.Log (data.strPrefabName.ToLower ());
			if( _strPrefabName.Equals( data.strPrefabName.ToLower()) ){
				bRet = true;
				_goPrefab = data.goPrefab;
				break;
			}

			// 古いタイプの救済
			if ( bRet == false && (_strPrefabName.Contains ("Prefab/") || _strPrefabName.Contains ("prefab/") )) {

				string[] splitArr;
				string c;
				char[] KUGIRI = { '/' };	//データの区切り文字

				splitArr = _strPrefabName.Split (KUGIRI );	//KUGIRI変数内の各文字で分割
				_strPrefabName = splitArr [splitArr.Length - 1];	//cには『文字列A』が入る
				_strPrefabName = _strPrefabName.ToLower ();

				//Debug.Log (_strPrefabName);
				if( true == _strPrefabName.Equals( data.strPrefabName) ){
					bRet = true;
					_goPrefab = data.goPrefab;
					break;
				}
			}


		}
		return bRet;
	}

	public bool Add( string _strPrefabName , GameObject _goPrefab ){

		if (IsLoadedPrefab (_strPrefabName.ToLower()) == true) {
			return false;
		}

		TPrefabPair addData = new TPrefabPair();
		addData.strPrefabName = _strPrefabName;
		addData.goPrefab = _goPrefab;
		m_prefLoadedPrefabList.Add (addData);
		return true;
	}

	public GameObject PrefabLoadInstance( string _strPrefabName ){

		_strPrefabName = _strPrefabName.ToLower ();

		GameObject goRet = null;
		if( getPrefab( _strPrefabName , ref goRet) ){
			return goRet;
		}
		goRet = Resources.Load( _strPrefabName , typeof(GameObject) ) as GameObject;

		Add (_strPrefabName, goRet);

		return goRet;
	}

    public GameObject MakeObject( GameObject _goPrefab , GameObject _goParent ,bool _setParentPosition = true){

		if (_goPrefab == null) {
			Debug.LogError ("tabun error");
		}
		Vector3 pos = Vector3.zero;
		Quaternion rot = new Quaternion(0.0f , 0.0f ,0.0f ,0.0f );

		if( _goParent != null ){
			pos = _goParent.transform.localPosition;
			rot = _goParent.transform.rotation;
		}

        GameObject obj;

        if (_setParentPosition) {
            obj = Instantiate (_goPrefab, pos, rot) as GameObject;
            if (_goParent != null) {
                obj.transform.parent = _goParent.transform;
                obj.transform.localScale = Vector3.one;
            }
        } else {
            obj = Instantiate (_goPrefab) as GameObject;
            if (_goParent != null) {
                obj.transform.parent = _goParent.transform;
            }
        }

		// とりあえず最初は起動させる
		obj.SetActive (true);
		return obj;

	}
	public GameObject MakeObject( string _strPrefabName , GameObject _goParent ){
		GameObject prefab = PrefabLoadInstance (_strPrefabName);

		GameObject goRet = MakeObject (prefab, _goParent);
		goRet.transform.localPosition = Vector3.zero;
		return goRet;
	}

	public string ConvertAssetName( string _strFilePath ){
		// 古いタイプの救済
		string strRet = _strFilePath;

		if ( _strFilePath.Contains ("Prefab/")) {

			string[] splitArr;
			string c;
			char[] KUGIRI = { '/' };	//データの区切り文字

			splitArr = _strFilePath.Split (KUGIRI );	//KUGIRI変数内の各文字で分割
			_strFilePath = splitArr [splitArr.Length - 1];	//cには『文字列A』が入る
			strRet = _strFilePath.ToLower ();
		}
		return strRet;
	}

	/*
	 * この関数は再実装お余地あり
    public void PrefabLoadInstance( string _strPrefabName , BaseLoader.onComplete _callBack){
		GameObject goRet = null;
		if (getPrefab (_strPrefabName, ref goRet)) {
			_callBack (goRet);
			return;
		}
		string assetBundlePath = SystemSetting.GetAssetBundlesBasePath () + _strPrefabName.ToLower () + ".unity3d";
		AssetBundleLoader.Instance.StartLoadRequest (assetBundlePath, _callBack, (_go) => {
			TPrefabPair addData = new TPrefabPair ();
			addData.strPrefabName = _strPrefabName;
			addData.goPrefab = _go;
			m_prefLoadedPrefabList.Add (addData);
		});
	}
	*/

	public Queue<string > m_LoadAssetBundle = new Queue<string > ();
	public string m_strLoadingAssetBundleName;

	public bool LoadAssetBundleQueue( string _strAssetName ){

		// 小文字意外使わないので
		_strAssetName = _strAssetName.ToLower ();
		if (IsLoadedPrefab (_strAssetName) == true ) {
			return false;
		}
		m_LoadAssetBundle.Enqueue (_strAssetName);
		return true;
	}

	/*
	public bool LoadAssetBundleQueue( MasterLoadPrefab.Data _tData ){
		return LoadAssetBundleQueue (_tData.filename);
	}
	*/
	public bool IsIdle(){

		if (0 < m_LoadAssetBundle.Count) {
			return false;
		}

		return (m_eStep == STEP.IDLE);
	}


	public enum STEP {
		NONE		= 0,
		IDLE		,
		LOAD_START	,
		LOADING		,
		LOAD_END	,
		MAX			,
	}
	public STEP m_eStep;
	public STEP m_eStepPre;

	void Update(){
		bool bInit = false;

		if (m_eStepPre != m_eStep) {
			m_eStepPre  = m_eStep;
			bInit = true;
		}
		switch (m_eStep) {
		case STEP.NONE:
		case STEP.IDLE:
			if (0 < m_LoadAssetBundle.Count) {
				m_eStep = STEP.LOADING;
			}
			break;

		case STEP.LOADING:
			if( bInit ){
				m_strLoadingAssetBundleName = m_LoadAssetBundle.Dequeue ();
				
				Debug.Log( m_strLoadingAssetBundleName );
				m_csUtilAssetBundlePrefab.Load (m_strLoadingAssetBundleName);
			}
			if( m_csUtilAssetBundlePrefab.IsLoaded()){
				m_eStep = STEP.IDLE;
			}
			break;

		case STEP.MAX:
		default:
			break;

		}
	}




















}













