//----------------------------------------------
// UTAGE: Unity Text Adventure Game Engine
// Copyright 2014 Ryohei Tokimura
//----------------------------------------------

using UnityEngine;
using Utage;

/// <summary>
/// メッセージウィドウ処理のサンプル
/// </summary>
[RequireComponent(typeof(Node2D))]
[AddComponentMenu("Utage/ADV/UiMessageWindow")]
public class AdvUiMessageWindow : MonoBehaviour
{
	/// <summary>ADVエンジン</summary>
	[SerializeField]
	AdvEngine engine;

	/// <summary>本文テキスト</summary>
	[SerializeField]
	TextArea2D text;

	/// <summary>名前表示テキスト</summary>
	[SerializeField]
	TextArea2D nameText;

	/// <summary>ウインドウのルート</summary>
	[SerializeField]
	GameObject rootChildren;

	/// <summary>コンフィグの透明度を反映させるスプライトのルート</summary>
	[SerializeField]
	Node2D transrateMessageWindowRoot;

	/// <summary>2D表示ノード</summary>
	Node2D Node2D { get { return node2D ?? (node2D = GetComponent<Node2D>()); } }
	Node2D node2D;

	/// <summary>
	/// 初期化。スクリプトから動的に生成する場合に
	/// </summary>
	/// <param name="engine">ADVエンジン</param>
	public void InitOnCreate(AdvEngine engine, TextArea2D text, TextArea2D nameText, GameObject rootChildren, Node2D transrateMessageWindowRoot)
	{
		this.engine = engine;
		this.text = text;
		this.nameText = nameText;
		this.rootChildren = rootChildren;
		this.transrateMessageWindowRoot = transrateMessageWindowRoot;
	}

	/// <summary>
	/// ウィンドウのクローズ
	/// </summary>
	public void Close()
	{
		this.gameObject.SetActive(false);
		rootChildren.SetActive(false);
	}

	/// <summary>
	/// ウィンドウのオープン
	/// </summary>
	public void Open()
	{
		this.gameObject.SetActive(true);
		rootChildren.SetActive(false);
	}

	void Update()
	{
		if (engine.UiManager.Status == AdvUiManager.UiStatus.Default)
		{
			rootChildren.SetActive(engine.Page.IsShowingText);
			if (engine.Page.IsShowingText)
			{
				//ウィンドのアルファ値反映
				transrateMessageWindowRoot.LocalAlpha = engine.Config.MessageWindowAlpha;
				//テキスト表示の反映
				engine.Page.UpdateText();
				nameText.text = engine.Page.NameText;
				text.TextData = engine.Page.TextData;
				text.LengthOfView = engine.Page.CurrentTextLen;
			}
		}
	}


	//ウインドウ閉じるボタンが押された
	void OnTapCloseWindow()
	{
		engine.UiManager.Status = AdvUiManager.UiStatus.HideMessageWindow;
	}

	//バックログボタンが押された
	void OnTapBackLog()
	{
		engine.UiManager.Status = AdvUiManager.UiStatus.Backlog;
	}
}

