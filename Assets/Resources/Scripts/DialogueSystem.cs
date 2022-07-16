using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] private DialogueSystemData dialogueSystemData;
    [SerializeField] private LevelLoader levelLoader;

    private DialogueMessages messages;
    private TextMeshProUGUI dialogueContainer;
    private string targetMessage;
    private Coroutine typpingAnimation; 
    [SerializeField] private int startingMessage;
    [SerializeField] private Dictionary<int, bool> playerChoices = new Dictionary<int, bool>();
    [SerializeField] private Button agreeButton;
    [SerializeField] private Button disagreeButton;

    [SerializeField] private bool listenForInputs = false;

    private TextMeshProUGUI agreeButtonText;
    private TextMeshProUGUI disagreeButtonText;

    public event EventHandler OnDialogueFinished;

    void Awake()
    {
        dialogueContainer = GetComponent<TextMeshProUGUI>();
        agreeButtonText = agreeButton.transform.GetComponentInChildren<TextMeshProUGUI>();
        disagreeButtonText = disagreeButton.transform.GetComponentInChildren<TextMeshProUGUI>();
    }

    void Start()
    {
        LoadDialogue(startingMessage);
    }

    /// <summary>
    /// Load dialogue based on given index
    /// </summary>
    /// <param name="index">Index of the dialogue</param>
    public void LoadDialogue(int index){
        if(!isTyppingAnimationFinished()) StopCoroutine(typpingAnimation);
        agreeButton.onClick.RemoveAllListeners();
        disagreeButton.onClick.RemoveAllListeners();

        if(messages == null) messages = ReadMessagesFromFile();

        DialogueMessage message = messages.messages.Where(item => item.id == index).ToArray()[0];

        if(message == null){
            Debug.LogWarning("Nie znaleziono dialogu!");
            return;
        }

        if(agreeButtonText != null) agreeButtonText.text = message.yesText;
        if(disagreeButtonText != null) disagreeButtonText.text = message.noText;

        if(message.yesText == null || message.yesText == string.Empty) agreeButton.gameObject.SetActive(false);
        else agreeButton.gameObject.SetActive(true);

        if(message.noText == null || message.noText == string.Empty) disagreeButton.gameObject.SetActive(false);
        else disagreeButton.gameObject.SetActive(true);

        agreeButton.onClick.AddListener(delegate {
            if(isTyppingAnimationFinished()){
                if(playerChoices.ContainsKey(index)) playerChoices[index] = false;
                else playerChoices.Add(index, false);

                if(message.yesAction == 0){
                    levelLoader.PlayTransitionAnimation("MainGameplay");
                } else 
                    LoadDialogue(message.yesAction);
            }
            else 
                SkipDialogue();
        });
        disagreeButton.onClick.AddListener(delegate {
            if(isTyppingAnimationFinished()){
                if(playerChoices.ContainsKey(index)) playerChoices[index] = false;
                else playerChoices.Add(index, false);

                if(message.noAction == 0){
                    levelLoader.PlayTransitionAnimation("MainGameplay");
                } else 
                    LoadDialogue(message.noAction);
            }
            else 
                SkipDialogue();
        });

        typpingAnimation = StartCoroutine(TyppingAnimation(message.message));
    }

    /// <summary>
    /// Skips typping animation if key is pressed
    /// </summary>
    public void OnSkipDialogue(){
        if(!listenForInputs) return;

        Debug.Log(isTyppingAnimationFinished());

        if(!isTyppingAnimationFinished()) SkipDialogue();
        else OnDialogueFinished?.Invoke(this, EventArgs.Empty);
    }

    private void SkipDialogue(){
        StopCoroutine(typpingAnimation);
        dialogueContainer.text = targetMessage;
    }

    /// <summary>
    /// Runs typping animation for dialogue system
    /// </summary>
    /// <param name="text">Target text</param>
    /// <returns></returns>
    private IEnumerator TyppingAnimation(string text){
        dialogueContainer.text = "";
        targetMessage = text;
        
        for (int i = 0; i < text.Length; i++)
        {
            dialogueContainer.text += text[i];
            yield return new WaitForSeconds(dialogueSystemData.typpingDelay);
        }
    }

    /// <summary>
    /// Checks if typping animation is finished
    /// </summary>
    /// <returns></returns>
    private bool isTyppingAnimationFinished() {
        if(typpingAnimation == null) return true;
        return dialogueContainer.text == targetMessage;
    }

    /// <summary>
    /// Read messages json file content and pass it into DialogueMessages
    /// </summary>
    /// <returns>
    /// An array of DialogueMessages
    /// </returns>
    private DialogueMessages ReadMessagesFromFile(){
        string context = File.ReadAllText(Application.dataPath + "/Resources/content/dialogues.json"); 
        DialogueMessages result = JsonUtility.FromJson<DialogueMessages>("{\"messages\":" + context + "}");
        return result;
    }

    [Serializable]
    private class DialogueMessages{
        public DialogueMessage[] messages;
    }

    [Serializable]
    private class DialogueMessage{
        public int id;
        public string message;
        public string yesText;
        public string noText;
        public int yesAction;
        public int noAction;
    }

}