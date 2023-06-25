using System.Collections;
using System.Collections.Generic;
using TMPro;
using static UnityEngine.InputSystem.InputAction;

public class UIMenuConfirm : UIMenuPile
{
    public TMP_Text title;
    public TextMeshProUGUI description;

    public delegate void OnInput();
    public OnInput onConfirm;
    public OnInput onDeny;

    public UIMenuConfirm Load(string title, string description, OnInput onConfirm, OnInput onDeny)
    {
        this.title.text = title;
        this.description.text = description;
        this.onConfirm += onConfirm;
        this.onDeny += onDeny;
        return this;
    }

    public void HandleConfirm(CallbackContext context)
    {
        if (context.phase == UnityEngine.InputSystem.InputActionPhase.Started && canvasGroup.interactable)
        {
            HandleConfirm();
        }
    }
    public void HandleDeny(CallbackContext context)
    {
        if (context.phase == UnityEngine.InputSystem.InputActionPhase.Started && canvasGroup.interactable)
        {
            HandleDeny();
        }
    }

    public void HandleConfirm()
    {
        UIPauseMenuMaster.GetInstance()?.CloseCurrentMenu();
        onConfirm?.Invoke();
    }
    public void HandleDeny()
    {
        UIPauseMenuMaster.GetInstance()?.CloseCurrentMenu();
        onDeny?.Invoke();
    }
}
