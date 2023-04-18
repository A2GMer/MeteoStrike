using UnityEngine;

public class SetUpPlayer : MonoBehaviour {

	void FixedUpdate ()
    {
        string activeCannon = PlayerPrefs.GetString("active_cannon", "null");

        if (activeCannon == "null")
            activeCannon = "cannon_1";

        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
        sr.sprite = Resources.Load<Sprite>("Cannons/" + activeCannon);
    }
	

}
