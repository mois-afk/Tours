namespace Tours.Service
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using PdfSharpCore.Drawing;
    using PdfSharpCore.Fonts;
    using PdfSharpCore.Pdf;
    using PdfSharpCore.Pdf.Advanced;
    using Tours.Models;

    public class PdfService
    {
        public byte[] CreateUserOrdersReport(List<OrderModel> model)
        {
            using (var stream = new MemoryStream())
            {
                var document = new PdfDocument();
                var page = document.AddPage();
                var gfx = XGraphics.FromPdfPage(page);
                var font = new XFont("Arial", 12);

                XRect rect = new XRect(40, 40, page.Width - 80, page.Height - 80);
                gfx.DrawRectangle(XBrushes.White, rect);

                gfx.DrawString($"Выписка по заказам пользователя {model[0].Username}", font, XBrushes.Black, rect.Left, rect.Top, XStringFormats.TopLeft);

                // Создаем таблицу
                int rowHeight = 40;
                int x = (int)rect.Left;
                int y = (int)rect.Top + 40;

                gfx.DrawString("Дата", font, XBrushes.Black, x, y);
                gfx.DrawString("Описание", font, XBrushes.Black, x + 100, y);
                gfx.DrawString("Сумма", font, XBrushes.Black, x + 300, y);

                y += rowHeight;

                double totalAmount = 0;

                foreach (var order in model)
                {
                    gfx.DrawString(order.Date.ToString("dd.MM.yyyy"), font, XBrushes.Black, x, y);

                    string tourDescriptions = " ";
                    foreach (var group in order.TourList.GroupBy(t => t.TourName))
                    {
                        tourDescriptions += $"{group.Key} ({group.Count()})";
                    }

                    gfx.DrawString(tourDescriptions, font, XBrushes.Black, x + 100, y);

                    gfx.DrawString(order.TotalPrice.ToString("C", CultureInfo.CurrentCulture), font, XBrushes.Black, x + 300, y);

                    y += rowHeight;
                    totalAmount += order.TotalPrice;
                }

                // Выводим общий итог
                gfx.DrawString($"Общий итог: {totalAmount.ToString("C", CultureInfo.CurrentCulture)}", font, XBrushes.Black, x, y + 20);

                document.Save(stream, false);
                return stream.ToArray();
            }
        }

        public byte[] CreateTourPriceList(List<Tour> tours)
        {
            using (var stream = new MemoryStream())
            {
                var document = new PdfDocument();
                var page = document.AddPage();
                var gfx = XGraphics.FromPdfPage(page);
                var font = new XFont("Arial", 10); // Уменьшил размер шрифта для более компактного отображения

                XRect rect = new XRect(40, 40, page.Width - 80, page.Height - 80);
                gfx.DrawRectangle(XBrushes.White, rect);

                gfx.DrawString("Прайс-лист всех туров", new XFont("Arial", 14, XFontStyle.Bold), XBrushes.Black, rect.Left, rect.Top, XStringFormats.TopLeft);

                // Создаем таблицу
                int rowHeight = 20;
                int x = (int)rect.Left;
                int y = (int)rect.Top + 40;

                gfx.DrawString("Наименование", font, XBrushes.Black, x, y);
                gfx.DrawString("Описание", font, XBrushes.Black, x + 120, y); // Уменьшил смещение для описания
                gfx.DrawString("Дата начала", font, XBrushes.Black, x + 240, y); // Уменьшил смещение для даты начала
                gfx.DrawString("Дата конца", font, XBrushes.Black, x + 360, y); // Уменьшил смещение для даты конца
                gfx.DrawString("Цена", font, XBrushes.Black, x + 480, y); // Уменьшил смещение для цены

                y += rowHeight;

                foreach (var tour in tours)
                {
                    gfx.DrawString(tour.TourName, font, XBrushes.Black, x, y);
                    gfx.DrawString(tour.TourDescription, font, XBrushes.Black, new XRect(x + 120, y, 100, 40), XStringFormats.TopLeft); // Используем XRect для ограничения размеров ячейки
                    gfx.DrawString(tour.StartDate.ToString("dd.MM.yyyy"), font, XBrushes.Black, x + 240, y);
                    gfx.DrawString(tour.EndDate.ToString("dd.MM.yyyy"), font, XBrushes.Black, x + 360, y);
                    gfx.DrawString(tour.TourPrice.ToString("C", CultureInfo.CurrentCulture), font, XBrushes.Black, x + 480, y);

                    y += rowHeight;
                }

                document.Save(stream, false);
                return stream.ToArray();
            }
        }

        public byte[] CreateRevenueReport(List<OrderModel> orders, DateTime startDate, DateTime endDate)
        {
            using (var stream = new MemoryStream())
            {
                var document = new PdfDocument();
                var page = document.AddPage();
                var gfx = XGraphics.FromPdfPage(page);
                var font = new XFont("Arial", 12);

                XRect rect = new XRect(40, 40, page.Width - 80, page.Height - 80);
                gfx.DrawRectangle(XBrushes.White, rect);

                gfx.DrawString($"Отчет о выручке за период: {startDate:dd.MM.yyyy} - {endDate:dd.MM.yyyy}", font, XBrushes.Black, rect.Left, rect.Top, XStringFormats.TopLeft);

                // Фильтрация заказов по датам
                var filteredOrders = orders.Where(o => o.Date >= startDate && o.Date <= endDate).ToList();

                // Группировка по месяцам и вычисление суммы
                var groupedByMonth = filteredOrders.GroupBy(o => new { o.Date.Year, o.Date.Month })
                    .Select(g => new
                    {
                        Year = g.Key.Year,
                        Month = g.Key.Month,
                        TotalAmount = g.Sum(o => o.TotalPrice),
                    }).OrderBy(g => g.Year).ThenBy(g => g.Month).ToList();

                // Создаем таблицу
                int rowHeight = 20;
                int x = (int)rect.Left;
                int y = (int)rect.Top + 40;

                gfx.DrawString("Год", font, XBrushes.Black, x, y);
                gfx.DrawString("Месяц", font, XBrushes.Black, x + 100, y);
                gfx.DrawString("Сумма", font, XBrushes.Black, x + 200, y);

                y += rowHeight;

                double grandTotal = 0;

                foreach (var group in groupedByMonth)
                {
                    gfx.DrawString(group.Year.ToString(), font, XBrushes.Black, x, y);
                    gfx.DrawString(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(group.Month), font, XBrushes.Black, x + 100, y);
                    gfx.DrawString(group.TotalAmount.ToString("C", CultureInfo.CurrentCulture), font, XBrushes.Black, x + 200, y);

                    y += rowHeight;
                    grandTotal += group.TotalAmount;
                }

                // Выводим общий итог
                gfx.DrawString($"Общий итог: {grandTotal.ToString("C", CultureInfo.CurrentCulture)}", font, XBrushes.Black, x, y + 20);

                document.Save(stream, false);
                return stream.ToArray();
            }
        }
    }
}
