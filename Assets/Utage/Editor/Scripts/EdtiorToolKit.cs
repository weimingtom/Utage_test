//----------------------------------------------
// UTAGE: Unity Text Adventure Game Engine
// Copyright 2014 Ryohei Tokimura
//----------------------------------------------

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Utage
{

	/// <summary>
	/// 便利クラス
	/// </summary>
	public class UtageEditorToolKit
	{
		public static void BeginGroup(string label)
		{
			EditorGUILayout.BeginVertical("box");
			GUILayout.Space(4f);
			GruopLabel(label);
			GUILayout.Space(4f);
			GUILayout.BeginHorizontal();
			GUILayout.Space(12f);
			EditorGUILayout.BeginVertical();
		}

		public static void EndGroup()
		{
			EditorGUILayout.EndVertical();
			GUILayout.Space(4f);
			GUILayout.EndHorizontal();
			GUILayout.Space(4f);
			EditorGUILayout.EndVertical();
		}

		public static void GruopLabel(string label)
		{
			GUIStyle style = new GUIStyle();
			style.richText = true;
			GUILayout.Label("<b>" + label + "</b>", style);
		}

		public static void PropertyField(SerializedObject serializedObject, string propertyPath, string label, params GUILayoutOption[] options)
		{
			SerializedProperty property = serializedObject.FindProperty(propertyPath);
			if (property == null)
			{
				Debug.LogError(propertyPath + " is Not Found");
			}
			else
			{
				EditorGUILayout.PropertyField(property, new GUIContent(label), options);
			}
		}

		public static void PropertyFieldArray(SerializedObject serializedObject, string propertyPath, string label, params GUILayoutOption[] options)
		{
			SerializedProperty property = serializedObject.FindProperty(propertyPath);
			if (property == null)
			{
				Debug.LogError(propertyPath + " is Not Found");
			}
			else
			{
				EditorGUILayout.PropertyField(property, new GUIContent(label), true, options);
			}
		}

		public static T PrefabField<T>(string title, T currentPrefab) where T : Component
		{
			GameObject asset = (currentPrefab != null) ? currentPrefab.gameObject : null;
			EditorGUILayout.BeginHorizontal();

			GUILayout.Label(title);
			asset = EditorGUILayout.ObjectField(asset, typeof(GameObject), false) as GameObject;

			EditorGUILayout.EndHorizontal();

			T prefabComponent = (asset != null) ? asset.GetComponent<T>() : null;
			return prefabComponent;
		}


		//インポート後のアセット（ScriptableObject）を取得。
		//既にあったらロード。なかったらCreate
		static public T GetImportedAssetCreateIfMissing<T>(string path) where T : ScriptableObject
		{
			var asset = AssetDatabase.LoadAssetAtPath(path, typeof(T)) as T;
			if (asset == null)
			{
				asset = ScriptableObject.CreateInstance<T>() as T;
				AssetDatabase.CreateAsset(asset, path);
			}
			return asset;
		}

		static public T CreateNewUniqueAsset<T>() where T : ScriptableObject
		{
			string path = GetSelectedDirectory();
			string typeName = typeof(T).ToString();

			//ネームスペース対策
			if( typeName.Contains(".") )
			{
				int index = typeName.LastIndexOf('.') + 1;
				typeName = typeName.Substring( index, typeName.Length -index );
			}
			path += "/New " + typeName + ".asset";
			return CreateNewUniqueAsset<T>(path);
		}

		static public T CreateNewUniqueAsset<T>(string path) where T : ScriptableObject
		{
			path = AssetDatabase.GenerateUniqueAssetPath(path);
			T asset = ScriptableObject.CreateInstance<T>() as T;
			AssetDatabase.CreateAsset(asset, path);
			EditorUtility.SetDirty(asset);
			return asset;
		}

		static public void CreateDirectory(string path)
		{
			string dir = System.IO.Path.GetDirectoryName(path);
			if (System.IO.Directory.Exists(dir)) return;

			CreateDirectorySub(dir);
		}

		static void CreateDirectorySub(string dir)
		{
			if (System.IO.Directory.Exists(dir)) return;

			System.IO.DirectoryInfo paretInfo = System.IO.Directory.GetParent(dir);
			CreateDirectorySub(paretInfo.FullName);
			string parentFolder = FileUtil.GetProjectRelativePath(paretInfo.FullName);
			parentFolder = "Assets" + ( string.IsNullOrEmpty(parentFolder) ?  "" : "/" + parentFolder );
			string newFolderName = System.IO.Path.GetFileName(dir);
			string guid = AssetDatabase.CreateFolder(parentFolder,newFolderName);
		}

		static public string GenerateUniqueDirectoryName(string dir)
		{
			string path = Application.dataPath + "/" + dir;
			path = GenerateUniqueDirectoryPathSub(path);
			return path.Replace(Application.dataPath+"/","");
		}

		static string GenerateUniqueDirectoryPathSub(string path)
		{
			string uniquePath = path;
			int count = 0;
			while (System.IO.Directory.Exists(uniquePath))
			{
				uniquePath = path + (++count);
			}
			return uniquePath;
		}

		//選択中のディレクトリ名
		static public string GetSelectedDirectory()
		{
			string path = "";
			foreach (var obj in Selection.objects)
			{
				path = AssetDatabase.GetAssetPath(obj);
				if (!string.IsNullOrEmpty(path) && !System.IO.Directory.Exists(path))
				{
					path = System.IO.Path.GetDirectoryName(path);
				}

				break;
			}

			if (string.IsNullOrEmpty(path))
			{
				return "Assets";
			}

			return path;
		}

		/// <summary>
		/// アセットリストからファイルパスのリストを取得
		/// </summary>
		/// <param name="assets">アセットのリスト</param>
		/// <returns>ファイルパスのリスト</returns>
		static public List<string> AssetsToPathList( List<Object> assets )
		{
			List<string> pathList = new List<string>();
			foreach (var asset in assets)
			{
				pathList.Add(AssetDatabase.GetAssetPath(asset));
			}
			return pathList;
		}

		/// <summary>
		/// アセットの拡張子をチェック
		/// </summary>
		/// <param name="asset">アセット</param>
		/// <param name="extensions">チェックする拡張子</param>
		/// <returns>指定の拡張子があればtrue。なければfalse</returns>
		static public bool CheckAssetExtensiton(Object asset, params string[] extensions )
		{
			string path = AssetDatabase.GetAssetPath(asset);
			string ext = System.IO.Path.GetExtension(path).ToLower();
			foreach( var extexsion in extensions )
			{
				if( ext == extexsion.ToLower() )
				{
					return true;
				}
			}
			return false;
		}

		static public T LoadAssetAtPath<T>(string path) where T : Object
		{
			return AssetDatabase.LoadAssetAtPath(path, typeof(T)) as T;
		}
	}
}