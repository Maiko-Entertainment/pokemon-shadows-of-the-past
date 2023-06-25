using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISaveFiles : MonoBehaviour
{
    public UISaveFileOption optionPrefab;
    public ViewTransition saveTransitionPrefab;
    public ViewTransition loadTransitionPrefab;
    public AudioClip selectSound;

    public RectTransform saveFileList;
    public ScrollRect scrollRect;
    public Button newGameButton;

    public List<GameObject> activeInGame = new List<GameObject>();
    public List<GameObject> activeInMainMenu = new List<GameObject>();

    protected int currentSaveIndex = 0;
    protected bool disabled = false;

    public void Load(bool isMainMenu = false)
    {
        List<SaveFile> saveFiles = SaveMaster.Instance.GetSaveFiles();
        bool reachedMaxSaves = saveFiles.Count >= SaveMaster.Instance.maxSaveFiles;
        List<Selectable> selectables = new List<Selectable> { newGameButton };
        foreach (Transform t in saveFileList)
        {
            if (t != saveFileList.GetChild(0) || reachedMaxSaves || isMainMenu)
            {
                Destroy(t.gameObject);
            }
        }
        int index = 0;
        foreach (SaveFile sf in saveFiles)
        {
            UISaveFileOption sfo = Instantiate(optionPrefab, saveFileList).Load(sf, index);
            sfo.onClick += isMainMenu ? (SaveFile sf, int i) => LoadSaveFile(i) : OverrideSaveFile;
            sfo.onHover += (SaveFile sf, int i) => ViewSaveFile(sfo.GetComponent<RectTransform>(), i);
            selectables.Add(sfo.GetComponent<Button>());
            index++;
        }
        UtilsMaster.LineSelectables(selectables);
        int activeIndexSelected = SaveMaster.Instance.lastSaveIndex;
        if (SaveMaster.Instance.saveFiles.Count > activeIndexSelected)
        {
            UtilsMaster.SetSelected(selectables[activeIndexSelected + 1].gameObject);
        }
        else
        {
            UtilsMaster.SetSelected(selectables[0].gameObject);
        }
        foreach (GameObject go in activeInGame)
        {
            go.SetActive(!isMainMenu);
        }
        foreach (GameObject go in activeInMainMenu)
        {
            go.SetActive(isMainMenu);
        }
    }

    public void OverrideSaveFile(SaveFile sf, int index)
    {
        if (!disabled)
        {
            UIPauseMenuMaster.GetInstance()?.OpenConfirmMenu(
                "Override Savefile?",
                "Once a savefile is overritten it can't be recovered.",
                () =>
                {
                    int indexOfSaveFile = SaveMaster.Instance.GetSaveFiles().IndexOf(sf);
                    SaveMaster.Instance.Save(indexOfSaveFile);
                    TransitionMaster.GetInstance()?.RunTransition(saveTransitionPrefab);
                    StartCoroutine(EnableAfterDelay(saveTransitionPrefab.totalDuration));
                    Load();
                });
        }
    }

    public void ViewSaveFile(RectTransform t, int index)
    {
        UtilsMaster.GetSnapToPositionToBringChildIntoView(scrollRect, t);
        AudioMaster.GetInstance().PlaySfx(selectSound);
        currentSaveIndex = index;
    }

    public void SelectSaveNewGame()
    {
        AudioMaster.GetInstance().PlaySfx(selectSound);
        UtilsMaster.GetSnapToPositionToBringChildIntoView(scrollRect, newGameButton.GetComponent<RectTransform>());
    }

    public void HandleSaveNewGame()
    {
        if (!disabled)
        {
            SaveMaster.Instance.Save(SaveMaster.Instance.GetSaveFiles().Count);
            TransitionMaster.GetInstance()?.RunTransition(saveTransitionPrefab);
            StartCoroutine(EnableAfterDelay(saveTransitionPrefab.totalDuration));
            Load();
        }
    }
    public void LoadSaveFile(int index)
    {
        if (!disabled)
        {
            TransitionMaster.GetInstance()?.RunTransition(loadTransitionPrefab);
            StartCoroutine(LoadSaveAfterTransition(loadTransitionPrefab.changeTime, index));
            disabled = true;
        }
    }

    public void LoadSelectedSave()
    {
        if (!disabled)
        {
            TransitionMaster.GetInstance()?.RunTransition(loadTransitionPrefab);
            StartCoroutine(LoadSaveAfterTransition(loadTransitionPrefab.changeTime, currentSaveIndex));
            disabled = true;
        }
    }

    protected IEnumerator LoadSaveAfterTransition(float delay, int index)
    {
        yield return new WaitForSeconds(delay);
        SaveMaster.Instance.LoadSaveFile(index);
        UIPauseMenuMaster.GetInstance().CloseAllMenus();
        UIPauseMenuMaster.Instance?.timeofDayContainer.parent.gameObject.SetActive(true);
    }

    protected IEnumerator EnableAfterDelay(float delay)
    {
        disabled = true;
        yield return new WaitForSeconds(delay);
        disabled = false;
    }
}
