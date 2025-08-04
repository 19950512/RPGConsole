namespace RPGConsole.Models.Itens;

public class AmuletoDoCondenado : Equipamento
{
    public AmuletoDoCondenado()
        : base(
            nome: "Amuleto do Condenado",
            tipo: TipoEquipamento.Amuleto,
            bonusAtaque: 0,
            bonusDefesa: 5,
            raridade: Raridade.Comum
        )
    {
    }
}
