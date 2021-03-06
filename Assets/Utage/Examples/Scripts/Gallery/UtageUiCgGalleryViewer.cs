//----------------------------------------------
// UTAGE: Unity Text Adventure Game Engine
// Copyright 2014 Ryohei Tokimura
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utage;

/// <summary>
/// CGギャラリー画面のサンプル
/// </summary>
[AddComponentMenu("Utage/Examples/CgGalleryViewer")]
[RequireComponent(typeof(BoxCollider2D))]
public class UtageUiCgGalleryViewer : UtageUiView
{
	/// <summary>
	/// CG表示画面
	/// </summary>
	public Sprite2D texture;
	public float pixelsToUnits = 100;

	/// <summary>ADVエンジン</summary>
	public AdvEngine Engine { get { return this.engine ?? (this.engine = FindObjectOfType<AdvEngine>() as AdvEngine); } }
	[SerializeField]
	AdvEngine engine;

	/// <summary>コライダー</summary>
	public BoxCollider2D BoxCollider2D { get { return this.boxCollider2D ?? (this.boxCollider2D = FindObjectOfType<BoxCollider2D>() as BoxCollider2D); } }
	[SerializeField]
	BoxCollider2D boxCollider2D;

	AdvCgGalleryData data;
	int cuurentIndex = 0;

	/// <summary>
	/// オープンしたときに呼ばれる
	/// </summary>
	public void Open( UtageUiView prev, AdvCgGalleryData data )
	{
		this.Open(prev);
		this.data = data;
		this.cuurentIndex = 0;
		LoadCurrentTexture();
	}

	/// <summary>
	/// クローズしたときに呼ばれる
	/// </summary>
	void OnClose()
	{
		texture.ClearTextureFile();
	}

	void Update()
	{
		//右クリックで戻る
		if (InputUtil.IsMousceRightButtonDown())
		{
			Back();
		}
	}

	void OnClick(TouchData2D touch)
	{
		++cuurentIndex;
		if (cuurentIndex >= data.NumOpen)
		{
			Back();
			return;
		}
		else
		{
			LoadCurrentTexture();
		}
	}

	void LoadCurrentTexture()
	{
		AdvTextureSettingData textureData = data.GetDataOpened(cuurentIndex);
		texture.SetTextureFile(Engine.DataManager.SettingDataManager.TextureSetting.LabelToFilePath(textureData.Key), pixelsToUnits);
	}
}
