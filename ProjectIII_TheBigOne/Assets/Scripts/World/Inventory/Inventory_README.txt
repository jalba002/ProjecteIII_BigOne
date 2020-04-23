Inventory:

public class Inventory
- pertenece al Player, se usa para gestionar el inventario para añadir o quitar items
	- AddItem() 
	- RemoveItem()

public class InventoryItem
- clase que define las variables del item (aquí las más importantes)
	- itemName
	- itemDescription
	- MaxStackQuantity
	- itemIcon
	- isStackable
	- destroyOnUse	

public class InventoryItemList : ScriptableObject
- clase donde se crea una lista de todos los items que haya en el juego
- Uso: Assets/Create/New Item List; en esta itemList, definir los items

public class SceneItem
- pertenece al GameObject del item dentro de la escena
	- itemName 
- Uso:  el item dentro de la escena tiene este script i en la variable itemName 
	se le pone el nombre exacto del item que representa de la itemList.

(Interficie)

public class InventoryDisplay
- pertenece al inventoryDisplay del canvas, que contiene una Grid. 
  Esta clase crea y gestiona los distintos slots del inventario
	- slotPrefab
	- numberOfSlots
- tambien contiene los textos del Item seleccionado y su descripción
	- selectedItemName
	- selectedItemInfo
- Funciones importantes:
	- AddNewItem()
	- RemoveItem()
	
public class InventorySlot
- pertenece al prefab del Slot. Contiene las variables que se mostrarán del item en el inventario
	- background
	- spriteImage
	- itemNameText
	- quantityText
- Funciones importantes:
	- SetUp()
		- En el Awake se hace un SetUp(null) del slot para dejarlo sin item
		- Se invoca desde el InventoryDisplay cuando en el AddNewItem()
	- SelectThisSlot()
		- Si el slot contiene un item, se marca como selectedItem