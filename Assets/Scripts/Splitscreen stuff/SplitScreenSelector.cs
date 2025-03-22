using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // Required for detecting controllers

public class SplitScreenSelector : MonoBehaviour
{
    public Camera cam1, cam2, cam3, cam4;
    public GameObject player1, player2, player3, player4; // Player objects to enable/disable

    private int lastControllerCount = 0;

    void Start()
    {
        UpdatePlayers();
    }

    void Update()
    {
        int currentControllerCount = Gamepad.all.Count;

        // Only update if the controller count has changed
        if (currentControllerCount != lastControllerCount)
        {
            lastControllerCount = currentControllerCount;
            UpdatePlayers();
        }
    }

    private void UpdatePlayers()
    {
        // Enable/Disable players based on controller count
        player1.SetActive(lastControllerCount >= 1);
        player2.SetActive(lastControllerCount >= 2);
        player3.SetActive(lastControllerCount >= 3);
        player4.SetActive(lastControllerCount >= 4);

        // Update camera layout
        SetSplitScreen(lastControllerCount);
    }

    private void SetSplitScreen(int activePlayers)
    {
        switch (activePlayers)
        {
            case 1:
                cam1.rect = new Rect(0f, 0f, 1f, 1f);
                cam2.gameObject.SetActive(false);
                cam3.gameObject.SetActive(false);
                cam4.gameObject.SetActive(false);
                break;
            case 2:
                cam1.rect = new Rect(0f, .5f, 1f, .5f);
                cam2.rect = new Rect(0f, 0f, 1f, .5f);
                cam2.gameObject.SetActive(true);
                cam3.gameObject.SetActive(false);
                cam4.gameObject.SetActive(false);
                break;
            case 3:
                cam1.rect = new Rect(0f, .5f, 1f, .5f);
                cam2.rect = new Rect(0f, 0f, .5f, .5f);
                cam3.rect = new Rect(.5f, 0f, .5f, .5f);
                cam2.gameObject.SetActive(true);
                cam3.gameObject.SetActive(true);
                cam4.gameObject.SetActive(false);
                break;
            case 4:
                cam1.rect = new Rect(0f, .5f, .5f, .5f);
                cam2.rect = new Rect(.5f, .5f, .5f, .5f);
                cam3.rect = new Rect(0f, 0f, .5f, .5f);
                cam4.rect = new Rect(.5f, 0f, .5f, .5f);
                cam2.gameObject.SetActive(true);
                cam3.gameObject.SetActive(true);
                cam4.gameObject.SetActive(true);
                break;
            default:
                cam1.rect = new Rect(0f, 0f, 1f, 1f);
                cam2.gameObject.SetActive(false);
                cam3.gameObject.SetActive(false);
                cam4.gameObject.SetActive(false);
                break;
        }
    }
}
