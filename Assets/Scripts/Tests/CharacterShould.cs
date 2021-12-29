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
     *
     * ITERATION 4
     * One player may join or leave one or more factions
     * Players of the same faction are allies:
     *  - They can't hurt each other
     *  - they can heal each other
     *
     * ITERATION 5
     * 
     */
    private BattleField _battlefield;

    private Character player;
    private Character opponent;
    private Character allie1;
    private Character allie2;


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
        character.HealSelf(100);
        Assert.AreEqual(MaxHealth, character.Health);
    }


    [Test]
    public void DealDamageToOthersButNotToHimself()
    {
        GivenAPlayerTypeMeleeFighterInPosition(1);
        GivenAOpponentTypeMeleeFighterInPosition(2);
        WhenPlayerAttacksOpponentWith(100);
        ThenPLayerHasMaxHealth();
        ThenOpponentHealthIs(900);
    }


    [Test]
    public void BeAbleToHealHimselfButNoOthers()
    {
        GivenAPlayerTypeMeleeFighterInPosition(1);
        GivenAOpponentTypeMeleeFighterInPosition(2);
        WhenPlayerAttacksOpponentWith(100);
        WhenOpponentAttacksPlayerWith(100);
        WhenPlayerHeals(100);


        Assert.AreEqual(MaxHealth, player.Health);
        Assert.AreEqual(900, opponent.Health);
    }


    private void WhenOpponentAttacksPlayerWith(int damageAmount)
    {
        opponent.Attack(player, damageAmount);
    }

    [Test]
    public void HaveHisAttacksDamagedAppliedReducedBy50PercentIfOpponentLevelIs5LevelsOrMoreAboveHisLevel()
    {
        //Given
        GivenAPlayerTypeMeleeFighterInPosition(1);
        GivenAOpponentTypeMeleeFighterInPosition(2);
        opponent.Level = 6;
        //when
        WhenPlayerAttacksOpponentWith(100);
        //then
        ThenOpponentHealthIs(950);
    }

    [Test]
    public void HaveHisAttacksDamagedAppliedBoostedBy50PercentIfOpponentLevelIs5LevelsOrMoreBelowHisLevel()
    {
        //Given
        GivenAPlayerTypeMeleeFighterInPosition(1);
        GivenAOpponentTypeMeleeFighterInPosition(2);
        player.Level = 10;

        //when
        WhenPlayerAttacksOpponentWith(100);

        //then
        ThenOpponentHealthIs(850);
    }

    [Test]
    public void HaveAttackRangeOfTwoIfHeIsAMeleeFighter()
    {
        MeleeFighter meleeFighter = new MeleeFighter(_battlefield);
        Assert.AreEqual(2, meleeFighter.AttackRange);
    }

    [Test]
    public void HaveAttackRangeOfTwentyIfHeIsARangedFighter()
    {
        RangedFighter rangedFighter = new RangedFighter(_battlefield);
        Assert.AreEqual(20, rangedFighter.AttackRange);
    }

    [Test]
    public void OnlyBeAbleToDealDamageIfOpponentIsInRange()
    {
        GivenAPlayerTypeMeleeFighterInPosition(1);
        GivenAOpponentTypeMeleeFighterInPosition(2);
        WhenPlayerAttacksOpponentWith(100);
        ThenOpponentHealthIs(MaxHealth - 100);
    }


    [Test]
    public void NotDealDamageIfOpponentIsOutRange()
    {
        GivenAPlayerTypeMeleeFighterInPosition(1);
        GivenAOpponentTypeMeleeFighterInPosition(55);
        WhenPlayerAttacksOpponentWith(100);
        ThenOpponentHealthIs(MaxHealth);
    }

    [Test]
    public void NotBeAbleToDamageAnAllie()
    {
        GivenTwoAllies();

        //when
        player.Attack(opponent, 100);

        //then
        Assert.AreEqual(MaxHealth, opponent.Health);
    }

    [Test]
    public void BeAbleToHealOpponentIfHeIsAnAllie()
    {
        GivenTwoAllies();
        opponent.Health = 800;

        player.Heal(opponent, 200);
        Assert.AreEqual(MaxHealth, opponent.Health);
    }
    
    [Test]
    public void NotBeAbleToHealOpponentIfHeIsNotAnAllie()
    {
        GivenAPlayerTypeMeleeFighterInPosition(1);
        GivenAOpponentTypeMeleeFighterInPosition(2);
        opponent.Health = 800;
        player.Heal(opponent, 200);
        Assert.AreEqual(MaxHealth-200, opponent.Health);
    }

    private void GivenTwoAllies()
    {
        GivenAOpponentTypeMeleeFighterInPosition(1);
        GivenAPlayerTypeMeleeFighterInPosition(1);
        Faction factionA = new Faction("FactionA");
        opponent.JoinFaction(factionA);
        player.JoinFaction(factionA);
    }

    private void ThenOpponentHealthIs(int amount)
    {
        Assert.AreEqual(amount, opponent.Health);
    }

    private void ThenPLayerHasMaxHealth()
    {
        Assert.AreEqual(MaxHealth, player.Health);
    }

    private void WhenPlayerAttacksOpponentWith(int damageAmmount)
    {
        player.Attack(opponent, damageAmmount);
    }


    private void GivenAPlayerTypeMeleeFighterInPosition(int playerPosition)
    {
        player = new MeleeFighter(_battlefield);
        _battlefield.AddCharacter(player, playerPosition);
    }

    private void GivenAOpponentTypeRangedFighterInPosition(int playerPosition)
    {
        opponent = new RangedFighter(_battlefield);
        _battlefield.AddCharacter(opponent, playerPosition);
    }

    private void GivenAOpponentTypeMeleeFighterInPosition(int playerPosition)
    {
        opponent = new MeleeFighter(_battlefield);
        _battlefield.AddCharacter(opponent, playerPosition);
    }

    private Character GivenACharacter()
    {
        Character character = new Character();
        character.BattleField = _battlefield;
        return character;
    }


    private Character GivenARangedFighter()
    {
        Character character = new RangedFighter(_battlefield);
        character.BattleField = _battlefield;
        return character;
    }

    private void WhenPlayerHeals(int amount)
    {
        player.HealSelf(amount);
    }
}