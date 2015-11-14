using UnityEngine;
using System.Collections;

public class DatabaseAssetMain : MonoBehaviour {

	//Singleton使用時はコンストラクタはつぶしておく
	private EditPlayerSettingsData m_EditPlayerSettingsData;

	public EditPlayerSettingsData GetEditPlayerSettingsData ()
	{
		if (m_EditPlayerSettingsData == null) {
			Debug.Log ("no EditPlayserSettingsData");
			UnityEngine.Object player_settings_assets;
			player_settings_assets = Resources.Load ("EditPlayerSettingsData");
			m_EditPlayerSettingsData = (EditPlayerSettingsData)player_settings_assets;
		}
		return m_EditPlayerSettingsData;
	}

	public DataSample m_DataSample;


	// Use this for initialization
	void Start () {

		UnityEngine.Object player_settings_assets;
		player_settings_assets = Resources.Load ("ExampleAsset");
		m_DataSample = (DataSample)player_settings_assets;

		Debug.Log (m_DataSample.moji);
		Debug.Log (m_DataSample.param);

		m_DataSample.moji = "chang";

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
