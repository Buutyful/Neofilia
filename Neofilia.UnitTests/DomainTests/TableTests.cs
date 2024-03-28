using Neofilia.Domain;

namespace Neofilia.UnitTests.DomainTests;

public class TableTests
{
    [Fact]
    public void TableEquality_WhenTablesAreEqual_ShouldReturnTrue()
    {
        //Arrange
        //Tables are equal when their unique id is equal (assigned on db insertion)
        var table1 = Table.CreateTestTable(new Table.TableId(4), new Local.LocalId(2), 3);
        var table2 = Table.CreateTestTable(new Table.TableId(4), new Local.LocalId(1), 5);

        var table3 = Table.CreateTestTableId(new Table.TableId(4));
        var table4 = Table.CreateTestTableId(new Table.TableId(3));

        //Act and Assert
        Assert.True(table1.Equals(table2));
        Assert.True(table1 == table2);
        Assert.False(table1 != table2);
        Assert.True(table1.Equals((object)table2));
        Assert.True(table2.Equals(table3));
        Assert.True(table3 != table4);
        Assert.False(table3.Equals(table4));
        Assert.False(table3 == table4);
    }
}