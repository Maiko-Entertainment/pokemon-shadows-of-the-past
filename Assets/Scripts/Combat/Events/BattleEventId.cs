[System.Serializable]
public enum BattleEventId
{
    pokemonEnter = 0,
    pokemonSwitch = 1,
    pokemonFaint = 2,
    pokemonAbilityUse = 16,
    preDesition = 20,
    pokemonUseMove = 3,
    pokemonUseMoveSuccess = 10,
    pokemonTakeDamage = 4,
    pokemonTakeDamageSuccess = 17,
    pokemonRecoverHealth = 5,
    pokemonChangeStats = 6,
    pokemonAddStatus = 12,
    pokemonRemoveStatus = 19,
    pokemonTurnEnd = 11,
    pokemonGainExp = 14,
    roundEnd = 8,
    useTactic = 7,
    useItemOnPokemon = 13,
    pokemonLeaveCleanUp = 9,
    battleEnd = 15,
    battleRun = 18,
    animation = 19,
    pokemonAddStatusSuccess = 21
}
