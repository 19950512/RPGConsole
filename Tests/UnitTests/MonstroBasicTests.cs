using Xunit;
using FluentAssertions;
using RPGShared.Models.Monstros;

namespace RPGConsole.Tests.UnitTests;

public class MonstroBasicTests
{
    [Fact]
    public void Amazon_DeveSerCriada_Corretamente()
    {
        // Arrange & Act
        var amazon = new Amazon();

        // Assert
        amazon.Should().NotBeNull();
        amazon.Nome.Should().NotBeEmpty();
        amazon.Vida.Should().BeGreaterThan(0);
        amazon.VidaMax.Should().BeGreaterThan(0);
        amazon.Ataque.Should().BeGreaterThan(0);
    }
    
    [Fact]
    public void Amazon_DeveReceber_Dano()
    {
        // Arrange
        var amazon = new Amazon();
        var vidaInicial = amazon.Vida;
        
        // Act
        amazon.ReceberDano(10);
        
        // Assert
        amazon.Vida.Should().BeLessThan(vidaInicial);
    }
}
