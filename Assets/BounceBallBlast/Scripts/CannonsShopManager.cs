using UnityEngine;
using UnityEngine.UI;

public class CannonsShopManager : MonoBehaviour
{

    public GameObject[] lookedCannons;
    public RectTransform content;
    public Text priceCannon;
    public Text numberOfCannons;

    private int unlookedCannonsNumber;
    private bool unlook;
    private int price;
    private int amountOfCoins;
    private string activeCannon;

    void Start()
    {
       // PlayerPrefs.DeleteAll();
       // PlayerPrefs.SetInt("amountOfCoins",2500);

        price = PlayerPrefs.GetInt("priceCannon");
        amountOfCoins = PlayerPrefs.GetInt("amountOfCoins");
        unlookedCannonsNumber = PlayerPrefs.GetInt("cannonsNumber");

        if (price == 0)
            price = 200;

        for(int i =0; i < lookedCannons.Length;i++)
            if (PlayerPrefs.GetInt(lookedCannons[i].name) == 1)
                lookedCannons[i].SetActive(false);

        if (unlookedCannonsNumber == 0)
            unlookedCannonsNumber = 1;
    }
    private void Update()
    {
        Vector3 pContent = Camera.main.ScreenToWorldPoint(content.position);
        numberOfCannons.text = unlookedCannonsNumber + "/6";

        if (price < 1000)
            priceCannon.text = "" + price;
        else
            priceCannon.text = "" + GameManager.instace.ConvertToThousands(price) + "k";

        if (pContent.x > 2.4f && pContent.x < 2.7f && lookedCannons[0].activeSelf)
        {
            if (unlook)
            {
                UnlookCannon(lookedCannons[0], price, amountOfCoins);
                unlook = false;
            }
        }
        if (pContent.x > 0.6f && pContent.x < 1.0f && lookedCannons[1].activeSelf)
        {
            if (unlook)
            {
                UnlookCannon(lookedCannons[1], price, amountOfCoins);
                unlook = false;
            }
        }
        if (pContent.x > -1.1f && pContent.x < -0.8f && lookedCannons[2].activeSelf)
        {
            if (unlook)
            {
                UnlookCannon(lookedCannons[2], price, amountOfCoins);
                unlook = false;
            }
        }
        if (pContent.x > -2.8f && pContent.x < -2.5f && lookedCannons[3].activeSelf)
        {
            if (unlook)
            {
                UnlookCannon(lookedCannons[3], price, amountOfCoins);
                unlook = false;
            }
        }
        if (pContent.x < -4.3f && lookedCannons[4].activeSelf)
        {
            if (unlook)
            {
                UnlookCannon(lookedCannons[4], price, amountOfCoins);
                unlook = false;
            }
        }
    }
    private void UnlookCannon(GameObject lookedCannon,int iPrice,int money)
    {
        Debug.Log(iPrice);
        PlayerPrefs.SetInt("test",10);
        if (iPrice <= money)
        {
            lookedCannon.SetActive(false);
            amountOfCoins -= iPrice;
            PlayerPrefs.SetInt("amountOfCoins",amountOfCoins);
            PlayerPrefs.SetInt(lookedCannon.name,1);

            price = price * 3;
            unlookedCannonsNumber++;

            PlayerPrefs.SetInt("priceCannon",price);
            PlayerPrefs.SetInt("cannonsNumber",unlookedCannonsNumber);
            PlayerPrefs.SetInt("amountOfCoins",amountOfCoins);
            PlayerPrefs.Save();
        }
    }
    public void UnlookButton()
    {
        unlook = true;
    }
    public void ChangeCannon(string name)
    {
        PlayerPrefs.SetString("active_cannon",name);
        PlayerPrefs.Save();
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}
