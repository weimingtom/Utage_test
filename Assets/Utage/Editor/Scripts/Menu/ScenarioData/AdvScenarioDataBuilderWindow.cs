//----------------------------------------------
// UTAGE: Unity Text Adventure Game Engine
// Copyright 2014 Ryohei Tokimura
//----------------------------------------------

using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Utage
{

	//「Utage」のシナリオデータ用のエクセルファイルの管理エディタウイドウ
	public class AdvScenarioDataBuilderWindow : EditorWindow
	{
		static public AdvScenarioDataProject ProjectData
		{
			get	{
				if (scenarioDataProject == null)
				{
					scenarioDataProject = UtageEditorPrefs.LoadAsset<AdvScenarioDataProject>(UtageEditorPrefs.Key.AdvScenarioProject);
				}
				return scenarioDataProject;
			}
			set
			{
				if (scenarioDataProject != value)
				{
					scenarioDataProject = value;
					UtageEditorPrefs.SaveAsset(UtageEditorPrefs.Key.AdvScenarioProject, scenarioDataProject);
				}
			}
		}
		//プロジェクトデータ
		static AdvScenarioDataProject scenarioDataProject;
		//プロジェクトデータの基本パス
		const string excelProjectPath = "Assets/Utage/Editor/EditorSave/";

		/// <summary>
		/// エクセルをコンバート
		/// </summary>
		public static void Convert()
		{
			if (ProjectData == null)
			{
				Debug.LogWarning("Scenario Data Excel project is no found");
				return;
			}
			if (string.IsNullOrEmpty(ProjectData.ConvertPath))
			{
				Debug.LogWarning("Convert Path is not defined");
				return;
			}
			AdvExcelCsvConverter converter = new AdvExcelCsvConverter();
			if ( !converter.Convert(ProjectData.ConvertPath, ProjectData.ExcelPathList, ProjectData.ConvertVersion ) )
			{
				Debug.LogWarning("Convert is failed");
				return;
			}
			if (ProjectData.IsAutoUpdateVersionAfterConvert)
			{
				ProjectData.ConvertVersion = ProjectData.ConvertVersion + 1;
				EditorUtility.SetDirty(ProjectData);
			}
		}

		/// <summary>
		/// 管理対象のエクセルをインポート
		/// </summary>
		static public void Import()
		{
			AdvExcelImporter importer = new AdvExcelImporter();
			importer.Import(ProjectData.ExcelPathList);
			if (ProjectData.IsAutoConvertOnImport)
			{
				Convert();
			}
		}

		/// <summary>
		/// インポートされたアセットが管理対象ならインポート
		/// </summary>
		public static void Import( string[] importedAssets )
		{
			if (ProjectData == null)
			{
				//シナリオが設定されてないのでインポートしない
				return;
			}

			bool isContains = false;
			List<string> assetPathList = ProjectData.ExcelPathList;
			foreach (string importedAsset in importedAssets)
			{
				isContains |= assetPathList.Contains(importedAsset);
			}
			if (isContains)
			{
				Import();
			}
		}

		bool isOpenNewProject = false;
		string newProjectName = "";

		void OnGUI()
		{
			if (isOpenNewProject)
			{
				DrawNewProject();
			}
			else
			{
				DrawDefault();
			}
		}

		void DrawNewProject()
		{
			GUIStyle style = new GUIStyle();
			GUILayout.Space(4f);
			GUILayout.Label("<b>" + "Input New Project Name" + "</b>", style, GUILayout.Width(80f));
			newProjectName = GUILayout.TextField(newProjectName);

			EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(newProjectName));
			if (GUILayout.Button("Create", GUILayout.Width(80f)))
			{
				isOpenNewProject = false;
				ProjectData = UtageEditorToolKit.CreateNewUniqueAsset<AdvScenarioDataProject>(excelProjectPath + newProjectName + ".project.asset");
				Selection.activeObject = ProjectData;
			}
			EditorGUI.EndDisabledGroup();
			if (GUILayout.Button("Cancel", GUILayout.Width(80f)))
			{
				isOpenNewProject = false;
			}
		}

		void DrawDefault()
		{
			GUILayout.Space(4f);
			EditorGUILayout.BeginHorizontal();
			GUIStyle style = new GUIStyle();
			style.richText = true;
			GUILayout.Label("<b>" + "Project" + "</b>", style, GUILayout.Width(80f) );
			if (GUILayout.Button("Create New", GUILayout.Width(80f)))
			{
				isOpenNewProject = true;
			}
			EditorGUILayout.EndHorizontal();
			GUILayout.Space(4f);

			AdvScenarioDataProject project = EditorGUILayout.ObjectField("", ProjectData, typeof(AdvScenarioDataProject), false) as AdvScenarioDataProject;
			if (project != ProjectData)
			{
				ProjectData = project;
			}

			if (ProjectData != null)
			{
				DrawProject();
			}
		}

		void DrawProject()
		{
			SerializedObject serializedObject = new SerializedObject(ProjectData);
			serializedObject.Update();

			UtageEditorToolKit.BeginGroup("Import Files");
			UtageEditorToolKit.PropertyFieldArray(serializedObject, "excelList", "Excel List");
			UtageEditorToolKit.EndGroup();

			GUILayout.Space(8f);

			EditorGUI.BeginDisabledGroup(!ProjectData.IsEnableImport);
			if (GUILayout.Button("Import", GUILayout.Width(180)))
			{
				Import();
			}
			EditorGUI.EndDisabledGroup();

			GUILayout.Space(8f);

			UtageEditorToolKit.BeginGroup("Covert Setting");

			EditorGUILayout.BeginHorizontal();
			UtageEditorToolKit.PropertyField(serializedObject, "convertPath", "Output directory");
			if (GUILayout.Button("Select", GUILayout.Width(100)))
			{
				string convertPath = ProjectData.ConvertPath;
				string dir = string.IsNullOrEmpty(convertPath) ? "" : Path.GetDirectoryName(convertPath);
				string name = string.IsNullOrEmpty(convertPath) ? "" : Path.GetFileName(convertPath);
				string path = EditorUtility.SaveFolderPanel("Select folder to output", dir, name);
				Debug.Log(path);
				if (!string.IsNullOrEmpty(path))
				{
					ProjectData.ConvertPath = path;
				}
			}
			EditorGUILayout.EndHorizontal();

			UtageEditorToolKit.PropertyField(serializedObject, "convertVersion", "Version");

			UtageEditorToolKit.PropertyField(serializedObject, "isAutoUpdateVersionAfterConvert", "Auto Update Version");


			UtageEditorToolKit.EndGroup();
			GUILayout.Space(4f);

			EditorGUI.BeginDisabledGroup(!ProjectData.IsEnableConvert);
			UtageEditorToolKit.PropertyField(serializedObject, "isAutoConvertOnImport", "Auto Convert OnImport");
			if (GUILayout.Button("Convert", GUILayout.Width(180)))
			{
				Convert();
			}
			EditorGUI.EndDisabledGroup();


			serializedObject.ApplyModifiedProperties();
		}
	}
}