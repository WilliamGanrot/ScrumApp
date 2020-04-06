using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FFImageLoading;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ScrumApp.Models;

namespace ScrumApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        public HomeController(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult test()
        {
            List<string> colorList = new List<string> {"Yellow", "Blue","Red","Green","Brown","Azure","Ivory","Teal","Purple","Orange","Maroon","Aquamarine","Coral","Fuchsia", "Wheat","Lime","Crimson","Khaki","Magenta","Olden","Plum","Olive", "Cyan"};

            Random random = new Random();
            int index = random.Next(colorList.Count());

            int width = 200;
            int height = 200;

            Bitmap bmp = new Bitmap(width, height);

            Graphics graphics = Graphics.FromImage(bmp);
            Rectangle rect = new Rectangle(0, 0, width, height);

            Color color = Color.FromName(colorList[index]);
            Brush backgroundBrush = new SolidBrush(color);

            Color textColor = Color.FromName("White");
            Font font = new Font("Arial", 60, FontStyle.Bold, GraphicsUnit.Pixel); 
            SizeF textSize = graphics.MeasureString("WG", font);
            Brush textBrush = new SolidBrush(textColor);

            graphics.FillRectangle(backgroundBrush, rect);
            graphics.DrawString("WG", font, textBrush, (int)((width - textSize.Width) / 2), (int)((height - textSize.Height) / 2));

            string uploadsDir = Path.Combine(webHostEnvironment.WebRootPath, "media\\Users");
            string filePath = Path.Combine(uploadsDir, "coolimg.jpg");

            bmp.Save(filePath);

            return Ok();
        }

    }
}
