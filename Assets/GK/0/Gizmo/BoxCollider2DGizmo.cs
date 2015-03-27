using UnityEngine;
using System.Collections;

namespace gk
{
	public class BoxCollider2DGizmo : MonoBehaviour 
	{
		public BoxCollider boxColliderComponent;

		public Color gizmoColor = Color.green;

		void OnDrawGizmos()
		{
			BoxCollider usedBoxCollider = boxColliderComponent;

			if(usedBoxCollider == null)
				usedBoxCollider = GetComponent<BoxCollider>();

			if(usedBoxCollider == null)
				return;

			Gizmos.color = gizmoColor;

			Gizmos.matrix = usedBoxCollider.transform.localToWorldMatrix;

			Gizmos.DrawWireCube(usedBoxCollider.center,
			                    usedBoxCollider.size);
		}
	}
}