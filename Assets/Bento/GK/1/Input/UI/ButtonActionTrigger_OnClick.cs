using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace gk
{
	[AddComponentMenu("GK/Input/ButtonActionTrigger_OnClick")]
	public class ButtonActionTrigger_OnClick : MonoBehaviour
	{
		public gkButton button;
		
		public UnityEvent onEvent;
		
		void Awake()
		{
			button.onClick += OnEvent;
		}
		
		void OnDestroy()
		{
			button.onClick -= OnEvent;
		}
		
		void OnEvent()
		{
			onEvent.Invoke();
		}
	}
}