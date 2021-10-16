using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(AudioSource))]
public class ReplicDispether : MonoBehaviour
{
    [SerializeField] private GameObject replicPanel;
    [SerializeField] private Text replicText;
    [SerializeField] private Text speakerText;

    [SerializeField] private InputMove inputMove;
    [SerializeField] private MouseLock mouseLock;

    private List<ReplicItem> replicas;
    private AudioSource source;
    private ReplicItem bufer;

    [ContextMenu("Setup()")]
    public void Setup()
    {
        replicas = new List<ReplicItem>();
        source = GetComponent<AudioSource>();
        replicPanel.SetActive(false);
    }

    private void Update()
    {
        if(bufer != null && Input.GetKeyDown(KeyCode.Space))
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
            if(replicas[0].playerTarget != null)
            {
                inputMove.SetDialogueState(true);
                mouseLock.SetDialogueState(true);
                mouseLock.SmoothLookToTarget(replicas[0].playerTarget);
            }
            StartCoroutine(CheckReplicas(0));
        }
    }
    public IEnumerator CheckReplicas(float time)
    {
        yield return new WaitForSeconds(time);
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
        }
    }
    private void StartReplica()
    {
        bufer = replicas[0];
        source.clip = bufer.clip;
        replicPanel.SetActive(true);
        replicText.text = bufer.replicText;
        replicText.color = bufer.textColor;
        source.Play();
        StartCoroutine(CheckReplicas(source.clip.length + 0.3f));
        replicas.Remove(replicas[0]);
    }
}

[Serializable]
public class ReplicItem
{
    public Transform playerTarget;
    public AudioClip clip;
    public Color textColor = new Color(0, 0, 0, 1);
    public string replicText;
    public string speakerName;
}

public interface IDialogueActor
{
    void SetDialogueState(bool inDialogueState);
}
