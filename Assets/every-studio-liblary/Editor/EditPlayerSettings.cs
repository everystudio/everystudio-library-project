using UnityEditor;
using System.IO;
using UnityEngine;
using System.Collections;

public class EditPlayerSettings : ScriptableWizard
{
	[MenuItem ("Tools/EditPlayerSettings")]
	static void CreateWizard ()
	{
		EditorWindow.GetWindow<EditPlayerSettings> ("EditPlayerSettings");
	}
	public string m_strVersion = "0.0.0";
	public Define.ENVIROMENT m_eEnviroment = Define.ENVIROMENT.PRODUCTION;
	public string m_strCompanyName = "EveryStudio Inc.";
	public string m_strProductName = "右向け右";
	public string m_strBundleIdentify = "none";
	public string m_strServerUrl = "none";
	public string m_strNodejsUrl = "none";
	public string m_strS3Url = "none";
	public bool m_bIsLocalModeS3;
	public bool m_bIsSecondS3;
	public bool m_bIsLocalModeServer;
	public string m_strLocalServerUrl = "http://192.168.30.10/";
	public string m_strMypageUrl;
	protected Define.ENVIROMENT m_eEnviromentPre;

	public Define.ENVIROMENT Enviroment{ get { return m_eEnviroment; } }

	void OnGUI ()
	{
		EditorGUILayout.LabelField ("環境設定エディター");
		if (GUILayout.Button ("設定")) {

			// 設定ボタンでキャッシュクリア
			Caching.CleanCache();

			//Debug.Log( "現在の環境は:" + m_eEnviroment );
			string strBundleIdentify = "none";
			string strS3Header = Define.S3_SERVER_HEADER;

			m_strMypageUrl = "https://every-studio.com/";

			switch (m_eEnviroment) {
			case Define.ENVIROMENT.LOCAL:
				//m_strS3Url = strS3Header + "/development/";
				m_strS3Url = GetLocalAssetBundlePath () + "/assets/assetbundleresource";
				m_strServerUrl = "http://newapp-devlop-lb-1498349273.ap-northeast-1.elb.amazonaws.com:8080/";
				m_strNodejsUrl = "ws://newapp-devlop-node-lb-776612323.ap-northeast-1.elb.amazonaws.com:3000/socket.io/";
				strBundleIdentify = "jp.everystudio.turnright";
				break;

			case Define.ENVIROMENT.STREAMING_ASSETS:
				m_strS3Url = GetStreamingAssetsAssetBundlePath ();
				strBundleIdentify = "jp.everystudio.turnright";
				break;
			case Define.ENVIROMENT.PRODUCTION:
			default:
				//m_strS3Url = strS3Header + "/production/";
				m_strS3Url = strS3Header;
				m_bIsLocalModeS3 = false;
				m_bIsSecondS3 = false;
				strBundleIdentify = "jp.everystudio.turnright";
				m_strServerUrl = "";
				break;
			}

			if (m_bIsLocalModeS3) {
				
			}

			if (m_bIsLocalModeServer) {
				m_strServerUrl = m_strLocalServerUrl;
			}


			m_strBundleIdentify = strBundleIdentify;
			/*
			PlayerSettings.bundleIdentifier	= m_strBundleIdentify;
			PlayerSettings.bundleVersion = m_strVersion;
			PlayerSettings.companyName = m_strCompanyName;
			PlayerSettings.productName = m_strProductName;
			*/

			EditPlayerSettingsData resources_data = CreateInstance<EditPlayerSettingsData> ();
			resources_data.m_strVersion = m_strVersion;
			resources_data.m_eEnviroment = m_eEnviroment;
			resources_data.m_strCompanyName = m_strCompanyName;
			resources_data.m_strProductName = m_strProductName;
			resources_data.m_strBundleIdentify	= m_strBundleIdentify;
			resources_data.m_strServerUrl = m_strServerUrl;
			resources_data.m_strS3Url = m_strS3Url;
			resources_data.m_bIsLocalModeS3 = m_bIsLocalModeS3;
			resources_data.m_bIsSecondS3 = m_bIsSecondS3;
			resources_data.m_bIsLocalModeServer	= m_bIsLocalModeServer;
			resources_data.m_strLocalServerUrl	= m_strLocalServerUrl;
			resources_data.m_strMypageUrl = m_strMypageUrl;
			resources_data.m_strNodejsUrl = m_strNodejsUrl;

			Debug.Log ("here1");
			string directory_path = "Assets/every-studio/EditorConfig/Resources";
			Directory.CreateDirectory (directory_path);
			string path = directory_path + "/EditPlayerSettingsData.asset";
			Debug.Log ("here2");

			AssetDatabase.CreateAsset (resources_data, path);
			Debug.Log ("here3");
			AssetDatabase.ImportAsset (path);
			Debug.Log ("here4");
		}
		m_bIsLocalModeS3 = GUILayout.Toggle (m_bIsLocalModeS3, "LocalModeS3(True is Debug&Editor Only)");
		m_bIsSecondS3 = GUILayout.Toggle (m_bIsSecondS3, "Second (It is Development & Web Only)");

/*
		if( GUILayout.RepeatButton( "RepeatButton" ) ){
			//Debug.Log( "RepeatButton!" );
		}
*/
		m_eEnviroment = (Define.ENVIROMENT)EditorGUILayout.EnumPopup ("設定環境", m_eEnviroment);

		GUILayout.Label ("Version");
		m_strVersion = GUILayout.TextField (m_strVersion);

		GUILayout.Label ("CompanyName");
		m_strCompanyName = GUILayout.TextField (m_strCompanyName);

		GUILayout.Label ("ProductName");
		m_strProductName = GUILayout.TextField (m_strProductName);
		GUILayout.Label ("BundleIdentify");
		m_strBundleIdentify = GUILayout.TextField (m_strBundleIdentify);
		m_bIsLocalModeServer = GUILayout.Toggle (m_bIsLocalModeServer, "LocalModeServer(True is Debug&Editor Only)");
		m_strLocalServerUrl = GUILayout.TextField (m_strLocalServerUrl);


		/*
		GUILayout.Label( "CompanyName" );
		m_strCompanyName = GUILayout.TextField( m_strCompanyName );
		GUILayout.Label( "CompanyName" );
		m_strCompanyName = GUILayout.TextField( m_strCompanyName );
		GUILayout.Label( "CompanyName" );
		m_strCompanyName = GUILayout.TextField( m_strCompanyName );
		*/
		/*
		if( m_eEnviromentPre != m_eEnviroment ){
			m_eEnviromentPre  = m_eEnviroment;
			//Debug.Log( "Change Selecting Enviroment : " + m_eEnviroment);
		}
		*/

		return;

	}



	private string GetStreamingAssetsAssetBundlePath() {
		//#if UNITY_EDITOR

		//return "file://"+Application.dataPath + "/StreamingAssets";

		#if UNITY_IPHONE
		return  Application.streamingAssetsPath + "/iOS/assets/assetbundleresource";
		return  "file://" + Application.streamingAssetsPath + "/iOS/assets/assetbundleresource";
		#elif UNITY_ANDROID
		return "jar:file://" + Application.dataPath + "!/assets/";
		#else
		return "file://"+Application.dataPath + "/StreamingAssets/";
		#endif
	}


	private string GetLocalAssetBundlePath() {
        #if UNITY_EDITOR

		return "file://"+Application.dataPath + "/../" + Define.ASSET_BUNDLES_ROOT;

		//return "file://"+Application.dataPath + "/StreamingAssets/";
        #elif UNITY_IPHONE
        return  "file://" + Application.streamingAssetsPath + "/";
        #elif UNITY_ANDROID
        return "jar:file://" + Application.dataPath + "!/assets/";
        #else
        return "file://"+Application.dataPath + "/StreamingAssets/";
        #endif
    }

	/*
	// Use this for initialization
	void Start () {
		PlayerSettings.bundleIdentifier = "test";
		// com.taisoninc.puzzlekingdom
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	*/
}
