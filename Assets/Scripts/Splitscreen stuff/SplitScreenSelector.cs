using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitScreenSelector : MonoBehaviour
{
    //Creating a public variable for all the player cameras
    public Camera cam1, cam2, cam3, cam4;

    //Creating a true or false for which camera mode is activated at the moment
    private bool onePlayer, twoPlayer, threePlayer, fourPlayer;

    void Update()
    {
        //Sets the camera mode to one player
        if (Input.GetKeyDown(KeyCode.F1))
        {
            onePlayer = true;
            twoPlayer = false;
            threePlayer = false;
            fourPlayer = false;
            SetSplitScreen();
        }
        //sets the camera mode to two player
        if (Input.GetKeyDown(KeyCode.F2))
        {
            onePlayer = false;
            twoPlayer = true;
            threePlayer = false;
            fourPlayer = false;
            SetSplitScreen();
        }
        //Sets the camera mode to three player
        if (Input.GetKeyDown(KeyCode.F3))
        {
            onePlayer = false;
            twoPlayer = false;
            threePlayer = true;
            fourPlayer = false;
            SetSplitScreen();
        }
        //Sets the camera mode to four player
        if (Input.GetKeyDown(KeyCode.F4))
        {
            onePlayer = false;
            twoPlayer = false;
            threePlayer = false;
            fourPlayer = true;
            SetSplitScreen();
        }
        //This wont be constantly called in the real game, but instead we can assign these variables to a menu option so they are only seen once
    }

    public void SetSplitScreen()
    {
        if (onePlayer == true)
        {
            cam1.rect = new Rect(0f, 0f, 1f, 1f);
            cam2.rect = new Rect(1f, 1f, 1f, 1f);
            cam3.rect = new Rect(1f, 1f, 1f, 1f);
            cam4.rect = new Rect(1f, 1f, 1f, 1f);
        }
        else if (twoPlayer == true)
        {
            cam1.rect = new Rect(0f, .5f, 1f, .5f);
            cam2.rect = new Rect(0f, 0f, 1f, .5f);
            cam3.rect = new Rect(1f, 1f, 1f, 1f);
            cam4.rect = new Rect(1f, 1f, 1f, 1f);
        }
        else if (threePlayer == true)
        {
            cam1.rect = new Rect(0f, .5f, 1f, .5f);
            cam2.rect = new Rect(0f, 0f, .5f, .5f);
            cam3.rect = new Rect(.5f, 0f,.5f, .5f);
            cam4.rect = new Rect(1f, 1f, 1f, 1f);
        }
        else if (fourPlayer == true)
        {
            cam1.rect = new Rect(0f, .5f, .5f, .5f);
            cam2.rect = new Rect(.5f, .5f, .5f, .5f);
            cam3.rect = new Rect(0f, 0f, .5f, .5f);
            cam4.rect = new Rect(.5f, 0f, .5f, .5f); ;
        }
    }
}
