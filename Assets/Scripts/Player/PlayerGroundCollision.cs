using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCollision : MonoBehaviour
{
    private Player player;

    void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player.Ground();
        SoundManager.Instance.PlaySound("Ground");
    }

    private void OnTriggerExit2D(Collider2D collision) {
        player.Unground();
    }
}
