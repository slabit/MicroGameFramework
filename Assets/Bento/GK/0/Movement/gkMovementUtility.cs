using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gk
{
	public static class gkMovementUtility
	{
		public static Vector3 FallTo(Vector3 current, Vector3 target, ref Vector3 currentVelocity, float gravity, float deltaTime)
		{
			if(target == current)
				return target;
			
			Vector3 fallingDirection = (target - current).normalized;	
			
			currentVelocity +=  gravity * fallingDirection * deltaTime;
			
			current += currentVelocity * deltaTime;
			
			Vector3 newFallingDirection = target - current;
			if(Vector3.Dot(newFallingDirection, fallingDirection) <= 0.0f)
			{
				current = target;
				currentVelocity = Vector2.zero;
			}
			else
			{
				if(newFallingDirection.x * fallingDirection.x <= 0.0f)
				{
					current.x = target.x;
					currentVelocity.x = 0.0f;
				}
				
				if(newFallingDirection.y * fallingDirection.y <= 0.0f)
				{
					current.y = target.y;
					currentVelocity.y = 0.0f;
				}
			}
			
			return current;
		}
	}
}