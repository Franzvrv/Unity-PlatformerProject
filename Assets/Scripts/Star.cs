using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : Item
{
    protected int score;

    void Awake() {
        itemType = ItemType.Score;
        score = 1000;
    }

    virtual public int GetItem()
    {
        return score;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent(typeof(Player))) {
            GameManager.Instance.ChangeScore(score, score);
            SoundManager.Instance.PlaySound("Collect");
            Destroy(this.gameObject);
        }
    }
}
