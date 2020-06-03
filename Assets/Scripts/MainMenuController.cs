using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public Camera mainMenuCamera;
    public float rotateSpeed;

    public void StartGameButton()
    {
        SceneManager.LoadScene(1);
    }

    public void HowToPlayButton()
    {
        Application.OpenURL("https://docs.google.com/document/d/1tvtsWmotHmJYpR_yKkokHBZLO9Kuv5Q2jwE_bIYh-tI/edit?usp=sharing");
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void Update()
    {
        mainMenuCamera.transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
    }

    public void Start()
    {
        mainMenuCamera.transform.Rotate(Vector3.forward * Random.Range(0, 360));
    }
}
