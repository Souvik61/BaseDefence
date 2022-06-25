using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using CustomInspectorGeneration;

public class CustomInspectorGenerator : MonoBehaviour {

	const string csharpFileEnding = ".cs"; // file ending for csharp scripts
	const string editorScriptAppendix = "Editor"; // this is put at the end of the inspector script name, e.g. the inspector script for "TestScript" becomes "TestScriptEditor"


	// add menu item
	[MenuItem("Edit/Generate Custom Inspector %I")]
	// go through selection and generate inspector for selected items
	static void GenerateCustomInspectorFromSelection () {
		Object[] selectedObjects = Selection.objects;
		if (selectedObjects.Length == 0) Debug.LogWarning("No script selected to generate a custom inspector for. Please select a C# script.");
		foreach (Object obj in selectedObjects) {
			GenerateCustomInspector(obj);
		}
	}

	// generates a custom inspector for an object in the assets folder
	static void GenerateCustomInspector(Object obj) {
		string assetPath = AssetDatabase.GetAssetPath(obj); 
		string scriptName = obj.name;
		string inspectorScriptName = scriptName + editorScriptAppendix;
		// make sure the selected script i a c-sharp script
		if (assetPath.EndsWith(csharpFileEnding)) { 
			// make sure the selected script is not in an "Editor" folder
			if (!IsInEditorFolder(assetPath)) {
				// create the path where to save the inspector file
				string targetPath = CreateTargetPath(assetPath, inspectorScriptName);

				// make sure target file does not already exist
				if (File.Exists(targetPath)) Debug.LogError("Could not create custom inspector script '" + targetPath + "' because the script already exists. If you would like to replace this script, please delete it and try again.");
				else {
					// get the fields from the script
					string[] fields = GetFields(obj);

					// condense the info needed to build the script in an isntance of ScriptInfo
					ScriptInfo scriptInfo = new ScriptInfo(scriptName, inspectorScriptName, targetPath, fields);
					// start the window that will handle member editing
					FieldSelectionWindow.Create(scriptInfo);
				}
			}
			else {
				Debug.LogError("Can not generate custom inspector scripts for files that already are in an 'Editor' folder.");
			}
		}
		else {
			Debug.LogWarning("The selected asset '" + scriptName + "' is not a c-sharp (" + csharpFileEnding + ") file. Could not generate custom inspector.");
		}
	}


	// tells if an asset is located in an "Editor" folder from its path
	static bool IsInEditorFolder(string assetPath) {
		string dirPath = Path.GetDirectoryName(assetPath);
		DirectoryInfo dirInfo = new DirectoryInfo(AssetPathToFilePath(dirPath));
//		Debug.Log ("Folder name: " + dirInfo.Name);
		return (dirInfo.Name == "Editor");
	}

	// get the fields / members / properties
	static string[] GetFields(Object obj) {
		// list of fields to return
		List<string> foundFields = new List<string>();
		// first type gotten is type of the asset / if it's a script etc.
		System.Type objType = obj.GetType();
//		Debug.Log ("Type: " + objType);
		if (objType == typeof(MonoScript)) { // if it's a monoscript, that's what we want!
			MonoScript monoScript = (MonoScript)obj;
			System.Type scriptType = monoScript.GetClass();
//			Debug.Log ("Class of the script: " + scriptType);
			// properties
//			PropertyInfo[] properties = scriptType.GetProperties(BindingFlags.Public | BindingFlags.Instance| BindingFlags.DeclaredOnly);
//			foreach (PropertyInfo property in properties) {
//				Debug.Log ("Has property " + property.Name);
//				foundFields.Add(property.Name);
//			}
			// get fields
			FieldInfo[] fields = scriptType.GetFields(BindingFlags.Public | BindingFlags.Instance| BindingFlags.DeclaredOnly);
			foreach (FieldInfo field in fields) {
//				Debug.Log ("Has field " + field.Name);
				foundFields.Add(field.Name);
			}
			// get members
//			MemberInfo[] members = scriptType.GetMembers(BindingFlags.Public | BindingFlags.Instance| BindingFlags.DeclaredOnly);
//			foreach (MemberInfo memberInfo in members) {
//				Debug.Log ("Has member " + memberInfo.Name);
//			}
		}
		else {
			Debug.LogWarning ("The selected asset is not a MonoScript.");
		}

		return foundFields.ToArray();
	}

	// takes the path of the source script and builds the target path
	static string CreateTargetPath(string assetPath, string inspectorScriptName) {
		// path now points to directory of source script
		string targetPath = Path.GetDirectoryName(assetPath);		
		targetPath += "/Editor/"; // now to editor folder

		// add the target file name to the path of the folder it's gonna be in
		targetPath += inspectorScriptName + csharpFileEnding;
		return targetPath;
	}

	// returns the file path
	#if UNITY_EDITOR
	static string AssetPathToFilePath(string assetPath) {
		string path = Application.dataPath;
		path = Path.GetDirectoryName(path);
		path = path + "/" + assetPath;
		return path;
	}
	#endif
	
}
