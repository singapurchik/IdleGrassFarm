using UnityEngine;
using System;

namespace IGF
{
	public class HayBale : MonoBehaviour
	{
		public event Action<HayBale> OnDestroyed;

		public void Destroy() => OnDestroyed?.Invoke(this);
	}
}