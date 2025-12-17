using UnityEngine;
using Zenject;

namespace IGF.Players
{
	public class Player : MonoBehaviour
	{
		[Inject] private PlayerStateMachine _stateMachine;
		[Inject] private IPlayerZoneInfo _zoneInfo;

		private void Start()
		{
			_stateMachine.Initialize();
		}

		private void Update()
		{
			if (_zoneInfo.CurrentZoneType == ZoneType.Default)
				_stateMachine.TrySwitchToDefaultState();
			else if (_zoneInfo.CurrentZoneType == ZoneType.Grass)
				_stateMachine.TrySwitchToAttackState();
		}
	}
}