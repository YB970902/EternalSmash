using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BehaviourTree를 생성해주는 빌더.
/// </summary>
public class BTBuilder
{
    private List<BTNodeBase> nodeBases;
    private List<BTData> nodeDatas;

    public BTController Controller { get; private set; }
    public BTCaller Caller { get; private set; }

    private static BTBuilder instance;

    public static BTBuilder Instance => instance ??= instance = new BTBuilder();
    
    private BTBuilder()
    {
        nodeBases = new List<BTNodeBase>();
        nodeDatas = new List<BTData>();
        Controller = null;
        Caller = null;
    }
    
    /// <summary>
    /// 빌더를 세팅한다.
    /// </summary>
    public void Set(BTCaller _btCaller)
    {
        nodeBases.Clear();
        nodeDatas.Clear();
        Controller = null;
        Caller = _btCaller;
    }
    
    /// <summary>
    /// 입력된 데이터를 기반으로 노드를 만든 후에 Controller를 반환한다.
    /// </summary>
    public BTController Build()
    {
        foreach (var data in nodeDatas)
        {
            nodeBases.Add(data.GetNodeInstance());
        }

        for (int i = 0, count = nodeDatas.Count; i < count; ++i)
        {
            nodeBases[i].Init(this, nodeDatas[i]);
        }

        return Controller;
    }

    /// <summary>
    /// 노드의 데이터를 추가한다.
    /// </summary>
    public BTBuilder AddNodeData(BTData _data)
    {
        nodeDatas.Add(_data);
        return this;
    }
    
    /// <summary>
    /// 아이디가 동일한 노드를 반환한다. 없으면 null을 리턴한다.
    /// </summary>
    public BTNodeBase GetNode(int _id)
    {
        return nodeBases.Find(_ => _.ID == _id);
    }
}