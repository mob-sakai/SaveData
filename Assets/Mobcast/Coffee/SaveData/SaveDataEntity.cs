using UnityEngine;
using System.Collections.Generic;
using System;


namespace Mobcast.Coffee.SaveData
{
	/// <summary>
	/// ローカルデータエンティティの基底クラス
	/// これを継承してローカルデータに保存したいクラスを作成する
	/// </summary>
	public abstract class SaveDataEntity
	{
		[SerializeField]
		public string m_Key;
	}

	/// <summary>
	/// ローカルデータの基底クラス
	/// これを継承してローカルデータに保存したいクラスを作成する
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

		[SerializeField] List<T> m_List;

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
		/// 現在のセーブデータエンティティを、ストレージへ書き込みます.
		/// </summary>
		public static void SaveEntity()
		{
			list.RemoveAll(x => x.m_Key == current.m_Key);
			list.Insert(0, JsonUtility.FromJson<T>(JsonUtility.ToJson(current)));
			Store();

			if (onCurrentChanged != null)
				onCurrentChanged(current);
		}

		/// <summary>
		/// 指定したキーのセーブデータエンティティを、ストレージから読み込みます.
		/// </summary>
		public static void LoadEntity(string key)
		{
			key = (key == null) ? current.m_Key : key;
			var data = list.Find(x => x.m_Key == key);
			current = (data != null) ? JsonUtility.FromJson<T>(JsonUtility.ToJson(data)) : JsonUtility.FromJson<T>("{}");
			current.m_Key = key;

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
		/// 指定したキーのセーブデータエンティティを、ストレージから削除します.
		/// </summary>
		public static void DeleteStoredEntity(string key = "")
		{
			list.RemoveAll(x => x.m_Key == key);
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