using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;
using System;

public class Main : MonoBehaviour
{
    [SerializeField] int money;
    public int totalMoney;
    public Text moneyText;
    public GameObject effectSparksRocketl;
    public GameObject effectPointOne;
    public GameObject buttonRocket;
    public AudioSource audioClick;

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

    public void ButtonClick()
    {
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
    public void ButtonClickUp()
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
}
