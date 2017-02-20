using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using InventoryList.Objects;

namespace InventoryList
{
  public class InventoryTest : IDisposable
  {
    public InventoryTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=inventory_test;Integrated Security=SSPI;";
    }
    public void Dispose()
    {
      Inventory.DeleteAll();
    }
    [Fact]
    public void Test_DatabaseEmpty()
    {
        //Arrange //Act
        int result = Inventory.GetAll().Count;

        //Assert
        Assert.Equal(0, result);
    }
    [Fact]
    public void Save_HaveSavedFirstItem_ListHoldsFirstItem()
    {
      //Arrange //Act
      Inventory firstInventory = new Inventory("green");
      Inventory secondInventory = new Inventory("green");
      //Assert
      Assert.Equal(firstInventory, secondInventory);
    }
    [Fact]
    public void Test_Save_SavesToDatabase()
    {
      //Arrange
      Inventory testInventory = new Inventory("red");

      //Act
      testInventory.Save();
      List<Inventory> result = Inventory.GetAll();
      List<Inventory> testList = new List<Inventory>{testInventory};

      //Assert
      Assert.Equal(testList, result);
    }
    [Fact]
    public void Test_Save_AssignsIdToObject()
    {
      //Arrange
      Inventory testInventory = new Inventory("blue");

      //Act
      testInventory.Save();
      Inventory savedInventory = Inventory.GetAll()[0];

      int result = savedInventory.GetId();
      int testId = testInventory.GetId();

      //Assert
      Assert.Equal(testId, result);
    }
    [Fact]
    public void Test_Find_FindsInventoryInDatabase()
    {
      //Arrange
      Inventory testInventory = new Inventory("yellow");
      testInventory.Save();

      //Act
      Inventory foundInventory = Inventory.Find(testInventory.GetId());

      //Assert
      Assert.Equal(testInventory, foundInventory);
    }

  }
}
