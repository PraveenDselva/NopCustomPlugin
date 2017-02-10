using Nop.Core.Data;
using Nop.Core.Plugins;
using Nop.Plugin.Misc.AddFootwear.Domain;
using Nop.Web.Framework.Menu;

namespace Nop.Plugin.Misc.AddFootwear
{
    public class AddFootwearPlugin : BasePlugin, IAdminMenuPlugin
    {
        private IRepository<Item> _itemRepo;

        public AddFootwearPlugin(IRepository<Item> itemRepo)
        {
            _itemRepo = itemRepo;
        }

        public void ManageSiteMap(SiteMapNode rootNode)
        {
            var node = new SiteMapNode {Visible = true, Title = "Add Footwear", Url = "/Footwear/Manage"};
            rootNode.ChildNodes.Insert(0, node);
        }
    }
}