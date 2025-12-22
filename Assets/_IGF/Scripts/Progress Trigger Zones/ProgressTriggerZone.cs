using IGF.Players;
using UnityEngine;

namespace IGF
{
	public abstract class ProgressTriggerZone : MonoBehaviour
	{
		[SerializeField] private float _progressSpeed = 0.5f;

		private bool _isPlayerInside;

		protected float Progress { get; private set; }

		protected const float PROGRESS_TO_COMPLETE = 1f;

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out IPlayer _))
				_isPlayerInside = true;
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent(out IPlayer _))
			{
				_isPlayerInside = false;
				OnPlayerExit();
				ResetProgress();
			}
		}

		protected virtual void OnPlayerExit() { }

		protected abstract void OnProgressComplete();

		protected virtual void OnPlayerInside()
			=> Progress += Time.deltaTime * _progressSpeed;

		protected void ResetProgress()
			=> Progress = 0f;

		protected void ClampProgressToComplete()
			=> Progress = PROGRESS_TO_COMPLETE;

		private void Update()
		{
			if (!_isPlayerInside)
				return;

			OnPlayerInside();

			if (Progress > PROGRESS_TO_COMPLETE)
				OnProgressComplete();
		}
	}
}