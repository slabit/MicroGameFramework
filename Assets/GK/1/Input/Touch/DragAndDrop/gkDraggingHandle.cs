using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	[AddComponentMenu("GK/Input/Touch/DragAndDrop/gkDraggingHandle")]
	public class gkDraggingHandle : MonoBehaviour
	{
		public Action<gkDraggingHandle> onStartDragging;
		
		public Action<gkDraggingHandle> onStopDragging;
		
		public gkButton button;
		
		public gkTouchButtonController touchButtonController;
		
		private bool m_bDragging;
		
		public bool IsDragging
		{
			get
			{
				return m_bDragging;
			}
		}
		
		public float DraggingDistance
		{
			get
			{
				return touchButtonController.TouchDistanceFromStart;
			}
		}
		
		public Vector2 DraggingMovement
		{
			get
			{
				return touchButtonController.TouchMovementFromStart;
			}
		}
		
		public Vector2 DraggingPosition
		{
			get
			{
				return touchButtonController.CurrentTouchPosition;
			}
		}
		
		public Vector2 DraggingStartPosition
		{
			get
			{
				return touchButtonController.StartTouchPosition;
			}
		}
		
		public void ForceDraggingStart()
		{
			if(m_bDragging == false)
			{
				float fSwipeMinDistanceSave = touchButtonController.swipe.minDistance;
				touchButtonController.swipe.minDistance = 0;
				
				touchButtonController.UpdateController();
				
				touchButtonController.swipe.minDistance = fSwipeMinDistanceSave;
			}
		}
		
		public void GiveToTouch(int a_iTouchIndex)
		{
			touchButtonController.ForceStartTouch(a_iTouchIndex, true);
		}
		
		private void Start()
		{
			touchButtonController.onSwipe += OnSwipe;
			button.onUp += OnUp;
			if(touchButtonController.IsSwiping)
			{
				OnSwipe(true);
			}
		}
		
		private void OnDestroy()
		{
			touchButtonController.onSwipe -= OnSwipe;
			button.onUp -= OnUp;
		}
		
		private void OnSwipe(bool a_bSwiping)
		{
			if(a_bSwiping)
			{
				if(m_bDragging == false)
				{
					StartDragging();
				}
			}
		}
		
		private void OnUp()
		{
			if(m_bDragging)
			{
				StopDragging();
			}
		}
		
		private void StartDragging()
		{
			m_bDragging = true;
			if(onStartDragging != null)
			{
				onStartDragging(this);
			}
		}
		
		private void StopDragging()
		{
			m_bDragging = false;
			if(onStopDragging != null)
			{
				onStopDragging(this);
			}
		}
	}
}