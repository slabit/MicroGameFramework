using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace gk
{
	[AddComponentMenu("GK/Input/ButtonActionTrigger_OnHold")]
	public class ButtonActionTrigger_OnHold : MonoBehaviour
	{
		public gkButton button;
		
		public UnityEvent onEvent;
		
		void Awake()
		{
			button.onHold += OnEvent;
		}
		
		void OnDestroy()
		{
			button.onHold -= OnEvent;
		}
		
		void OnEvent()
		{
			onEvent.Invoke();
		}
	}
}