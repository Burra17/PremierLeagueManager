using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PremierLeagueManager
{
    public class DataStore <T> // Generisk klass för att hantera datalagring
    {
        private List<T> items; // Lista för att lagra objekt av typ T
        // Konstruktor för att initiera datalagringen
        public DataStore()
        {
            items = new List<T>();
        }
        // Metod för att lägga till ett objekt i datalagringen
        public void AddItem(T item)
        {
            items.Add(item);
        }
        // Metod för att ta bort ett objekt från datalagringen
        public bool RemoveItem(T item)
        {
            return items.Remove(item);
        }

        // Metod för att hämta alla objekt från datalagringen
        public List<T> GetAllItems()
        {
            return items;
        }

        // Metod för att hitta objekt baserat på ett villkor
        public List<T> FindItems(Func<T, bool> predicate)
        {
            return items.Where(predicate).ToList();
        }

        // Metod för att spara till json-fil
        public void SaveToJson(string filePath)
        {
            try 
            {
                var json = System.Text.Json.JsonSerializer.Serialize(items);
                System.IO.File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to JSON: {ex.Message}");
            }
        }

        // Metod för att ladda från json-fil
        public void LoadFromJson(string filePath)
        {
            try
            {
            if (System.IO.File.Exists(filePath))
            {
                var json = System.IO.File.ReadAllText(filePath);
                items = System.Text.Json.JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
            }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading from JSON: {ex.Message}");
            }
        }
    }
}
