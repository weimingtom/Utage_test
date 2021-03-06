//----------------------------------------------
// UTAGE: Unity Text Adventure Game Engine
// Copyright 2014 Ryohei Tokimura
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace Utage
{
	[CustomEditor(typeof(CameraManager))]
	public class CameraManagerInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			DrawProperties();
			serializedObject.ApplyModifiedProperties();
		}

		void DrawProperties()
		{
			CameraManager obj = target as CameraManager;

			UtageEditorToolKit.PropertyFieldArray(serializedObject, "cameras2D", "Cameras 2D");
			UtageEditorToolKit.PropertyField(serializedObject, "pixelsToUnits", "Pixels To Units");
			UtageEditorToolKit.PropertyFieldArray(serializedObject, "cameras3D", "Cameras 3D");

			//画面サイズ
			UtageEditorToolKit.BeginGroup("Game Screen size");
			UtageEditorToolKit.PropertyField(serializedObject, "defaultHeight", "Default Height");

			//縦長アスペクト比
			UtageEditorToolKit.BeginGroup("Nallow");
			UtageEditorToolKit.PropertyField(serializedObject, "nallowAspect", "Aspect");
			if (obj.NallowAspect == CameraManager.ASPECT.Custom)
			{
				UtageEditorToolKit.PropertyField(serializedObject, "customNallowAspect", "Custom Aspect");
			}
			UtageEditorToolKit.EndGroup();

			//基本アスペクト比
			UtageEditorToolKit.BeginGroup("Default");
			UtageEditorToolKit.PropertyField(serializedObject, "defaultAspect", "Aspect");
			if (obj.DefaultAspect == CameraManager.ASPECT.Custom)
			{
				UtageEditorToolKit.PropertyField(serializedObject, "customDefaultAspect", "Custom Aspect");
			}
			UtageEditorToolKit.EndGroup();

			//横長アスペクト比
			UtageEditorToolKit.BeginGroup("Wide");
			UtageEditorToolKit.PropertyField(serializedObject, "wideAspect", "Aspect");
			if (obj.WideAspect == CameraManager.ASPECT.Custom)
			{
				UtageEditorToolKit.PropertyField(serializedObject, "customWideAspect", "Custom Aspect");
			}
			UtageEditorToolKit.EndGroup();

			UtageEditorToolKit.PropertyFieldArray(serializedObject, "isAnchorBottom", "Is Anchor Bottom");

			UtageEditorToolKit.EndGroup();
		}
	}
}

 