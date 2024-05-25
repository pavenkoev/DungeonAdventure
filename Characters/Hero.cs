using System;
using System.Data.SQLite;

namespace DungeonAdventure.Characters;

public class Hero
{
	public int ID { get; set; }
	public string Name { get; set; }
	public int Strength { get; set; }
	public int Speed { get; set; }
	public int Health { get; set; }
	public int AttackSpeed { get; set; }
	public double ChanceToHit { get; set; }
	public double ChanceToBlock { get; set; }

	// Constructor to initialize the Hero object
	public Hero(int id, string name, int strength, int speed, int health, int attackSpeed, double chanceToHit, double chanceToBlock)
	{
		ID = id;
		Name = name;
		Strength = strength;
		Speed = speed;
		Health = health;
		AttackSpeed = attackSpeed;
		ChanceToHit = chanceToHit;
		ChanceToBlock = chanceToBlock;
	}

	// Static method to get a Hero from the database by its ID
	public static Hero GetHeroById(int id)
	{
		const string DBSource = $"Data Source=dungeonDB;Version=3;";
		const string query = "SELECT * " +
							 "FROM Heroes " +
							 "WHERE ID = @ID";
		
		Hero hero = null; // initialize hero to null

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
						int heroId = Convert.ToInt32(reader["ID"]);
						string name = reader["Name"].ToString();
						int strength = Convert.ToInt32(reader["Strength"]);
						int speed = Convert.ToInt32(reader["Speed"]);
						int health = Convert.ToInt32(reader["Health"]);
						int attackSpeed = Convert.ToInt32(reader["AttackSpeed"]);
						double chanceToHit = Convert.ToDouble(reader["ChanceToHit"]);
						double chanceToBlock = Convert.ToDouble(reader["ChanceToBlock"]);
						hero = new Hero(heroId, name, strength, speed, health, attackSpeed, chanceToHit, chanceToBlock);
					}
				}
			}
			connDB.Close();
		}
		return hero;
	}
}
