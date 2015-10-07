using UnityEngine;
using System.Collections;

public class ButtonBase : MonoBehaviourEx {

	public void OnCollider(bool _bFlag){
		BoxCollider collider = gameObject.GetComponent<BoxCollider> ();
		if (_bFlag == true) {
			collider.enabled = true;
		} else {
			collider.enabled = false;
		}
		return;
	}

	public void ButtonInit( int _intIndex = -1){
		m_intIndex = _intIndex;
		m_bButtonClicked = false;
		return;
	}

	public void TriggerClear(){
		m_bButtonClicked = false;
		return;
	}

	[SerializeField]
	protected int m_intIndex;
	public int Index 
	{
		set { m_intIndex = value;}
		get { return m_intIndex;}
	}

	[SerializeField]
	protected bool m_bButtonClicked;
	public bool ButtonPushed
	{
		get { return m_bButtonClicked;}
	}

	public void OnClickButton(){
//		Debug.Log("ButtonPushed[" + m_intIndex + "]");
		m_bButtonClicked = true;
	}
/*
	void Update(){
		//Debug.Log("button update[" +m_intIndex+"]");
	}
*/

}






