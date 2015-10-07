using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class Textreader : MonoBehaviour {

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

		FileInfo fi = new FileInfo(Application.dataPath+"/test.txt");
		//write
		StreamWriter sw = fi.AppendText();
		//sw.Write(text);      // 未改行
		sw.WriteLine(text);        // 改行
		sw.Flush();
		sw.Close(); 
	}

	// 読み込み
	public void read () {
		FileInfo fi = new FileInfo(Application.dataPath+"/test.txt");
		StreamReader sr = new StreamReader(fi.OpenRead());
		while( sr.Peek() != -1 ){
			print( sr.ReadLine() );
		}
		sr.Close();
	}
}

