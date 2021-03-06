//----------------------------------------------
// UTAGE: Unity Text Adventure Game Engine
// Copyright 2014 Ryohei Tokimura
//----------------------------------------------
using UnityEngine;

namespace Utage
{

	/// <summary>
	/// コマンド：SE再生
	/// </summary>
	internal class AdvCommandSe : AdvCommand
	{

		public AdvCommandSe(StringGridRow row, AdvSettingDataManager dataManager)
		{
			string label = AdvParser.ParseCell<string>(row, AdvColumnName.Arg1);
			if (!dataManager.SoundSetting.Contains(label, SoundType.Se))
			{
				Debug.LogError(row.ToErrorString(label + " is not contained in file setting"));
			}

			this.file = AddLoadFile(dataManager.SoundSetting.LabelToFilePath(label, SoundType.Se));
		}

		public override void DoCommand(AdvEngine engine)
		{
			engine.SoundManager.PlaySE(file);
		}

		AssetFile file;
	}
}