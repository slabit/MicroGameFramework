using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace gk
{
	[AddComponentMenu("GK/Physics2D/Physics2D_CollisionNotifier_OnTriggerEnter2D")]
	public class Physics2D_CollisionNotifier_OnTriggerEnter2D : MonoBehaviour
	{
		public delegate void CollisionDelegate(Collider collider, Physics2D_CollisionNotifier_OnTriggerEnter2D collisionNotifier);

		public CollisionDelegate onTriggerEnter2D;

		public static Physics2D_CollisionNotifier_OnTriggerEnter2D AddCollisionNotifier(GameObject observedGameObject,
		                                                                                  CollisionDelegate collisionDelegate)
		{
			Physics2D_CollisionNotifier_OnTriggerEnter2D notifier = observedGameObject.AddComponent<Physics2D_CollisionNotifier_OnTriggerEnter2D>();
			notifier.onTriggerEnter2D += collisionDelegate;

			return notifier;
		}

		void OnTriggerEnter(Collider collider)
		{
			if(onTriggerEnter2D != null)
			{
				onTriggerEnter2D(collider, this);
			}
		}
	}
}