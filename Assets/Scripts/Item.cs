using UnityEngine;

// Al agregar IInteractable, el detector del jugador sabrá que puede presionar "E"
public class Item : MonoBehaviour, IInteractable
{
    public int ID;

    public bool CanInteract() => true;

    public void Interact()
    {
        // Cuando presionas E, el objeto busca al recolector del jugador y le dice "recógeme"
        PlayerItemCollector collector = FindObjectOfType<PlayerItemCollector>();
        if (collector != null)
        {
            collector.CollectSpecificItem(this.gameObject);
        }
    }
}