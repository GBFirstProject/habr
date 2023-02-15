"use strict";
//variables
let userClaims = null;
const page_size = 10;
//arrays
let articles = [];
//
document.getElementById("login").addEventListener("click", login, false);
document.getElementById("remote").addEventListener("click", localApi, false);
//document.getElementById("remote-admin").addEventListener("click", remoteApi, false);
document.getElementById("logout").addEventListener("click", logout, false);
document.getElementById("testocelot").addEventListener("click", testocelot, false);
//
document.addEventListener("DOMContentLoaded", load);

//скрыть блок авторизации
const container = document.querySelector('.container');
if (container != null) {
    if (!container.classList.contains('hidden'))
        container.classList.add('hidden');
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
            log("user logged in", userClaims);
        } else if (resp.status === 401) {
            log("user not logged in");
        }
    } catch (e) {
        log("error checking user status");
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
    window.location = `/page_item.html?articleId=${article_id}`;
}

async function get_articles() {
    const request = await fetch(`/articles?${new URLSearchParams({ PageNumber: 2, PageSize: page_size })}`, {
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

async function load() {
    //получить статьи
    articles = await get_articles();
    if (articles.length == 0)
        return;

    //пагинация
    if (!await render_pagination())
        return;

    //вывод статей
    if (!render_articles(articles))
        return;
}

async function localApi() {
    var req = new Request("/api/q", {
        headers: new Headers({
            "X-CSRF": "1",
        }),
    });

    try {
        var resp = await fetch(req);

        let data;
        if (resp.ok) {
            data = await resp.json();
        }
        log("Local API Result: " + resp.status, data);
    } catch (e) {
        log("error calling local API");
    }
}

function log() {
    document.getElementById("results").innerText = "";

    Array.prototype.forEach.call(arguments, function (msg) {
        if (typeof msg !== "undefined") {
            if (msg instanceof Error) {
                msg = "Error: " + msg.message;
            } else if (typeof msg !== "string") {
                msg = JSON.stringify(msg, null, 2);
            }
            document.getElementById("results").innerText += msg + "\r\n";
        }
    });
}

function login() {
    window.location = "/bff/login";
}

function logout() {
    if (userClaims) {
        var logoutUrl = userClaims.find(
            (claim) => claim.type === "bff:logout_url"
        ).value;
        window.location = logoutUrl;
    } else {
        window.location = "/bff/logout";
    }
}

async function remoteApi() {
    var req = new Request("api", {
        headers: new Headers({
            "X-CSRF": "1",
        }),
    });

    try {
        var resp = await fetch(req);

        let data;
        if (resp.ok) {
            data = await resp.json();
        }
        log("Remote API Result: " + resp.status, data);
    } catch (e) {
        log("error calling remote API");
    }
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
                        <img class="section_new_post_img" src="${article['imageURL']}" width="480" height="320" alt="image_${article['hubrId']}">
                    </div>
                    <div class="all_posts_item_texts">
                        <h3 class="all_posts_item_h3">${hubs}</h3>
                        <a class="site_links" href="#">
                            <h2 class="all_posts_item_h2">${article['title']}</h2>
                        </a>
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
        //
        textHTML = `<a class="all_posts_pag " href="#prev">Назад</a>
                    <div class="pagination">1</div>
                    <div class="pagination">2</div>
                    <div class="pagination">3</div>
                    <div class="pagination">...</div>
                    <div class="pagination">${penultimate_page}</div>
                    <div class="pagination">${last_page}</div>
                    <a class="all_posts_pag" href="#next">Вперед</a>`;
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

function testocelot() {
    document.location = "/testocelot.html"
}