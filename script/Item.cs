using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType // 열거형 itemType 지정
{
    Evidence, // 증거품 인가
    NoneEvidence // 증거품이 아닌가
}

public class Item : MonoBehaviour
{
    public ItemType itemType; // itemType형 itemtype을 생성
    public string itemName; // 아이템 이름 string형으로 생성
    public Sprite itemImage; // 아이템 이미지 sprite형으로 생성
    [Multiline(10)] // 설명글은 멀티라인 10줄 까지 입력되도록 지정
    public string Discription; // 설명글을 적을 변수 string형으로 생성

    public bool IsBox; // booltime으로 boxcolider를 사용할시에 체크 하기 위해 사용하는 불 타입.(물리충돌 위해서 사용할 예정)

    public void SetItem(Item _item) // 매개변수로 받은 _item에 대한 각각의 정보를 변수 값에 저장하는 함수.
    {
        itemType = _item.itemType;
        itemName = _item.itemName;
        itemImage = _item.itemImage;
        Discription = _item.Discription;
    }

    internal bool Contains(string itemName) // itemName이 포함되있는지 불타입. internal은 해당 프로젝트에서 접근
    {
        throw new NotImplementedException(); // 예외를 던짐.
    }
}


