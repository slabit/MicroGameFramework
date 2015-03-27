using UnityEngine;
using System.Collections.Generic;

namespace gk
{
	public class PositionsSnapshot
	{
		public class SnapshotItem
		{
			Vector2 position;
			Quaternion rotation;
			Transform transform;
			
			public SnapshotItem(Transform transform)
			{
				position = transform.localPosition;
				rotation = transform.localRotation;
				this.transform = transform;
			}
			
			public void Restore()
			{
				transform.localPosition = position;
				transform.localRotation = rotation;
			}
		}
		
		List<SnapshotItem> snapshotItems = new List<SnapshotItem>();

		public static gk.PositionsSnapshot CreateSnapshot(Component component)
		{
			return new gk.PositionsSnapshot(component.transform);
		}

		public static gk.PositionsSnapshot CreateSnapshot<ComponentType>(IEnumerable<ComponentType> components) where ComponentType : Component
		{
			gk.PositionsSnapshot snapshot = new gk.PositionsSnapshot();
			foreach(Component component in components)
			{
				snapshot.snapshotItems.Add(new SnapshotItem(component.transform));
			}

			return snapshot;
		}

		public PositionsSnapshot()
		{
		}

		public PositionsSnapshot(Transform transform)
		{
			snapshotItems.Add(new SnapshotItem(transform));
		}

		public PositionsSnapshot(List<Transform> transforms)
		{
			foreach(Transform transform in transforms)
			{
				snapshotItems.Add(new SnapshotItem(transform));
			}
		}
		
		public void Restore()
		{
			foreach(SnapshotItem snapshotItem in snapshotItems)
			{
				snapshotItem.Restore();
			}
		}
	}
}