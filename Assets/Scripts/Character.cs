using System;
using System.Collections.Generic;
using System.Linq;

public class Character
{
    public decimal AttackRange { get; set; }
    public const float MaxHealth = 1000;
    public bool IsAlive { get; set; } = true;
    public int Level { get; set; } = 1;
    private List<Faction> _factionList = new List<Faction>();
    public float Health { get; set; } = MaxHealth;

    public BattleField BattleField { get; set; }

    public void ReceiveDamage(float ammount)
    {
        Health = Math.Max(0, Health - ammount);
        if (Health == 0)
        {
            IsAlive = false;
        }
    }


    public void HealSelf(int ammount)
    {
        Health = Math.Min(MaxHealth, Health + ammount);
      
    }

    public void Heal(Character character, int ammount)
    {
        if (IsAnAllie(character))
        {
            character.Health = Math.Min(MaxHealth, Health + ammount);
        }
    }
    
    public void Attack(Character opponent, int damageAmmount)
    {
        if (IsAnAllie(opponent)) return;
        if (BattleField.InRange(this, opponent))
        {
            opponent.ReceiveDamage(CalculateEffectiveDamage(opponent, damageAmmount));
        }
    }

    private float CalculateEffectiveDamage(Character opponent, int damageAmount)
    {
        if (opponent.Level - this.Level >= 5)
        {
            return damageAmount * 0.5f;
        }

        if (this.Level - opponent.Level >= 5)
        {
            return damageAmount * 1.5f;
        }

        return damageAmount;


    }

    private bool IsAnAllie(Character character)
    {
       var factionsInCommom = this._factionList.Select(x => x.Name).Intersect(character._factionList.Select(x => x.Name));
       return factionsInCommom.Any();

    }

    public void JoinFaction(Faction faction)
    {
        _factionList.Add(faction);
    }
}