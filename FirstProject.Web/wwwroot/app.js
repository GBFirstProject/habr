"use strict";
//variables
let userClaims = null;
const page_number_deafult = 1,
    page_size_default = 10;
let page_number = null,
    page_size = null;
let account_name = 'гость',
    account_role = 'guest';
//arrays
let articles = [],
    articles_all = [],
    last_article = [];
//
document.addEventListener("DOMContentLoaded", load);

window.onclick = function (e) {
    if (!e.target.matches('.dropbtn')) {
        let authorize_dropdown = document.getElementById("authorize_dropdown");
        if (!authorize_dropdown.classList.contains('hidden'))
            authorize_dropdown.classList.add('hidden');
    }
};

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

async function button_last_article_click(e) {
    if (e.currentTarget == null || typeof e.currentTarget === 'undefined')
        return;
    //
    const hubr_id = e.currentTarget.id.substring('15');
    if (hubr_id.trim() == '')
        return;
    //
    let article_id = '';    
    if (String(last_article[0].hubrId).trim() == hubr_id.trim())
        article_id = last_article[0].id; 
    //
    if (article_id.trim() == '')
        return;
    //запрос
    window.location = `/article.html?id=${article_id}`;
}

async function get_articles(current_page_number, current_page_size) {
    const request = await fetch(`/articles?${new URLSearchParams({ PageNumber: current_page_number, PageSize: current_page_size })}`, {
        method: 'GET',
        headers: new Headers({ "X-CSRF": "1" })
    })
        .then(response => response.json())
        .catch(e => console.log(e));
    //
    if (request == null || typeof request === 'undefined')
        return [];
    if (!request.hasOwnProperty('result'))
        return [];
    return request.result;
}

async function get_articles_count() {
    const request = await fetch("/articles/get-articles-count?/api/articles/get-articles-count", {
        method: 'GET',
        headers: new Headers({ "X-CSRF": "1" })
    })
        .then(response => response.json())
        .catch(e => console.log(e));
    //
    if (request == null || typeof request === 'undefined')
        return -1;
    if (!request.hasOwnProperty('result'))
        return 0;
    return request.result;
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

async function load() {
    await render_page();
}

function render_articles(articles) {
    //вывод статей
    let textHTML = '';
    articles.forEach(article => {
        //hubs
        let hubs = '';
        article['hubs'].forEach(hub => hubs += `${hub}, `);
        hubs = hubs.substring(0, hubs.length - 2);

        //text
        const text = article['text'];
        const text_without_img = text.replace(/<img[^>]*>/g, "");
        //
        textHTML += `
            <div class="all_posts_item">
                <div class="all_posts_item_flex">
                    <div class="all_posts_item_pic">
                        <img class="section_new_post_img" src="${article['imageURL']}" alt="image_${article['hubrId']}">
                    </div>
                    <div class="all_posts_item_texts">
                        <h3 class="all_posts_item_h3">${hubs}</h3>
                        <a class="site_links" href="${window.location}article.html?id=${article['id']}">
                            <h2 class="all_posts_item_h2">${article['title']}</h2>
                        </a>
                        <p class="section_p_attr">${article['authorNickName']} | ${get_datetime_string(article['timePublished'])}</p>
                        <p class="all_posts_item_text">${text_without_img}</p>
                        <button id="button_article_${article['hubrId']}" type="button">Читать дальше</button>
                    </div>
                </div>
            </div>`;
    });

    //добавить на страницу
    let section_all_posts = document.querySelector('.section_all_posts_block');
    if (section_all_posts == null)
        return false;
    //
    section_all_posts.innerHTML = '';
    section_all_posts.insertAdjacentHTML('afterbegin', textHTML);

    //события
    const button_article_array = document.querySelectorAll(`[id^="button_article_"]`);
    button_article_array.forEach(button_article => button_article.addEventListener('click', button_article_click));
    return true;
}

function render_data() {
    //main
    const textHTML = `
        <section class="section_new_post">
            <div class="container"></div>
        </section>
        <section class="section_all_posts">
            <div class="container">
                <h1 class="site_h1">Все публикации</h1>
                <hr>
                <div class="section_all_posts_block"></div>
            </div>
            <div class="all_posts_pagination">
                <a class="all_posts_pag" href="#prev">Назад</a>
                <a class="all_posts_pag" href="#next">Вперед</a>
            </div>
        </section>
        <section class="section_reg_welcome">
            <div class="container">
                <h3 class="reg_welcome_h3">Зарегистрируйтесь, <br> чтобы оставлять комментарии</h3>
                <button class="btn_welcome_reg">Регистрация</button>
            </div>
        </section>`;
    //
    const main = document.getElementsByTagName('main');
    if (main.length == 0)
        return false;
    //
    main[0].insertAdjacentHTML('afterbegin', textHTML);
    return true;
}

async function render_main() {
    render_data();
    //получить статьи
    articles = await get_articles(page_number, page_size);
    if (articles.length == 0)
        return false;
    //получить последнюю статью
    last_article = await get_articles(1, 1);
    if (last_article.length == 0)
        return false;

    //вывод последней статьи
    if (!render_last_article(last_article))
        return false;
    //вывод статей
    if (!render_articles(articles))
        return false;

    //пагинация
    if (!await render_pagination())
        return false;
    return true;    
}

async function render_last_article(last_article) {
    //hubs
    let hubs = '';
    last_article[0]['hubs'].forEach(hub => hubs += `${hub}, `);
    hubs = hubs.substring(0, hubs.length - 2);
    //
    const textHTML = `
        <div class="container">
            <div class="section_new_post_flex">
                <div class="section_new_post_text">
                    <h3 class="section_h3">Новый пост</h3>
                    <a class="site_links" href="${window.location}article.html?id=${last_article[0]['id']}">
                        <h2 class="section_h2">${last_article[0]['title']}</h2>
                    </a>
                    <p class="section_p_attr">${last_article[0]['authorNickName']} | ${get_datetime_string(last_article[0]['timePublished'])}</p>
                    <p class="section_p_legend">${last_article[0]['text']}</p>
                    <button id="button_article_${last_article[0]['hubrId']}" type="submit">Читать</button>                
                </div>
                <div class="section_new_post_pic">
                    <img class="section_new_post_img" src="${last_article[0]['imageURL']}" alt="image_${last_article[0]['hubrId']}">
                </div>           
            </div>
        </div>`;

    //добавить на страницу
    let section_new_post = document.querySelector('.section_new_post');
    if (section_new_post == null)
        return false;
    //
    section_new_post.innerHTML = '';
    section_new_post.insertAdjacentHTML('afterbegin', textHTML);

    //события
    const button_article = document.getElementById(`button_article_${last_article[0]['hubrId']}`);
    if (button_article != null)
        button_article.addEventListener('click', button_last_article_click);
    return true;
}

async function render_pagination() {
    //пагинация
    let textHTML = '';
    const articles_count = await get_articles_count();
    //
    if (articles_count == -1)
        return false;
    const pages_count = Math.ceil(articles_count / page_size);
    //
    if (articles_count >= 0 && articles_count < pages_count) {
        //единственная страница
        textHTML = `<a class="all_posts_pag " href="#prev">Назад</a>
                <div class="pagination">1</div>
                <a class="all_posts_pag" href="#next">Вперед</a>`;
    } else if (articles_count > pages_count) {
        //несколько страниц
        const penultimate_page = pages_count - 1;
        const last_page = pages_count;

        //назад
        textHTML = page_number > 1
            ? `<a class="all_posts_pag" href="${window.location.origin}?PageNumber=${page_number - 1}&PageSize=${page_size}">Назад</a>`
            : `<a class="all_posts_pag">Назад`;

        //1 и 2 страницы обязательно
        textHTML += `<a href="${window.location.origin}?PageNumber=1&PageSize=${page_size}"><div class="pagination">1</div></a>
            <a href="${window.location.origin}?PageNumber=2&PageSize=${page_size}"><div class="pagination">2</div></a>`;

        //предыдущие страницы (если есть)      
        if (page_number > 2) {
            const prev_count = page_number - 2;
            //
            if (prev_count <= 4) {
                for (let i = 3, j = 0; j < prev_count - 1; i++, j++)
                    textHTML += `<a href="${window.location.origin}?PageNumber=${i}&PageSize=${page_size}"><div class="pagination">${i}</div></a>`;
            } else {
                textHTML += `<div class="pagination">...</div>`;
                for (let i = page_number - 2; i < page_number; i++)
                    textHTML += `<a href="${window.location.origin}?PageNumber=${i}&PageSize=${page_size}"><div class="pagination">${i}</div></a>`;
            }
        }

        //текущая страница
        if (page_number > 2)
            textHTML += `<a href="${window.location.origin}?PageNumber=${page_number}&PageSize=${page_size}"><div class="pagination">${page_number}</div></a>`;

        //последующие следующие (если есть)
        const next_count = last_page - page_number;
        if (next_count > 5) {
            //page_number, page_number + 1, page_number + 2 ... penultimate_page, last_page            
            for (let i = 1; i < 3; i++) {
                if (page_number + i > 2)
                //if (page_number + i < page_size)
                    textHTML += `<a href="${window.location.origin}?PageNumber=${page_number + i}&PageSize=${page_size}"><div class="pagination">${page_number + i}</div></a>`;
            }
            textHTML += `<div class="pagination">...</div>
                <a href="${window.location.origin}?PageNumber=${penultimate_page}&PageSize=${page_size}"><div class="pagination">${penultimate_page}</div></a>
                <a href="${window.location.origin}?PageNumber=${last_page}&PageSize=${page_size}"><div class="pagination">${last_page}</div></a>`;
        } else {
            for (let i = page_number + 1; i <= last_page; i++)
                textHTML += `<a href="${window.location.origin}?PageNumber=${i}&PageSize=${page_size}"><div class="pagination">${i}</div></a>`;
        }

        //вперед
        textHTML += page_number < last_page
            ? `<a class="all_posts_pag" href="${window.location.origin}?PageNumber=${page_number + 1}&PageSize=${page_size}">Вперед</a>`
            : `<a class="all_posts_pag">Вперед`;
    }
    //
    const all_posts_pagination = document.querySelector('.all_posts_pagination');
    if (all_posts_pagination == null)
        return false;
    all_posts_pagination.innerHTML = '';
    all_posts_pagination.insertAdjacentHTML('afterbegin', textHTML);

    //события пагинации
    return true;
}