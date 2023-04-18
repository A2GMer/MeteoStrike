using UnityEngine;

public class Bullet : MonoBehaviour {

	void Start ()
    {
        string activeCannon = PlayerPrefs.GetString("active_cannon", "null");

        if (activeCannon == "null")
            activeCannon = "cannon_1";

        if (activeCannon != "cannon_5")
        {
            Player.instance.activeBulletCannon5 = false;

            SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
            sr.sprite = Resources.Load<Sprite>("Bullets/" + activeCannon + "_b");
        }
        else
            Player.instance.activeBulletCannon5 = true;
    }

}
