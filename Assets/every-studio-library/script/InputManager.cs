using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputManager : MonoBehaviourEx
{
	protected static InputManager instance = null;
	protected static bool m_bInitialized = false;

	public static InputManager Instance
	{
		get
		{
			// InputManagerの唯一のインスタンスを生成
			if (instance == null)
			{

				GameObject obj = GameObject.Find("InputManager");

				if (obj == null)
				{
					obj = new GameObject("InputManager");
				}
				instance = obj.GetComponent<InputManager>();
				if (instance == null)
				{
					instance = obj.AddComponent<InputManager>();
				}
				instance.initialize();
			}
			if (m_bInitialized == false)
			{
				instance.initialize();
			}
			return instance;
		}
	}
	private void initialize(){
		return;
	}

	public bool CheckInsertMedal( ref Vector2 _point ){

		bool bRet = false;
		if (InputManager.Instance.m_TouchInfo.TapON)
		{
			InputManager.Instance.m_TouchInfo.TapON = false;
			bRet = true;
		}
		_point = InputManager.Instance.m_TouchInfo.TouchPoint;

		//左側をタップした時
		//if (InputManager.Instance.m_TouchInfo.TouchPoint.x >= 300.0f) {
		//}

		return bRet;
	}





	[System.SerializableAttribute]
	public class TouchInfo
	{
		//画面タッチがあるか
		public bool TouchON;
		public bool TouchUp;
		//タッチ位置
		public Vector2 TouchPoint;
		//スワイプ中か
		public bool Swipe;
		//スワイプの方向
		public Vector2 SwipeVec;
		//スワイプの現在の加速度
		public Vector2 SwipeAdd;
		//スワイプの速度
		public float SwipeSpeed;
		//経過時間
		public float SetDeltaTime;
		//イベント発生時間
		public float EventTime;
		// 画面タップがあるか（トリガーを利用する人が下げてください）
		public bool TapON;
		//ピンチイン中かどうか
		public bool PinchIn;
		//ピンチアウト中かどうか
		public bool PinchOut;
		//ピンチインアウトの中心座標
		public Vector2 PinchPos;

		public float PinchDelta;


	}

	static public TouchInfo Info{
		get{ return Instance.m_TouchInfo; }
	}
	[SerializeField]
	private TouchInfo m_TouchInfo = new TouchInfo();
	private bool m_TouchStartFlag = false;
	private bool m_TouchUpFlag = false;
	private bool m_SwipeStartFlag = false;
	private bool m_SwipeEndFlag = false;
	private float m_TouchSetTime = 0.0f;

	//入力の更新
	public void UpdateInput()
	{
		//前回通知からの経過時間
		m_TouchInfo.SetDeltaTime = m_TouchSetTime;
		m_TouchSetTime = 0.0f;

		//タッチフラグ
		if (m_TouchUpFlag && m_TouchStartFlag == false)
		{
			m_TouchInfo.TouchON = false;
			m_TouchUpFlag = false;
		}
		if (m_TouchStartFlag)
		{
			m_TouchInfo.TouchON = true;
			m_TouchInfo.TouchUp = false;
			m_TouchStartFlag = false;
		}

		//スワイプフラグ
		if (m_SwipeEndFlag && m_SwipeStartFlag == false)
		{
			m_TouchInfo.Swipe = false;
			m_SwipeEndFlag = false;
		}
		if (m_SwipeStartFlag)
		{
			m_TouchInfo.Swipe = true;
			m_SwipeStartFlag = false;
		}

		//
		if (m_TouchInfo.Swipe == false && m_TouchInfo.PinchIn) {
			m_TouchInfo.PinchIn = false;
		}

		if (m_TouchInfo.Swipe == false && m_TouchInfo.PinchOut) {
			m_TouchInfo.PinchOut = false;
		}

	}

	void Update()
	{
		m_TouchSetTime += Time.deltaTime;
		UpdateInput();
	}

	// 使うときはUNITY_5とかに変更してください

	//==================================================================
	//	Touch Event
	//==================================================================
	void OnEnable()
	{
		#if USE_EASYTOUCH
		EasyTouch.On_TouchStart += On_TouchStart;
//			EasyTouch.On_TouchDown += On_TouchDown;
		EasyTouch.On_TouchUp += On_TouchUp;
		EasyTouch.On_SwipeStart += On_SwipeStart;
		EasyTouch.On_Swipe += On_Swipe;
		EasyTouch.On_SwipeEnd += On_SwipeEnd;

		EasyTouch.On_PinchIn += On_PinchIn;
		EasyTouch.On_PinchOut += On_PinchOut;
		EasyTouch.On_PinchEnd += On_PinchEnd;
		#endif
	}

	void OnDisable()
	{
		#if USE_EASYTOUCH
		EasyTouch.On_TouchStart -= On_TouchStart;
//			EasyTouch.On_TouchDown -= On_TouchDown;
		EasyTouch.On_TouchUp -= On_TouchUp;
		EasyTouch.On_SwipeStart -= On_SwipeStart;
		EasyTouch.On_Swipe -= On_Swipe;
		EasyTouch.On_SwipeEnd -= On_SwipeEnd;

		EasyTouch.On_PinchIn -= On_PinchIn;
		EasyTouch.On_PinchOut -= On_PinchOut;
		EasyTouch.On_PinchEnd -= On_PinchEnd;
		#endif
	}

	#region EASY TOUCH
	#if USE_EASYTOUCH
	private void On_TouchStart(Gesture gesture)
	{
		if (gesture.touchCount > 0)
		{
			m_TouchStartFlag = true;
			//タッチ位置
			m_TouchInfo.TouchPoint = gesture.position;
		}
		m_TouchInfo.TapON = true;
	}
	//		private void On_TouchDown( Gesture gesture ){
	//			if (gesture.touchCount > 0) {
	//				GameManager.Instance.dataModel.TouchON = true;
	//				GameManager.Instance.dataModel.TouchPoint = gesture.position;
	//			}
	//		}
	private void On_TouchUp(Gesture gesture)
	{
		if (gesture.touchCount == 1)
		{
			m_TouchInfo.TouchUp = true;
			m_TouchUpFlag = true;
		}
	}

	private void On_SwipeStart(Gesture gesture)
	{
		Debug.Log ("On_SwipeStart");
		//スワイプ方向リセット
		m_TouchInfo.SwipeVec = Vector2.zero;
		m_SwipeStartFlag = true;
	}

	private void On_Swipe(Gesture gesture)
	{
		//加速度
		m_TouchInfo.SwipeAdd = gesture.swipeVector;
		//タッチ位置
		m_TouchInfo.TouchPoint = gesture.position;
		//タッチ速度
		m_TouchInfo.SwipeSpeed = gesture.swipeVector.magnitude;
	}

	private void On_SwipeEnd(Gesture gesture)
	{
		Debug.Log ("On_SwipeEnd");
		m_SwipeEndFlag = true;
		if (gesture.touchCount == 1)
		{
			//スワイプ方向
			m_TouchInfo.SwipeVec = gesture.swipeVector.normalized;
		}
	}

	private void On_PinchIn( Gesture gesture )
	{

		m_TouchInfo.PinchPos = gesture.startPosition;
//		gesture.deltaPinch;
		m_TouchInfo.PinchIn = true;
		m_TouchInfo.PinchOut = false;
		m_TouchInfo.PinchDelta = gesture.deltaPinch * -1.0f;
		//Debug.Log ("pinche in  :" + gesture.deltaPinch);
	}
		
	private void On_PinchOut( Gesture gesture )
	{
		m_TouchInfo.PinchPos = gesture.startPosition;
		m_TouchInfo.PinchOut = true;
		m_TouchInfo.PinchIn = false;
		m_TouchInfo.PinchDelta = gesture.deltaPinch *  1.0f;
		//Debug.Log ("pinche Out:" + gesture.deltaPinch);
	}

	private void On_PinchEnd( Gesture gesture )
	{
		Debug.Log ("pinche End");
		m_TouchInfo.PinchOut = false;
		m_TouchInfo.PinchIn = false;
		m_TouchInfo.PinchDelta = 0.0f;
	}

	public bool IsPinch(){
		if (m_TouchInfo.PinchOut || m_TouchInfo.PinchIn) {
			return true;
		}
		return false;
	}
	#endif
	#endregion


}
















