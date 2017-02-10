using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Services.Catalog;
using Nop.Services.ExportImport;
using Nop.Services.ExportImport.Help;
using OfficeOpenXml;

namespace Nop.Plugin.Misc.AddFootwear.Infrastructure
{
    public class ItemParser
    {
        private readonly IProductService _product;

        public ItemParser(IProductService product)
        {
            _product = product;
        }

        public void ImportProductsFromXlsx(Stream stream)
        {
            //var start = DateTime.Now;
            using (var xlPackage = new ExcelPackage(stream))
            {
                // get the first worksheet in the workbook
                var worksheet = xlPackage.Workbook.Worksheets.FirstOrDefault();
                if (worksheet == null)
                    throw new NopException("No worksheet found");

                //the columns
                var properties = new List<PropertyByName<Product>>();
                var poz = 1;
                while (true)
                {
                    try
                    {
                        var cell = worksheet.Cells[1, poz];

                        if (cell == null || cell.Value == null || String.IsNullOrEmpty(cell.Value.ToString()))
                            break;

                        poz += 1;
                        properties.Add(new PropertyByName<Product>(cell.Value.ToString()));
                    }
                    catch
                    {
                        break;
                    }
                }

                var manager = new PropertyManager<Product>(properties.ToArray());
                var endRow = 2;
                //find end of data
                while (true)
                {
                    var allColumnsAreEmpty = manager.GetProperties
                        .Select(property => worksheet.Cells[endRow, property.PropertyOrderPosition])
                        .All(cell => cell == null || cell.Value == null || String.IsNullOrEmpty(cell.Value.ToString()));

                    if (allColumnsAreEmpty)
                        break;

                    endRow++;
                }


                for (var iRow = 2; iRow < endRow; iRow++)
                {
                    manager.ReadFromXlsx(worksheet, iRow, ExportProductAttribute.ProducAttributeCellOffset);
                    var website = worksheet.GetValue(iRow, manager.GetProperty("Website").PropertyOrderPosition);
                    var itemId = worksheet.GetValue(iRow, manager.GetProperty("Id").PropertyOrderPosition);
                    var title = worksheet.GetValue(iRow, manager.GetProperty("Title").PropertyOrderPosition);
                    var itemUrl = worksheet.GetValue(iRow, manager.GetProperty("Item Url").PropertyOrderPosition);
                    var imageUrls = worksheet.GetValue(iRow, manager.GetProperty("ImageUrls").PropertyOrderPosition);
                    var price = worksheet.GetValue(iRow, manager.GetProperty("Price").PropertyOrderPosition);
                    var discount = worksheet.GetValue(iRow, manager.GetProperty("Discount").PropertyOrderPosition);
                    var type = worksheet.GetValue(iRow, manager.GetProperty("Type").PropertyOrderPosition);
                    var subType = worksheet.GetValue(iRow, manager.GetProperty("SubType").PropertyOrderPosition);
                    var brand = worksheet.GetValue(iRow, manager.GetProperty("Brand").PropertyOrderPosition);
                    var sizes = worksheet.GetValue(iRow, manager.GetProperty("Sizes").PropertyOrderPosition);
                    var Properties = worksheet.GetValue(iRow, manager.GetProperty("Properties").PropertyOrderPosition);


                    var product = new Product
                    {
                        CreatedOnUtc = DateTime.UtcNow,
                        BasepriceBaseAmount = Convert.ToDecimal(price),
                        FullDescription = Properties.ToString(),
                        MarkAsNew = true,
                        MarkAsNewStartDateTimeUtc = DateTime.UtcNow,
                        Name = title.ToString(),
                        MetaDescription = itemUrl.ToString(),
                        MetaTitle = title.ToString(),
                        Price = Convert.ToDecimal(price)
                    };
                    _product.InsertProduct(product);
                }
            }
        }
    }
}