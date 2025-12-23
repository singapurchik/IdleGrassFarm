using UnityEngine;
using Zenject;

namespace IGF
{
	public class Cart : MonoBehaviour, ICartTarget
	{
		[Inject] private CartMover _mover;
		
		public Vector3 Position => transform.position;
		public Vector3 Forward => transform.forward;
		
		
		public void Initialize(ICartTarget target) => _mover.SetTarget(target);
	}
}