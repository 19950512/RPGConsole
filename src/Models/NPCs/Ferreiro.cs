namespace RPGConsole.Models.NPCs;

public abstract class Ferreiro : NPC
{
    protected Ferreiro(string nome) : base(nome, TipoNPC.Ferreiro) { }
}
