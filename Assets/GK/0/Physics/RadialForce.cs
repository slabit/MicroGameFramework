using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine.UI;

namespace gk
{
	[AddComponentMenu("GK/Animator/RadialForce")]
	public class RadialForce : MonoBehaviour
	{
		[Serializable]
		public class GizmoOptions
		{
			public bool alsoDisplayWhenUnselected = true;
			public Color radiusColor = new Color(0.0f, 1.0f, 0.0f);
			public Color falloffRadiusColor = new Color(0.0f, 0.75f, 0.0f);
		}

		public bool deactivateWhenLowPhysics = true;

		public bool trigger;

		public bool play;

		public bool x = true;
		public bool y = true;
		public bool z = false;

		public LayerMask layerMask = unchecked((int)0xFFFFFFFF);

		public float radius = 10.0f;

		public float falloff = 0.0f;

		public float force = 10.0f;

		public float upwardsModifier;

		public bool applyForceOnGravityCenter = true;

		public ForceMode mode = ForceMode.Impulse;

		public GizmoOptions gizmo = new GizmoOptions();

		HashSet<Rigidbody> effectedRigidbodies = new HashSet<Rigidbody>();

		bool lastTriggerState;

		public Vector3 Center
		{
			get
			{
				return transform.position;
			}
		}

		public float TotalRadius
		{
			get
			{
				return radius + falloff;
			}
		}

		void FixedUpdate()
		{
			if(gk.gkTime.Paused)
				return;

			if(play == false)
			{
				if(trigger == lastTriggerState)
					return;
			}
			lastTriggerState = trigger;

			#if lowPhysics
			if(deactivateWhenLowPhysics)
			{
				return;
			}
			#endif

			Collider[] effectedColliders = Physics.OverlapSphere(Center, TotalRadius, layerMask.value);
			effectedRigidbodies.Clear();
			foreach(Collider effectedCollider in effectedColliders)
			{
				if(effectedCollider.attachedRigidbody != null)
					effectedRigidbodies.Add(effectedCollider.attachedRigidbody);
			}

			foreach(Rigidbody effectedRigidbody in effectedRigidbodies)
			{
				Vector3 distanceFromSourceVector = effectedRigidbody.position - Center;

				Vector3 modifiedCenter = Center - upwardsModifier * Vector3.up;
				Vector3 distanceFromModifierCenter = effectedRigidbody.position - modifiedCenter;

				if(!x)
				{
					distanceFromSourceVector.x = 0.0f;
					distanceFromModifierCenter.x = 0.0f;
				}
				
				if(!y)
				{
					distanceFromSourceVector.y = 0.0f;
					distanceFromModifierCenter.y = 0.0f;
				}
				
				if(!z)
				{
					distanceFromSourceVector.z = 0.0f;
					distanceFromModifierCenter.z = 0.0f;
				}

				float forceValue;
				float distanceFromSource = distanceFromSourceVector.magnitude;
				if(distanceFromSource == 0.0f)
				{
					forceValue = force;
				}
				else
				{
					float fallOffPercent = 0.0f;
					if(distanceFromSource > radius)
					{
						fallOffPercent = (distanceFromSource - radius)/falloff;
						fallOffPercent = Mathf.Clamp01(fallOffPercent);
					}

					forceValue = force * (1.0f - fallOffPercent);
				}

				Vector3 forceVector = distanceFromModifierCenter.normalized * forceValue;
				if(applyForceOnGravityCenter)
				{
					effectedRigidbody.AddForceAtPosition(forceVector, -distanceFromModifierCenter, mode);
				}
				else
				{
					effectedRigidbody.AddForce(forceVector, mode);
				}
			}
		}

		void OnDrawGizmos()
		{
			if(gizmo.alsoDisplayWhenUnselected)
				DisplayGizmos();
		}

		void OnDrawGizmosSelected()
		{
			DisplayGizmos();
		}

		void DisplayGizmos()
		{
			if(enabled == false)
				return;
			Gizmos.color = gizmo.radiusColor;
			Gizmos.DrawWireSphere(Center, radius);

			Gizmos.color = gizmo.falloffRadiusColor;
			Gizmos.DrawWireSphere(Center, radius + falloff);
		}
	}
}