using UnityEngine;
using System.Collections;

public class ConfigManager : Singleton<ConfigManager>
{
	private bool m_bInited = false;

	protected ConfigManager ()
	{
	}
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

	void Awake ()
	{
		base.Awake ();
		Init ();
	}
	/*
	private bool m_isReady = false;
	public bool IsReady { get { return m_isReady; } } 
	
	
	public bool GetIsReady ()
	{
		return m_isReady;
	}
	*/
	public void Init ()
	{
		if (m_bInited == false) {
			m_bInited = true;
			LoadSetting ();
			DontDestroyOnLoad (this);
		}
		return;
	}

	public void LoadSetting ()
	{
		UnityEngine.Object player_settings_assets;
		player_settings_assets = Resources.Load ("EditPlayerSettingsData");
		m_EditPlayerSettingsData = (EditPlayerSettingsData)player_settings_assets;
	}
}









