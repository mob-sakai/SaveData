using UnityEngine;
using System.Collections;
using System;

namespace Mobcast.Coffee.Saving
{
	public class SaveDataManager : MonoSingleton<SaveDataManager>
	{
		public static void LoadSummaryAll()
		{
		}

		public static void LoadDetail(string filePath)
		{
		}

		public static void Save(object data)
		{
			string filePath = "";

		}


		byte[] Serialize(object obj)
		{
			return null;
		}

		object Deserialize(byte[] data)
		{
			return null;
		}
	}

	public interface ISaveData<TSummary>
	{
		string filePath { get; }
		TSummary CloneAsSummary();
	}


	/// <summary>
	/// セーブデータエンティティの基底クラス.
	/// これを継承してセーブデータに保存したい項目を追加してください.
	/// メニューより「Coffee > Save Data Editor」を選択すると、新しいセーブデータクラスをテンプレートから生成します.
	/// </summary>
	[System.Serializable]
	public abstract class SaveData
	{
		[SerializeField] public string uniqueId;

//		public string filePath { get; set;}
//		public DateTime savedTime { get; set;}
//
//		public static SaveData LoadFromFile(string filePath)
//		{
//			this.filePath = filePath;
//			return null;
//		}
	}
}