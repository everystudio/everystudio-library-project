using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

public class BuildPostProcessor
{
	[PostProcessBuild]
	public static void OnPostProcessBuild(BuildTarget target, string xcodeProjectPath)
	{
		if(target != BuildTarget.iOS) return;
		AddUrlScheme(xcodeProjectPath, "jp.everystudio.equal");
	}

	private static void AddUrlScheme(string xcodeProjectPath, string scheme)
	{
		var plistPath = Path.Combine(xcodeProjectPath, "Info.plist");
		var info = File.ReadAllText(plistPath);
		var beforeText = "<plist version=\"1.0\">\n  <dict>";
		//"<plist version=\\\"1.0\\\">\\n  <dict>"; // どこかのバージョンから <plist version=\"1.0\">\n  <dict>になっています。対応した方使ってください。
		var afterText = string.Format("<plist version=\"1.0\">\n<dict>\n<key>CFBundleURLTypes</key><array><dict><key>CFBundleURLSchemes</key><array><string>{0}</string></array></dict></array>", scheme);

		info = info.Replace(beforeText, afterText);
		File.WriteAllText(plistPath, info);
	}
}



