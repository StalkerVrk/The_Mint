using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;


public class AchievementsScripts : MonoBehaviour
{
    [SerializeField] int money;
    public int totalMoney;

    public string[] arrayTitles;
    public Sprite[] arraySprites;
    public GameObject buttonPrefab;
    public GameObject content;

    private List<GameObject> list = new List<GameObject>();
    private VerticalLayoutGroup _group;


    void Start()
    {
        money = PlayerPrefs.GetInt("money");
        totalMoney = PlayerPrefs.GetInt("totalMoney");

        RectTransform rectT = content.GetComponent<RectTransform>();
        rectT.transform.localPosition = Vector3.zero;
        _group = GetComponent<VerticalLayoutGroup>();
        SetAchievs();
    }

    private void RemovedList()
    {
        foreach (var elem in list)
        {
            Destroy(elem);
        }
        list.Clear();
    }

    void SetAchievs()
    {
        RectTransform rectT = content.GetComponent<RectTransform>();
        rectT.localPosition = Vector3.zero;
        RemovedList();
        if(arraySprites.Length > 0)
        {
            GameObject pr1 = Instantiate(buttonPrefab, transform);
            float h = pr1.GetComponent<RectTransform>().rect.height;
            RectTransform tr = GetComponent<RectTransform>();
            tr.sizeDelta = new Vector2(tr.rect.width, h * arrayTitles.Length);
            Destroy(pr1);
            for(int i = 0; i < arrayTitles.Length; i++)
            {
                GameObject prefabButton = Instantiate(buttonPrefab, transform);
                RectTransform prefabButtonSizeDelta = prefabButton.GetComponent<RectTransform>();
                prefabButtonSizeDelta.sizeDelta = new Vector2(tr.rect.width - 100, prefabButtonSizeDelta.rect.height);

                TextMeshProUGUI textComponent = prefabButton.GetComponentInChildren<TextMeshProUGUI>();
                textComponent.text = arrayTitles[i];

                 //imageComponent = prefabButton.GetComponent<Image>();
                Image imageComponent = prefabButton.GetComponentInChildren<Image>();
                Debug.Log(imageComponent);

                //pr.GetComponentsInChildren<Image>()[1].sprite = arraySprites[i];
                int j = i;
                prefabButton.GetComponent<Button>().onClick.AddListener(() => { Debug.Log($"Clicked button at index {j}"); GetAchievement(j); });
                list.Add(prefabButton);
            }
        }
    }

    void GetAchievement(int id)
    {
        switch (id)
        {
            case 0:
                Debug.Log(id);
                break; 
            case 1:
                Debug.Log(id);
                money += 10;
                PlayerPrefs.SetInt("money", money);
                break;
        }
    }
}
