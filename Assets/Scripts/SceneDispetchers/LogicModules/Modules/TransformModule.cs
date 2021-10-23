using UnityEngine;
using System.Collections;

public enum LoopType
{
    Once,
    PingPong,
    Repeat
}

/// <summary>
/// ������ ������ ������������ ��� �������� ������� ����� ����� �������
/// </summary>
[HelpURL("https://docs.google.com/document/d/1YA0fSq6q6Nbarg9PwMFs5GzNYFtaNGaD8IXdj8drbr0/edit?usp=sharing")]
public class TransformModule : LogicModule
{
    [SerializeField, Tooltip("�������� ������� ����� ������������ ���������")]
    private Vector3 target = Vector3.forward;

    [SerializeField]
    [Tooltip("��� �����������:" +
        "Once - ���� ��������=���� �����������. " +
        "loop - ����� ������ ������� �� ������, �� ��������������� �� ������. " +
        "Ping-Pong - �������� ����-�������.")]
    private LoopType loopType;

    [SerializeField, Tooltip("������� ����� ������� ����������� (�)"), Min(0)]
    private float duration = 1;

    [SerializeField, Tooltip("������ ��������� ��������")]
    private AnimationCurve accelCurve;

    private Vector3 localStart;
    private Vector3 localTarget;
    private bool activate = false;
    private float time = 0f;
    private float position = 0f;
    private float direction = 1f;

    private void Awake()
    {
        localStart = transform.localPosition;
        localTarget = transform.position +
                            transform.forward * target.z +
                            transform.right * target.x +
                            transform.up * target.y;
    }

    /// <summary>
    /// Once - ������ ��������
    /// Loop,PingPong - ������/���������� ��������
    /// </summary>
    [ContextMenu("������������ ������")]
    public override void ActivateModule()
    {
        if (loopType == LoopType.Once)
        {
            activate = true;
        }
        else
        {
            activate = !activate;
        }
    }

    /// <summary>
    /// ������� � ����������� �������
    /// </summary>
    [ContextMenu("������� � �������� ���������")]
    public override void ReturnToDefaultState()
    {
        StartCoroutine(ReturnToDefaultStateCoroutine());
    }

    private void Update()
    {
        if (activate)
        {
            time = time + (direction * Time.deltaTime / duration);
            switch (loopType)
            {
                case LoopType.Once:
                    LoopOnce();
                    break;
                case LoopType.PingPong:
                    LoopPingPong();
                    break;
                case LoopType.Repeat:
                    LoopRepeat();
                    break;
            }
            PerformTransform(position);
        }
    }

    private void PerformTransform(float position)
    {
        var curvePosition = accelCurve.Evaluate(position);
        var pos = Vector3.Lerp(localStart, localTarget, curvePosition);
        transform.localPosition = pos;
    }

    void LoopPingPong()
    {
        position = Mathf.PingPong(time, 1f);
    }

    void LoopRepeat()
    {
        position = Mathf.Repeat(time, 1f);
    }

    void LoopOnce()
    {
        position = Mathf.Clamp01(time);
        if (position <= 0 || position >= 1)
        {
            time = position;
            activate = false;
            direction *= -1;
        }
    }

    private void OnDrawGizmos()
    {
        if(debug)
        {
            Gizmos.color = Color.cyan;
            Vector3 targetPos = transform.position + transform.forward * target.z + transform.right * target.x + transform.up * target.y;
            Gizmos.DrawWireCube(targetPos, transform.localScale);
            Gizmos.DrawLine(transform.position, targetPos);
        }
    }

    private IEnumerator ReturnToDefaultStateCoroutine()
    {
        activate = false;
        direction = -1;
        while(time > 1)
        {
            time--;
        }
        while(time >= 0)
        {
            time += direction * Time.deltaTime / duration;
            position = Mathf.Clamp01(time);
            PerformTransform(position);
            yield return null;
        }
        position = time = 0;
        direction = 1;
        activate = false;
    }
}
