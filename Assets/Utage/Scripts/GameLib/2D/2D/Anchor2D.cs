using UnityEngine;
using System.Collections;

namespace Utage
{

	/// <summary>
	/// 2D座標を画面中央、画面端などに合わせて固定する
	/// </summary>
	[ExecuteInEditMode]
	[AddComponentMenu("Utage/Lib/2D/Anchor")]
	public class Anchor2D : MonoBehaviour
	{
		/// <summary>
		/// 画面のどの位置にするかのタイプ
		/// </summary>
		enum AnchorType
		{
			TopLeft,
			Top,
			TopRight,
			Left,
			Center,
			Right,
			BottomLeft,
			Bottom,
			BottomRight,
		}

		[SerializeField]
		AnchorType type = AnchorType.Center;

		/// <summary>
		/// トランスフォームのキャッシュ(this.transformだと低速なため)
		/// </summary>
		Transform cachedTransform;
		Transform CachedTransform { get { if (null == cachedTransform) cachedTransform = this.transform; return cachedTransform; } }

		/// <summary>
		/// カメラーマネージャー
		/// </summary>
		[SerializeField]
		CameraManager cameraManager = null;
		CameraManager CameraManager { get { if (null == cameraManager) cameraManager = FindObjectOfType<CameraManager>(); return cameraManager; } }

		/// <summary>
		/// カメラのトランスフォームのキャッシュ(this.transformだと低速なため)
		/// </summary>
		Transform cachedCameraTransform;
		Transform CachedCameraTransform
		{ 
			get {
				if (null == cachedCameraTransform)
				{
					Camera cam2D = CameraManager.Find2DCamera(this.gameObject.layer);
					if (null != cam2D) cachedCameraTransform = cam2D.transform;
				}
				return cachedCameraTransform;
			}
		}

		void Update()
		{
			RefreshPositiosn();
		}

		/// <summary>
		/// 位置を更新
		/// </summary>
		void RefreshPositiosn()
		{
			if (CameraManager == null || CachedCameraTransform == null) return;

			float z = CachedTransform.position.z;
			Vector3 pos = CachedCameraTransform.position;
			pos.z = z;

			float w = 1.0f * CameraManager.CurrentWidth / CameraManager.PixelsToUnits / 2;
			float h = 1.0f * CameraManager.CurrentHeight / CameraManager.PixelsToUnits / 2;
			switch (type)
			{
				case AnchorType.TopLeft:
					pos.x -= w;
					pos.y += h;
					break;
				case AnchorType.Top:
					pos.y += h;
					break;
				case AnchorType.TopRight:
					pos.x += w;
					pos.y += h;
					break;
				case AnchorType.Left:
					pos.x -= w;
					break;
				case AnchorType.Center:
					break;
				case AnchorType.Right:
					pos.x += w;
					break;
				case AnchorType.BottomLeft:
					pos.x -= w;
					pos.y -= h;
					break;
				case AnchorType.Bottom:
					pos.y -= h;
					break;
				case AnchorType.BottomRight:
					pos.x += w;
					pos.y -= h;
					break;
			}

			if (CachedTransform.position != pos)
			{
				CachedTransform.position = pos;
			}
		}
	}
}