using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone : MonoBehaviour {

    private Quaternion baseRotation;
    private Quaternion oldRotation;

    public bool flipped = false;

    private float secondTimer = 0;
    private bool rotating = false;


	// Use this for initialization
	void Start () {
        baseRotation = GetComponent<Transform>().rotation;
        GetComponent<Transform>().Rotate(new Vector3(1, 0, -1), 90);
        oldRotation = GetComponent<Transform>().rotation;
	}
	
	// Update is called once per frame
	void Update () {
        if (rotating == true)
        {
            GetComponent<Transform>().rotation = Quaternion.Lerp(oldRotation, baseRotation, secondTimer);
            
            if (secondTimer > 1)
            {
                rotating = false;
            }
        }
        secondTimer += Time.deltaTime;
    }

    public void startRotation()
    {
        secondTimer = 0.0f;
        rotating = true;
        flipped = true;
    }
}
