namespace RPGConsole.Models.Itens;

public class AnelDePrata : Equipamento
{
    public AnelDePrata()
        : base(
            nome: "Anel de Prata",
            tipo: TipoEquipamento.Anel,
            bonusAtaque: 10,
            bonusDefesa: 10,
            raridade: Raridade.Incomum
        )
    {
    }
}
