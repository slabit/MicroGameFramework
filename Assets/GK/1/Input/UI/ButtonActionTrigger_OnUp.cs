using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace gk
{
	[AddComponentMenu("GK/Input/ButtonActionTrigger_OnUp")]
	public class ButtonActionTrigger_OnUp : MonoBehaviour
	{
		public gkButton button;
		
		public UnityEvent onEvent;
		
		void Awake()
		{
			button.onUp += OnEvent;
		}
		
		void OnDestroy()
		{
			button.onUp -= OnEvent;
		}
		
		void OnEvent()
		{
			onEvent.Invoke();
		}
	}
}