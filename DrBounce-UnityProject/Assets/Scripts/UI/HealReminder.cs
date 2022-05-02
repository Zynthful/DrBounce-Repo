using UnityEngine;

public class HealReminder: MonoBehaviour
{
    [SerializeField] private GameObject Text;

    private void Start()
    {
        TurnOff();
    }

    void OnEnable()
    {
        PlayerHealth.ShouldHeal += TurnOn;
        Health.HasHealed += TurnOff;
    }


    void OnDisable()
    {
        PlayerHealth.ShouldHeal -= TurnOn;
        Health.HasHealed -= TurnOff;
    }

    private void TurnOn() 
    {
        Text.SetActive(true);
    }

    private void TurnOff() 
    {
        Text.SetActive(false);
    }
}
