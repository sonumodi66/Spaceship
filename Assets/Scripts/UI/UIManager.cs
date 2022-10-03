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
    [SerializeField] Button quitButton;

    [Header("Weapon Type Buttons")]
    [SerializeField] Button[] weaponSelctionButtons;

    Color startingColor;
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
        quitButton.onClick.AddListener(Button_OnGameQuit);
        startingColor = weaponSelctionButtons[0].GetComponent<Image>().color;

        for (int i = 0; i < weaponSelctionButtons.Length; i++)
        {
            int index = i;
            weaponSelctionButtons[i].onClick.AddListener(() => Button_ChangeWeaponType(index));
        }

        Button_ChangeWeaponType(0);
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
    void Button_ChangeWeaponType(int _typeIndex)
    {
        GameManager.instance.PlayerSpaceship.ChangeWeapon(_typeIndex);

        for (int i = 0; i < weaponSelctionButtons.Length; i++)
        {
            if (i == _typeIndex)
                weaponSelctionButtons[i].GetComponent<Image>().color = Color.green;
            else
                weaponSelctionButtons[i].GetComponent<Image>().color = startingColor;
        }
    }

    void Button_OnRestartGame()
    {
        GameManager.instance.RestartGame();
    }

    void Button_OnGameQuit()
    {
        //Save all data before quitting 
        Application.Quit();
    }


}
