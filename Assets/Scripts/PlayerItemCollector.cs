using UnityEngine;

public class PlayerItemCollector : MonoBehaviour
{
    private InventoryController inventoryController;

    void Start()
    {
        inventoryController = FindObjectOfType<InventoryController>();
    }

    // ESTA ES LA FUNCIÓN QUE TE FALTA:
    public void CollectSpecificItem(GameObject itemObject)
    {
        if (inventoryController != null)
        {
            // Intentamos añadir el objeto al inventario
            bool itemAdded = inventoryController.AddItem(itemObject);

            if (itemAdded)
            {
                Destroy(itemObject);
            }
        }
    }
}