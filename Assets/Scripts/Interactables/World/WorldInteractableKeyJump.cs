using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class WorldInteractableKeyJump : WorldInteractableKey
{
    public MoveBrainDirection direction = MoveBrainDirection.Bottom;

    bool isPressing = true;

    public override void OnInteract()
    {

    }

    public void KeyInteract(CallbackContext context)
    {
        Vector2 readValue = context.ReadValue<Vector2>();
        switch (direction)
        {
            case MoveBrainDirection.Right:
                isPressing = readValue.x > 0;
                break;
            case MoveBrainDirection.Left:
                isPressing = readValue.x < 0;
                break;
            case MoveBrainDirection.Top:
                isPressing = readValue.y > 0;
                break;
            case MoveBrainDirection.Bottom:
                isPressing = readValue.y < 0;
                break;
        }
    }
    private void Update()
    {
        if (CanInteract() && isPlayerInside && isPressing)
        {
            MoveBrainDirectionData move = new MoveBrainDirectionData(direction, false);
            move.jump = true;
            WorldMapMaster.GetInstance().GetPlayer().AddDirection(move);
            WorldMapMaster.GetInstance().GetPlayer().AddDirection(move);
            MarkInteracted();
        }   
    }
}
