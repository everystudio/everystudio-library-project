using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AssetBundleData : ScriptableObject 
{

	public List<Param> list = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		public string assetbundleName;
		public int version;
		public int type;
		public bool update;
		public int arr_index;
		public string folderName;
	}
	/*
	public void OnEnable()
	{
		Debug.LogError("生成成功 :");
	}*/

}
