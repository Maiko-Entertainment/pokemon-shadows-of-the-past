using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanUseResult
{
    public bool canUse;
    public string message;

    public CanUseResult(bool canUse, string message)
    {
        this.canUse = canUse;
        this.message = message;
    }
}
