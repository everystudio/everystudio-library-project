using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.SerializableAttribute]
public class AudioSettingData : ScriptableObject {


	public BGMSoundList audioData = new BGMSoundList();

	public BGMSoundSrcList audioSrcData = new BGMSoundSrcList();

	[System.SerializableAttribute]
	public class BGMSoundList
	{
		public List<AudioParam> bgmParamList = new List<AudioParam>();
		public List<AudioParam> soundParamList = new List<AudioParam>();
	}

	[System.SerializableAttribute]
	public class BGMSoundSrcList
	{
		public List<AudioSrcParam> bgmSrcList = new List<AudioSrcParam>();
		public List<AudioSrcParam> soundSrcList = new List<AudioSrcParam>();
	}

	[System.SerializableAttribute]
	public class AudioParam
	{
		public int index;
		public int soundID;
		public string audioName;
	}


	[System.SerializableAttribute]
	public class AudioSrcParam
	{
		public AudioClip audioClip;
	}

}
