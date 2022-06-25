namespace CustomInspectorGeneration {
	using UnityEditor;
	using UnityEngine;
	using System.Collections;
	using System.IO;

	/// <summary>
	/// This script contains all information needed to generate an editor script
	/// </summary>

	public class ScriptInfo {

		const string templateFileName = "CustomInspectorTemplate.txt";

		string targetPath;
		string inspectorScriptName;
		string scriptName;
		public string ScriptName {
			get {
				return scriptName;
			}
		}

		string[] fields;
		public string[] Fields {
			get {
				return fields;
			}
			set {
				fields = value;
			}
		}

		// constructor
		public ScriptInfo (string scriptName, string inspectorScriptName, string targetPath, string[] fields) {
			this.scriptName = scriptName;
			this.inspectorScriptName = inspectorScriptName;
			this.targetPath = targetPath;
			this.fields = fields;
		}

		// build and save the script to file
		public void CreateScript() {			
			// create the target folder if it doesn't exist
			string targetFolderPath = Path.GetDirectoryName(targetPath);
			if (!Directory.Exists(targetFolderPath))  {
				Directory.CreateDirectory(targetFolderPath);
				AssetDatabase.Refresh(); // refresh so that the new folder will be shown
			}

			// load the template file as a text asset
			string templateAssetPath = GetTemplateFileAssetPath();
			TextAsset templateAsset = (TextAsset) AssetDatabase.LoadAssetAtPath(templateAssetPath, typeof(TextAsset));

			// create and save the inspector script
			string inspectorScriptContent = InspectorScriptBuilder.BuildInspectorScript(scriptName, inspectorScriptName, templateAsset.text, fields);
			File.WriteAllText(targetPath, inspectorScriptContent);
			AssetDatabase.Refresh();

			// select the created script
			Object createdScript = AssetDatabase.LoadAssetAtPath(targetPath, typeof(Object));
			Selection.activeObject = createdScript;			
			AssetDatabase.OpenAsset(createdScript);
			EditorGUIUtility.PingObject(createdScript);
		}

		
		// returns the path of the script template file relative to the asset folder
		static string GetTemplateFileAssetPath() {
			TemplateMarker dummy = (TemplateMarker)ScriptableObject.CreateInstance(typeof(TemplateMarker));
			MonoScript script = MonoScript.FromScriptableObject(dummy);
			string assetPath = AssetDatabase.GetAssetPath(script);
			GameObject.DestroyImmediate(dummy);
			assetPath = Path.GetDirectoryName(assetPath); // go up to folder
			assetPath += "/" + templateFileName; // add file name
			return assetPath;
		}
	}
}