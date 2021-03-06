//----------------------------------------------
// UTAGE: Unity Text Adventure Game Engine
// Copyright 2014 Ryohei Tokimura
//----------------------------------------------
using UnityEngine;

namespace Utage
{

	/// <summary>
	/// コマンド：スプライト表示
	/// </summary>
	internal class AdvCommandSprite : AdvCommand
	{
		public AdvCommandSprite(StringGridRow row, AdvSettingDataManager dataManager)
		{
			this.spriteName = AdvParser.ParseCell<string>(row, AdvColumnName.Arg1);
			string fileName = AdvParser.ParseCellOptional<string>(row, AdvColumnName.Arg2, spriteName);

			if (!dataManager.TextureSetting.ContainsLabel(fileName))
			{
				Debug.LogError(row.ToErrorString(fileName + " is not contained in file setting"));
			}

			this.file = AddLoadFile(dataManager.TextureSetting.LabelToFilePath(fileName));
			this.layerName = AdvParser.ParseCellOptional<string>(row, AdvColumnName.Arg3, "");
			if (!string.IsNullOrEmpty(layerName) && !dataManager.LayerSetting.Contains(layerName, AdvLayerSettingData.LayerType.Sprite))
			{
				Debug.LogError(row.ToErrorString( layerName + " is not contained in layer setting"));
			}

			//表示位置
			float x;
			if (AdvParser.TryParseCell<float>(row, AdvColumnName.Arg4, out x))
			{
				this.x = x;
			}
			float y;
			if (AdvParser.TryParseCell<float>(row, AdvColumnName.Arg5, out y))
			{
				this.y = y;
			}
			//フェード時間
			this.fadeTime = AdvParser.ParseCellOptional<float>(row, AdvColumnName.Arg6, 0.2f);
		}

		public override void DoCommand(AdvEngine engine)
		{
			engine.LayerManager.SetSprite(file, layerName, spriteName, x, y, fadeTime);
		}

		AssetFile file;
		string layerName;
		string spriteName;
		object x = null;
		object y = null;
		float fadeTime;

	}
}