using UnityEngine;
using System.Collections;

using MicroGameFramework;

namespace MicroGameFrameworkSample
{
	[AddComponentMenu("MicroGameFrameworkSample/ButtonGameOver")]
	public class ButtonGameOver : MonoBehaviour
	{
		public void OnClick()
		{
			Game.Instance.NotifyGameOver();
		}
	}
}