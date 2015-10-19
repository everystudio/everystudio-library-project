using UnityEngine;
using System.Collections;

public class UtilRand : MonoBehaviour {

	public static int GetIndex( int [] _intParamArr ){

		int intRet = 0;

		int intParam = 0;
		for( int i = 0 ; i < _intParamArr.Length ; i++ ){
			intParam += _intParamArr[i];
		}
		int intRand = UnityEngine.Random.Range(0, intParam);

		for( intRet = 0 ; intRet < _intParamArr.Length ; intRet++ ){
			int intProb = _intParamArr[intRet];
			if( intRand < intProb ){
				break;
			}
			else {
				intRand -= intProb;
			}
		}
		return intRet;
	}

	public static int GetRand( int _iMax , int _iMin = 0 ){
		if( _iMax < 0){
			return 0;
		}

		int iRet = UnityEngine.Random.Range(_iMin,_iMax);

		return iRet;
	}

	public static float GetRange( float _fMax , float _fMin = 0.0f ){

		int iSeido = 1000;
		int iRand = GetRand ((int)_fMax * iSeido, (int)_fMin * iSeido);

		float fRet = (float)iRand / (float)iSeido;

		return fRet;

	}

}
