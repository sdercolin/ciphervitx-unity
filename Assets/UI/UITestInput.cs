using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UITestInput : MonoBehaviour
{
    [SerializeField]
    SinglePlayerField Field;

    [SerializeField]
    Button NiceButton;

    [SerializeField]
    RequesterBase requester;

    private Card tmp = null;

    // Use this for initialization
    void Start()
    {
        Game.Initialize();
        Field.Init();

        if (NiceButton != null)
        {
            NiceButton.onClick.AddListener(delegate () {
                UILogger.LogYellow("nice!");
                NiceOperation();
            });
        }
    }

    private async void NiceOperation()
    {
        List<int> intList = new List<int>();
        intList.Add(4);
        intList.Add(2);
        intList.Add(1);
        List<int> result = await UIMainController.GetUIMainController().RequestChoose<int>(intList, 1, 2, "choose 1 or 2 number(s)");

        foreach (int i in result)
        {
            UILogger.LogYellow("chosen " + i.ToString());
        }
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
