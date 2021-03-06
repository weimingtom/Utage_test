//----------------------------------------------
// UTAGE: Unity Text Adventure Game Engine
// Copyright 2014 Ryohei Tokimura
//----------------------------------------------

using UnityEngine;
using System.Collections;

namespace Utage
{
	/// <summary>
	/// ボタン3つのダイアログ
	/// </summary>
	[AddComponentMenu("Utage/Lib/UI/Dialog3Button")]
	public class Dialog3Button : Dialog2Button
	{

		[SerializeField]
		protected TextArea2D button3Label;

		[SerializeField]
		protected string func3;

		/// <summary>
		/// 3ボタンダイアログをダイアログを起動
		/// </summary>
		/// <param name="text">表示テキスト</param>
		/// <param name="buttonText1">ボタン1用のテキスト</param>
		/// <param name="buttonText2">ボタン2用のテキスト</param>
		/// <param name="buttonText3">ボタン3用のテキスト</param>
		/// <param name="target">ボタンを押したときのメッセージの送り先</param>
		/// <param name="func1">ボタン1を押したときに送られるメッセージ</param>
		/// <param name="func2">ボタン2を押したときに送られるメッセージ</param>
		/// <param name="func3">ボタン3を押したときに送られるメッセージ</param>

		public void Open(string text, string buttonText1, string buttonText2, string buttonText3, GameObject target, string func1, string func2, string func3 )
		{
			button3Label.text = buttonText3;
			this.func3 = func3;
			base.Open(text, buttonText1, buttonText2, target, func1, func2 );
		}

		/// <summary>
		/// ボタン3が押された
		/// </summary>
		protected void OnTapButton3()
		{
			UtageToolKit.SafeSendMessage(target, func3);
			Close();
		}
	}

}