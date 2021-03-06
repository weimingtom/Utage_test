//----------------------------------------------
// UTAGE: Unity Text Adventure Game Engine
// Copyright 2014 Ryohei Tokimura
//----------------------------------------------
using UnityEngine;
using System.Collections;

namespace Utage
{

/// <summary>
/// コマンド：Tweenアニメーションをする
/// </summary>
	public class AdvCommandTween : AdvCommand
	{
		public AdvCommandTween(StringGridRow row, AdvSettingDataManager dataManger )
		{
			this.name = AdvParser.ParseCell<string>(row, AdvColumnName.Arg1);
			string type = AdvParser.ParseCell<string>(row, AdvColumnName.Arg2);
			string arg = AdvParser.ParseCellOptional<string>(row, AdvColumnName.Arg3, "");
			string easeType = AdvParser.ParseCellOptional<string>(row, AdvColumnName.Arg4, "");
			string loopType = AdvParser.ParseCellOptional<string>(row, AdvColumnName.Arg5, "");
			this.waitType = AdvParser.ParseCellOptional<WaitType>(row, AdvColumnName.Arg6, WaitType.Default);
			this.isComplete = false;

			this.tweenData = new iTweenData(type, arg, easeType, loopType);
			if (this.tweenData.Type == iTweenType.Stop)
			{
				waitType = WaitType.Add;
			}

			if (!string.IsNullOrEmpty(tweenData.ErrorMsg))
			{
				Debug.LogError(row.ToErrorString(tweenData.ErrorMsg));
			}
		}

		public override void DoCommand(AdvEngine engine)
		{
			if (!string.IsNullOrEmpty(tweenData.ErrorMsg))
			{
				Debug.LogError(tweenData.ErrorMsg);
				isComplete = true;
			}
			else
			{
				GameObject go = engine.LayerManager.FindGameObject(name);
				if (null == go)
				{
					isComplete = true;

					//記述ミス
					Debug.LogError(LanguageAdvErrorMsg.LocalizeTextFormat(AdvErrorMsg.NotFoundTweenGameObject,name));
				}
				else
				{
					isComplete = false;
					iTweenPlayer player = go.AddComponent<iTweenPlayer>() as iTweenPlayer;
					float skipSpeed = engine.Page.CheckSkip() ? engine.Config.SkipSpped : 0;
					player.Init( tweenData, engine.LayerManager.PixelsToUnits, skipSpeed, CallbackComplete);
					player.Play();
					if (player.IsEndlessLoop) waitType = WaitType.Add;
				}
			}
		}

		//コマンド終了待ち
		public override bool Wait(AdvEngine engine)
		{
			return (waitType == WaitType.Default) && !isComplete;
		}


		void CallbackComplete( iTweenPlayer player )
		{
			isComplete = true;
		}

		enum WaitType
		{
			Default,
			Add,
		};

		string name;
		iTweenData tweenData;
		WaitType waitType;
		bool isComplete = false;
	}
}
