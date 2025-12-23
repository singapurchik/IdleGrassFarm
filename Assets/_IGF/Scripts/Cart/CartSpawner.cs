using System.Collections.Generic;
using UnityEngine;
using VInspector;
using Zenject;

namespace IGF
{
	public class CartSpawner : MonoBehaviour, ICartSpawner
	{
		[SerializeField] private List<Cart> _carts;
		[SerializeField] private Cart _cartPrefab;

		[Inject] private DiContainer _diContainer;
		[Inject] private ICartTarget _firstTarget;
		
		private readonly List<Cart> _spawnedCarts = new (10);
		
		private int _currentIndex;

		private void Start()
		{
			Spawn();
		}

		public void Spawn()
		{
			Cart cart;

			if (_currentIndex < _carts.Count)
			{
				cart = _carts[_currentIndex];
				cart.gameObject.SetActive(true);
				_currentIndex++;
			}
			else
			{
				cart = _diContainer.InstantiatePrefabForComponent<Cart>(_cartPrefab, transform);
			}

			var target = _spawnedCarts.Count == 0 ? _firstTarget : _spawnedCarts[^1];
			_spawnedCarts.Add(cart);
			cart.Initialize(target);
		}
		
#if UNITY_EDITOR
		[Button]
		private void FindDependencies()
		{
			_carts.Clear();
			_carts.AddRange(FindObjectsOfType<Cart>(true));
		}
#endif
	}
}