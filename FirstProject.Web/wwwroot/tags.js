"use strict";
//variables
let userClaims = null;
const page_number_deafult = 1,
    page_size_default = 10;
let page_number = null,
    page_size = null,
    tag = null,
    response_json = null;
const tags_count = 10;
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
    tag = urlParams.get('tag').trim().toLowerCase();
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

async function get_tags(page_number, page_size, tag, tags_count) {
    let response = tag == null
        ? await get_top_tags(tags_count)
        : await get_tags_query(page_number, page_size, tag, tags_count);
    //
    if (response.title.trim() == '')
        response.title = 'Популярные тэги';
    return response;
}

function get_tags_html(tags, tag, tags_count) {
    return tag == null
        ? get_tags_top_html(tags)
        : get_tags_query_html(tags);
}

async function get_tags_query(page_number, page_size, tag, tags_count) {
    const response = await fetch(`/articles/get-by-tag?PageNumber=${page_number}&PageSize=${page_size}&tag=${tag}`, {
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
    return { result: response.result, title: tag, comments: comments };
}

function get_tags_top_html(value) {
    //добавить хабы
    const tags = value.result;
    const title = value.title;

    let textHTML = `<h4 class="title_row">${title}:</h4>`;
    tags.forEach(tag => {
        textHTML += `
            <div class="all_tags_new_item">
            <div class="all_tags_new_item_flex">                
                <div class="all_tags_new_item_texts">
                    <h4>Название:
                        <a class="site_links" href="${window.location.origin}/tags.html?tag=${tag.tagName.trim().toLowerCase()}">
                            ${tag.tagName}
                        </a>
                    </h4>
                    <h4>Кол-во: ${tag.articlesCount}</h4>
                    <br>                  
                </div>
            </div>
        </div>`;
    });
    return textHTML;
}

function get_tags_query_html(value) {
    //добавить хабы
    const articles = get_articles_html(value.result.resultData, value.comments, value.result.count);//articles
    return `
        <h4 class="title_row">${value.title}:</h4>
        ${articles.articles_html}
        <div class="all_posts_pagination">${articles.articles_pagination_html}</div>`;
}

function get_main_html() {
    return `<div id="tags_div"></div>`;
}

async function load() {
    //загрузка данных
    response_json = await load_data();
    if (response_json == null)
        return;

    //рендер html
    const header_hubs_html = get_header_links_html(response_json.header_hubs);
    const main_html = get_main_html();
    const tags_html = get_tags_html(response_json.tags, tag, response_json.tags_count);

    //добавить на страницу
    delete_progressbar();
    render_page(
        response_json,
        {
            header_hubs_html: header_hubs_html,
            main_html: main_html,
            tags_html: tags_html
        }
    );
}

async function load_data() {
    let header_html = '',
        footer_html = '',
        header_hubs = '';
    let tags = null,        
        account_data = null;

    //загрузка данных
    try {
        header_html = await get_header();
        footer_html = await get_footer();
        header_hubs = await get_top_hubs(4);//популярные хабы
        //
        tags = await get_tags(page_number, page_size, tag, tags_count);
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
        tags: tags,
        tags_count: tags_count
    };
}

function render_tags(tags_html) {
    //добавить на страницу
    let tags_div = document.getElementById('tags_div');
    if (tags_div == null)
        return false;
    //
    tags_div.innerHTML = '';
    tags_div.insertAdjacentHTML('afterbegin', tags_html);
    return true;
}

function render_page(response_json, html) {
    if (response_json == null)
        return false;
    //
    const header_html = response_json.header_html;
    const header_hubs_html = html.header_hubs_html;
    const main_html = html.main_html;
    const tags_html = html.tags_html;
    const account_data = response_json.account_data;
    const footer_html = response_json.footer_html;

    render_header({ header_html: header_html, header_hubs_html: header_hubs_html });
    render_account(account_data);
    render_main(main_html);
    render_tags(tags_html);
    render_footer(footer_html);
}