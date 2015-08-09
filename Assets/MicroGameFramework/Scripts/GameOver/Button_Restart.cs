using UnityEngine;
using System.Collections;

namespace MicroGameFramework
{
	[AddComponentMenu("MicroGameFramework/Button_Restart")]
	public class Button_Restart : MonoBehaviour
	{
		public void OnClick()
		{
			Game.Instance.Restart();
		}
	}
}