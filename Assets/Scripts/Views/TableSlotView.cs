using UnityEngine;

namespace SCG
{
	public class TableSlotView : MonoBehaviour
	{
		public CardView CardView { get; private set; }

		public void SetCardView(CardView cardView)
		{
			CardView = cardView;
			if (cardView == null) return;
			cardView.gameObject.transform.SetParent(transform);
			cardView.gameObject.transform.localPosition = new Vector3(0, 1, 0);
			CardView.canDrag = false;
		}
	}
}