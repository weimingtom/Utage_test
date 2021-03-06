//----------------------------------------------
// UTAGE: Unity Text Adventure Game Engine
// Copyright 2014 Ryohei Tokimura
//----------------------------------------------


namespace Utage
{

	/// <summary>
	/// コマンド：背景表示OFF
	/// </summary>
	internal class AdvCommandBgOff : AdvCommand
	{
		public AdvCommandBgOff(StringGridRow row)
		{
			this.fadeTime = AdvParser.ParseCellOptional<float>(row, AdvColumnName.Arg6, 0.2f);
		}

		public override void DoCommand(AdvEngine engine)
		{
			engine.LayerManager.BgOff(fadeTime);
		}

		float fadeTime;
	}
}