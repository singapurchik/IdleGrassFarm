using UnityEngine.Animations.Rigging;
using UnityEngine;
using Zenject;

namespace IGF.Players.AnimRig
{
	public class PlayerAnimationRigging : MonoBehaviour
	{
		[Inject] private RigBuilder _rigBuilder;
		
		private readonly ChainConstraintWrapper _leftHandRig = new ();
		
		private Transform _currentLeftHandTarget;

		public void RequestEnableLeftHand(Transform target = null)
		{
			_currentLeftHandTarget = target;
			_leftHandRig.RequestEnable();
		}

		private void Update()
		{
			_leftHandRig.Update(_currentLeftHandTarget);
		}
	}
}