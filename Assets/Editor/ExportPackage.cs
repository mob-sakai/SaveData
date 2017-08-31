using UnityEditor;

namespace Mobcast.Coffee
{
	public static class ExportPackage
	{
		const string kPackageName = "SaveData.unitypackage";
		static readonly string[] kAssetPathes = {
			"Assets/Mobcast/Coffee/SaveData",
		};

		[MenuItem ("Export Package/" + kPackageName)]
		[InitializeOnLoadMethod]
		static void Export ()
		{
			if (EditorApplication.isPlayingOrWillChangePlaymode)
				return;
			
			AssetDatabase.ExportPackage (kAssetPathes, kPackageName, ExportPackageOptions.Recurse | ExportPackageOptions.Default);
			UnityEngine.Debug.Log ("Export successfully : " + kPackageName);
		}
	}
}