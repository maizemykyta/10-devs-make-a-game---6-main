using Bonjoura.Inventory;
using Bonjoura.Managers;
using Bonjoura.Player;
using Bonjoura.UI.Inventory;
using System.Collections;
using UnityEngine;

public class Fireplace : MonoBehaviour
{
    [SerializeField] private BaseInventoryItem itemToGet;
    [SerializeField] private InventoryUI _inventoryUI;
    [SerializeField] private SpriteRenderer _cookingItemSpriteRenderer;
    [SerializeField] private GameObject _droppedItemReference;
    private BaseInventoryItem _rawItem;   
    private BaseInventoryItem _cookedItem; 
    private bool _isCooking;

    public float TimeToCook;

    public bool HasFood => _rawItem != null; 

    private void Using()
    {
        if (PlayerController.Instance.InteractRaycast.CurrentDetectObject != gameObject) return;
        if (!InputManager.Instance.Player.Interact.WasPressedThisFrame()) return;


        if (_isCooking) return;

        if (HasFood) TryTakeCookedFood();
        else TryPlaceFood();

    }

    private void TryPlaceFood()
    {
        
        if (_isCooking || HasFood) return;
        BaseInventoryItem placedFood = _inventoryUI.ReturnSelectedItem().ItemInSlot?.item;

        if (placedFood == null || !placedFood.OtherTypes.Contains(OtherType.Raw)) return; 

        _rawItem = placedFood;
        _cookedItem = placedFood.GetCookedVersion(); 
        StartCoroutine(CookingFood());
    }

    private void TryTakeCookedFood()
    {
        if (!_isCooking && _cookedItem != null)
        {
            DropItemFromFire();
            _rawItem = null;
            _cookedItem = null;
        }
    }

    IEnumerator CookingFood()
    {
        _isCooking = true;
        PlaceItemOnFire();
        yield return new WaitForSeconds(TimeToCook);
        if (_rawItem.DropWhenCoocked != null)
        {
            _cookingItemSpriteRenderer.sprite = _rawItem.DropWhenCoocked.ItemIcon;
        }
        else
        {
            _cookingItemSpriteRenderer.sprite = itemToGet.ItemIcon;
        }
        
        _isCooking = false;


    }

    void PlaceItemOnFire()
    {
        PlayerController.Instance.ItemInventory.RemoveItem(_rawItem);
        _cookingItemSpriteRenderer.sprite = _cookedItem.ItemIcon;
    }

    void DropItemFromFire()
    {
        Vector3 randomOffset = Random.insideUnitSphere;
        randomOffset.y = 0.5f;
        if(_rawItem.DropWhenCoocked != null)
        {
            GameObject droppedItemObject = Instantiate(_droppedItemReference, transform.position + randomOffset, Quaternion.identity);

            DroppedItem droppedItem = droppedItemObject.GetComponent<DroppedItem>();
            Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f));
            droppedItem.Drop(randomDirection, 2);


            droppedItem.SetSprite(_rawItem.DropWhenCoocked.ItemIcon);
            droppedItem.SetSpriteScale(_rawItem.DropWhenCoocked.IconScale);
            droppedItem.Drop(randomDirection, 2);
            droppedItem.SetItem(_rawItem.DropWhenCoocked);
        } 
        else
        {
            GameObject droppedItemObject = Instantiate(_droppedItemReference, transform.position + randomOffset, Quaternion.identity);

            DroppedItem droppedItem = droppedItemObject.GetComponent<DroppedItem>();
            Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f));
            droppedItem.Drop(randomDirection, 2);


            droppedItem.SetSprite(itemToGet.ItemIcon);
            droppedItem.SetSpriteScale(itemToGet.IconScale);
            droppedItem.Drop(randomDirection, 2);
            droppedItem.SetItem(itemToGet);
        }
        
        

        _cookingItemSpriteRenderer.sprite = null;
    }


    private void OnEnable()
    {
        PlayerController.Instance.InteractRaycast.OnRaycastEvent += Using;
    }

    private void OnDisable()
    {
        PlayerController.Instance.InteractRaycast.OnRaycastEvent -= Using;
    }
}
