using UnityEngine;

namespace SCG
{
	public class TableSlotView : MonoBehaviour
	{
		public CardView CardView { get; private set; }

		public void SetCardView(CardView cardView)
		{
			cardView.gameObject.transform.SetParent(transform);
			cardView.gameObject.transform.localPosition = new Vector3(0, 1, 0);
			CardView = cardView;

			CardView.canDrag = false;
		}
	}
}