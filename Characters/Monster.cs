using System;
using System.Data.SQLite;

namespace DungeonAdventure.Characters;
public class Monster
{
	public int ID { get; set; }
	public string Type { get; set; }
	public int Strength { get; set; }
	public int Speed { get; set; }
	public int Health { get; set; }
	public int AttackSpeed { get; set; }
	public double ChanceToHit { get; set; }
	public double ChanceToBlock { get; set; }

	// Constructor to initialize the Monster object
	public Monster(int id, string type, int strength, int speed, int health, int attackSpeed, double chanceToHit, double chanceToBlock)
	{
		ID = id;
		Type = type;
		Strength = strength;
		Speed = speed;
		Health = health;
	}

	// Static method to get a Monster from the database by ID
	public static Monster GetMonsterById(int id)
	{
		const string DBSource = $"Data Source=dungeonDB;Version=3;";
		const string query = "SELECT * " +
		                     "FROM Monsters " +
							 "WHERE ID = @ID";
		
		Monster monster = null; // initialize monster to null

		using (SQLiteConnection connDB = new SQLiteConnection(DBSource))
		{
			connDB.Open();
			using (SQLiteCommand cmd = new SQLiteCommand(query, connDB))
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
						int attackSpeed = Convert.ToInt32(reader["AttackSpeed"]);
						double chanceToHit = Convert.ToDouble(reader["ChanceToHit"]);
						double chanceToBlock = Convert.ToDouble(reader["ChanceToBlock"]);
						monster = new Monster(monsterId, type, strength, speed, health, attackSpeed, chanceToHit, chanceToBlock);
					}
				}
				connDB.Close();
			}
		}
		return monster;
	}
}



