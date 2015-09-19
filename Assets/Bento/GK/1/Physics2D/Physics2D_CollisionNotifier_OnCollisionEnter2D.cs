using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace gk
{
	[AddComponentMenu("GK/Physics2D/Physics2D_CollisionNotifier_OnCollisionEnter2D")]
	public class Physics2D_CollisionNotifier_OnCollisionEnter2D : MonoBehaviour
	{
		public delegate void CollisionDelegate(Collision collision, Physics2D_CollisionNotifier_OnCollisionEnter2D collisionNotifier);

		public CollisionDelegate onCollisionEnter2D;

		public static Physics2D_CollisionNotifier_OnCollisionEnter2D AddCollisionNotifier(GameObject observedGameObject,
		                                                                                  CollisionDelegate collisionDelegate)
		{
			Physics2D_CollisionNotifier_OnCollisionEnter2D notifier = observedGameObject.AddComponent<Physics2D_CollisionNotifier_OnCollisionEnter2D>();
			notifier.onCollisionEnter2D += collisionDelegate;

			return notifier;
		}

		void OnCollisionEnter(Collision collision)
		{
			if(onCollisionEnter2D != null)
			{
				onCollisionEnter2D(collision, this);
			}
		}
	}
}