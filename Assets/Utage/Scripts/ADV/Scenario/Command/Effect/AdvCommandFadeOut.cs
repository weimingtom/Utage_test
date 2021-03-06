//----------------------------------------------
// UTAGE: Unity Text Adventure Game Engine
// Copyright 2014 Ryohei Tokimura
//----------------------------------------------

using UnityEngine;

namespace Utage
{

	/// <summary>
	/// コマンド：フェードアウト処理
	/// </summary>
	internal class AdvCommandFadeOut : AdvCommand
	{

		public AdvCommandFadeOut(StringGridRow row)
		{
			this.color = AdvParser.ParseCellOptional<Color>(row, AdvColumnName.Arg1, Color.white);
			this.time = AdvParser.ParseCellOptional<float>(row, AdvColumnName.Arg6, 0.2f);
		}

		public override void DoCommand(AdvEngine engine)
		{
			float fadetime = engine.Page.CheckSkip() ? time / engine.Config.SkipSpped : time;
			engine.TransitionManager.FadeOut(fadetime, color);
		}

		public override bool Wait(AdvEngine engine)
		{
			return engine.TransitionManager.IsWait;
		}

		float time;
		Color color;
	}
}