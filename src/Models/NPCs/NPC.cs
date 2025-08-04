namespace RPGConsole.Models.NPCs;

public abstract class NPC
{
    public string Nome { get; }
    public TipoNPC Tipo { get; }

    protected NPC(string nome, TipoNPC tipo)
    {
        Nome = nome;
        Tipo = tipo;
    }

    public abstract void Interagir(Jogador jogador);
}
