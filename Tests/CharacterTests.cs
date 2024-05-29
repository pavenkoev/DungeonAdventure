using System.Collections.Generic;
using DungeonAdventure.Characters.Models;
using DungeonAdventure.Items;
using GdUnit4;
using HealthPotionItem = DungeonAdventure.Items.HealthPotionItem;
using Item = DungeonAdventure.Items.Item;

namespace DungeonAdventure.Tests;

[TestSuite]
public class CharacterTests
{
    private CharacterModel _model;

    [BeforeTest]
    public void Setup()
    {
        _model = new(100, 100, 
            10, 30, 
            0.8f, 0.2f, 
            new List<Item>(), "Knight", "Sword");
    }
    
    [TestCase]
    public void NewCharacterIsAlive()
    {
        Assertions.AssertThat(_model.IsAlive).IsTrue();
    }
    
    [TestCase]
    public void NewCharacterIsAtFullHealth()
    {
        Assertions.AssertThat(_model.Health).IsEqual(_model.MaxHealth);
    }

    [TestCase]
    public void CharacterApplyDamage()
    {
        float damage = 30;
        _model.ApplyDamage(damage);
        Assertions.AssertThat(_model.Health).IsEqual(_model.MaxHealth - damage);
    }

    [TestCase]
    public void CharacterDie()
    {
        _model.Die();
        Assertions.AssertThat(_model.IsAlive).IsFalse();
        Assertions.AssertThat(_model.Health).IsZero();
    }

    [TestCase]
    public void CharacterApplyFatalDamage()
    {
        float damage = _model.MaxHealth + 10;
        _model.ApplyDamage(damage);
        Assertions.AssertThat(_model.IsAlive).IsFalse();
        Assertions.AssertThat(_model.Health).IsZero();
    }

    [TestCase]
    public void CharacterHeal()
    {
        float damage = 50;
        float heal = 30;
        
        _model.ApplyDamage(damage);
        _model.Heal(heal);

        Assertions.AssertThat(_model.Health).IsEqual(_model.MaxHealth - damage + heal);
    }

    [TestCase]
    public void CharacterHealPastMaxHealth()
    {
        float damage = 30;
        float heal = 50;
        
        _model.ApplyDamage(damage);
        _model.Heal(heal);

        Assertions.AssertThat(_model.Health).IsEqual(_model.MaxHealth);
    }

    [TestCase]
    public void CharacterAddItem()
    {
        Item item = new HealthPotionItem();
        _model.AddItem(item);

        Assertions.AssertThat(_model.Items.Count).IsEqual(1);
        Assertions.AssertThat(_model.Items[0] == item).IsTrue();
    }
    
    [TestCase]
    public void CharacterRemoveItem()
    {
        Item item = new HealthPotionItem();
        _model.AddItem(item);
        _model.RemoveItem(item);

        Assertions.AssertThat(_model.Items.Count).IsEqual(0);
    }

    [TestCase]
    public void CharacterRandomizeDamage()
    {
        const int numRuns = 100;
        for (int i = 0; i < numRuns; i++)
        {
            float damage = _model.RandomizeDamage();
            Assertions.AssertThat(damage).IsBetween(_model.DamageMin, _model.DamageMax);
        }
    }
    
    [TestCase]
    public void CharacterRandomizeHitChance()
    {
        const int numRuns = 10000;
        int hits = 0;
        for (int i = 0; i < numRuns; i++)
        {
            if (!_model.RandomizeMiss())
                hits++;
        }

        float percent = (float)hits / numRuns;
        Assertions.AssertThat(percent).IsBetween(_model.HitChance * 0.9f, _model.HitChance * 1.1f);
    }
    
    [TestCase]
    public void CharacterRandomizeBlockChance()
    {
        const int numRuns = 10000;
        int blocks = 0;
        for (int i = 0; i < numRuns; i++)
        {
            if (_model.RandomizeBlock())
                blocks++;
        }

        float percent = (float)blocks / numRuns;
        Assertions.AssertThat(percent).IsBetween(_model.BlockChance * 0.9f, _model.BlockChance * 1.1f);
    }
}