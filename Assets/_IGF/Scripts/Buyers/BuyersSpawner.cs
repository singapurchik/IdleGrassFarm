using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace IGF.Buyers
{
	public class BuyersSpawner : MonoBehaviour
	{
		[SerializeField] private Transform _spawnPoint;
		[SerializeField] private float _spawnDelay = 1f;

		[Inject] private BuyersQueue _queue;
		[Inject] private BuyersPool _pool;

		private readonly List<Buyer> _buyers = new(6);
		
		private float _nextSpawnTime;

		private void OnDisable()
		{
			for (int i = 0; i < _buyers.Count; i++)
			{
				var buyer = _buyers[i];
				if (buyer == null) continue;

				buyer.OnPurchaseCompleted -= OnBuyerPurchaseCompleted;
			}
		}

		private void Spawn(Transform queuePoint)
		{
			var buyer = _pool.Get();
			buyer.RequestTeleport(_spawnPoint.position);
			buyer.SetMovementTarget(queuePoint.position);

			_buyers.Add(buyer);
			buyer.OnPurchaseCompleted += OnBuyerPurchaseCompleted;
		}

		private void OnBuyerPurchaseCompleted(Buyer buyer)
		{
			if (_buyers.Count == 0 || _buyers[0] != buyer)
				return;

			buyer.OnPurchaseCompleted -= OnBuyerPurchaseCompleted;
			_buyers.RemoveAt(0);

			_queue.SetEmptyPoint();

			ReassignTargets();

			// Дальше он нам не интересен: пусть уходит сам (у тебя это в FSM)
			// buyer.SetMovementTarget(_exitPoint);
			// или buyer.ClearTarget(); и FSM переводит в Exit state
		}

		private void ReassignTargets()
		{
			for (int i = 0; i < _buyers.Count; i++)
			{
				if (i >= _queue.PointsCount)
					break;

				_buyers[i].SetMovementTarget(_queue.GetPoint(i).position);
			}
		}

		private void Update()
		{
			if (Time.time > _nextSpawnTime && _queue.TryGetEmptyPoint(out var point))
			{
				Spawn(point);
				_nextSpawnTime = Time.time + _spawnDelay;
			}
		}
	}
}