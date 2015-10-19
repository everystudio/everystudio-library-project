using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if USE_NGUI
[RequireComponent (typeof(UIAtlas))]
#endif
public class CtrlDispNumbers : MonoBehaviourEx
{
	public enum STEP
	{
        NONE = 0,
        //INIT,
        IDLE,
		COUNT_UP,
		END,
		MAX,
	}
    public STEP m_eStep;
    public STEP m_eStepPre;
	List<GameObject> m_ObjList = new List<GameObject>();


	public int m_iNumDisp;
	public int m_iNum;
	public int m_iNumTarget;
	public float m_fTimer;
	public int m_iKeta;
	public bool m_bFill;
	public float m_fWidth;
	public float m_fHeight;
	public float m_fOffset;
	public float m_fInterval;
	public string m_strHead;

	public float m_fCountUp;

    public float m_fpos;
	#if USE_NGUI
	protected UIAtlas m_atlNumber;
	#endif
	protected CtrlDispNumber[] m_arrDispNumber;

    // Use this for initialization
    void Start () {
//		Debug.Log("Start");
//		m_eStep = STEP.NONE;
//		m_eStepPre = STEP.MAX;
    }

    // Update is called once per frame
    void Update () {

        bool bInit = false;
        if (m_eStepPre != m_eStep)
        {
            m_eStepPre = m_eStep;
            bInit = true;
            //Debug.Log(m_eStep);
        }

		switch (m_eStep) {
		case STEP.NONE:
			break;
		case STEP.IDLE:
			DiffCheck ();
			break;
		case STEP.COUNT_UP:
			if (bInit) {
				int iDiff = m_iNumTarget - m_iNumDisp;
				m_fCountUp = (float)iDiff / 60.0f;
			}
			m_iNumDisp += (int)m_fCountUp;
			if (m_iNumTarget < m_iNumDisp) {
				m_iNumDisp = m_iNumTarget;
				m_eStep = STEP.IDLE;
			}
			break;
		case STEP.MAX:
		default:
			break;
		}
    }

    /// <summary>
    /// 指定した桁数の数字を画面に表示する
    /// </summary>
    /// <param name="_iNum">表示したい数</param>
    /// <param name="_strHead">画像名</param>
    /// <param name="_iKeta">最大表示桁数</param>
    /// <param name="_bFill">0詰めにするかどうか<c>true</c> _b fill.</param>
    /// <param name="_fWidth">横幅</param>
    /// <param name="_fHeight">縦幅</param>
    /// <param name="_fOffset">数字同士の間隔（オフセット量）</param>
    /// <param name="_fInterval">カウントアップしていく間隔</param>
	#if USE_NGUI
	public void Initialize (int _iNum, string _strHead , int _iKeta, bool _bFill, float _fWidth, float _fHeight, float _fOffset, float _fInterval , UIAtlas _atlNumber = null)
	{
		foreach (GameObject obj in m_ObjList) {
			Release (obj);
		}
		m_ObjList.Clear ();

		m_iNumDisp = _iNum;
		m_iNum = _iNum;
		m_iNumTarget = _iNum;
		m_strHead = _strHead;

		m_fTimer = 0.0f;
		m_iKeta = _iKeta;
		m_bFill = _bFill;
		m_fWidth = _fWidth;
		m_fHeight = _fHeight;
		m_fOffset = _fOffset;
		m_fInterval = _fInterval;

		if (_atlNumber == null) {
			_atlNumber = GetComponent<UIAtlas> ();
		}
		m_atlNumber = _atlNumber;

		m_arrDispNumber = new CtrlDispNumber[m_iKeta];

		for (int i = 0; i < m_iKeta; i++) {
			GameObject obj = AddChildGameObject ("keta" + i.ToString ());
			obj.transform.localPosition = new Vector3 (m_fpos + (m_fWidth + m_fOffset) * i * -1, 0.0f, 0.0f);
			m_arrDispNumber [i] = obj.AddComponent<CtrlDispNumber> ();
			m_arrDispNumber [i].Initialize (m_strHead, m_atlNumber, m_fWidth, m_fHeight);
			//m_arrDispNumber [i].Initialize (m_strHead , m_atlNumber, 100.0f, 200.0f);
			m_ObjList.Add (obj);
		}
		SetNum (m_iNumTarget);
		m_eStep = STEP.IDLE;

		return;
	}
	#endif

    public void Initialize (int _iNum, string _strHead , int _iKeta, bool _bFill, float _fWidth, float _fHeight, float _fOffset, float _fInterval , string _strAtlasName){
		#if USE_NGUI
        UIAtlas atlas = null;
        //atlas = AtlasManager.Instance.GetAtlasFromAtlasName(_strAtlasName);
        Initialize(_iNum, _strHead, _iKeta, _bFill, _fWidth, _fHeight, _fOffset, _fInterval, atlas );
		#endif
        return;
    }
	public void ForceSet( int _iNum ){
		m_eStep = STEP.IDLE;

		m_iNumTarget = _iNum;
		m_iNumDisp = _iNum;
		return;
	}

	public void InitializeNumberOnly( int _iNum ){
		m_eStep = STEP.IDLE;

		m_iNumTarget = _iNum;
		m_iNumDisp = _iNum;
		return;
	}

	public void AddPos( float _fX , float _fY , float _fZ ){
		for (int i = 0; i < m_iKeta; i++) {
			Transform tf = m_arrDispNumber [i].transform;
			tf.localPosition = new Vector3 (tf.localPosition.x + _fX, tf.localPosition.y + _fY, tf.localPosition.z + _fZ);
		}
		return;
	}
        
    public void Change(int iNumTarget)
    {
		// 目指してる値より小さい場合は上書き
		if (iNumTarget < m_iNumTarget) {
			m_iNumTarget = iNumTarget;
		}
        if (m_eStep != STEP.IDLE)
        {
            return;
        }
        m_iNumTarget = iNumTarget;

        return;
    }

    /// <summary>
    /// Diffs the check.
    /// </summary>
    private void DiffCheck()
    {
        if (m_eStep != STEP.IDLE)
        {
            return;
        }
            
        if (m_iNumTarget == m_iNumDisp)
        {
            // 何もしない
            return;
        }
        else if (m_iNumTarget > m_iNumDisp)
        {
            // カウントアップ処理に遷移する
            m_eStep = STEP.COUNT_UP;
        }
        else if(m_iNumTarget < m_iNumDisp)
        {
            // 画面表示をすぐに切替える
            SetNum (m_iNumTarget);

            // 値を更新
            m_iNumDisp = m_iNumTarget;
            m_iNum = m_iNumTarget;
        }
    }

    private void CountUp()
    {
        if (m_eStep != STEP.COUNT_UP)
        {
            return;
        }
        // _fIntervalの間隔でm_iNumDispをインクリメントしていく
        StartCoroutine("Wait");
        m_eStep = STEP.IDLE;
    }
        
    private IEnumerator Wait()
    {
        m_iNumDisp++;
        SetNum (m_iNumDisp);
        yield return new WaitForSeconds(m_fInterval);
    }

	public void SetNum (int _iNum)
	{
		// 桁計算
		int iTempNum = _iNum;
		int iDiv = 10;

		bool bDisp = true;

		if (0 == iTempNum) {
			bDisp = false;
		}

		// ex)20001の場合
		// 0詰めする場合は結果020001
		// 0詰めしない場合は結果20001

		// ex)0の場合
		// 0詰めする場合は結果00000
		// 0詰めしない場合は結果0
		for (int i = 0; i < m_iKeta; i++) {
			int iDispNum = iTempNum % iDiv;

			iTempNum = iTempNum / iDiv;
			if (0 == iTempNum) {
				bDisp = false;
			}

			if (bDisp || m_bFill) {
				// そのままでOK
			} else if (0 < iDispNum) {
				;// 表示する数字がある場合はそのまま
			} else if (i == 0 && _iNum == 0) {
				;// 0桁目で数字が０の時はそのまま( to disp) 
			} else {
				iDispNum = -1;
			}
			#if USE_NGUI
			m_arrDispNumber [i].SetNum (iDispNum);
			#endif
		}
		return;
	}


	/// <summary>
	/// 描画優先度を設定する
	/// </summary>
	public void SetDepth(int _depth)
	{
		for (int i = 0; i < m_arrDispNumber.Length; i++) {
			#if USE_NGUI
			m_arrDispNumber [i].SetDepth (_depth);
			#endif
		}
		return;
	}

	/// <summary>
	/// 色を設定する
	/// </summary>
	public void SetColor(Color _color)
	{
		for (int i = 0; i < m_arrDispNumber.Length; i++) {
			#if USE_NGUI
			m_arrDispNumber [i].SetColor (_color);
			#endif
		}
		return;
	}

	[SerializeField]
	protected bool m_bButtonClicked;
	public bool ButtonPushed
	{
		get { return m_bButtonClicked;}
	}

	public void OnClickButton() {
		m_bButtonClicked = true;
		return;
	}

	public void TriggerClear(){
		m_bButtonClicked = false;
		return;
	}
}






















