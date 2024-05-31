using System;
using DungeonAdventure.Characters.Models;
using DungeonAdventure.Items;
using Microsoft.Data.Sqlite;

namespace DungeonAdventure.Characters;

/// <summary>
/// Provides data access methods for character data.
/// </summary>
public class CharacterData : IDisposable
{
    private static readonly string DBSource = $"Data Source=dungeonDB;";

    private SqliteConnection _connection;

    /// <summary>
    /// Initializes a new instance of the <see cref="CharacterData"/> class and opens a connection to the database.
    /// </summary>
    public CharacterData()
    {
        _connection = new SqliteConnection(DBSource);
        _connection.Open();
    }
    
    /// <summary>
    /// Initializes the character data by creating the necessary tables and populating them with default characters.
    /// </summary>
    public void InitializeData()
    {
        using SqliteCommand dropCommand = new SqliteCommand("drop table if exists Characters;", _connection);
        dropCommand.ExecuteNonQuery();

        using SqliteCommand createTableCommand = new SqliteCommand(@"
            create table Characters(
                character_id text primary key,
                health integer not null,
                speed integer not null,
                damage_min integer not null,
                damage_max integer not null,
                attack_rate real not null,
                hit_chance real not null,
                block_chance real not null,
                visual_name text not null,
                weapon_name text not null
            );
        ", _connection);
        createTableCommand.ExecuteNonQuery();
        
        SaveCharacter("knight", new Knight());
        SaveCharacter("rogue", new Rogue());
        SaveCharacter("wizard", new Wizard());
        SaveCharacter("orc", new Orc());
        SaveCharacter("skeleton", new Skeleton());
        SaveCharacter("ghost", new Ghost());
    }

    /// <summary>
    /// Saves a character to the database.
    /// </summary>
    /// <param name="characterId">The ID of the character.</param>
    /// <param name="character">The character model to save.</param>
    private void SaveCharacter(string characterId, CharacterModel character)
    {
        SaveCharacter(characterId, (int)character.Health, (int)character.Speed, 
            (int)character.DamageMin, (int)character.DamageMax, character.AttackRate,
            character.HitChance, character.BlockChance, 
            character.VisualName, character.WeaponName);
    }
    
    /// <summary>
    /// Saves a character to the database with specified attributes.
    /// </summary>
    /// <param name="characterId">The ID of the character.</param>
    /// <param name="health">The health of the character.</param>
    /// <param name="speed">The speed of the character.</param>
    /// <param name="damageMin">The minimum damage of the character.</param>
    /// <param name="damageMax">The maximum damage of the character.</param>
    /// <param name="attackRate">The attack rate.</param>
    /// <param name="hitChance">The hit chance of the character.</param>
    /// <param name="blockChance">The block chance of the character.</param>
    /// <param name="visualName">The visual name of the character.</param>
    /// <param name="weaponName">The weapon name of the character.</param>
    private void SaveCharacter(string characterId, int health, int speed, 
        int damageMin, int damageMax, float attackRate,
        float hitChance, float blockChance,
        string visualName, string weaponName)
    {
        using SqliteCommand command = new SqliteCommand(@"
            insert into Characters(
                character_id, health, speed, damage_min, damage_max, attack_rate,
                hit_chance, block_chance, visual_name, weapon_name
            ) values (
                @character_id, @health, @speed, @damage_min, @damage_max, @attack_rate,
                @hit_chance, @block_chance, @visual_name, @weapon_name
            );
        ", _connection);

        command.Parameters.AddWithValue("@character_id", characterId);
        command.Parameters.AddWithValue("@health", health);
        command.Parameters.AddWithValue("@speed", speed);
        command.Parameters.AddWithValue("@damage_min", damageMin);
        command.Parameters.AddWithValue("@damage_max", damageMax);
        command.Parameters.AddWithValue("@attack_rate", attackRate);
        command.Parameters.AddWithValue("@hit_chance", hitChance);
        command.Parameters.AddWithValue("@block_chance", blockChance);
        command.Parameters.AddWithValue("@visual_name", visualName);
        command.Parameters.AddWithValue("@weapon_name", weaponName);

        command.ExecuteNonQuery();
    }

    /// <summary>
    /// Loads a character from the database based on the character ID.
    /// </summary>
    /// <param name="characterId">The ID of the character to load.</param>
    /// <returns>The loaded character model, or null if the character is not found.</returns>
    public CharacterModel LoadCharacter(string characterId)
    {
        using SqliteCommand command = new SqliteCommand(@"
            select * from Characters where character_id = @character_id;
        ", _connection);
        
        command.Parameters.AddWithValue("@character_id", characterId);

        using SqliteDataReader reader = command.ExecuteReader();

        if (reader.Read())
        {
            string id = Convert.ToString(reader["character_id"]);
            int health = Convert.ToInt32(reader["health"]);
            int speed = Convert.ToInt32(reader["speed"]);
            int damageMin = Convert.ToInt32(reader["damage_min"]);
            int damageMax = Convert.ToInt32(reader["damage_max"]);
            float attackRate = Convert.ToSingle(reader["attack_rate"]);
            float hitChance = Convert.ToSingle(reader["hit_chance"]);
            float blockChance = Convert.ToSingle(reader["block_chance"]);
            string visualName = Convert.ToString(reader["visual_name"]);
            string weaponName = Convert.ToString(reader["weapon_name"]);

            return CharacterFactory.CreateByName(characterId, health, speed, damageMin, damageMax, attackRate,
                hitChance, blockChance, Array.Empty<Item>(),
                visualName, weaponName);
        }

        return null;
    }
    
    /// <summary>
    /// Disposes the instance, closing the database connection.
    /// </summary>
    public void Dispose()
    {
        _connection?.Close();
        _connection?.Dispose();
    }
}