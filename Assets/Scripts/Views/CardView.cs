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
		private const int layerMask = 1 << 8;
		private Vector3 _startPosition;
		public bool canDrag = false;
		private bool isDragging;

		public void Init(CardModel cardModel)
		{
			Debug.Log("[CardView][Init]");

			CardModel = cardModel;

			_name.text = cardModel.Name;
			_health.text = cardModel.Health.ToString();
			_damage.text = cardModel.Damage.ToString();
			_description.text = cardModel.Description;
			_imageName.text = cardModel.Image;
			_image.sprite = Sprite.Create(Texture2D.blackTexture, Rect.zero, Vector2.down);
		}

		public void GetDamage(int damage)
		{
			Debug.Log(string.Format("[CardView][GetDamage] Damage = {0}; Health = {1}", damage, CardModel.Health));
			CardModel.DoDamage(damage);
		}

		private void Update()
		{
			_health.text = CardModel.Health.ToString();
		}

		private void OnMouseDrag()
		{
			if (!canDrag) return;
			if (!isDragging)
				_startPosition = transform.localPosition;

			isDragging = true;

			var distance = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
			transform.position =
				Camera.main.ScreenToWorldPoint(
					new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance));

			MoveToTableSlot();
		}

		private void MoveToTableSlot()
		{
			if (!isDragging) return;

			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			var rayInfos = Physics.RaycastAll(ray, Mathf.Infinity, layerMask).ToList();

			if (!rayInfos.Any()) return;
			var slots = rayInfos.Where(
					info =>
					{
						var slot = info.collider.gameObject.GetComponent<TableSlotView>();
						return slot != null && slot.CardView == null;
					})
				.ToList();

			if (!slots.Any()) return;
			var tableSlot = slots.FirstOrDefault().collider.GetComponent<TableSlotView>();
			var parentHandSlot = transform.parent.GetComponent<HandSlotView>();

			tableSlot.SetCardView(this);
			parentHandSlot.SetCardView(null);

			isDragging = false;
		}

		private void OnMouseUp()
		{
			if (!isDragging) return;
			transform.localPosition = _startPosition;
		}
	}
}