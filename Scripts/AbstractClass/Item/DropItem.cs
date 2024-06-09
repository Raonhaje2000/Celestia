using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DropItem : InventoryItem
{
    protected Rigidbody _rigidbody;

    float forceAmount;      // 가하는 힘의 양
    Vector3 forceDirection; // 힘의 방향

    /// <summary>
    /// 드랍 아이템 초기화
    /// </summary>
    protected void SetInit()
    {
        _rigidbody = GetComponent<Rigidbody>();

        forceDirection = Vector3.up;
        forceAmount = 50;

        Destroy(gameObject, 60); // 생성 60초 후 제거
    }

    /// <summary>
    /// 드랍 아이템 튀어오르기
    /// </summary>
    protected void BounceDropItem()
    {
        // 생성 위치로부터 랜덤하게 떨어진 위치에 아이템 떨어뜨리기
        float x = Random.Range(-0.5f, 0.5f);
        float z = Random.Range(-0.5f, 0.5f);

        forceDirection = Vector3.Normalize(new Vector3(x, 3.0f, z));

        // 힘을 가함 (물체 질량에 영향을 받음)
        _rigidbody.AddForce(forceDirection * forceAmount, ForceMode.Impulse);
    }

    /// <summary>
    /// 드랍 아이템 획득(인벤토리 저장) 처리를 위한 드랍 아이템 데이터 반환<br/>
    /// 플레이어가 획득 가능한 거리 내에서 아이템 획득 키를 눌렀을 때 호출
    /// </summary>
    /// <returns> 획득하는 드랍 아이템 </returns>
    public abstract DropItem ObtainDropItem();

    /// <summary>
    /// 드랍 아이템을 획득(인벤토리에 추가)하는 처리</br>
    /// OnCollisionEnter에서 Ground와 충돌했는지 확인 후, Invoke에서 호출하여 몇 초 딜레이 후 인벤토리에 저장함</br>           
    /// InventoryManager.instance.AddItems() 호출 후 몇 초 후 해당 아이템 삭제
    /// </summary>
    public abstract void MoveItemToInventory();
}
