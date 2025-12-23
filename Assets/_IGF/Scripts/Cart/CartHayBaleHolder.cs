using Zenject;

namespace IGF
{
	public class CartHayBaleHolder : HayBaleHolder
	{
		[Inject] private IHayBaleHolders _hayBaleHolders;
		
		private void Start() => _hayBaleHolders.Add(this);
	}
}