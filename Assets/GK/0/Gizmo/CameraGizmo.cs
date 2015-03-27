using UnityEngine;
using System.Collections;

namespace gk
{
	public class CameraGizmo : MonoBehaviour 
	{
		public Camera cameraComponent;

		public bool alwaysDrawOrthographic;

		public Color gizmoColor = Color.white;

		void OnDrawGizmos()
		{
			Camera usedCamera = cameraComponent;

			if(usedCamera == null)
				usedCamera = GetComponent<Camera>();

			if(usedCamera == null)
				return;

			Gizmos.color = gizmoColor;

			Gizmos.matrix = usedCamera.transform.localToWorldMatrix;
			if(alwaysDrawOrthographic || usedCamera.orthographic)
			{
				Vector3 size = new Vector3(usedCamera.orthographicSize * 2.0f * usedCamera.aspect,
				                           usedCamera.orthographicSize * 2.0f,
				                           usedCamera.farClipPlane - usedCamera.nearClipPlane);

				Vector3 center = Vector3.forward * (usedCamera.nearClipPlane + size.z * 0.5f);

				Gizmos.DrawWireCube(center, size);
			}
			else
			{
				Gizmos.DrawFrustum(Vector3.zero, usedCamera.fieldOfView, usedCamera.farClipPlane, usedCamera.nearClipPlane, usedCamera.aspect);
			}
		}
	}
}