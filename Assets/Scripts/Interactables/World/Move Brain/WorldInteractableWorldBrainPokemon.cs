using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WorldInteractableWorldBrainPokemon : WorldInteractableBrainFollower
{
    public PokemonCaughtData pokemon;
    public SpriteRenderer sprite;

    public static float intervalBetweenSpriteChange = 15f / 60;
    public static int spriteSheetColumns = 4;

    [SerializeField] float timePassed = 0f;
    [SerializeField] int frameIndex = 0;
    [SerializeField] private Sprite[] spritesDown;
    [SerializeField] private Sprite[] spritesUp;
    [SerializeField] private Sprite[] spritesLeft;
    [SerializeField] private Sprite[] spritesRight;
    [SerializeField] protected Vector2 lastDirection = new Vector2();

    protected override void Start()
    {
        if (pokemon != null && pokemon.GetPokemonBaseData())
        {
            Load(pokemon);
        }
    }

    public int GetDirectionStartingIndex(MoveBrainDirection direction)
    {
        switch (direction)
        {
            default:
                return 0;
            case MoveBrainDirection.Left:
                return spriteSheetColumns * 1;
            case MoveBrainDirection.Right:
                return spriteSheetColumns * 2;
            case MoveBrainDirection.Top:
                return spriteSheetColumns * 3;
        }
    }

    public string GetPath(PokemonCaughtData pokemon)
    {
        return ResourceMaster.Instance.GetBaseWorldPath(pokemon.GetPokemonBaseData());
    }

    public WorldInteractableWorldBrainPokemon Load(PokemonCaughtData pokemon)
    {
        this.pokemon = pokemon;
        Sprite[] sprites = Resources.LoadAll<Sprite>(GetPath(pokemon));

        spritesDown = new Sprite[spriteSheetColumns];
        spritesLeft = new Sprite[spriteSheetColumns];
        spritesRight = new Sprite[spriteSheetColumns];
        spritesUp = new Sprite[spriteSheetColumns];
        if (sprites.Length == 0)
            Debug.LogError(pokemon.GetPokemonBaseData().species + " has no sprites for world");
        Array.Copy(sprites, 0, spritesDown, 0, spriteSheetColumns);
        Array.Copy(sprites, GetDirectionStartingIndex(MoveBrainDirection.Left), spritesLeft, 0, spriteSheetColumns);
        Array.Copy(sprites, GetDirectionStartingIndex(MoveBrainDirection.Right), spritesRight, 0, spriteSheetColumns);
        Array.Copy(sprites, GetDirectionStartingIndex(MoveBrainDirection.Top), spritesUp, 0, spriteSheetColumns);
        HandleAnimator(true);
        base.Load();

        return this;
    }

    public MoveBrainDirection GetDirectionFromVector(Vector2 direction)
    {
        if (direction.x < 0)
        {
            return MoveBrainDirection.Left;
        }
        else if (direction.x > 0)
        {
            return MoveBrainDirection.Right;
        }
        else if (direction.y > 0)
        {
            return MoveBrainDirection.Top;
        }
        else
        {
            return MoveBrainDirection.Bottom;
        }
    }

    public Sprite GetSprite()
    {
        Vector2 direction = lastDirection;
        frameIndex = frameIndex % spriteSheetColumns;
        switch (GetDirectionFromVector(direction))
        {
            case MoveBrainDirection.Left:
                return spritesLeft[frameIndex];
            case MoveBrainDirection.Right:
                return spritesRight[frameIndex];
            case MoveBrainDirection.Top:
                return spritesUp[frameIndex];
            default:
                return spritesDown[frameIndex];
        }
    }

    public void HandleAnimator(bool force = false)
    {
        if (force)
        {
            sprite.sprite = GetSprite();
        }
        else if (timePassed > intervalBetweenSpriteChange)
        {
            timePassed -= intervalBetweenSpriteChange;
            frameIndex++;
            sprite.sprite = GetSprite();
        }
    }

    protected override void Update()
    {
        sprite.sortingOrder = (int)(transform.position.y * -10 + heightOffset);
        timePassed += Time.deltaTime;
        if (HasReachedTarget())
        {
            if (cachedDirections.Count > 0)
            {
                Vector2 direction = (Vector3)GetCurrentDirection();
                bool justTurn = cachedDirections[0].justTurn;
                if (!justTurn)
                    target = transform.position + (Vector3)direction;
                lastDirection = direction;
                cachedDirections.RemoveAt(0);
            }
            else
            {
                // Idle behavoiur if any
            }
        }
        else
        {
            if (followMode)
            {
                Vector3 direction = ((Vector3)target - transform.position).normalized;
                if (Vector3.Distance(direction, lastDirection) != 0)
                {
                    lastDirection = direction;
                    // We want to update the sprite instantly when we change direction
                    HandleAnimator(true);
                }
                else
                {
                    lastDirection = direction;
                }
                float playerSpeed = WorldMapMaster.GetInstance().GetPlayer().speed;
                transform.position = Vector2.MoveTowards(transform.position, target, playerSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
            }
        }
        HandleAnimator();
    }
}
