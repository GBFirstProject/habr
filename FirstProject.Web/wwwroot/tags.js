"use strict";
//variables
let userClaims = null;
const tags_count = 10;
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

async function get_tags(tags_count) {
    const response = await fetch(`/tag/top?count=${tags_count}`, {
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

async function render_tags() {
    const tags = await get_tags(tags_count);
    //добавить тэги
    let textHTML = '<h4 class="title_row">Все тэги:</h4>';
    tags.forEach(tag => {
        textHTML += `
            <div class="all_tags_new_item">
            <div class="all_tags_new_item_flex">                
                <div class="all_tags_new_item_texts">
                    <h4><a class="site_links" href="${window.location.href}/">${tag.tagName}</a></h3>
                    <h4><a class="site_links" href="#">${tag.articlesCount}</a></h3>
                    <br>                  
                </div>
            </div>
        </div>`;
    });

    //добавить на страницу
    let tags_div = document.getElementById('tags_div');
    if (tags_div == null)
        return false;
    //
    tags_div.innerHTML = '';
    tags_div.insertAdjacentHTML('afterbegin', textHTML);    
    return true;
}

async function render_main() {
    if (!await render_tags())
        return false;

}