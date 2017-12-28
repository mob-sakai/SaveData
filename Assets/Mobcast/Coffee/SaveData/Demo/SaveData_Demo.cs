//using UnityEngine;
//using Mobcast.Coffee.SaveData;
//
///// <summary>
///// LipSaveData.
///// </summary>
//[System.Serializable]
//public class SaveData_Demo : SaveData<SaveData_Demo.Entity, SaveData_Demo>
//{
//	/// <summary>
//	/// 現在のセーブデータ.
//	/// </summary>
//	public static DetailEntity currentDetail { get{ return instance.m_CurrentDetail; } }
//	DetailEntity m_CurrentDetail = new DetailEntity();
//
//
//	[System.Serializable]
//	public class DetailEntity : Entity
//	{
//		// #### ADD THE SERIALIZED FIELDS TO BE SAVED BELOW ####
//		[SerializeField] public int m_Score;
//	}
//
//
//	[System.Serializable]
//	public class Entity : SaveDataEntity
//	{
//		// #### ADD THE SERIALIZED FIELDS TO BE SAVED BELOW ####
//		[SerializeField] public int m_Score;
//	}
//
//	/// <summary>
//	/// セーブデータ保存パス.
//	/// PlayerPrefsのキーやローカルファイルパスを指定します.
//	/// </summary>
//	public override string path { get { return "SaveData_Demo"; } }
//
//
//	protected void OnEnable()
//	{
//		onCurrentChanged += OnCurrentChanged;
//	}
//
//	protected void OnDisable()
//	{
//		onCurrentChanged -= OnCurrentChanged;
//	}
//
//	/// <summary>
//	/// ストレージへセーブデータJsonを書き込むためのコールバックです.
//	/// PlayerPrefsやローカルファイルを対象にすることができます.
//	/// </summary>
//	protected override void OnStoreJson(string json)
//	{
//		PlayerPrefs.SetString(path, json);
//		PlayerPrefs.SetString(path + "_" +current.m_UniqueId, JsonUtility.ToJson(currentDetail));
//	}
//
//	/// <summary>
//	/// ストレージからセーブデータJsonを読み込むためのコールバックです.
//	/// PlayerPrefsやローカルファイルを対象にすることができます.
//	/// </summary>
//	void OnCurrentChanged()
//	{
//		JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(path + "_" +current.m_UniqueId, "{}"), m_CurrentDetail);
//	}
//
//}