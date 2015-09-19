using UnityEngine;
using System.Collections;
using System;

namespace gk
{
	[AddComponentMenu("GK/Input/Button/gkButton")]
	public class gkButton : MonoBehaviour
	{
		public enum EButtonType
		{
			ClickButton,
			ToggleButton
		}
		
		[Serializable]
		public class HoldParameters
		{
			public float holdDelay = 0.5f;
			
			private float m_fHoldDuration;
			
			private bool m_bHeld;
			
			public bool IsHeld
			{
				get
				{
					return m_bHeld;
				}
			}
			
			public void OnDown()
			{
				m_fHoldDuration = 0.0f;
			}
			
			public void OnUp()
			{
				m_bHeld = false;
				m_fHoldDuration = 0.0f;
			}
			
			public bool Update()
			{
				m_fHoldDuration += Time.deltaTime;
				if(m_bHeld == false)
				{
					if(m_fHoldDuration >= holdDelay)
					{
						m_bHeld = true;
						return true;
					}
				}	
				return false;
			}
		}
		
		[Serializable]
		public class RepeatParameters
		{
			public bool repeatButton;
			
			public float firstRepeatButtonDelay = 0.25f;
			
			public float repeatButtonPeriod = 0.05f;
			
			private float m_fPressedTimeRemainingBeforeNextRepeat;
			
			public bool Update()
			{
				if(repeatButton)
				{
					m_fPressedTimeRemainingBeforeNextRepeat -= Time.deltaTime;
					if(m_fPressedTimeRemainingBeforeNextRepeat <= 0.0f)
					{
						m_fPressedTimeRemainingBeforeNextRepeat = repeatButtonPeriod;
						return true;
					}
				}
				return false;
			}
			
			public void OnDown()
			{
				m_fPressedTimeRemainingBeforeNextRepeat = firstRepeatButtonDelay;
			}
		}
		
		[Serializable]
		public class CancelParameters
		{
			public bool onHold;
			public bool onDisable;
		}
		
		[Serializable]
		public class ToggleParameters
		{
			public bool onHold;
		}
		
		public Action<gkButton, bool> onStateChange;

		public Action onEnable;

		public Action<bool> onToggle;
		
		public Action onDown;
		
		public Action onUp;
		
		public Action onClick; 
		
		public Action onHold;

		public EButtonType buttonType = EButtonType.ClickButton;
		
		public HoldParameters hold = new HoldParameters();
		
		public RepeatParameters repeat = new RepeatParameters();
		
		public CancelParameters cancel = new CancelParameters();

		private bool m_bPressed;

		bool wasPressedOnDisable;

		public bool Pressed
		{
			get
			{
				return m_bPressed;
			}
		}
		
		public void Toggle()
		{
			Toggle(!m_bPressed);
		}
		
		public void Toggle(bool a_bPressed)
		{
			if(a_bPressed)
			{
				_Press();
			}
			else
			{
				_Release();
			}
		}
		
		public void Press()
		{
			switch(buttonType)
			{
				case EButtonType.ClickButton:
				{
					if(m_bPressed == false)
					{
						_Press();
					}
				}
				break;
				
				case EButtonType.ToggleButton:
				{
					if(m_bPressed)
					{
						_Release();
					}
					else
					{
						_Press();
					}
				}
				break;
			}
		}
		
		public void Release()
		{
			switch(buttonType)
			{
				case EButtonType.ClickButton:
				{
					if(m_bPressed)
					{
						_Release();
					}
				}
				break;
				
				case EButtonType.ToggleButton:
				{
				}
				break;
			}
		}
		
		public void Cancel()
		{
			if(m_bPressed)
			{
				_Cancel();
			}
		}

		void OnEnable()
		{
			if(onEnable != null)
				onEnable();
		}

		void OnDisable()
		{
			if(cancel.onDisable)
				Cancel();
		}

		private void Update()
		{
			if(m_bPressed)
			{
				if(hold.Update())
				{
					Hold();
				}
				
				if(repeat.Update())
				{
					_Release();
					_Press();
				}
			}
		}
		
		private void _Press()
		{	
			Down();
			NotifyStateChange(false);
		}
		
		private void _Release()
		{
			Click();
			Up();
			NotifyStateChange(false);
		}
		
		private void _Cancel()
		{
			Up();
			NotifyStateChange(true);
		}
		
		private void Down()
		{	
			m_bPressed = true;
			hold.OnDown();
			repeat.OnDown();
			NotifyDown();
			NotifyToggle(true);
		}
		
		private void Up()
		{
			m_bPressed = false;
			hold.OnUp();
			NotifyUp();
			NotifyToggle(false);
		}
		
		private void Click()
		{
			if(buttonType != EButtonType.ToggleButton)
			{
				NotifyClick();
			}
		}
		
		private void Hold()
		{
			if(cancel.onHold)
			{
				_Cancel();
			}
			NotifyHold();
		}
		
		private void NotifyToggle(bool a_bPressed)
		{
			if(buttonType == EButtonType.ToggleButton)
			{
				if(onToggle != null)
				{
					onToggle(a_bPressed);
				}
			}
		}
		
		private void NotifyStateChange(bool a_bCancel)
		{
			if(onStateChange != null)
			{
				onStateChange(this, a_bCancel);
			}
		}
		
		private void NotifyDown()
		{
			if(onDown != null)
			{
				onDown();
			}
		}
		
		private void NotifyUp()
		{
			if(onUp != null)
			{
				onUp();
			}
		}
		
		private void NotifyClick()
		{
			if(onClick != null)
			{
				onClick();
			}
		}
		
		private void NotifyHold()
		{
			if(onHold != null)
			{
				onHold();
			}
		}
	}
}