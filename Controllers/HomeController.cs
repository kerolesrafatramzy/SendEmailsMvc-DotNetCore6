using Microsoft.AspNetCore.Mvc;
using SendEmailsDotNetCore6.Dtos;
using SendEmailsDotNetCore6.Models;
using SendEmailsDotNetCore6.Services;
using System.Diagnostics;

namespace SendEmailsDotNetCore6.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMailingService _mailingService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IMailingService mailingService)
        {
            _logger = logger;
            _mailingService = mailingService;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMail(MailRequestDto dto)
        {

            await _mailingService.SendEmailAsync(dto.ToEmail, dto.Subject, dto.Body, dto.Attachments);

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMailTemplate(WelcomeRequestDto dto)
        {

            var filePath = $"{Directory.GetCurrentDirectory()}\\Template\\email.html";
            var str = new StreamReader(filePath);

            var mailtext = str.ReadToEnd();
            str.Close();

            mailtext = mailtext.Replace("[username]", dto.UserName).Replace("[email]", dto.Email);

            await _mailingService.SendEmailAsync(dto.Email, "Welcome to our website", mailtext);

            return RedirectToAction(nameof(Index));
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}