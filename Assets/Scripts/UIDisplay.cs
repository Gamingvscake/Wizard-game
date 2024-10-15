using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class UIDisplay : MonoBehaviour
{

    public TMP_Text HealthText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HealthText.text = PlayerInflicts.PlayerCurrentHealth.ToString();

    }
}
