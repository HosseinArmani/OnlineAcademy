using Academy.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineAcademy.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class InvoiceController : Controller
    {
        IOrderServise _orderServise;
        public InvoiceController(IOrderServise orderServise)
        {
            _orderServise = orderServise;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ShowInvoice(int id)
        {
            var order = _orderServise.GetOrderForUserPanel(User.Identity.Name, id);

            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }
    }
}
