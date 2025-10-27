using ProjectD_API.Data.Models;

namespace ProjectD_API.Data.Messages
{
    public class PlayerItemRequest
    {
        public int DataId { get; set; }
        public int Amount { get; set; }
        public int Level { get; set; }
        public int Exp { get; set; }
        public byte Rarity { get; set; }
        public float Durability { get; set; }
        public int SlotIndex { get; set; }
        public byte InventoryType { get; set; } // 0 = Equip, 1 = Inventory, 2 = Storage
    }

    public class PlayerItemRequest2
    {
        public string Id { get; set; }
        public int DataId { get; set; }
        public int Amount { get; set; }
        public int Level { get; set; }
        public int Exp { get; set; }
        public byte Rarity { get; set; }
        public float Durability { get; set; }
        public int SlotIndex { get; set; }
        public byte InventoryType { get; set; } // 0 = Equip, 1 = Inventory, 2 = Storage
    }

    public class PlayerAddItemsRequest
    {
        public string PlayerId { get; set; }
        public List<PlayerItemRequest> Items { get; set; }
    }

    public class PlayerUpdateItemsRequest
    {
        public string PlayerId { get; set; }
        public List<PlayerItemRequest2> Items { get; set; }
    }

    public class PlayerItemRequest_Remove
    {
        public string Id { get; set; }
        public int DataId { get; set; }
        public int SlotIndex { get; set; }
        public byte InventoryType { get; set; }
    }



    public class PlayerRemoveItemRequest()
    {
        public string PlayerId { get; set; }
        public List<PlayerItemRequest_Remove> Items { get; set; }
    }
}
