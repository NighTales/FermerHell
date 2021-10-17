using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class ReplicDispether : MonoBehaviour
{
    [SerializeField] private GameObject replicPanel;
    [SerializeField] private Text replicText;
    [SerializeField] private Image speackerImage;
    [SerializeField] private Image skipImage;

    [SerializeField] private InputMove inputMove;
    [SerializeField] private MouseLock mouseLock;

    private List<ReplicItem> replicas;
    private AudioSource source;
    private ReplicItem bufer;
    private bool opportunityToSkip;

    private void Awake()
    {
        Setup();
    }


    public void Setup()
    {
        replicas = new List<ReplicItem>();
        source = GetComponent<AudioSource>();
        replicPanel.SetActive(false);
        opportunityToSkip = false;
    }

    private void Update()
    {
        if(opportunityToSkip && Input.GetKeyDown(KeyCode.Space))
        {
            StopAllCoroutines();
            StartCoroutine(CheckReplicas(0));
        }
    }

    public void ClearList()
    {
        replicas.Clear();
    }
    public void AddInList(List<ReplicItem> items)
    {
        replicas.AddRange(items);
        if(bufer == null)
        {
            StartCoroutine(CheckReplicas(0));
        }
    }
    public IEnumerator CheckReplicas(float time)
    {
        yield return new WaitForSeconds(time);
        bufer?.afterReplicaAction?.Invoke();
        source.Stop();
        replicPanel.SetActive(false);
        if (replicas.Count > 0)
            StartReplica();
        else
        {
            bufer = null;
            mouseLock.ReturnView();
            inputMove.SetDialogueState(false);
            mouseLock.SetDialogueState(false);
            skipImage.enabled = false;
            opportunityToSkip = false;
        }
    }
    private void StartReplica()
    {
        bufer = replicas[0];

        if(!opportunityToSkip)
        {
            if (bufer.playerTarget != null)
            {
                inputMove.SetDialogueState(true);
                mouseLock.SetDialogueState(true);
                mouseLock.SmoothLookToTarget(replicas[0].playerTarget);
                skipImage.enabled = true;
                opportunityToSkip = true;
            }
        }

        source.clip = bufer.clip;
        replicPanel.SetActive(true);
        replicText.text = bufer.replicText;
        replicText.color = bufer.role.roleTextColor;
        speackerImage.sprite = bufer.role.roleIcon;
        source.Play();
        StartCoroutine(CheckReplicas(source.clip.length + 0.3f));
        replicas.Remove(replicas[0]);
    }
}

[Serializable]
public class ReplicItem
{
    public ReplicaRole role;
    public Transform playerTarget;
    public AudioClip clip;
    public string replicText;
    public UnityEvent afterReplicaAction;
}

public interface IDialogueActor
{
    void SetDialogueState(bool inDialogueState);
}
