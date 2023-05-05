using static UnityEditor.Progress;

[System.Serializable]
public class InventorySystem
{
    public class InventoryEntry
    {
        public int Count;
        public Item Item;
    }

    public InventoryEntry[] Entries = new InventoryEntry[3];
    CharacterData m_Owner;

    public void Init(CharacterData owner)
    {
        m_Owner = owner;
    }

    // Add an item to the inventory. If this item already exists, increment the counter.
    public void AddItem(Item item)
    {
        bool found = false;
        int firstEmpty = -1;
        for (int i = 0; i < 10; ++i)
        {
            if (Entries[i] == null)
            {
                if (firstEmpty == -1)
                    firstEmpty = i;
            }
            else if (Entries[i].Item == item)
            {
                Entries[i].Count += 1;
                found = true;
            }
        }

        if (!found && firstEmpty != -1)
        {
            InventoryEntry entry = new InventoryEntry();
            entry.Item = item;
            entry.Count = 1;

            Entries[firstEmpty] = entry;
        }
    }

    // <summary>
    // This will *try* to use the item. If the item return true when used, this will decrement the stack count and
    // if the stack count reach 0 this will free the slot. If it return false, it will just ignore that call.
    // (e.g. a potion will return false if the user is at full health, not consuming the potion in that case)
    // </summary>
    // <param name="item"></param>
    // <returns></returns>
    public bool UseItem(InventoryEntry item)
    {
        if (item.Item.UsedBy(m_Owner))
        {
            item.Count -= 1;

            if (item.Count <= 0)
            {
                //maybe store the index in the InventoryEntry to avoid having to find it again here
                for (int i = 0; i < 3; ++i)
                {
                    if (Entries[i] == item)
                    {
                        Entries[i] = null;
                        break;
                    }
                }
            }

            return true;
        }

        return false;
    }
}
