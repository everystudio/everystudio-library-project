using UnityEngine;
using System.Collections;

public class UtilAssetBundlePrefab : UtilAssetBundleBase {


	public void Load( string _strAssetName ){

		_strAssetName = _strAssetName.ToLower ();
		//Debug.LogError (_strAssetName);
		CsvPrefabData data = new CsvPrefabData ();


		foreach (CsvPrefabData temp in DataManager.master_prefab_list) {
			if (_strAssetName.Equals (temp.filename.ToLower()) == true) {
				data = temp;
				break;
			}
		}
		EditPlayerSettingsData epsData = ConfigManager.instance.GetEditPlayerSettingsData ();
		//string resultUrl = epsData.m_strS3Url + Define.ASSET_BUNDLES_ROOT + data.path.ToLower() +"/"+  data.filename.ToLower() + ".unity3d";
		string resultUrl = epsData.m_strS3Url + "/" + data.path+"/" +  data.filename + ".unity3d";
		Debug.Log (resultUrl);

		Load (data.filename.ToLower(), resultUrl, data.version , m_goParent);
	}

	public override void afterLoaded( AssetBundle _assetBundle , string _strAssetName ){

		if (_strAssetName == "") {
			m_goLoadObject = (GameObject)Instantiate (_assetBundle.mainAsset);
		} else {

			_strAssetName = _strAssetName.ToLower ();
			m_goLoadObject = Instantiate (_assetBundle.LoadAsset (_strAssetName, typeof(GameObject))) as GameObject;

			//foreach (MasterLoadPrefab.Data data in DataContainer.Instance.MasterLoadPrefabList) {
			foreach (CsvPrefabData data in DataManager.master_prefab_list) {
				if ( _strAssetName.Equals( data.filename.ToLower()) == true ) {

					//Debug.Log ("insert:" + _strAssetName);

					// これは強引・・・
					/*
					 * アトラスをセットする機能はなしですな・・・
					UISprite[] spriteArr = m_goLoadObject.GetComponentsInChildren<UISprite>();
					foreach (UISprite sprite in spriteArr) {
						sprite.atlas = AtlasManager.Instance.GetAtlas (sprite.spriteName);
					}
					*/

					Renderer[] renderers = m_goLoadObject.GetComponentsInChildren<Renderer>();
					foreach (Renderer renderer in renderers) {

						if (renderer == null) {
							Debug.LogError (renderer);
						} else if (renderer.sharedMaterial == null) {
							Debug.LogError (renderer.sharedMaterial);
						} else if (renderer.sharedMaterial.shader == null) {
							Debug.LogError (renderer.sharedMaterial.shader);
						} else if (renderer.sharedMaterial.shader.name == null) {
							Debug.LogError (renderer.sharedMaterial.shader.name);
						} else {
							Shader shader = Shader.Find (renderer.sharedMaterial.shader.name);
							if (shader) {
								//Debug.Log (renderer.sharedMaterial.shader.name + " found " + shader.name);
								renderer.sharedMaterial.shader = shader;
							} else {
								Debug.Log (renderer.sharedMaterial.shader.name + " found no matching shader");
							}
						}
					}
					m_goLoadObject.SetActive (false);
					PrefabManager.Instance.Add (_strAssetName, m_goLoadObject);
				}
			}
		}
	}

}
