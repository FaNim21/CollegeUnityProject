using Main.Datas;
using System;

namespace Main.UI.Equipment
{
    public enum SlotType
    {
        Type,
        Data,
        ItemName,
    }

    [Serializable]
    public class SlotHandler
    {
        public SlotType type;
        public ItemType itemType; //??????
        public ItemData itemData;

        public SlotData data;


        //Dac jeszcze opcje tego zeby output byl typem ore'a jaki obecnie wykopywuje drill
        //Zrobic zeby handle odpowiadal za to ze tylko dany item mozna dac do slota lub dany typ
    }
}
