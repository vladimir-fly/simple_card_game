namespace SCG
{
	public class CardModel
	{
		public string Name { get; private set; }
		public int Health { get; private set; }
		public int Damage { get; private set; }
		public string Description { get; private set; }
		public string Image { get; private set; }

		public CardModel(string name, int health, int damage, string description, string image)
		{
			Name = name;
			Health = health;
			Damage = damage;
			Description = description;
			Image = image;
		}

		public void DoDamage(int damage)
		{
			Health = damage < Health ? Health - damage : 0;
		}

	}
}