using UnityEngine;
using Zenject;

namespace IGF.Buyers
{
	public sealed class BuyersSpawnScheduler : MonoBehaviour
	{
		[SerializeField] private Transform _spawnPoint;
		[SerializeField] private float _spawnDelay = 1f;

		[Inject] private BuyersQueue _queue;
		[Inject] private BuyersPool _pool;

		private float _nextSpawnTime;

		private void Update()
		{
			if (Time.time > _nextSpawnTime && _queue.TryGetEmptyPoint(out var point))
			{
				var buyer = _pool.Get();
				buyer.RequestTeleport(_spawnPoint.position);
				buyer.MovementTarget.Set(point.position, BuyerMovementTargetType.QueuePoint);
				_queue.AddBuyer(buyer);
				_nextSpawnTime = Time.time + _spawnDelay;
			}
		}
	}
}