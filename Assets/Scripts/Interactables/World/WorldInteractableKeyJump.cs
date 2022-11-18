using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class WorldInteractableKeyJump : WorldInteractableKey
{
    public MoveBrainDirection direction = MoveBrainDirection.Bottom;
    public override void OnInteract()
    {

    }

    public void KeyInteract(CallbackContext context)
    {
        Vector2 readValue = context.ReadValue<Vector2>();
        if (CanInteract() && isPlayerInside && context.phase == UnityEngine.InputSystem.InputActionPhase.Started)
        {
            MoveBrainDirectionData move = new MoveBrainDirectionData();
            move.direction = direction;
            move.jump = true;
            switch (direction)
            {
                case MoveBrainDirection.Right:
                    if (readValue.x > 0)
                    {
                        WorldMapMaster.GetInstance().GetPlayer().AddDirection(move);
                        WorldMapMaster.GetInstance().GetPlayer().AddDirection(move);
                        MarkInteracted();
                    }
                    break;
                case MoveBrainDirection.Left:
                    if (readValue.x < 0)
                    {
                        WorldMapMaster.GetInstance().GetPlayer().AddDirection(move);
                        WorldMapMaster.GetInstance().GetPlayer().AddDirection(move);
                        MarkInteracted();
                    }
                    break;
                case MoveBrainDirection.Top:
                    if (readValue.y > 0)
                    {
                        WorldMapMaster.GetInstance().GetPlayer().AddDirection(move);
                        WorldMapMaster.GetInstance().GetPlayer().AddDirection(move);
                        MarkInteracted();
                    }
                    break;
                case MoveBrainDirection.Bottom:
                    if (readValue.y < 0)
                    {
                        WorldMapMaster.GetInstance().GetPlayer().AddDirection(move);
                        WorldMapMaster.GetInstance().GetPlayer().AddDirection(move);
                        MarkInteracted();
                    }
                    break;
            }
        }
    }
}
