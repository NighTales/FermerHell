using System;
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
    [Header("0 - влево, 1 - вправо")]
    private List<GameObject> sprintMarkers;

    [SerializeField] private GameObject finalPanel;
    [SerializeField] private Text finalPanelMessageText;
    [SerializeField] private Text finalPanelScoreText;

    private bool opportunityToShowSettings = true;
    private int score = 0;
    //private int currentStatEffect = 0;
    [SerializeField]
    private PlayerBonusStat PlayerStatInstant = PlayerBonusStat.Instant;


    [SerializeField] private Sprite SpeedBonus;
    [SerializeField] private Sprite JumpBonus;
    [SerializeField] private Sprite DOTBonus;
    [SerializeField] private Sprite ResistBonus;
    [SerializeField] private Sprite MagnetBonus;
    [SerializeField] private Sprite SpeedDebuff;
    [SerializeField] private Sprite JumpDebuff;
    [SerializeField] private Sprite DOTDebuff;
    [SerializeField] private Sprite ResistDebuff;
    [SerializeField] private Sprite MagnetDebuff;

    private Dictionary<BonusType, Tuple<Sprite, Image>> bonusPack = new Dictionary<BonusType, Tuple<Sprite, Image>>();
    private Dictionary<BonusType, Tuple<Sprite, Image>> debuffPack = new Dictionary<BonusType, Tuple<Sprite, Image>>();

    public Skill.RGBCharge rGBCharge = new Skill.RGBCharge(0, 0, 0);
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //MenuPanelToggle_ButtonClick();
        //DontDestroyOnLoad(gameObject);
        OnChangeScore();
        ReturnHitMarker();
        DisableAllSprintEffects();
        ClearAllStatEffect();
        ClearColorPower();
        if (pausePanel.activeSelf)
        {
            MenuPanelToggle_ButtonClick();
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;

        bonusPack.Add(BonusType.Speed, new Tuple<Sprite, Image>(SpeedBonus, statEffect[0]));
        bonusPack.Add(BonusType.Resist, new Tuple<Sprite, Image>(ResistBonus, statEffect[1]));
        bonusPack.Add(BonusType.Magnet, new Tuple<Sprite, Image>(MagnetBonus, statEffect[2]));
        bonusPack.Add(BonusType.Jump, new Tuple<Sprite, Image>(JumpBonus, statEffect[3]));
        bonusPack.Add(BonusType.DOT, new Tuple<Sprite, Image>(DOTBonus, statEffect[4]));

        debuffPack.Add(BonusType.Speed, new Tuple<Sprite, Image>(SpeedDebuff, statEffect[5]));
        debuffPack.Add(BonusType.Resist, new Tuple<Sprite, Image>(ResistDebuff, statEffect[6]));
        debuffPack.Add(BonusType.Magnet, new Tuple<Sprite, Image>(MagnetDebuff, statEffect[7]));
        debuffPack.Add(BonusType.Jump, new Tuple<Sprite, Image>(JumpDebuff, statEffect[8]));
        debuffPack.Add(BonusType.DOT, new Tuple<Sprite, Image>(DOTDebuff, statEffect[9]));
    }
    List<Action<int>> actions;
    private void Awake()
    {
        actions = new List<Action<int>>();

        actions.Add(new Action<int>(c => OnSetBonus(BonusType.Jump, c)));
        actions.Add(new Action<int>(c => OnSetBonus(BonusType.Speed, c)));
        actions.Add(new Action<int>(c => OnSetBonus(BonusType.DOT, c)));
        actions.Add(new Action<int>(c => OnSetBonus(BonusType.Resist, c)));
        actions.Add(new Action<int>(c => OnSetBonus(BonusType.Magnet, c)));

        actions.Add(new Action<int>(c => OnSetDebuff(BonusType.Jump, c)));
        actions.Add(new Action<int>(c => OnSetDebuff(BonusType.Speed, c)));
        actions.Add(new Action<int>(c => OnSetDebuff(BonusType.DOT, c)));
        actions.Add(new Action<int>(c => OnSetDebuff(BonusType.Resist, c)));
        actions.Add(new Action<int>(c => OnSetDebuff(BonusType.Magnet, c)));

        Messenger.AddListener(GameEvent.HIT, OnHit);
        Messenger<int>.AddListener(GameEvent.CHANGE_SPRINT_COUNT, OnChangeSprint);
        Messenger<int>.AddListener(GameEvent.ENEMY_HIT, OnChangeScore);
        Messenger<float>.AddListener(GameEvent.CHANGE_HEALTH, OnChangeHealth);
        Messenger<Vector3>.AddListener(GameEvent.START_SPRINT, EnebleSprintEffect);
        Messenger.AddListener(GameEvent.STOP_SPRINT, DisableAllSprintEffects);

        Messenger.AddListener(GameEvent.CLEAR_COLORS, ClearColorPower);
        Messenger.AddListener(GameEvent.ADD_R_CHARGE, AddRColor);
        Messenger.AddListener(GameEvent.ADD_G_CHARGE, AddGColor);
        Messenger.AddListener(GameEvent.ADD_B_CHARGE, AddBColor);
        Messenger.AddListener(GameEvent.READY_TO_MAGIC, ReadyMagic);

        Messenger<int>.AddListener(GameEvent.Set_BONUS_JUMP, actions[0]);
        Messenger<int>.AddListener(GameEvent.Set_BONUS_SPEED, actions[1]);
        Messenger<int>.AddListener(GameEvent.Set_BONUS_DOT, actions[2]);
        Messenger<int>.AddListener(GameEvent.Set_BONUS_RESIST, actions[3]);
        Messenger<int>.AddListener(GameEvent.Set_BONUS_MAGNET, actions[4]);

        Messenger<int>.AddListener(GameEvent.Set_DEBUFF_JUMP, actions[5]);
        Messenger<int>.AddListener(GameEvent.Set_DEBUFF_SPEED, actions[6]);
        Messenger<int>.AddListener(GameEvent.Set_DEBUFF_DOT, actions[7]);
        Messenger<int>.AddListener(GameEvent.Set_DEBUFF_RESIST, actions[8]);
        Messenger<int>.AddListener(GameEvent.Set_DEBUFF_MAGNET, actions[9]);
        //   Messenger.AddListener(GameEvent.EXIT_LEVEL, OnDestroy);
    }
    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.HIT, OnHit);
        Messenger<int>.RemoveListener(GameEvent.CHANGE_SPRINT_COUNT, OnChangeSprint);
        Messenger<int>.RemoveListener(GameEvent.ENEMY_HIT, OnChangeScore);
        Messenger<float>.RemoveListener(GameEvent.CHANGE_HEALTH, OnChangeHealth);
        Messenger<Vector3>.RemoveListener(GameEvent.START_SPRINT, EnebleSprintEffect);
        Messenger.RemoveListener(GameEvent.STOP_SPRINT, DisableAllSprintEffects);

        Messenger.RemoveListener(GameEvent.CLEAR_COLORS, ClearColorPower);
        Messenger.RemoveListener(GameEvent.ADD_R_CHARGE, AddRColor);
        Messenger.RemoveListener(GameEvent.ADD_G_CHARGE, AddGColor);
        Messenger.RemoveListener(GameEvent.ADD_B_CHARGE, AddBColor);
        Messenger.RemoveListener(GameEvent.READY_TO_MAGIC, ReadyMagic);

        Messenger<int>.RemoveListener(GameEvent.Set_BONUS_JUMP, actions[0]);
        Messenger<int>.RemoveListener(GameEvent.Set_BONUS_SPEED, actions[1]);
        Messenger<int>.RemoveListener(GameEvent.Set_BONUS_DOT, actions[2]);
        Messenger<int>.RemoveListener(GameEvent.Set_BONUS_RESIST, actions[3]);
        Messenger<int>.RemoveListener(GameEvent.Set_BONUS_MAGNET, actions[4]);

        Messenger<int>.RemoveListener(GameEvent.Set_DEBUFF_JUMP, actions[5]);
        Messenger<int>.RemoveListener(GameEvent.Set_DEBUFF_SPEED, actions[6]);
        Messenger<int>.RemoveListener(GameEvent.Set_DEBUFF_DOT, actions[7]);
        Messenger<int>.RemoveListener(GameEvent.Set_DEBUFF_RESIST, actions[8]);
        Messenger<int>.RemoveListener(GameEvent.Set_DEBUFF_MAGNET, actions[9]);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && opportunityToShowSettings)
            MenuPanelToggle_ButtonClick();
    }
    private void ClearAllStatEffect()
    {
        foreach (var item in statEffect)
        {
            item.gameObject.SetActive(false);
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
        rGBCharge.ClearColors(true);
    }
    private void AddColor()
    {
        clearHint.SetActive(true);
        if (rGBCharge.ColorCount >= 3)
        {
            applyHint.SetActive(true);
        }
    }
    private void AddRColor()
    {
        colorSkillPower[rGBCharge.ColorCount].enabled = true;
        colorSkillPower[rGBCharge.ColorCount].color = Color.red;
        rGBCharge.red++;

        AddColor();
    }
    private void AddGColor()
    {
        colorSkillPower[rGBCharge.ColorCount].enabled = true;
        colorSkillPower[rGBCharge.ColorCount].color = Color.green;
        rGBCharge.green++;

        AddColor();
    }

    private void AddBColor()
    {
        colorSkillPower[rGBCharge.ColorCount].enabled = true;
        colorSkillPower[rGBCharge.ColorCount].color = Color.blue;
        rGBCharge.blue++;

        AddColor();
    }

    private void ReadyMagic()
    {
        skillImage.enabled = true;
        skillImage.sprite = PlayerStatInstant.ActiveSlillSprite;
        useSkillHint.SetActive(true);
        Debug.Log("UI ReadyMagic");
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
        score += value * PlayerStatInstant.scoreMultiplicator;
        scoreLabel.text = "Счёт: " + score;
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

    public void ShowFinalPanel(string message)
    {
        Messenger<bool>.Broadcast(GameEvent.PAUSE, true);
        opportunityToShowSettings = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        finalPanel.SetActive(true);
        finalPanelMessageText.text = message;
        finalPanelScoreText.text = "Ваш счёт\n" + score;
    }

    public void Exit_ButtonClick()
    {
        Application.Quit();
    }

    private void OnSetBonus(BonusType bonusType, int value)
    {
        if (value > 0)
        {
            AddStatEffect(bonusType);
            Debug.Log("OnSetBonus " + bonusType + " AddStatEffect");
        }
        else
        {
            RemoveStatEffect(bonusType);
            Debug.Log("OnSetBonus " + bonusType + " RemoveStatEffect");
        }
    }

    private void OnSetDebuff(BonusType bonusType, int value)
    {
        if (value > 0)
        {
            AddStatDebuff(bonusType);
            Debug.Log("OnSetDebuff " + bonusType + "  AddStatDebuff");
        }
        else
        {
            RemoveStatDebuff(bonusType);
            Debug.Log("OnSetDebuff " + bonusType + "  RemoveStatDebuff");
        }
    }
    private void AddStatEffect(BonusType bonusType)
    {
        bonusPack[bonusType].Item2.sprite = bonusPack[bonusType].Item1;
        bonusPack[bonusType].Item2.gameObject.SetActive(true);
    }

    private void RemoveStatEffect(BonusType bonusType)
    {
        bonusPack[bonusType].Item2.gameObject.SetActive(false);
    }

    private void AddStatDebuff(BonusType bonusType)
    {
        debuffPack[bonusType].Item2.sprite = debuffPack[bonusType].Item1;
        debuffPack[bonusType].Item2.gameObject.SetActive(true);
    }

    private void RemoveStatDebuff(BonusType bonusType)
    {
        debuffPack[bonusType].Item2.gameObject.SetActive(false);
    }
}
