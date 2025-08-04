namespace RPGConsole.Models.Itens;

public class CalcaDeLona : Equipamento
{
    public CalcaDeLona()
        : base(
            nome: "Calça de Lona",
            tipo: TipoEquipamento.Pernas,
            bonusAtaque: 0,
            bonusDefesa: 10,
            raridade: Raridade.Comum
        )
    {
    }
}
