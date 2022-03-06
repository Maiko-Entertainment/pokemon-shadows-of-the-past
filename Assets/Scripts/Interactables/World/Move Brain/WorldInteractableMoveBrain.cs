using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldInteractableMoveBrain : WorldInteractable
{
    public string moveIdentifier;

    public Animator animator;
    public Transform innerTransform;
    public float speed = 1;

    protected Tilemap groundTilemap;
    protected Tilemap waterTilemap;
    protected Tilemap collisionTilemap;

    protected Vector2 target;
    protected List<MoveBrainDirectionData> cachedDirections = new List<MoveBrainDirectionData>();
    protected float jumpHeight = 0.5f;
    protected float jumpSpeed = 1f;

    private void Start()
    {
        Load();
    }

    public void Load()
    {
        WorldMap currentMap = WorldMapMaster.GetInstance().GetCurrentMap();
        groundTilemap = currentMap.groundTilemap;
        waterTilemap = currentMap.waterTilemap;
        collisionTilemap = currentMap.collisionTilemap;
        target = transform.position;
    }

    public string GetIdentifier()
    {
        return moveIdentifier;
    }

    public void SetTarget(Vector3 newTarget)
    {
        target = newTarget;
    }
    // return time it takes to complete current walk
    public float AddDirection(MoveBrainDirectionData direction)
    {
        cachedDirections.Add(direction);
        float movesSum = 0;
        foreach(MoveBrainDirectionData d in cachedDirections)
        {
            if (!d.justTurn)
                movesSum += 1;
        }
        return movesSum / speed;
    }
    protected bool HasReachedTarget()
    {
        return Vector2.Distance(transform.position, target) == 0;
    }
    protected Vector2 GetCurrentDirection()
    {
        if (cachedDirections.Count == 0)
            return Vector2.zero;
        MoveBrainDirection direction = cachedDirections[0].direction;
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

    public float Jump()
    {
        StartCoroutine(BeginJump());
        return jumpSpeed * 2 + 0.1f;
    }

    IEnumerator BeginJump()
    {
        Vector3 target = innerTransform.localPosition + Vector3.up * jumpHeight;
        Vector3 startingPos = innerTransform.localPosition;
        float timePassed = 0;
        while (Vector3.Distance(innerTransform.localPosition, target) > 0)
        {
            timePassed += Time.deltaTime;
            Vector3.Lerp(innerTransform.localPosition, target, timePassed * jumpSpeed);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(0.1f);
        while (Vector3.Distance(innerTransform.localPosition, startingPos) > 0)
        {
            timePassed += Time.deltaTime;
            Vector3.Lerp(innerTransform.localPosition, startingPos, timePassed * jumpSpeed);
            yield return new WaitForEndOfFrame();
        }
    }

    protected virtual void Update()
    {
        if (HasReachedTarget())
        {
            if (cachedDirections.Count > 0)
            {
                Vector2 direction = (Vector3)GetCurrentDirection();
                bool justTurn = cachedDirections[0].justTurn;
                if (!justTurn)
                    target = transform.position + (Vector3)direction;
                animator.SetFloat("Horizontal", GetCurrentDirection().x);
                animator.SetFloat("Vertical", GetCurrentDirection().y);
                cachedDirections.RemoveAt(0);
            }
            else
            {
                animator.SetBool("Moving", false);
            }
        }
        else
        {
            animator.SetBool("Moving", true);
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }
    }
}
