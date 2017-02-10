using System;
using System.Web;
using System.Web.Mvc;
using Nop.Core.Data;
using Nop.Plugin.Misc.AddFootwear.Domain;
using Nop.Plugin.Misc.AddFootwear.Infrastructure;
using Nop.Services.Catalog;
using Nop.Services.Stores;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Misc.AddFootwear.Controller
{
    public class FootwearController : BasePluginController
    {
        private IRepository<Item> _itemRepo;
        private IProductService _product;
        private readonly IStoreService _store;
        private readonly ItemParser _parser;

        public FootwearController(IRepository<Item> itemRepo,
            IProductService product, IStoreService store)
        {
            _itemRepo = itemRepo;
            _product = product;
            _store = store;
            _parser=new ItemParser(_product);
        }

        public ActionResult Manage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadExcel(HttpPostedFileBase fileUpload)
        {
            try
            {
                if (fileUpload != null)
                {
                    if (fileUpload.ContentType == "application/vnd.ms-excel" ||
                        fileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        _parser.ImportProductsFromXlsx(fileUpload.InputStream);
                    }
                }
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
            }
            return RedirectToAction("Manage");
        }
    }
}