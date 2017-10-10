using UnityEngine;
using System.Collections.Generic;
using System;


namespace Mobcast.Coffee.SaveData
{
	/// <summary>
	/// セーブデータエンティティの基底クラス.
	/// これを継承してセーブデータに保存したい項目を追加してください.
	/// メニューより「Coffee > Save Data Editor」を選択すると、新しいセーブデータクラスをテンプレートから生成します.
	/// </summary>
	public abstract class SaveDataEntity
	{
		[SerializeField]
		public string m_UniqueId;
	}

	/// <summary>
	/// セーブデータの基底クラス.
	/// これを継承してセーブデータクラスを作成してください.
	/// 実装先でセーブデータのパスや保存方法、暗号化の有無などをカスタマイズできます.
	/// </summary>
	[System.Serializable]
	public abstract class SaveData<T, T2> : ScriptableObject where T : SaveDataEntity where T2 : SaveData<T, T2>
	{
		public static event Action<T> onCurrentChanged;
		public static event Action<List<T>> onStoredDataChanged;

		/// <summary>
		/// 現在のセーブデータ.
		/// </summary>
		public static T current { get { return instance.m_Current; } protected set { instance.m_Current = value; } }


		[SerializeField] T m_Current;

		/// <summary>
		/// セーブデータリスト.
		/// ストレージに保存済みのセーブデータリストです.
		/// </summary>
		public static List<T> list { get { return instance.m_List; } }

		[SerializeField] List<T> m_List = new List<T>();

		/// <summary>
		/// セーブデータ保存パス.
		/// PlayerPrefsのキーやローカルファイルパスを指定します.
		/// </summary>
		public abstract string path { get; }

		/// <summary>
		/// セーブデータインスタンス.
		/// </summary>
		static T2 instance
		{
			get
			{
				if (s_Instance == null)
				{
					Restore();
				}
				return s_Instance;
			}
		}

		static T2 s_Instance;


		public static void Initialize()
		{
			s_Instance = null;
			Restore();
		}


		/// <summary>
		/// 指定したセーブデータエンティティを入力します.
		/// </summary>
		public static void ImportEntity(string json)
		{
			if (string.IsNullOrEmpty(json))
				return;
			
			var data = JsonUtility.FromJson<T>(json);

			var id = list.FindIndex(x => x.m_UniqueId == data.m_UniqueId);
			if (id < 0)
			{
				list.Add(data);
			}
			else
			{
				list.RemoveAt(id);
				list.Insert(id, data);
			}
		}

		/// <summary>
		/// 指定したセーブデータエンティティを出力します.
		/// </summary>
		public static string ExportEntity(string uniqueId)
		{
			var data = list.Find(x => x.m_UniqueId == uniqueId);
			if (data == null)
				return null;

			return JsonUtility.ToJson(data, true);
		}

		/// <summary>
		/// 現在のセーブデータエンティティを、ストレージへ書き込みます.
		/// </summary>
		public static void SaveEntity()
		{
			list.RemoveAll(x => x.m_UniqueId == current.m_UniqueId);
			list.Insert(0, JsonUtility.FromJson<T>(JsonUtility.ToJson(current)));
			Store();

			if (onCurrentChanged != null)
				onCurrentChanged(current);
		}

		/// <summary>
		/// 指定したユニークIDのセーブデータエンティティを、ストレージから読み込みます.
		/// </summary>
		public static void LoadEntity(string uniqueId)
		{
			uniqueId = uniqueId ?? current.m_UniqueId;
			var data = list.Find(x => x.m_UniqueId == uniqueId);
			current = (data != null) ? JsonUtility.FromJson<T>(JsonUtility.ToJson(data)) : JsonUtility.FromJson<T>("{}");
			current.m_UniqueId = uniqueId;

			if (onCurrentChanged != null)
				onCurrentChanged(current);
		}

		/// <summary>
		/// 新しいセーブデータエンティティを作ります.
		/// </summary>
		public static void CreateEntity()
		{
			current = JsonUtility.FromJson<T>("{}");

			if (onCurrentChanged != null)
				onCurrentChanged(current);
		}

		/// <summary>
		/// 指定したユニークIDのセーブデータエンティティを、ストレージから削除します.
		/// </summary>
		public static void DeleteStoredEntity(string uniqueId = "")
		{
			list.RemoveAll(x => x.m_UniqueId == uniqueId);
			Store();
		}

		/// <summary>
		/// ストレージへセーブデータを書き込みます.
		/// </summary>
		static void Store()
		{
			instance.OnStoreJson(JsonUtility.ToJson(instance, true));

			if (onStoredDataChanged != null)
				onStoredDataChanged(list);
		}

		/// <summary>
		/// ストレージからセーブデータを読み込みます.
		/// </summary>
		static void Restore()
		{
			s_Instance = CreateInstance<T2>();
			string json = s_Instance.OnRestoreJson();
			if (!string.IsNullOrEmpty(json))
			{
				JsonUtility.FromJsonOverwrite(json, s_Instance);
			}

			if (onStoredDataChanged != null)
				onStoredDataChanged(list);
		
			if (onCurrentChanged != null)
				onCurrentChanged(current);
		}

		/// <summary>
		/// ストレージへセーブデータJsonを書き込むためのコールバックです.
		/// PlayerPrefsやローカルファイルを対象にすることができます.
		/// </summary>
		protected abstract void OnStoreJson(string json);

		/// <summary>
		/// ストレージからセーブデータJsonを読み込むためのコールバックです.
		/// PlayerPrefsやローカルファイルを対象にすることができます.
		/// </summary>
		protected abstract string OnRestoreJson();
	}
}