using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMain : MonoBehaviour {

    [SerializeField]
    private CardListBase front;
    [SerializeField]
    private CardListBase back;
    [SerializeField]
    private CardListBase orb;
    [SerializeField]
    private CardListBase bond;
    [SerializeField]
    private CardListBase enemy;

    // Use this for initialization
    void Start () {
        front.SetClick((item) => item.Tap());
        back.SetClick((item) => item.Tap());
        enemy.SetClick((item) => item.Tap());
        bond.SetClick((item => item.TurnBack()));
        orb.SetClick((item => item.TurnFront("largeImg2")));
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.F))
        {
            front.AddListItem();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            back.AddListItem();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            enemy.AddListItem();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            orb.AddListItem();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            bond.AddListItem();
        }
    }
}
