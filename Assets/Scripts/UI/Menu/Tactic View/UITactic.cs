using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITactic : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI cost;
    public Image icon;
    public Image iconShadow;
    public TransitionFade selectedArrow;
    public TransitionFade pickedIcon;
    public Color notEnoughGaugeColor;
    public AudioClip selectClip;
    public AudioClip clickClip;
    public AudioClip errorClip;

    public delegate void Select(TacticData tactic);
    public event Select onSelect;
    public delegate void Click(TacticData tactic);
    public event Click onClick;

    public TacticData tactic;
    private bool canPay = true;

    public UITactic Load(TacticData tactic)
    {
        this.tactic = tactic;

        title.text = tactic.GetName();
        cost.text = ""+tactic.GetCost();
        icon.sprite = tactic.tacticIcon;
        iconShadow.sprite = tactic.tacticIcon;
        return this;
    }

    public void UpdateCanPay(int currentGauge)
    {
        if (tactic.GetCost() > currentGauge)
        {
            cost.color = notEnoughGaugeColor;
            canPay = false;
        }
        else
        {
            canPay = true;
        }
    }

    public void HandleClick()
    {
        if (canPay)
        {
            AudioMaster.GetInstance().PlaySfx(clickClip);
        }
        else
        {
            AudioMaster.GetInstance().PlaySfx(errorClip);
        }
        onClick?.Invoke(tactic);
    }
    public void HandleSelect()
    {
        AudioMaster.GetInstance().PlaySfx(selectClip);
        onSelect?.Invoke(tactic);
    }

    public void UpdateSelectedStatus(TacticData tactic)
    {
        if (this.tactic == tactic)
        {
            selectedArrow.FadeIn();
        }
        else
        {
            selectedArrow.FadeOut();
        }
    }
    public void UpdatePickedStatus(TacticData tactic)
    {
        if (this.tactic == tactic)
        {
            pickedIcon.FadeIn();
        }
        else
        {
            pickedIcon.FadeOut();
        }
    }
}
