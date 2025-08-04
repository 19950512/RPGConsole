namespace RPGConsole.Models.Itens;

public class EscudoSagrado : Equipamento
{
    public EscudoSagrado()
        : base(
            nome: "Escudo Sagrado",
            tipo: TipoEquipamento.MaoEsquerda,
            bonusAtaque: 0,
            bonusDefesa: 100,
            raridade: Raridade.Lendário
        )
    {
    }
}
