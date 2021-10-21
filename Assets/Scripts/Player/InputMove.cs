using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class InputMove : MonoBehaviour, IDialogueActor
{
    [SerializeField, Range(1, 100)] private float speed = 5f;
    [SerializeField, Range(1, 50)] private float jumpForce = 15.0f;
    [SerializeField, Range(-40, -1)] private float terminalVelocity = -10.0f;
    [SerializeField, Tooltip("Сила притяжения на земле"), Range(-2, 0)] private float minFall = -1.5f;
    [SerializeField, Range(0.1f, 20)] private float gravity = 9.8f;
    [SerializeField, Range(1, 5), Tooltip("Ускорение при рывке")] private float sprintMultiplicator = 3;
    [SerializeField, Range(0.1f, 1), Tooltip("Время рывка")] private float sprintTime = 0.3f;
    [SerializeField, Range(0.1f, 5f), Tooltip("Время перезарядки рывка")] private float sprintReloadTime = 1;

    private CharacterController charController;
    private Vector3 moveVector;
    private Vector3 horSpeed;
    [SerializeField] private float sprintMultiplicatorBufer;
    [SerializeField] private float currentSprintReloadTime;
    [SerializeField] private float vertSpeed;
    [SerializeField] private int sprintCount;
    [SerializeField] private int sprintCountBuffer;
    [SerializeField] private bool inMenu;
    [SerializeField] private bool startup;
    [SerializeField] private bool fall;
    [SerializeField] private bool inDialogue;
    [SerializeField] private float fallTimer;

    float deltaX;
    float deltaZ;
    bool inputShift;
    bool inputSpace;
    float timeDelta;
    void Awake()
    {
        PlayerBonusStat.Init();
        Messenger<bool>.AddListener(GameEvent.PAUSE, OnPause);
        Messenger<int>.AddListener(GameEvent.TAKE_BONUS_JUMP, OnTakeBonusJump);
        Messenger<int>.AddListener(GameEvent.TAKE_BONUS_SPEED, OnTakeBonusSpeed);
        //    Messenger.AddListener(GameEvent.EXIT_LEVEL, OnDestroy);
    }
    void OnDestroy()
    {
        Messenger<bool>.RemoveListener(GameEvent.PAUSE, OnPause);
        Messenger<int>.RemoveListener(GameEvent.TAKE_BONUS_JUMP, OnTakeBonusJump);
        Messenger<int>.RemoveListener(GameEvent.TAKE_BONUS_SPEED, OnTakeBonusSpeed);
        //   Messenger.AddListener(GameEvent.EXIT_LEVEL, OnDestroy);
    }

    private void Start()
    {
        vertSpeed = minFall;
        charController = GetComponent<CharacterController>();
        sprintMultiplicatorBufer = 1;
        fall = true;
    }
    void Update()
    {
        Input();
        if (!inMenu && !inDialogue)
        {
            Jump();
            PlayerSprint();
            PlayerMove();
        }
    }

    private void Input()
    {
        deltaX = UnityEngine.Input.GetAxis("Horizontal");
        deltaZ = UnityEngine.Input.GetAxis("Vertical");
        inputShift = UnityEngine.Input.GetKeyDown(KeyCode.LeftShift);
        inputSpace = UnityEngine.Input.GetKeyDown(KeyCode.Space);
        timeDelta = Time.deltaTime;
    }

    private void FixedUpdate()
    {
        
    }
    public void Setup(Transform targetPos)
    {
        startup = true;
        moveVector = Vector3.zero;
        transform.position = targetPos.position;
        transform.rotation = targetPos.rotation;
        Invoke("StopStartup", 0.5f);
    }

    private void OnTakeBonusJump(int value)
    {
        PlayerBonusStat.bonusPack[BonusType.Jump] = value;
    }
    private void OnTakeBonusSpeed(int value)
    {
        PlayerBonusStat.bonusPack[BonusType.Speed] = value;
    }

    private void OnTakeDebuffJump(int value)
    {
        PlayerBonusStat.debuffPack[BonusType.Jump] = value;
    }
    private void OnTakeDebuffSpeed(int value)
    {
        PlayerBonusStat.debuffPack[BonusType.Speed] = value;
    }
    private void Jump()
    {
        if (charController.isGrounded)
        {
            fallTimer = 0;
            fall = true;
            if (inputSpace)
            {
                vertSpeed = jumpForce;
            }
            else
            {
                vertSpeed = minFall;
            }
        }
        else
        {
            if (fall)
            {
                vertSpeed -= gravity * 5 / PlayerBonusStat.bonusPack[BonusType.Jump] * timeDelta/PlayerBonusStat.debuffPack[BonusType.Jump];
                if (vertSpeed < terminalVelocity)
                {
                    vertSpeed = terminalVelocity;
                }
            }
            else
            {
                fallTimer -= timeDelta;
                if (fallTimer <= 0)
                {
                    fallTimer = 0;
                    fall = true;
                }
                vertSpeed = 0;
            }
        }
    }
    private void PlayerMove()
    {
        if (!startup)
        {

            moveVector = new Vector3(deltaX, 0, deltaZ).normalized;//Ограничим движение по диагонали той же скоростью, что и движение параллельно осям
            moveVector = moveVector * speed * sprintMultiplicatorBufer * PlayerBonusStat.bonusPack[BonusType.Speed]/PlayerBonusStat.debuffPack[BonusType.Speed];
            //moveVector = Vector3.ClampMagnitude(moveVector, speed) * sprintMultiplicatorBufer * PlayerBonusStat.bonusPack[BonusType.Speed]; 
            horSpeed = moveVector;
            moveVector.y = vertSpeed;
            moveVector *= timeDelta;
            moveVector = transform.TransformDirection(moveVector); //Преобразуем вектор движения от локальных к глобальным координатам.
            //transform.position += moveVector;
            //charController.SimpleMove(moveVector*100);
            charController.Move(moveVector);
        }
    }
    private void PlayerSprint()
    {
        if (charController.isGrounded && sprintCountBuffer > 0)
        {
            sprintCount += sprintCountBuffer;
            sprintCountBuffer = 0;
            Messenger<int>.Broadcast(GameEvent.CHANGE_SPRINT_COUNT, sprintCount);
        }
        if (inputShift && horSpeed.magnitude != 0 && sprintMultiplicatorBufer == 1)
        {
            if (sprintCount > 0)
            {
                fall = false;
                fallTimer += sprintTime + 0.1f;
                sprintMultiplicatorBufer = sprintMultiplicator;
                Invoke("ReturnSprintOpportunity", sprintTime);
                Invoke("StopSprint", sprintTime + 0.1f);
                sprintCount--;
                currentSprintReloadTime = charController.isGrounded ? sprintReloadTime / 2f : sprintReloadTime;
                Messenger<int>.Broadcast(GameEvent.CHANGE_SPRINT_COUNT, sprintCount);
                Messenger.Broadcast(GameEvent.SPRINT_ACTION);
                Messenger<Vector3>.Broadcast(GameEvent.START_SPRINT, horSpeed);
            }
        }
        if (sprintCount < 3 && sprintCountBuffer < 3)
        {
            currentSprintReloadTime -= timeDelta;
            if (charController.isGrounded)
                currentSprintReloadTime -= timeDelta;
            if (currentSprintReloadTime <= 0)
            {
                sprintCountBuffer++;
                currentSprintReloadTime = sprintCountBuffer == 3 ? 0 : sprintReloadTime;
                sprintMultiplicatorBufer = 1;
            }
        }
    }

    private void OnPause(bool pause)
    {
        inMenu = pause;
    }
    public void SetDialogueState(bool inDialogueState)
    {
        inDialogue = inDialogueState;
    }
    #region Вызовы Invoke
    private void StopStartup() => startup = false;
    private void ReturnSprintOpportunity() => sprintMultiplicatorBufer = 1;
    private void StopSprint()
    {
        Messenger.Broadcast(GameEvent.STOP_SPRINT);
    }


    #endregion
}
