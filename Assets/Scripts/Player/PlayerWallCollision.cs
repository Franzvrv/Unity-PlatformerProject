using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallCollision : MonoBehaviour
{
    private Player player;
    private int wallInstance = 0;

    void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(gameObject.transform.position.x < collision.transform.position.x && player.horizontal > 0) {
            player.Wall(WallDirection.Right);
        } else if (gameObject.transform.position.x > collision.transform.position.x && player.horizontal < 0){
            player.Wall(WallDirection.Left);
        }
        wallInstance++;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(gameObject.transform.position.x < collision.transform.position.x && player.horizontal > 0) {
            player.Wall(WallDirection.Right);
        } else if (gameObject.transform.position.x > collision.transform.position.x && player.horizontal < 0){
            player.Wall(WallDirection.Left);
        }
        else {
            player.Unwall();
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        wallInstance--;
        if (wallInstance == 0) {
            player.Unwall();
        }
    }
}
