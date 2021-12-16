using System;

public class Character
{
    public decimal AttackRange { get; set; }
    public const float MaxHealth = 1000;
    public bool IsAlive { get; set; } = true;
    public int Level { get; set; } = 1;

    public float Health { get; set; } = MaxHealth;

    public BattleField battleField { get; set; }

    public void ReceiveDamage(float ammount)
    {
        Health = Math.Max(0, Health - ammount);
        if (Health == 0)
        {
            IsAlive = false;
        }
    }


    public void Heal(int ammount)
    {
        Health = Math.Min(MaxHealth, Health + ammount);
        if (Health == 0) ;
    }

    public void Attack(Character opponent, int damageAmmount)
    {
        if (battleField.InRange(this, opponent))
        {
            opponent.ReceiveDamage(CalculateEffectiveDamage(opponent, damageAmmount));
        }
    }

    private float CalculateEffectiveDamage(Character opponent, int damageAmmount)
    {
        if (opponent.Level - this.Level >= 5)
        {
            return damageAmmount * 0.5f;
        }

        if (this.Level - opponent.Level >= 5)
        {
            return damageAmmount * 1.5f;
        }

        return damageAmmount;


    }
}