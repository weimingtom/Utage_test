//----------------------------------------------
// UTAGE: Unity Text Adventure Game Engine
// Copyright 2014 Ryohei Tokimura
//----------------------------------------------


namespace Utage
{

	/// <summary>
	/// コマンド：環境音停止
	/// </summary>
	internal class AdvCommandStopAmbience : AdvCommand
	{
		public AdvCommandStopAmbience(StringGridRow row)
		{
			this.fadeTime = AdvParser.ParseCellOptional<float>(row, AdvColumnName.Arg6, 0.2f);
		}

		public override void DoCommand(AdvEngine engine)
		{
			engine.SoundManager.Stop(SoundManager.StreamType.Ambience, fadeTime);
		}

		float fadeTime;
	}
}