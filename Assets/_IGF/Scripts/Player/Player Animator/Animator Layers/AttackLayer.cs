using System.Collections.Generic;
using UnityEngine;

namespace IGF.Players.Animations
{
	public class AttackLayer : Layer
	{
		private readonly int _isAttackBoolHash = Animator.StringToHash("isAttack");
		
		public AttackLayer(Animator animator, int index, List<Layer> layersList) : base(animator, index, layersList)
		{
		}

		public void StopAttackAnim() => Animator.SetBool(_isAttackBoolHash, false);
		
		public void PlayAttackAnim() => Animator.SetBool(_isAttackBoolHash, true);
	}
}