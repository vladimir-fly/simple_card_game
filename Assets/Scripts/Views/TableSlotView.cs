using UnityEngine;
using UnityEngine.EventSystems;

namespace SCG
{
	public class TableSlotView : MonoBehaviour, IDropHandler
	{
		public CardView CardView;

		private void Start()
		{

		}

		private void Update()
		{

		}

		public void OnDrop(PointerEventData eventData)
		{
			Debug.Log("Dropped");

		}
	}
}