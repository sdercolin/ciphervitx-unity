using UnityEngine;
using System.Collections;

public class UITestInput : MonoBehaviour
{
    [SerializeField]
    SinglePlayerField Field;

    // Use this for initialization
    void Start()
    {
        Field.Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            //Field.DeployCardToBattleField(new Card00001())
        }
    }
}
