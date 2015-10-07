using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;


public class CSVLoader : EditorWindow
{
	public static void LoadCSVAndMakeExcel ()
	{
		List<Type> addList = CSMaker.ReadClass ();

		foreach (Type item in addList) {
			ReadCSV (item);
		}
	}

	private static void ReadCSV (Type _type)
	{
		string exeName = _type.Name + "CSVLoader";
		Type exeExcelloader = Type.GetType (exeName);
		exeExcelloader.InvokeMember ("LoadCSVAndMakeExcel", BindingFlags.InvokeMethod, null, null, null);
	}




}