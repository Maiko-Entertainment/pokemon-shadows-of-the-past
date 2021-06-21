[System.Serializable]
public enum BattleEventId
{
    pokemonEnter = 0,
    pokemonSwitch = 1,
    pokemonFaint = 2,
    pokemonUseMove = 3,
    pokemonUseMoveSuccess = 10,
    pokemonTakeDamage = 4,
    pokemonRecoverHealth = 5,
    pokemonChangeStats = 6,
    pokemonAddStatus = 12,
    pokemonTurnEnd = 11,
    roundEnd = 8,
    useTactic = 7,
    pokemonLeaveCleanUp = 9
}
