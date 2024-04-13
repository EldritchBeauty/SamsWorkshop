window.addEventListener('load', async (e) => {
    document.getElementById('ChangeThemeButton').addEventListener('click', async (e) => {
        await ChangeTheme()
    })
})

async function ChangeTheme() {

    let currentTheme = localStorage.getItem('theme')


    if (currentTheme && currentTheme == 'light') {


        localStorage.setItem('theme', 'dark')


        let result = await advFetch('/api/Theme/ChangeTheme', {
            method: 'POST',
            headers: {
                "content-type": "application/json"
            },
            body: JSON.stringify({ theme: "dark" })
        })
        console.log(result)

        document.getElementById('themeStyle').setAttribute('href', '/css/NightTime.css')

    } else {

        localStorage.setItem('theme', 'light')


        let result = await advFetch('/api/Theme/ChangeTheme', {
            method: 'POST',
            headers: {
                "content-type": "application/json"
            },
            body: JSON.stringify({ theme: "light" })
        })
        console.log(result)

        document.getElementById('themeStyle').setAttribute('href', '/css/DayTime.css')
    }
}

function advFetch(url, options) {
    let verifyToken = document.querySelector('input[name="__RequestVerificationToken"]').value;
    if (options == undefined) {
        options = {};
    }
    if (options.headers == undefined) {
        options.headers = {};
    }
    if (verifyToken != undefined) {
        options.headers['RequestVerificationToken'] = verifyToken;
    }

    options.headers['x-fetch-request'] = "";

    var promise = fetch(url, options)
    return promise;
}