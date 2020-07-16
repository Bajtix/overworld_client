using UnityEngine;

public class PlayerInventory : MonoBehaviour
{

    public Transform arm;
    public GameObject instance;

    public ItemStack[] items;


    public void SelectItem(string itemname)
    {
        if (instance != null)
        {
            Destroy(instance);
        }

        if (itemname == "none")
            return;

        Item selectedItem = null;
        selectedItem = GameManager.instance.GetItem(itemname);

        if (selectedItem == null)
        {
            Debug.LogWarning("Selected item was null");
            return;
        }

        instance = Instantiate(selectedItem.model, arm);
    }
}
