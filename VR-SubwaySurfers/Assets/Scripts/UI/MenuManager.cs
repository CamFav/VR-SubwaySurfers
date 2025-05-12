using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manages the main menu, options, and leaderboard UI.
/// </summary>
public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject leaderboardPanel;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Locomotion UI")]

    [SerializeField] private Button buttonManette;
    [SerializeField] private Button buttonPhysique;
    [SerializeField] private Image manetteImage;
    [SerializeField] private Image physiqueImage;
    [SerializeField] private Color activeColor = new Color(0.3f, 0.7f, 1f); // bleu clair
    [SerializeField] private Color inactiveColor = Color.white;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        AudioManager.Instance.PlayMusic(AudioManager.Instance.menuMusic);
    }

    public void ShowOptions()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);

        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        sfxSlider.value = PlayerPrefs.GetFloat("SfxVolume", 0.75f);

        UpdateLocomotionText();
    }

    public void ShowMainMenuUI()
    {
        optionsPanel.SetActive(false);
        leaderboardPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void OnMusicSliderChanged(float value)
    {
        AudioManager.Instance.SetMusicVolume(value);
    }

    public void OnSfxSliderChanged(float value)
    {
        AudioManager.Instance.SetSfxVolume(value);
    }

    public void SetLocomotionMode(int mode)
    {
        PlayerPrefs.SetInt("LocomotionMode", mode);
        PlayerPrefs.Save();

        UpdateLocomotionText();
    }

    private void UpdateLocomotionText()
    {
        int mode = PlayerPrefs.GetInt("LocomotionMode", 0);

        if (manetteImage != null && physiqueImage != null)
        {
            bool isManette = mode == 0;
            manetteImage.color = isManette ? activeColor : inactiveColor;
            physiqueImage.color = isManette ? inactiveColor : activeColor;
        }
    }
}
