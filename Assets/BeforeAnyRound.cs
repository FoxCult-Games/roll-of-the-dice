using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BeforeAnyRound : MonoBehaviour
{
    private RollTheDiceHandler rollTheDiceHandler;

    void Start()
    {
        rollTheDiceHandler = GetComponent<RollTheDiceHandler>();
        rollTheDiceHandler.OnRollDicePanelFaded += InitDialogueSystem;
    }

    private void InitDialogueSystem(object o, EventArgs args){
        DialogueSystem dialogueSystem = rollTheDiceHandler.dialogueSystem;

        dialogueSystem.gameObject.SetActive(true);
        dialogueSystem.OnDialogueFinished += LoadBallsGameplayScene;
        dialogueSystem.LoadDialogue(15);
    }

    private void LoadBallsGameplayScene(object o, EventArgs args){
        rollTheDiceHandler.levelLoader.PlayTransitionAnimation("BallsGameplay");
    }
}
