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
    [SerializeField] private ReplicaRole blackGrimoirRole;
    [SerializeField] private Animator blackGrimoirAnim;

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
        replicText.CrossFadeAlpha(0, 0, false);
        replicPanel.SetActive(false);
        opportunityToSkip = false;
    }

    private void Update()
    {
        if(opportunityToSkip && Input.GetKeyDown(KeyCode.Space))
        {
            StopAllCoroutines();
            if(replicas.Count > 0)
            {
                replicas.RemoveAt(0);
                replicText.CrossFadeAlpha(0, 0, false);
            }
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
            replicPanel.SetActive(true);
            skipImage.enabled = false;
            StartCoroutine(CheckReplicas(0));
        }
    }
    public IEnumerator CheckReplicas(float time)
    {
        yield return new WaitForSeconds(time);
        bufer?.afterReplicaAction?.Invoke();
        source.Stop();
        
        if (replicas.Count > 0)
            StartCoroutine(StartReplica());
        else
        {
            bufer = null;
            mouseLock.ReturnView();
            inputMove.SetDialogueState(false);
            mouseLock.SetDialogueState(false);
            skipImage.enabled = false;
            opportunityToSkip = false;
            replicText.CrossFadeAlpha(0, 0.5f, true);
            yield return new WaitForSeconds(0.6f);
            replicText.text = string.Empty;
            replicPanel.SetActive(false);
            blackGrimoirAnim.SetBool("Talk", false);
        }
    }
    private IEnumerator StartReplica()
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

        replicText.CrossFadeAlpha(0, 0.5f, true);

        yield return new WaitForSeconds(0.5f);

        replicText.text = bufer.replicText;
        replicText.color = bufer.role.roleTextColor;
        speackerImage.sprite = bufer.role.roleIcon;
        replicText.CrossFadeAlpha(1, 0.5f, true);
        yield return new WaitForSeconds(0.5f);
        if (bufer.role == blackGrimoirRole)
        {
            blackGrimoirAnim.SetBool("Talk", true);
        }
        else
        {
            blackGrimoirAnim.SetBool("Talk", false);
        }
        source.Play();
        StartCoroutine(CheckReplicas(source.clip.length + 0.3f));
        replicas.Remove(replicas[0]);
    }

    public void SetTriggerForAnim()
    {
        blackGrimoirAnim.SetTrigger("Action");
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
