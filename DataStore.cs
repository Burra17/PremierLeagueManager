using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace PremierLeagueManager
{
    public class DataStore<T> // Generisk klass för datalagring
    {
        public List<T> Items { get; private set; } = new();

        // === Lägg till objekt ===
        public void AddItem(T item)
        {
            Items.Add(item);
        }

        // === Ta bort objekt ===
        public bool RemoveItem(T item)
        {
            return Items.Remove(item);
        }

        // === Hämta alla objekt ===
        public List<T> GetAllItems() => Items;

        // === Hitta objekt utifrån villkor ===
        public List<T> FindItems(Func<T, bool> predicate)
        {
            return Items.Where(predicate).ToList();
        }

        // === Rensa listan ===
        public void Clear() => Items.Clear();

        // === Spara till JSON ===
        public void SaveToJson(string filePath)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true }; // snygg JSON
                var json = JsonSerializer.Serialize(Items, options);
                File.WriteAllText(filePath, json);
                Console.WriteLine($"✔ Saved {Items.Count} items to {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to JSON: {ex.Message}");
            }
        }

        // === Läs in från JSON ===
        public void LoadFromJson(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    var json = File.ReadAllText(filePath);
                    Items = JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading from JSON: {ex.Message}");
            }
        }
    }
}
