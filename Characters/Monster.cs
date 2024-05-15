using System;
using System.Data.SQLite;

namespace DungeonAdventure.Characters
{
	public class Monster
	{
		public int ID { get; set; }
		public string Type { get; set; }
		public int Strength { get; set; }
		public int Speed { get; set; }
		public int Health { get; set; }

		// Constructor to initialize the Monster object
		public Monster(int id, string type, int strength, int speed, int health)
		{
			ID = id;
			Type = type;
			Strength = strength;
			Speed = speed;
			Health = health;
		}

		// Static method to fetch a Monster from the database by ID
		public static Monster GetMonsterById(int id)
		{
			string dbPath = "dungeonDB";
			string connString = $"Data Source={dbPath};Version=3;";
			string query = "SELECT * FROM Monsters WHERE ID = @ID";
			Monster monster = null;

			using (SQLiteConnection conn = new SQLiteConnection(connString))
			{
				conn.Open();
				using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
				{
					cmd.Parameters.AddWithValue("@ID", id);
					using (SQLiteDataReader reader = cmd.ExecuteReader())
					{
						if (reader.Read())
						{
							int monsterId = Convert.ToInt32(reader["ID"]);
							string type = reader["Type"].ToString();
							int strength = Convert.ToInt32(reader["Strength"]);
							int speed = Convert.ToInt32(reader["Speed"]);
							int health = Convert.ToInt32(reader["Health"]);
							monster = new Monster(monsterId, type, strength, speed, health);
						}
					}
				}
			}

			return monster;
		}
	}
}


