using System.Collections.Generic;
using UnityEngine;

namespace IGF.Buyers.Animations
{
	public class UpperBodyLayer : Layer
	{
		private static readonly int _holdingHayBaleAnimHash = Animator.StringToHash("Holding Hay Bale");
		
        protected override float EnableSpeed => 15f;

		public UpperBodyLayer(Animator animator, int index, List<Layer> layersList) : base(animator, index, layersList)
		{
		}

		public void PlayHoldingHayBaleAnim() => Animator.Play(_holdingHayBaleAnimHash);

		public void StopHoldingHayBaleAnim() => Animator.Play(EmptyAnimHash);
	}
}