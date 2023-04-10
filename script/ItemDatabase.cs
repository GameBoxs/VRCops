using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase instance; // static으로 ItemDatabase클래스 객체 instance를 만듬.
    // Start is called before the first frame update
    void Start()
    {
        instance = this; // instance는 this임.
    }

    
    public List<Item> itemDB = new List<Item>(); // item 리스트 형식으로 itemDB를 만듬.

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddItem(Item _item) // item을 추가해줌.
    {
        if (itemDB.Count < 20) // itemDB의 갯수는 20개 미만 이라면 리스트에 매개변수 _item을 추가.
            itemDB.Add(_item);
    }
}
