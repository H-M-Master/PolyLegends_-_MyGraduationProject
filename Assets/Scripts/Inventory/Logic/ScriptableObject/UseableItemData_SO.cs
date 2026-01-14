using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Useable Item", menuName = "Inventory/Useable Item Data")]
public class UseableItemData_SO : ScriptableObject
{
    // 所有可恢复类的效果
    public int healthPoint;
}
