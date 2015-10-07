using UnityEngine;
using System.Collections;

public class Keta : MonoBehaviour {

	static public int GetKeta( int _iNum , int _iMaxKeta ){
		if (_iNum == 0) {
			// 0の場合、下記の除法でエラーとなるため1桁を固定で返す。
			return 1;
		}

		// 低い桁のチェックしてない
		int iRet = 0;
		for (int i = _iMaxKeta ; 0 <= i ; i--) {
			int sho = (int)Mathf.Pow (10.0f, i-1);// * 10;
			if (0 < _iNum / sho) {
				iRet = i;
				break;
			}
		}
		return iRet;

	}

}
