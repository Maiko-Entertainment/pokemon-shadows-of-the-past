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

    private Controls controls;
    private bool wantsToMove = false;
    private Vector2 target;
    private Vector2 cacheDirection;
    private bool waterMode = false;

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
    }

    void Start()
    {
        controls.PlayerCharacter.Movement.started += ctx => HandleMovementStart(ctx);
        controls.PlayerCharacter.Movement.canceled += ctx => HandleMovementStop();
        controls.PlayerCharacter.Interact.started += ctx => Interact(ctx);
        target = transform.position;
    }
    void Interact(CallbackContext context)
    {
        bool isInteractionPlaying = InteractionsMaster.GetInstance().IsInteractionPlaying();
        if (!isInteractionPlaying)
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
        Vector3Int gridPosition = groundTilemap.WorldToCell(transform.position + (Vector3)direction);
        bool reachedTarget = HasReachedTarget();
        if (!groundTilemap.HasTile(gridPosition) || collisionTilemap.HasTile(gridPosition) || (waterTilemap.HasTile(gridPosition) && !waterMode))
        {
            return false;
        }
        return wantsToMove && reachedTarget;
    }

    bool HasReachedTarget()
    {
        return Vector2.Distance(transform.position, target) == 0;
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, target) != 0)
        {
            animator.SetBool("Moving", true);
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }
        else
        {
            bool isInteractionPlaying = InteractionsMaster.GetInstance().IsInteractionPlaying();
            if (CanMove(cacheDirection) && !isInteractionPlaying)
            {
                target = transform.position + (Vector3)cacheDirection;
                animator.SetFloat("Horizontal", cacheDirection.x);
                animator.SetFloat("Vertical", cacheDirection.y);
            }
            else
            {
                animator.SetBool("Moving", false);
            }
        }
    }
}
