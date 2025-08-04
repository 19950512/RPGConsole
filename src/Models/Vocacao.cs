namespace RPGConsole.Models;

public abstract class Vocacao
{
    public string Nome { get; }
    public int AtaqueBase { get; protected set; }

    public int AtaqueExtraPorItens { get; set; }

    public int DefesaBase { get; protected set; }
    public int VidaBase { get; protected set; }

    protected Vocacao(string nome, int ataqueBase, int defesaBase, int vidaBase)
    {
        Nome = nome;
        AtaqueBase = ataqueBase;
        DefesaBase = defesaBase;
        VidaBase = vidaBase;
    }

    // 🔹 Dano básico (considera ataque base)
    public virtual int CalcularDano()
    {
        return new Random().Next(AtaqueBase, AtaqueBase + 5) + AtaqueExtraPorItens;
    }

    // 🔹 Skill especial (cada vocação define seu efeito)
    public abstract int UsarSkill();

    // 🔹 Bônus ao subir de nível (diferente para cada vocação)
    public virtual void AoSubirDeNivel(Jogador jogador)
    {
        AtaqueBase += 1;    // Todas as classes ganham um pouco de ataque
        DefesaBase += 1;    // Ganham também um pouco de defesa
        jogador.VidaMax += 5;
    }
}
