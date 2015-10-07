using UnityEngine;
using System.Collections;

public class ButtonManager : ButtonBase {

	[SerializeField]
	private ButtonBase [] m_csButtonList;

	public void ResetButtonNum( int _iNum ){
		ButtonRefresh (_iNum);
		return;
	}

	public void ButtonRefresh(int _intLength ){
		//Array.Clear( m_csButtonList , 0 , m_csButtonList.Length );
		m_csButtonList = new ButtonBase[_intLength];
		return;
	}

	public void AddButtonBase(  int _intIndex , ButtonBase _csButtonBase ){
		m_csButtonList[_intIndex] = _csButtonBase;
		return;
	}
	public void AddButtonBase(  int _intIndex , GameObject _goButton ){
		ButtonBase script = ((ButtonBase)_goButton.GetComponent("ButtonBase"));
		AddButtonBase (_intIndex, script);
		return;
	}

	public void ButtonInit( int _intIndex = -1){
		// とりあえず初期化

		//		Debug.Log("ButtonInit:length=" + m_csButtonList.Length  );
		this.Index = 0;
		for( int i = 0 ; i < m_csButtonList.Length ; i++ ){
			//Debug.Log ("count=" + i);
			if (m_csButtonList [i] != null) {
				m_csButtonList [i].ButtonInit (i);
			}
		}

		TriggerClearAll();
		return;
	}

	virtual public void TriggerClearAll(){
		TriggerClear ();
		for( int i = 0 ; i < m_csButtonList.Length ; i++ ){
			if (m_csButtonList [i]) {
				m_csButtonList [i].TriggerClear ();
			}
		}
		return;
	}

	public bool ButtonPushed {
		get {
			for( int i = 0 ; i < m_csButtonList.Length ; i++ ){
				if (m_csButtonList [i] != null) {
					if (m_csButtonList [i].ButtonPushed) {
						m_bButtonClicked = true;
						m_intIndex = m_csButtonList [i].Index;
					}
				}
			}
			return m_bButtonClicked;
		}
	}

	/*
	 * 外部から押されたことにしたいとき
	*/
	public void TriggerOn( int _iIndex ){
		m_intIndex = _iIndex;
		m_bButtonClicked = true;
	}

	public void Update(){

		for( int i = 0 ; i < m_csButtonList.Length ; i++ ){
			if( m_csButtonList[i].ButtonPushed ){
				m_bButtonClicked = true;
				m_intIndex = m_csButtonList[i].Index;
			}
		}
	}

	public ButtonBase GetButtonBase( int _intIndex ){
		if( _intIndex < m_csButtonList.Length ){
			return m_csButtonList[_intIndex];
		}
		return new ButtonBase();
	}

}














