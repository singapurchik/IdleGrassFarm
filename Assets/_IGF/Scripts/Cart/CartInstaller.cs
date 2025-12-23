using UnityEngine;
using VInspector;
using Zenject;

namespace IGF
{
	public class CartInstaller : MonoInstaller
	{
		[SerializeField] private CartMover _mover;
		[SerializeField] private Cart _cart;

		public override void InstallBindings()
		{
			Container.BindInstance(_mover).WhenInjectedIntoInstance(_cart);
		}
		
#if UNITY_EDITOR
		[Button]
		private void FindDependencies()
		{
			_mover = GetComponentInChildren<CartMover>(true);
			_cart = GetComponentInChildren<Cart>(true);
		}
#endif
	}
}