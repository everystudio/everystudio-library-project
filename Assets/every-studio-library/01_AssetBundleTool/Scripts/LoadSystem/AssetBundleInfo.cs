using UnityEngine;
using System.Collections;

public class AssetBundleInfo : MonoBehaviour {

	public string lastAuthor;
	public int verID;
	public string makeTime;

	public enum InfoType{
		LASTAUTHER,
		VERID,
		MAKETIME,
	}

}
