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
    public Tilemap groundTilemap;
    public Tilemap collisionTilemap;

    private Controls controls;
    private bool wantsToMove = false;
    private Vector2 target;
    private Vector2 cacheDirection;

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

    void Start()
    {
        controls.PlayerCharacter.Movement.started += ctx => HandleMovementStart(ctx);
        controls.PlayerCharacter.Movement.canceled += ctx => HandleMovementStop();
        target = transform.position;
    }

    void HandleMovementStart(CallbackContext context)
    {
         Move(context.ReadValue<Vector2>());
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
        if (!groundTilemap.HasTile(gridPosition) || collisionTilemap.HasTile(gridPosition))
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
            if (CanMove(cacheDirection))
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
