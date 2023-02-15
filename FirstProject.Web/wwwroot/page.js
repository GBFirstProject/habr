"use strict";
//variables
let userClaims = null;
const page_size = 10;
//arrays
let articles = [];
//
/*document.getElementById("login").addEventListener("click", login, false);
document.getElementById("remote").addEventListener("click", localApi, false);
//document.getElementById("remote-admin").addEventListener("click", remoteApi, false);
document.getElementById("logout").addEventListener("click", logout, false);
document.getElementById("testocelot").addEventListener("click", testocelot, false);*/
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

async function load() {
    //получить id
    let gg = window.location.origin;
    const queryString = window.location.search;
    const urlParams = new URLSearchParams(queryString);
    const article_id = urlParams.get('articleId');
    //
    const request = await fetch(`/articles/get-by-id?${new URLSearchParams({ articleId: article_id })}`, {
        method: 'GET',
        headers: new Headers({ "X-CSRF": "1" })
    })
        .then(response => response.json())
        .catch(e => console.log(e));
    //
    if (request == null || typeof request === 'undefined')
        return;
    if (!request.hasOwnProperty('result'))
        return;
    
    //hubs
    let hubs = '';
    request.result['hubs'].forEach(hub => hubs += `${hub}, `);
    hubs = hubs.substring(0, hubs.length - 2);
    //
    const textHTML = `
        <div class="section_new_post_text">
            <h3 class="all_posts_item_h3">${hubs}</h3>
            <a class="site_links" href="#">
                <h2 class="section_h2">${request.result['title']}</h2>
            </a>
            <p class="section_p_attr">${request.result['authorNickName']} | ${request.result['timePublished']}</p>
        </div>
        <p class="article_text">${request.result['fullTextHtml']}</p>

        <div class="all_posts_pagination">
            <a class="all_posts_pag" href="${gg}">Назад</a>
        </div>`;
    //
    const article_div = document.getElementById('article_div');
    if (article_div == null)
        return;
    article_div.innerHTML = '';
    article_div.insertAdjacentHTML('afterbegin', textHTML);
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
    /*document.getElementById("results").innerText = "";

    Array.prototype.forEach.call(arguments, function (msg) {
        if (typeof msg !== "undefined") {
            if (msg instanceof Error) {
                msg = "Error: " + msg.message;
            } else if (typeof msg !== "string") {
                msg = JSON.stringify(msg, null, 2);
            }
            document.getElementById("results").innerText += msg + "\r\n";
        }
    });*/
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

async function render_page() {

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

function testocelot() {
    document.location = "/testocelot.html"
}