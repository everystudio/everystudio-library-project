using UnityEngine;
using System.Collections;

public class EditPlayerSettingsData : ScriptableObject {
	public string m_strVersion;
	public Define.ENVIROMENT m_eEnviroment;
	public string m_strCompanyName;
	public string m_strProductName;
	public string m_strBundleIdentify;

	public string m_strServerUrl;
	public string m_strS3Url;
	public bool m_bIsLocalModeS3;
	public bool m_bIsSecondS3;
	public bool m_bIsLocalModeServer;
	public string m_strLocalServerUrl;
	public string m_strNodejsUrl;

	public string m_strMypageUrl;
}
