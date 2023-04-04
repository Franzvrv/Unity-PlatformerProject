using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    private Animator animator;
    void Awake() {
        animator = GetComponent<Animator>();
    }

    public void LoadScene(int scene) {
        SceneManager.LoadScene(scene);
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void Click() {
        SoundManager.Instance.PlayUntimedSound("Click");
    }

    public void OnMouseEnter() {
        animator.SetBool("hover", true);
        SoundManager.Instance.PlayUntimedSound("ButtonHover");
    }

    public void OnMouseExit() {
        animator.SetBool("hover", false);
    }
}
