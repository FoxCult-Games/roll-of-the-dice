using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class BallGameplayController : MonoBehaviour
{
    [SerializeField] private int ballsCount = 4;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Basket[] ballsTypes;
    private List<BallColor> ballsColors = new List<BallColor>();
    private List<BallController> balls = new List<BallController>();
    [SerializeField] private int mistakes = 0;

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
        Debug.Log("Checking baskets!");

        foreach (BallController ball in balls)
        {
            if(ball.IsAssigned()) {
                if(ball.GetBasket().color == ball.GetComponent<Image>().color){

                } else {
                    Debug.Log("Nie właściwe przypisanie!");
                    mistakes++;
                }
            } else {
                Debug.Log("Nie skończono!");
            }
        }
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
