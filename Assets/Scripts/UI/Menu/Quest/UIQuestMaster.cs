using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestMaster : MonoBehaviour
{
    public static UIQuestMaster Instance;

    public List<UIQuestData> questsData = new List<UIQuestData>();

    public TextMeshProUGUI questTitle;
    public TransitionBase transitionComponent;
    public Animator exclamationAnimator;

    protected UIQuestData currentQuest;
    protected bool fading = false;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        LoadCurrentQuest();
    }

    public UIQuestMaster GetInstance()
    {
        return Instance;
    }

    public void HideQuest()
    {
        transitionComponent?.FadeOut();
    }
    public void ShowQuest()
    {
        if (currentQuest && !fading)
        {
            transitionComponent?.FadeIn();
        }
    }

    public void LoadCurrentQuest()
    {
        UIQuestData questData = null;
        foreach(UIQuestData q in questsData)
        {
            if (q.MeetsAppearConditions() && !q.IsFinished())
            {
                questData = q;
                break;
            }
        }
        if (questData && questTitle)
        {
            if (currentQuest != questData)
            {
                TransitionNewQuest(questData);
            }
            else
            {
                questTitle.text = questData.GetTitle();
                transitionComponent?.FadeIn();
            }
            currentQuest = questData;
        }
        else if (questTitle)
        {
            transitionComponent?.FadeOut();
        }
    }

    public void TransitionNewQuest(UIQuestData questData)
    {
        StartCoroutine(Transition(questData));
    }

    protected IEnumerator Transition(UIQuestData questData)
    {
        fading = true;
        float time = 1 / transitionComponent.speed;
        transitionComponent.FadeOut();
        yield return new WaitForSeconds(time);
        questTitle.text = questData.GetTitle();
        yield return new WaitForEndOfFrame();
        transitionComponent.FadeIn();
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
        yield return new WaitForSeconds(time);
        exclamationAnimator?.SetTrigger("Shake");
        fading = false;
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
