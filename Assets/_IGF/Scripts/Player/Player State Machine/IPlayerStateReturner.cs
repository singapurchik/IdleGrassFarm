namespace IGF.Players
{
	public interface IPlayerStateReturner : IStateReturner
	{
		public void TryReturnLastControlledState();
	}
}