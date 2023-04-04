using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance;
    private float platformCooldown = 10;
    private float starCooldown = 5;
    [SerializeField] GameObject platform;
    [SerializeField] GameObject star;
    void Awake() {
        Instance = this;
    }

    public void cooldown(float amount) {
        platformCooldown -= amount;
        starCooldown -= amount;
        if (platformCooldown <= 0) {
            SpawnPlatform();
            platformCooldown = 10;
        }
        if (starCooldown <= 0) {
            SpawnStar();
            starCooldown = 20;
        }
    }

    public void SpawnPlatform() {
        Transform cameraTransform = GameManager.Instance.GetCameraTransform();
        Vector2 position = new Vector3(Random.Range(-50, 50), cameraTransform.position.y + 40, 3.237868f);
        GameObject _platform = Instantiate(platform, position, Quaternion.identity);
        _platform.transform.localScale = new Vector3(Random.Range(4, 30), Random.Range(1, 10), 9.303145f);
    }

    public void SpawnStar() {
        Transform cameraTransform = GameManager.Instance.GetCameraTransform();
        Vector2 position = new Vector2(Random.Range(-40, 40), cameraTransform.position.y + 40);
        Instantiate(star, position, Quaternion.identity);
    }
}
