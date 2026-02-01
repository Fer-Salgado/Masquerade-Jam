using JetBrains.Annotations;
using UnityEngine;

public class byebye : MonoBehaviour, IInteractable

{
    public bool CanInteract()
    {  return true; }
    public void Interact()
    {
        Destroy(gameObject);
    }

}
