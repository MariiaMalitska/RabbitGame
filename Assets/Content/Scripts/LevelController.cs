using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController current;
    Vector3 startingPosition;
    int coins = 0;
    int fruit = 0;
    int crystals = 0;

    void Awake()
    {
        current = this;
    }

    public void setStartPosition(Vector3 pos)
    {
        this.startingPosition = pos;
    }
    public void onRabbitDeath(HeroController rabbit)
    {
        StartCoroutine(waitForDeath(1, rabbit));
    }

    IEnumerator waitForDeath(int sec, HeroController rabbit)
    {
        Animator animator = rabbit.GetComponent<Animator>();
        animator.SetBool("die", true);
        // Debug.LogWarning("Starting");
        yield return new WaitForSeconds(sec);
        // Debug.LogWarning("End");
        animator.SetBool("die", false);
        rabbit.transform.position = this.startingPosition;
        
    }

    public void addCoins(int amount)
    {
        coins++;
    }

    public void addCrystals(int amount)
    {
        crystals++;
    }

    public void addFruit(int amount)
    {
        fruit++;
    }
}
