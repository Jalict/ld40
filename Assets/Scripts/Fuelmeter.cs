using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuelmeter : MonoBehaviour {
    public float fuel = 1;

    public float fuelUsage = 1;

    private Vector3 pos;
    private Transform fuelSprite;

	// Use this for initialization
	void Start ()
    {
        fuelSprite = transform.GetChild(0);
        pos = fuelSprite.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 t = fuelSprite.localScale;
        t.x = fuel;
        fuelSprite.localScale = t;

        fuel -= fuelUsage * Time.deltaTime;

        if (fuel <= 0)
            gameObject.SetActive(false);
	}
}
