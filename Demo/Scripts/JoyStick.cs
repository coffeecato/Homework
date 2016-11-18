using UnityEngine;
using System.Collections;

public class JoyStick : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public Vector2 GetJoyStickAxis()
    {
        Vector2 vec2 = new Vector2();

        return vec2;
    }

    public void OnMouseClick()
    {
        Debug.LogWarning("JoyStick OnMouseClick");
    }

    public void OnPress()
    {
        Debug.LogWarning("JoyStick OnPress");
    }

}
