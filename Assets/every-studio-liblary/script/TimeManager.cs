using UnityEngine;
using System.Collections;
using System;

public class TimeManager : MonoBehaviour {

	private const string KEY_TODAY = "today_date";
	static public readonly string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";
	private const string DEFAULT_DATE = "0000-00-00 00:00:00";

	//private const bool USE_DEBUG_TIME = false;
	//public static string DEBUG_NOW = "2014-12-04 06:00:00";
	public bool USE_DEBUG_TIME = false;
	public int OFFSET_TIME = 0;
	public bool USE_FIX_TIME = false;
	public string DEBUG_NOW = "2014-12-04 06:00:00";

	private const int DIFF_DAY_TIME = 5;

	protected static TimeManager instance;
	public static TimeManager Instance
	{
		get
		{
			if(instance == null)
			{
				instance = (TimeManager) FindObjectOfType(typeof(TimeManager));
				if (instance == null)
				{
					GameObject obj = new GameObject();
					obj.name = "TimeManager";
					obj.AddComponent<TimeManager>();
					instance = obj.GetComponent<TimeManager>();

					Debug.LogWarning("TimeManager Instance Error");
				}
			}
 			return instance;
 		}
	}

	/**
		欲しい機能とかなんぞ？
		日付変更チェック()
		日付変更リフレッシュ
	*/

	private string m_strDateNow;
	static public DateTime GetNow(){

		DateTime retDateTime = DateTime.Now;

		if( 0 != TimeManager.Instance.OFFSET_TIME){
			retDateTime = retDateTime.AddSeconds( TimeManager.Instance.OFFSET_TIME );
		}

		if( TimeManager.Instance.USE_FIX_TIME ){
			return DateTime.Parse(TimeManager.Instance.DEBUG_NOW);
		}

		return retDateTime;
		/*
		//デバッグフラグを確認して固定値かローカル時間を返すか判別
		if(TimeManager.Instance.USE_DEBUG_TIME){
			return DateTime.Parse(TimeManager.Instance.DEBUG_NOW);
		}else{
			return DateTime.Now;
		}
		*/
	}

	static public string StrNow(){
		return GetNow().ToString(DATE_FORMAT);
	}

	static public string StrGetTime( int _iOffset = 0 ){
		DateTime retDateTime = DateTime.Now;
		retDateTime = retDateTime.AddSeconds( _iOffset );
		return retDateTime.ToString (DATE_FORMAT);
	}


	/**
		違う日になっていたら真
	*/
	public bool IsDifferentDay(){

		bool bRet = false;
/*
		string save_today = SQLDataManager.Instance.ReadSQL( KEY_TODAY );

		if( save_today == SQLDataManager.Instance.READ_ERROR_STRING){
			//Debug.Log("error_string!!");
			save_today = GetNow().ToString( DATE_FORMAT );
		}

		DateTime dtSave = MakeDateTime( save_today );
		DateTime dtNow  = GetNow();
		if( dtSave < dtNow ){
			bRet = true;
			//Debug.Log( "diff_day!!");
		}
		else {
			bRet = false;
			//Debug.Log( "same_day!!");
		}
*/
		return bRet;
	}

	public string RefreshDay(){
		// debug_date
		string strNow = GetNow().ToString( DATE_FORMAT );

		DateTime dtNow = MakeDateTime( strNow );

		// 5時以降は日付を次の日にして修正
		if( DIFF_DAY_TIME <= dtNow.Hour ){
			dtNow = dtNow.AddDays(1);
		}
		else {
			;// 日付的にはそのまま
		}

		dtNow = new DateTime( dtNow.Year , dtNow.Month , dtNow.Day , DIFF_DAY_TIME , 0 , 0 );

		string strSave = dtNow.ToString( DATE_FORMAT );
		//Debug.Log("save_new_day:" + strSave);

		//SQLDataManager.Instance.WriteSQL( KEY_TODAY , strSave );
		return strNow;
	}


	public string GetNokoriTime( string _strTimeFormat , string _strHeader = "あと "){

		TimeSpan tTimeSpan = GetDiffNow(_strTimeFormat);

		if( 0 < tTimeSpan.Days ){
			return _strHeader +(tTimeSpan.Days ) + " 日";
		}
		else if( 0 < tTimeSpan.Hours ){
			return _strHeader +(tTimeSpan.Hours ) + " 時間";
		}
		else if( 0 < tTimeSpan.Minutes  ){
			return _strHeader +(tTimeSpan.Minutes ) + " 分";
		}
		else {
			return _strHeader +(tTimeSpan.Seconds ) + " 秒";
		}

		return _strHeader + " 31 分前";

	}

	/**
		メールを受信したのがどのくらい前かを生成する関数
	*/
	public string GetMailTime( string _strTimeFormat ){

		TimeSpan tTimeSpan = GetDiffNow(_strTimeFormat);

		if( tTimeSpan.Days < 0 ){
			return (tTimeSpan.Days * -1) + " 日前";
		}
		else if( tTimeSpan.Hours < 0 ){
			return (tTimeSpan.Hours * -1) + " 時間前";
		}
		else if( tTimeSpan.Minutes < 0 ){
			return (tTimeSpan.Minutes * -1) + " 分前";
		}
		else {
			return (tTimeSpan.Seconds * -1) + " 秒前";
		}


		return "31 分前";
	}

	/**
		保存している日時を取得する
		データがない場合は、いったん保存します
	*/
	public string GetSaveDay(){
		string strRet = "test";
		/*
		string strRet = SQLDataManager.Instance.ReadSQL( KEY_TODAY );

		if( strRet == SQLDataManager.Instance.READ_ERROR_STRING ){
			strRet = RefreshDay();
		}
		*/
		return strRet;
	}

	public DateTime MakeDateTime( string _strDateString ){
		////Debug.Log(_strDateString);
		if( _strDateString == null ){
			_strDateString  = GetNow().ToString( DATE_FORMAT );
		}

		if( _strDateString == DEFAULT_DATE){
			_strDateString  = GetNow().ToString( DATE_FORMAT );
		}

		//private const string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";
		int intYear = int.Parse(_strDateString.Substring( 0 , 4 ) );
		int intMonth= int.Parse(_strDateString.Substring( 5 , 2 ) );
		int intDay  = int.Parse(_strDateString.Substring( 8 , 2 ) );
		int intHour = int.Parse(_strDateString.Substring(11 , 2 ) );
		int intMin  = int.Parse(_strDateString.Substring(14 , 2 ) );
		int intSec  = int.Parse(_strDateString.Substring(17 , 2 ) );

		return new DateTime(intYear,intMonth,intDay,intHour,intMin,intSec);
	}

	public TimeSpan GetDiff( string _strRoot , string _strCheck ){
		TimeSpan tsRet = new TimeSpan();
		////Debug.Log(_strRoot);
		////Debug.Log(_strCheck);
		DateTime check_date = MakeDateTime(_strCheck);
		DateTime root_date = MakeDateTime(_strRoot);
		////Debug.Log(check_date);
		////Debug.Log(root_date);
		tsRet = check_date - root_date;
		////Debug.Log(tsRet);
		return tsRet;
	}

	public TimeSpan GetDiffNow( string _strDateString ){

		////Debug.Log("_strDateString:"+ _strDateString);
		if( _strDateString == DEFAULT_DATE ){
			_strDateString = GetNow().ToString( DATE_FORMAT );
		}
		string strNow = GetNow().ToString( DATE_FORMAT );
		////Debug.Log("strNow:"+ strNow);
		return GetDiff( strNow , _strDateString );
	}

	//1:月曜日〜7:日曜日
	public int GetWeekDay( DateTime _date ){
		int week = (int)_date.DayOfWeek;
		if (week == 0) {
			return 7;
		}
		return week;
	}

	public bool ChkTimeNow(string _strStartDateString,string _strEndDateString) {
		DateTime start_date;
		DateTime end_date;
		DateTime now_date = GetNow();
		string strNow = now_date.ToString( DATE_FORMAT );
		string strNext = now_date.AddDays(1).ToString( DATE_FORMAT );
		if (int.Parse(_strStartDateString.Replace("：","")) > int.Parse(_strEndDateString.Replace("：",""))) {
			_strStartDateString = strNow.Substring (0, 11) + _strStartDateString;
			_strEndDateString = strNext.Substring (0, 11) + _strEndDateString;
		} else {
			_strStartDateString = strNow.Substring (0, 11) + _strStartDateString;
			_strEndDateString = strNow.Substring (0, 11) + _strEndDateString;
		}

		start_date = TimeManager.Instance.MakeDateTime (_strStartDateString);
		end_date = TimeManager.Instance.MakeDateTime (_strEndDateString);
		if (now_date < start_date || end_date < now_date) {
			return false;
		}

		return true;
	}

	static public bool IsPeriod( string _strStart , string _strEnd ){
		bool bRet = false;
		DateTime start_date_time = TimeManager.Instance.MakeDateTime(_strStart);
		DateTime end_date_time = TimeManager.Instance.MakeDateTime(_strEnd);

		DateTime now = GetNow();

		if (start_date_time < now && now < end_date_time) {
			bRet = true;
		}
		return bRet;
	}




}

































