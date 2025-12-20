using Zenject;
using System;

namespace IGF
{
	public class HayBaleSpawner : IHayBaleSpawner, IHayBaleSpawnEvents
	{
		[Inject(Id = HayBaleType.Yellow)] private HayBalePool _hayBaleYellowPool;
		[Inject(Id = HayBaleType.Green)] private HayBalePool _hayBaleGreenPool;

		public event Action<HayBale> OnSpawned;

		void IHayBaleSpawner.Spawn(HayBaleType type)
		{
			HayBale hayBale;
			
			switch (type)
			{
				case HayBaleType.Green:
				default:
					hayBale = _hayBaleGreenPool.Get();
					break;
				case HayBaleType.Yellow:
					hayBale = _hayBaleYellowPool.Get();
					break;
			}
			
			OnSpawned?.Invoke(hayBale);
		}
	}
}