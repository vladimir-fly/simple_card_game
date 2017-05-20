using System.Linq;
using UnityEngine;

namespace SCG
{
	public class CardView : MonoBehaviour
	{
		[SerializeField] private TextMesh _name;
		[SerializeField] private TextMesh _health;
		[SerializeField] private TextMesh _damage;
		[SerializeField] private TextMesh _description;
		[SerializeField] private TextMesh _imageName;
		[SerializeField] private SpriteRenderer _image;

		public CardModel CardModel { get; private set; }

		[HideInInspector] public bool HasMoved;

		private Vector3 _startPosition;
		private Transform _parentTransform;
		public int CurrentSlotId;

		public void Init(CardModel cardModel)
		{
			CardModel = cardModel;

			_name.text = cardModel.Name;
			_health.text = cardModel.Health.ToString();
			_damage.text = cardModel.Damage.ToString();
			_description.text = cardModel.Description;
			_imageName.text = cardModel.Image;
			_image.sprite = Sprite.Create(Texture2D.blackTexture, Rect.zero, Vector2.down);
		}

		private void Update()
		{
			if (!isDragging) return;
			var layerMask = 1 << 8; //slot layer

			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			var rayInfos = Physics.RaycastAll(ray, Mathf.Infinity, layerMask).ToList();
			Debug.DrawRay(ray.origin, ray.direction, Color.cyan);

			if (rayInfos.Any())
			{
				Debug.Log("rays " + rayInfos.Count);

				var slots = rayInfos.Where(r => r.collider.gameObject.GetComponent<TableSlotView>()).ToList();

				if (slots.Any())
				{
					transform.SetParent(slots.FirstOrDefault().transform);
					transform.localPosition = new Vector3(0, 1, 0);
					canDrag = false;
					isDragging = false;
					Debug.Log("table slot");
				}

			}

		}

		private bool canDrag = true;
		private bool isDragging = false;

		private void OnMouseDrag()
		{
			if (!canDrag) return;

			isDragging = true;

			Debug.Log("on drag");
			var distance = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

			transform.position =
				Camera.main.ScreenToWorldPoint(
					new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance));

		}

	}
}