namespace RPGConsole.Models.Itens;

public class EspadaDeFerro : Equipamento
{
    public EspadaDeFerro()
        : base(
            nome: "Espada de Ferro",
            tipo: TipoEquipamento.MaoDireita,
            bonusAtaque: 15,
            bonusDefesa: 8,
            raridade: Raridade.Comum
        )
    {
    }
}
