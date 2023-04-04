using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private Player player;

    void Awake()
    {
        player = GetComponentInParent<Player>();
        
    }
    
    void Update()
    {
        if (!GameManager.Instance.Paused) {
            //Horizontal input
            player.horizontal = Input.GetAxisRaw("Horizontal");

            //Jump input
            if (Input.GetKeyDown(KeyCode.Space)) {
                player.JumpInput();
            }

            //Dash input
            if (Input.GetKeyDown(KeyCode.LeftShift) && player.dashable) {
                player.Dash();
            }
        }
        
    }
}
