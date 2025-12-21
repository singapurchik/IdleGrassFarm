using UnityEngine;
using Zenject;

namespace IGF.Buyers
{
	public class Buyer : MonoBehaviour
	{
		[Inject] private BuyerStateMachine _stateMachine;

		private void Start()
		{
			_stateMachine.Initialize();
		}
	}
}