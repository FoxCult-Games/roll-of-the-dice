using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private BallGameplayController ballGameplayController;

    [SerializeField] private Basket basket;
    [SerializeField] private bool assigned = false;

    public bool IsAssigned(){
        return assigned;
    }

    public Basket GetBasket() {
        return basket;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("trigger");

        if(!other.CompareTag("Basket")) return;

        if(other.transform.parent.TryGetComponent<BallGameplayController>(out ballGameplayController)){
            foreach (Basket b in ballGameplayController.getBallsTypes())
            {
                if(b.basket == other.transform) {
                    basket = b;
                    transform.SetParent(basket.basket);
                    assigned = true;
                    return;
                }
            }}
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(!other.CompareTag("Basket")) return;

        assigned = false;
    }
}
