using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RollTheDiceHandler : MonoBehaviour
{
    [Header("Transition")]
    [SerializeField] private LevelLoader levelLoader;
    [SerializeField] private float transitionDuration;
    [SerializeField] private CanvasGroup RollDicePanel;
    [SerializeField] private float rollDicePanelFadeTransitionOffset;

    [Header("Dialogue System")]
    [SerializeField] private DialogueSystem dialogueSystem;

    private event EventHandler OnRollDicePanelFaded; 

    public void HandleButtonClicked(){
        OnRollDicePanelFaded += InitDialogueSystem;
        StartCoroutine(AfterButtonAnimation());
    }

    private IEnumerator AfterButtonAnimation(){
        yield return new WaitForSeconds(transitionDuration);
        StartCoroutine(RollDicePanelFadeAnimation());
    }

    private IEnumerator RollDicePanelFadeAnimation(){
        while(RollDicePanel.alpha > 0){
            yield return new WaitForSeconds(rollDicePanelFadeTransitionOffset);
            RollDicePanel.alpha -= rollDicePanelFadeTransitionOffset;
        }

        OnRollDicePanelFaded?.Invoke(this, EventArgs.Empty);
    }

    private void InitDialogueSystem(object o, EventArgs args){
        dialogueSystem.gameObject.SetActive(true);
        dialogueSystem.OnDialogueFinished += LoadBallsGameplayScene;
        dialogueSystem.LoadDialogue(15);
    }

    private void LoadBallsGameplayScene(object o, EventArgs args){
        levelLoader.PlayTransitionAnimation("BallsGameplay");
    }
}
