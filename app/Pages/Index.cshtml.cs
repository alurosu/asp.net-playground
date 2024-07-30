using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using app.Models;

namespace app.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private const string FilePath = "items.json";

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        [BindProperty]
        public string NewItem { get; set; }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int ItemsPerPage { get; set; } = 5;
        public List<Item> Items { get; set; } = new List<Item>();

        public void OnGet(int p = 1)
        {
            LoadItems();
            CurrentPage = p;
            TotalPages = (int)System.Math.Ceiling(Items.Count / (double)ItemsPerPage);

            Items = Items.Skip((CurrentPage - 1) * ItemsPerPage).Take(ItemsPerPage).ToList();
        }

        public IActionResult OnPost()
        {
            if (!string.IsNullOrEmpty(NewItem))
            {
                LoadItems();

                var newItem = new Item
                {
                    Id = Items.Any() ? Items.Max(i => i.Id) + 1 : 1,
                    Name = NewItem,
                    EncryptedData = EncryptData(NewItem)
                };

                Items.Insert(0, newItem);
                SaveItems();

                return RedirectToPage(new { p = 1 });
            }

            return Page();
        }

        private void LoadItems()
        {
            if (System.IO.File.Exists(FilePath))
            {
                var jsonData = System.IO.File.ReadAllText(FilePath);
                Items = JsonSerializer.Deserialize<List<Item>>(jsonData) ?? new List<Item>();
            }
            else
            {
                Items = new List<Item>();
            }
        }

        private void SaveItems()
        {
            var jsonData = JsonSerializer.Serialize(Items);
            System.IO.File.WriteAllText(FilePath, jsonData);
        }

        private string EncryptData(string input)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha1.ComputeHash(inputBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
