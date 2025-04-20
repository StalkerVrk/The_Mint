using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;
using System;
using Unity.VisualScripting;

public class Main : MonoBehaviour
{
    [SerializeField] int money;
    public int totalMoney;
    public Text moneyText;
    public GameObject effectSparksRocketl;
    public GameObject effectPointOne;
    public GameObject buttonRocket;
    public AudioSource audioClick;
    
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private Sprite originalSprite;  
    [SerializeField] private Sprite clickedSprite;  
    private bool isClickProcessed = false;

    public void Start()
    {
        audioClick = GetComponent<AudioSource>();
        money = PlayerPrefs.GetInt("money");
        totalMoney = PlayerPrefs.GetInt("totalMoney");

        if (totalMoney > 10)
        {
            StartCoroutine(AutoFarmClick());
        }
        OfflineTime();
    }

    void Update()
    {
        moneyText.text = money.ToString();

    }

    public void ButtonClickUp()
    {
        if (!isClickProcessed)
        {
            isClickProcessed = true;

            // Меняем спрайт на clickedSprite
            if (playerSprite != null && clickedSprite != null)
            {
                playerSprite.sprite = clickedSprite;
            }

            // Запускаем корутину для возврата спрайта через 1 секунду
            StartCoroutine(ResetSpriteAfterDelay(0.1f));
        }

        money++;
        totalMoney++;
        PlayerPrefs.SetInt("money", money);
        PlayerPrefs.SetInt("totalMoney", totalMoney);


        var positionForParticleSparks = buttonRocket.GetComponent<RectTransform>().position.normalized;
        positionForParticleSparks.y = -1.6f;
        Quaternion rotation = Quaternion.Euler(180f, 0f, 0f);
        Instantiate(effectSparksRocketl, positionForParticleSparks, rotation);
        Instantiate(effectPointOne, buttonRocket.GetComponent<RectTransform>().position.normalized, Quaternion.identity);

        buttonRocket.GetComponent<RectTransform>().localScale = new Vector3(0.98f, 0.97f);
        audioClick.Play();
    }

    public void ButtonClick()
    {
        buttonRocket.GetComponent<RectTransform>().localScale = new Vector3(1, 1);
    }

    public void ToAchievements()
    {
        SceneManager.LoadScene(1);
    }

    IEnumerator AutoFarmClick()
    {
        yield return new WaitForSeconds(1);
        money++;
        Debug.Log(money);
        PlayerPrefs.SetInt("money", money);
        StartCoroutine(AutoFarmClick());
    }

    private void OfflineTime()
    {
        TimeSpan ts;
        if (PlayerPrefs.HasKey("LastSession"))
        {
            ts = DateTime.Now - DateTime.Parse(PlayerPrefs.GetString("LastSession"));
            Debug.Log(ts.ToString());
            money += (int)ts.TotalSeconds;
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString("LastSession", DateTime.Now.ToString());
    }

    private IEnumerator ResetSpriteAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (playerSprite != null && originalSprite != null)
        {
            playerSprite.sprite = originalSprite;
        }

        isClickProcessed = false;
    }
}
