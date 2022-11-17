using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class UIBattleOptionsTactics : MonoBehaviour
{
    public UITactic tacticprefab;

    public TextMeshProUGUI description;
    public Transform tacticListContainer;
    private void Start()
    {
        Load();
    }
    public void Load()
    {
        List<TacticData> tactics = BattleMaster.GetInstance().GetCurrentBattle().GetTeamData(BattleTeamId.Team1).tactics;
        List<Selectable> selectables = new List<Selectable>();
        TacticData currentTacticSelected = BattleMaster.GetInstance().GetCurrentBattle().GetSelectedTactic();
        int tacticGauge = BattleMaster.GetInstance().GetCurrentBattle().GetTeamData(BattleTeamId.Team1).tacticGauge;
        foreach (Transform t in tacticListContainer)
        {
            Destroy(t.gameObject);
        }
        int index = 0;
        foreach (TacticData tactic in tactics)
        {
            UITactic uitactic = Instantiate(tacticprefab, tacticListContainer).Load(tactic);
            selectables.Add(uitactic.GetComponent<Button>());
            uitactic.onClick += PickTactic;
            uitactic.onSelect += SetAsSelected;
            uitactic.UpdateCanPay(tacticGauge);
            if (index == 0)
            {
                UtilsMaster.SetSelected(uitactic.gameObject);
                uitactic.UpdateSelectedStatus(tactic);
            }
            uitactic.UpdatePickedStatus(currentTacticSelected);
            index++;
        }
        UtilsMaster.LineSelectables(selectables);
    }

    public void PickTactic(TacticData tactic)
    {
        int tacticGauge = BattleMaster.GetInstance().GetCurrentBattle().GetTeamData(BattleTeamId.Team1).tacticGauge;

        if (BattleMaster.GetInstance().GetCurrentBattle().GetSelectedTactic() == tactic)
        {
            BattleMaster.GetInstance().GetCurrentBattle().SetSelectedTactic(null);
            HandleClose();
        }
        else if (tactic.GetCost() <= tacticGauge) { 
            BattleMaster.GetInstance().GetCurrentBattle().SetSelectedTactic(tactic);
            HandleClose();
        }
    }
    public void SetAsSelected(TacticData tactic)
    {
        description.text = tactic.tacticDescription;
        foreach (Transform t in tacticListContainer)
        {
            t.GetComponent<UITactic>().UpdateSelectedStatus(tactic);
        }
    }

    public void HandleClose(CallbackContext context)
    {
        if (context.phase == UnityEngine.InputSystem.InputActionPhase.Started)
        {
            HandleClose();
        }
    }
    public void HandleClose()
    {
        BattleAnimatorMaster.GetInstance().HideTacticSelection(true);
    }
}
