using UnityEngine;
using System.Collections;

// TODO 

public class UtilAssetBundleSprite : UtilAssetBundleBase {

	public Sprite m_spLoadedSprite;

	public void Load( string _strAssetName , string _strPath , int _iVersion ){

		EditPlayerSettingsData epsData = ConfigManager.instance.GetEditPlayerSettingsData ();
		string resultUrl = epsData.m_strS3Url + Define.ASSET_BUNDLES_ROOT + _strPath +"/"+  _strAssetName + ".unity3d";

		//Debug.Log (resultUrl);
		Load (_strAssetName, resultUrl, _iVersion , m_goParent);
	}

	public void Load( string _strAssetName ){

		CsvSpriteData data = new CsvSpriteData ();
		foreach (CsvSpriteData temp in DataManager.master_sprite_list) {
			if (_strAssetName.Equals (temp.filename.ToLower()) == true) {
				data = temp;
				break;
			}
		}
		/*
		foreach (MasterLoadSprite.Data temp in DataContainer.Instance.MasterLoadSpriteList) {
			//Debug.Log ("file:" + temp.filename + " path:" + temp.path + " ver:" + temp.version);
			if (temp.filename.Equals (_strAssetName) == true) {
				data = temp;
				break;
			}
		}
		*/

		Load (data.filename, data.path, data.version);
		return;
	}

	public override void afterLoaded( AssetBundle _assetBundle , string _strAssetName ){

		if (_strAssetName == "") {
			m_goLoadObject = (GameObject)Instantiate (_assetBundle.mainAsset);
		} else {

			//m_goLoadObject = Instantiate (_assetBundle.LoadAsset (_strAssetName, typeof(GameObject))) as GameObject;
			m_spLoadedSprite = Instantiate (_assetBundle.LoadAsset (_strAssetName, typeof(Sprite))) as Sprite;

			/*
			foreach (MasterLoadSprite.Data data in DataContainer.Instance.MasterLoadSpriteList) {
				if (data.filename.Equals (_strAssetName) == true) {

					//Debug.Log ("insert sprite:" + _strAssetName);
					if (m_goLoadObject) {
						m_goLoadObject.SetActive (false);
					}
					SpriteManager.Instance.Add (_strAssetName, m_spLoadedSprite);
				}
			}
			*/
		}
	}

}



