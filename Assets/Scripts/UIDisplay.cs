using UnityEngine;
using UnityEngine.UI;

public class UIDisplay : MonoBehaviour
{
    public Slider slider;
    public PlayerInflicts UIPI;
    public Image fill;
    // Start is called before the first frame update
    void Start()
    {
        slider.maxValue = UIPI.PlayerMaxHealth;
        slider.value = UIPI.PlayerCurrentHealth;
        UIPI = transform.root.GetComponent<PlayerInflicts>();
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = UIPI.PlayerCurrentHealth;
        if (slider.value > UIPI.PlayerMaxHealth / 2)
        {
            fill.color = Color.green;
        }
        else if (slider.value < UIPI.PlayerMaxHealth / 4)
        {
            fill.color = Color.red;
        }
        else if (slider.value < UIPI.PlayerMaxHealth / 2)
        {
            fill.color = Color.yellow;
        }
    }
}
