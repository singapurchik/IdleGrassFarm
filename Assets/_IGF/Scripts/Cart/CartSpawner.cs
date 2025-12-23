using System.Collections.Generic;
using UnityEngine;
using VInspector;

namespace IGF
{
	public class CartSpawner : MonoBehaviour, ICartSpawner
	{
		[SerializeField] private List<Cart> _carts;
		[SerializeField] private Cart _cartPrefab;

		private int _currentIndex;

		private void Start()
		{
			Spawn();
		}

		public void Spawn()
		{
			if (_currentIndex < _carts.Count)
			{
				_carts[_currentIndex].gameObject.SetActive(true);
				_currentIndex++;
			}
			else
			{
				Instantiate(_cartPrefab, transform);
			}
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