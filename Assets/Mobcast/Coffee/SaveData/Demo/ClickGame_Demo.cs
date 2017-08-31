using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ClickGame_Demo : MonoBehaviour
{

	[SerializeField]InputField inputName;
	[SerializeField]Text score;

	[SerializeField]ScrollRect scroll;
	[SerializeField]EntityView_Demo entityView;
	List<EntityView_Demo> views = new List<EntityView_Demo>();

	// Use this for initialization
	void Start()
	{
		entityView.gameObject.SetActive(false);

		SaveData_Demo.onCurrentChanged += OnCurrentChanged;
		SaveData_Demo.onStoredDataChanged += OnStoredDataChanged;

		SaveData_Demo.Initialize();
	}


	void OnCurrentChanged(SaveData_Demo.Entity entity)
	{
		Debug.Log(inputName);
		Debug.Log(entity);

		inputName.text = entity.m_Key ?? "";
		score.text = entity.m_Score.ToString();
		Debug.Log("Current SaveData has been changed: " + JsonUtility.ToJson(entity));
	}

	void OnStoredDataChanged(List<SaveData_Demo.Entity> entities)
	{
		views.ForEach(x => Destroy(x.gameObject));
		views.Clear();
		entities.ForEach(x =>
			{
				var newView = Object.Instantiate<EntityView_Demo>(entityView);
				newView.transform.SetParent(scroll.content);
				newView.transform.localScale = Vector3.one;
				newView.SetEntity(x);
				newView.gameObject.SetActive(true);
				views.Add(newView);
			});

		Debug.Log("StoredDatas have been changed: " + entities.Count);
	}

	public void OnClick_AddScore()
	{
		SaveData_Demo.current.m_Score++;
		score.text = SaveData_Demo.current.m_Score.ToString();
	}

	public void OnChange_Name(string name)
	{
		SaveData_Demo.current.m_Key = name;
	}

	public void OnClick_Save()
	{
		SaveData_Demo.SaveEntity();
	}

	public void OnClick_Clear()
	{
		SaveData_Demo.CreateEntity();
	}


	void OnApplicationQuit()
	{
//		DemoSaveData.SaveEntity;
	}
}
