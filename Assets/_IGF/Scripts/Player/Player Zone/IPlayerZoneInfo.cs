using System;

namespace IGF.Players
{
	public interface IPlayerZoneInfo
	{
		public ZoneType CurrentZoneType { get; }

		public event Action<ZoneType> OnEnterZone;
		public event Action<ZoneType> OnExitZone;
	}
}