"use strict";
//variables
let userClaims = null;
const page_number_deafult = 1,
    page_size_default = 10;
let page_number = null,
    page_size = null,
    hub = null,
    response_json = null;
const hubs_count = 10;
//arrays
window.addEventListener("load", load);//DOMContentLoaded
window.onclick = function (e) {
    if (!e.target.matches('.dropbtn')) {
        let authorize_dropdown = document.getElementById("authorize_dropdown");
        if (!authorize_dropdown.classList.contains('hidden'))
            authorize_dropdown.classList.add('hidden');
    }
};
add_progressbar();

//получить параметры
try {
    const queryString = window.location.search;
    const urlParams = new URLSearchParams(queryString);
    //
    page_number = urlParams.get('PageNumber');
    page_size = urlParams.get('PageSize');
    hub = urlParams.get('hub').trim().toLowerCase();
    //
    if (page_number == null || page_size == null) {
        page_number = page_number_deafult;
        page_size = page_size_default;
    } else {
        page_number = parseInt(page_number);
        page_size = parseInt(page_size);
        //
        if (page_number < 1 || page_size < 1)
            throw new Error('указаны некорректные параметры');
    }
} catch (e) {   
    page_number = page_number_deafult;
    page_size = page_size_default; 
}
//
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
})();

async function get_hubs(page_number, page_size, hub, hubs_count) {
    let response = hub == null
        ? await get_top_hubs(hubs_count)
        : await get_hubs_query(page_number, page_size, hub, hubs_count);
    //
    if (response.title.trim() == '')
        response.title = 'Популярные хабы';
    return response;
}

function get_hubs_html(hubs, hub, hubs_count) {
    return hub == null
        ? get_hubs_top_html(hubs)
        : get_hubs_query_html(hubs);
}

async function get_hubs_query(page_number, page_size, hub, hubs_count) {
    const response = await fetch(`/articles/get-by-hub?PageNumber=${page_number}&PageSize=${page_size}&hub=${hub}`, {
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

    //комментарии
    let article_comment_count = 0;
    let article_comments = null;
    //
    let comments = [];
    for (let article of response.result.resultData)
        comments.push(await get_comments_count(article['id']));
    return { result: response.result, title: hub, comments: comments };
}

function get_hubs_top_html(value) {
    //добавить хабы
    const hubs = value.result;
    const title = value.title;

    let textHTML = `<h4 class="title_row">${title}:</h4>`;
    hubs.forEach(hub => {
        textHTML += `
            <div class="all_hubs_new_item">
            <div class="all_hubs_new_item_flex">                
                <div class="all_hubs_new_item_texts">
                    <h4>Название:
                        <a class="site_links" href="${window.location.origin}/hubs.html?hub=${hub.title.trim().toLowerCase()}">
                            ${hub.title}
                        </a>
                    </h4>
                    <h4>Кол-во: ${hub.articlesCount}</h4>
                    <br>                  
                </div>
            </div>
        </div>`;
    });
    return textHTML;
}

function get_hubs_query_html(value) {
    //добавить хабы
    const articles = get_articles_html(value.result.resultData, value.comments, value.result.count);//articles
    return `
        <h4 class="title_row">${value.title}:</h4>
        ${articles.articles_html}
        <div class="all_posts_pagination">${articles.articles_pagination_html}</div>`;
}

function get_main_html() {
    return `<div id="hubs_div"></div>`;
}

async function load() {
    //загрузка данных
    response_json = await load_data();
    if (response_json == null)
        return;

    //рендер html
    const header_hubs_html = get_header_links_html(response_json.header_hubs);
    const main_html = get_main_html();
    const hubs_html = get_hubs_html(response_json.hubs, hub, response_json.hubs_count);

    //добавить на страницу
    delete_progressbar();
    render_page(
        response_json,
        {
            header_hubs_html: header_hubs_html,
            main_html: main_html,
            hubs_html: hubs_html
        }
    );
}

async function load_data() {
    let header_html = '',
        footer_html = '',
        header_hubs = '';
    let hubs = null,        
        account_data = null;

    //загрузка данных
    try {
        header_html = await get_header();
        footer_html = await get_footer();
        header_hubs = await get_top_hubs(4);//популярные хабы
        //
        hubs = await get_hubs(page_number, page_size, hub, hubs_count);
        account_data = get_account_data();
    } catch (ex) {
        throw new Error('ошибка загрузки данных');
    }

    //объект
    return {
        header_html: header_html,
        footer_html: footer_html,
        header_hubs: header_hubs,
        account_data: account_data,
        hubs: hubs,
        hubs_count: hubs_count
    };
}

function render_hubs(hubs_html) {
    //добавить на страницу
    let hubs_div = document.getElementById('hubs_div');
    if (hubs_div == null)
        return false;
    //
    hubs_div.innerHTML = '';
    hubs_div.insertAdjacentHTML('afterbegin', hubs_html);
    return true;
}

function render_page(response_json, html) {
    if (response_json == null)
        return false;
    //
    const header_html = response_json.header_html;
    const header_hubs_html = html.header_hubs_html;
    const main_html = html.main_html;
    const hubs_html = html.hubs_html;
    const account_data = response_json.account_data;
    const footer_html = response_json.footer_html;

    render_header({ header_html: header_html, header_hubs_html: header_hubs_html });
    render_account(account_data);
    render_main(main_html);
    render_hubs(hubs_html);
    render_footer(footer_html);
}