using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using Cinemachine;
public class DialogueManager : MonoBehaviour
{
    public List<string> dialogues;
    private int currentDialogueIndex;

    public DialogueUI dialogueUI;

    private InputAction zAction;

    public PlayableDirector director;
    public CinemachineVirtualCamera cutsceneCam;

    private bool isDialogueStarted = false;

    void Start()
    {
        zAction = new InputAction("ZPress", InputActionType.Button, "<Keyboard>/z");
        zAction.Enable();

        currentDialogueIndex = 0;

        director.played += Director_played;
        director.stopped += Director_stopped;
    }

    private void Director_played(PlayableDirector obj)
    {
        cutsceneCam.Priority = 11;
    }

    private void Director_stopped(PlayableDirector obj)
    {
        cutsceneCam.Priority = 0;
    }

    void Update()
    {
        if (isDialogueStarted && zAction.triggered)
        {
            ShowNextDialogue();
        }
    }

    void ShowNextDialogue()
    {
        if (currentDialogueIndex < dialogues.Count)
        {
            dialogueUI.ShowDialogue(dialogues[currentDialogueIndex]);
            currentDialogueIndex++;
        }
        else
        {
            dialogueUI.HideDialogue();
            isDialogueStarted = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isDialogueStarted && other.CompareTag("Player"))
        {
            director.Play();
            ShowNextDialogue();
            isDialogueStarted = true;
        }
    }

    private void OnDisable()
    {
        zAction.Disable();
    }
}