//----------------------------------------------
// UTAGE: Unity Text Adventure Game Engine
// Copyright 2014 Ryohei Tokimura
//----------------------------------------------
using UnityEngine;
using System.IO;

namespace Utage
{

	/// <summary>
	/// 選択肢のデータ
	/// </summary>
	public class AdvSelection
	{
		/// <summary>
		/// 表示テキスト
		/// </summary>
		public string Text { get { return this.text; } }
		string text;

		/// <summary>
		/// ジャンプ先のラベル
		/// </summary>
		public string JumpLabel { get { return this.jumpLabel; } }
		string jumpLabel;

		/// <summary>
		/// 選択時に実行する演算式
		/// </summary>
		public ExpressionParser Exp { get { return this.exp; } }
		ExpressionParser exp;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="jumpLabel">飛び先のラベル</param>
		/// <param name="text">表示テキスト</param>
		/// <param name="exp">選択時に実行する演算式</param>
		public AdvSelection(string jumpLabel, string text, ExpressionParser exp)
		{
			this.jumpLabel = jumpLabel;
			this.text = text;
			this.exp = exp;
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="reader">バイナリリーダー</param>
		public AdvSelection(BinaryReader reader)
		{
			Read(reader);
		}

		const int VERSION = 0;
		//バイナリ書き込み
		public void Write(BinaryWriter writer)
		{
			writer.Write(VERSION);
			writer.Write(this.jumpLabel);
			writer.Write(this.text );
		}
		//バイナリ読み込み
		void Read(BinaryReader reader)
		{
			int version = reader.ReadInt32();
			if (version == VERSION)
			{
				this.jumpLabel = reader.ReadString();
				this.text = reader.ReadString();
			}
			else
			{
				Debug.LogError(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.UnknownVersion, version));
			}
		}

	}
}