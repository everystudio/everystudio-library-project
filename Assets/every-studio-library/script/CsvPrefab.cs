using UnityEngine;
using System.Collections;

public class CsvPrefab : CsvBase<CsvPrefabData>  {

	private static readonly string FilePath = "CSV/master_load_prefab";
	public void Load() { Load(FilePath); }
}

public class CsvPrefabData : MasterBase
{
	public string filename { get; private set; }
	public int version { get; private set; }
	public int pre_load { get; private set; }
	public string path { get; private set; }
	public int del_flg { get; private set; }
}





