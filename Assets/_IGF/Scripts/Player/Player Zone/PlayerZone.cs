using UnityEngine;
using System;

namespace IGF.Players
{
	public class PlayerZone : MonoBehaviour, IPlayerZoneInfo, IPlayerZoneChanger
	{
		private ZoneType _requestedZoneType;
		
		private bool _isChangeZoneRequested;
		
		public ZoneType CurrentZoneType { get; private set; } = ZoneType.Default;
		
		public event Action<ZoneType> OnEnterZone;
		public event Action<ZoneType> OnExitZone;


		void IPlayerZoneChanger.RequestChangeZone(ZoneType zoneType)
		{
			_requestedZoneType =  zoneType;
			_isChangeZoneRequested = true;
		}

		private void TryChangeZone(ZoneType zoneType)
		{
			if (CurrentZoneType != zoneType)
			{
				OnExitZone?.Invoke(CurrentZoneType);
				CurrentZoneType = zoneType;
				OnEnterZone?.Invoke(CurrentZoneType);
			}
		}

		private void Update()
		{
			if (_isChangeZoneRequested)
			{
				TryChangeZone(_requestedZoneType);
				_isChangeZoneRequested = false;
			}
			else
			{
				TryChangeZone(ZoneType.Default);
			}
		}
	}
}