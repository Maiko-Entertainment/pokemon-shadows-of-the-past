using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
[CommandInfo("Variable",
                "First pkmn has type",
                "Check if the first party member has at least 1 of the required types.")]
public class FungusCheckForPokemonType : Command
{
    public List<TypeData> possibleTypes = new List<TypeData>();
    [Tooltip("Boolean variable to set result")]
    [VariableProperty(typeof(BooleanVariable))]
    [SerializeField] protected BooleanVariable variable;

    public override void OnEnter()
    {
        List<PokemonCaughtData> party = PartyMaster.GetInstance().GetParty();
        bool result = false;
        if (party.Count > 0)
        {
            PokemonCaughtData pokemon = party[0];
            foreach(TypeData type in possibleTypes)
            {
                if (pokemon.GetTypes().Contains(type))
                {
                    result = true;
                    break;
                }
            }
        }
        variable.Apply(SetOperator.Assign, result);
        Continue();
    }
}
