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

function add_progressbar() {
    //добавить "прогрессбар"
    let progress_div = document.getElementById('progress_div');
    if (progress_div == null)
        return false;
    //
    let textHTML = `
        <h4 class="progress_div" id="progress">
            <div class="dot-flashing"></div>
        </h4>`;
    progress_div.insertAdjacentHTML('afterbegin', textHTML);
    return true;
}

async function button_dislike_article_click(e) {
    if (e.currentTarget == null || e.currentTarget === 'undefined')
        return;
    //
    const id = e.currentTarget.id.substr(8);
    const response = await fetch(`/articles/dislike?articleId=${id}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
            'X-CSRF': '1'
            }
    })
        .then(response => response.json())
        .catch(e => console.log(e));
    //
    if (response == null || typeof response === 'undefined')
        return false;
    if (!response.hasOwnProperty('result'))
        return false;
    
    //обновить страницу
    if (!response.isSuccess)
        return false;
    //
    const dislike = document.getElementById(`dislike_${id}`);
    if (dislike != null)
        dislike.innerText = `Дизлайки: ${response.result.dislikes.length}`;
    return true;
}

async function button_like_article_click(e) {
    if (e.currentTarget == null || e.currentTarget === 'undefined')
        return;
    //
    const id = e.currentTarget.id.substr(5);
    const response = await fetch(`/articles/like?articleId=${id}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
            'X-CSRF': '1'
            }
    })
        .then(response => response.json())
        .catch(e => console.log(e));
    //
    if (response == null || typeof response === 'undefined')
        return false;
    if (!response.hasOwnProperty('result'))
        return false;
    
    //обновить страницу
    if (!response.isSuccess)
        return false;
    //
    const like = document.getElementById(`like_${id}`);
    if (like != null)
        like.innerText = `Лайки: ${response.result.likes.length}`;
    return true;
}

function contains_role(role) {
    return roles.includes(role);
}

function delete_progressbar() {
    //убрать "прогрессбар"
    let progress_div = document.getElementById('progress');
    if (progress_div != null)
        progress_div.remove();
}

function get_account_data() {
    //данные пользователя
    let account_data = {'email': '', 'name': 'гость', 'role': 'guest', 'author_id': ''};
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

                case 'sub':
                    account_data.author_id = claim.value.trim().toLowerCase();
            }
        }
    }
    return account_data;
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

function get_articles_html(articles, comment_count_array) {
    //articles
    const articles_comment_count = comment_count_array;
    let articles_html = '';
    //
    for (let i = 0; i < articles.length; i++)
        articles_html += render_preview_article(articles[i], articles_comment_count[i]);
    return { articles_html: articles_html };
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
    if (response.result == null)
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

function get_header_links_html(header_hubs) {
    const hubs = header_hubs;
    let header_hubs_html = `
        <li class="header_nav_item" id="all_hubs">
            <a class="header_link" href="${window.location.origin}/hubs.html">
                Топ хабы
            </a>
        </li>
        <li class="header_nav_item" id="all_tags">
            <a class="header_link" href="${window.location.origin}/tags.html">
                Топ тэги
            </a>
        </li>`;
    //
    hubs.result.forEach(hub => {
        header_hubs_html += `
            <li class="header_nav_item">
                <a class="header_link" href="${window.location.origin}/hubs.html?hub=${hub.title.trim().toLowerCase()}">
                    ${hub.title}
                </a>
            </li>`;
    });
    return header_hubs_html;
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

async function get_top_article() {
    const request = await fetch(`/articles/get-best-article`, {
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

function get_top_article_html(top_article, top_article_comment_count) {
    return render_preview_top_article(top_article, top_article_comment_count);
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
    return { result: response.result, title: '', comments: null };
}

async function get_top_tags(tags_count) {
    const response = await fetch(`/tag/top?count=${tags_count}`, {
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
    return { result: response.result, title: '', comments: null };
}

function get_roles() {
    return roles;
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

function render_account(account_data) {
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
    }
}

function render_added_articles(articles_html) {
    const section_all_posts_block = document.querySelector('.section_all_posts_block');
    if (section_all_posts_block != null)
        section_all_posts_block.insertAdjacentHTML('beforeend', articles_html);
}

function render_articles(articles_html, articles/*, articles_pagination_html*/) {
    //статьи
    let section_all_posts = document.querySelector('.section_all_posts_block');
    if (section_all_posts == null)
        return false;
    //
    section_all_posts.innerHTML = '';
    section_all_posts.insertAdjacentHTML('afterbegin', articles_html);

    //события
    articles.forEach(article => {
        //лайк
        const button_like = document.getElementById(`like_${article.id}`);
        if (button_like != null)
            button_like.addEventListener('click', button_like_article_click);

        //дизлайк
        const button_dislike = document.getElementById(`dislike_${article.id}`);
        if (button_dislike != null)
            button_dislike.addEventListener('click', button_dislike_article_click);
    });
}

function render_footer(footer_html) {
    const footer = document.getElementById('footer');
    if (footer == null)
        return;
    //
    footer.innerHTML = '';
    footer.insertAdjacentHTML('afterbegin', footer_html);
}

function render_header(html) {
    if (html == null)
        return false;
    //
    const header_html = html.header_html;
    const header_hubs_html = html.header_hubs_html;

    const header = document.getElementById('header');
    if (header == null)
        return false;
    //
    header.innerHTML = '';
    header.insertAdjacentHTML('afterbegin', header_html);

    //добавить 4 популярных хаба
    const header_nav_list = document.querySelector('.header_nav_list');
    if (header_nav_list != null)
        header_nav_list.insertAdjacentHTML('beforeend', header_hubs_html);
    return true;
}

function render_main(main_html) {
    const main = document.getElementsByTagName('main');
    if (main.length == 0)
        return false;
    main[0].insertAdjacentHTML('beforeend', main_html);
    return true;
}

function render_preview_article(article, comment_count) {
    //вывод статьи
    //hubs
    let hubs = '';
    for (let i = 0; i < article['hubs'].length; i++) {
        hubs += i == article['hubs'].length - 1
            ? `<p class="advanced_data"><a class="adv_data_a" href="${window.location.origin}/hubs.html?hub=${article['hubs'][i].trim().toLowerCase()}">${article['hubs'][i]} </a></p>`
            : `<p class="advanced_data"><a class="adv_data_a" href="${window.location.origin}/hubs.html?hub=${article['hubs'][i].trim().toLowerCase()}">${article['hubs'][i]}, </a></p>`;
    }

    //tags
    let tags = '';
    for (let i = 0; i < article['tags'].length; i++) {
        tags += i == article['tags'].length - 1
            ? `<p class="advanced_data"><a class="adv_data_a" href="${window.location.origin}/tags.html?tag=${article['tags'][i].trim().toLowerCase()}">${article['tags'][i]} </a></p>`
            : `<p class="advanced_data"><a class="adv_data_a" href="${window.location.origin}/tags.html?tag=${article['tags'][i].trim().toLowerCase()}">${article['tags'][i]}, </a></p>`;
    }
    return `
        <div class="all_posts_first all_posts_item">
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
                        <p class="advanced_data" id="like_${article['id']}">Лайки: ${article['likes'].length}</p>
                        <p class="advanced_data" id="dislike_${article['id']}">Дизлайки: ${article['dislikes'].length}</p>
                    </div>                    
                    <div class="section_new_post_data">
                        <p class="advanced_data" id="comments_${article['id']}">Комментарии: ${comment_count}</p>
                        <p class="advanced_data">Просмотров: ${article['readingCount']}</p>
                    </div>
                    <div class="section_new_post_data"><p class="advanced_data">Хабы: ${hubs}</p></div>
                    <div class="section_new_post_data"><p class="advanced_data">Тэги: ${tags}</p></div>
                    <p class="all_posts_item_text">${article['text']}</p>                       
                    <button class="all_read_more_btn" id="button_article_${article['hubrId']}"><a class="all_read_more_btn_text" href="${window.location.origin}/article.html?id=${article['id']}">Читать дальше</a></button>
                </div>
            </div>
        </div>`;
}

function render_preview_top_article(top_article, top_article_comment_count) {
    //hubs
    let top_article_hubs = '';
    for (let i = 0; i < top_article['hubs'].length; i++) {
        top_article_hubs += i == top_article['hubs'].length - 1
            ? `<p class="advanced_data"><a class="adv_data_a" href="${window.location.origin}/hubs.html?hub=${top_article['hubs'][i].trim().toLowerCase()}">${top_article['hubs'][i]} </a></p>`
            : `<p class="advanced_data"><a class="adv_data_a" href="${window.location.origin}/hubs.html?hub=${top_article['hubs'][i].trim().toLowerCase()}">${top_article['hubs'][i]}, </a></p>`;
    }

    //tags
    let top_article_tags = '';
    for (let i = 0; i < top_article['tags'].length; i++) {
        top_article_tags += i == top_article['tags'].length - 1
            ? `<p class="advanced_data"><a class="adv_data_a" href="${window.location.origin}/tags.html?tag=${top_article['tags'][i].trim().toLowerCase()}">${top_article['tags'][i]} </a></p>`
            : `<p class="advanced_data"><a class="adv_data_a" href="${window.location.origin}/tags.html?tag=${top_article['tags'][i].trim().toLowerCase()}">${top_article['tags'][i]}, </a></p>`;
    }
    return `
        <div class="container">
            <div class="section_new_post_flex">
                <div class="section_new_post_text">
                    <h3 class="section_h3">Самый просматриваемый пост</h3>
                    <p class="section_p_attr">${top_article['authorNickName']} | ${get_datetime_string(top_article['timePublished'])}</p>
                    <h2 class="section_h2">
                        <a class="site_links" href="${window.location}article.html?id=${top_article['id']}">
                            ${top_article['title']}
                        </a>
                    </h2>
                    <div class="section_new_post_data">
                        <p class="advanced_data" id="like_${top_article['id']}">Лайки: ${top_article['likes'].length}</p>
                        <p class="advanced_data" id="dislike_${top_article['id']}">Дизлайки: ${top_article['dislikes'].length}</p>
                    </div>
                    <div class="section_new_post_data">
                        <p class="advanced_data" id="comments_${top_article['id']}">Комментарии: ${top_article_comment_count}</p>
                        <p class="advanced_data">Просмотров: ${top_article['readingCount']}</p>
                    </div>
                    <div class="section_new_post_data"><p class="advanced_data">Хабы: ${top_article_hubs}</p></div>
                    <div class="section_new_post_data"><p class="advanced_data">Тэги: ${top_article_tags}</p></div>
                    <p class="section_p_legend">${top_article['text']}</p>                    
                    <button class="read_more_btn" id="button_article_${top_article['hubrId']}"><a class="read_more_btn_text" href="${window.location.origin}/article.html?id=${top_article['id']}">Читать дальше</a></button>
                </div>
                <div class="section_new_post_pic">
                    <img class="section_new_post_img" src="${top_article['imageURL']}" alt="image_${top_article['hubrId']}">
                </div>                    
            </div>               
        </div>`;
}

function render_top_article(top_article_html, id) {
    //добавить на страницу
    let section_new_post = document.querySelector('.section_new_post');
    if (section_new_post == null)
        return false;
    //
    section_new_post.innerHTML = '';
    section_new_post.insertAdjacentHTML('afterbegin', top_article_html);

    //события
    //лайк
    const button_like = document.getElementById(`like_${id}`);
    if (button_like != null)
        button_like.addEventListener('click', button_like_article_click);

    //дизлайк
    const button_dislike = document.getElementById(`dislike_${id}`);
    if (button_dislike != null)
        button_dislike.addEventListener('click', button_dislike_article_click);
}

function throttle(callee, timeout) {
    let timer = null;
    return function perform(...args) {
        if (timer)
            return;
        //
        timer = setTimeout(() => {
            callee(...args)
            clearTimeout(timer)
            timer = null
        }, timeout);
    }
}