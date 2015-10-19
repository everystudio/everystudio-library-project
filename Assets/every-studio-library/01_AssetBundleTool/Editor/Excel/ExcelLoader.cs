using System;
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using System.Xml.Serialization;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.Reflection;


public class ExcelLoader : EditorWindow
{

	public static void LoadExcelAndMakeCSV ()
	{
		List<Type> addList = CSMaker.ReadClass ();

		foreach (Type item in addList) {
			ReadExcel (item);
		}
	}

	private static void ReadExcel (Type _type)
	{
		string exeName = _type.Name + "ExcelLoader";
		Type exeExcelloader = Type.GetType(exeName);
		exeExcelloader.InvokeMember ("LoadExcelAndMakeCSV", BindingFlags.InvokeMethod, null, null, null);
	}

	static bool BoolSetting (int _val)
	{
		if (_val == 0) {
			return false;
		}
		return  true;
	}


}
