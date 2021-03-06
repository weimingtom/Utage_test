//----------------------------------------------
// UTAGE: Unity Text Adventure Game Engine
// Copyright 2014 Ryohei Tokimura
//----------------------------------------------

using UnityEngine;
using Utage;



/// <summary>
/// サウンドルーム用のUIのサンプル
/// </summary>
[AddComponentMenu("Utage/Examples/SoundRoomItem")]
public class UtageUiSoundRoomItem : MonoBehaviour
{
	/// <summary>本文</summary>
	public TextArea2D title;

	/// <summary>
	/// 初期化
	/// </summary>
	/// <param name="data">セーブデータ</param>
	/// <param name="index">インデックス</param>
	public void Init(AdvSoundSettingData data, int index)
	{
		title.text = data.Title;
	}
}
