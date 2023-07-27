using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapDamageDealer : MonoBehaviour
{
    public OutOfCombatDamage damage;
    public float afterDamageCooldown = 1f;
    public AudioOptions outOfBattleDamageSound;

    public Flowchart onKillFlowchart;
    public string onKillBlock = "Kill";

    public bool isDamageActive = false;
    protected float damageCooldownTime = 0f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && isDamageActive && damageCooldownTime <= 0)
        {
            HandleHit();
        }
    }

    public virtual void HandleHit()
    {
        damageCooldownTime = afterDamageCooldown;
        PokemonCaughtData pokemon = PartyMaster.GetInstance().GetFirstAvailablePokemon();
        DamageSummary summary = BattleMaster.GetInstance().CalculateOutOfBattleDamage(pokemon, damage);
        pokemon.ChangeHealth(summary.damageAmount * -1);
        UIPauseMenuMaster.GetInstance().DoHitPokemonAnim(pokemon);
        TransitionMaster.GetInstance().RunTransition(TransitionMaster.GetInstance().damageWorldTransition);
        if (pokemon.IsFainted())
        {
            AudioMaster.GetInstance().PlaySfx(new AudioOptions(pokemon.GetPokemonBaseData().GetCry(), 0.7f));
        }
        WorldMapMaster.GetInstance().GetPlayer().UpdatePokeFollower();
        if (outOfBattleDamageSound != null) AudioMaster.GetInstance().PlaySfx(outOfBattleDamageSound);
        pokemon = PartyMaster.GetInstance().GetFirstAvailablePokemon();
        if (onKillFlowchart && pokemon == null)
        {
            onKillFlowchart.ExecuteBlock(onKillBlock);
        }
    }

    protected void Update()
    {
        if (!InteractionsMaster.GetInstance().IsInteractionPlaying())
            damageCooldownTime = Mathf.Max(0, damageCooldownTime - Time.deltaTime);
    }
}
