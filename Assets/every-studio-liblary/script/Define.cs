using UnityEngine;
using System.Collections;

/// <summary>
/// 基本的には直接使わないようにする
/// </summary>

public abstract class Define : MonoBehaviour {

	public enum ENVIROMENT
	{
		PRODUCTION			= 0,
		LOCAL				,
		STREAMING_ASSETS	,
		MAX					,
	}

	#if UNITY_ANDROID
	public const string ASSET_BUNDLE_PREFIX             = "android";
	public const string ASSET_BUNDLES_ROOT              = "AssetBundles/Android";
	public const string S3_SERVER_HEADER = "http://ad.xnosserver.com/apps/myzoo_data/Android/assets/assetbundleresource";
	#elif UNITY_IPHONE
	public const string ASSET_BUNDLE_PREFIX             = "iphone";
	public const string ASSET_BUNDLES_ROOT              = "AssetBundles/iOS";
	public const string S3_SERVER_HEADER = "http://ad.xnosserver.com/apps/myzoo_data/iOS/assets/assetbundleresource";
	#else
	public const string ASSET_BUNDLE_PREFIX             = "iphone";
	public const string ASSET_BUNDLES_ROOT              = "AssetBundles/iOS";
	public const string S3_SERVER_HEADER = "http://ad.xnosserver.com/apps/myzoo_data/iOS/assets/assetbundleresource";
	#endif



}






























