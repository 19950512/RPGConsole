namespace RPGConsole.Models.Itens;

public class BraceleteDoLutador : Equipamento
{
    public BraceleteDoLutador()
        : base(
            nome: "Bracelete do Lutador",
            tipo: TipoEquipamento.Bracelete,
            bonusAtaque: 13,
            bonusDefesa: 8,
            raridade: Raridade.Incomum
        )
    {
    }
}
