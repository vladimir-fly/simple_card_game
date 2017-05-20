using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCG
{
    public class CardDeckView : MonoBehaviour
    {
        public Queue<CardView> Cards { get; private set; }

        public void Init(CardView cardViews)
        {

        }

        public void Dequeue()
        {
            Cards.Dequeue();
        }
    }
}