using UnityEngine;
using System.Collections;

public class UtilAssetBundleAtlas : UtilAssetBundleBase {

	/*
	public void Load( string _strAssetName ){

		MasterLoadAtlas.Data data = new MasterLoadAtlas.Data ();
		foreach (MasterLoadAtlas.Data temp in DataContainer.Instance.MasterLoadAtlasList) {
			if (_strAssetName.Equals (temp.filename) == true) {
				data = temp;
				break;
			}
		}
		EditPlayerSettingsData epsData = ConfigManager.instance.GetEditPlayerSettingsData ();
		string resultUrl = epsData.m_strS3Url + Define.ASSET_BUNDLES_ROOT + data.path + "/" +  data.filename + ".unity3d";

		Load (data.filename, resultUrl, data.version , m_goParent);
	}

	public override void afterLoaded( AssetBundle _assetBundle , string _strAssetName ){

		if (_strAssetName == "") {
			m_goLoadObject = (GameObject)Instantiate (_assetBundle.mainAsset);
		} else {
			Debug.Log (_strAssetName);
			m_goLoadObject = Instantiate (_assetBundle.LoadAsset (_strAssetName, typeof(GameObject))) as GameObject;

		}

		UIAtlas script = m_goLoadObject.GetComponent<UIAtlas> ();

		if (null != script) {
			foreach (MasterLoadAtlas.Data data in DataContainer.Instance.MasterLoadAtlasList) {
				if (data.filename.Equals (_strAssetName) == true) {

					Debug.Log ("insert:" + _strAssetName);
					m_goLoadObject.SetActive (false);

					// 現在はAtlasManagerは使わない
					//AtlasManager.Instance.Add (_strAssetName, script , data.replace_prefab );

				}
			}
		}

	}
	*/

}
