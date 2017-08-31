using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Linq;
using System.Reflection;


namespace Mobcast.Coffee.SaveData
{
	/// <summary>
	/// ユーザーのUUID、PlayerIDを管理します.
	/// </summary>
	public class SaveDataEditor : EditorWindow
	{
		static readonly MethodInfo miCreateScriptAssetFromTemplate = typeof(ProjectWindowUtil)
		.GetMethod("CreateScriptAssetFromTemplate", BindingFlags.Static | BindingFlags.NonPublic);
		static readonly Type s_BaseType = typeof(SaveData<,>);
		static readonly Type s_SaveDataType = AppDomain.CurrentDomain.GetAssemblies()
		.SelectMany(x => x.GetTypes())
		.Where(x => x.BaseType != null && x.BaseType.IsGenericType && x.BaseType.GetGenericTypeDefinition() == s_BaseType)
		.OrderBy(x => x.Name == "SaveData_Demo")
		.FirstOrDefault();

		static MethodInfo miSaveEntity = null;
		static MethodInfo miCreateEntity = null;
		static MethodInfo miStore = null;
		static MethodInfo miLoadEntity = null;
		static PropertyInfo piInstance = null;
		static PropertyInfo piPath = null;


		Vector2 scroll;

		SerializedObject serializedObject;

		/// <summary>
		/// ウィンドウを表示します.
		/// </summary>
		[MenuItem("Coffee/Save Data Editor")]
		static void OpenFromMenu()
		{
			GetWindow<SaveDataEditor>();
		}

		void OnEnable()
		{
			titleContent.text = "Save Data";

			if (s_SaveDataType != null)
			{
				miSaveEntity = s_SaveDataType.BaseType.GetMethod("SaveEntity");
				miCreateEntity = s_SaveDataType.BaseType.GetMethod("CreateEntity");
				miStore = s_SaveDataType.BaseType.GetMethod("Store", BindingFlags.Static | BindingFlags.NonPublic);
				miLoadEntity = s_SaveDataType.BaseType.GetMethod("LoadEntity");
				piInstance = s_SaveDataType.BaseType.GetProperty("instance", BindingFlags.Static | BindingFlags.NonPublic);
				piPath = s_SaveDataType.GetProperty("path", BindingFlags.Instance | BindingFlags.Public);
			}
		}

		/// <summary>
		/// ウィンドウを描画します.
		/// </summary>
		void OnGUI()
		{
			if (s_SaveDataType == null)
			{
				DrawCreateScriptButton("No SaveData script found.\nPlease create new Savedata script for your project.");
				return;
			}
			else if (s_SaveDataType.Name == "SaveData_Demo")
			{
				DrawCreateScriptButton("SaveData script seems demo script.\nPlease create new Savedata script for your project.");
			}


			var instance = piInstance.GetValue(null, new object[0]) as UnityEngine.ScriptableObject;
			serializedObject = new SerializedObject(instance);
			serializedObject.Update();

			// SaveData class summary.
			GUILayout.Label("[" + s_SaveDataType.Name + " Summary]", EditorStyles.boldLabel);
			GUI.enabled = false;
			EditorGUILayout.ObjectField("Script", MonoScript.FromScriptableObject(instance), typeof(MonoScript), false);
			EditorGUILayout.TextField("SaveData Path", (string)piPath.GetValue(instance, new object[0]));
			GUI.enabled = true;


			EditorGUI.BeginChangeCheck();

			//現在のユーザーデータ描画.
			GUILayout.Space(15);
			DrawCurrentData();

			//全ユーザーデータ描画.
			GUILayout.Space(15);
			DrawListData();

			//変更がある場合はデータをセーブ.
			if (EditorGUI.EndChangeCheck())
			{
				serializedObject.ApplyModifiedProperties();
				miStore.Invoke(null, new object[0]);
			}
		}

		void DrawCreateScriptButton(string message)
		{
			EditorGUILayout.HelpBox(message, MessageType.Warning);

			Rect buttonRect = EditorGUILayout.GetControlRect(false, 22);
			buttonRect.x += buttonRect.width - 200;
			buttonRect.width = 200;

			if (GUI.Button(buttonRect, new GUIContent("Create new SaveData script", EditorGUIUtility.FindTexture("vcs_document"))))
			{
				var scriptPath = EditorUtility.SaveFilePanelInProject("Create New SaveData Script", "NewSaveData", "cs", "");
				scriptPath = Path.Combine(Path.GetDirectoryName(scriptPath), Path.GetFileName(scriptPath).Replace(" ", ""));
				if (!string.IsNullOrEmpty(scriptPath))
				{
					var templatePath = AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("SaveDataTemplate t:TextAsset")[0]);
					miCreateScriptAssetFromTemplate.Invoke(null, new object[]{ scriptPath, templatePath });
				}
			}
		}

		/// <summary>
		/// 現在のデータを描画.
		/// </summary>
		void DrawCurrentData()
		{
			//現在のユーザーデータ.
			using (new GUILayout.HorizontalScope())
			{
				GUILayout.Label("[Current SaveData]", EditorStyles.boldLabel);
				GUILayout.FlexibleSpace();

				//ユーザーの保存.
				if (GUILayout.Button(new GUIContent(" Save", EditorGUIUtility.FindTexture("saveactive")), GUILayout.Height(21)))
				{
					GUIUtility.keyboardControl = 0;
					miSaveEntity.Invoke(null, new object[0]);
				}

				//新しいユーザーの作成
				Color color = GUI.color;
				GUI.color = new Color(0.6f, 1, 0.6f);
				if (GUILayout.Button(new GUIContent("New", EditorGUIUtility.FindTexture("toolbar plus")), GUILayout.Height(21)))
				{
					GUIUtility.keyboardControl = 0;
					miSaveEntity.Invoke(null, new object[0]);
					miCreateEntity.Invoke(null, new object[0]);
				}
				GUI.color = color;
			}

			var sp = serializedObject.FindProperty("m_Current");

			using (new GUILayout.HorizontalScope("helpbox"))
			{
				sp.isExpanded = true;
				EditorGUILayout.PropertyField(sp, true);
			}
		}

		/// <summary>
		/// 全データを描画.
		/// </summary>
		void DrawListData()
		{
			// ヘッダー.
			GUILayout.Label("[Stored SaveData]", EditorStyles.boldLabel);

			using (var sc = new GUILayout.ScrollViewScope(scroll))
			{
				// セーブ済みの全データを描画.
				var spList = serializedObject.FindProperty("m_List");
				for (int i = 0; i < spList.arraySize; i++)
				{
					using (new EditorGUILayout.HorizontalScope("helpbox"))
					{
						// 要素の描画.
						var spElement = spList.GetArrayElementAtIndex(i);
						spElement.isExpanded = true;
						EditorGUILayout.PropertyField(spElement, true);

						// コントロール
						using (new EditorGUILayout.VerticalScope("ProgressBarBack", GUILayout.Width(30)))
						{
							//ロードボタン.
							if (GUILayout.Button(new GUIContent(" Load", EditorGUIUtility.FindTexture("d_treeeditor.refresh")), GUILayout.Height(21)))
							{
								GUIUtility.keyboardControl = 0;
								var key = spElement.FindPropertyRelative("m_Key").stringValue;
								miLoadEntity.Invoke(null, new object[]{ key });
								//							ScriptableSaveData<T, T2>.LoadEntity(spElement.FindPropertyRelative("m_Key").stringValue);
							}

							//削除ボタン.
							Color color = GUI.color;
							GUI.color = new Color(1, 0.6f, 0.6f);
							if (GUILayout.Button(new GUIContent("  DEL ", EditorGUIUtility.FindTexture("d_treeeditor.trash")), GUILayout.Height(21)))
							{
								GUIUtility.keyboardControl = 0;
								spList.DeleteArrayElementAtIndex(i);
							}
							GUI.color = color;
						}
					}
				}
				scroll = sc.scrollPosition;
			}
		}
	}
}