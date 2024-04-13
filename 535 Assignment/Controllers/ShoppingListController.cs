using _535_Assignment.Models;
using _535_Assignment.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace _535_Assignment.Controllers
{
    public class ShoppingListController : Controller
    {
        private readonly ContextShopping _contextShopping;
        private readonly SanitiserService _sanitiserService;

        public ShoppingListController(ContextShopping context, SanitiserService sanitiserService)
        {
            _contextShopping = context;
            _sanitiserService = sanitiserService;
        }

        [Authorize]
        public IActionResult Index()
        {
            ViewBag.UserId = HttpContext.User.Claims.Where(c => c.Type == "ID").FirstOrDefault().Value;

            return View(_contextShopping.ShoppingList.AsEnumerable());
        }

        /// <summary>
        /// Handles the drop down list and its contents, showing all lists assigned to the currently authenticated user. 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<IActionResult> ShoppingListDDL()
        {
            var id = HttpContext.User.Claims.Where(c => c.Type == "ID").FirstOrDefault().Value;
            var intid = int.Parse(id);

            var list = _contextShopping.ShoppingList.Where(c => c.UserId == intid).ToList();
            var selectList = list.Select(c => new SelectListItem
            {
                Text = c.ListName + ": Created on - " + c.Created,
                Value = c.ShoppingListId.ToString()
            }).ToList();

            ViewBag.SelectList = selectList;

            return PartialView("_ShoppingListDDL");
        }

        /// <summary>
        /// Add's a new list to the database, it's name defined by the user and its date created being set to the current time. 
        /// </summary>
        /// <param name="listName"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddNewList([FromBody] string listName)
        {

            var id = HttpContext.User.Claims.Where(c => c.Type == "ID").FirstOrDefault().Value;
            var intid = int.Parse(id);

            ShoppingList newList = new ShoppingList()
            {
                ListName = listName,
                UserId = intid
            };
            _contextShopping.ShoppingList.Add(newList);
            _contextShopping.SaveChanges();
            return Ok();
        }

        /// <summary>
        /// Removes a list the user has access to.
        /// </summary>
        /// <param name="listID"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> RemoveList([FromBody] int listID)
        {

            if(listID != null)
            {
                var selectedList = _contextShopping.ShoppingList.Where(c => c.ShoppingListId == listID).FirstOrDefault();
                _contextShopping.ShoppingList.Remove(selectedList);
                _contextShopping.SaveChanges();
                return Ok();
            }
            return BadRequest();    
        }


        /// <summary>
        /// Remove's a product from the user's shopping list.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> RemoveItemFromList([FromBody] ItemsToListConnection item)
        {
            var listItem = _contextShopping.ItemsInList.Where(c => c.ListId == item.ListId && c.ItemId == item.ItemId).FirstOrDefault();
            
            if(listItem != null)
            {
                _contextShopping.ItemsInList.Remove(listItem);
                _contextShopping.SaveChanges();
                return Ok();
            }
            
            return BadRequest();
        }

        /// <summary>
        /// Add's an item to a list, from the products page.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [Authorize]
        public async Task<ActionResult> AddItemToList([FromBody] ItemsToListConnection item)
        {
            if(_contextShopping.ItemsInList.Any(c => c.ListId == item.ListId && c.ItemId == item.ItemId))
            {
                return BadRequest();
            }

            _contextShopping.ItemsInList.Add(item);
            _contextShopping.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Checks the database for all items in a shopping list
        /// </summary>
        /// <param name="listId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetListItems([FromQuery] int listId)
        {

            List<Item> items = _contextShopping.ItemsInList.Include(c => c.item).ThenInclude(c => c.ItemList)
                .Where(c => c.ListId == listId).Select(c => c.item).ToList();

            return PartialView("_ShoppingListTable", items);
        }

        [Authorize (Roles = "Admin")]
        public async Task<IActionResult> EditItem(int id)
        {
            try
            {
                if (id == 0)
            {
                return BadRequest();
            }

                var item = await _contextShopping.Items.FirstOrDefaultAsync(c => c.ItemId == id);

                return item != null ? PartialView("_EditItem", item) : RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Edit's an item in the product list. 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> EditItem([FromQuery] int id, [FromBody] Item item)
        {
            try
            {
                Item sanitisedItem = new Item();

                sanitisedItem = item;
                var sanitisedName = _sanitiserService.Sanitiser.Sanitize(item.ItemName);
                var sanitisedUnit = _sanitiserService.Sanitiser.Sanitize(item.Unit);
                sanitisedItem.ItemName = sanitisedName;
                sanitisedItem.Unit = sanitisedUnit;

                if (id == 0)
                {
                    return BadRequest();
                }
                _contextShopping.Items.Update(sanitisedItem);
                _contextShopping.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
