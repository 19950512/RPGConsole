namespace RPGConsole.Models.Itens;

public class EscudoDeMadeira : Equipamento
{
    public EscudoDeMadeira()
        : base(
            nome: "Escudo de Madeira",
            tipo: TipoEquipamento.MaoEsquerda,
            bonusAtaque: 0,
            bonusDefesa: 10,
            raridade: Raridade.Comum
        )
    {
    }
}
