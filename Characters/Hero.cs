using System;
using System.Data.SQLite;

namespace DungeonAdventure.Characters
{
    public class Hero
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Strength { get; set; }
        public int Speed { get; set; }
        public int Health { get; set; }

        // Constructor to initialize the Hero object
        public Hero(int id, string name, int strength, int speed, int health)
        {
            ID = id;
            Name = name;
            Strength = strength;
            Speed = speed;
            Health = health;
        }

        // Static method to fetch a Hero from the database by ID
        public static Hero GetHeroById(int id)
        {
            string dbPath = "dungeonDB";
            string connString = $"Data Source={dbPath};Version=3;";
            string query = "SELECT * FROM Heroes WHERE ID = @ID";
            Hero hero = null;

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
                            int heroId = Convert.ToInt32(reader["ID"]);
                            string name = reader["Name"].ToString();
                            int strength = Convert.ToInt32(reader["Strength"]);
                            int speed = Convert.ToInt32(reader["Speed"]);
                            int health = Convert.ToInt32(reader["Health"]);
                            hero = new Hero(heroId, name, strength, speed, health);
                        }
                    }
                }
            }

            return hero;
        }
    }
}