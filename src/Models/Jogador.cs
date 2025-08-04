namespace RPGConsole.Models;

using RPGConsole.Models;
using RPGConsole.Models.Itens;

public class Jogador : Combatente
{
    public string Nome { get; set; }
    public Vocacao Vocacao { get; set; }
    public int Level { get; set; } = 1;
    public int Experiencia { get; set; } = 0;
    public int VidaMax { get; set; } = 100;
    public int Vida { get; set; } = 100;
    public int Moedas { get; set; } = 100;

    public List<Item> Inventario { get; } = new();
    public int SkillCooldown { get; private set; } = 0;
    
    public Dictionary<TipoEquipamento, List<Equipamento>> Equipamentos { get; } =
    Enum.GetValues<TipoEquipamento>().ToDictionary(slot => slot, _ => new List<Equipamento>());

    public int AtaqueTotal => Vocacao.AtaqueBase + Equipamentos.Values.SelectMany(e => e).Sum(e => e.BonusAtaque);
    public int DefesaTotal => Vocacao.DefesaBase + Equipamentos.Values.SelectMany(e => e).Sum(e => e.BonusDefesa);
    public int VidaMaxTotal => VidaMax + Equipamentos.Values.SelectMany(e => e).Sum(e => e.BonusVida);

    public Jogador(string nome, Vocacao vocacao)
    {
        Nome = nome;
        Vocacao = vocacao;

        VidaMax = vocacao.VidaBase;
        Vida = VidaMax;
    }

    public void ReceberDano(int dano)
    {
        int danoFinal = Math.Max(0, dano - DefesaTotal);
        Vida -= danoFinal;
        if (Vida < 0) Vida = 0;
    }

    public void AdicionarItem(Item novoItem)
    {

        if(novoItem is Moeda moeda)
        {
            Moedas += moeda.Quantidade;
            Console.WriteLine($"💰 {Nome} recebeu {moeda.Quantidade} moedas! Total: {Moedas}");
            return;
        }

        Item? existente = Inventario.FirstOrDefault(i => i.GetType() == novoItem.GetType());

        if (existente != null && existente.Consumivel)
        {
            existente.AdicionarQuantidade(novoItem.Quantidade);
        }
        else
        {
            Inventario.Add(novoItem);
        }
    }

    public void RemoverItem(Item item)
    {
        var itemExistente = Inventario.FirstOrDefault(i => i.Nome == item.Nome);
        if (itemExistente != null)
        {
            if (itemExistente.Consumivel)
            {
                itemExistente.RemoverQuantidade(item.Quantidade);
                if (itemExistente.Quantidade <= 0)
                {
                    Inventario.Remove(itemExistente);
                }
            }
            else
            {
                Inventario.Remove(itemExistente);
            }
        }
    }
    
    public bool Equipar(Equipamento equipamento)
    {
        TipoEquipamento slot = equipamento.Tipo;
        List<Equipamento> lista = Equipamentos[slot];

        if (slot == TipoEquipamento.Anel)
        {
            if (lista.Count >= 4)
            {
                Console.WriteLine("❌ Você já está com 4 anéis equipados!");
                return false;
            }
        }
        else
        {
            if (lista.Count > 0)
            {
                Console.WriteLine($"❌ Já existe um item equipado no slot {slot}!");
                return false;
            }
        }

        lista.Add(equipamento);

        if (equipamento.BonusVida > 0)
        {
            Vida = Math.Min(Vida + equipamento.BonusVida, VidaMaxTotal);
        }

        Console.WriteLine($"✅ {Nome} equipou {equipamento.Nome} no slot {slot}!");
        return true;
    }

    public bool Desequipar(TipoEquipamento slot, int index = 0)
    {
        List<Equipamento> lista = Equipamentos[slot];
        if (lista.Count == 0)
        {
            Console.WriteLine($"❌ Nenhum equipamento no slot {slot}.");
            return false;
        }

        if (index < 0 || index >= lista.Count)
        {
            Console.WriteLine($"❌ Índice inválido para o slot {slot}.");
            return false;
        }

        Equipamento equipamento = lista[index];
        lista.RemoveAt(index);

        Console.WriteLine($"🔹 {Nome} removeu {equipamento.Nome} do slot {slot}.");
        return true;
    }

    public bool EstaVivo() => Vida > 0;

    public void GanharExperiencia(int xp)
    {
        Experiencia += xp;
        Console.WriteLine($"{Nome} ganhou {xp} XP! (Total: {Experiencia}/{Level * 100})");

        if (Experiencia >= Level * 100)
        {
            SubirDeNivel();
        }
    }

    public void PassarTurno()
    {
        if (SkillCooldown > 0) SkillCooldown--;
    }

    public int Atacar()
    {
        return CalcularDano(AtaqueTotal, criticoChance: 15, variacaoMax: 5);
    }

    public int UsarSkill()
    {
        if (SkillCooldown > 0)
        {
            Console.WriteLine("⏳ Skill ainda em cooldown!");
            return 0;
        }

        SkillCooldown = 3;

        int danoBase = AtaqueTotal + Vocacao.UsarSkill();
        return CalcularDano(danoBase, criticoChance: 20, variacaoMax: 3, skill: true);
    }
    
    private int CalcularDano(int baseDano, int criticoChance, int variacaoMax, bool skill = false)
    {
        Random rand = new();
        int dano = rand.Next(baseDano, baseDano + variacaoMax + 1);

        bool critico = rand.Next(0, 100) < criticoChance;
        if (critico)
        {
            dano = (int)(dano * 1.5);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(skill ? "💥 SKILL CRÍTICA!" : "💥 ATAQUE CRÍTICO!");
            Console.ResetColor();
        }
        else if (skill)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"✨ {Nome} lançou sua skill!");
            Console.ResetColor();
        }

        return dano;
    }

    private void SubirDeNivel()
    {
        Level++;
        Experiencia = 0;

        Vocacao.AoSubirDeNivel(this);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"🎉 {Nome} subiu para o level {Level}! Vida aumentada para {VidaMax}.");
        Console.ResetColor();
    }
}
