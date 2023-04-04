using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDestroyer : MonoBehaviour
{
    void Update() {
        Transform cameraTransform = GameManager.Instance.GetCameraTransform();
        transform.position = new Vector2(0, cameraTransform.position.y - 60);
    }

    void OnTriggerEnter2D(Collider2D collision) {
        Destroy(collision.gameObject);
        if(collision.GetComponent(typeof(Player))) {
            GameManager.Instance.GameOver();
        }
    }
}
