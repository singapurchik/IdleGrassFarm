using System;

namespace IGF.UI
{
	public interface IUIScreenInfo : IUIScreenEvents
	{
		public float ShowFadeDuration { get; }
		public float HideFadeDuration { get; }
		
		public bool IsHasCloseButton { get; }
		public bool IsProcessShow { get; }
		public bool IsProcessHide { get; }
		public bool IsVisible { get; }
		
		public event Action OnCloseButtonClicked;
	}
}