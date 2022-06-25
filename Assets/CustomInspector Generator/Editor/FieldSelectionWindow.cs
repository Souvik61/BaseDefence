namespace CustomInspectorGeneration {
	using UnityEngine;
	using UnityEditor;
	using System.Collections.Generic;

	/// <summary>
	/// Member selection window.
	/// </summary>

	public class FieldSelectionWindow : EditorWindow {

		// the script info to build the script
		ScriptInfo scriptInfo;
		// list of all members in the script
		string[] fields;
		// active status for each member
		bool [] fieldActive;
		// focus control OK button on first OnGUI;
		bool firstOnGUI;

		// static method to create / open the window
		public static void Create (ScriptInfo scriptInfo) {
			// Get existing open window or if none, make a new one:
			FieldSelectionWindow window = (FieldSelectionWindow)EditorWindow.GetWindow (typeof (FieldSelectionWindow));
			window.Initialize(scriptInfo);
			window.Show();
		}

		// initialize the window
		public void Initialize(ScriptInfo scriptInfo) {
			// remember information and init variables
			this.scriptInfo = scriptInfo;
			this.fields = scriptInfo.Fields;
			fieldActive = new bool[fields.Length];

			// fields are active by default
			for (int i = 0; i < fields.Length; i++) {
				fieldActive[i] = true;
			}
			
			// visual things			
			this.titleContent = new GUIContent("Generate");
			position = CalcPositionRect(fields.Length);
			firstOnGUI = true;
		}

		// calculate the position rect for the window
		Rect CalcPositionRect(int lineCount) {
			// starting values for window dims
			const float width = 380f;
			const float baseHeight = 110f;
			// calc height of one line
			GUIStyle guiStyle = new GUIStyle();
			float lineHeight = guiStyle.CalcHeight(new GUIContent("Foo"), width);
			// assign values to rect
			Rect r = new Rect();
			r.width = width;
			r.height = baseHeight + (lineHeight * lineCount);

			// position the window somewhere more centered
			r.x = Screen.width / 2f;
			r.y = Screen.height / 2f;
			return r;
		}
		
		void OnGUI () {
			// labels
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Generating inspector script for '" + scriptInfo.ScriptName + "'");
			EditorGUILayout.Space();

			if (fields.Length > 0) {
				// label for field selection
				EditorGUILayout.LabelField("Please select the fields you would like to use in the Editor script:");
				EditorGUILayout.Space();

				// list of fields
				for (int i = 0; i < fields.Length; i++) {
					fieldActive[i] = EditorGUILayout.ToggleLeft(fields[i], fieldActive[i]);
				}
			}
			else {
				EditorGUILayout.HelpBox("The chosen script has no public fields.", MessageType.Warning);
			}
			EditorGUILayout.Space();

			// okay button
			// workaround solution to have the text be black and readable even if the button is focused
			Color rememberColor = GUI.contentColor;
			GUI.contentColor = Color.black;
			GUI.SetNextControlName("Button");
			if (GUILayout.Button("OK")) OnOKButton();
			GUI.contentColor = rememberColor;

			// focus button when window is started
			if (firstOnGUI) {
				GUI.FocusControl("Button");
				firstOnGUI = false;
			}
		}

		// when pressing the button, build the script
		void OnOKButton() {
			// update the fields in the script info
			List<string> selectedMembers = new List<string>();
			for (int i = 0; i < fields.Length; i++) {
				if (fieldActive[i]) selectedMembers.Add(fields[i]);
			}
			scriptInfo.Fields = selectedMembers.ToArray();

			// make the script info build the script
			scriptInfo.CreateScript();

			// close window
			Close();
		}
	}
}