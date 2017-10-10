using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SaveDataEntityView_Demo : MonoBehaviour
{
	[SerializeField]Text playerName;
	[SerializeField]Text score;

	string id;

	public void SetEntity(SaveData_Demo.Entity entity)
	{
		id = entity.m_UniqueId;
		playerName.text = entity.m_UniqueId;
		score.text = entity.m_Score.ToString();
	}

	public void OnClick_Load()
	{
		SaveData_Demo.LoadEntity(id);
	}

	public void OnClick_Delete()
	{
		SaveData_Demo.DeleteStoredEntity(id);
	}

}
