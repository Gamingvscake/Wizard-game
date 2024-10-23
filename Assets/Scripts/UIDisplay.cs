using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class UIDisplay : MonoBehaviour
{

    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider.maxValue = PlayerInflicts.PlayerCurrentHealth;
        slider.value = PlayerInflicts.PlayerCurrentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = PlayerInflicts.PlayerCurrentHealth;
    }
}
