//----------------------------------------------
// UTAGE: Unity Text Adventure Game Engine
// Copyright 2014 Ryohei Tokimura
//----------------------------------------------

using UnityEngine;
using Utage;


/// <summary>
/// ギャラリー表示のサンプル
/// </summary>
[AddComponentMenu("Utage/Examples/Gallery")]
public class UtageUiGallery : UtageUiView
{
	public UtageUiView cgGallery;
	public UtageUiView sceneGallery;
	public UtageUiView soundRoom;

	///「CGギャラリー」ボタンが押された
	void OnTapCgGallery(Button button)
	{
		Close();
		cgGallery.Open(this);
	}

	///「シーン回想」ボタンが押された
	void OnTapSceneGallery(Button button)
	{
		Close();
		sceneGallery.Open(this);
	}

	///「サウンドルーム」ボタンが押された
	void OnTapSoundRoom(Button button)
	{
		Close();
		soundRoom.Open(this);
	}
}
