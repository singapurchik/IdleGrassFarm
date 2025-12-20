using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace IGF
{
	public class HayBalesDistributor : MonoBehaviour
	{
		[Inject] private IHayBaleSpawnEvents _events;
		
		private static readonly List<IHayBaleHolder> _holders = new (6);

		private void OnEnable()
		{
			_events.OnSpawned += OnHayBaleSpawned;
		}

		private void OnDisable()
		{
			_events.OnSpawned -= OnHayBaleSpawned;
		}

		public static void SetHolder(IHayBaleHolder holder)
		{
			if (!_holders.Contains(holder))
				_holders.Add(holder);
		}

		private void OnHayBaleSpawned(HayBale hayBale)
		{
			bool isHasParent = false;
			
			for (int i = 0; i < _holders.Count; i++)
			{
				var holder = _holders[i];
				
				if (holder.TryGetPoint(out var point))
				{
					hayBale.SetParent(point);
					isHasParent = true;
					break;
				}
			}
			
			if (!isHasParent)
				hayBale.Destroy();
		}
	}
}