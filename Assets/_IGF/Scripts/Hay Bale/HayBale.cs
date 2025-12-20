using UnityEngine;
using System;

namespace IGF
{
	public class HayBale : MonoBehaviour
	{
		public event Action<HayBale> OnDestryed;

		public void SetParent(Transform parent)
		{
			transform.SetParent(parent, false);
			transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
		}

		public void Destroy() => OnDestryed?.Invoke(this);
	}
}