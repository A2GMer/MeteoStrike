using UnityEngine;
using UnityEngine.UI;

public class SkinsShop : MonoBehaviour
{

    public GameObject[] lookedSkins;
    public RectTransform content;
    public Text taskForUnllookSkin;
    public Text numberOfPlayedGames;
    public Text unlookedSkinsText;
    public Image skinInfo;
    public Animator infoPannel;

    private int unlookedSkinsNumber;
    private int numberOfPlayGames;

    public Sprite[] spriteSkins;

    void Start()
    {
        unlookedSkinsNumber = 1;
        numberOfPlayGames = PlayerPrefs.GetInt("numberOfPlayedGames");

        if (numberOfPlayGames > 5)
        {
            unlookedSkinsNumber++;
            lookedSkins[0].SetActive(false);
        }
        if (numberOfPlayGames > 25)
        {
            unlookedSkinsNumber++;
            lookedSkins[1].SetActive(false);
        }
        if (numberOfPlayGames > 100)
        {
            unlookedSkinsNumber++;
            lookedSkins[2].SetActive(false);
        }

        PlayerPrefs.SetInt("unlookedSkins",unlookedSkinsNumber);
        PlayerPrefs.Save();
    }
    private void Update()
    {
        Vector3 pContent = Camera.main.ScreenToWorldPoint(content.position);
        unlookedSkinsText.text = unlookedSkinsNumber + "/" + "4";

        if (pContent.x > 2.3f && pContent.x < 2.7f)
        {
            GetInfo(0,0);
        }
        if (pContent.x > 0.6f && pContent.x < 0.8f)
        {
            GetInfo(5,1);
        }
        if (pContent.x > -1.1f && pContent.x < -0.75f)
        {
            GetInfo(25, 2);
        }
        if (pContent.x < -2.5f)
        {
            GetInfo(100, 3);
        }
    }
    private void GetInfo(int npGames,int nSkin)
    {
        if (npGames > 0)
        {
            skinInfo.sprite = spriteSkins[nSkin];
            taskForUnllookSkin.text = "Play " + npGames + " games";
            numberOfPlayedGames.text = numberOfPlayGames + " / " + npGames;
        }
        else
        {
            skinInfo.sprite = spriteSkins[nSkin];
            taskForUnllookSkin.text = "Install Bounce Ball Blast";
            numberOfPlayedGames.text = numberOfPlayGames + " / " + npGames;
        }
    }
    public void Info()
    {
        infoPannel.SetBool("appear",true);
    }
    public void CloseInfoPannel()
    {
        infoPannel.SetBool("appear",false);
    }
}
