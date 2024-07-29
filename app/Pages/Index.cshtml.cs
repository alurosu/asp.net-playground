using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace app.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger) {
            _logger = logger;
        }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int ItemsPerPage { get; set; } = 10; // Number of items per page
        public List<string> Items { get; set; }

        public void OnGet(int p = 1) {
            // Sample data
            var allItems = Enumerable.Range(1, 50)
                             .Select(i => $"Item {i.ToString("D3")} - " + EncryptData(i.ToString()))
                             .ToList();

            CurrentPage = p;
            TotalPages = (int)System.Math.Ceiling(allItems.Count / (double)ItemsPerPage);

            Items = allItems.Skip((CurrentPage - 1) * ItemsPerPage).Take(ItemsPerPage).ToList();
        }

        private string EncryptData(string input) {
            using (SHA1 sha1 = SHA1.Create()) {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha1.ComputeHash(inputBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
