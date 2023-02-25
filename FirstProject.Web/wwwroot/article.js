"use strict";
let userClaims = null;
let article = null;
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

    let login = null;
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

async function get_article() {
    //получить id
    const queryString = window.location.search;
    const urlParams = new URLSearchParams(queryString);
    const article_id = urlParams.get('id');
    //
    const request = await fetch(`/articles/get-by-id?${new URLSearchParams({ articleId: article_id })}`, {
        method: 'GET',
        headers: new Headers({ "X-CSRF": "1" })
    })
        .then(response => response.json())
        .catch(e => console.log(e));
    //
    if (request == null || typeof request === 'undefined')
        return null;
    if (!request.hasOwnProperty('result'))
        return null;
    return request.result;
}

async function load() {
    await render_page();
}

function render_article(article) {
    const prev_link = window.location.origin;
    //hubs
    let hubs = '';
    article['hubs'].forEach(hub => hubs += `${hub}, `);
    hubs = hubs.substring(0, hubs.length - 2);
    //
    const textHTML = `
        <div class="section_new_post_text">
            <h3 class="all_posts_item_h3">${hubs}</h3>
            <a class="site_links" href="#">
                <h2 class="section_h2">${article['title']}</h2>
            </a>
            <p class="section_p_attr">${article['authorNickName']} | ${article['timePublished']}</p>
        </div>
        <p class="article_text">${article['fullTextHtml']}</p>
        <div class="all_posts_pagination">
            <button class="all_posts_pag"}">Назад</button>
        </div>`;
    //
    const article_div = document.getElementById('article_div');
    if (article_div == null)
        return false;
    article_div.innerHTML = '';
    article_div.insertAdjacentHTML('afterbegin', textHTML);

    //события
    const all_posts_pag = document.querySelector('.all_posts_pag');
    if (all_posts_pag != null)
        all_posts_pag.addEventListener('click', () => window.history.back());
    //
    const img_array = document.getElementsByTagName('img');
    for (let img of img_array) {
        const data_src = img.getAttribute('data-src');
        let src = document.createAttribute('src');
        src.value = data_src;
        //
        img.setAttributeNode(src);
        img.removeAttribute('data-src');
        img.removeAttribute('width');
        img.removeAttribute('height');
        img.classList.add('section_new_post_img_full');
    }
    return true;
}

async function render_main() {
    //статья
    article = await get_article();
    if (article == null)
        return;
    return render_article(article);
}