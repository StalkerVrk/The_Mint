using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainForAchievements : MonoBehaviour
{
    [SerializeField] int money;
    public int totalMoney;
    public Text moneyText;

    void Start()
    {
        money = PlayerPrefs.GetInt("money");
        totalMoney = PlayerPrefs.GetInt("totalMoney");

        if (totalMoney > 10)
        {
            StartCoroutine(AutoFarmClick());
        }

    }

    void Update()
    {
        moneyText.text = money.ToString();
        //if (money > 400)
        //{
        //    buyButton.interactable = true;
        //}
    }

    public void ToMainScence()
    {
        SceneManager.LoadScene(0);
    }

    IEnumerator AutoFarmClick()
    {
        yield return new WaitForSeconds(1);
        money = PlayerPrefs.GetInt("money");
        money++;
        Debug.Log(money);
        PlayerPrefs.SetInt("money", money);
        StartCoroutine(AutoFarmClick());
    }


}
