using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{
    public float speed = 1f;
    public Animator animator;
    public Collider2D touchCollider;
    public Tilemap groundTilemap;
    public Tilemap waterTilemap;
    public Tilemap collisionTilemap;
    public List<string> customColissionTags;
    public Sprite preview;
    public AudioClip jumpClip;

    // Instanciated on play
    public List<WorldInteractableBrainFollower> followers = new List<WorldInteractableBrainFollower>();

    private Controls controls;
    private bool wantsToMove = false;
    private Vector2 target;
    private Vector2 cacheDirection;
    private bool waterMode = false;
    private bool jumping = false;
    private List<Vector3> movements = new List<Vector3>();

    List<MoveBrainDirectionData> storedDirections = new List<MoveBrainDirectionData>();

    void Awake()
    {
        controls = new Controls();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    public void Load(WorldMap map)
    {
        groundTilemap = map.groundTilemap;
        waterTilemap = map.waterTilemap;
        collisionTilemap = map.collisionTilemap;
        target = transform.position;
        UpdatePokeFollower();
    }

    void Start()
    {
        StartCoroutine(SetControls(0.2f));
    }

    IEnumerator SetControls(float delay)
    {
        yield return new WaitForSeconds(delay);
        // controls.PlayerCharacter.Movement.performed += ctx => HandleMovementStart(ctx);
        // controls.PlayerCharacter.Movement.canceled += ctx => HandleMovementStop();
        controls.PlayerCharacter.Interact.started += ctx => Interact(ctx);
        target = transform.position;
    }
    void Interact(CallbackContext context)
    {
        bool isInteractionPlaying = InteractionsMaster.GetInstance().IsInteractionPlaying();
        if (!isInteractionPlaying && storedDirections.Count == 0)
        {
            touchCollider.gameObject.SetActive(true);
            if (Mathf.Abs(cacheDirection.x) > 0)
            {
                touchCollider.transform.localPosition = Vector3.right * cacheDirection.x;
            }
            else if (Mathf.Abs(cacheDirection.y) > 0)
            {
                touchCollider.transform.localPosition = Vector3.up * cacheDirection.y;
            }
            StartCoroutine(ResetAfterFrame());
        }
        else
        {
            touchCollider.gameObject.SetActive(false);
        }
    }

    IEnumerator ResetAfterFrame()
    {
        yield return new WaitForSeconds(0.1f);
        touchCollider.transform.localPosition = Vector3.zero;
    }

    public void ClearCache()
    {
        cacheDirection = Vector2.zero;
    }

    void HandleMovementStop()
    {
        wantsToMove = false;
    }

    void Move(Vector2 direction)
    {
        cacheDirection = direction;
        wantsToMove = true;
    }

    bool CanMove(Vector2 direction)
    {
        if (!groundTilemap || !collisionTilemap || !waterTilemap)
            return false;
        Vector3 nextPosition = transform.position + (Vector3)direction;
        Vector3Int gridPosition = groundTilemap.WorldToCell(nextPosition);
        bool reachedTarget = HasReachedTarget();
        bool wouldHitCustomColliders = AreCustomCollidersInPosition(nextPosition);
        if (jumping) return wantsToMove && reachedTarget;
        if (!groundTilemap.HasTile(gridPosition) || collisionTilemap.HasTile(gridPosition) || (waterTilemap.HasTile(gridPosition) && !waterMode) || wouldHitCustomColliders)
        {
            return false;
        }
        return wantsToMove && reachedTarget;
    }

    bool AreCustomCollidersInPosition(Vector2 position)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(position, 0.4f);
        foreach(Collider2D col in hitColliders)
        {
            // print("Found collider for custom check: " + col.tag);
            if (customColissionTags.Contains(col.tag))
                return true;
        }
        return false;
    }

    bool HasReachedTarget()
    {
        return Vector2.Distance(transform.position, target) == 0;
    }
    Vector2 GetCurrentStoredDirection()
    {
        if (storedDirections.Count == 0)
            return Vector2.zero;
        MoveBrainDirection direction = storedDirections[0].direction;
        switch (direction)
        {
            case MoveBrainDirection.Top:
                return Vector2.up;
            case MoveBrainDirection.Left:
                return Vector2.left;
            case MoveBrainDirection.Bottom:
                return Vector2.down;
            case MoveBrainDirection.Right:
                return Vector2.right;
            default:
                return Vector2.zero;
        }
    }

    public float AddDirection(MoveBrainDirectionData direction)
    {
        storedDirections.Add(direction);
        float movesSum = 0;
        foreach (MoveBrainDirectionData d in storedDirections)
        {
            if (!d.justTurn)
                movesSum += 1;
        }
        ClearCache();
        return movesSum / speed;
    }

    public void AddFollower(WorldInteractableBrainFollower newFollowerPrefab, bool repeatable=false)
    {
        WorldInteractableBrainFollower newFollower = Instantiate(newFollowerPrefab);
        AddFollowerInstanced(newFollower, repeatable);
    }

    public void AddFollowerInstanced(WorldInteractableBrainFollower newFollower, bool repeatable = false)
    {
        bool alreadyFollowing = false;
        if (!repeatable)
        {
            foreach (WorldInteractableBrainFollower follower in followers)
            {
                if (follower.moveIdentifier == newFollower.moveIdentifier)
                {
                    alreadyFollowing = true;
                    break;
                }
            }
        }
        if (!alreadyFollowing || repeatable)
        {
            newFollower.followMode = true;
            newFollower.gameObject.tag = "Untagged";
            if (newFollower.GetComponent<BoxCollider2D>())
                newFollower.GetComponent<BoxCollider2D>().isTrigger = true;
            newFollower.transform.position = transform.position;
            newFollower.SetTarget(transform.position);
            followers.Add(newFollower);
        }
        else
        {
            Destroy(newFollower.gameObject);
        }
    }

    public WorldInteractableBrainFollower RemoveFollower(string followerId)
    {
        List<WorldInteractableBrainFollower> newFollowers = new List<WorldInteractableBrainFollower>();
        WorldInteractableBrainFollower removedFollower = null;
        foreach (WorldInteractableBrainFollower follower in followers)
        {
            if (follower.moveIdentifier == followerId)
            {
                follower.followMode = true;
                follower.gameObject.tag = "Object";
                if (follower.GetComponent<BoxCollider2D>())
                    follower.GetComponent<BoxCollider2D>().isTrigger = false;
                follower.transform.parent = null;
                removedFollower = follower;
            }
            else
            {
                newFollowers.Add(follower);
            }
        }
        followers = newFollowers;
        return removedFollower;
    }
    public void DestroyFollowers()
    {
        foreach (WorldInteractableBrainFollower follower in followers)
        {
            Destroy(follower.gameObject);
        }
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
        target = position;
        ResetFollowersPosition();
    }

    public void HideFollowers()
    {
        foreach (WorldInteractableBrainFollower follow in followers)
        {
            follow.gameObject.SetActive(false);
        }
    }

    public void ShowFollowers()
    {
        foreach (WorldInteractableBrainFollower follow in followers)
        {
            follow.gameObject.SetActive(true);
            ResetFollowersPosition();
        }
    }

    public void UpdatePokeFollower()
    {
        List<WorldInteractableBrainFollower> newList = new List<WorldInteractableBrainFollower>();
        foreach (WorldInteractableBrainFollower follow in followers)
        {
            if (follow.GetIdentifier() == "Pokefollower")
            {
                Destroy(follow.gameObject);
            }
            else
            {
                newList.Add(follow);
            }
        }
        followers = newList;
        PokemonCaughtData current = PartyMaster.GetInstance().GetFirstAvailablePokemon();
        if (current != null)
        {
            WorldInteractableWorldBrainPokemon followerPrefab = WorldMapMaster.GetInstance().pokeFollowerPrefab;
            if (followerPrefab)
            {
                WorldInteractableWorldBrainPokemon follower = Instantiate(followerPrefab);
                follower.pokemon = current;
                follower.transform.position = transform.position;
                followers.Insert(0, follower);
            }
        }
    }

    public void NotifyMoveToFollowers(Vector3 direction)
    {
        movements.Insert(0, direction);
        // Always 1 block away from previous
        int index = 1;
        foreach (WorldInteractableBrainFollower follower in followers)
        {
            if (index < movements.Count)
            {
                follower.SetTarget(movements[index]);
                index++;
            }
        }
    }

    public void ResetFollowersPosition()
    {
        movements.Clear();
        List<WorldInteractableBrainFollower> newFollowers = new List<WorldInteractableBrainFollower>();
        foreach (WorldInteractableBrainFollower follower in followers)
        {
            if (follower != null)
                newFollowers.Add(follower);
        }
        followers = newFollowers;
        foreach (WorldInteractableBrainFollower follower in followers)
        {
            follower.transform.position = transform.position;
        }
    }

    public void ReadMovement()
    {
        bool isInteractionPlaying = InteractionsMaster.GetInstance().IsInteractionPlaying();
        if (!isInteractionPlaying)
        {
            Vector2 movement = controls.PlayerCharacter.Movement.ReadValue<Vector2>();
            if (movement.y != 0 && cacheDirection.x != 0)
            {
                movement.x = 0;
            }
            if (movement.x != 0 && cacheDirection.y != 0)
            {
                movement.y = 0;
            }
            if (movement.x == 0 && movement.y == 0)
                HandleMovementStop();
            else
                Move(movement);
        }
    }

    void Update()
    {
        animator.GetComponent<SpriteRenderer>().sortingOrder = (int)(transform.position.y * -10);
        if (HasReachedTarget())
        {
            ReadMovement();
            if (storedDirections.Count > 0)
            {
                Vector2 direction = (Vector3)GetCurrentStoredDirection();
                bool justTurn = storedDirections[0].justTurn;
                if (!jumping && storedDirections[0].jump)
                {
                    AudioMaster.GetInstance().PlaySfx(jumpClip);
                }
                jumping = storedDirections[0].jump;
                if (!justTurn)
                {
                    target = transform.position + (Vector3)direction;
                    NotifyMoveToFollowers(target);
                }
                animator.SetFloat("Horizontal", GetCurrentStoredDirection().x);
                animator.SetFloat("Vertical", GetCurrentStoredDirection().y);
                // 
                cacheDirection = direction;
                storedDirections.RemoveAt(0);
            }
            else
            {
                bool isInteractionPlaying = InteractionsMaster.GetInstance().IsInteractionPlaying();
                bool isTransitioning = TransitionMaster.GetInstance().IsTransitioning();
                jumping = false;
                if (CanMove(cacheDirection) && !isInteractionPlaying && !isTransitioning)
                {
                    target = transform.position + (Vector3)cacheDirection;
                    NotifyMoveToFollowers(target);
                }
                else
                {
                    animator.SetBool("Moving", false);
                }
                animator.SetFloat("Horizontal", cacheDirection.x);
                animator.SetFloat("Vertical", cacheDirection.y);
            }
            ReturnToGround();
        }
        else
        {
            animator.SetBool("Moving", true);
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
            animator.speed = 1;
            if (jumping)
            {
                animator.speed = 0;
                Vector3 jumpHeight = Vector3.up * .5f;
                animator.gameObject.transform.localPosition = Vector3.MoveTowards(animator.gameObject.transform.localPosition, jumpHeight, 2f * Time.deltaTime);
            }
            else
            {
                ReturnToGround();
            }
        }
    }

    private void ReturnToGround()
    {
        animator.gameObject.transform.localPosition = Vector3.MoveTowards(animator.gameObject.transform.localPosition, Vector2.zero, 3f * Time.deltaTime);
    }
}
