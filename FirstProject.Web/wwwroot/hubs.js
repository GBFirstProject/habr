"use strict";
//variables
let userClaims = null;
const hubs_count = 10;
//arrays
window.onclick = function (e) {
    if (!e.target.matches('.dropbtn')) {
        let authorize_dropdown = document.getElementById("authorize_dropdown");
        if (!authorize_dropdown.classList.contains('hidden'))
            authorize_dropdown.classList.add('hidden');
    }
};

(async function () {
    var req = new Request("/bff/user", {
        headers: new Headers({
            "X-CSRF": "1",
        }),
    });

    try {
        var resp = await fetch(req);
        if (resp.ok) {
            userClaims = await resp.json();
        } else if (resp.status === 401) {
        }
    } catch (e) {
    }
    load();
})();

async function get_hubs(hubs_count) {
    const response = await fetch(`/hub/top?count=${hubs_count}`, {
        method: 'GET',
        headers: new Headers({ "X-CSRF": "1" })
    })
        .then(response => response.json())
        .catch(e => console.log(e));
    //
    if (response == null || typeof response === 'undefined')
        return [];
    if (!response.hasOwnProperty('result'))
        return [];
    return response.result;
}

async function load() {
    await render_page();
}

async function render_hubs() {
    const hubs = await get_hubs(hubs_count);
    //добавить хабы
    let textHTML = '<h4 class="title_row">Все хабы:</h4>';
    hubs.forEach(hub => {
        textHTML += `
            <div class="all_hubs_new_item">
            <div class="all_hubs_new_item_flex">                
                <div class="all_hubs_new_item_texts">
                    <h4><a class="site_links" href="${window.location.href}/">${hub.title}</a></h3>
                    <h4><a class="site_links" href="#">${hub.articlesCount}</a></h3>
                    <br>                  
                </div>
            </div>
        </div>`;
    });

    //добавить на страницу
    let hubs_div = document.getElementById('hubs_div');
    if (hubs_div == null)
        return false;
    //
    hubs_div.innerHTML = '';
    hubs_div.insertAdjacentHTML('afterbegin', textHTML);    
    return true;
}

async function render_main() {
    if (!await render_hubs())
        return false;

}