using UnityEngine;
using System.Collections;

public class MonoBehaviourEx : MonoBehaviour {

	private Transform m_myTransform;
	public Transform myTransform {
		get{ 
			if (m_myTransform == null) {
				m_myTransform = gameObject.transform;
			}
			return m_myTransform;
		}
	}

	protected void SetPos( GameObject _obj , float _fX , float _fY ){
		_obj.transform.localPosition = new Vector3( _fX , _fY , 0.0f );
		return;
	}

	protected bool m_bEndTween;
	protected void EndTween(){
		m_bEndTween = true;
		return;
	}

	// ここはUGUIを使う場合あ#ifをうまいことしてください
	#if USE_NGUI
	/**
	 * 戻り値：TweenAlpha 何も引っかからなかったらnullが返る
	 * 
	 * _goObj  親になるGameObject
	 * _fTime  TweenAlphaで変化にかかる時間
	 * _fAlpha 終了時のAlpha
	 * */
	public UISprite[] GetSpriteArr( GameObject _goRoot ){
		UISprite[] ret_arr;
		if (_goRoot) {
			ret_arr = _goRoot.GetComponentsInChildren<UISprite>();
		}
		else {
			ret_arr = null;
		}
		return ret_arr;
	}

	public void SetAlphaAll( GameObject _goRoot , float _fAlpha ){
		m_bEndTween = false;
		if (_goRoot) {
			UILabel[] label_children = _goRoot.GetComponentsInChildren<UILabel>();
			foreach (UILabel child in label_children) {
				TweenAlpha.Begin (child.gameObject, 0.0f , _fAlpha);
			}
			UISprite[] spr_children = _goRoot.GetComponentsInChildren<UISprite>();
			foreach (UISprite child in spr_children) {
				TweenAlpha.Begin (child.gameObject, 0.0f , _fAlpha);
			}
		}
		return;
	}

	public TweenAlpha TweenAlphaAll( GameObject _goRoot , float _fTime , float _fAlpha ){
		m_bEndTween = false;
		TweenAlpha tw = null;
		if (_goRoot) {
			UILabel[] label_children = _goRoot.GetComponentsInChildren<UILabel>();
			foreach (UILabel child in label_children) {
				tw = TweenAlpha.Begin (child.gameObject, _fTime , _fAlpha );
			}
			UISprite[] spr_children = _goRoot.GetComponentsInChildren<UISprite>();
			foreach (UISprite child in spr_children) {
				tw = TweenAlpha.Begin (child.gameObject, _fTime , _fAlpha );
			}
		}
		return tw;
	}

	public TweenColor TweenColorAll( GameObject _goRoot , float _fTime , Color _color ){
		m_bEndTween = false;
		TweenColor tc = null;
		if (_goRoot) {
			UILabel[] label_children = _goRoot.GetComponentsInChildren<UILabel>();
			foreach (UILabel child in label_children) {
				tc = TweenColor.Begin (child.gameObject, _fTime , _color );
			}
			UISprite[] spr_children = _goRoot.GetComponentsInChildren<UISprite>();
			foreach (UISprite child in spr_children) {
				tc = TweenColor.Begin (child.gameObject, _fTime , _color );
			}
		}
		return tc;
	}
	public bool SetText( UILabel _lbLabel , string _strMsg ){
		if( _lbLabel ){
			_lbLabel.text = _strMsg;
			return true;
		}
		return false;
	}

	public void AddDepth( GameObject _goRoot , int _iDepth ){
		UILabel[] label_children = _goRoot.GetComponentsInChildren<UILabel>();
		foreach (UILabel child in label_children) {
			child.depth += _iDepth;
		}
		UISprite[] spr_children = _goRoot.GetComponentsInChildren<UISprite>();
		foreach (UISprite child in spr_children) {
			//Debug.Log( child.gameObject.name );
			child.depth += _iDepth;
		}
		return;
	}

	public void SetLayer( GameObject _goRoot , int _iLayer ){
		if (_goRoot) {
			UILabel[] label_children = _goRoot.GetComponentsInChildren<UILabel>();
			foreach (UILabel child in label_children) {
				child.gameObject.layer = _iLayer;
			}
			UISprite[] spr_children = _goRoot.GetComponentsInChildren<UISprite>();
			foreach (UISprite child in spr_children) {
				child.gameObject.layer = _iLayer;
			}
		}
		return;
	}
	#endif

	public void SetActiveObj( GameObject _goObj , bool _bFlag ){
		if( _goObj ){
			_goObj.SetActive( _bFlag );
		}
		return;
	}

	public GameObject AddChildGameObject( string _strName , GameObject _goRoot = null ){
		GameObject retObj = new GameObject ();

		if (_goRoot == null) {
			_goRoot = this.gameObject;
		}
		retObj.transform.parent = _goRoot.transform;
		retObj.transform.localPosition = Vector3.zero;
		retObj.transform.localScale = Vector3.one;
		retObj.transform.localRotation = new Quaternion (0.0f, 0.0f, 0.0f, 0.0f);
		retObj.name = _strName;
		return retObj;
	}

	public Vector3 GetLocalPositon( GameObject _goObj ){
		return new Vector3 (_goObj.transform.localPosition.x, _goObj.transform.localPosition.y, _goObj.transform.localPosition.z);
	}
	public Vector3 GetLocalEuler( GameObject _goObj ){
		return new Vector3 (_goObj.transform.localEulerAngles.x, _goObj.transform.localEulerAngles.y, _goObj.transform.localEulerAngles.z);
	}

	public Vector3 CloneVector3( Vector3 _vec ){
		return new Vector3 (_vec.x, _vec.y, _vec.z);
	}

	protected float Linear (float _fRate, float _fStart, float _fEnd)
	{
		if (_fRate < 0) {
			_fRate = 0.0f;
		} else if (1.0f <= _fRate) {
			_fRate = 1.0f;
		} else {
			;// そのまま
		}
		float fDiv = _fEnd - _fStart;
		return _fStart + (fDiv * _fRate);
	}

	protected Vector3 LinearV3( float _fRate , Vector3 _v3Start , Vector3 _v3End ){

		float fPosX = Linear (_fRate, _v3Start.x, _v3End.x);
		float fPosY = Linear (_fRate, _v3Start.y, _v3End.y);
		float fPosZ = Linear (_fRate, _v3Start.z, _v3End.z);

		return new Vector3 (fPosX, fPosY, fPosZ);
	}

	protected void Release(GameObject _goRelease) {
		if (_goRelease ) {
			Destroy(_goRelease);
			_goRelease = null;
		}
	}



}
