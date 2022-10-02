using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Hierarachy References")]
    [SerializeField] private Text coinsText;
    [SerializeField] private Text pointsText;

    [Header("SpaceShip Health")]
    [SerializeField] Image healthFillImage;
    [SerializeField] Text healthText;
    [SerializeField] DieEffect dieEffect;
    //-------------------------------------------------------------------------
    private void OnEnable()
    {
        PlayerSpaceship.onPlayerHealthChanged += UpdateHealthData;
    }

    private void OnDisable()
    {
        PlayerSpaceship.onPlayerHealthChanged -= UpdateHealthData;
    }
    //-------------------------------------------------------------------------
    public void UpdateCoinsAmount(int _coinsCount)
    {
        coinsText.text = _coinsCount.ToString();
    }

    public void UpdatePointsText(int _pointsAmount)
    {
        pointsText.text = _pointsAmount.ToString();
    }

    public void UpdateHealthData(int _healthAmount)
    {
        healthFillImage.fillAmount = _healthAmount * 0.01f;
        healthText.text = _healthAmount.ToString();

        if (_healthAmount < 95)
            dieEffect.gameObject.SetActive(true);
    }

    public void ChangePlayerWeaponType(int _typeIndex)
    {
        GameManager.instance.PlayerSpaceship.ChangeWeapon(_typeIndex);
    }
}
