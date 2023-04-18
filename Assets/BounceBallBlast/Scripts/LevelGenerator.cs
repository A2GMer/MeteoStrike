using UnityEngine.UI;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    public Text level_left;
    public Text level_right;
    public Text levelCompletedText;
    public Text scoreLevelDone;
    public Image content;
    public Animator levelComplete;
    public GameObject destroyAllBalls;

    public static LevelGenerator instance { get; set; }

    [HideInInspector] public int harderStatusBalls;

    private int initialLevel;
    private int harderStatusLevel;
    private bool levelDone;
    private float timer;
    private bool checkTimer;
    private bool nextLevel;

	void Awake ()
    {
        initialLevel = PlayerPrefs.GetInt("level");
        harderStatusLevel = PlayerPrefs.GetInt("harderLevel");
        harderStatusBalls = PlayerPrefs.GetInt("harderBall");

        if (initialLevel == 0)
            initialLevel = 0;
        if (harderStatusBalls == 0)
            harderStatusBalls = 8;
        if (harderStatusLevel == 0)
            harderStatusLevel = 30;
	}
    private void Start()
    {
        instance = this;

        int n = initialLevel + 1;

        level_left.text = "" + initialLevel;
        level_right.text = "" + n;
    }
    void Update ()
    {
        BarStatus(harderStatusLevel);
	}
    void CreateLevel()
    {
        content.fillAmount = 0.0f;
        initialLevel++;

        harderStatusBalls = Mathf.RoundToInt(harderStatusBalls * 1.25f);
        harderStatusLevel = Mathf.RoundToInt(harderStatusLevel * 1.25f);

        int n = initialLevel + 1;

        level_left.text = "" + initialLevel;
        level_right.text = "" + n;

        PlayerPrefs.SetInt("level",initialLevel);
        PlayerPrefs.SetInt("harderLevel",harderStatusLevel);
        PlayerPrefs.SetInt("harderBalls",harderStatusBalls);
        PlayerPrefs.Save();
    }
    void BarStatus(int hLevel)
    {
        float value = (float)GameManager.instace.score / (float)hLevel;
        GameManager.instace.percentPassedLevel.text = "" + Mathf.Round(value * 100.0f) + "% completed";
        content.fillAmount = value;
        levelCompletedText.text = "Level " + initialLevel + " completed!";
        scoreLevelDone.text = "" + GameManager.instace.score;

        if (value >= 1.0f)
        {
            if (PlayerPrefs.GetInt("noAds") == 0 && ServicesManager.instance != null)
            {
                ServicesManager.instance.ShowInterstitialUnityAds();
                ServicesManager.instance.ShowInterstitialAdmob();
            }

            levelDone = true;
            if (!checkTimer)
            {
                timer = Time.time + 0.75f;
                checkTimer = true;
            }
            GameObject.Find("Player").GetComponent<Player>().enabled = false;
            destroyAllBalls.SetActive(true);
            levelComplete.SetBool("appear", true);

        }
        if (levelDone && Input.GetMouseButton(0) && timer < Time.time)
        {
            levelComplete.SetBool("appear", false);
            GameObject.Find("Player").GetComponent<Player>().enabled = true;
            destroyAllBalls.SetActive(false);
            levelDone = false;
            GameManager.instace.score = 0;
            checkTimer = false;

            GenerateEnemies.instance.numberAliveBalls = 0;

            CreateLevel();
        }
        if (Input.touchCount > 0)
        {
            if (levelDone && Input.GetTouch(0).phase == TouchPhase.Ended && timer < Time.time)
            {
                content.fillAmount = 0.0f;
                levelComplete.SetBool("appear", false);
                GameObject.Find("Player").GetComponent<Player>().enabled = true;
                destroyAllBalls.SetActive(false);
                levelDone = false;
                GameManager.instace.score = 0;
                checkTimer = false;

                GenerateEnemies.instance.numberAliveBalls = 0;

                CreateLevel();
            }
        }
    }
}
