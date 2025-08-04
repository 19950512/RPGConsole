namespace RPGConsole.Models.Itens;

public class ArmaduraDeCouro : Equipamento
{
    public ArmaduraDeCouro()
        : base(
            nome: "Armadura de Couro",
            tipo: TipoEquipamento.Corpo,
            bonusAtaque: 0,
            bonusDefesa: 10,
            raridade: Raridade.Comum
        )
    {
    }
}
