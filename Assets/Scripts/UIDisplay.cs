using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIDisplay : MonoBehaviour
{
    public Slider slider;
    public PlayerInflicts UIPI;
    // Start is called before the first frame update
    void Start()
    {
        slider.maxValue = UIPI.PlayerMaxHealth;
        slider.value = PlayerInflicts.PlayerCurrentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = PlayerInflicts.PlayerCurrentHealth;
    }
}
