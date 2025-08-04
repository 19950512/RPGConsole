namespace RPGConsole.Models.Itens;

public class CapaceteDeAco : Equipamento
{
    public CapaceteDeAco()
        : base(
            nome: "Capacete de Aço",
            tipo: TipoEquipamento.Cabeca,
            bonusAtaque: 0,
            bonusDefesa: 8,
            raridade: Raridade.Comum
        )
    {
    }
}
