using UnityEditor;
using UnityEngine;
using System;
using System.Collections;


public class DatabaseAssetEditor : ScriptableObject {
	[Range(0,30)]
	public int number = 10;
	public bool toggle = false;

	public const string FILE_PATH = "Assets/every-studio-library/90_Example/database_asset/Resources/ExampleAsset.asset";

	[MenuItem ("Example/Load ExampleAsset")]
	static void LoadExampleAsset ()
	{
		var exampleAsset = AssetDatabase.LoadAssetAtPath<DataSample>(FILE_PATH);

	}
	[MenuItem ("Example/Create ExampleAsset")]
	static void CreateExampleAsset ()
	{
		/*
		var exampleAsset = CreateInstance<DatabaseAssetEditor> ();
		AssetDatabase.CreateAsset (exampleAsset, FILE_PATH);
		AssetDatabase.Refresh ();
		*/

		DataSample resources_data = CreateInstance<DataSample> ();
		AssetDatabase.CreateAsset (resources_data, FILE_PATH);
		AssetDatabase.Refresh ();

	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
