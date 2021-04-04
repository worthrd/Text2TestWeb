using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Text2TestWebApp.Luis;
using Text2TestWebApp.Models;

namespace Text2TestWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult TextToTestConverter(String Utterance)
        {
            Intent userIntent = new Intent();
            CodeConverter codeConverter = new CodeConverter();
            
            userIntent.Utterance = Utterance;
            string[] lines = Utterance.Split(new[] { "\r\n"},StringSplitOptions.None);
            if (lines != null && lines.Count() > 0)
            {
                String codeResult = String.Empty;
                foreach (var item in lines)
                {
                    Task<String> intentTask = IntentManager.GetIntent(item);
                    codeResult += codeConverter.ConvertToCode(intentTask.Result);
                }
                userIntent.IntentValue = codeResult;
            }
            else
            {
                Task<String> intentTask = IntentManager.GetIntent(Utterance);
                String codeResult = codeConverter.ConvertToCode(intentTask.Result);
                userIntent.IntentValue = codeResult;
            }
           
            
            return View(userIntent);

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
