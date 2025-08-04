namespace RPGConsole.Models.Itens;

public class BotasDeCouro : Equipamento
{
    public BotasDeCouro()
        : base(
            nome: "Botas de Couro",
            tipo: TipoEquipamento.Botas,
            bonusAtaque: 0,
            bonusDefesa: 8,
            raridade: Raridade.Comum
        )
    {
    }
}
