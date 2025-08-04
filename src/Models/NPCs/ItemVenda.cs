namespace RPGConsole.Models.NPCs;

public class ItemVenda
{
    public Item Item { get; set; }
    public int Preco { get; set; }

    public ItemVenda(Item item, int preco)
    {
        Item = item;
        Preco = preco;
    }
}
