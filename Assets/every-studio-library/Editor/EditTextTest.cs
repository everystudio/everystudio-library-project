using UnityEngine;
using System.Collections;
using System;
using System.IO;
using UnityEditor;

public class EditTextTest : MonoBehaviour {

	void Start () {
		Debug.Log (Application.dataPath);
		//write("Hello, world!");
		//read();
	}

	// 書き込み

	static public void Append( string _path , string _text ){



		FileInfo fi = new FileInfo(Application.dataPath+ _path );
		//write
		StreamWriter sw = fi.AppendText();
		//sw.Write(text);      // 未改行
		sw.WriteLine(_text);        // 改行
		sw.Flush();
		sw.Close(); 
	}

	static public void write(string text){

		string path = System.IO.Path.Combine (Application.persistentDataPath, "test.txt" );
		//FileInfo fi = new FileInfo(Application.dataPath+"/test.txt");
		FileInfo fi = new FileInfo(path);

		//write
		StreamWriter sw = fi.AppendText();
		//sw.Write(text);      // 未改行
		sw.WriteLine(text);        // 改行
		sw.Flush();
		sw.Close(); 
	}

	[MenuItem ("Tools/EditTextTest/SampleCall")]
	static public void SampleCall(){
		string path = System.IO.Path.Combine (Application.persistentDataPath, "test.txt" );

		Debug.Log( path );
		bool bFileCtrlError = false;
		if (!System.IO.File.Exists (path)) {
			Debug.Log ("no exist");
			write ("test");
		} else {
			Debug.Log ("exist!!");
		}
	}

	public class SampleTalbe
	{
		public string prefecture_code;
		public int area_id;
		public int sub_area_id;
		public int large_category_type;
		public int category_type;
		public int purpose_id;
		public int purpose2_id;
		public int station_id;
		public int landmark_id;
		public int large_keyword_menu_id;
		public int keyword_menu_id;

		public SampleTalbe(){
		}
	}
	static public void write2( int _test_type , SampleTalbe _data){

		string path = System.IO.Path.Combine (Application.dataPath, "out.txt" );
		//FileInfo fi = new FileInfo(Application.dataPath+"/test.txt");
		FileInfo fi = new FileInfo(path);

		//write
		StreamWriter sw = fi.AppendText();
		//sw.Write(text);      // 未改行
		sw.WriteLine(string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}),",
			_test_type,
			_data.prefecture_code,
			_data.area_id,
			_data.sub_area_id,
			_data.large_category_type,
			_data.category_type,
			_data.purpose_id,
			_data.purpose2_id,
			_data.station_id,
			_data.landmark_id,
			_data.large_keyword_menu_id,
			_data.keyword_menu_id
		));        // 改行
		sw.Flush();
		sw.Close(); 
	}

	[MenuItem ("Tools/EditTextTest/SampleRead")]
	static public void SampleRead(){

		string write_path = System.IO.Path.Combine (Application.dataPath, "out.txt" );
		Debug.Log (write_path);
		if (System.IO.File.Exists (write_path)) {
			Debug.LogError ("delete");
			System.IO.File.Delete (write_path);
		}

		int test_type = 0;

		FileInfo fi = new FileInfo(Application.dataPath+"/test.txt");
		StreamReader sr = new StreamReader(fi.OpenRead());
		while( sr.Peek() != -1 ){

			string line_data = sr.ReadLine ();

			Debug.Log( line_data);

			if (line_data.Contains ("test_type") == true) {
				test_type = int.Parse (line_data.Replace ("test_type=" , "" ));
				continue;
			}

			int purpose_count = 0;
			//Debug.Log (line_data.Contains ("/"));

			if (line_data.Contains ("/") == false) {
				continue;
			}
			string[] stArrayData = line_data.Split('/');

			SampleTalbe write_table = new SampleTalbe ();

			foreach (string div in stArrayData) {
				Debug.Log (div);
				if( div.Contains( "PRE" ) == true ){
					write_table.prefecture_code = div.Replace( "PRE" , "" );
					Debug.LogError (write_table.prefecture_code);
				}
				else if( div.Contains( "ARE" ) == true ){
					write_table.area_id = int.Parse (div.Replace("ARE",""));
				}
				else if( div.Contains( "SUB" ) == true ){
					write_table.sub_area_id = int.Parse (div.Replace("SUB",""));
				}
				else if( div.Contains( "LCAT" ) == true ){
					Debug.LogError (div.Replace ("LCAT", ""));
					write_table.large_category_type = int.Parse (div.Replace("LCAT",""));
				}
				else if( div.Contains( "CAT" ) == true ){
					write_table.category_type = int.Parse (div.Replace("CAT",""));
				}
				else if( div.Contains( "PUR" ) == true ){
					if (purpose_count == 0) {
						write_table.purpose_id = int.Parse (div.Replace ("PUR", ""));
						purpose_count += 1;
					} else {
						write_table.purpose2_id = int.Parse (div.Replace ("PUR", ""));
					}
				}
				else if( div.Contains( "STAN" ) == true ){
					write_table.station_id = int.Parse (div.Replace("STAN",""));
				}
				else if( div.Contains( "LND" ) == true ){
					write_table.landmark_id = int.Parse (div.Replace("LND",""));
				}
				else if( div.Contains( "LMENU" ) == true ){
					write_table.large_keyword_menu_id = int.Parse (div.Replace("LMENU",""));
				}
				else if( div.Contains( "MENU" ) == true ){
					write_table.keyword_menu_id = int.Parse (div.Replace("MENU",""));
				}

			}
			write2 (test_type , write_table);

			//write2 (sr.ReadLine ());
		}
		sr.Close();

	}

	// 読み込み
	public void read () {
		string pathDB = System.IO.Path.Combine (Application.persistentDataPath, "test.txt" );
		Debug.Log( pathDB );
		bool bFileCtrlError = false;
		if (!System.IO.File.Exists (pathDB)) {
		}


		FileInfo fi = new FileInfo(Application.dataPath+"/test.txt");
		StreamReader sr = new StreamReader(fi.OpenRead());
		while( sr.Peek() != -1 ){
			print( sr.ReadLine() );
		}
		sr.Close();
	}
}

