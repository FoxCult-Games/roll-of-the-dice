using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class BallGameplayController : MonoBehaviour
{
    [SerializeField] private int round = 1;
    [SerializeField] private int ballsCount = 4;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Basket[] ballsTypes;
    private List<BallColor> ballsColors = new List<BallColor>();
    [SerializeField] private List<BallController> balls = new List<BallController>();
    [SerializeField] private int mistakes = 0;
    [SerializeField] private DialogueSystem dialogueSystem;
    [SerializeField] private LevelLoader levelLoader;
    private bool gameEnded = false;
    [SerializeField] private bool playerControlGame = true;
    [SerializeField] private string nextSceneName;
    [SerializeField] private int endingMistakes = 7;

    void Start()
    {
        SpawnBalls();
    }

    private void SpawnBalls(){
        foreach (Basket basket in ballsTypes)
            ballsColors.Add(new BallColor(ballsCount, basket.color));

        StartCoroutine(SpawnBall());
    }

    public void CheckBaskets(){
        if(!dialogueSystem.isTyppingAnimationFinished()){
            dialogueSystem.SkipDialogue();
            return;
        }

        if(gameEnded){
            PlayerPrefs.SetInt($"round{round}_mistakes", mistakes);
            levelLoader.PlayTransitionAnimation(nextSceneName);
            return;
        } 

        Dictionary<Basket, bool> assignedBaskets = new Dictionary<Basket, bool>();

        foreach(Basket basket in ballsTypes){
            assignedBaskets.Add(basket, false);
        }

        int _mistakes = 0;

        foreach (BallController ball in balls)
        {
            if(ball.IsAssigned()) {
                if(ball.GetBasket().color != ball.GetComponent<Image>().color){
                    mistakes++;
                    _mistakes++;
                    break;
                }
            } else {
                mistakes++;
                _mistakes++;
                break;
            }
        }

        foreach(Basket assignedBasket in new List<Basket>(assignedBaskets.Keys)){
            bool assigned = false;

            foreach(BallController ball in balls){
                if(ball.GetBasket().basket == assignedBasket.basket){
                    assignedBaskets[assignedBasket] = true;
                    assigned = true;
                    break;
                }
            }

            if(!assigned) _mistakes++;
        }

        if(_mistakes > 0) ChooseMessage(dialogueSystem.startingMessage + mistakes);
        else ChooseMessage(dialogueSystem.startingMessage);

        if((_mistakes == 0 || mistakes == endingMistakes) && !gameEnded){
            gameEnded = true;
        }
    }

    private void ChooseMessage(int index){
        dialogueSystem.LoadDialogue(index);
    }

    private IEnumerator SpawnBall(){
        for(int i = 0; i < ballsTypes.Length; i++){
            Basket ball = ballsTypes[i];

            for(int j = 0; j < ballsCount; j++){
                BallColor ballColor;

                do {
                    ballColor = ballsColors[(int)UnityEngine.Random.Range(0, ballsColors.ToArray().Length)];
                } while (ballColor.ballsLeft == 0);

                GameObject ballObject = Instantiate(ballPrefab, ball.spawnPoint.position, Quaternion.identity, ball.basket);
                ballObject.GetComponent<Image>().color = ballColor.color;
                balls.Add(ballObject.GetComponent<BallController>());
                
                yield return new WaitForSeconds(.02f);
            }
        }
    }

    public Basket[] getBallsTypes(){
        return ballsTypes;
    }

    private struct BallColor {
        public int ballsLeft;
        public Color color;

        public BallColor(int ballsLeft, Color color){
            this.ballsLeft = ballsLeft;
            this.color = color;
        }
    }
}

[Serializable]
public struct Basket {
    public Color color;
    public Transform basket;
    public Transform spawnPoint;
}
