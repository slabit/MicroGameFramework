using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace gk
{
	[AddComponentMenu("GK/Input/Touch/DragAndDrop/gkDraggingMover")]
	public class gkDraggingMover : MonoBehaviour
	{
		public gkDraggingHandle draggingHandle;
		
		public Transform transformToDrag;
		
		public Camera draggingMovementCamera;
		
		private Vector2 m_f2DragOffset;
		
		private Vector3 m_f3DraggingPosition;
		
		public Camera DraggingMovementCamera
		{
			get
			{
				return draggingMovementCamera;
			}
		}
		
		public bool IsDragging
		{
			get
			{
				return draggingHandle.IsDragging;
			}
		}
		
		private void Start()
		{
			if(draggingMovementCamera == null)
			{
				draggingMovementCamera = Camera.main;
			}
			draggingHandle.onStartDragging += OnStartDragging;
			if(draggingHandle.IsDragging)
			{
				OnStartDragging(draggingHandle);
			}
		}
		
		private void OnDestroy()
		{
			draggingHandle.onStartDragging -= OnStartDragging;
		}
		
		private void LateUpdate()
		{
			if(draggingHandle.IsDragging)
			{
				UpdateDragging();
			}
		}
		
		private void OnStartDragging(gkDraggingHandle a_rDraggingHandle)
		{
			Vector2 f2TransformScreenPosition = draggingMovementCamera.WorldToScreenPoint(transformToDrag.position);
			m_f2DragOffset = a_rDraggingHandle.DraggingStartPosition - f2TransformScreenPosition;
		}
		
		private void UpdateDragging()
		{
			Vector3 f3NewPosition = ComputeWorldDraggingPosition(draggingHandle.DraggingPosition, -m_f2DragOffset);
			transformToDrag.position = f3NewPosition;
		}
		
		private Vector3 ComputeWorldDraggingPosition(Vector2 a_f2Position, Vector2 a_f2Offset)
		{
			float fDepth = draggingMovementCamera.WorldToScreenPoint(transformToDrag.position).z;
			
			Vector3 f3DraggingPosition = a_f2Position + a_f2Offset;
			f3DraggingPosition.z = fDepth;
			f3DraggingPosition = draggingMovementCamera.ScreenToWorldPoint(f3DraggingPosition);
			
			return f3DraggingPosition;
		}
	}
}