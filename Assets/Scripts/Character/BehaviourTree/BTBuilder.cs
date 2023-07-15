using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BehaviourTree를 생성해주는 빌더.
/// 빌더를 하나만 인스턴스 해놓으면 여러종류의 BehaviourTree를 만들 수 있다.
/// </summary>
public class BTBuilder
{
    private List<BTNodeBase> nodeBases;
    private List<BTData> nodeDatas;

    /// <summary> 결과로 반환할 컨트롤러 </summary>
    private BTController controller;
    
    public BTBuilder()
    {
        nodeBases = new List<BTNodeBase>();
        nodeDatas = new List<BTData>();
        controller = null;
    }

    /// <summary>
    /// 모든 데이터를 지운다.
    /// </summary>
    public void Clear()
    {
        nodeBases.Clear();
        nodeDatas.Clear();
        controller = null;
    }
    
    /// <summary>
    /// 입력된 데이터를 기반으로 노드를 만든 후에
    /// </summary>
    /// <returns></returns>
    public BTController Build()
    {
        nodeDatas.ForEach(_ => _.SetValue());
        
        return controller;
    }

    public BTBuilder AddRootNode(BTRootData _data)
    {
        // Root는 트리에서 단 한번만 호출되기 때문에, 여기에서 컨트롤러를 만든다.
        controller = new BTController();
        
        BTRoot node = new BTRoot();
        node.Init(null, controller, _data);
        
        nodeBases.Add(node);
        nodeDatas.Add(_data);
        
        return this;
    }
    
    public BTNodeBase GetNode(int _id)
    {
        return nodeBases.Find(_ => _.Data.ID == _id);
    }
}
