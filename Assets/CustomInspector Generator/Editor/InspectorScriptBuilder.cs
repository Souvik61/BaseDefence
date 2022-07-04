namespace CustomInspectorGeneration {
	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;
	using System.Xml;
	using System.Xml.XPath;
	using System;
	using System.Text.RegularExpressions;

	/// <summary>
	/// this script takes care of creating the text file content of the inspector script, taking the template script and a list of fields to include as input
	/// </summary>

	public static class InspectorScriptBuilder {

		// definitions of xml elements that are parsed
		const string scriptNameElement = "ScriptName";
		const string editorScriptNameElement = "EditorScriptName";
		const string foreachFieldElement = "ForEachField";
		const string fieldNameElement = "FieldName";

		// builds the inspector script using the information from the parameters
		public static string BuildInspectorScript(string scriptName, string editorScriptName, string templateText, string[] fields) {
			// load the xml doc
			string fileContent = templateText;
			XmlDocument doc = new XmlDocument();
			doc.PreserveWhitespace = true;
			doc.LoadXml(fileContent);

			string rootNodeName = doc.FirstChild.Name;

			// for each node that is a "foreach" node
			XmlNodeList nodeList = doc.SelectNodes("//" + foreachFieldElement);
			foreach(XmlNode node in nodeList) {
				// get the code that will be executed for each field
				string codeForEachLine = node.InnerXml;
				string newInnerXml = "\n";

				// get a string that contains the offset tabs for this line of code
				string offsetTabs = GetTabsFromStart(codeForEachLine);
				// the next two lines get rid of superfluous newlines while preserving the tabs
				codeForEachLine = codeForEachLine.Trim();
				codeForEachLine = offsetTabs + codeForEachLine;


				foreach(string field in fields) { // then, for each field
					newInnerXml += ReplaceAllXmlElements(codeForEachLine, fieldNameElement, field); // add the code thats "individualized" for the field, without adding a new line
				}
				node.InnerXml = newInnerXml; // fill the node with that new code
			}

			// done editing the xml doc using xml tools, now string-based stuff
			
			// get the content as a string and remove anything xml from the document
			fileContent = doc.InnerXml;

			// now remove all the foreach elements from the document
			fileContent = ReplaceAllXmlElements(fileContent, foreachFieldElement, "");

			// replace the script name elements
			fileContent = ReplaceAllXmlElements(fileContent, scriptNameElement, scriptName);
			// replace the editor script name elements
			fileContent = ReplaceAllXmlElements(fileContent, editorScriptNameElement, editorScriptName);

			// remove the root nodes by removing the first and last line
			fileContent = ReplaceAllXmlElements(fileContent, rootNodeName, "");

			return fileContent;
		}

		// splits off and returns all tabs that a string starts with
		static string GetTabsFromStart (string s) {
			string orgString = s;
			string tabString = "";
//			int tabCount = 0;
			while (StartsWithTab(orgString)) {
				orgString = orgString.Substring(2);
				tabString += "\t";
//				tabCount++;
			}
//			Debug.Log ("Found " + tabCount + " tabs");
			return tabString;
		}

		// tells if a string starts with tab
		static bool StartsWithTab(string s) {
			const int tabCharLength = 2;
			if (s.Length >= tabCharLength) {
				string startPart = s.Substring(0, tabCharLength);
				return (startPart.Contains("\t"));
			}
			else return false;
		}

		// replaces all occurrences of an xml element in the file content with a specified string
		static string ReplaceAllXmlElements(string fileContent, string elementName, string replaceWith) {
//			Debug.Log ("Replacing '" + elementName + "' elements with '" + replaceWith + "'");
			// being tolerant for various xml writing ways, create a list of versions of the element that are accepted for replacing
			List<string> possibleElementNames = new List<string>();
			possibleElementNames.Add("<" + elementName + ">");
			possibleElementNames.Add("<" + elementName + "/>");
			possibleElementNames.Add("<" + elementName + " />");
			possibleElementNames.Add("</" + elementName + ">");

			// check for lines that contain only an element and replace these, so that the lines disappear
			// rebuild file content again from all the modified lines			
			string[] allLines = Regex.Split(fileContent, "\r\n|\r|\n");
			fileContent = "";
			foreach (string line in allLines) {
				if (LineIsElement(line, possibleElementNames)) { // if the line ONLY contains the searched element (and some spaces maybe), replace the whole line
//					Debug.Log ("Line '" + line + "' is ONLY the element");
					fileContent += replaceWith;
				}
				else { // otherwise, keep the old line
//					Debug.Log ("Line '" + line + "' is more than the element");
					fileContent += line;
					fileContent += Environment.NewLine;
				}

			}

			// replace any elements that may be anywhere else
			foreach (string possibleElement in possibleElementNames) {
				fileContent = fileContent.Replace(possibleElement, replaceWith);
			}

			return fileContent;
		}

		// tells if a line is "only" the element, so does not contain any more important information (blanks etc. are not considered important)
		static bool LineIsElement(string line, List<string> possibleElementNames) {
			foreach(string elementName in possibleElementNames) {
				if (line.Contains(elementName)) {
					string testLine = line;
					// remove element and all blanks and carriage returns from the line
					testLine = testLine.Replace(elementName, "");
					testLine = testLine.Replace(" ", "");
					testLine = testLine.Replace("\r", "");
					testLine = testLine.Replace("\n", "");
					testLine = testLine.Replace("\r\n", "");
					testLine = testLine.Replace("\t", "");
					// if that's all there was in it, the line is only that element!
					if (testLine == "") return true;
				}
			}
			return false;
		}

	}
}
