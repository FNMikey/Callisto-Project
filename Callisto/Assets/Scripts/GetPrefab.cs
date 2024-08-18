using UnityEngine;
using System.Collections.Generic;

public class GetPrefab : MonoBehaviour
{
        public GameObject prefab; 
        public Item item;

    public GameObject GetPrefabFromItem(string name){

    if(name==item.itemName)
    {
        return prefab;
    }else{ 
        return null;
    }
    }
}
