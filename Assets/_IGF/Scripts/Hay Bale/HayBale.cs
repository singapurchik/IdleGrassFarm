using UnityEngine;
using IGF.Wallet;
using System;

namespace IGF
{
	public class HayBale : MonoBehaviour
	{
		[field: SerializeField] public CurrencyType CurrencyForSaleType { get; private set; }
		
		public event Action<HayBale> OnDestroyed;

		public void Destroy() => OnDestroyed?.Invoke(this);
	}
}