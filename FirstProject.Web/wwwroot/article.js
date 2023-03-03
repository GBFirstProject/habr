"use strict";
let userClaims = null;
let article = null;
let id = null;
const page_number_deafult = 1,
    page_size_default = 10;
let page_number = null,
    page_size = null,
    response_json = null;
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
    id = urlParams.get('id').trim().toLowerCase();
    page_number = urlParams.get('PageNumber');
    page_size = urlParams.get('PageSize');
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

    let login = null;
    try {
        var resp = await fetch(req);
        if (resp.ok) {
            userClaims = await resp.json();
        } else if (resp.status === 401) {
        }
    } catch (e) {
    }
})();

function get_main_html() {
    return `
        <p id="button_back" onclick="window.history.back()"><u>Назад</u></p>
        <div id="article_div"></div>`;
}

async function load() {
    //загрузка данных
    response_json = await load_data();
    if (response_json == null)
        return;

    //рендер html
    const header_hubs_html = get_header_links_html(response_json.header_hubs);
    const main_html = get_main_html();
    const article_html = get_article_html(response_json.article, response_json.article_comment_count);
    const article_comments_html = get_article_comments_html(response_json.article, response_json.article_comment_count, response_json.article_comments);
    const article_comments_pagination_html = get_article_comments_pagination_html(response_json.article_comment_count);

    //добавить на страницу
    delete_progressbar();
    render_page(
        response_json,
        {
            header_hubs_html: header_hubs_html,
            main_html: main_html,
            article_html: article_html,
            article_comments_html: article_comments_html,
            article_comments_pagination_html: article_comments_pagination_html
        }
    );
}

async function load_data() {
    let header_html = '',
        footer_html = '',
        header_hubs = '';
    let article = null,
        article_comments = null,
        account_data = null;
    let article_comment_count = 0;

    //arrays
    let comment_count_array = [];

    //загрузка данных
    try {
        header_html = await get_header();
        footer_html = await get_footer();
        header_hubs = await get_top_hubs(4);//популярные хабы
        //
        article = await get_article(id);
        article_comment_count = await get_comments_count(id);
        article_comments = await get_comments(id, page_number, page_size);
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
        article: article,
        article_comments: article_comments,
        article_comment_count: article_comment_count 
    };
}

function render_page(response_json, html) {
    if (response_json == null)
        return false;
    //
    const header_html = response_json.header_html;
    const header_hubs_html = html.header_hubs_html;
    const main_html = html.main_html;
    const article_html = html.article_html;
    const article_comments_html = html.article_comments_html;
    const article_comments_pagination_html = html.article_comments_pagination_html;
    const account_data = response_json.account_data;
    const footer_html = response_json.footer_html;

    render_header({ header_html: header_html, header_hubs_html: header_hubs_html });
    render_account(account_data);
    render_main(main_html);
    render_article(article_html, article_comments_html, article_comments_pagination_html);
    render_footer(footer_html);
}