using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuManager : MonoBehaviour
{
    public UIDocument uiDocument;

    [Header("Volume Icons")]
    public Sprite volumeOnSprite;
    public Sprite volumeOffSprite;

    private Button volumeButton;
    private Button playButton;

    void OnEnable()
    {
        var root = uiDocument.rootVisualElement;
        volumeButton = root.Q<Button>("Volume");
        playButton = root.Q<Button>("Play");

        if (volumeButton != null)
        {
            volumeButton.clicked += ToggleMute;
            UpdateVolumeUI();
        }

        if (playButton != null)
        {
            playButton.clicked += () => SceneManager.LoadScene("Minigame");
        }
    }

    void ToggleMute()
    {
        AudioListener.volume = AudioListener.volume > 0 ? 0f : 1f;
        UpdateVolumeUI();
    }

    void UpdateVolumeUI()
    {
        if (volumeButton == null) return;
        volumeButton.style.backgroundImage = new StyleBackground(
            AudioListener.volume > 0 ? volumeOnSprite : volumeOffSprite
        );
    }
}