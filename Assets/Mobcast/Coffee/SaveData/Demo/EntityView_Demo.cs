using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EntityView_Demo : MonoBehaviour
{
	[SerializeField]Text playerName;
	[SerializeField]Text score;

	string entityKey;

	public void SetEntity(SaveData_Demo.Entity entity)
	{
		entityKey = entity.m_Key;
		playerName.text = entity.m_Key;
		score.text = entity.m_Score.ToString();
	}

	public void OnClick_Load()
	{
		SaveData_Demo.LoadEntity(entityKey);
	}

	public void OnClick_Delete()
	{
		SaveData_Demo.DeleteStoredEntity(entityKey);
	}

}
