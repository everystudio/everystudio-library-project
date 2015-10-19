using UnityEngine;
using System.Collections;

public class CsvAudio : CsvBase<CsvAudioData> 
{
	private static readonly string FilePath = "CSV/master_load_audio";
	public void Load() { Load(FilePath); }
}

public class CsvAudioData : MasterBase
{
	public string filename { get; private set; }
	public int version { get; private set; }
	public string path { get; private set; }
	public int audio_type { get; private set; }
	public int del_flg { get; private set; }
}

