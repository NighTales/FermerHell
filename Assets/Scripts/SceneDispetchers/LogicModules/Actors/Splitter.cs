using UnityEngine;

/// <summary>
/// ������� ���� ������ �������� ��������� ������� (������������ ��� ��������)
/// </summary>
[HelpURL("https://docs.google.com/document/d/1elo1YsFfbbhLMpHGXa8g3z7TVpwf1LCiTqJkcKCFaak/edit?usp=sharing")]
public class Splitter : LogicActor
{
    [SerializeField, Tooltip("��������� �����")] private bool UseOnStart = false;

    private void Start()
    {
        if (UseOnStart)
        {
            ActivateModule();
        }
    }

    public override void ReturnToDefaultState()
    {
        used = false;
        AllNextModulesToDefaultState();
    }

    public override void ActivateModule()
    {
        used = true;
        ActivateAllNextModules();
    }

    private void OnDrawGizmos()
    {
        if (debug)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 0.3f);
            for (int i = 0; i < nextModules.Count; i++)
            {
                if (nextModules[i] != null)
                {
                    Gizmos.DrawLine(transform.position, nextModules[i].transform.position);
                }
                else
                {
                    Debug.LogWarning("������� " + i + " ����� null. ��������, ���� ������� ������. �������� :" + gameObject.name);
                }
            }
        }
    }
}
