//----------------------------------------------
// UTAGE: Unity Text Adventure Game Engine
// Copyright 2014 Ryohei Tokimura
//----------------------------------------------


namespace Utage
{

	/// <summary>
	/// コマンド：キャラクター＆台詞表示
	/// </summary>
	internal class AdvCommandCharacterOff : AdvCommand
	{

		public AdvCommandCharacterOff(StringGridRow row)
		{
			this.name = AdvParser.ParseCellOptional<string>(row, AdvColumnName.Arg1, "");
			//フェード時間
			this.fadeTime = AdvParser.ParseCellOptional<float>(row, AdvColumnName.Arg6, 0.2f);
		}

		public override void DoCommand(AdvEngine engine)
		{
			if (string.IsNullOrEmpty(name))
			{
				engine.LayerManager.CharacterFadeOutAll(fadeTime);
			}
			else
			{
				engine.LayerManager.CharacterFadeOut(name, fadeTime);
			}
		}

		string name;
		float fadeTime;
	}

}