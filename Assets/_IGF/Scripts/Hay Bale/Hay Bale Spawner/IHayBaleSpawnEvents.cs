using System;

namespace IGF
{
	public interface IHayBaleSpawnEvents
	{
		public event Action<HayBale> OnSpawned;
	}
}