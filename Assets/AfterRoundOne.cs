using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AfterRoundOne : MonoBehaviour
{
    private RollTheDiceHandler rollTheDiceHandler;

    void Start()
    {
        rollTheDiceHandler = GetComponent<RollTheDiceHandler>();
        rollTheDiceHandler.OnRollDicePanelFaded += InitDialogueSystem;
    }

    private void InitDialogueSystem(object o, EventArgs args){
        DialogueSystem dialogueSystem = rollTheDiceHandler.dialogueSystem;

        dialogueSystem.OnDialogueFinished += LoadBallsGameplayScene;

        int mistakes = PlayerPrefs.GetInt("round1_mistakes");

        Debug.Log(mistakes);

        if(mistakes >= 0 && mistakes < 3) dialogueSystem.LoadDialogue(24);
        else if(mistakes >= 3 && mistakes < 6) dialogueSystem.LoadDialogue(25);
        else dialogueSystem.LoadDialogue(26);
    }

    private void LoadBallsGameplayScene(object o, EventArgs args){
        rollTheDiceHandler.levelLoader.PlayTransitionAnimation("BallsGameplay_second");
    }
}
