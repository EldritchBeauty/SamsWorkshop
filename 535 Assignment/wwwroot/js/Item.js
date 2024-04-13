window.addEventListener('load', (e) => {
    document.getElementById('AddItemToListForm').addEventListener('submit', async (e) => {
        handleAddItemToList(e);
    })
})

//Creates button to add item's to a list
window.addEventListener('load', (e) => {
    var buttons = document.querySelectorAll('#AddToListButton')

    var buttonArray = Array.from(buttons)

    buttonArray.forEach((Item, Index) => {
        Item.addEventListener('click', async (e) => {
            console.log(Item.dataset.value)
            addToList(Item.dataset.value)
        })
    })
})
//Creates button to show the edit item modal
window.addEventListener('load', (e) => {
    var buttons = document.querySelectorAll('#ShowEditModalButton')

    var buttonArray = Array.from(buttons)

    buttonArray.forEach((Item, Index) => {
        Item.addEventListener('click', async (e) => {
            console.log(Item.dataset.value)
            showEditModal(Item.dataset.value)
        })
    })
})
//Adds an item to a shopping list
async function addToList(itemId) {

    sessionStorage.setItem('selectedItemId', itemId);

    $('#addToListModal').modal('show');

    let result = await fetch('/ShoppingList/ShoppingListDDL');
    let htmlResult = await result.text();
    document.getElementById('ddlContainer').innerHTML = htmlResult;
}

//Performs some clientside validation to ensure items are added smoothly
async function handleAddItemToList(e) {
    e.preventDefault();


    let itemId = sessionStorage.getItem('selectedItemId');
    let listId = e.target['listList'].selectedOptions[0].value

    if (listId == 0) {
        return;
    }

    let quantity = e.target["productQuantity"].value;
    if (quantity < 1) {
        quantity = 1;
    }

    let shoppingListItem = {
        ListId: listId,
        ItemId: itemId,
        Quantity: quantity
    }

    let result = await fetch('/ShoppingList/AddItemToList', {
        method: 'POST',
        headers: {
            'content-type': 'application/json'
        },
        body: JSON.stringify(shoppingListItem)
    });

    if (result.ok) {
        $('#addToListModal').modal('hide');
    }
    else if (result.status == 400) {
        alert('That list already has that item!');
    }
}

//Shows Edit Modall, for editing products
async function showEditModal(id) {
    let response = await fetch('/ShoppingList/EditItem?id=' + id);

    let htmlResponse = await response.text();

    document.getElementById('editModalBody').innerHTML = htmlResponse;
    document.getElementById('editModalTitle').innerHTML = "Edit Item";

    let formReference = document.querySelector('form[action="/ShoppingList/EditItem"]');

    console.log(formReference)

    formReference.addEventListener('submit', (e) => { handleEditSubmit(e, id) });

    $('#editListModal').modal('show');
}

//Closes the edit modal after submitting the edit to the controller. 
async function handleEditSubmit(e, id) {
    e.preventDefault();

    let form = e.target;

    let Edititem = {
        ItemId: id,
        ItemName: form["ItemName"].value,
        Unit: form["Unit"].value,
        UnitPrice: form["UnitPrice"].value
    };

    let response = await fetch('/ShoppingList/EditItem?id=' + id, {
        method: 'PUT',
        headers: {
            'content-type': 'application/json'
        },
        body: JSON.stringify(Edititem)
    });

    
    $('#editListModal').modal('hide');

    window.location = "/Item"
}