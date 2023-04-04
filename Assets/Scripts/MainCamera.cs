using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    //public static MainCamera mainCamera;
    [SerializeField] private float panIntensity = 0.5f;
    private RectTransform panelTransform;
    private Camera _camera;

    void Awake() {
        _camera = GetComponent<Camera>();
    }

    void FixedUpdate()
    {
        PanCamera();
    }

    void PanCamera() {
        float initialYPosition = transform.position.y;
        Transform playerTransform = GameManager.Instance.GetPlayerTransform();
        if (playerTransform == null) {

        } else {
            float playerYPositionToCamera = _camera.WorldToScreenPoint(playerTransform.position, Camera.MonoOrStereoscopicEye.Mono).y;
            float panIntensityTime = Time.deltaTime * panIntensity;
            float panIntensityTime10 = panIntensityTime * 10;
            if (playerYPositionToCamera > Screen.height * 0.7f) {
                float panMagnitude = (playerYPositionToCamera - Screen.height * 0.7f) * 0.1f;
                if(panMagnitude < panIntensityTime) {
                    transform.Translate(Vector3.up * panIntensityTime);
                    GameManager.Instance.ChangeScore(panIntensityTime10, panIntensityTime10);
                } else {
                    transform.Translate(Vector3.up * panMagnitude);
                    GameManager.Instance.ChangeScore(panMagnitude * 10, panIntensityTime10);
                }
            }
            else {
                transform.Translate(Vector3.up * panIntensityTime);
                GameManager.Instance.ChangeScore(panIntensityTime10, panIntensityTime10);
            }
            panIntensity += Time.deltaTime * 0.05f;
            GameManager.Instance.CameraPanChange(transform.position.y - initialYPosition);
            
            PlayerPositionCheck(playerYPositionToCamera);
        }
    }

    private void PlayerPositionCheck(float playerYPositionToCamera) {
        panelTransform = GameManager.Instance.dangerPanel.GetComponent<RectTransform>();
        if (playerYPositionToCamera < 0) {
            float DestroyerYPositionToCamera = _camera.WorldToScreenPoint(GameManager.Instance.mapDestroyer.transform.position, Camera.MonoOrStereoscopicEye.Mono).y;
            panelTransform.localPosition = new Vector2(0, _camera.pixelHeight * 2f + (DestroyerYPositionToCamera + playerYPositionToCamera) * 1.8f);
        }
        else if (panelTransform.localPosition.y < _camera.pixelHeight) {
            panelTransform.localPosition = new Vector2(0, _camera.pixelHeight);
        }
    }
}
