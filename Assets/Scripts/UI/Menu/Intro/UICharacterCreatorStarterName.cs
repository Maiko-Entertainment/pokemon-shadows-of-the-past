using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class UICharacterCreatorStarterName : MonoBehaviour
{
    public void OnSubmit(string pokemonName)
    {
        List<PokemonCaughtData> party = PartyMaster.GetInstance()?.GetParty();
        if (party.Count > 0)
        {
            party[0].SetName(pokemonName);
        }
        UIPauseMenuMaster.GetInstance()?.UpdatePartyMiniPreview();
        UIPauseMenuMaster.GetInstance()?.CloseAllMenus();
        InteractionsMaster.GetInstance().ExecuteNext();
    }
    public void HandleGoBack(CallbackContext context)
    {
        bool interactable = GetComponent<CanvasGroup>().interactable;
        if (context.phase == UnityEngine.InputSystem.InputActionPhase.Started && interactable)
        {
            UIPauseMenuMaster.GetInstance().CloseCurrentMenu();
        }
    }
}
