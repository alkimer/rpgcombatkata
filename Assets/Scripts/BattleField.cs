using System;

public class BattleField
{
    private Character[] _battleFieldPositions = new Character[100];

    public void AddCharacter(Character player, int position)
    {
        _battleFieldPositions[position] = player;
    }

    public bool InRange(Character character, Character opponent)
    {
        return Math.Abs(
                   Array.IndexOf(_battleFieldPositions, character) - Array.IndexOf(_battleFieldPositions, opponent)) <=
               character.AttackRange;
    }
}