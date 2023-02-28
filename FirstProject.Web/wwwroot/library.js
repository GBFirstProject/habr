"use strict";

function account() {
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
}

async function add_header_links() {
    //ссылки в header
    //все хабы (all_hubs)
    const all_hubs_item = document.getElementById('all_hubs');
    if (all_hubs_item == null)
        return false;
    add_link(all_hubs_item, `${window.location.origin}/hubs.html`);

    //все тэги
    const all_tags_item = document.getElementById('all_tags');
    if (all_tags_item == null)
        return false;
    add_link(all_tags_item, `${window.location.origin}/tags.html`);
    
    //добавить 4 популярных хаба
    const hubs = await get_top_hubs(4);
    let textHTML = '';
    hubs.forEach(hub => textHTML += `<li class="header_nav_item"><a href="${window.location.origin}/hubs.html?hub=${hub.title.trim().toLowerCase()}">${hub.title}</a></li>`);

    const header_nav_list = document.querySelector('.header_nav_list');
    if (header_nav_list != null)
        header_nav_list.insertAdjacentHTML('beforeend', textHTML);
    return true;
}

function add_link(item, link) {
    if (item == null || typeof item === 'undefined')
        return false;
    //
    let a_element = null;
    for(let child of item.childNodes) {
        if (child.tagName.toLowerCase() == 'a') {
            a_element = child;
            break;
        }
    }
    
    //добавить
    if (a_element != null)
        a_element.href = link;
    return true;
}

function contains_role(role) {
    return roles.includes(role);
}

function get_account_data() {
    //данные пользователя
    let account_data = {'email': '', 'name': 'гость', 'role': 'guest'};
    //
    if (userClaims != null) {
        for (const claim of userClaims) {
            switch(claim.type) {
                case 'email':
                    account_data.email = claim.value.trim().toLowerCase();
                    break;

                case 'name':
                    account_data.name = claim.value;
                    break;

                case 'role':
                    account_data.role = claim.value.trim().toLowerCase();
                    break;
            }
        }
    }
    return account_data;
}

async function get_comments(id, page_number, page_size) {
    //comments
    const response = await fetch(`/comments?articleId=${id}&index=${(page_number - 1) * page_size}&count=${page_size}`, {
        method: 'GET',
        headers: new Headers({ "X-CSRF": "1" })
    })
        .then(response => response.json())
        .catch(e => console.log(e));
    //
    if (response == null || typeof response === 'undefined')
        return -1;
    if (!response.hasOwnProperty('result'))
        return 0;
    return response.result;
}

async function get_comments_count(id) {
    //comments
    const response = await fetch(`/comments/getCount?articleId=${id}`, {
        method: 'GET',
        headers: new Headers({ "X-CSRF": "1" })
    })
        .then(response => response.json())
        .catch(e => console.log(e));
    //
    if (response == null || typeof response === 'undefined')
        return -1;
    if (!response.hasOwnProperty('result'))
        return 0;
    return response.result;
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

async function get_header() {
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
}

function get_roles() {
    return roles;
}

async function get_top_hubs(hubs_count) {
    const response = await fetch(`/hub/top?count=${hubs_count}`, {
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

function render_account() {
    const account_data = get_account_data();
    if (account_data == null)
        return false;
    //
    const name = account_data.name;
    const role = account_data.role;    
    
    //вывод информации о пользователе
    const account_login = document.getElementById('account_login');
    if (account_login != null)
        account_login.innerText = name;

    //пункты меню для пользователя
    const authorize_dropdown = document.getElementById('authorize_dropdown');
    if (authorize_dropdown == null)
        return;   
    
    //вывод в зависимости от роли
    const personal = document.getElementById('account');
    const sign_in = document.getElementById('sign_in');
    const sign_up = document.getElementById('sign_up');
    const sign_out = document.getElementById('sign_out');
    //
    if (personal == null ||
        sign_in == null ||
        sign_up == null ||
        sign_out == null)
        return false;

    //пункты меню
    if (role == 'user' || role == 'moderator' || role == 'admin') {
        //показать
        if (personal.classList.contains('hidden'))
            personal.classList.remove('hidden');
        if (sign_out.classList.contains('hidden'))
            sign_out.classList.remove('hidden');

        //события
        personal.addEventListener('click', account);
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
        if (!personal.classList.contains('hidden'))
            personal.classList.add('hidden');
        if (!sign_out.classList.contains('hidden'))
            sign_out.classList.add('hidden');
        return true;
    }
    return false;
}

async function render_preview_article(article) {
    //вывод статьи
    //hubs
    let hubs = '';
    for(let i = 0; i < article['hubs'].length; i++) {
        hubs += i == article['hubs'].length - 1
            ? `<p class="advanced_data"><a href="${window.location.origin}/hubs.html?hub=${article['hubs'][i].trim().toLowerCase()}">${article['hubs'][i]} </a></p>`
            : `<p class="advanced_data"><a href="${window.location.origin}/hubs.html?hub=${article['hubs'][i].trim().toLowerCase()}">${article['hubs'][i]}, </a></p>`;
    }

    //tags
    let tags = '';
    for(let i = 0; i < article['tags'].length; i++) {
        tags += i == article['tags'].length - 1
            ? `<p class="advanced_data"><a href="${window.location.origin}/tags.html?tag=${article['tags'][i].trim().toLowerCase()}">${article['tags'][i]} </a></p>`
            : `<p class="advanced_data"><a href="${window.location.origin}/tags.html?tag=${article['tags'][i].trim().toLowerCase()}">${article['tags'][i]}, </a></p>`;
    }

    //comments
    const comment_count = await get_comments_count(article['id']);
    return `
        <div class="all_posts_item">
            <div class="all_posts_item_flex">
                <div class="all_posts_item_pic">
                    <img class="section_new_post_img" src="${article['imageURL']}" alt="image_${article['hubrId']}">
                </div>
                <div class="all_posts_item_texts">
                    <p class="section_p_attr">${article['authorNickName']} | ${get_datetime_string(article['timePublished'])}</p>
                    <h2 class="all_posts_item_h2">
                        <a class="site_links" href="${window.location}article.html?id=${article['id']}">
                            ${article['title']}
                        </a>
                    </h2>
                    <div class="section_new_post_data">
                        <p class="advanced_data">Комментарии: ${comment_count}</p>
                        <p class="advanced_data">Просмотров: ${article['readingCount']}</p>
                    </div>
                    <div class="section_new_post_data"><p class="advanced_data">Хабы: ${hubs}</p></div>
                    <div class="section_new_post_data"><p class="advanced_data">Тэги: ${tags}</p></div>
                    <p class="all_posts_item_text">${article['text']}</p>                       
                    <p id="button_article_${article['hubrId']}"><a href="${window.location.origin}/article.html?id=${article['id']}">Читать дальше</a></p>
                </div>
            </div>
        </div>`;
}

async function render_preview_last_article(last_article) {
    //hubs
    let hubs = '';
    for(let i = 0; i < last_article[0]['hubs'].length; i++) {
        hubs += i == last_article[0]['hubs'].length - 1
            ? `<p class="advanced_data"><a href="${window.location.origin}/hubs.html?hub=${last_article[0]['hubs'][i].trim().toLowerCase()}">${last_article[0]['hubs'][i]} </a></p>`
            : `<p class="advanced_data"><a href="${window.location.origin}/hubs.html?hub=${last_article[0]['hubs'][i].trim().toLowerCase()}">${last_article[0]['hubs'][i]}, </a></p>`;
    }

    //tags
    let tags = '';
    for(let i = 0; i < last_article[0]['tags'].length; i++) {
        tags += i == last_article[0]['tags'].length - 1
            ? `<p class="advanced_data"><a href="${window.location.origin}/tags.html?tag=${last_article[0]['tags'][i].trim().toLowerCase()}">${last_article[0]['tags'][i]} </a></p>`
            : `<p class="advanced_data"><a href="${window.location.origin}/tags.html?tag=${last_article[0]['tags'][i].trim().toLowerCase()}">${last_article[0]['tags'][i]}, </a></p>`;
    }

    //comments
    const comment_count = await get_comments_count(last_article[0]['id']);
    return `
        <div class="container">
            <div class="section_new_post_flex">
                <div class="section_new_post_text">
                    <h3 class="section_h3">Новый пост</h3>
                    <p class="section_p_attr">${last_article[0]['authorNickName']} | ${get_datetime_string(last_article[0]['timePublished'])}</p>
                    <h2 class="section_h2">
                        <a class="site_links" href="${window.location}article.html?id=${last_article[0]['id']}">
                            ${last_article[0]['title']}
                        </a>
                    </h2>
                    <div class="section_new_post_data">
                        <p class="advanced_data">Комментарии: ${comment_count}</p>
                        <p class="advanced_data">Просмотров: ${last_article[0]['readingCount']}</p>
                    </div>
                    <div class="section_new_post_data"><p class="advanced_data">Хабы: ${hubs}</p></div>
                    <div class="section_new_post_data"><p class="advanced_data">Тэги: ${tags}</p></div>
                    <p class="section_p_legend">${last_article[0]['text']}</p>                    
                    <p id="button_article_${last_article[0]['hubrId']}"><a href="${window.location.origin}/article.html?id=${last_article[0]['id']}">Читать дальше</a></p>
                </div>
                <div class="section_new_post_pic">
                    <img class="section_new_post_img" src="${last_article[0]['imageURL']}" alt="image_${last_article[0]['hubrId']}">
                </div>                    
            </div>               
        </div>`;
}

async function render_page() {
    //1 header
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
    //
    await add_header_links();//ссылки в header

    //2 main
    if (!await render_main())
        return;

    //account
    render_account();

    //3 footer
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
}