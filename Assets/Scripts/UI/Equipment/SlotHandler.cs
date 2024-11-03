using Main.Datas;
using System;

namespace Main.UI.Equipment
{
    public enum SlotType
    {
        None,
        Type,
        Data,
        ItemName,
    }

    [Serializable]
    public class SlotHandler
    {
        public Guid structureGuid;
        public SlotType type;
        public ItemType itemType; //??????
        public ItemData itemData;
        //na razie nic z tego nie jest zaimplementowane i jest szansa ze w ogole nie bedzie, bo malo czasu pozdrawiam

        private InventorySlot _slot;
        public SlotData data;


        public void Setup(InventorySlot slot)
        {
            _slot = slot;
        }

        public void CreateItem(ItemData item, int amount = 0)
        {
            data.itemData = item;
            UpdateAmount(amount);
        }
        public void UpdateAmount(int amount = 0)
        {
            data.quantity += amount;

            if (!_slot.IsCorrectWindowOpened(structureGuid)) return;
            _slot.UpdateSlot();
        }

        public void Swap(SlotHandler slotHandler)
        {
            (data, slotHandler.data) = (slotHandler.data, data);
        }

        public void Clear()
        {
            data = null;
            data = new SlotData();
        }
        public void DeepClear()
        {
            data.itemData = null;
            data.quantity = 0;
        }

        //Dac jeszcze opcje tego zeby output byl typem ore'a jaki obecnie wykopywuje drill
        //Zrobic zeby handle odpowiadal za to ze tylko dany item mozna dac do slota lub dany typ
    }
}
