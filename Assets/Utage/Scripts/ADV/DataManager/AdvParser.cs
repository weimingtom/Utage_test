//----------------------------------------------
// UTAGE: Unity Text Adventure Game Engine
// Copyright 2014 Ryohei Tokimura
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utage
{

	/// <summary>
	/// ADVデータ解析
	/// </summary>
	public class AdvParser
	{
		public static string Localize(AdvColumnName name)
		{
			return LanguageAdvColumnName.LocalizeText(name);
		}
		public static T ParseCell<T>(StringGridRow row, AdvColumnName name)
		{
			return row.ParseCell<T>(Localize(name));
		}
		public static T ParseCellOptional<T>(StringGridRow row, AdvColumnName name, T defaultVal)
		{
			return row.ParseCellOptional<T>(Localize(name), defaultVal);
		}

		public static bool TryParseCell<T>(StringGridRow row, AdvColumnName name, out T val)
		{
			return row.TryParseCell<T>(Localize(name), out val);
		}
	}
}
