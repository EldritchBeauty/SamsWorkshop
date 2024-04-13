
using _535_Assignment.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _535_Assignment.Controllers
{
    public class ItemController : Controller
    {
        private readonly ContextShopping _contextShopping;

        public ItemController(ContextShopping context)
        {
            _contextShopping = context;
        }

        // GET: ItemController
        public ActionResult Index()
        {
            return View(_contextShopping.Items.AsEnumerable());
        }
       
    }
}
