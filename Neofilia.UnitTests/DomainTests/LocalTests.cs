using Neofilia.Domain;
using static Neofilia.Domain.Menu;
using System.Net;
using System;

namespace Neofilia.UnitTests.DomainTests;

public class LocalTests
{
    private static readonly List<Table> _tables = new()
    {
        Table.CreateTestTable(new Table.TableId(1), new Local.LocalId(1), 1),
        Table.CreateTestTable(new Table.TableId(2), new Local.LocalId(1), 2),
        Table.CreateTestTable(new Table.TableId(3), new Local.LocalId(1), 3),
        Table.CreateTestTable(new Table.TableId(4), new Local.LocalId(1), 4),
        Table.CreateTestTable(new Table.TableId(5), new Local.LocalId(2), 1),
        Table.CreateTestTable(new Table.TableId(6), new Local.LocalId(2), 2),
        Table.CreateTestTable(new Table.TableId(7), new Local.LocalId(2), 3),
        Table.CreateTestTable(new Table.TableId(8), new Local.LocalId(3), 1),
        Table.CreateTestTable(new Table.TableId(9), new Local.LocalId(3), 2),
        Table.CreateTestTable(new Table.TableId(10), new Local.LocalId(3), 3),
    };
    private static readonly List<Menu> _menus = new()
    {
        Menu.CreateTestMenu(new Menu.MenuId(1), new Uri("http://example.com")),
        Menu.CreateTestMenu(new Menu.MenuId(2), new Uri("http://example.com")),
        Menu.CreateTestMenu(new Menu.MenuId(3), null),
        Menu.CreateTestMenu(new Menu.MenuId(4), new Uri("http://example.com")),
    };

    private static readonly Address _adress = new Address(new NotEmptyString("test"),
                                                          new NotEmptyString("test"),
                                                          new NotEmptyString("test"));
    
    [Fact]
    public void CreateLocal_WhenEventEndGreaterThenEventStart_ShouldThrow()
    {
        //Arrange
        DateTimeOffset currentTime = DateTimeOffset.Now;
        // Act and Assert
        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            new Local(Guid.NewGuid(),
                      "test",
                      _adress,
                      currentTime,
                      currentTime.AddHours(-5),
                      _tables,
                      _menus);
        });
    }

    [Fact]
    public void AddTable_WhenTableAlreadyExists_ShouldThrow()
    {
        //Arrange
        var table = Table.CreateTestTable(new Table.TableId(1), new Local.LocalId(1), 1);
        var local = CreateTestLocal();
        //Act and Assert
        Assert.Throws<InvalidOperationException>(() => 
        {
            local.AddTable(table);
        });
    }

    [Fact]
    public void AddPartecipantToTable_WhenPartecipantIsAlreadyInAnyOtherTable_ShouldThrow()
    {
        //Arrange
        var guid = Guid.NewGuid();
        var table = Table.CreateTestTable(new Table.TableId(1), new Local.LocalId(1), 1);
        var local = CreateTestLocal();
        local.AddPartecipantToTable(guid, table);
        //Act and Assert
        Assert.Throws<InvalidOperationException>(() =>
        {
            local.AddPartecipantToTable(guid, table);
        });
    }
    [Fact]
    public void AddPartecipantToTable_WhenTableIsNotFoundInLocal_ShouldThrow()
    {
        //Arrange
        var guid = Guid.NewGuid();
        var table = Table.CreateTestTable(new Table.TableId(15), new Local.LocalId(1), 1);
        var local = CreateTestLocal();       
        //Act and Assert
        Assert.Throws<InvalidOperationException>(() =>
        {
            local.AddPartecipantToTable(guid, table);
        });
    }
    private Local CreateTestLocal()
    {
        return new Local(Guid.NewGuid(),
                         "test",
                         _adress,
                         DateTimeOffset.Now,
                         DateTimeOffset.Now.AddHours(5),
                         _tables,
                         _menus);
    }
}
