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
        controls.PlayerCharacter.Movement.started += ctx => HandleMovementStart(ctx);
        controls.PlayerCharacter.Movement.canceled += ctx => HandleMovementStop();
        controls.PlayerCharacter.Interact.started += ctx => Interact(ctx);
        target = transform.position;
    }
    void Interact(CallbackContext context)
    {
        bool isInteractionPlaying = InteractionsMaster.GetInstance().IsInteractionPlaying();
        if (!isInteractionPlaying && storedDirections.Count == 0)
        {
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
    }

    IEnumerator ResetAfterFrame()
    {
        yield return new WaitForSeconds(0.1f);
        touchCollider.transform.localPosition = Vector3.zero;
    }

    void HandleMovementStart(CallbackContext context)
    {
        bool isInteractionPlaying = InteractionsMaster.GetInstance().IsInteractionPlaying();
        if (!isInteractionPlaying)
        {
            Move(context.ReadValue<Vector2>());
        }
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
        return movesSum / speed;
    }

    public void AddFollower(WorldInteractableBrainFollower newFollowerPrefab)
    {
        WorldInteractableBrainFollower newFollower = Instantiate(newFollowerPrefab);
        followers.Add(newFollower);
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
            WorldInteractableBrainFollower followerPrefab = current.GetPokemonBaseData().GetOverWorldPrefab();
            if (followerPrefab)
            {
                WorldInteractableBrainFollower follower = Instantiate(followerPrefab);
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

            }
        }
    }

    public void ResetFollowersPosition()
    {
        movements.Clear();
        foreach (WorldInteractableBrainFollower follower in followers)
        {
            follower.transform.position = transform.position;
        }
    }

    void Update()
    {
        animator.GetComponent<SpriteRenderer>().sortingOrder = (int)transform.position.y * -1;
        if (HasReachedTarget())
        {
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
                }
                animator.SetFloat("Horizontal", GetCurrentStoredDirection().x);
                animator.SetFloat("Vertical", GetCurrentStoredDirection().y);
                storedDirections.RemoveAt(0);
            }
            else
            {
                bool isInteractionPlaying = InteractionsMaster.GetInstance().IsInteractionPlaying();
                jumping = false;
                if (CanMove(cacheDirection) && !isInteractionPlaying)
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
            transform.position = Vector2.MoveTowards(transform.position, target, (jumping ? 1f : 1f) * speed * Time.deltaTime);
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
