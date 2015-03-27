using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace gk
{
	[AddComponentMenu("GK/Input/ControlUIButtonWithGKButton")]
	public class ControlUIButtonWithGKButton : MonoBehaviour
	{
		public gkButton button;

		public UnityEngine.UI.Button uiButton;

		void Awake()
		{
			button.onDown += OnDown;
			button.onClick += OnClick;
		}

		void OnDestroy()
		{
			button.onDown -= OnDown;
			button.onClick -= OnClick;
		}

		void OnDown()
		{
		}

		void OnClick()
		{
			uiButton.onClick.Invoke();
		}
	}
}