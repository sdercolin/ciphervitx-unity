using UnityEngine;
using System.Collections;

public class UITestInput : MonoBehaviour
{
    [SerializeField]
    SinglePlayerField Field;

    private Card tmp = null;

    // Use this for initialization
    void Start()
    {
        Game.Initialize();
        Field.Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            tmp = CardFactory.CreateCard(1, Game.Player);
            Field.DeployCardToBattleField(tmp, true);
        }

        if(Input.GetKeyDown(KeyCode.G))
        {
            Field.DeployCardToBattleField(CardFactory.CreateCard(20, Game.Player), false);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            Card tmp2 = CardFactory.CreateCard(2, Game.Player);
            Field.UpdateCardOfBattleField(tmp, tmp2);
            tmp = tmp2;
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            Field.TapCard(tmp);
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            Field.UntapCard(tmp);
        }
    }
}
