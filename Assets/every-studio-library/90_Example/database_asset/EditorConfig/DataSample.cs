using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class DataSample : ScriptableObject {

	public string moji;
	public int param = 10;
	public List<int> int_list = new List<int>();

	public bool toggle;

}
