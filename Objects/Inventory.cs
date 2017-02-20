using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace InventoryList.Objects
{
    public class Inventory
    {
        public string input;
        public int id;

        public Inventory (string newInput, int newId = 0)
        {
            input = newInput;
            id = newId;
        }
        public override bool Equals(System.Object otherInventory)
        {
            if (!(otherInventory is Inventory))
            {
                return false;
            }
            else
            {
                Inventory newInventory = (Inventory) otherInventory;
                bool idEquality = (this.GetId() == newInventory.GetId());
                bool descriptionEquality = (this.GetDescription() == newInventory.GetDescription());
                return (idEquality && descriptionEquality);
            }
        }
        public int GetId()
        {
          return id;
        }
        public string GetDescription()
        {
            return input;
        }
        public static List<Inventory> GetAll()
        {
            List<Inventory> allInventories = new List<Inventory>{};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM inventory_item;", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int inventoryId = rdr.GetInt32(0);
                string inventoryDescription = rdr.GetString(1);
                Inventory newInventory = new Inventory(inventoryDescription, inventoryId);
                allInventories.Add(newInventory);
            }

            if (rdr != null)
            {
                rdr.Close();
            }

            if (conn != null)
            {
                conn.Close();
            }

            return allInventories;
        }
        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO inventory_item (name) OUTPUT INSERTED.id VALUES (@InventoryDescription);", conn);

            SqlParameter descriptionParameter = new SqlParameter();
            descriptionParameter.ParameterName = "@InventoryDescription";
            descriptionParameter.Value = this.GetDescription();
            cmd.Parameters.Add(descriptionParameter);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                this.id = rdr.GetInt32(0);
            }
            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
        }
        public static void DeleteAll()
        {
          SqlConnection conn = DB.Connection();
          conn.Open();
          SqlCommand cmd = new SqlCommand("DELETE FROM inventory_item;", conn);
          cmd.ExecuteNonQuery();
          conn.Close();
        }
        public static Inventory Find(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM inventory_item WHERE id = @InventoryId;", conn);
            SqlParameter inventoryIdParameter = new SqlParameter();
            inventoryIdParameter.ParameterName = "@InventoryId";
            inventoryIdParameter.Value = id.ToString();
            cmd.Parameters.Add(inventoryIdParameter);
            SqlDataReader rdr = cmd.ExecuteReader();

            int foundInventoryId = 0;
            string foundInventoryDescription = null;
            while(rdr.Read())
            {
                foundInventoryId = rdr.GetInt32(0);
                foundInventoryDescription = rdr.GetString(1);
            }
            Inventory foundInventory = new Inventory(foundInventoryDescription, foundInventoryId);

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }

            return foundInventory;
        }
    }
}
