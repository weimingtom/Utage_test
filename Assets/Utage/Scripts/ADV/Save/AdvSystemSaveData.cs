//----------------------------------------------
// UTAGE: Unity Text Adventure Game Engine
// Copyright 2014 Ryohei Tokimura
//----------------------------------------------

using System.IO;
using UnityEngine;

namespace Utage
{

	/// <summary>
	/// ゲームで共通して使うシステムセーブデータ
	/// </summary>
	[AddComponentMenu("Utage/ADV/Internal/SystemSaveData")]
	public class AdvSystemSaveData : MonoBehaviour
	{
		[SerializeField]
		bool isAutoSaveOnQuit = true;

		FileIOManager FileIOManager { get { return this.fileIOManager ?? (this.fileIOManager = FindObjectOfType<FileIOManager>() as FileIOManager); } }
		[SerializeField]
		FileIOManager fileIOManager;

		/// <summary>
		/// ディレクトリ名
		/// </summary>
		public string DirectoryName
		{
			get { return directoryName; }
			set { directoryName = value; }
		}
		[SerializeField]
		string directoryName = "Save";

		/// <summary>
		/// ファイル名
		/// </summary>
		public string FileName
		{
			get { return fileName; }
			set { fileName = value; }
		}
		[SerializeField]
		string fileName = "system";

		/// <summary>
		/// ファイルパス
		/// </summary>
		public string Path { get { return this.path; } }
		string path;

		/// <summary>
		/// 既読のデータ
		/// </summary>
		public AdvReadHistorySaveData ReadData { get { return this.readData; } }
		AdvReadHistorySaveData readData = new AdvReadHistorySaveData();

		//コンフィグ設定のデータ
		AdvConfig config;

		/// <summary>
		/// 回想モード用のデータ
		/// </summary>
		public AdvGallerySaveData GalleryData { get { return this.galleryData; } }
		AdvGallerySaveData galleryData = new AdvGallerySaveData();

		/// <summary>
		/// 初期化フラグ
		/// </summary>
		bool isInit = false;

		/// <summary>
		/// 初期化
		/// </summary>
		public void Init(AdvConfig config)
		{
			this.config = config;
			string saveDir = FileIOManager.SdkPersistentDataPath + DirectoryName + "/";
			//セーブデータのディレクトリがなければ作成
			FileIOManager.CreateDirectory(saveDir);

			path = saveDir + FileName;
			isInit = true;
			if (!FileIOManager.Exists(Path))
			{
				//デフォルト値で初期化
				InitDefault();
			}
			else
			{
				Read();
			}
		}

		/// <summary>
		/// デフォルト値で初期化(初回でセーブデータがない場合)
		/// </summary>
		public void InitDefault()
		{
			config.InitDefault();
		}

		/// <summary>
		/// 読み込み
		/// </summary>
		public void Read()
		{
			if (!FileIOManager.ReadBinaryDecode(Path, ReadBinary))
			{
				//デフォルト値で初期化
				InitDefault();
			}
		}


		static readonly int MagicID = FileIOManager.ToMagicID('S', 'y', 's', 't');	//識別ID
		const int Version = 1;	//ファイルバージョン

		//バイナリ読み込み
		void ReadBinary(BinaryReader reader)
		{
			int magicID = reader.ReadInt32();
			if (magicID != MagicID)
			{
				throw new System.Exception("Read File Id Error");
			}

			int fileVersion = reader.ReadInt32();
			if (fileVersion == Version)
			{
				ReadData.Read(reader);
				config.Read(reader);
				GalleryData.Read(reader);
			}
			else
			{
				throw new System.Exception(LanguageErrorMsg.LocalizeTextFormat(ErrorMsg.UnknownVersion, fileVersion));
			}
		}

		/// <summary>
		/// 書き込み
		/// </summary>
		public void Write()
		{
			FileIOManager.WriteBinaryEncode(Path, WriteBinary);
		}

		//バイナリ書き込み
		void WriteBinary(BinaryWriter writer)
		{
			writer.Write(MagicID);
			writer.Write(Version);
			ReadData.Write(writer);
			config.Write(writer);
			GalleryData.Write(writer);
		}

		//セーブデータを消去して終了(SendMessageでコールバックされるので名前固定)
		void OnDeleteAllSaveDataAndQuit()
		{
			Delete();
			isInit = false;
		}

		/// <summary>
		/// セーブデータを消去
		/// </summary>
		public void Delete()
		{
			FileIOManager.Delete(Path);
		}
		//ゲーム終了時
		void OnApplicationQuit()
		{
			if (isInit)
			{
				//オートセーブ
				if (!isAutoSaveOnQuit) return;
				Write();
			}
		}
	}
}