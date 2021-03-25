using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class SaveAssetBundles : EditorWindow {

	public const string MODS_FOLDER_PATH = "D:/games/new/steamapps/common/Clone Drone in the Danger Zone/mods/";
	public const string ASSET_BUNDLES_FOLDER_NAME = "AssetBundles";

	[MenuItem("Window/Save AssetBundles")]
	public static void Open(){

		string AssetBundlesFolderPath = Application.dataPath + "/" + ASSET_BUNDLES_FOLDER_NAME;
		
		Debug.Log("Deleting old files...");
		string[] oldFiles = Directory.GetFiles(AssetBundlesFolderPath);
		for (int i = 0; i < oldFiles.Length; i++)
		{
			File.Delete(oldFiles[i]);
		}

		BuildPipeline.BuildAssetBundles ("Assets/" + ASSET_BUNDLES_FOLDER_NAME + "/", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
		AssetDatabase.Refresh();
		
		List<string> actualFiles = new List<string>();
		string[] newFiles = Directory.GetFiles(AssetBundlesFolderPath);
		for (int i = 0; i < newFiles.Length; i++)
		{
			if (Path.GetFileName(newFiles[i]).Split(".".ToCharArray()).Length != 1)
				continue;

			actualFiles.Add(newFiles[i]);
		}

		string[] folders = Directory.GetDirectories(MODS_FOLDER_PATH);

		foreach (string fileToCopy in actualFiles)
		{
			string fileName = Path.GetFileName(fileToCopy).ToLower();

			foreach (string folder in folders)
			{
				string folderName = Path.GetFileName(folder).ToLower();
				if (folderName == fileName)
                {
					string dest = folder + "/" + fileName;

					File.Copy(fileToCopy, dest, true);

					Debug.Log("Copied \"" + fileToCopy + "\" to \"" + dest + "\".");

					break;
				}
			}
		}

        foreach (string folder in folders)
        {
			string folderName = Path.GetFileName(folder);

			//if (folderName.ToLower() )

        }

	}
}
