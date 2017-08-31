//using UnityEngine;
//using Mobcast.Coffee.SaveData;
//
///// <summary>
///// NewSaveData.
///// </summary>
//[System.Serializable]
//public class NewSaveData : SaveData<NewSaveData.Entity, NewSaveData>
//{
//	[System.Serializable]
//	public class Entity : SaveDataEntity
//	{
//		// #### ADD THE SERIALIZED FIELDS TO BE SAVED BELOW ####
//		[SerializeField] public int m_SampleValue;
//	}
//
//	/// <summary>
//	/// セーブデータ保存パス.
//	/// PlayerPrefsのキーやローカルファイルパスを指定します.
//	/// </summary>
//	public override string path { get { return "NewSaveData"; } }
//
//
//	/// <summary>
//	/// ストレージへセーブデータJsonを書き込むためのコールバックです.
//	/// PlayerPrefsやローカルファイルを対象にすることができます.
//	/// </summary>
//	protected override void OnStoreJson(string json)
//	{
//		PlayerPrefs.SetString(path, json);
//	}
//
//	/// <summary>
//	/// ストレージからセーブデータJsonを読み込むためのコールバックです.
//	/// PlayerPrefsやローカルファイルを対象にすることができます.
//	/// </summary>
//	protected override string OnRestoreJson()
//	{
//		return PlayerPrefs.GetString(path, "{}");
//	}
//}