using UnityEngine;

namespace SCG
{
	public class HandSlotView : MonoBehaviour
	{
		public CardView CardView { get; private set; }

		public void SetCardView(CardView cardView)
		{
			CardView = cardView;
			if (cardView == null) return;
			cardView.gameObject.transform.SetParent(transform);
			cardView.gameObject.transform.localPosition = new Vector3(0, 1, 0);
			cardView.gameObject.transform.Rotate(new Vector3(0f, -90f, 90f), Space.Self);

			cardView.canDrag = true;
		}
	}
}