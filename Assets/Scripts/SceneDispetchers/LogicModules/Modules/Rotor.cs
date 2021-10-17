using UnityEngine;
using System.Collections;

/// <summary>
/// ����������� ������ ����� ����� �����������
/// </summary>
[HelpURL("https://docs.google.com/document/d/142HK72fGCNHiZQ9tjFPH0bYUkP3qN4SmC-kfPqA5aSU/edit?usp=sharing")]
public class Rotor : LogicModule
{
    [SerializeField, Tooltip("���������� ��������� �������� �� ���������� (������ ������ forward) � ��������")]
    private Vector3 rotVector = new Vector3(0, 90, 0);

    [SerializeField]
    [Tooltip("��� ��������:" +
    "Once - ���� ��������=���� ��������. " +
    "loop - ����� ������ ��������������� �� �����, �� ��������������� � ��������� ���������. " +
    "Ping-Pong - �������� ����-�������.")]
    private LoopType loopType;

    [SerializeField, Tooltip("������� ����� ������� �������� (�)"), Min(0)]
    private float duration = 1;

    [SerializeField, Tooltip("������ ��������� ��������")]
    private AnimationCurve accelCurve;

    private Quaternion localStart;
    private Quaternion localTarget;
    private bool activate = false;
    private float time = 0f;
    private float angle = 0f;
    private float direction = 1f;

    private void Awake()
    {
        localStart = transform.localRotation;
        localTarget = localStart * Quaternion.Euler(rotVector);
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
            PerformRotation(angle);
        }
    }

    /// <summary>
    /// ������� ������ � �������� ���������
    /// </summary>
    [ContextMenu("������� � �������� ���������")]
    public override void ReturnToDefaultState()
    {
        StartCoroutine(ReturnToDefaultStateCoroutine());
    }

    /// <summary>
    /// ������� ������ � ������
    /// </summary>
    [ContextMenu("������� ������ � ������")]
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

    private void PerformRotation(float position)
    {
        var curveAngle = accelCurve.Evaluate(position);
        var ang = Quaternion.Lerp(localStart, localTarget, curveAngle);
        transform.localRotation = ang;
    }

    void LoopPingPong()
    {
        angle = Mathf.PingPong(time, 1f);
    }

    void LoopRepeat()
    {
        angle = Mathf.Repeat(time, 1f);
    }

    void LoopOnce()
    {
        angle = Mathf.Clamp01(time);
        if (angle <= 0 || angle >= 1)
        {
            time = angle;
            activate = false;
            direction *= -1;
        }
    }

    private IEnumerator ReturnToDefaultStateCoroutine()
    {
        activate = false;
        direction = -1;
        while (time > 1)
        {
            time--;
        }
        while (time >= 0)
        {
            time += direction * Time.deltaTime / duration;
            angle = Mathf.Clamp01(time);
            PerformRotation(angle);
            yield return null;
        }
        angle = time = 0;
        direction = 1;
        activate = false;
    }

    private void OnDrawGizmos()
    {
        if(debug)
        {
            Gizmos.color = Color.cyan;
            Quaternion Target = transform.localRotation * Quaternion.Euler(rotVector);
            Vector3 start, end;
            start = transform.position + transform.forward * 3;
            float t = 0;
            Gizmos.DrawLine(transform.position, start);
            Gizmos.DrawSphere(start, 0.4f);
            while (t < 1)
            {
                t += Time.fixedDeltaTime;
                Quaternion currentTarget = Quaternion.Lerp(transform.localRotation, Target, t);
                end = transform.position + (transform.localRotation * currentTarget * Vector3.forward * 3);
                Gizmos.DrawLine(start, end);
                Gizmos.color = Color.green;
                Gizmos.DrawLine(end, end + transform.localRotation * currentTarget * Vector3.up * 1);
                start = end;
                Gizmos.color = Color.cyan;
            }
            end = transform.position + transform.localRotation * Target * Vector3.forward * 3;
            Gizmos.DrawWireSphere(end, 0.4f);
        }
    }
}
