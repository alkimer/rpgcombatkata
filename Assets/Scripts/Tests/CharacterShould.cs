using NUnit.Framework;

public class CharacterShould
{
    private const int MaxHealth = 1000;

    /** ITERATION 1
    -health starts at 1000
    -level starts at 1
    -characters are dead or alive
    -characters can deal damage and heal

    conditions:
    -when the character damage received is higher than the actual health, its health drops to 0 and dies
    -the character cannot be healed over 1000.
    */
    /** ITERATION 2
     * a character can deal damage to others but not to himself
     * a character can heal himself but not other
     * Level now affects damage:
     *  if target is 5 or more levels above the player, the damage applied will be reduced by 50%
     *  if target is 5 or more levels below the player, the damaged applied will be boosted by 50%
     */
    /**
     * iTERATION 3
     *  The player has an Attack Range
     * If he is a melee fighter, his attack range is 2 meters
     * If he is a ranged fighter, his attack range is 20 meters
     * When trying to deal damage the player must be in range
     */

    private BattleField _battlefield;
    
    [SetUp]
    public void TestInit()
    {
        // Runs before each test. (Optional)
        _battlefield = new BattleField();
    }
    
    
    [Test]
    public void StartWith1000Health()
    {
        var character = GivenACharacter();
        Assert.AreEqual(MaxHealth, character.Health);
    }



    [Test]
    public void StartWithLeve1()
    {
        var character = GivenACharacter();
        Assert.AreEqual(1, character.Level);
    }

    [Test]
    public void HaveHisHealthDecrementedToZeroWhenDamageReceivedHigherThanActualHealthAndDie()
    {
        //Given
        var character = GivenACharacter();

        //When
        character.ReceiveDamage(1001);

        //Then
        Assert.AreEqual(0, character.Health);
        Assert.AreEqual(false, character.IsAlive);
    }

    [Test]
    public void NotHealMoreThan1000()
    {
        var character = GivenACharacter();

        character.Heal(100);

        Assert.AreEqual(MaxHealth, character.Health);
    }


    [Test]
    public void DealDamageToOthersButNotToHimself()
    {
        //given
        var character1 = GivenACharacter();
        var character2 = GivenACharacter();

        //when
        character1.Attack(character2, 100);

        //then

        Assert.AreEqual(MaxHealth, character1.Health);
        Assert.AreEqual(900, character2.Health);
    }

    [Test]
    public void BeAbleToHealHimselfButNoOthers()
    {
        //given
        var character1 = GivenACharacter();
        var character2 = GivenACharacter();
        
        //when
        character1.Attack(character2,100);
        character2.Attack(character1,100);
        character1.Heal(100);
        
        //then
        Assert.AreEqual(MaxHealth, character1.Health);
        Assert.AreEqual(900,character2.Health);

    }

    [Test]
    public void HaveHisAttacksDamagedAppliedReducedBy50PercentIfOpponentLevelIs5LevelsOrMoreAboveHisLevel()
    {
        //Given
        var character = GivenACharacter();
        var opponent = GivenACharacter();
        opponent.Level = 6;
        //when
        character.Attack(opponent,100);
        //then
         
        Assert.AreEqual(950, opponent.Health);
    }

    [Test]
    public void HaveHisAttacksDamagedAppliedBoostedBy50PercentIfOpponentLevelIs5LevelsOrMoreBelowHisLevel()
    {
        //Given
        var character = GivenACharacter();
        var opponent = GivenACharacter();
        character.Level = 10;
        
        //when
        character.Attack(opponent,100);
        
        //then
         Assert.AreEqual(850, opponent.Health);
    }

    [Test]
    public void HaveAttackRangeOfTwoIfHeIsAMeleeFighter()
    {
        MeleeFighter meleeFighter = new MeleeFighter();

        Assert.AreEqual(2,meleeFighter.AttackRange);
    }

    [Test]
    public void HaveAttackRangeOfTwentyIfHeIsARangedFighter()
    {
        RangedFighter rangedFighter = new RangedFighter();

        Assert.AreEqual(20,rangedFighter.AttackRange);
    }

    [Test]
    public void OnlyBeAbleToDealDamageIfOpponentIsInRange()
    {
        Character opponent = GivenACharacter();
        Character player = GivenARangedFighter();
        _battlefield.AddEnemy(player, 1);
        _battlefield.AddEnemy(opponent, 5);
        player.Attack(opponent,100);
        Assert.AreEqual(MaxHealth-100, opponent.Health);
    }
    
    
    [Test]
    public void NotDealDamageIfOpponentIsOutRange()
    {
        Character opponent = GivenACharacter();
        Character player = GivenARangedFighter();
        _battlefield.AddEnemy(player, 1);
        _battlefield.AddEnemy(opponent, 55);
        player.Attack(opponent,100);
        Assert.AreEqual(MaxHealth, opponent.Health);
    }
    
    
    private Character GivenACharacter()
    {
        Character character = new Character();
        character.battleField = _battlefield;
        return character;
    }
    
    private Character GivenARangedFighter()
    {
        Character character = new RangedFighter();
        character.battleField = _battlefield;
        return character;
    }
}