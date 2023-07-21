using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly AnalysisDbContext _context;
        public HomeController(AnalysisDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Products> products = _context.products.ToList();
           /* Console.Write("Enter a text: ");
            string inputText = Console.ReadLine();

            int sentimentScore = AnalyzeInput(inputText);
            int sentimentLabel = GetSentiment(sentimentScore);

            Console.WriteLine($"Sentiment: {sentimentLabel}");

            static int AnalyzeInput(string text)
            {
                string FilePath = "C:\\Users\\Simran Kaur\\Documents\\Team_C\\sentimentanalysis\\WebApplication1\\List\\AFINN-111.txt";

                var ReadFile = System.IO.File.ReadLines(FilePath)
                                .Select(line => line.Split('\t'))
                                .ToDictionary(parts => parts[0], parts => int.Parse(parts[1]));
                int sentimentScore = text.Split()
                                         .Select(word => ReadFile.ContainsKey(word) ? ReadFile[word] : 0)
                                         .Sum();

                return sentimentScore;
            }

            static int GetSentiment(int sentimentScore)
            {
                int score = sentimentScore;

                return score;
                   
            }*/

            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}