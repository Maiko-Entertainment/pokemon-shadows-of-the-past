using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class UISettingsViewer : MonoBehaviour
{
    public Slider musicVolumeSlider;
    public Slider soundVolumeSlider;
    public Toggle expShareToggle;

    public AudioClip selectSound;
    public AudioClip turnOnSound;
    public AudioClip turnOffSound;

    private void Start()
    {
        if (musicVolumeSlider) musicVolumeSlider.value = AudioMaster.GetInstance().musicVolume;
        if (soundVolumeSlider) soundVolumeSlider.value = AudioMaster.GetInstance().soundEffectVolume;
        if (expShareToggle)
        {
            bool isExpShareOn = BattleMaster.GetInstance().isExpShareOn;
            expShareToggle.isOn = isExpShareOn;
        }
    }

    public void OnUpdateSound(float volume)
    {
        AudioMaster.GetInstance().SetSoundEffectVolume(volume);
        AudioMaster.GetInstance()?.PlaySfx(selectSound);
    }
    public void OnUpdateMusic(float volume)
    {
        AudioMaster.GetInstance().SetMusicVolume(volume);
        AudioMaster.GetInstance()?.PlaySfx(selectSound);
    }
    public void OnUpdateExpShare(bool isOn)
    {
        BattleMaster.GetInstance().isExpShareOn = isOn;
        if (isOn)
        {
            AudioMaster.GetInstance()?.PlaySfx(turnOnSound);
        }
        else
        {
            AudioMaster.GetInstance()?.PlaySfx(turnOffSound);
        }
    }
    public void OnCloseGame()
    {
        UIPauseMenuMaster.GetInstance().OpenConfirmMenu(
                    "Close Game?",
                    "Just making sure you didn't hit this by accident lol.",
                    HandleConfirmCloseGame
                    );
    }

    private async void HandleConfirmCloseGame()
    {
        float time = TransitionMaster.Instance.TransitionCloseGame();
        await Task.Delay((int)(time * 1000));
        Application.Quit();
    }
}
