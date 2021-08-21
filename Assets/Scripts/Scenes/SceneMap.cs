using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMap : MonoBehaviour
{
    public int sceneId = 0;
    public string title;
    public AudioClip mapMusic;
    public TransitionBase titleCard;

    public void HandleEntrance()
    {
        AudioMaster.GetInstance().PlayMusic(mapMusic);
        if (titleCard != null)
            WorldMapMaster.GetInstance().CreateTitleCard(titleCard);
    }

    public void HandleReturn()
    {
        AudioMaster.GetInstance().PlayMusic(mapMusic);
    }
}
