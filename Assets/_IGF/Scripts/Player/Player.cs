using UnityEngine;
using Zenject;

namespace IGF.Players
{
	public class Player : MonoBehaviour, IPlayer
	{
		[Inject] private PlayerStateMachine _stateMachine;

		private void Start()
		{
			_stateMachine.Initialize();
		}
	}
}