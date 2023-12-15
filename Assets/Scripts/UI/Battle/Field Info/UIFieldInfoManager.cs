using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UIFieldInfoManager : MonoBehaviour
{
    public static UIFieldInfoManager Instance;

    public UIFieldStatus fieldStatusPrefab;
    public Transform fieldStatusPanel;
    public float updateStatusDelay = 1f;

    protected List<UIFieldStatus> _statusList = new List<UIFieldStatus>();

    private void Awake()
    {
        Instance = this;
    }

    public float LoadFieldStatus(List<StatusField> statusList)
    {
        List<UIFieldStatus> spawnedStatus = new List<UIFieldStatus>();
        List<StatusField> newStatus = new List<StatusField>();
        foreach (StatusField status in statusList)
        {
            UIFieldStatus fieldInstance = null;
            // Check if status already exists
            foreach(UIFieldStatus uIFieldStatus in _statusList)
            {
                if (uIFieldStatus.GetStatusField() == status)
                {
                    fieldInstance = uIFieldStatus;
                    spawnedStatus.Add(fieldInstance);
                    break;
                }
            }
            // If it doesnt we instanciate
            if (!fieldInstance)
            {
                newStatus.Add(status);
            }

        }
        // Clear all field status that dont exist anymore
        foreach(UIFieldStatus uiStatus in _statusList)
        {
            if (!spawnedStatus.Contains(uiStatus))
            {
                uiStatus.Close();
            }
        }
        _statusList = spawnedStatus;
        OpenDelay(newStatus);
        return updateStatusDelay;
    }

    public async void OpenDelay(List<StatusField> statusField)
    {
        await Task.Delay(500);
        foreach(StatusField status in statusField)
        {

            UIFieldStatus uiFieldInstance = Instantiate(fieldStatusPrefab, fieldStatusPanel).Load(status);
            uiFieldInstance.Open();
            _statusList.Add(uiFieldInstance);
        }
    }

}
