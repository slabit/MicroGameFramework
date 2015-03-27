using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace gk
{
	public enum EGUILayoutOrientation
	{
		Previous,
		Horizontal,
		Vertical
	}
	
	public static class gkGUILayoutUtility
	{
		public static bool Button(string a_oButtonLabel, Vector2 a_f2Size, EGUILayoutOrientation a_eOrientation, bool a_bCentered)
		{
			bool bButtonClicked;
			
			BeginOrientation(a_eOrientation);
			{
				if(a_bCentered)
				{
					GUILayout.FlexibleSpace();
				}
				
				bButtonClicked = GUILayout.Button(a_oButtonLabel, GUILayout.Width(a_f2Size.x), GUILayout.Height(a_f2Size.y));
				
				if(a_bCentered)
				{
					GUILayout.FlexibleSpace();
				}
			}
			EndOrientation(a_eOrientation);
			
			return bButtonClicked;
		}
		
		public static void Label(string a_oLabel, EGUILayoutOrientation a_eOrientation, bool a_bCentered)
		{
			BeginOrientation(a_eOrientation);
			{
				if(a_bCentered)
				{
					GUILayout.FlexibleSpace();
				}
				
				GUILayout.Label(a_oLabel);
				
				if(a_bCentered)
				{
					GUILayout.FlexibleSpace();
				}
			}
			EndOrientation(a_eOrientation);
		}
		
		public static void BeginOrientation(EGUILayoutOrientation a_eOrientation)
		{
			switch(a_eOrientation)
			{
				case EGUILayoutOrientation.Previous:
				{
					// Nothing
				}
				break;
				
				case EGUILayoutOrientation.Horizontal:
				{
					GUILayout.BeginHorizontal();
				}
				break;
				
				case EGUILayoutOrientation.Vertical:
				{
					GUILayout.BeginVertical();
				}
				break;
			}
		}
		
		public static void EndOrientation(EGUILayoutOrientation a_eOrientation)
		{
			switch(a_eOrientation)
			{
				case EGUILayoutOrientation.Previous:
				{
					// Nothing
				}
				break;
				
				case EGUILayoutOrientation.Horizontal:
				{
					GUILayout.EndHorizontal();
				}
				break;
				
				case EGUILayoutOrientation.Vertical:
				{
					GUILayout.EndVertical();
				}
				break;
			}
		}
	}
}