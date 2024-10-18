namespace Tavaris.Manager
{
    internal class ItemUnlocker : LockableUnlocker
    {
        public string itemID = "Default ID";
        public override bool ResolveLock()
        {
            return GameManager.Instance.HasPlayerThisItemInInventory(itemID);
        }
        public override void Complete()
        {
            GameManager.Instance.RemoveItemFromPlayerInventory(itemID);
        }
    }
}