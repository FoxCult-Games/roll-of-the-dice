using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AfterRoundTwo : MonoBehaviour
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

        int mistakes = PlayerPrefs.GetInt("round2_mistakes");

        Debug.Log(mistakes);

        if(mistakes >= 0 && mistakes < 2) dialogueSystem.LoadDialogue(33);
        else if(mistakes >= 3 && mistakes < 5) dialogueSystem.LoadDialogue(34);
        else dialogueSystem.LoadDialogue(35);
    }

    private void LoadBallsGameplayScene(object o, EventArgs args){
        rollTheDiceHandler.levelLoader.PlayTransitionAnimation("DialogueScene_End");
    }
}
