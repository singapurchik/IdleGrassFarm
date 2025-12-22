using UnityEngine;
using System;
using IGF.Wallet;

namespace IGF
{
	public class HayBale : MonoBehaviour
	{
		[field: SerializeField] public CurrencyType CurrencyForSaleType { get; private set; }
		
		public event Action<HayBale> OnDestroyed;

		public void Destroy() => OnDestroyed?.Invoke(this);
	}
}