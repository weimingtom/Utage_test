//----------------------------------------------
// UTAGE: Unity Text Adventure Game Engine
// Copyright 2014 Ryohei Tokimura
//----------------------------------------------

using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Utage
{

	/// <summary>
	/// パラメーターのデータ
	/// </summary>	
	[System.Serializable]
	public partial class AdvParamSettingData : SerializableDictionaryFileReadKeyValue
	{
		/// <summary>
		/// 型
		/// </summary>
		public enum ParamType
		{
			/// <summary>bool</summary>
			Bool,
			/// <summary>float</summary>
			Float,
			/// <summary>int</summary>
			Int,
			/// <summary>string</summary>
			String,
		};

		/// <summary>
		/// 型
		/// </summary>
		public ParamType Type { get { return this.type; } }
		[SerializeField]
		ParamType type;

		/// <summary>
		/// 値
		/// </summary>
		public object Parameter { 
			get
			{
				if (parameter==null) ParseParmeterString();
				return this.parameter; 
			}
			set
			{
				switch (type)
				{
					case ParamType.Bool:
						this.parameter = (bool)value;
						break;
					case ParamType.Float:
						this.parameter = (float)value;
						break;
					case ParamType.Int:
						this.parameter = (int)value;
						break;
					case ParamType.String:
						this.parameter = (string)value;
						break;
				}
				parameterString = parameter.ToString();
			}
		}
		object parameter;
		[SerializeField]
		string parameterString;

		public void Copy( AdvParamSettingData src )
		{
			InitKey(src.Key);
			this.type = src.type;
			this.parameterString = src.parameterString;
			ParseParmeterString();
		}

		/// <summary>
		/// StringGridの一行からデータ初期化
		/// </summary>
		/// <param name="row">初期化するためのデータ</param>
		/// <returns>成否</returns>
		public override bool InitFromStringGridRow(StringGridRow row)
		{
			string key = AdvParser.ParseCell<string>(row, AdvColumnName.Label);
			if (string.IsNullOrEmpty(key))
			{
				return false;
			}
			else
			{
				InitKey(key);
				this.type = AdvParser.ParseCell<ParamType>(row, AdvColumnName.Type);
				this.parameterString = AdvParser.ParseCell<string>(row, AdvColumnName.Value);
				switch (type)
				{
					case ParamType.Bool:
						parameter = AdvParser.ParseCell<bool>(row, AdvColumnName.Value);
						break;
					case ParamType.Float:
						parameter = AdvParser.ParseCell<float>(row, AdvColumnName.Value);
						break;
					case ParamType.Int:
						parameter = AdvParser.ParseCell<int>(row, AdvColumnName.Value);
						break;
					case ParamType.String:
						parameter = AdvParser.ParseCell<string>(row, AdvColumnName.Value);
						break;
				}
				return true;
			}
		}

		/// <summary>
		/// バイナリ書き込み
		/// </summary>
		/// <param name="writer">バイナリライター</param>
		public void Write(BinaryWriter writer)
		{
			writer.Write(Key);
			writer.Write((int)type);
			writer.Write(parameterString);
		}

		/// <summary>
		/// バイナリ読み込み
		/// </summary>
		/// <param name="reader">バイナリリーダー</param>
		public void Read(BinaryReader reader)
		{
			InitKey(reader.ReadString());
			type = (ParamType)reader.ReadInt32();
			parameterString = reader.ReadString();
			ParseParmeterString();
		}

		void ParseParmeterString()
		{
			switch (type)
			{
				case ParamType.Bool:
					parameter = bool.Parse(parameterString);
					break;
				case ParamType.Float:
					parameter = float.Parse(parameterString);
					break;
				case ParamType.Int:
					parameter = int.Parse(parameterString);
					break;
				case ParamType.String:
					parameter = parameterString;
					break;
			}
		}
	}

	/// <summary>
	/// パラメーター設定
	/// </summary>
	[System.Serializable]
	public partial class AdvParamSetting : SerializableDictionaryFileRead<AdvParamSettingData>
	{
		/// <summary>
		/// オブジェクトのクローン
		/// </summary>
		/// <returns>クローン結果</returns>
		public void Copy( AdvParamSetting src )
		{
			this.Clear();
			foreach (AdvParamSettingData item in src.List)
			{
				AdvParamSettingData data = new AdvParamSettingData();
				data.Copy(item);
				Add(data);
			}
		}

		/// <summary>
		/// 値の代入を試みる
		/// </summary>
		/// <param name="key">値の名前</param>
		/// <param name="parameter">値</param>
		/// <returns>代入に成功したらtrue。指定の名前の数値なかったらfalse</returns>
		public bool TrySetParameter(string key, object parameter)
		{
			AdvParamSettingData data;
			if (TryGetValue(key, out data))
			{
				///bool値のキャストは気をつける
				if( data.Type == AdvParamSettingData.ParamType.Bool || parameter is bool )
				{
					if (data.Type != AdvParamSettingData.ParamType.Bool || !(parameter is bool) )
					{
						return false;
					}
				}
				///string値のキャストは気をつける
				if ( parameter is string)
				{
					if (data.Type != AdvParamSettingData.ParamType.String )
					{
						return false;
					}
				}
				if (data.Type == AdvParamSettingData.ParamType.String)
				{
					if (parameter is bool)
					{
						return false;
					}
				}
				data.Parameter = parameter;
				return true;
			}
			return false;
		}

		/// <summary>
		/// 値の取得を試みる
		/// </summary>
		/// <param name="key">値の名前</param>
		/// <param name="parameter">値</param>
		/// <returns>成功したらtrue。指定の名前の数値なかったらfalse</returns>
		public bool TryGetParameter(string key, out object parameter)
		{
			AdvParamSettingData data;
			if (TryGetValue(key, out data))
			{
				parameter = data.Parameter;
				return true;
			}
			parameter = null;
			return false;
		}

		/// <summary>
		/// 値の代入が可能かチェックする。実際には代入しない
		/// </summary>
		/// <param name="key">値の名前</param>
		/// <param name="parameter">値</param>
		/// <returns>代入に成功したらtrue。指定の名前の数値なかったらfalse</returns>
		public bool CheckSetParameter(string key, object parameter)
		{
			AdvParamSettingData data;
			if (TryGetValue(key, out data))
			{
				///bool値のキャストは気をつける
				if (data.Type == AdvParamSettingData.ParamType.Bool || parameter is bool)
				{
					if (data.Type != AdvParamSettingData.ParamType.Bool || !(parameter is bool))
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

/*
		/// <summary>
		/// 値の設定
		/// </summary>
		/// <param name="key">値の名前</param>
		/// <param name="parameter">値</param>
		public void SetParameter(string key, object parameter)
		{
			TrySetParameter(key, parameter);
		}
*/
		/// <summary>
		/// 値の取得
		/// </summary>
		/// <param name="key">値の名前</param>
		/// <returns>数値</returns>
		public object GetParameter(string key)
		{
			object parameter;
			if (TryGetParameter(key, out parameter))
			{
				return parameter;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// 文字列で書かれた数式から数式を作成
		/// </summary>
		/// <param name="exp">文字列で書かれた数式</param>
		/// <returns>数式</returns>
		public ExpressionParser CreateExpression(string exp)
		{
			return new ExpressionParser(exp, GetParameter, CheckSetParameter);
		}


		/// <summary>
		/// 文字列で書かれた数式を計算して結果を返す
		/// ただし、パラメーターに代入はしない
		/// </summary>
		/// <param name="exp">文字列で書かれた数式</param>
		/// <returns>計算結果</returns>
		public object CalcExpressionNotSetParam(string exp)
		{
			ExpressionParser expression = CreateExpression(exp);
			if (string.IsNullOrEmpty(expression.ErrorMsg))
			{
				return expression.CalcExp(GetParameter, CheckSetParameter);
			}
			else
			{
				throw new System.Exception(expression.ErrorMsg);
			}
		}


		/// <summary>
		/// 数式を計算して結果を返す
		/// </summary>
		/// <param name="exp">数式</param>
		/// <returns>計算結果</returns>
		public object CalcExpression(ExpressionParser exp)
		{
			return exp.CalcExp(GetParameter, TrySetParameter);
		}

		/// <summary>
		/// 文字列で書かれた論理式から数式を作成
		/// </summary>
		/// <param name="exp">文字列で書かれた論理式</param>
		/// <returns>数式</returns>
		public ExpressionParser CreateExpressionBoolean(string exp)
		{
			return new ExpressionParser(exp, GetParameter, CheckSetParameter, true);
		}

		/// <summary>
		/// 論理式を計算して結果を返す
		/// </summary>
		/// <param name="exp">数式</param>
		/// <returns>計算結果</returns>
		public bool CalcExpressionBoolean(ExpressionParser exp)
		{
			return exp.CalcExpBoolean(GetParameter, TrySetParameter);
		}

		/// <summary>
		/// 文字列で書かれた論理式を計算して結果を返す
		/// </summary>
		/// <param name="exp">文字列で書かれた論理式</param>
		/// <returns>計算結果</returns>
		public bool CalcExpressionBoolean(string exp)
		{
			return CalcExpressionBoolean(CreateExpressionBoolean(exp));
		}



		/// <summary>
		/// セーブデータ用のバイナリ変換
		/// </summary>
		/// <returns>変換後のバイナリ配列</returns>
		public byte[] ToSaveDataBuffer()
		{
			using (MemoryStream stream = new MemoryStream())
			{
				//バイナリ化
				using (BinaryWriter writer = new BinaryWriter(stream))
				{
					WriteSaveData(writer);
				}
				return stream.ToArray();
			}
		}

		/// <summary>
		/// セーブデータを読みこみ
		/// </summary>
		/// <param name="buffer">セーブデータのバイナリ</param>
		public void ReadSaveDataBuffer(byte[] buffer)
		{
			using (MemoryStream stream = new MemoryStream(buffer))
			{
				using (BinaryReader reader = new BinaryReader(stream))
				{
					ReadSaveData(reader);
				}
			}
		}

		const int VERSION = 0;

		//バイナリ書き込み
		void WriteSaveData(BinaryWriter writer)
		{
			writer.Write(VERSION);
			writer.Write(List.Count);
			foreach (AdvParamSettingData item in List)
			{
				item.Write(writer);
			}
		}
		//バイナリ読み込み
		void ReadSaveData(BinaryReader reader)
		{
			int version = reader.ReadInt32();
			if (version == VERSION)
			{
				this.Clear();
				int count = reader.ReadInt32();
				for (int i = 0; i < count; i++)
				{
					AdvParamSettingData data = new AdvParamSettingData();
					data.Read(reader);
					Add(data);
				}
			}
			else
			{
				Debug.LogError(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.UnknownVersion, version));
			}
		}
	}
}