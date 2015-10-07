using UnityEngine;
using System.Collections;

#if USE_NGUI
[RequireComponent (typeof(UISprite))]
#else
#endif
public class CtrlDispNumber : MonoBehaviourEx
{


	string m_strHeadName;
	#if USE_NGUI
	UISprite m_sprImage;
	#endif

	#if USE_NGUI
	public void Initialize (string _strHead, UIAtlas _atlas, float _fWidth, float _fHeight)
	{
		m_strHeadName = _strHead;
		m_sprImage = GetComponent<UISprite> ();
		m_sprImage.atlas = _atlas;
		m_sprImage.spriteName = _strHead + "0";

		m_sprImage.width = (int)_fWidth;
		m_sprImage.height = (int)_fHeight;

		return;
	}

	public void SetNum (int _iNum)
	{
		if (_iNum < -1 ) {
			m_sprImage.enabled = false;
		} else if (10 <= _iNum) {
			m_sprImage.enabled = false;
		} else {
			m_sprImage.enabled = true;

			m_sprImage.spriteName = m_strHeadName + _iNum.ToString ();
		}

		return;
	}

	public void SetDepth(int _depth)
	{
		m_sprImage.depth = _depth;

		return;
	}


	public void SetColor(Color _color)
	{
		m_sprImage.color = _color;

		return;
	}
	#endif
}
