//----------------------------------------------
// UTAGE: Unity Text Adventure Game Engine
// Copyright 2014 Ryohei Tokimura
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;

namespace Utage
{

	/// <summary>
	/// シナリオのエクスポートデータ
	/// </summary>
	[System.Serializable]
	public partial class AdvScenarioDataExported : ScriptableObject
	{
		/// <summary>
		/// シナリオデータ
		/// </summary>
		public List<StringGridDictionaryKeyValue> List
		{
			get { return dictionary.List; }
		}
		[SerializeField]
		StringGridDictionary dictionary;

		/// <summary>
		/// データクリア
		/// </summary>
		public void Clear()
		{
			dictionary.Clear();
		}

		/// <summary>
		/// エクセルからデータ解析
		/// </summary>
		/// <param name="sheetName">シート名</param>
		/// <param name="grid">エクセルの1シートから作成したStringGrid</param>
		public void ParseFromExcel(string sheetName, StringGrid grid)
		{
			dictionary.Add( sheetName, grid );
		}

		/// <summary>
		/// エラーチェックのために実際にデータを作成
		/// エクスポートするときに使用。
		/// </summary>
		/// <param name="sheetName">シート名</param>
		/// <param name="grid">チェックするシナリオを記述したStringGrid</param>
		/// <param name="settingDataManger">データ管理の大本</param>
		/// <returns>シナリオデータ</returns>
		public AdvScenarioData ErrorCheck(string sheetName, StringGrid grid, AdvSettingDataManager settingDataManger)
		{
			AdvScenarioData scenario = new AdvScenarioData();
			scenario.Init( sheetName, grid, settingDataManger);
			return scenario;
		}
	}
}