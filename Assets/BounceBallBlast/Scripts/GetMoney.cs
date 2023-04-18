using UnityEngine;

public class GetMoney : MonoBehaviour {

    private int powerCoins;
    private float time;

    private void Start()
    {
        powerCoins = PlayerPrefs.GetInt("powerCoins");

        if (powerCoins == 0)
            powerCoins = 100;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GameManager.instace.time = Time.time + 0.5f;
            StartCoroutine(GameManager.instace.GetCoinSound());

            GameManager.instace.moneyPannel.GetComponent<Animator>().SetBool("addMoney",true);
            Debug.Log(powerCoins / 100);
            GameManager.instace.amountOfCoins +=  Mathf.RoundToInt(powerCoins / 100) * 15;
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "bullet")
        {
            //Destroy(collision.gameObject);
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
            


        PlayerPrefs.SetInt("amountOfCoins",GameManager.instace.amountOfCoins);
        PlayerPrefs.Save();
    }
}
