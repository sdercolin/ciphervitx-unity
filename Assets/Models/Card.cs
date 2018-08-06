using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 所有卡的基类
/// </summary>
public abstract class Card
{
    public Card(User controller)
    {
        Controller = controller;
        Guid = System.Guid.NewGuid().ToString();
    }

    /// <summary>
    /// 卡的GuId，唯一识别卡的依据
    /// </summary>
    public string Guid;
    public override string ToString()
    {
        return "{\"guid\": \"" + Guid + "\" }";
    }
    
    /// <summary>
    /// 卡的序列号
    /// </summary>
    public string Serial { get; protected set; }

    #region 基本信息
    /// <summary>
    /// 卡包号
    /// </summary>
    public string Pack { get; protected set; }

    /// <summary>
    /// 编号
    /// </summary>
    public string CardNum { get; protected set; }

    /// <summary>
    /// 称号
    /// </summary>
    public string Title { get; protected set; }

    /// <summary>
    /// 单位名
    /// </summary>
    public string UnitName { get; protected set; }

    /// <summary>
    /// 当前具备的全部单位名
    /// </summary>
    public List<string> AllUnitNames
    {
        get
        {
            List<string> result = new List<string>();
            result.Add(UnitName);
            BuffList.ForEach(x =>
            {
                UnitNameBuff buff = x as UnitNameBuff;
                if (buff != null)
                {
                    if (buff.IsAdding)
                    {
                        result.Add(buff.Value);
                        return;
                    }
                    else
                    {
                        result.Remove(buff.Value);
                    }
                }
            });
            return result;
        }
    }

    /// <summary>
    /// 是否具备某个单位名
    /// </summary>
    /// <param name="unitName">单位名</param>
    /// <returns></returns>
    public bool HasUnitNameOf(string unitName)
    {
        return AllUnitNames.Contains(unitName);
    }

    /// <summary>
    /// 是否与某张卡具备相同单位名
    /// </summary>
    /// <param name="card">卡</param>
    /// <returns></returns>
    public bool HasSameUnitNameWith(Card card)
    {
        return !card.AllUnitNames.TrueForAll(x =>
       {
           return !HasUnitNameOf(x);
       });
    }

    /// <summary>
    /// 战斗力
    /// </summary>
    protected int power;
    public int Power
    {
        get
        {
            int powerNow = power;
            BuffList.ForEach(x =>
            {
                PowerBuff buff = x as PowerBuff;
                if (buff != null)
                {
                    powerNow += buff.Value;
                }
            });
            return powerNow;
        }
    }

    /// <summary>
    /// 支援力
    /// </summary>
    protected int support;
    public int Support
    {
        get
        {
            int supportNow = support;
            BuffList.ForEach(x =>
            {
                SupportBuff buff = x as SupportBuff;
                if (buff != null)
                {
                    supportNow += buff.Value;
                }
            });
            return supportNow;
        }
    }

    /// <summary>
    /// 出击费用
    /// </summary>
    protected int deployCost;
    public int DeployCost
    {
        get
        {
            int deployCostNow = deployCost;
            BuffList.ForEach(x =>
            {
                DeployCostBuff buff = x as DeployCostBuff;
                if (buff != null)
                {
                    if (buff.IsBecoming)
                    {
                        deployCostNow = buff.Value;
                        return;
                    }
                    else
                    {
                        deployCostNow += buff.Value;
                    }
                }
            });
            return deployCostNow;
        }
    }

    /// <summary>
    /// 转职费用，若无则为0
    /// </summary>
    protected int classChangeCost;
    public int ClassChangeCost
    {
        get
        {
            int classChangeCostNow = classChangeCost;
            BuffList.ForEach(x =>
            {
                ClassChangeCostBuff buff = x as ClassChangeCostBuff;
                if (buff != null)
                {
                    if (buff.IsBecoming)
                    {
                        classChangeCostNow = buff.Value;
                        return;
                    }
                    else
                    {
                        classChangeCostNow += buff.Value;
                    }
                }
            });
            return classChangeCostNow;
        }
    }

    /// <summary>
    /// 势力
    /// </summary>
    protected List<SymbolEnum> symbols = new List<SymbolEnum>();

    /// <summary>
    /// 是否具备某个势力
    /// </summary>
    /// <param name="symbol">势力</param>
    /// <returns></returns>
    public bool HasSymbol(SymbolEnum symbol)
    {
        bool hasNow = symbols.Contains(symbol);
        BuffList.ForEach(x =>
        {
            SymbolBuff buff = x as SymbolBuff;
            if (buff != null)
            {
                if (buff.IsAdding)
                {
                    hasNow = true;
                    return;
                }
                else
                {
                    hasNow = false;
                }
            }
        });
        return hasNow;
    }

    /// <summary>
    /// 性别
    /// </summary>
    protected List<GenderEnum> genders = new List<GenderEnum>();

    /// <summary>
    /// 是否具备某个性别
    /// </summary>
    /// <param name="gender">性别</param>
    /// <returns></returns>
    public bool HasGender(GenderEnum gender)
    {
        bool hasNow = genders.Contains(gender);
        BuffList.ForEach(x =>
        {
            GenderBuff buff = x as GenderBuff;
            if (buff != null)
            {
                if (buff.IsAdding)
                {
                    hasNow = true;
                    return;
                }
                else
                {
                    hasNow = false;
                }
            }
        });
        return hasNow;
    }

    /// <summary>
    /// 武器
    /// </summary>
    protected List<WeaponEnum> weapons = new List<WeaponEnum>();

    /// <summary>
    /// 是否具备某个武器
    /// </summary>
    /// <param name="weapon">武器</param>
    /// <returns></returns>
    public bool HasWeapon(WeaponEnum weapon)
    {
        bool hasNow = weapons.Contains(weapon);
        BuffList.ForEach(x =>
        {
            WeaponBuff buff = x as WeaponBuff;
            if (buff != null)
            {
                if (buff.IsAdding)
                {
                    hasNow = true;
                    return;
                }
                else
                {
                    hasNow = false;
                }
            }
        });
        return hasNow;
    }

    /// <summary>
    /// 属性
    /// </summary>
    protected List<TypeEnum> types = new List<TypeEnum>();

    /// <summary>
    /// 是否具备某个属性
    /// </summary>
    /// <param name="type">属性</param>
    /// <returns></returns>
    public bool HasType(TypeEnum type)
    {
        bool hasNow = types.Contains(type);
        BuffList.ForEach(x =>
        {
            TypeBuff buff = x as TypeBuff;
            if (buff != null)
            {
                if (buff.IsAdding)
                {
                    hasNow = true;
                    return;
                }
                else
                {
                    hasNow = false;
                }
            }
        });
        return hasNow;
    }

    /// <summary>
    /// 射程
    /// </summary>
    protected List<RangeEnum> ranges = new List<RangeEnum>();

    /// <summary>
    /// 是否具备某个射程
    /// </summary>
    /// <param name="range">射程</param>
    /// <returns></returns>
    public bool HasRange(RangeEnum range)
    {
        bool hasNow = ranges.Contains(range);
        BuffList.ForEach(x =>
        {
            RangeBuff buff = x as RangeBuff;
            if (buff != null)
            {
                if (buff.IsAdding)
                {
                    hasNow = true;
                    return;
                }
                else
                {
                    hasNow = false;
                }
            }
        });
        return hasNow;
    }
    #endregion

    #region 卡片状态
    /// <summary>
    /// 控制者
    /// </summary>
    public User Controller { get; protected set; }

    /// <summary>
    /// 卡下方所叠放的卡
    /// </summary>
    protected List<Card> stacks = new List<Card>();

    /// <summary>
    /// 卡叠放时的顶端卡，null表示自身在顶端
    /// </summary>
    protected Card stackTop = null;

    /// <summary>
    /// 等级
    /// </summary>
    public int Level
    {
        get
        {
            return stacks.Count + 1;
        }
    }

    /// <summary>
    /// 是否已升级
    /// </summary>
    public bool IsLevelUped
    {
        get
        {
            return Level > 1;
        }
    }

    /// <summary>
    /// 是否已转职
    /// </summary>
    public bool IsClassChanged
    {
        get
        {
            return IsLevelUped && ClassChangeCost > 0;
        }
    }

    /// <summary>
    /// 卡是否横置
    /// </summary>
    public bool IsHorizontal = false;

    /// <summary>
    /// 卡是否正面朝上
    /// </summary>
    public bool FrontShown = false;

    /// <summary>
    /// 卡是否公开
    /// </summary>
    public bool Visible = false;

    /// <summary>
    /// 卡是否为主人公
    /// </summary>
    public bool IsHero = false;

    /// <summary>
    /// 卡被击破的计数，2以上对应主人公被击破时所破坏的宝玉数量
    /// </summary>
    public int DestroyedCount = 0;

    /// <summary>
    /// 卡是否在场
    /// </summary>
    /// <returns></returns>
    public bool IsOnField
    {
        get
        {
            return BelongedRegion is FrontField || BelongedRegion is BackField;
        }
    }

    /// <summary>
    /// 获取卡所在的区域
    /// </summary>
    /// <returns>卡所在的区域</returns>
    public Area BelongedRegion
    {
        get
        {
            foreach (Area area in Controller.AllAreas)
            {
                if (area.Contains(this))
                {
                    return area;
                }
            }
            return null;
        }
    }

    /// <summary>
    /// 附加列表
    /// </summary>
    public List<IAttachable> AttachableList = new List<IAttachable>();

    /// <summary>
    /// 能力列表
    /// </summary>
    public List<Skill> SkillList
    {
        get
        {
            List<Skill> skillList = new List<Skill>();
            foreach (var item in AttachableList)
            {
                if (item is Skill && !(item is SubSkill))
                {
                    skillList.Add(item as Skill);
                }
            }
            return skillList;
        }
    }

    /// <summary>
    /// 附加值列表
    /// </summary>
    public List<Buff> BuffList
    {
        get
        {
            List<Buff> buffList = new List<Buff>();
            foreach (var item in AttachableList)
            {
                if (item is Buff)
                {
                    buffList.Add(item as Buff);
                }
            }
            return buffList;
        }
    }

    /// <summary>
    /// 附加能力列表
    /// </summary>
    public List<SubSkill> SubSkillList
    {
        get
        {
            List<SubSkill> subSkillList = new List<SubSkill>();
            foreach (var item in AttachableList)
            {
                if (item is SubSkill)
                {
                    subSkillList.Add(item as SubSkill);
                }
            }
            return subSkillList;
        }
    }
    #endregion

    #region 卡片动作
    /// <summary>
    /// 移动至某区域
    /// </summary>
    /// <param name="des">目标区域</param>
    /// <param name="withStack">是否连同叠放卡</param>
    public void MoveTo(Area des, bool withStack = true)
    {
        MoveTo(des, des.Count, withStack);
    }

    /// <summary>
    /// 移动至特定卡前
    /// </summary>
    /// <param name="card">参考卡</param>
    /// <param name="withStack">是否连同叠放卡</param>
    public void MoveTo(Card card, bool withStack = true)
    {
        MoveTo(card.BelongedRegion, card.BelongedRegion.IndexOf(card), withStack);
    }

    /// <summary>
    /// 移动至某区域的特定位置
    /// </summary>
    /// <param name="des">目标区域</param>
    /// <param name="pos">特定位置</param>
    /// <param name="withStack">是否连同叠放卡</param>
    public void MoveTo(Area des, int pos, bool withStack = true)
    {
        if (withStack)
        {
            stacks.Reverse();
            stacks.ForEach(x =>
            {
                x.stackTop = null;
                x.MoveTo(des, pos, false);
            });
            stacks.Clear();
        }
        BelongedRegion.RemoveCard(this);
        des.AddCard(this, pos);
    }

    /// <summary>
    /// 叠放到上方
    /// </summary>
    /// <param name="card">对象卡</param>
    public void StackUnder(Card card)
    {
        card.MoveTo(Controller.Overlay);
        card.stacks.Add(this);
        stackTop = card;
        stacks.ForEach(x =>
        {
            card.stacks.Add(x);
            x.stackTop = card;
        });
        stacks.Clear();
    }

    /// <summary>
    /// 叠放到下方
    /// </summary>
    /// <param name="card">对象卡</param>
    public void StackOver(Card card)
    {
        MoveTo(card);
        card.MoveTo(Controller.Overlay);
        stacks.Add(card);
        card.stackTop = this;
        card.stacks.ForEach(x =>
        {
            stacks.Add(x);
            x.stackTop = this;
        });
        card.stacks.Clear();
    }

    /// <summary>
    /// 添加附加
    /// </summary>
    /// <param name="item">要添加的对象</param>
    public void Attach(IAttachable item)
    {
        AttachableList.Add(item);
        item.Owner = this;
        item.Attached();
    }

    /// <summary>
    /// 重置所有状态
    /// </summary>
    public void Reset()
    {
        DestroyedCount = 0;
        IsHorizontal = false;
        FrontShown = true;
        Visible = true;
        foreach (var item in BuffList)
        {
            item.Detach();
        }
        foreach (var item in SubSkillList)
        {
            item.Detach();
        }
    }
    #endregion

    #region 别名
    /// <summary>
    /// 我方
    /// </summary>
    public User Player { get { return Controller; } }

    /// <summary>
    /// 对手
    /// </summary>
    public User Rival { get { return Controller.Opponent; } }
    #endregion

    /// <summary>
    /// 卡片接受消息
    /// </summary>
    /// <param name="message">接受到的消息</param>
    public void Read(Message message)
    {
        ListUtils.Clone(AttachableList).ForEach(item =>
       {
           item.Read(message);
       });
    }

    /// <summary>
    /// 询问卡片是否允许某操作
    /// </summary>
    /// <param name="message">表示该操作的消息</param>
    /// <param name="substitute">拒绝该操作时表示作为代替的动作的的消息</param>
    /// <returns>如允许，则返回True</returns>
    public bool Try(Message message, ref Message substitute)
    {
        foreach (var item in AttachableList)
        {
            if (!item.Try(message, ref substitute))
            {
                return false;
            }
        }
        return true;
    }
}

/// <summary>
/// 势力
/// </summary>
public enum SymbolEnum
{
    Red, //光之剑
    Blue, //圣痕
    White, //白夜
    Black, //暗夜
    Green, //纹章
    Purple //神器
}

/// <summary>
/// 性别
/// </summary>
public enum GenderEnum
{
    Male, //男
    Female //女
}

/// <summary>
/// 武器
/// </summary>
public enum WeaponEnum
{
    Sword, //剑
    Lance, //枪
    Axe, //斧
    Bow, //弓
    Magic, //魔法
    Staff, //杖
    DragonStone, //龙石
    Knife, //暗器
    Strike //牙
}

/// <summary>
/// 属性
/// </summary>
public enum TypeEnum
{
    Armor, //重甲
    Flight, //飞行
    Beast, //兽马
    Dragon, //龙
    Phantom, //幻影
    Monster //魔物
}

/// <summary>
/// 射程
/// </summary>
public enum RangeEnum
{
    None, //-
    One, //1
    Two, //2
    Three, //3
    OnetoTwo, //1-2
    OnetoThree //1-3
}