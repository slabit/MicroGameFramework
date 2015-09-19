 using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	[AddComponentMenu("GK/Input/Touch/gkTouchButtonController")]
	public class gkTouchButtonController : MonoBehaviour
	{
		public enum ETouchStartMode
		{
			MustTap,
			DontNeedToTap,
			MustTapButDontNeedToOnEnable,
			MustTapButDontNeedToOnStart
		}
		
		public enum ETouchUsageMode
		{
			UseTouchExclusively,
			DontUseTouchExclusively,
			CancelOtherTouchOnRelease,
		}
		
		public enum ETouchEndMode
		{
			EndOnRelease,
			EndOnSwipe,
			EndOnSwipeOrOnRelease,
			EndOnHold
		}
		
		public enum ETouchCancelationMode
		{
			CancelWhenReleaseOutOfZone,
			CancelWhenCursorOutOfZone,
			CancelWhenSwiped,
			NeverCancel
		}
		
		public enum ETouchTransfertMode
		{
			SelectAnOtherValidTouchOnTouchEnd,
			DontTransfertTouch
		}
		
		private enum ETouchEndType
		{
			Release,
			Swipe,
			Hold
		}
		
		private enum ETouchInstigator
		{
			Mouse,
			Finger
		}
		
		[Serializable]
		public class ClickCountRules
		{
			public int numberOfClickNeeded = 1;
		
			public float clickRemanenceDuration = 0.1f;
		}
		
		[Serializable]
		public class ClickFilter
		{
			public List<int> mouseButtons = new List<int>{0};
		}
		
		[Serializable]
		public class SwipeParameters
		{
			public float minDistance = 5.0f;
			
			public bool IsASwipe(Vector2 a_f2StartPosition, Vector2 a_f2CurrentPosition)
			{
				
				float fSwipeSquareDistance = (a_f2CurrentPosition - a_f2StartPosition).sqrMagnitude;
				float fSwipeMinSquareDistance = (minDistance * minDistance);
				return fSwipeSquareDistance >= fSwipeMinSquareDistance;
			}
		}
		
		[Serializable]
		public class HoldParameters
		{
			public float minimumHoldDuration = 0.5f;
			
			private float m_fHoldDuration;
			
			public void StartTouch()
			{
				m_fHoldDuration = 0.0f;
			}
			
			public void UpdateTouch()
			{
				m_fHoldDuration += Time.deltaTime;
			}
			
			public bool HasHeldLongEnough()
			{
				return m_fHoldDuration >= minimumHoldDuration;
			}
		}
		
		private class TouchInfos
		{
			private ETouchInstigator m_eTouchInstigator;
			
			private int m_iTouchIndex = -1;
			
			private Vector2 m_f2TouchPosition;
			
			private Vector2 m_f2StartTouchPosition;
			
			public Vector2 StartTouchPosition
			{
				get
				{
					return m_f2StartTouchPosition;
				}
				
				set
				{
					m_f2StartTouchPosition = value;
				}
			}
			
			public Vector2 CurrentTouchPosition
			{
				get
				{
					return m_f2TouchPosition;
				}
			}
			
			public int TouchIndex
			{
				get
				{
					return m_iTouchIndex;
				}
				
				set
				{
					m_iTouchIndex = value;
				}
			}
			
			public void StartTouch(ETouchInstigator a_eTouchInstigator, int a_iMouseButtonIndex, Vector2 a_f2TouchPosition)
			{
				m_eTouchInstigator = a_eTouchInstigator;
				m_iTouchIndex = a_iMouseButtonIndex;
				m_f2StartTouchPosition = a_f2TouchPosition;
				m_f2TouchPosition = a_f2TouchPosition;
			}
			
			public bool TryToUpdateTouchPosition()
			{
				switch(m_eTouchInstigator)
				{
					case ETouchInstigator.Finger:
					{
						Touch rCurrentTouch;
					
						// Get the current touch
						if(gkTouchUtility.TryGetTouchByFingerId(m_iTouchIndex, out rCurrentTouch))
						{
							if((rCurrentTouch.phase != TouchPhase.Ended && rCurrentTouch.phase != TouchPhase.Canceled))
							{
								m_f2TouchPosition = rCurrentTouch.position;
								return true;
							}
						}
					}
					break;
					
					case ETouchInstigator.Mouse:
					{
						if(Input.GetMouseButton(m_iTouchIndex))
						{
							m_f2TouchPosition = Input.mousePosition;
							return true;
						}
					}
					break;
				}
				
				return false;
			}
		}
		
		public Action<bool> onSwipe;
		
		public Action afterUpdate;
		
		[SerializeField]
		private string touchSortingLayerName = "";
		
		[SerializeField]
		private int touchSortingOrder = 0;
		
		public gkButton controlledButton;
		
		public ETouchStartMode touchStartMode;
		
		public ETouchUsageMode touchUsageMode;
		
		public ETouchEndMode touchEndMode;
		
		public ETouchCancelationMode touchCancelationMode;
		
		public ETouchTransfertMode touchTransfertMode;
		
		public SwipeParameters swipe = new SwipeParameters();
		
		public HoldParameters hold = new HoldParameters();
			
		public ClickCountRules clickCountRules = new ClickCountRules();
		
		public ClickFilter clickFilter = new ClickFilter();
		
		// the touch zones
		public List<gkTouchZoneBase> touchZones = new List<gkTouchZoneBase>();
		
		public List<gkTouchZoneBase> touchZonesExcluded = new List<gkTouchZoneBase>();
		
		private bool m_bTouched;
		
		private TouchInfos m_oCurrentTouchInfos = new TouchInfos();
		
		private int m_iNumberOfClick;
		
		private float m_fTimeSinceLastClick;
		
		private bool m_bTouchHasBeenCanceled;
		
		private ETouchEndType m_eTouchEndType;
		
		private bool m_bButtonEnabled;
		
		private bool m_bUseCurrentTouch;
		
		private bool m_bManageByScheduler;
		
		private bool m_bSwiping;
		
		private int m_iTouchSortingLayerID;
		
		public string TouchSortingLayerName
		{
			get
			{
				return touchSortingLayerName;
			}
			
			set
			{
				if(value != touchSortingLayerName)
				{
					touchSortingLayerName = value;
					
					AccordTouchLayerIDToName();
					
					gkTouchControllerScheduler.OnUpdateSorting(this);
				}
			}
		}
		
		public int TouchSortingLayerID
		{
			get
			{
				return m_iTouchSortingLayerID;
			}
			
			set
			{
				if(value != m_iTouchSortingLayerID)
				{
					m_iTouchSortingLayerID = value;
					
					AccordTouchLayerNameToID();
					
					gkTouchControllerScheduler.OnUpdateSorting(this);
				}
			}
		}
		
		public int TouchSortingOrder
		{
			get
			{
				return touchSortingOrder;
			}
			
			set
			{
				if(value != touchSortingOrder)
				{
					touchSortingOrder = value;
					gkTouchControllerScheduler.OnUpdateSorting(this);
				}
			}
		}
		
		public Vector2 StartTouchPosition
		{
			get
			{
				if(m_oCurrentTouchInfos == null)
				{
					return Vector2.zero;
				}
				else
				{
					return m_oCurrentTouchInfos.StartTouchPosition;
				}
			}
		}
		
		public Vector2 CurrentTouchPosition
		{
			get
			{
				if(m_oCurrentTouchInfos == null)
				{
					return Vector3.zero;
				}
				else
				{
					return m_oCurrentTouchInfos.CurrentTouchPosition;
				}
			}
		}
		
		public int CurrentTouchIndex
		{
			get
			{
				if(m_oCurrentTouchInfos == null)
				{
					return -1;
				}
				else
				{
					return m_oCurrentTouchInfos.TouchIndex;
				}
			}
		}
		
		public float TouchDistanceFromStart
		{
			get
			{
				return (CurrentTouchPosition - StartTouchPosition).magnitude;
			}
		}
		
		public Vector3 TouchMovementFromStart
		{
			get
			{
				return CurrentTouchPosition - StartTouchPosition;
			}
		}
		
		public bool IsSwiping
		{
			get
			{
				return m_bSwiping;
			}
		}
		
		public bool IsTouched
		{
			get
			{
				return m_bTouched;
			}
		}
		
	    public void UpdateController()
	    {	
			bool bButtonEnabled;
			if(controlledButton == null)
			{
				bButtonEnabled = true;
			}
			else
			{
				bButtonEnabled = controlledButton.enabled;
			}
			
			if(bButtonEnabled != m_bButtonEnabled)
			{
				if(bButtonEnabled)
				{
					OnButtonEnable();
				}
				else
				{
					OnButtonDisable();
				}
			}
			
			if(m_bButtonEnabled)
			{
				UpdateTouch();
			}
			
			if(afterUpdate != null)
			{
				afterUpdate();
			}
		}
		
		public void CancelTouch()
		{
			// Cancel touch
			if(m_bUseCurrentTouch)
			{
				StopUseCurrentTouch();
			}
			m_bTouched = false;
			CancelButton();
			StopSwipe();
			
			m_bTouched = false;
			m_iNumberOfClick = 0;
			m_fTimeSinceLastClick = 0;
			
			m_bButtonEnabled = false;
		}
		
		public void SendSwipeEvent(int a_iTouchIndex)
		{
			m_oCurrentTouchInfos.TouchIndex = a_iTouchIndex;
			NotifySwipe(true);
		}
		
		public void ForceStartTouch(int a_iTouchIndex, bool a_bForceSwipe)
		{
			// Cancel current touch
			CancelTouch();
		
			// Try to start touch
			bool bTouchStarted = false;
			// Touches
			if(gkTouch.MultiTouchEnabled)
			{
				foreach(Touch rTouch in Input.touches)
				{
					TouchPhase eTouchPhase = rTouch.phase;
					if(rTouch.fingerId == a_iTouchIndex && eTouchPhase != TouchPhase.Canceled && eTouchPhase != TouchPhase.Ended)
					{
						m_oCurrentTouchInfos.StartTouch(ETouchInstigator.Finger, a_iTouchIndex, rTouch.position);
						bTouchStarted = true;
						break;
					}
				}
			}
			else
			{
				// Mouse buttons
				if(Input.GetMouseButton(a_iTouchIndex) && clickFilter.mouseButtons.Contains(a_iTouchIndex))
				{
					m_oCurrentTouchInfos.StartTouch(ETouchInstigator.Mouse, a_iTouchIndex, Input.mousePosition);
					bTouchStarted = true;
				}
			}
			
			if(bTouchStarted)
			{
				CancelAllOtherUsedTouches();
				StartTouch();
				if(a_bForceSwipe)
				{
					StartSwipe();
				}
			}
		}
		
		private void Start()
		{
			AccordTouchLayerIDToName();
			m_bManageByScheduler = gkTouchControllerScheduler.TryToRegister(this);
			
			if(touchStartMode == ETouchStartMode.MustTapButDontNeedToOnStart)
			{
				TryToTouch_DontNeedATap();
			}
		}
		
		private void OnDestroy()
		{
			gkTouchControllerScheduler.TryToUnregister(this);
		}
		
		private void OnEnable()
	    {
			OnButtonEnable();
		}
		
		private void OnDisable()
		{
			OnButtonDisable();
		}
		
	    private void Update()
	    {	
			if(m_bManageByScheduler == false)
			{
				UpdateController();
			}
		}
		
		private void OnButtonEnable()
	    {
			if(touchStartMode == ETouchStartMode.MustTapButDontNeedToOnEnable)
			{
				TryToTouch_DontNeedATap();
			}
			m_bButtonEnabled = true;
		}
		
		private void OnButtonDisable()
		{
			CancelTouch();
		}
		
		private void PressButton()
		{
			if(controlledButton != null)
			{
				controlledButton.Press();
			}
		}
		
		private void ReleaseButton()
		{
			if(controlledButton != null)
			{
				controlledButton.Release();
			}
		}
		
		private void CancelButton()
		{
			if(controlledButton != null)
			{
				controlledButton.Cancel();
			}
		}
		
		private void StartTouch()
		{
			switch(touchUsageMode)
			{
				case ETouchUsageMode.UseTouchExclusively:
				{
					StartUseCurrentTouch(true);	
				}
				break;
				
				case ETouchUsageMode.DontUseTouchExclusively:
				case ETouchUsageMode.CancelOtherTouchOnRelease:
				{
					StartUseCurrentTouch(false);
				}
				break;
			}
			
			hold.StartTouch();
			
			m_bTouched = true;
			m_bTouchHasBeenCanceled = false;
			
			PressButton();
		}
		
		private void EndTouch()
		{		
			if(m_bUseCurrentTouch)
			{
				StopUseCurrentTouch();
			}
			
			m_bTouched = false;
			
			// Transfert the touch if needed
			switch(touchTransfertMode)
			{
				case ETouchTransfertMode.SelectAnOtherValidTouchOnTouchEnd:
				{
					if(CanTransfertCurrentTouch())
					{
						if(TryToTouch_DontNeedATap())
						{
							return;
						}
					}
				}
				break;
			}
			
			if(m_bTouchHasBeenCanceled == false && IsTouchASuccess(m_oCurrentTouchInfos.CurrentTouchPosition))
			{
				if(touchUsageMode == ETouchUsageMode.CancelOtherTouchOnRelease)
				{
					CancelOtherUsedTouchesWithLesserPriorities();
				}
				ReleaseButton();
			}
			else
			{
				CancelButton();
			}
			
			StopSwipe();
		}
		
		private bool CanTransfertCurrentTouch()
		{
			return m_eTouchEndType == ETouchEndType.Release;
		}
		
		private bool TryToStartTheTouch()
		{			
			switch(touchStartMode)
			{
				case ETouchStartMode.MustTap:
				case ETouchStartMode.MustTapButDontNeedToOnEnable:
				case ETouchStartMode.MustTapButDontNeedToOnStart:
				{
					return TryToTouch_NeedATap();
				}
				
				case ETouchStartMode.DontNeedToTap:
				{
					return TryToTouch_DontNeedATap();
				}
			}
			return false;
		}
		
		private void UpdateTouch()
		{	
			ProcessClickCount();
			
			if(m_bTouched)
			{
				hold.UpdateTouch();
			}
			
			if(m_bTouched == false)
			{
				TryToStartTheTouch();
			}
			
			if(m_bTouched)
			{	
				// Update the touch position if still active
				bool touchUpdateSucceeded = TryToUpdateCurrentTouchPosition();
				
				UpdateSwiping();
				
				if(touchUpdateSucceeded == false)
				{
					EndTouch();
				}
			}
		}
		
		private void UpdateSwiping()
		{	
			bool bSwiping = IsTheCurrentTouchASwipe();
			if(bSwiping != m_bSwiping)
			{
				if(bSwiping)
				{
					StartSwipe();
				}
				else
				{
					StopSwipe();
				}
			}
		}
		
		private void StartSwipe()
		{
			if(m_bSwiping == false)
			{
				m_bSwiping = true;
				NotifySwipe(m_bSwiping);
			}
		}
		
		private void StopSwipe()
		{	
			if(m_bSwiping)
			{
				m_bSwiping = false;
				NotifySwipe(m_bSwiping);
			}
		}
		
		private void NotifySwipe(bool a_bSwipe)
		{
			if(onSwipe != null)
			{
				onSwipe(m_bSwiping);
			}
		}
		
		private bool TryToTouch_NeedATap()
		{
			if(TryToFindABeginningTouchInTheZone())
			{
				return StartTouchIfEnoughClick();
			}
			return false;
		}
		
		private bool TryToTouch_DontNeedATap()
		{
			if(TryToFindATouchInTheZone())
			{
				return StartTouchIfEnoughClick();
			}
			return false;
		}
		
		private bool StartTouchIfEnoughClick()
		{
			AddClick();
			if(EnoughClick())
			{
				StartTouch();
				return true;
			}
			return false;
		}
		
		private bool TryToFindABeginningTouchInTheZone()
		{
			// Touches
			if(gkTouch.MultiTouchEnabled)
			{
				foreach(Touch rTouch in Input.touches)
				{
					if(rTouch.phase == TouchPhase.Began)
					{
						if(TryToGetTheTouchIfInTheZone(ETouchInstigator.Finger, rTouch.fingerId, rTouch.position))
						{
							return true;
						}
					}
				}
			}
			
			// Mouse buttons
			foreach(int iMouseButton in clickFilter.mouseButtons)
			{
				if(Input.GetMouseButtonDown(iMouseButton))
				{
					if(TryToGetTheTouchIfInTheZone(ETouchInstigator.Mouse, iMouseButton, Input.mousePosition))
					{
						return true;
					}
				}
			}
			
			return false;
		}
		
		private bool TryToFindATouchInTheZone()
		{
			// Touches
			if(gkTouch.MultiTouchEnabled)
			{
				foreach(Touch rTouch in Input.touches)
				{
					if(rTouch.phase == TouchPhase.Moved || rTouch.phase == TouchPhase.Stationary)
					{
						if(TryToGetTheTouchIfInTheZone(ETouchInstigator.Finger, rTouch.fingerId, rTouch.position))
						{
							return true;
						}
					}
				}
			}
			
			// Mouse buttons
			foreach(int iMouseButton in clickFilter.mouseButtons)
			{
				if(Input.GetMouseButton(iMouseButton))
				{
					if(TryToGetTheTouchIfInTheZone(ETouchInstigator.Mouse, iMouseButton, Input.mousePosition))
					{
						return true;
					}
				}
			}
			
			return false;
		}
		
		private bool CanTheTouchBeUsed(int a_iTouchIndex)
		{
			return gkTouchUsageManager.IsTouchUsedExclusively(a_iTouchIndex) == false;
		}
		
		private void StartUseCurrentTouch(bool a_bExclusively)
		{
			gkTouchUsageManager.StartUseTouch(m_oCurrentTouchInfos.TouchIndex, this, a_bExclusively);
			m_bUseCurrentTouch = true;
		}
		
		private void StopUseCurrentTouch()
		{
			gkTouchUsageManager.StopUseTouch(m_oCurrentTouchInfos.TouchIndex, this);
			m_bUseCurrentTouch = false;
		}
		
		private void CancelOtherUsedTouchesWithLesserPriorities()
		{
			gkTouchUsageManager.CancelOtherUsedTouches(m_oCurrentTouchInfos.TouchIndex, this);
		}
		
		private void CancelAllOtherUsedTouches()
		{
			gkTouchUsageManager.CancelOtherUsedTouches(m_oCurrentTouchInfos.TouchIndex, this, false);
		}
		
		private bool TryToGetTheTouchIfInTheZone(ETouchInstigator a_eInstigator, int a_iTouchIndex, Vector2 a_f2TouchPosition)
		{
			if(IsInTheTouchZone(a_f2TouchPosition) && CanTheTouchBeUsed(a_iTouchIndex))
			{
				m_oCurrentTouchInfos.StartTouch(a_eInstigator, a_iTouchIndex, a_f2TouchPosition);
				return true;
			}
			return false;
		}
		
		public bool IsInTheTouchZone(Vector2 a_f2TouchPosition)
		{
			return IsInTheTouchZone(a_f2TouchPosition, touchZones, true) 
				&& IsInTheTouchZone(a_f2TouchPosition, touchZonesExcluded, false) == false;
		}
		
		private bool IsInTheTouchZone(Vector2 a_f2TouchPosition, List<gkTouchZoneBase> a_rTouchZones, bool a_bInfiniteZoneIfNoZones)
		{
			// If there isn't any touch zone we can touch anywhere in the screen
			if(a_rTouchZones.Count == 0)
			{
				return a_bInfiniteZoneIfNoZones;
			}
			else
			{
				// loop through the touch zones to see if we are touching at least one
				foreach(gkTouchZoneBase rTouchZone in a_rTouchZones)
				{
					if(rTouchZone == null)
					{
						return true;
					}
					else
					{
						if(rTouchZone.ContainsScreenPoint(a_f2TouchPosition))
						{
							return true;
						}
					}
				}
				return false;
			}
		}
		
		private bool TryToUpdateCurrentTouchPosition()
		{
			if(m_oCurrentTouchInfos.TryToUpdateTouchPosition())
			{
				if(IsTouchStillValidOnTouched(m_oCurrentTouchInfos.CurrentTouchPosition))
				{
					if(HasTouchEnded(m_oCurrentTouchInfos.CurrentTouchPosition) == false)
					{
						return true;
					}
				}
				else
				{
					m_bTouchHasBeenCanceled = true;
				}
			}
			else
			{
				m_eTouchEndType = ETouchEndType.Release;
			}
				
			return false;
		}
		
		private bool IsTouchStillValidOnTouched(Vector2 a_f2TouchPosition)
		{
			switch(touchCancelationMode)
			{
				case ETouchCancelationMode.CancelWhenCursorOutOfZone:
				{
					if(IsInTheTouchZone(a_f2TouchPosition) == false)
					{
						m_eTouchEndType = ETouchEndType.Release;
						return false;	
					}
				}
				break;
				
				case ETouchCancelationMode.CancelWhenSwiped:
				{
					if(IsSwiping)
					{
						m_eTouchEndType = ETouchEndType.Swipe;
						return false;
					}
				}
				break;
			}
			
			return true;
		}
		
		private bool HasTouchEnded(Vector2 a_f2TouchPosition)
		{		
			switch(touchEndMode)
			{
				case ETouchEndMode.EndOnSwipe:
				case ETouchEndMode.EndOnSwipeOrOnRelease:
				{
					if(IsSwiping)
					{
						m_eTouchEndType = ETouchEndType.Swipe;
						return true;
					}
				}
				break;
				
				case ETouchEndMode.EndOnHold:
				{
					if(HasHeldTheCurrentTouchLongEnough())
					{
						m_eTouchEndType = ETouchEndType.Hold;
						return true;
					}
				}
				break;
			}
			
			return false;
		}
		
		private bool IsTouchASuccess(Vector2 a_f2TouchPosition)
		{	
			// Enough?
			switch(touchEndMode)
			{
				case ETouchEndMode.EndOnRelease:
				{
					if(touchCancelationMode == ETouchCancelationMode.CancelWhenReleaseOutOfZone && IsInTheTouchZone(a_f2TouchPosition) == false)
					{
						return false;
					}
				}
				break;
				
				case ETouchEndMode.EndOnHold:
				{
					if(m_eTouchEndType != ETouchEndType.Hold)
					{
						return false;
					}
				}
				break;
				
				case ETouchEndMode.EndOnSwipe:
				{
					if(m_eTouchEndType != ETouchEndType.Swipe)
					{
						return false;
					}
				}
				break;
				
				case ETouchEndMode.EndOnSwipeOrOnRelease:
				{
					if(touchCancelationMode == ETouchCancelationMode.CancelWhenReleaseOutOfZone && IsInTheTouchZone(a_f2TouchPosition) == false)
					{
						return false;
					}
				}
				break;
			}
			
			return true;
		}
		
		private bool IsTheCurrentTouchASwipe()
		{
			if(m_bTouched)
			{
				return swipe.IsASwipe(m_oCurrentTouchInfos.StartTouchPosition, m_oCurrentTouchInfos.CurrentTouchPosition);
			}
			else
			{
				return false;
			}
		}
		
		private bool HasHeldTheCurrentTouchLongEnough()
		{
			return hold.HasHeldLongEnough();
		}
		
		void AddClick()
		{
			m_iNumberOfClick++;
			m_fTimeSinceLastClick = 0.0f;
		}
		
		bool EnoughClick()
		{
			return m_iNumberOfClick >= clickCountRules.numberOfClickNeeded;
		}
		
		void ProcessClickCount()
		{
			if(m_fTimeSinceLastClick >= clickCountRules.clickRemanenceDuration)
			{
				m_iNumberOfClick = 0;
			}
			else
			{
				m_fTimeSinceLastClick += Time.deltaTime;
			}
		}
		
		private void AccordTouchLayerIDToName()
		{
			m_iTouchSortingLayerID = gkTouchLayer.TouchLayerNameToID(touchSortingLayerName);
		}
		
		private void AccordTouchLayerNameToID()
		{
			touchSortingLayerName = gkTouchLayer.TouchLayerIDToName(m_iTouchSortingLayerID);
		}
	}
}