//----------------------------------------------
// UTAGE: Unity Text Adventure Game Engine
// Copyright 2014 Ryohei Tokimura
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;

namespace Utage
{

	/// <summary>
	/// テクスチャ設定（ラベルとファイルの対応）
	/// </summary>
	[System.Serializable]
	public partial class AdvTextureSettingData : SerializableDictionaryFileReadKeyValue
	{
		/// <summary>
		/// ファイル名
		/// </summary>
		[SerializeField]
		string fileName;

		/// <summary>
		/// ファイルパス
		/// </summary>
		public string FilePath { get { return this.filePath; } }
		string filePath;

		/// <summary>
		/// テクスチャのタイプ
		/// </summary>
		public enum Type
		{
			/// <summary>背景</summary>
			Bg,
			/// <summary>イベントCG</summary>
			Event,
			/// <summary>スプライト</summary>
			Sprite,
		}

		/// <summary>テクスチャのタイプ</summary>
		public Type TextureType { get { return this.type; } }
		[SerializeField]
		Type type;

		/// <summary>
		/// ピボット位置
		/// </summary>
		public Vector2 Pivot { get { return this.pivot; } }
		[SerializeField]
		Vector2 pivot;

		/// <summary>
		/// スケール
		/// </summary>
		public float Scale { get { return this.scale; } }
		[SerializeField]
		float scale;

		/// <summary>バージョン</summary>
		public int Version { get { return this.version; } }
		[SerializeField]
		int version;


		/// <summary>
		/// サムネイル用ファイル名
		/// </summary>
		[SerializeField]
		string thumbnailName;

		/// <summary>
		/// サムネイル用ファイルパス
		/// </summary>
		public string ThumbnailPath { get { return this.thumbnailPath; } }
		string thumbnailPath;

		/// <summary>
		/// サムネイル用ファイルのバージョン
		/// </summary>
		public int ThumbnailVersion { get { return this.thumbnailVersion; } }
		[SerializeField]
		int thumbnailVersion;

		/// <summary>
		/// StringGridの一行からデータ初期化
		/// </summary>
		/// <param name="row">初期化するためのデータ</param>
		/// <returns>成否</returns>
		public override bool InitFromStringGridRow(StringGridRow row)
		{
			string key = AdvParser.ParseCell<string>(row, AdvColumnName.Label);
			InitKey(key);
			this.type = AdvParser.ParseCell<Type>(row, AdvColumnName.Type);

			this.pivot = Vector2.one * 0.5f;
			try
			{
				this.pivot = PivotUtil.ParsePivotOptional(AdvParser.ParseCellOptional<string>(row, AdvColumnName.Pivot, ""), pivot);
			}
			catch (System.Exception e)
			{
				Debug.LogError(row.ToErrorString(e.Message));
			}
			this.scale = AdvParser.ParseCellOptional<float>(row, AdvColumnName.Scale, 1.0f);

			this.fileName = AdvParser.ParseCell<string>(row, AdvColumnName.FileName);
			this.version = AdvParser.ParseCellOptional<int>(row, AdvColumnName.Version, 0);
			this.thumbnailName = AdvParser.ParseCellOptional<string>(row, AdvColumnName.Thumbnail, "");
			this.thumbnailVersion = AdvParser.ParseCellOptional<int>(row, AdvColumnName.ThumbnailVersion, 0);
			return true;
		}

		/// <summary>
		/// 起動時の初期化
		/// </summary>
		/// <param name="settingData">設定データ</param>
		public void BootInit(AdvBootSetting settingData)
		{
			filePath = FileNameToPath(fileName,settingData);
			thumbnailPath = settingData.ThumbnailDirInfo.FileNameToPath(thumbnailName);
		}

		string FileNameToPath(string fileName, AdvBootSetting settingData)
		{
			switch (type)
			{
				case AdvTextureSettingData.Type.Event:
					return settingData.EventDirInfo.FileNameToPath(fileName);
				case AdvTextureSettingData.Type.Sprite:
					return settingData.SpriteDirInfo.FileNameToPath(fileName);
				case AdvTextureSettingData.Type.Bg:
				default:
					return settingData.BgDirInfo.FileNameToPath(fileName);
			}
		}
	}

	/// <summary>
	/// テクスチャ設定
	/// </summary>
	[System.Serializable]
	public partial class AdvTextureSetting : SerializableDictionaryFileRead<AdvTextureSettingData>
	{
		/// <summary>
		/// 起動時の初期化
		/// </summary>
		/// <param name="settingData">設定データ</param>
		public void BootInit(AdvBootSetting settingData)
		{
			//ファイルマネージャーにバージョンの登録
			foreach (AdvTextureSettingData data in List)
			{
				data.BootInit(settingData);

				{
					AssetFile file = AssetFileManager.GetFileCreateIfMissing(data.FilePath);
					file.Version = data.Version;
					file.SpriteInfo.pivot = data.Pivot;
					file.SpriteInfo.scale = data.Scale;
				}

				if( !string.IsNullOrEmpty(data.ThumbnailPath) )
				{
					AssetFile file = AssetFileManager.GetFileCreateIfMissing(data.ThumbnailPath);
					file.Version = data.ThumbnailVersion;
				}
			}
		}

		/// <summary>
		/// 全てのリソースをダウンロード
		/// </summary>
		public void DownloadAll()
		{
			//ファイルマネージャーにバージョンの登録
			foreach (AdvTextureSettingData data in List)
			{
				AssetFileManager.Download(data.FilePath);
				if (!string.IsNullOrEmpty(data.ThumbnailPath))
				{
					AssetFileManager.Download(data.ThumbnailPath);
				}
			}
		}


		/// <summary>
		/// ラベルからファイルパスを取得
		/// </summary>
		/// <param name="label">ラベル</param>
		/// <returns>ファイルパス</returns>
		public string LabelToFilePath(string label)
		{
			//既に絶対URLならそのまま
			if (UtageToolKit.IsAbsoluteUri(label))
			{
				return label;
			}
			else
			{
				AdvTextureSettingData data = FindData(label);
				if (data == null)
				{
					//ラベルをそのままファイル名扱いに
					return label;
				}
				else
				{
					return data.FilePath;
				}
			}
		}

		/// <summary>
		/// ラベルからファイルパスを取得
		/// </summary>
		/// <param name="label">ラベル</param>
		/// <returns>ファイルパス</returns>
		public bool ContainsLabel(string label)
		{
			//既に絶対URLならそのまま
			if (UtageToolKit.IsAbsoluteUri(label))
			{
				return true;
			}
			else
			{
				AdvTextureSettingData data = FindData(label);
				if (data == null)
				{
					return false;
				}
				else
				{
					return true;
				}
			}
		}

		//ラベルからファイル名を取得
		AdvTextureSettingData FindData(string label)
		{
			AdvTextureSettingData data;
			if (!TryGetValue(label, out data))
			{
				return null;
			}
			else
			{
				return data;
			}
		}

		/// <summary>
		/// CGギャラリー用のデータを取得
		/// </summary>
		/// <param name="saveData">セーブデータ</param>
		/// <param name="gallery">ギャリーのデータ</param>
		public List<AdvCgGalleryData> CreateCgGalleryList( AdvGallerySaveData saveData )
		{
			List<AdvCgGalleryData> list = new List<AdvCgGalleryData>();
			AdvCgGalleryData currentData = null;
			foreach (var item in List)
			{
				if (item.TextureType == AdvTextureSettingData.Type.Event)
				{
					if (string.IsNullOrEmpty(item.ThumbnailPath)) continue;

					string path = item.ThumbnailPath;
					if (currentData == null )
					{
						currentData = new AdvCgGalleryData(path, saveData);
						list.Add(currentData);
					}
					else
					{
						if (path != currentData.ThumbnailPath)
						{
							currentData = new AdvCgGalleryData(path, saveData);
							list.Add(currentData);
						}
					}
					currentData.AddTextureData(item);
				}
			}
			return list;
		}
	}
}