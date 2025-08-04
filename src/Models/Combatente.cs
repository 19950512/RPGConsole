public interface Combatente
{
    string Nome { get; }
    int Vida { get; set; }
    int VidaMax { get; }
    int Atacar();
    void ReceberDano(int dano);
    bool EstaVivo();
}
