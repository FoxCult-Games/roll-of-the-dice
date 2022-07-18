using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class RollTheDiceHandler : MonoBehaviour
{
    [Header("Transition")]
    public LevelLoader levelLoader;
    [SerializeField] private float transitionDuration;
    [SerializeField] private CanvasGroup RollDicePanel;
    [SerializeField] private float rollDicePanelFadeTransitionOffset;

    [Header("Dialogue System")]
    public DialogueSystem dialogueSystem;

    public event EventHandler OnRollDicePanelFaded; 

    public void HandleButtonClicked(){
        GetComponent<Button>().interactable = false;
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

        RollDicePanel.gameObject.SetActive(false);

        OnRollDicePanelFaded?.Invoke(this, EventArgs.Empty);
    }
}
