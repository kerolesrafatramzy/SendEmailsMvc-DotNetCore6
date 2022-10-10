using Microsoft.AspNetCore.Mvc;
using SendEmailsDotNetCore6.Dtos;
using SendEmailsDotNetCore6.Services;

namespace SendEmailsDotNetCore6.Controllers
{
    public class MailingController : Controller
    {
        private readonly IMailingService _mailingService;
        public MailingController(IMailingService mailingService)
        {
            _mailingService = mailingService;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMail(MailRequestDto dto)
        {
            await _mailingService.SendEmailAsync(dto.ToEmail, dto.Subject, dto.Body, dto.Attachments);

            return RedirectToAction(nameof(Index));
        }

    }
}
