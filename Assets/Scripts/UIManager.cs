using DG.Tweening;
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

    [Header("Game Over")]
    [SerializeField] Transform gameOverPanel;
    [SerializeField] Text coinsCollectedText;
    [SerializeField] Text pointsCollectedText;
    [SerializeField] Button restartGameButton;
    //-------------------------------------------------------------------------
    private void OnEnable()
    {
        PlayerSpaceship.onPlayerHealthChanged += UpdateHealthData;
        PlayerSpaceship.onPlayerSpaceshipDied += ShowGameOverPanel;
    }

    private void OnDisable()
    {
        PlayerSpaceship.onPlayerHealthChanged -= UpdateHealthData;
        PlayerSpaceship.onPlayerSpaceshipDied -= ShowGameOverPanel;
    }
    private void Start()
    {
        restartGameButton.onClick.AddListener(Button_OnRestartGame);
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
    
    void ShowGameOverPanel()
    {
        coinsCollectedText.text = "Coins : " + coinsText.text;
        pointsCollectedText.text = "Score : " + pointsText.text;
        gameOverPanel.gameObject.SetActive(true);
        gameOverPanel.transform.DOScale(Vector3.one, 0.2f);
            //.OnComplete(() => container.SetActive(false))
            //.OnComplete(() => _onHidden?.Invoke());
    }
    //------------------------------------------------------------------------------------------
    public void ChangePlayerWeaponType(int _typeIndex)
    {
        GameManager.instance.PlayerSpaceship.ChangeWeapon(_typeIndex);
    }

    void Button_OnRestartGame()
    {
        GameManager.instance.RestartGame();
    }


}
