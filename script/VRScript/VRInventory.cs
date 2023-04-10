using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRInventory : MonoBehaviour
{
    public GameObject InvenContent; // 인벤토리내 콘텐츠 게임 오브젝트 선언

    [SerializeField] // 직렬화
    public List<Item> items = new List<Item>(); // Item형 리스트 생성
    Item tempItem; // Item 객체 tempItem생성
    public void AddItem(Item _item) // Item형 매개변수 _item을 추가하기전 검사하는 함수
    {
        if (items.Count < 20) // 만약 items가 20개 미만이라면
            items.Add(_item); // 리스트에 매개변수를 추가함.
    }
    public void CheckItem(Item _item) // 아이템 인벤토리내 중복, 비어있는자리 확인하는 함수
    {
        int checkcount = items.Count; // int형 checkcount에 items의 갯수만큼을 저장함.

        for (int i = 0; i < 20; i++) // for문으로 0부터 20까지
        {
            if (items[i] == null) // 만약 items[i]번재가 null이라면 
            {
                items.RemoveAt(i); // 먼저 items i번째를 remove해줌
                items.Insert(i, _item); // 다음에 items의 i번째에 _item을 Insert를 해줌(위에 RemoveAt을 한 이유는 Insert하면 리스트가 늘어나기 때문)
                updateInvenUI(i, _item); // 인벤토리 UI 업데이트 해주는 함수로 i, _item을 매개변수로 전달함.
                break; // for문 종료
            }
            else if (items[i] == null && items[i].itemName != _item.itemName) // items[i]번이 null이고 그리고 i번째 item이름이 매개변수_item의 이름과 같지 않을때
            {
                items.RemoveAt(i); // 26~29까지 설명이 같으므로 생략
                items.Insert(i, _item);
                updateInvenUI(i, _item);
                break;
            }
            else if (items[i].itemName == _item.itemName) // items[i]번째의 아이템 이름과 _itme의 아이템 이름이 같으면
            {
                break; // 아무것도 하지 않고 for문 탈출
            }
        }
    }
    public void DeleteInventoryItem(Item _item) // 인벤토리에 아이템을 제거하는 함수
    {
        int checkcount = items.Count; // int형 checkcount에 items의 갯수만큼을 저장함.

        for (int i = 0; i < 20; i++) // for문으로 0부터 20까지
        {
            if (items[i].itemName == _item.itemName) // items[i]번째의 아이템 이름과 _itme의 아이템 이름이 같으면
            {
                items[i] = null; // items[i]번째에 null값을 넣음
                InvenContent.transform.Find($"Slot{i}").transform.Find("ItemImage").gameObject.SetActive(false); // 8번줄에서 생성한 게임 오브젝트에서 slot{i}를 찾고 그 slot{i} 자식인 ItemImage의 활성화를 꺼버림
                InvenContent.transform.Find($"Slot{i}").transform.Find("ItemImage").gameObject.GetComponent<Image>().sprite = null; // 8번줄에서 생성한 게임 오브젝트에서 slot{i}를 찾고 그 slot{i} 자식인 ItemImage의 이미지 요소중 sprite(2d이미지)를 null로 바꿈
                InvenContent.transform.Find($"Slot{i}").transform.Find("ItemImage").gameObject.GetComponent<Image>().preserveAspect = false; // 8번줄에서 생성한 게임 오브젝트에서 slot{i}를 찾고 그 slot{i} 자식인 ItemImage의 이미지 요소중 preserveAspect를 false로 바꿈
                break;
            }
        }
    }
    public void updateInvenUI(int i, Item _item) // 인벤토리 UI를 업데이트 해주는 함수
    {
        InvenContent.transform.Find($"Slot{i}").transform.Find("ItemImage").gameObject.SetActive(true); // 8번줄에서 생성한 게임 오브젝트에서 slot{i}를 찾고 그 slot{i} 자식인 ItemImage의 활성화를 켬
        InvenContent.transform.Find($"Slot{i}").transform.Find("ItemImage").gameObject.GetComponent<Image>().sprite = _item.itemImage; // 8번줄에서 생성한 게임 오브젝트에서 slot{i}를 찾고 그 slot{i} 자식인 ItemImage의 이미지 요소중 sprite(2d이미지)를 _item의 아이템 이미지로 바꿈
        InvenContent.transform.Find($"Slot{i}").transform.Find("ItemImage").gameObject.GetComponent<Image>().preserveAspect = true; // 8번줄에서 생성한 게임 오브젝트에서 slot{i}를 찾고 그 slot{i} 자식인 ItemImage의 이미지 요소중 preserveAspect를 true로 바꿈
    }
    
    public void OnTriggerEnter(Collider col) // 이 스크립트가 적용된 오브젝트에서 콜라이더에 물체가 충돌 했을때(콜라이더에 IsTrigger가 체크 되어 있어야함)
    {
        if (col.transform.tag == "Item") // 만약 충돌된 콜라이더의 tag가 Item이라면
        {
            tempItem = col.transform.gameObject.GetComponent<Item>(); // 12번 줄에서 생성한 객체 tempItem에 부딪힌 콜라이더 오브젝트의 요소중 Item을 값을 저장함.
			/* 아래 4개의 줄은 에디터 상에서 디버그 로그를 띄워줌(디버그 내용: 아이템 Image, Name, Type, Discription)
            Debug.Log(tempItem.itemImage);
            Debug.Log(tempItem.itemName);
            Debug.Log(tempItem.itemType);
            Debug.Log(tempItem.Discription);
			*/
            CheckItem(tempItem); // tempItem을 매개변수로 CheckItem 함수를 실행함
        }
    }
    public void OnTriggerExit(Collider col) // 이 스크립트가 적용된 오브젝트에서 콜라이더에 출돌된 물체가 빠져 나갈때
    {
        if (col.transform.tag == "Item") // 만약 나가는 콜라이더의 tag가 Item이라면
        {
            tempItem = col.transform.gameObject.GetComponent<Item>(); // 12번 줄에서 생성한 객체 tempItem에 부딪힌 콜라이더 오브젝트의 요소중 Item을 값을 저장함.
            DeleteInventoryItem(tempItem); // tempItem을 매개변수로 DeleteInventoryItem함수를 실행함.
        }
    }
}
