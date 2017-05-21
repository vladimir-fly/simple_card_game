using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SCG
{
    public class CardDeckView : MonoBehaviour
    {
        private List<CardView> _cardDeck;
        public GameObject _cardDeckAchor;

        public int Count { get { return _cardDeck.Count; } }

        public void Init(List<CardView> cardViews)
        {
            _cardDeck = new List<CardView>(cardViews);

            InitCardDeckAnchor();

            var i = 0f;
            cardViews.ForEach(cv =>
            {
                cv.gameObject.transform.SetParent(_cardDeckAchor.transform);
                cv.gameObject.transform.localPosition = new Vector3(i, 0.5f, 0f);
                cv.gameObject.transform.localRotation = new Quaternion(0f, 90f, 0f, 0);
                cv.gameObject.SetActive(true);
                cv.canDrag = false;
                i += 0.15f;
            });
        }

        private void InitCardDeckAnchor()
        {
            if (_cardDeckAchor != null) return;
            _cardDeckAchor = new GameObject("CardDeckAnchor");
            _cardDeckAchor.transform.SetParent(transform);
            _cardDeckAchor.transform.localPosition = new Vector3(3f, 0.3f, -3f);
        }

        public CardView Dequeue()
        {
            var first = _cardDeck.First();
            _cardDeck.RemoveAt(0);

            Init(_cardDeck);
            return first;
        }

        public void Enqueue(CardView cardView)
        {
            _cardDeck.Add(cardView);
            Init(_cardDeck);
        }

        public CardView Peek()
        {
            return _cardDeck.FirstOrDefault();
        }

        public bool Any()
        {
            return _cardDeck.Any();
        }
    }

}