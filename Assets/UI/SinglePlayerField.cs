using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SinglePlayerField : UIBehaviour
{
    private CardListBond m_BondArea;
    private CardListH m_FrontArea;
    private CardListH m_BackArea;
    private CardListV m_OrbArea;
    private CardListH m_HandArea;
    private SingleCardPicture m_RetreatArea;
    private SingleCardPictureWithNum m_DeckArea;

    [SerializeField]
    private bool m_IsPlayer;

    private bool m_IsInited = false;
    public void Init()
    {
        if (m_IsInited)
        {
            return;
        }
        m_IsInited = true;

        m_BondArea = GetComponentInChildByName<CardListBond>("Bond");
        m_FrontArea = GetComponentInChildByName<CardListH>("Front");
        m_BackArea = GetComponentInChildByName<CardListH>("Back");
        m_OrbArea = GetComponentInChildByName<CardListV>("Orb");
        m_HandArea = GetComponentInChildByName<CardListH>("Hand");
        m_RetreatArea = GetComponentInChildByName<SingleCardPicture>("Retreat");
        m_DeckArea = GetComponentInChildByName<SingleCardPictureWithNum>("Deck");
    }

    private T GetComponentInChildByName<T>(string name)
    {
        return transform.Find(name).GetComponent<T>();
    }

    //开放接口来操作区域，不要开放区域
    //增加手牌
    //单位移动
    //横置重置
    //CardListItem上绑定Card对象，需要操作时跳过区域，查询可用的操作
    //如果绑定Card对象，那么不需要用SetFront(string)等方法来设置Image，只需要ShowFront(bool)，Card中已经有足够的信息

    public void DeployCardToBattleField(Card card, bool toFront)
    {
        CardListH list = toFront ? m_FrontArea : m_BackArea;
        CardListItem listItem = list.AddListItem();
        listItem.Card = card;
    }

    private CardListItem SearchCardInBattleField(Card card)
    {
        CardListItem listItem = m_FrontArea.SearchCard(card);
        if (listItem == null)
        {
            listItem = m_BackArea.SearchCard(card);
        }
        return listItem;
    }

    public void UpdateCardOfBattleField(Card from, Card to)
    {
        CardListItem listItem = SearchCardInBattleField(from);
        if (listItem != null)
        {
            listItem.Card = to;
        }
        else
        {
            UILogger.LogError("can not find card when update card, " + from.ToString() + "  " + to.ToString());
        }
    }

    public void TapCard(Card card)
    {
        CardListItem listItem = SearchCardInBattleField(card);
        if (listItem != null)
        {
            listItem.Tap();
        }
        else
        {
            UILogger.LogError("can not find card when tap card, " + card.ToString());
        }
    }

    public void UntapCard(Card card)
    {
        CardListItem listItem = SearchCardInBattleField(card);
        if (listItem != null)
        {
            listItem.Untap();
        }
        else
        {
            UILogger.LogError("can not find card when untap card, " + card.ToString());
        }
    }

    public void MoveCardOfBattleField(Card card, bool toFront)
    {
        CardListH from = toFront ? m_BackArea : m_FrontArea;
        CardListItem listItem = from.SearchCard(card);
        if (listItem != null)
        {
            from.RemoveItem(listItem);
            CardListH to = toFront ? m_FrontArea : m_BackArea;
            CardListItem newItem = to.AddListItem();
            newItem.Card = card;
        }
        else
        {
            UILogger.LogError("can not find card when move card, " + card.ToString());
        }
    }
}
