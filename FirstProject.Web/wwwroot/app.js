"use strict";
//variables
let userClaims = null;
const page_number_deafult = 1,
    page_size_default = 10;
let page_number = null,
    page_size = null,
    response_json = null;
let account_name = 'гость',
    account_role = 'guest';
//arrays
let articles = [],
    articles_all = [],
    top_article = [];
//
window.addEventListener("load", load);//DOMContentLoaded
window.addEventListener('scroll', throttle(checkPosition, 250));
window.addEventListener('resize', throttle(checkPosition, 250));
//
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

    try {
        var resp = await fetch(req);
        if (resp.ok) {
            userClaims = await resp.json();
        } else if (resp.status === 401) {
        }
    } catch (e) {
    }
})();

async function button_article_click(e) {
    if (e.currentTarget == null || typeof e.currentTarget === 'undefined')
        return;
    //
    const hubr_id = e.currentTarget.id.substring('15');
    if (hubr_id.trim() == '')
        return;
    //
    let article_id = '';
    for (const article of articles) {
        if (String(article.hubrId).trim() == hubr_id.trim()) {
            article_id = article.id;
            break;
        }            
    }
    //
    if (article_id.trim() == '')
        return;
    //запрос
    window.location = `/article.html?id=${article_id}`;
}

async function button_top_article_click(e) {
    if (e.currentTarget == null || typeof e.currentTarget === 'undefined')
        return;
    //
    const hubr_id = e.currentTarget.id.substring('15');
    if (hubr_id.trim() == '')
        return;
    //
    let article_id = '';    
    if (String(top_article.hubrId).trim() == hubr_id.trim())
        article_id = top_article.id; 
    //
    if (article_id.trim() == '')
        return;
    //запрос
    window.location = `/article.html?id=${article_id}`;
}

async function checkPosition() {
    // Нам потребуется знать высоту документа и высоту экрана:
    const height = document.body.offsetHeight;
    const screenHeight = window.innerHeight;

    // Они могут отличаться: если на странице много контента,
    // высота документа будет больше высоты экрана (отсюда и скролл).
    // Записываем, сколько пикселей пользователь уже проскроллил:
    const scrolled = window.scrollY;

    // Обозначим порог, по приближении к которому
    // будем вызывать какое-то действие.
    // В нашем случае — четверть экрана до конца страницы:
    const threshold = height - screenHeight / 3;//4

    // Отслеживаем, где находится низ экрана относительно страницы:
    const position = scrolled + screenHeight;

    if (position >= threshold) {
        // Если мы пересекли полосу-порог, вызываем нужное действие.
        let load_pause = page_number % 5;
        if (load_pause == 0) {
            const articles_load = document.querySelector('.articles_load');
            if (articles_load == null) {
                const textHTML = `<div class="articles_load"><u>Продолжить просмотр ленты</u></div>`;
                const section_all_posts_block = document.querySelector('.section_all_posts_block');
                if (section_all_posts_block != null)
                    section_all_posts_block.insertAdjacentHTML('beforeend', textHTML);
                //
                const articles_load = document.querySelector('.articles_load');
                articles_load.addEventListener('click', articles_load_click);
            }    
            return;                    
        }        
        articles_load_click();
    }
}

async function articles_load_click(e) {
    //удалить существующую ссылку на продолжение загрузки
    const articles_load = document.querySelector('.articles_load');
    if (articles_load != null)
        articles_load.remove();

    //загрузка статей
    let comment_count_array = [];
    const articles = await get_articles(++page_number, 3);
    //        
    for (const article of articles)
        comment_count_array.push(await get_comments_count(article['id']));
    //
    const articles_html = get_articles_html(articles, comment_count_array);
    render_added_articles(articles_html.articles_html);
}

function get_datetime_string(value) {
    //datetime
    const date = new Date(Date.parse(value));
    //день
    let day = date.getDate();
    if (day < 10)
        day = '0' + day;
    //месяц
    const formatter = new Intl.DateTimeFormat('ru', { month: 'short' });
    const month = formatter.format(date);
    //год
    const year = date.getFullYear();
    //время
    const time = date.toLocaleTimeString('ru-RU');
    return `${day} ${month} ${year}, ${time} МСК`;
}

function get_main_html() {
    return `
        <section class="section_new_post">
            <div class="container"></div>
        </section>
        <section class="section_all_posts">
            <div class="container">
                <h1 class="site_h1">Все публикации</h1>
                <hr>
                <div class="section_all_posts_block"></div>
            </div>
            <!--<div class="all_posts_pagination">
                <a class="all_posts_pag" href="#prev">Назад</a>
                <a class="all_posts_pag" href="#next">Вперед</a>
            </div>-->
        </section>`;
}

async function load() {
    //загрузка данных
    response_json = await load_data();
    if (response_json == null)
        return;

    //рендер html
    const header_hubs_html = get_header_links_html(response_json.header_hubs);
    const main_html = get_main_html();
    const top_article_html = get_top_article_html(response_json.top_article, response_json.top_article_comment_count);//top_article
    const articles = get_articles_html(response_json.articles, response_json.comment_count_array);//articles

    //добавить на страницу
    delete_progressbar();
    render_page(
        response_json,
        {
            header_hubs_html: header_hubs_html, 
            main_html: main_html,
            top_article_html: top_article_html,
            articles_html: articles.articles_html/*,
            articles_pagination_html: articles.articles_pagination_html*/
        }
    );
}

async function load_data() {
    let header_html = '',
        footer_html = '',
        header_hubs = '';
    let articles = null,
        top_article = null,
        account_data = null;
    let top_article_comment_count = 0,
        articles_count = 0;

    //arrays
    let comment_count_array = [];

    //загрузка данных
    try {
        header_html = await get_header();
        footer_html = await get_footer();
        header_hubs = await get_top_hubs(4);//популярные хабы
        //
        articles = await get_articles(page_number, 3);//page_size
        top_article = await get_top_article();//get_articles(1, 1);

        //preview_top_article
        top_article_comment_count = await get_comments_count(top_article['id']);
        //preview_articles
        comment_count_array = [];
        for (const article of articles)
            comment_count_array.push(await get_comments_count(article['id']));
        //
        //articles_count = await get_articles_count();
        account_data = get_account_data();
    } catch (ex) {
        throw new Error('ошибка загрузки данных');
    }

    //объект
    return {
        header_html: header_html,
        footer_html: footer_html,
        header_hubs: header_hubs,
        articles: articles,
        top_article: top_article,
        top_article_comment_count: top_article_comment_count,
        comment_count_array: comment_count_array,
        articles_count: articles_count,
        account_data: account_data
    };
}

function render_page(response_json, html) {
    if (response_json == null)
        return false;
    //
    const header_html = response_json.header_html;
    const header_hubs_html = html.header_hubs_html;
    const main_html = html.main_html;
    const top_article_html = html.top_article_html;
    const articles_html = html.articles_html;
    //const articles_pagination_html = html.articles_pagination_html;
    const account_data = response_json.account_data;
    const footer_html = response_json.footer_html;

    render_header({ header_html: header_html, header_hubs_html: header_hubs_html });
    render_account(account_data);
    render_main(main_html);
    render_top_article(top_article_html, response_json.top_article.id);
    render_articles(articles_html, response_json.articles/*, articles_pagination_html*/);
    render_footer(footer_html);
}