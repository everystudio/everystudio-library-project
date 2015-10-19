using System;
using UnityEngine;
using System.Collections;

public class UtilAssetBundleBase : MonoBehaviourEx {

	// インスペクターで簡単に確認出来るようにpublicで残しておきます。
	// スクリプトで利用する場合はできるだけ
	public GameObject m_goParent;
	public void SetParent( GameObject _goParent ){
		m_goParent = _goParent;
		return;
	}

	public GameObject m_goLoadObject;

	public string m_strAssetName;
	public string m_strLoadUrl;
	public int m_iLoadVersion;
	public bool m_bLoaded;
	public bool IsLoaded(){
		return m_bLoaded;
	}

	public virtual void afterLoaded( AssetBundle _assetBundle , string _strAssetName ){
		if (_strAssetName == "") {
			m_goLoadObject = (GameObject)Instantiate (_assetBundle.mainAsset);
		} else {
			m_goLoadObject = Instantiate (_assetBundle.LoadAsset (_strAssetName, typeof(GameObject))) as GameObject;
		}
		return;
	}

	public enum STEP
	{
		NONE 		= 0,
		LOAD_START	,
		LOADING		,
		LOAD_END	,
		SOUND_TEST	,
		MAX,
	}
	public STEP m_eStep;
	public STEP m_eStepPre;

	public void Load( string _strAssetName , string _strUrl , int _iVersion , GameObject _goParent ){
		m_strAssetName = _strAssetName.ToLower ();;
		m_strLoadUrl = _strUrl;//.ToLower();		// ここは小文字にしてはいけませんでした
		m_goParent = _goParent;
		m_iLoadVersion = _iVersion;
		m_bLoaded = false;
		StartCoroutine (load (m_strAssetName, m_strLoadUrl, m_iLoadVersion));
		return;
	}

	public void  Load( string _strAssetName , string _strUrl , int _iVersion = 1 ){
		Load (_strAssetName, _strUrl, _iVersion , null);
		return;
	}

	public IEnumerator load (string _strAssetName , string _strUrl, int _iVersion = 1)
	{
		// キャッシュシステムの準備が完了するのを待ちます
		while (!Caching.ready) {
			yield return null;
		}

		// 同じバージョンが存在する場合はアセットバンドルをキャッシュからロードするか、またはダウンロードしてキャッシュに格納します。
		using (WWW www = WWW.LoadFromCacheOrDownload (_strUrl, _iVersion)) {
			yield return www;
			if (www.error != null) {
				Debug.Log (_strUrl);
				throw new Exception ("WWWダウンロードにエラーがありました:" + www.error);

			}

			AssetBundle bundle = www.assetBundle;
			// メモリ節約のため圧縮されたアセットバンドルのコンテンツをアンロード
			afterLoaded (bundle, _strAssetName);

			bundle.Unload (false);

			if (m_goParent != null && m_goLoadObject != null ) {
				m_goLoadObject.transform.parent = m_goParent.transform;
			}
		}

		m_bLoaded = true;
	}

	void Update(){
		bool bInit = false;
		if (m_eStepPre != m_eStep) {
			m_eStepPre = m_eStep;
			bInit = true;
		}
		switch (m_eStep) {
		case STEP.LOAD_START:
			if (bInit) {
				Load (m_strAssetName, m_strLoadUrl);
			}
			m_eStep = STEP.LOADING;
			break;

		case STEP.LOADING:
			if (m_bLoaded) {
				m_eStep = STEP.LOAD_END;
			}
			break;
		case STEP.LOAD_END:
			break;

		case STEP.NONE:
		case STEP.MAX:
		default:
			break;
		}
	}

}











