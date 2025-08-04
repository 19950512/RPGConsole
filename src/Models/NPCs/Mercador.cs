namespace RPGConsole.Models.NPCs;

using RPGConsole.Models;
using RPGConsole.Utils;

public abstract class Mercador : NPC
{
    protected List<ItemVenda> ItensVenda { get; set; } = new();

    protected Mercador(string nome) : base(nome, TipoNPC.Mercador) { }

    public override void Interagir(Jogador jogador)
    {
        while (true)
        {
            string[] opcoesMenu = { "Comprar Itens", "Vender Itens" };
            int escolhaMenu = UIHelper.MenuInterativo($"🛒 {Nome} - Mercador | 💰 Moedas: {jogador.Moedas}", opcoesMenu, incluirVoltar: true);

            if (escolhaMenu == -1) break;
            if (escolhaMenu == 0) MenuComprar(jogador);
            else if (escolhaMenu == 1) MenuVender(jogador);
        }
    }

    private void MenuComprar(Jogador jogador)
    {
        while (true)
        {
            if (ItensVenda.Count == 0)
            {
                UIHelper.Mensagem("⚠ Não há itens à venda.", ConsoleColor.Yellow);
                UIHelper.EsperarTecla();
                return;
            }

            string[] opcoesItensVenda = ItensVenda
                .Select(itemVenda => $"{itemVenda.Item.Nome} - {itemVenda.Preco} moedas")
                .ToArray();

            int escolhaItem = UIHelper.MenuInterativo("=== Itens à Venda ===", opcoesItensVenda, incluirVoltar: true);
            if (escolhaItem == -1) break;

            ItemVenda itemEscolhido = ItensVenda[escolhaItem];

            Console.Clear();
            Console.WriteLine($"Você escolheu: {itemEscolhido.Item.Nome} ({itemEscolhido.Preco} moedas)");
            Console.Write($"Quantas unidades deseja comprar? ");

            int quantidadeComprar = int.TryParse(Console.ReadLine(), out var qtd) ? qtd : 0;
            if (quantidadeComprar <= 0)
            {
                UIHelper.Mensagem("❌ Quantidade inválida.", ConsoleColor.Red);
                UIHelper.EsperarTecla();
                continue;
            }

            int custoTotal = itemEscolhido.Preco * quantidadeComprar;
            if (jogador.Moedas < custoTotal)
            {
                UIHelper.Mensagem("❌ Moedas insuficientes!", ConsoleColor.Red);
                UIHelper.EsperarTecla();
                continue;
            }

            jogador.Moedas -= custoTotal;
            Item itemCompra = itemEscolhido.Item.Clone(); // Clonar para evitar referência direta
            itemCompra.Quantidade = quantidadeComprar;
            jogador.AdicionarItem(itemCompra);

            UIHelper.Mensagem($"✅ Você comprou {quantidadeComprar}x {itemCompra.Nome} por {custoTotal} moedas!", ConsoleColor.Green);
            UIHelper.EsperarTecla();
        }
    }

    private void MenuVender(Jogador jogador)
    {
        while (true)
        {
            if (jogador.Inventario.Count == 0)
            {
                UIHelper.Mensagem("⚠ Você não tem itens para vender.", ConsoleColor.Yellow);
                UIHelper.EsperarTecla();
                return;
            }

            string[] opcoesItensInventario = jogador.Inventario
                .Select(item => $"{item.Nome} x{item.Quantidade} - Vender por 10 moedas cada")
                .ToArray();

            int escolhaItem = UIHelper.MenuInterativo("=== Seus Itens ===", opcoesItensInventario, incluirVoltar: true);
            if (escolhaItem == -1) break;

            Item itemVender = jogador.Inventario[escolhaItem];
            int precoVendaUnitario = 10; // Pode calcular dinamicamente

            Console.Clear();
            int quantidadeVender = 1;

            if (itemVender.Quantidade > 1)
            {
                Console.Write($"Você tem {itemVender.Quantidade}x {itemVender.Nome}. Quantas deseja vender? ");
                quantidadeVender = int.TryParse(Console.ReadLine(), out var qtd) ? qtd : 0;

                if (quantidadeVender <= 0 || quantidadeVender > itemVender.Quantidade)
                {
                    UIHelper.Mensagem("❌ Quantidade inválida.", ConsoleColor.Red);
                    UIHelper.EsperarTecla();
                    continue;
                }
            }

            int precoVendaTotal = precoVendaUnitario * quantidadeVender;
            jogador.Moedas += precoVendaTotal;

            itemVender.Quantidade -= quantidadeVender;
            if (itemVender.Quantidade <= 0)
                jogador.RemoverItem(itemVender);

            UIHelper.Mensagem($"💰 Você vendeu {quantidadeVender}x {itemVender.Nome} por {precoVendaTotal} moedas!", ConsoleColor.Green);
            UIHelper.EsperarTecla();
        }
    }
}
