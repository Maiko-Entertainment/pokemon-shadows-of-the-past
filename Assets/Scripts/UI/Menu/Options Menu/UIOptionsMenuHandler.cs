using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOptionsMenuHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Close()
    {
        UIPauseMenuMaster.GetInstance().CloseMenu();
    }
}
