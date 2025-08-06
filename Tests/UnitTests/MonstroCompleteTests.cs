using FluentAssertions;
using RPGShared.Models;
using RPGShared.Models.Itens;
using RPGShared.Models.Monstros;
using RPGShared.Models.Vocacoes;
using Xunit;

namespace RPGConsole.Tests.UnitTests;

public class MonstroTests
{
    [Fact]
    public void Monstro_DeveReceber_DanoCorretamente()
    {
        // Arrange
        var amazon = new Amazon();
        var danoInicial = 10;
        var vidaInicial = amazon.Vida;

        // Act
        amazon.ReceberDano(danoInicial);

        // Assert
        var danoEsperado = Math.Max(1, danoInicial - amazon.Defesa);
        amazon.Vida.Should().Be(vidaInicial - danoEsperado);
    }

    [Fact]
    public void Monstro_DeveMorrer_QuandoVidaChegaAZero()
    {
        // Arrange
        var amazon = new Amazon();

        // Act
        amazon.ReceberDano(amazon.Vida + 100); // Dano suficiente para matar

        // Assert
        amazon.EstaVivo().Should().BeFalse();
        amazon.Vida.Should().Be(0);
    }

    [Fact]
    public void Monstro_DeveEstar_VivoInicialmente()
    {
        // Arrange & Act
        var amazon = new Amazon();

        // Assert
        amazon.EstaVivo().Should().BeTrue();
        amazon.Vida.Should().BeGreaterThan(0);
    }

    [Fact]
    public void Amazon_DeveDropar_LootCorreto()
    {
        // Arrange
        var amazon = new Amazon();
        
        // Act - Executar múltiplas vezes para testar probabilidade
        var todosDrops = new List<Item>();
        for (int i = 0; i < 100; i++)
        {
            var amazon2 = new Amazon();
            amazon2.ReceberDano(1000); // Garantir morte sem usar propriedade privada
            var drops = amazon2.DroparLoot();
            todosDrops.AddRange(drops);
        }

        // Assert
        todosDrops.Should().NotBeEmpty();
        
        // Deve sempre dropar moedas (100% chance)
        todosDrops.Should().Contain(item => item is Moeda);
        
        // Pode dropar Colar da Amazonas (75% chance)
        var temColar = todosDrops.Any(item => item is ColarDaAmazonas);
        
        // Pode dropar Poção de Vida (50% chance)
        var temPocao = todosDrops.Any(item => item is PocaoVida);
        
        // Em 100 tentativas, deveria ter pelo menos alguns drops especiais
        (temColar || temPocao).Should().BeTrue();
    }

    [Fact]
    public void Monstro_DeveAtacar_ComDanoVariavel()
    {
        // Arrange
        var amazon = new Amazon();
        var danos = new List<int>();

        // Act - Múltiplos ataques para verificar variação
        for (int i = 0; i < 50; i++)
        {
            danos.Add(amazon.Atacar());
        }

        // Assert
        danos.Should().NotBeEmpty();
        danos.Should().OnlyContain(dano => dano > 0);
        
        // Deve haver variação no dano
        var danoMin = danos.Min();
        var danoMax = danos.Max();
        (danoMax - danoMin).Should().BeGreaterThan(0);
    }

    [Fact]
    public void Amazon_DeveConfigurar_LootCorretamente()
    {
        // Arrange & Act
        var amazon = new Amazon();

        // Assert
        amazon.LootPossivel.Should().NotBeEmpty();
        amazon.LootPossivel.Should().HaveCount(3); // Moeda (base) + Colar + Poção
        
        // Verificar se tem loot de moeda (100% chance)
        amazon.LootPossivel.Should().Contain(loot => loot.chance == 100);
        
        // Verificar se tem loot de colar (75% chance)
        amazon.LootPossivel.Should().Contain(loot => loot.chance == 75);
        
        // Verificar se tem loot de poção (50% chance)
        amazon.LootPossivel.Should().Contain(loot => loot.chance == 50);
    }
}
