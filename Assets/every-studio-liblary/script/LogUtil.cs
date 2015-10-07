using UnityEngine;
using System.Collections;

public class LogUtil : MonoBehaviour {

	private static string LOG_TAG = "nekopokkuru :"; 

	private static bool isWriteLog = false; 
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static void d(string message){
		if(!isWriteLog){
			return;
		}
		Debug.Log(LOG_TAG + message);
	}

	public static void e(string message){
		if(!isWriteLog){
			return; 
		}
		Debug.LogError(LOG_TAG + message);
	}
}
