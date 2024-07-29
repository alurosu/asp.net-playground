using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace app.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int ItemsPerPage { get; set; } = 10; // Number of items per page
        public List<string> Items { get; set; }

        public void OnGet(int p = 1)
        {
            // Sample data
            var allItems = Enumerable.Range(1, 50).Select(i => $"Item {i}").ToList();

            CurrentPage = p;
            TotalPages = (int)System.Math.Ceiling(allItems.Count / (double)ItemsPerPage);

            Items = allItems.Skip((CurrentPage - 1) * ItemsPerPage).Take(ItemsPerPage).ToList();
        }
    }
}
