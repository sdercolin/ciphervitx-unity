using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Threading;

public class RequesterBase : UIBehaviour
{
    [SerializeField]
    private UIListBase buttonList;

    [SerializeField]
    private Button buttonConfirm;

    [SerializeField]
    private Text textDesc;

    protected volatile bool success = false;

    protected volatile List<int> results = new List<int>();

    protected int min = 1;
    protected int max = 1;

    public async Task<List<T>> Request<T>(List<T> choices, int min, int max, string desc)
    {
        this.min = min;
        this.max = max;
        textDesc.text = desc;
        SetText(choices);
        List<int> results = await StartRequest();

        List<T> ret = new List<T>();
        foreach (int index in results)
        {
            ret.Add(choices[index]);
        }
        return ret;
    }

    protected void SetText<T>(List<T> choices)
    {
        buttonList.Clear();
        int n = choices.Count;
        for (int i = 0; i < n; i++)
        {
            T choice = choices[i];
            RequestItem item = (RequestItem)buttonList.AddListItem();
            item.SetText(choice.ToString());
        }
    }

    protected async Task<List<int>> StartRequest()
    {
        results.Clear();
        var task = Task.Run(() =>
        {
            while (!success) { }
        });
        await task;
        success = false;
        return results;
    }

    protected override void Awake()
    {
        buttonConfirm.onClick.AddListener(delegate ()
        {
            if (!success)
            {
                List<int> selectedIndex = buttonList.GetSelectedItemsIndex();
                int count = selectedIndex.Count;
                if (count < min || count > max)
                {
                    UILogger.LogError("request min: " + min + ", max: " + max + ", but select " + count + " items.");
                    return;
                }
                else
                {
                    results.AddRange(selectedIndex);
                    success = true;
                }
            }
        });
    }
}
