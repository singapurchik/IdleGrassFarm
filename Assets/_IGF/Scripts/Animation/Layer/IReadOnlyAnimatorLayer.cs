using System;

namespace IGF
{
	public interface IReadOnlyAnimatorLayer
	{
		public int CurrentIgnoringPauseAnimHash { get; }
		public int CurrentAnimHash { get; }

		public float CurrentAnimNTime { get; }
		
		public bool IsPlayingIgnoringPauseAnim { get; }
		public float WeightBeforeIgnoringPause { get; }
		public bool IsInTransition { get; }
		public bool IsDisabled { get; }
		public bool IsEnabled { get; }
		public bool IsActive { get; }
		public float Weight { get; }
		
		public event Action<int> OnPlayIgnoringPauseAnim;
	}
}