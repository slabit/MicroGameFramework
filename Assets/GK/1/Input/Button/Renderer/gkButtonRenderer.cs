using UnityEngine;
using System.Collections;
using System;

namespace gk
{
	[AddComponentMenu("GK/Input/Button/Renderer/gkButtonRenderer")]
	public abstract class gkButtonRenderer : MonoBehaviour
	{
		public gkButton button;
		
		public float downDelay = 0.0f;
		
		private bool m_bWaitForDown = false;
		
		public float m_fWaitTimeRemainingBeforeDown = 0.0f;
		
		// Call this after created a buton renderer at runtime
		public void AfterRuntimeCreation()
		{
			Initialize();
		}
		
		protected abstract void SetUp();
		
		protected abstract void SetDown();
		
		protected virtual void OnEnable()
		{
			if(button != null)
			{
				Initialize();
			}
		}
		
		protected virtual void OnDisable()
		{
			if(button != null)
			{
				Terminate();
			}
		}
		
		protected virtual void Update()
		{
			if(m_bWaitForDown)
			{
				m_fWaitTimeRemainingBeforeDown -= Time.deltaTime;
				if(m_fWaitTimeRemainingBeforeDown <= 0.0f)
				{
					m_bWaitForDown = false;
					SetDown();
				}
			}
		}
		
		protected virtual void Initialize()
		{
			if(button.enabled)
			{
				if(button.Pressed)
				{
					OnDown();
				}
				else
				{
					OnUp();
				}
			}
			
			button.onUp += OnUp;
			button.onDown += OnDown;
		}
		
		private void Terminate()
		{
			button.onUp += OnUp;
			button.onDown += OnDown;
		}
		
		private void OnUp()
		{
			m_bWaitForDown = false;
			SetUp();
		}
		
		private void OnDown()
		{
			if(downDelay > 0)
			{
				m_bWaitForDown = true;
				m_fWaitTimeRemainingBeforeDown = downDelay;
			}
			else
			{
				m_bWaitForDown = false;
				SetDown();
			}
		}
	}
}