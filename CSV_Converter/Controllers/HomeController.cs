using CSV_Converter.Models;
using CSV_Converter.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CSV_Converter.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        public HomeController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _orderRepository.GetOrdersAsync());
        }

        [HttpGet]
        public IActionResult UploadCsv()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> UploadCsv(IFormFile csvFile, [FromServices] IWebHostEnvironment hosting)
        {
            if (csvFile != null && csvFile.Length > 0)
            {
                try
                {
                    string path = Path.Combine(hosting.WebRootPath, "files", csvFile.FileName);

					using (var fileStream = new FileStream(path, FileMode.Create))
					{
						await csvFile.CopyToAsync(fileStream);
					}

					await _orderRepository.LoadData(csvFile.FileName);

					return RedirectToAction("UploadSuccess");
				}
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Ошибка при загрузке данных: {ex.Message}");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Пожалуйста, выберите файл для загрузки.");
            }

            return View();
        }


        public IActionResult UploadSuccess()
        {
            return View();
        }


        public async Task<IActionResult> Orders()
        {
			return View(await _orderRepository.GetOrdersAsync());
		}
    }
}
