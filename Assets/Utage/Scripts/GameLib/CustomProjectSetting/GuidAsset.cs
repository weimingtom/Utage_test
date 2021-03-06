//----------------------------------------------
// UTAGE: Unity Text Adventure Game Engine
// Copyright 2014 Ryohei Tokimura
//----------------------------------------------

using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Utage
{

	/// <summary>
	/// 表示言語切り替え用のクラス
	/// </summary>
	public class GuidAsset : ScriptableObject
	{
		public string guid;

#if UNITY_EDITOR

		/// <summary>
		/// プロジェクトで共有できるように、アセットファイルとしてGUIDを保存
		/// </summary>
		public static void Save(string guidAssetPath, Object asset)
		{
			string assetPath = AssetDatabase.GetAssetPath(asset);
			string guid = AssetDatabase.AssetPathToGUID(assetPath);

			GuidAsset guidAsset = AssetDatabase.LoadAssetAtPath(guidAssetPath, typeof(GuidAsset)) as GuidAsset;
			if (guidAsset == null)
			{
				guidAsset = ScriptableObject.CreateInstance<GuidAsset>() as GuidAsset;
				AssetDatabase.CreateAsset(guidAsset, guidAssetPath);
			}
			guidAsset.hideFlags = HideFlags.NotEditable;

			if (guidAsset.guid == guid)
			{
				//変更がないならなにもしない
				return;
			}
			else
			{
				guidAsset.guid = guid;
				EditorUtility.SetDirty(guidAsset);
			}
		}
		/// <summary>
		/// アセットファイルとして保存してあるGUIDをロード
		/// </summary>
		public static Object Load(string guidAssetPath)
		{
			GuidAsset guidAsset = AssetDatabase.LoadAssetAtPath(guidAssetPath, typeof(GuidAsset)) as GuidAsset;
			if (guidAsset != null)
			{
				string assetPath = AssetDatabase.GUIDToAssetPath(guidAsset.guid);
				return AssetDatabase.LoadAssetAtPath(assetPath, typeof(UnityEngine.Object)) as UnityEngine.Object;
			}
			else
			{
				return null;
			}
		}
#else
		/// <summary>
		/// プロジェクトで共有できるように、アセットファイルとしてGUIDを保存
		/// </summary>
		public static void Save(string path, Object asset)
		{
			//非エディター環境ではなにもしない
			return;
		}
		/// <summary>
		/// アセットファイルとして保存してあるGUIDをロード
		/// </summary>
		public static Object Load(string path)
		{
			//非エディター環境ではnull
			return null;
		}

#endif
	}
}
