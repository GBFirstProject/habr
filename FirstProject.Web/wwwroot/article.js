"use strict";
let userClaims = null;
let account_name = 'гость',
    account_role = 'guest';
//arrays
//
document.addEventListener("DOMContentLoaded", load11);

function load11() {
    console.log(account_role);
}

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

/*function account() {
    window.location = "/account.html";
}

function account_click() {
    let dropdown_content = document.querySelector('.dropdown-content');
    if (dropdown_content != null) {
        //скрыть выпадающее меню
        if (!dropdown_content.classList.contains('hidden')) {
            dropdown_content.classList.add('hidden');
            return;
        }
    }
    //
    const dropbtn = document.querySelector('.dropbtn');
    if (dropbtn == null)
        return;

    //показать элемент
    const header_flex = document.querySelector('.header_flex');
    const header = document.querySelector('header');
    //
    if (header_flex == null || header == null)
        return;

    const dropbtn_computed = getComputedStyle(dropbtn);
    const header_flex_computed = getComputedStyle(header_flex);
    const header_computed = getComputedStyle(header);

    //right
    let header_computed_width = header_computed.width;
    let header_flex_computed_width = header_flex_computed.width;
    let right = (parseFloat(header_computed_width) - parseFloat(header_flex_computed_width)) / 2;
    dropdown_content.style.right = `${right}px`;

    //top
    dropdown_content.style.top = dropbtn_computed.height;
    dropdown_content.classList.toggle('hidden');
}*/

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

/*async function get_header() {
    const response = await fetch(`/header.html`, {
        method: 'GET',
        headers: new Headers({ "X-CSRF": "1" })
    })
        .then(response => response.text())
        .catch(e => console.log(e));
    //
    if (response == null || typeof response === 'undefined')
        return null;
    return response;
}

async function get_footer() {
    const response = await fetch(`/footer.html`, {
        method: 'GET',
        headers: new Headers({ "X-CSRF": "1" })
    })
        .then(response => response.text())
        .catch(e => console.log(e));
    //
    if (response == null || typeof response === 'undefined')
        return null;
    return response;
}*/

async function load() {
    //header
    const header_html = await get_header();
    if (header_html == null || typeof header_html === 'undefined')
        return;
    //
    const header = document.getElementById('header');
    if (header == null)
        return;
    //
    header.innerHTML = '';
    header.insertAdjacentHTML('afterbegin', header_html);

    //footer
    const footer_html = await get_footer();
    if (footer_html == null || typeof footer_html === 'undefined')
        return;
    //
    const footer = document.getElementById('footer');
    if (footer == null)
        return;
    //
    footer.innerHTML = '';
    footer.insertAdjacentHTML('afterbegin', footer_html);

    //данные пользователя
    if (userClaims != null) {
        for (const claim of userClaims) {
            if (claim.hasOwnProperty('type')) {
                if (claim.type == 'name') {
                    account_name = claim.value;
                    break;
                }
            }
        }
    }

    //вывод информации о пользователе
    const account_login = document.getElementById('account_login');
    if (account_login != null)
        account_login.innerText = account_name;

    //пункты меню для пользователя
    const myDropdown = document.getElementById('myDropdown');
    if (myDropdown == null)
        return;
    //
    if (userClaims != null) {
        for (const claim of userClaims) {
            if (claim.hasOwnProperty('type')) {
                if (claim.type == 'role') {
                    account_role = claim.value.trim().toLowerCase();
                    break;
                }
            }
        }
    }
    render_account(account_role);

    //статья
    const article = await get_article();
    if (article == null)
        return;
    render_page(article);
}

/*function login() {
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

function render_account(role) {
    const account = document.getElementById('account');
    const sign_in = document.getElementById('sign_in');
    const sign_up = document.getElementById('sign_up');
    const sign_out = document.getElementById('sign_out');
    //
    if (account == null ||
        sign_in == null ||
        sign_up == null ||
        sign_out == null)
        return false;

    //пункты меню
    if (role == 'user' || role == 'moderator' || role == 'admin') {
        //показать
        if (account.classList.contains('hidden'))
            account.classList.remove('hidden');
        if (sign_out.classList.contains('hidden'))
            sign_out.classList.remove('hidden');

        //события
        account.addEventListener('click', account);
        sign_out.addEventListener('click', logout);

        //скрыть
        if (!sign_in.classList.contains('hidden'))
            sign_in.classList.add('hidden');
        if (!sign_up.classList.contains('hidden'))
            sign_up.classList.add('hidden');
        return true;
    }

    if (role == 'guest') {
        //показать
        if (sign_in.classList.contains('hidden'))
            sign_in.classList.remove('hidden');

        if (sign_up.classList.contains('hidden'))
            sign_up.classList.remove('hidden');

        //события
        sign_in.addEventListener('click', login);
        sign_up.addEventListener('click', login);

        //скрыть
        if (!account.classList.contains('hidden'))
            account.classList.add('hidden');
        if (!sign_out.classList.contains('hidden'))
            sign_out.classList.add('hidden');
        return true;
    }
    return false;
}*/

function render_page(article) {
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