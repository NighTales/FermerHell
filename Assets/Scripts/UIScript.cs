using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIScript : MonoBehaviour
{
    [SerializeField] private GameObject hitMarker;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject taskPanel;
    [SerializeField] private List<GameObject> sprintCells;
    [SerializeField] private GameObject dashHint;
    [SerializeField] private Text scoreLabel;
    [SerializeField] private List<Image> statEffect;
    [SerializeField] private Image skillImage;
    [SerializeField] private List<Image> colorSkillPower;
    [SerializeField] private GameObject clearHint;
    [SerializeField] private GameObject applyHint;
    [SerializeField] private GameObject useSkillHint;
    [SerializeField] private Slider healthSlider;

    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider soundsVolumeSlider;
    [SerializeField] private Slider voiceVolumeSlider;

    [SerializeField]
    [Tooltip("0 - влево, 1 - вправо")]
    private List<GameObject> sprintMarkers;

    private bool opportunityToShowSettings = true;
    private int score = 0;
    private int currentStatEffect = 0;

    // Start is called before the first frame update
    void Start()
    {
        //MenuPanelToggle_ButtonClick();
        DontDestroyOnLoad(gameObject);
        OnChangeScore();
        ReturnHitMarker();
        DisableAllSprintEffects();
        ClearAllStatEffect();
        ClearColorPower();

        Messenger.AddListener(GameEvent.HIT, OnHit);
        Messenger<int>.AddListener(GameEvent.CHANGE_SPRINT_COUNT, OnChangeSprint);
        Messenger<int>.AddListener(GameEvent.ENEMY_HIT, OnChangeScore);
        Messenger<float>.AddListener(GameEvent.CHANGE_HEALTH, OnChangeHealth);
        Messenger<Vector3>.AddListener(GameEvent.START_SPRINT, EnebleSprintEffect);
        Messenger.AddListener(GameEvent.STOP_SPRINT, DisableAllSprintEffects);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && opportunityToShowSettings)
            MenuPanelToggle_ButtonClick();
    }
    private void ClearAllStatEffect()
    {
        foreach(var item in statEffect)
        {
            item.enabled = false;
        }
    }
    private void ClearColorPower()
    {
        foreach (var item in colorSkillPower)
        {
            item.enabled = false;
        }
        skillImage.enabled = false;
        clearHint.SetActive(false);
        applyHint.SetActive(false);
        useSkillHint.SetActive(false);
    }
    public void RestartScene_ButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void OnHit()
    {
        hitMarker.SetActive(true);
        Invoke(nameof(ReturnHitMarker), 0.5f);
    }
    private void ReturnHitMarker()
    {
        hitMarker.SetActive(false);
    }
    private void OnChangeScore(int value = 0)
    {
        score += value * PlayerBonusStat.scoreMultiplicator;
        scoreLabel.text = "—чЄт: " + score;
    }
    private void OnChangeHealth(float value)
    {
        healthSlider.value = value;
    }
    public void ViewTasks_ButtonClick()
    {
        taskPanel.SetActive(!taskPanel.activeSelf);
    }
    private void EnebleSprintEffect(Vector3 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
        {
            if (direction.x > 0)
            {
                sprintMarkers[1].SetActive(true);
            }
            else
            {
                sprintMarkers[0].SetActive(true);
            }
        }
        else
        {
            sprintMarkers[0].SetActive(true);
            sprintMarkers[1].SetActive(true);
        }
    }
    private void DisableAllSprintEffects()
    {
        foreach (var item in sprintMarkers)
        {
            item.SetActive(false);
        }
    }
    public void MenuPanelToggle_ButtonClick()
    {
        bool inMenu = !pausePanel.activeSelf;
        pausePanel.SetActive(inMenu);
        Cursor.visible = inMenu;
        Cursor.lockState = inMenu ? CursorLockMode.None : CursorLockMode.Locked;
        Time.timeScale = inMenu ? 0 : 1;
        if (taskPanel.activeSelf) ViewTasks_ButtonClick();
        Messenger<bool>.Broadcast(GameEvent.PAUSE, inMenu);
    }
    public void OnMusicValueChanged(Slider slider)
    {
        PlayerPrefs.SetFloat("Music", slider.value);
        Messenger<float>.Broadcast(GameEvent.MUSIC_CHANGED, slider.value);
    }
    public void OnSoundsValueChanged(Slider slider)
    {
        PlayerPrefs.SetFloat("Sounds", slider.value);
        Messenger<float>.Broadcast(GameEvent.SOUNDS_CHANGED, slider.value);
    }
    public void OnVoiceValueChanged(Slider slider)
    {
        PlayerPrefs.SetFloat("Voices", slider.value);
        Messenger<float>.Broadcast(GameEvent.VOICE_CHANGED, slider.value);
    }
    private void OnChangeSprint(int value)
    {
        for (int i = 0; i < sprintCells.Count; i++)
        {
            sprintCells[i].SetActive(i < value);
        }
        dashHint.SetActive(value > 0);
    }

    public void Exit_ButtonClick()
    {
        Application.Quit();
    }
}
