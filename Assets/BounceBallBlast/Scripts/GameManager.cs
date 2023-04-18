using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager instace { get; set; }

    [Header("Objects of game")]
    public GameObject gameOver;
    public GameObject destroyAllBalls;
    public GameObject moneyPannel;
    public GameObject notEnoughMoney;
    public Image downMenuBar;
    public GameObject fireSpeedData;
    public GameObject firePowerData;
    public GameObject coinsData;
    public Image sound;
    public Image vibration;

    [Header("Animator of game")]
    public Animator skinShop;
    public Animator cannonsShop;
    public Animator settingsPannel;

    [Header("UI Texts of game")]
    public Text levelNumber;
    public Text textMoney;
    public Text textScore;
    public Text fireSpeed;
    public Text firePower;
    public Text coinsPower;
    public Text upgradePrice;
    public Text gameOverScoreText;
    public Text percentPassedLevel;
    public Text levelDoneText;

    [Header("Sprites of game")]
    public Sprite blueBar;
    public Sprite yellowBar;
    public Sprite redBar;
    public Sprite soundOn;
    public Sprite soundOff;
    public Sprite vibrationOn;
    public Sprite vibrationOff;

    [Header("Audio clips of game")]
    public AudioClip cannonShoot;
    public AudioClip balDestruction;
    public AudioSource ballDestruction;
    public AudioClip getCoin;
    public AudioClip click;

    [HideInInspector] public int amountOfCoins;
    [HideInInspector] public int score;
    [HideInInspector] public float timeGameOver;
    [HideInInspector] public int priceForSpeed;
    [HideInInspector] public int priceForPower;
    [HideInInspector] public int priceForCoins;
    [HideInInspector] public bool gOver;
    [HideInInspector] public float time;
    [HideInInspector] public bool shop;


    private int initialSpeed;
    private int initialPower;
    private int initialCoins;
    private int pUpgrade;
    private double convertCoins;
    private AudioSource soundAudio;
    private bool uSpeed,uPower,uCoins;

    private void Awake()
    {
        Initialize();
       
    }
    void Start ()
    {
        Debug.Log(PlayerPrefs.GetInt("powerBullet"));

        instace = this;
        Initialize();

        PlayerPrefs.SetInt("amountOfCoins", 10000);
        PlayerPrefs.Save();

        //Add Audio Source to Game Manager
        soundAudio = gameObject.AddComponent<AudioSource>(); 

        //Set up baground of game at random
        if(SceneManager.GetActiveScene().name == "menu")
        {
            levelNumber.text = "Level " + PlayerPrefs.GetInt("level");

            if (PlayerPrefs.GetInt("unlookedSkins") == 0)
            {
                PlayerPrefs.SetInt("unlookedSkins",2);
                PlayerPrefs.Save();
            }

            int randBack = (int)Random.Range(1, PlayerPrefs.GetInt("unlookedSkins") + 1);
            

            SpriteRenderer sr = GameObject.Find("Background").GetComponent<SpriteRenderer>() as SpriteRenderer;
            sr.sprite = Resources.Load<Sprite>("Skins/skin_" + randBack);

            if (PlayerPrefs.GetInt("level") == 0)
                PlayerPrefs.SetInt("level",1);

            PlayerPrefs.SetString("iBack","skin_"+randBack);
            PlayerPrefs.Save();
        }
        if (SceneManager.GetActiveScene().name == "main")
        {
            SpriteRenderer sr = GameObject.Find("Background").GetComponent<SpriteRenderer>() as SpriteRenderer;
            sr.sprite = Resources.Load<Sprite>("Skins/"+PlayerPrefs.GetString("iBack"));
        }

        if (PlayerPrefs.GetInt("noAds") == 0 && ServicesManager.instance != null)
        {
            ServicesManager.instance.InitializeAdmob();
            ServicesManager.instance.InitializeUnityAds();
        }

        PlayerPrefs.Save();
    }
    private void FixedUpdate()
    {
        float screenMin = Screen.height / 4f;
        float screenMax = Screen.height / 1.25f;

        Debug.Log(PlayerPrefs.GetFloat("powerBullet"));

        //Update data of game each grame
        UpdateData();

        if (time < Time.time)
            moneyPannel.GetComponent<Animator>().SetBool("addMoney", false);
        //Start game when swipe with finger
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved && SceneManager.GetActiveScene().name == "menu" && !shop && Input.GetTouch(0).position.y > screenMin && Input.GetTouch(0).position.y < screenMax)
        {
            StartCoroutine(ChangeLevel("main"));
        }
    }
    //Data for fire speed of cannon bullet you can see it in the bottom of screen
    public void FireSpeed()
    {
        downMenuBar.GetComponent<Image>().sprite = redBar;

        firePowerData.SetActive(false);
        fireSpeedData.SetActive(true);
        coinsData.SetActive(false);

        uSpeed = true;
        uPower = false;
        uCoins = false;

        pUpgrade = PlayerPrefs.GetInt("pSpeed");
    }
    //Data for power destroy of cannon bullet you can see it in the bottom of screen
    public void PowerSpeed()
    {
        pUpgrade = priceForPower;

        firePowerData.SetActive(true);
        fireSpeedData.SetActive(false);
        coinsData.SetActive(false);

        uSpeed = false;
        uPower = true;
        uCoins = false;

        downMenuBar.GetComponent<Image>().sprite = yellowBar;
        pUpgrade = PlayerPrefs.GetInt("pPower");
    }
    //Data for power coin if you upgrade it from one coins you will get more money
    public void CoinPower()
    {
        pUpgrade = priceForCoins;

        firePowerData.SetActive(false);
        fireSpeedData.SetActive(false);
        coinsData.SetActive(true);

        uSpeed = false;
        uPower = false;
        uCoins = true;

        downMenuBar.GetComponent<Image>().sprite = blueBar;
        pUpgrade = PlayerPrefs.GetInt("pCoins");
    }
    //Function which responds about upgrade propreties of cannon,this function is public ,because we put it when we click on upgrade button
    public void Upgrade()
    {
        if (uSpeed)
            UpgradePropreties("pSpeed","iSpeed","speedBullet",2);
        if (uPower)
            UpgradePropreties("pPower", "iPower", "powerBullet", 20);
        if (uCoins)
            UpgradePropreties("pCoins", "iCoins", "powerCoins", 50);
    }
    //Function which responds about upgrade propreties of cannon
    private void UpgradePropreties(string priceName,string initialName,string nameProperty, int increment)
    {
        if (PlayerPrefs.GetInt(priceName) <= amountOfCoins)
        {
            amountOfCoins -= PlayerPrefs.GetInt(priceName);

            if(PlayerPrefs.GetFloat(nameProperty) == 0 && nameProperty == "powerBullet")
            {
                PlayerPrefs.SetFloat("powerBullet",1f);
                PlayerPrefs.Save();
            }

            PlayerPrefs.SetFloat(nameProperty, (PlayerPrefs.GetFloat(nameProperty) * 0.2f) + PlayerPrefs.GetFloat(nameProperty));
            PlayerPrefs.SetInt(priceName, PlayerPrefs.GetInt(priceName) * 3);

            int speed = PlayerPrefs.GetInt(initialName) + increment;
            fireSpeed.text = "" + speed;
            PlayerPrefs.SetInt(initialName, speed);

            if(nameProperty == "powerCoins")
                PlayerPrefs.SetInt(nameProperty,speed);

            PlayerPrefs.SetInt("amountOfCoins",amountOfCoins);
        }
        pUpgrade = PlayerPrefs.GetInt(priceName);
        PlayerPrefs.Save();
    }
    //Convert money to k format
    public double ConvertToThousands(int amount)
    {
        double x = (double)amount / 1000f;
        double convert = System.Math.Round(x, 1);

        return convert;
    }
    //Intialiaze all data from start
    private void Initialize()
    {
        uSpeed = true;
        gOver = false;

        if(PlayerPrefs.GetFloat("powerBullet") == 0)
        {
            PlayerPrefs.SetFloat("powerBullet", 1f);
            PlayerPrefs.Save();
        }
        if (PlayerPrefs.GetFloat("speedBullet",0) == 0)
        {
            PlayerPrefs.SetFloat("speedBullet", 2000f);
        }

        if (PlayerPrefs.GetInt("pSpeed",0) == 0)
        {
            PlayerPrefs.SetInt("pSpeed", 7);
        }
        else
            priceForSpeed = PlayerPrefs.GetInt("pSpeed");
        if (PlayerPrefs.GetInt("pPower",0) == 0)
        {
            PlayerPrefs.SetInt("pPower", 15);
        }
        else
            priceForPower = PlayerPrefs.GetInt("pPower");
        if (PlayerPrefs.GetInt("pCoins",0) == 0)
        {
            PlayerPrefs.SetInt("pCoins", 11);
        }
        else
            priceForCoins = PlayerPrefs.GetInt("pCoins");

        //Initialize initial speed of bullet
        if (PlayerPrefs.GetInt("iSpeed") == 0)
        {
            PlayerPrefs.SetInt("iSpeed", 20);
        }
        if (PlayerPrefs.GetInt("iPower") == 0)
        {
            PlayerPrefs.SetInt("iPower", 100);
        }
        if (PlayerPrefs.GetInt("iCoins") == 0)
        {
            PlayerPrefs.SetInt("iCoins", 100);
        }

        priceForCoins = PlayerPrefs.GetInt("pCoins");
        priceForSpeed = PlayerPrefs.GetInt("pSpeed");
        priceForPower = PlayerPrefs.GetInt("pPower");

        upgradePrice.text = "" + priceForSpeed;
        fireSpeed.text = "" + PlayerPrefs.GetInt("iSpeed");

        amountOfCoins = PlayerPrefs.GetInt("amountOfCoins");

        pUpgrade = priceForSpeed;
    }
    //Update data of game each frame
    private void UpdateData()
    {
        textScore.text = "" + score;

        if (gOver)
        {
            if (Input.GetMouseButtonDown(0) && timeGameOver < Time.time)
            {
                StartCoroutine(ChangeLevel("menu"));
            }
        }
        if (pUpgrade >= 1000)
        {
            upgradePrice.text = "" + ConvertToThousands(pUpgrade) + "k";
        }
        else
            upgradePrice.text = "" + pUpgrade;

        fireSpeed.text = "" + PlayerPrefs.GetInt("iSpeed");
        firePower.text = "" + PlayerPrefs.GetInt("iPower");
        coinsPower.text = "" + PlayerPrefs.GetInt("iCoins");

        if (amountOfCoins >= 1000)
        {
            textMoney.text = "" + ConvertToThousands(amountOfCoins) + "k";
        }
        else
            textMoney.text = "" + amountOfCoins;

        if (pUpgrade < amountOfCoins)
            notEnoughMoney.SetActive(false);
        else
            notEnoughMoney.SetActive(true);
    }
    public void SkinShopAppear()
    {
        if (PlayerPrefs.GetInt("noAds") == 0)
        {
            ServicesManager.instance.ShowInterstitialUnityAds();
            ServicesManager.instance.ShowInterstitialAdmob();
        }

        shop = true;
        skinShop.SetBool("appear",true);
    }
    public void SkinShopDisappear()
    {
        shop = false;
        skinShop.SetBool("appear", false);
    }
    public void CannonsShopAppear()
    {
        if (PlayerPrefs.GetInt("noAds") == 0)
        {
            ServicesManager.instance.ShowInterstitialUnityAds();
            ServicesManager.instance.ShowInterstitialAdmob();
        }

        shop = true;
        cannonsShop.SetBool("appear",true);
    }
    public void CannonsShopDisappear()
    {
        shop = false;
        cannonsShop.SetBool("appear", false);
    }
    public void Sound()
    {
        if (sound.sprite == soundOn)
        {
            PlayerPrefs.SetInt("sound", 1);
            sound.sprite = soundOff;
        }
        else
        {
            PlayerPrefs.SetInt("sound", 1);
            sound.sprite = soundOn;
        }
        PlayerPrefs.Save();
    }
    public void NoAds()
    {
#if UNITY_IAP
        IAPManager.instance.BuyNoAds()
#endif
    }
    public void Vibration()
    {
        if (vibration.sprite == vibrationOn)
        {
            PlayerPrefs.SetInt("vibration", 0);
            vibration.sprite = vibrationOff;
        }
        else
        {
            PlayerPrefs.SetInt("vibration", 1);
            vibration.sprite = vibrationOn;
        }
    }
    public void Settings()
    {
        if(!settingsPannel.GetBool("appear"))
            settingsPannel.SetBool("appear",true);
        else
            settingsPannel.SetBool("appear", false);
    }
    public void GameOver()
    {
        if (PlayerPrefs.GetInt("noAds") == 0)
        {
            if (ServicesManager.instance != null)
            {
                ServicesManager.instance.ShowInterstitialUnityAds();
                ServicesManager.instance.ShowInterstitialAdmob();
            }
        }

        destroyAllBalls.SetActive(true);
        GameObject.Find("Player").GetComponent<Player>().enabled = false;
        gameOverScoreText.text = "" + score;
        gameOver.GetComponent<Animator>().SetBool("appear",true);

        PlayerPrefs.SetInt("numberOfPlayedGames", PlayerPrefs.GetInt("numberOfPlayedGames")+1);
        PlayerPrefs.Save();
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
    public IEnumerator ChangeLevel(string levelName)
    {
        float fadeTime = GameObject.Find("Main Camera").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);

        SceneManager.LoadScene(levelName);
    }
    public void StartGame()
    {
        StartCoroutine(ChangeLevel("main"));
    }
    public IEnumerator CannonShootSound()
    {
        soundAudio.clip = cannonShoot;
        soundAudio.Play();

        yield return new WaitWhile(() => soundAudio.isPlaying);
    }
    public void BallDestructionSound()
    {
        ballDestruction.clip = balDestruction;
        ballDestruction.Play();
    }
    public IEnumerator GetCoinSound()
    {
        soundAudio.clip = getCoin;
        soundAudio.Play();

        yield return new WaitWhile(() => soundAudio.isPlaying);
    }
    public void CLick()
    {
        soundAudio.clip = click;
        soundAudio.Play();
    }
}
