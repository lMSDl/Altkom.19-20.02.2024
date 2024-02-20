using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class Garden
    {
        public int Size { get; }
        private ICollection<string> Items { get; }
        private ILogger? Logger { get; }

        public Garden(int size, ILogger? logger = null)
        {
            Size = size;
            Items = new List<string>();
            Logger = logger;
        }

        public bool Plant(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Roślina musi posiadać nazwę!", nameof(name));

            if (Items.Count() >= Size)
            {
                Logger?.Log($"Brak miejsca w ogrodzie na {name}");
                return false;
            }

            if (Items.Contains(name))
            {
                var newName = name + (Items.Count(x => x.StartsWith(name)) + 1);
                Logger?.Log($"Roślina {name} zmieniła nazwę na {newName}");
                name = newName;
            }

            Items.Add(name);
            Logger?.Log($"Roślina {name} została dodana do ogrodu");

            return true;
        }

        public ICollection<string> GetPlants()
        {
            return Items.ToList();
        }

        public bool Remove(string name)
        {
            if (!Items.Contains(name))
                return false;
            Items.Remove(name);
            Logger?.Log($"Roślina {name} została usunięta z ogrodu");
            return true;
        }

        public void Clear()
        {
            Items.Clear();
        }

        public int Count()
        {
            return Items.Count();
        }

        public string? GetLastLog()
        {
            string? log = Logger?.GetLogsAsync(new DateTime(), DateTime.Now).Result;
            return log?.Split('\n').Last();
        }
    }
}
