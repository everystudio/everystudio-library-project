using UnityEngine;
using System.Collections;

public class SupportQuaternion : MonoBehaviour {
	public float m_fSpeed;
	public float m_fAngle;
	public Vector3 m_vec3Axis;

	public Quaternion m_saveQuaternion;

	public bool m_bMove = false;
	public bool m_bSave = false;

	public void SaveQuaternion( Quaternion _save ){
		m_saveQuaternion = transform.rotation;
	}

	static public Quaternion MakeQuaternion( float _fAngle , Vector3 _v3Axis , Quaternion _qtBase ){
		Quaternion qtRet = Quaternion.AngleAxis (_fAngle, _v3Axis);
		qtRet = qtRet * _qtBase;
		return qtRet;
	}

	// Update is called once per frame
	void Update () {
		if (m_bMove ) {
			transform.localRotation = Quaternion.AngleAxis (m_fAngle,transform.TransformVector( m_vec3Axis.x , m_vec3Axis.y , m_vec3Axis.z ));
			transform.localRotation = transform.localRotation * m_saveQuaternion;
		}

		if (m_bSave) {
			m_bSave = false;
			m_saveQuaternion = transform.localRotation;
		}
	}
}
