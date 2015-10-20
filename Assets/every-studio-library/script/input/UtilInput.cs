using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UtilInput : MonoBehaviour {
	private static UtilInput instance = null;

	public static UtilInput Instance {
		get {
			if (instance == null) {
				GameObject obj = GameObject.Find ("UtilInput");
				if (obj == null) {
					obj = new GameObject("UtilInput");
					//Debug.LogError ("Not Exist AtlasManager!!");
				}
				instance = obj.GetComponent<UtilInput> ();
				if (instance == null) {
					//Debug.LogError ("Not Exist AtlasManager Script!!");
					instance = obj.AddComponent<UtilInput>() as UtilInput;
				}
				instance.initialize ();
			}
			return instance;
		}
	}
		
	private void initialize(){
		for (int i = 0; i < m_iUseDataSize; i++) {
			m_InputData.Add (new Data (i));
		}
		if (Input.touchSupported) {
			Debug.Log ("タッチ入力に対応している");
		} else {
			Debug.Log ("タッチ入力に対応してない");
			// でもUnityリモートでやってるとちゃんと動く
		}
	}

	static public float screen_width = 640.0f;
	static public float screen_height=1136.0f;

	[System.Serializable]
	public class Data{
		public int Index;
		public TouchPhase phase; 
		public bool isTouch;
		public Vector2 position;
		public Data( int _iIndex ){
			Index = _iIndex;
		}

		public void Set( TouchPhase _phase , Vector2 _pos ){
			phase = _phase;
			position = new Vector2 (
				_pos.x * UtilInput.screen_width / (float)Screen.width  ,
				_pos.y * UtilInput.screen_height / (float)Screen.height  );
		}
		public void Set( Touch _tTouch ){
			Set (_tTouch.phase, _tTouch.position);
			return;
		}
	}
	public List<Data> m_InputData = new List<Data> ();

	public int m_iUseDataSize = 2;
	void SetData( int _iIndex , TouchPhase _phase , Vector2 _pos ){
		if (_iIndex < m_InputData.Count) {
			m_InputData [_iIndex].Set (_phase,_pos);
		}
		return;
	}

	void SetData( int _iIndex , Touch _tTouch ){
		SetData (_iIndex, _tTouch.phase, _tTouch.position);
		return;
	}

	// Update is called once per frame
	void Update () {
		if (0 < Input.touches.Length) {
			foreach (Touch t in Input.touches) {
				int id = t.fingerId;
				SetData (id, t);
			}
		} else {
			Vector2 cursor = new Vector2( Input.mousePosition.x , Input.mousePosition.y ); 
			TouchPhase phase = TouchPhase.Canceled;
			if (Input.GetMouseButtonDown (0)) {  
				phase = TouchPhase.Began;
				Debug.Log ("Begin");
			} else if (Input.GetMouseButtonUp (0)) { 
				phase = TouchPhase.Ended;
			} else if (Input.GetMouseButton (0)) {
				phase = TouchPhase.Moved;
			} else {
			}
			SetData (0, phase, cursor);
			//Debug.Log (cursor);
		}
	}
}
