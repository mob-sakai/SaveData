using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System;
using System.Text;
using System.IO;
using Mobcast.Coffee.Saving;

public class NewEditorTest
{

	[Test]
	public void EditorTest()
	{
		var de = new detail(){ name = "hogehoge", desc = "fugafuga" };

		Debug.Log(JsonUtility.ToJson(de));
		Debug.Log(JsonUtility.ToJson(de.CloneAsSummary()));
		Save(de);

		var sum = LoadSummary(de.filePath);
		Debug.Log(JsonUtility.ToJson(sum));

	}


	void Save(summary de)
	{
		using (var fs = new FileStream(de.filePath, FileMode.OpenOrCreate, FileAccess.Write))
		{
			var bytesSummary = Serialize(de.CloneAsSummary());
			fs.Write(BitConverter.GetBytes(bytesSummary.Length), 0, 4);
			fs.Write(bytesSummary, 0, bytesSummary.Length);

			Debug.Log(bytesSummary.Length);

			var bytesDetail = Serialize(de);
			fs.Write(bytesDetail, 0, bytesDetail.Length);
		}
	}

	summary LoadSummary(string filePath)
	{
		using (var fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read))
		{
//			
			byte[] bytesSummarySize = new byte[4];
			if (fs.Read(bytesSummarySize, 0, 4) <= 0)
				return JsonUtility.FromJson<summary>("{}");



			byte[] bytesSummary = new byte[BitConverter.ToInt32(bytesSummarySize,0)];
			fs.Read(bytesSummary, 0, bytesSummary.Length);
			return Deserialize(bytesSummary);
		}
	}

	byte[] Serialize(summary obj)
	{
		return Encoding.UTF8.GetBytes(JsonUtility.ToJson(obj));
	}

	summary Deserialize(byte[] data)
	{
		Debug.Log(Encoding.UTF8.GetString(data));
		return JsonUtility.FromJson<summary>(Encoding.UTF8.GetString(data));
	}


	[System.Serializable]
	public class summary : ISaveData<summary>
	{
		public string name;

		#region ISaveData implementation

		public summary CloneAsSummary()
		{
			return new summary()
			{
				name = this.name,
			};
		}

		public string filePath { get { return "savedata.dat"; } }

		#endregion
	}


	[System.Serializable]
	public class detail : summary
	{
		public string desc;
	}
}
