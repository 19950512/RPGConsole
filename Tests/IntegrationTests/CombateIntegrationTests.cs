using FluentAssertions;
using RPGShared.Models;
using RPGShared.Models.Itens;
using RPGShared.Models.Monstros;
using RPGShared.Models.Vocacoes;
using RPGShared.Services;
using Xunit;

namespace RPGConsole.Tests.IntegrationTests;

public class CombateIntegrationTests
{
    [Fact]
    public void CombateCompleto_JogadorVsAmazon_DeveProcessar_CorretamenteQuandoMonstroMorre()
    {
        // Arrange
        var jogador = new Jogador("Herói", new Knight());
        var amazon = new Amazon();
        
        var expInicial = jogador.Experiencia;
        var inventarioInicial = jogador.Inventario.Count;

        // Act - Simular combate até a morte do monstro
        var originalOut = Console.Out;
        try
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw); // Capturar output do console
                
                while (amazon.EstaVivo())
                {
                    var dano = jogador.Atacar();
                    BatalhaService.ExecutarAtaque(jogador, amazon, dano);
                }

                // Processar vitória
                var monstrosDerrotados = new List<Monstro> { amazon };
                BatalhaService.ProcessarVitoria(jogador, monstrosDerrotados);
            }
        }
        finally
        {
            Console.SetOut(originalOut);
        }

        // Assert
        amazon.EstaVivo().Should().BeFalse();
        jogador.Experiencia.Should().BeGreaterThan(expInicial);
        jogador.Inventario.Count.Should().BeGreaterThan(inventarioInicial);
        
        // Deve ter ganho pelo menos algum item (moedas ou outros itens)
        jogador.Inventario.Should().NotBeEmpty();
    }

    [Fact]
    public void CombateCompleto_MultiplosMonstros_DeveProcessar_CorretamenteLootEExp()
    {
        // Arrange
        var jogador = new Jogador("Herói", new Knight());
        var monstros = new List<Monstro>
        {
            new Amazon(),
            new Amazon(),
            new Goblin()
        };
        
        var expInicial = jogador.Experiencia;
        var inventarioInicial = jogador.Inventario.Count;

        // Act - Derrotar todos os monstros
        var originalOut = Console.Out;
        try
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                
                foreach (var monstro in monstros)
                {
                    while (monstro.EstaVivo())
                    {
                        var dano = jogador.Atacar();
                        BatalhaService.ExecutarAtaque(jogador, monstro, dano);
                    }
                }

                BatalhaService.ProcessarVitoria(jogador, monstros);
            }
        }
        finally
        {
            Console.SetOut(originalOut);
        }

        // Assert
        monstros.Should().OnlyContain(m => !m.EstaVivo());
        
        // Experiência deve ser a soma de todos os monstros
        var expEsperada = monstros.Sum(m => m.Experiencia);
        jogador.Experiencia.Should().Be(expInicial + expEsperada);
        
        // Deve ter itens de todos os monstros
        jogador.Inventario.Count.Should().BeGreaterThan(inventarioInicial);
    }

    [Fact]
    public void CombateCompleto_JogadorMorre_BatalhaDeveTerminar()
    {
        // Arrange
        var jogador = new Jogador("Herói", new Knight());
        var amazon = new Amazon();

        // Act - Matar o jogador
        var originalOut = Console.Out;
        try
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                jogador.ReceberDano(1000);
            }
        }
        finally
        {
            Console.SetOut(originalOut);
        }

        // Assert
        jogador.EstaVivo().Should().BeFalse();
        BatalhaService.BatalhaTerminou(jogador, new List<Monstro> { amazon }).Should().BeTrue();
    }

    [Fact]
    public void BatalhaService_CalcularDano_DeveRetornar_DanoPositivo()
    {
        // Arrange
        var baseDano = 10;

        // Act
        var dano = BatalhaService.CalcularDano(baseDano, 0, 0); // Sem crítico e sem variação

        // Assert
        dano.Should().BeGreaterOrEqualTo(baseDano);
        dano.Should().BeLessOrEqualTo(baseDano);
    }

    [Fact]
    public void BatalhaService_ProcessarVitoria_DeveDistribuir_ExperienciaELoot()
    {
        // Arrange
        var jogador = new Jogador("Teste", new Knight());
        var monstros = new List<Monstro>
        {
            new Amazon(),
            new Amazon()
        };
        
        // Matar os monstros
        foreach (var monstro in monstros)
        {
            monstro.ReceberDano(1000);
        }

        var expInicial = jogador.Experiencia;
        var inventarioInicial = jogador.Inventario.Count;

        // Act
        var originalOut = Console.Out;
        try
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                BatalhaService.ProcessarVitoria(jogador, monstros);
            }
        }
        finally
        {
            Console.SetOut(originalOut);
        }

        // Assert
        jogador.Experiencia.Should().BeGreaterThan(expInicial);
        jogador.Inventario.Count.Should().BeGreaterOrEqualTo(inventarioInicial);
    }
}
