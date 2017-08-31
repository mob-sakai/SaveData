using UnityEngine;
using Mobcast.Coffee.SaveData;

/// <summary>
/// LipSaveData.
/// </summary>
[System.Serializable]
public class SaveData_Demo : SaveData<SaveData_Demo.Entity, SaveData_Demo>
{

	[System.Serializable]
	public class Entity : SaveDataEntity
	{
		// #### ADD THE SERIALIZED FIELDS TO BE SAVED BELOW ####
		[SerializeField] public int m_Score;
	}

	/// <summary>
	/// セーブデータ保存パス.
	/// PlayerPrefsのキーやローカルファイルパスを指定します.
	/// </summary>
	public override string path { get { return "SaveData_Demo"; } }


	/// <summary>
	/// ストレージへセーブデータJsonを書き込むためのコールバックです.
	/// PlayerPrefsやローカルファイルを対象にすることができます.
	/// </summary>
	protected override void OnStoreJson(string json)
	{
		PlayerPrefs.SetString(path, json);
	}

	/// <summary>
	/// ストレージからセーブデータJsonを読み込むためのコールバックです.
	/// PlayerPrefsやローカルファイルを対象にすることができます.
	/// </summary>
	protected override string OnRestoreJson()
	{
		return PlayerPrefs.GetString(path, "{}");
	}
}