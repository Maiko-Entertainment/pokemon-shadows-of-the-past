using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonBattleAnimator : MonoBehaviour
{
    public SpriteRenderer sprite;

    [SerializeField] bool seeFromBack = false;
    [SerializeField] PokemonBattleData pokemon;
    [SerializeField] PokemonBattleData pokemonBase;
    [SerializeField] float timePassed = 0f;
    [SerializeField] int frameIndex = 0;

    private Sprite[] spritesFront;
    private Sprite[] spritesBack;
    private SpriteRenderer shadow;

    public static float intervalBetweenSpriteChange = 3f / 60;

    public Sprite GetSprite()
    {
        if (seeFromBack)
        {
            if (spritesBack.Length > 0)
            {
                frameIndex %= spritesBack.Length;
                return spritesBack[frameIndex];
            }
        }
        else
        {
            if (spritesFront.Length > 0)
            {
                frameIndex %= spritesFront.Length;
                return spritesFront[frameIndex];
            }
        }
        return sprite.sprite;
    }

    public void UpdateSprite()
    {
        sprite.sprite = GetSprite();
        if (shadow)
        {
            shadow.sprite = GetSprite();
        }
    }

    public void SetPokemonDirection(bool seeFromBack)
    {
        this.seeFromBack = seeFromBack;
        UpdateSprite();
    }

    public string GetPath(PokemonBattleData pokemon, bool useBack = false)
    {
        PokemonBaseData baseData = pokemon.GetPokemonCaughtData().GetPokemonBaseData();
        return GetPath(baseData, useBack);
    }
    public string GetPath(PokemonBaseData baseData, bool useBack = false)
    {
        string facingId = (useBack ? ResourceMaster.Instance.battlerBackIndicator : ResourceMaster.Instance.battlerFrontIndicator);
        return ResourceMaster.Instance.GetBaseBattlerPath(baseData) + facingId;
    }
    public void AddShadow()
    {
        if (shadow == null)
        {
            Material shadowMaterial = BattleAnimatorMaster.GetInstance().shadowMaterial;
            shadow = Instantiate(sprite, sprite.transform);
            shadow.transform.localPosition = new Vector3(0, 0.15f, 0);
            shadow.transform.localScale = new Vector3(1, 1.2f, 1);
            shadow.sortingOrder = -10;
            shadow.transform.eulerAngles = new Vector3(65, 0, 330);
            shadow.GetComponent<Renderer>().material = shadowMaterial;
        }
    }

    public PokemonBattleAnimator Load(PokemonBattleData pokemon)
    {
        this.pokemon = pokemon;
        timePassed = 0;
        spritesFront = Resources.LoadAll<Sprite>(GetPath(pokemon, false));
        spritesBack = Resources.LoadAll<Sprite>(GetPath(pokemon, true));
        AddShadow();
        UpdateSprite();
        return this;
    }
    public PokemonBattleAnimator Load(PokemonBaseData pokemon)
    {
        timePassed = 0;
        spritesFront = Resources.LoadAll<Sprite>(GetPath(pokemon, false));
        spritesBack = Resources.LoadAll<Sprite>(GetPath(pokemon, true));
        AddShadow();
        UpdateSprite();
        return this;
    }


    private void LateUpdate()
    {
        timePassed += Time.deltaTime;
        if (timePassed > intervalBetweenSpriteChange)
        {
            timePassed %= intervalBetweenSpriteChange;
            frameIndex++;
            UpdateSprite();
        }
    }
}
