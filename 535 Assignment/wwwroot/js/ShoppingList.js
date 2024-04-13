window.addEventListener('load', async (e) => {
    SetShoppingDDL();
    document.getElementById('createListForm').addEventListener('submit', async (e) => {
        await handleNewList(e)
    })
})

window.addEventListener('load', async (e) => {
    console.log("reached1")

    document.getElementById('AddListButton').addEventListener('click', async (e) => {
        console.log("reached2")
        $('#createListModal').modal('show')
    })
})

window.addEventListener('load', async (e) => {
    document.getElementById('RemoveListButton').addEventListener('click', async (e) => {
        removeShoppingList()
    })
})  

//Remove an Item from the list
async function removeItem(itemId) {
    let listId = sessionStorage.getItem("listID");

    let shoppingListItem = {
        ListId: listId,
        ItemId: itemId,
    }

    let result = await advFetch('/ShoppingList/RemoveItemFromList', {
        method: 'DELETE',
        headers: {
            'content-type': 'application/json'
        },
        body: JSON.stringify(shoppingListItem)
    });

    if (result.ok) {
        GreatFetch()
    }
}

//Removes a shoppinglist from the database and the dropdown list.
async function removeShoppingList() {{
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
}
    let listID = sessionStorage.getItem("listID");

    let result = await advFetch('/ShoppingList/RemoveList?listID=' + listID, {
        method: 'DELETE',
        headers: {
            'content-type': 'application/json'
        },
        body: JSON.stringify(listID)
    });

    if (result.ok) {
        await SetShoppingDDL();
    }
}

//Performs clientside validation to ensure lists are created properly.
async function handleNewList(e) {
    e.preventDefault();

    let nullCheck = e.target["listName"].value;
    let listName = nullCheck.trim();

    if ( listName == "") {
        return alert("List requires a name.")
    }

    let result = await advFetch('/ShoppingList/AddNewList', {
        method: 'POST',
        headers: {
            'content-type': 'application/json'
        },
        body: JSON.stringify(e.target["listName"].value)
    });

    if (result.ok) {
        await SetShoppingDDL();
        $('#createListModal').modal('hide');
    }
}

//Sets the details of the dropdown list
async function SetShoppingDDL() {
    let result = await advFetch('/ShoppingList/ShoppingListDDL');
    let htmlResult = await result.text();
    document.getElementById('selectContainer').innerHTML = htmlResult;

    let ddlContainer = document.getElementById('selectContainer')
    let ddl = ddlContainer.querySelector('select');
    ddl.addEventListener('change', async (e) => {
        handleDDLChange(e);
    })
}
//Saves the details of the dropdown list to the session to 'remember' what is selected
async function handleDDLChange(e) {

    let selectedOption = e.target.selectedOptions[0]
    sessionStorage.setItem('listID', selectedOption.value)
    sessionStorage.setItem('listName', selectedOption.text)

    GreatFetch()
}

//A fetch alternative for setting buttons that require certain values. 
//Part of abiding by the Content Security Policy.
async function GreatFetch() {
    console.log("CheckFetch")
    let result = await advFetch('/ShoppingList/GetListItems?listID=' + sessionStorage.getItem('listID'));
    let htmlResult = await result.text();
    document.getElementById('shoppingListContainer').innerHTML = htmlResult

    console.log("CheckFetch2")

    var buttons = document.querySelectorAll('#RemoveItemButton')
    console.log(buttons)

    var buttonArray = Array.from(buttons)
    console.log(buttonArray)

    buttonArray.forEach((Item, Index) => {
        console.log("ForeachCheck")
        Item.addEventListener('click', async (e) => {
            console.log(Item.dataset.value)
            removeItem(Item.dataset.value)
        })
    })
}