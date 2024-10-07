using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public void Interact()
    {
        // 在这里定义可互动物件的互动行为
        Debug.Log(gameObject.name + " has been interacted with.");
        // 例如，打开门、拾取物品等
    }
}
