using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMagic : MonoBehaviour
{
    public Skill.RGBCharge rGBCharge = new Skill.RGBCharge(0, 0, 0);
    public GameObject camObject;
    [Range(100f, 500f)] public float maxDistance = 300;
    public LayerMask ignoreRaycast;
    public List<Skill> Skills = new List<Skill>(10);
    [SerializeField] private GameObject targetMark;
    [SerializeField] private AudioSource source;

    private Skill skill;
    private bool rightClickOn = false;
    private bool rightClickUp = false;
    private bool refresh = false;
    private bool apply = false;
    PlayerBonusStat playerStatInstant = PlayerBonusStat.Instant;
    private bool inMenu = false;
    private Vector3 dir;
    private bool isStarted = false;

    void Awake()
    {
        Messenger<bool>.AddListener(GameEvent.PAUSE, OnPause);
    }

    void OnDestroy()
    {
        Messenger<bool>.RemoveListener(GameEvent.PAUSE, OnPause);
    }
    private void OnPause(bool pause)
    {
        inMenu = pause;
    }
    // Use this for initialization
    void Start()
    {
        dir = transform.forward;
        targetMark.SetActive(false);
        CreateTargetMark();
    }

    private void CreateTargetMark()
    {
        //targetMark = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        targetMark = Instantiate(targetMark);
        targetMark.SetActive(false);
        //targetMark.AddComponent<NavMeshObstacle>();
        //targetMark.GetComponent<BoxCollider>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!inMenu)
            InputKeys();

    }
    private void LateUpdate()
    {
        if (!inMenu)
        {
            ApplyWeaponBuff();
            Refresh();
            UseMagic();
        }

    }
    private void InputKeys()
    {
        rightClickOn = Input.GetKey(KeyCode.Mouse1);
        rightClickUp = Input.GetKey(KeyCode.Mouse0);
        refresh = Input.GetKeyDown(KeyCode.Q);
        apply = Input.GetKeyDown(KeyCode.E);
    }
    private void ReadyMagic()
    {
        if (rGBCharge.ColorCount >= 3)
        {
            skill = Skills.Where(s => s.rGBCharge.Equals(rGBCharge)).First();
            if (skill != null)
            {
                playerStatInstant.ActiveSlillSprite = skill.Sprite1;
                Messenger.Broadcast(GameEvent.READY_TO_MAGIC);
                Debug.Log("ReadyMagic Broadcast");
                Debug.Log("R: " + rGBCharge.red + " G: " + rGBCharge.green + " B: " + rGBCharge.blue);
            }
            else
            {
                Debug.Log("ReadyMagic skill 404 NorFound:");
                Debug.Log("R: " + rGBCharge.red + " G: " + rGBCharge.green + " B: " + rGBCharge.blue);
            }
        }
    }

    public void ApplyWeaponBuff()
    {
        if (rGBCharge.ColorCount >= 3 && apply)
        {
            Messenger<int>.Broadcast(GameEvent.SET_R_BONUS, rGBCharge.red);
            Messenger<int>.Broadcast(GameEvent.SET_G_BONUS, rGBCharge.green);
            Messenger<int>.Broadcast(GameEvent.SET_B_BONUS, rGBCharge.blue);
            rGBCharge.ClearColors();
        }
    }
    public void Refresh()
    {
        if (rGBCharge.ColorCount > 0 && refresh)
        {
            rGBCharge.ClearColors();
        }
    }
    public void UseMagic()
    {
        if (rGBCharge.ColorCount >= 3 && (rightClickOn || targetMark.activeSelf))
        {
            Vector3 target = Vector3.zero;
            //isStarted = true;

            if (!skill.Self)
            {
                if (!targetMark.activeSelf)
                {
                    targetMark.transform.localScale = new Vector3(skill.Radius * 2, skill.Radius * 2, 0.25f);
                    //targetMark.gameObject.GetComponent<CapsuleCollider>
                    targetMark.SetActive(true);
                    Messenger<bool>.Broadcast(GameEvent.MAGIC_SHOOT, true);
                }

                if (Physics.Raycast(
                    camObject.transform.position,
                    camObject.transform.forward,
                    out RaycastHit hitInfo,
                    maxDistance,
                    ~ignoreRaycast))
                {
                    targetMark.transform.position = hitInfo.point + Vector3.up * 0.25f;

                    if (rightClickUp)
                    {
                        target = hitInfo.point;
                    }
                }
            }
            else
            {
                target = transform.position + Vector3.up * skill.upmyltiplier;
            }


            if (target != Vector3.zero)
            {
                if (skill.Radius > 0)
                {

                    if (skill.Self)
                    {
                        Instantiate(skill.SkillObject, target, skill.SkillObject.transform.rotation, this.transform).GetComponent<SkillLogic>().Init(skill.Radius, this.transform);


                    }
                    else
                    {
                        //skill.SkillObject.transform.localScale =new Vector3( skill.Radius,skill.Radius,skill.Radius);
                        Instantiate(skill.SkillObject, target, skill.SkillObject.transform.rotation).GetComponent<SkillLogic>().Init(skill.Radius, this.transform);

                    }

                    source.PlayOneShot(skill.sound);
                }
                else
                {
                    // if (gameObject.TryGetComponent(out PlayerBonusScript playerBonus))
                    //     foreach (var ef in skill.effects)
                    //     {
                    //
                    //         playerBonus.ActiveBuff(ef.bonusType, ef.power);
                    //     }
                }

                rGBCharge.ClearColors();

                targetMark.SetActive(false);
                StartCoroutine(Shoot());
            }
        }
    }

    private IEnumerator Shoot()
    {
        yield return new WaitForSeconds(1);
        Messenger<bool>.Broadcast(GameEvent.MAGIC_SHOOT, false);
    }
    public void AddColor(Color color)
    {
        if (rGBCharge.ColorCount < 3)
        {
            if (color == Color.red)
            {
                rGBCharge.red++;
                Messenger.Broadcast(GameEvent.ADD_R_CHARGE);
            }
            if (color == Color.green)
            {
                rGBCharge.green++;
                Messenger.Broadcast(GameEvent.ADD_G_CHARGE);
            }
            if (color == Color.blue)
            {
                rGBCharge.blue++;
                Messenger.Broadcast(GameEvent.ADD_B_CHARGE);
            }
            Debug.Log("PlayerMagic AddColor " + color);
            ReadyMagic();
        }
    }

}
[Serializable]
public class Skill
{
    public RGBCharge rGBCharge = new RGBCharge(0, 0, 0);
    public bool Self = false;
    // public GameObject ParticleEffect;
    [Range(0f, 10f)] public float Radius = 5f;
    //[Range(1f, 60f)] public float TimeSec = 20f;
    //[Range(0, 500)] public int Damage = 20;
    public GameObject SkillObject;
    //public List<Effect> effects = new List<Effect>();
    public Sprite Sprite1;
    public float upmyltiplier;

    public AudioClip sound;

    [Serializable]
    public class Effect
    {
        public BonusType bonusType = BonusType.DOT;
        [Range(1, 50)] public int power = 5;
    }
    [Serializable]
    public struct RGBCharge
    {
        [Range(0, 3)] public int red;
        [Range(0, 3)] public int green;
        [Range(0, 3)] public int blue;

        public int ColorCount => red + green + blue;

        public RGBCharge(int red, int green, int blue)
        {
            this.red = red;
            this.green = green;
            this.blue = blue;
        }
        public void ClearColors(bool ui = false)
        {
            red = 0;
            green = 0;
            blue = 0;
            if (!ui)
                Messenger.Broadcast(GameEvent.CLEAR_COLORS);
        }

        public bool Equals(RGBCharge charge)
        {
            return red == charge.red && green == charge.green && blue == charge.blue;
        }
    }
}