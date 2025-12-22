using UnityEngine;

namespace IGF
{
	public abstract class ProgressTriggerZone : MonoBehaviour
	{
		[SerializeField] private float _progressSpeed = 0.5f;
		
		private bool _isPlayerInside;

		private float _progress;
		
		private const float PROGRESS_TO_COMPLETE = 1;
		
		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out IPlayer seller))
				_isPlayerInside = true;
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent(out IPlayer seller))
			{
				_isPlayerInside = false;
				ResetProgress();
			}
		}

		protected abstract void OnProgressComplete();

		protected virtual void OnPlayerInside() => _progress += Time.deltaTime * _progressSpeed;
		
		protected void ResetProgress() => _progress = 0;
		
		private void Update()
		{
			if (_isPlayerInside)
			{
				OnPlayerInside();

				if (_progress > PROGRESS_TO_COMPLETE)
					OnProgressComplete();
			}
		}
	}
}