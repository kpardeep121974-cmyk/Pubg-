using UnityEngine;

public enum ItemType { Weapon, Ammo, Medkit, Armor }

public class InteractableItem : MonoBehaviour
{
    [Header("Item Details")]
    public string itemName = "M416";
    public ItemType itemType;
    public int amount = 1;
    public Sprite itemIcon; // For Inventory UI later

    [Header("Interaction Settings")]
    public float interactionRadius = 3.0f;

    // This will be called by the player's interaction system (Raycast or Keypress)
    public void Pickup(GameObject player)
    {
        // Find your existing InventoryManager through our Service Locator
        InventoryManager inventory = GameServiceLocator.Instance?.GetService<InventoryManager>();

        if (inventory != null)
        {
            Debug.Log($"Picked up {amount}x {itemName}!");
            
            // TODO: Call your inventory add method here, e.g., inventory.AddItem(this);
            
            // Destroy the physical item on the ground after picking it up
            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("InventoryManager not found via Service Locator!");
        }
    }

    // Optional: Draw a visual circle in the editor for debugging interaction range
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
