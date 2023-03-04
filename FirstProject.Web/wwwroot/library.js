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

async function get_article(id) {
    const request = await fetch(`/articles/get-by-id?${new URLSearchParams({ articleId: id })}`, {
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

function get_article_html(article, comment_count) {
    const prev_link = window.location.origin;
    //hubs
    let hubs = '';
    for (let i = 0; i < article['hubs'].length; i++) {
        hubs += i == article['hubs'].length - 1
            ? `<p class="advanced_data"><a class="adv_data_a" href="${window.location.origin}/hubs.html?hub=${article['hubs'][i].trim().toLowerCase()}">${article['hubs'][i]} </a></p>`
            : `<p class="advanced_data"><a class="adv_data_a" href="${window.location.origin}/hubs.html?hub=${article['hubs'][i].trim().toLowerCase()}">${article['hubs'][i]}, </a></p>`;
    }
    //
    let textHTML = `
        <div class="section_new_post_text">
            <p class="section_p_attr">${article['authorNickName']} | ${get_datetime_string(article['timePublished'])}</p>
            <h2 class="section_h2"><a class="site_links" href="#">${article['title']}</a></h2>            
            <div class="section_new_post_data">
                <p class="advanced_data">Комментарии: ${comment_count}</p>
                <p class="advanced_data">Просмотров: ${article['readingCount']}</p>
            </div>
            <div class="section_new_post_data">
                ${hubs}
            </div>            
        </div>
        <p class="article_text">${article['fullTextHtml']}</p>
        <div class="comments">
            <h2 class="section_h2" id="comments_header">Комментарии</h2>
            <div class="all_posts_pagination" id="comments_pagination">
            </div>
        </div>`;
    return textHTML;
}

function get_article_comments_html(articles, comment_count, comments) {
    //добавить комментарии
    let textHTML = '';
    for (let i = comments.length - 1; i > 0; i--) {
        textHTML += `
            <div>
                <p class="section_p_attr">пользователь | ${get_datetime_string(comments[i]['createdAt'])}</p>
                <p class="article_text">${comments[i]['content']}</p>
                <div class="section_new_post_data">
                    <p class="advanced_data">Лайки: ${comments[i]['likes'].length}</p>
                    <p class="advanced_data">Дизлайки: ${comments[i]['dislikes'].length}</p>
                </div>
            </div>`;
    }
    return textHTML;
}

function get_article_comments_pagination_html(comment_count) {
    //пагинация
    let textHTML = '';
    const pages_count = Math.ceil(comment_count / page_size);
    //
    if (comment_count >= 0 && comment_count < pages_count) {
        //единственная страница
        textHTML += `<a class="all_posts_pag " href="#prev">Назад</a>
                <div class="pagination">1</div>
                <a class="all_posts_pag" href="#next">Вперед</a>`;
    } else if (comment_count > pages_count) {
        //несколько страниц
        const penultimate_page = pages_count - 1;
        const last_page = pages_count;

        //назад
        textHTML += page_number > 1
            ? `<a class="all_posts_pag" href="${window.location.origin}/article.html?id=${id}&PageNumber=${page_number - 1}&PageSize=${page_size}">Назад</a>`
            : `<a class="all_posts_pag">Назад`;

        //1 и 2 страницы обязательно
        textHTML += `<a href="${window.location.origin}/article.html?id=${id}&PageNumber=1&PageSize=${page_size}"><div class="pagination">1</div></a>
            <a href="${window.location.origin}/article.html?id=${id}&PageNumber=2&PageSize=${page_size}"><div class="pagination">2</div></a>`;

        //предыдущие страницы (если есть)      
        if (page_number > 2) {
            const prev_count = page_number - 2;
            //
            if (prev_count <= 4) {
                for (let i = 3, j = 0; j < prev_count - 1; i++, j++)
                    textHTML += `<a href="${window.location.origin}/article.html?id=${id}&PageNumber=${i}&PageSize=${page_size}"><div class="pagination">${i}</div></a>`;
            } else {
                textHTML += `<div class="pagination">...</div>`;
                for (let i = page_number - 2; i < page_number; i++)
                    textHTML += `<a href="${window.location.origin}/article.html?id=${id}&PageNumber=${i}&PageSize=${page_size}"><div class="pagination">${i}</div></a>`;
            }
        }

        //текущая страница
        if (page_number > 2)
            textHTML += `<a href="${window.location.origin}/article.html?id=${id}&PageNumber=${page_number}&PageSize=${page_size}"><div class="pagination">${page_number}</div></a>`;

        //последующие следующие (если есть)
        const next_count = last_page - page_number;
        if (next_count > 5) {
            //page_number, page_number + 1, page_number + 2 ... penultimate_page, last_page            
            for (let i = 1; i < 3; i++) {
                if (page_number + i > 2)
                    //if (page_number + i < page_size)
                    textHTML += `<a href="${window.location.origin}/article.html?id=${id}&PageNumber=${page_number + i}&PageSize=${page_size}"><div class="pagination">${page_number + i}</div></a>`;
            }
            textHTML += `<div class="pagination">...</div>
                <a href="${window.location.origin}/article.html?id=${id}&PageNumber=${penultimate_page}&PageSize=${page_size}"><div class="pagination">${penultimate_page}</div></a>
                <a href="${window.location.origin}/article.html?id=${id}&PageNumber=${last_page}&PageSize=${page_size}"><div class="pagination">${last_page}</div></a>`;
        } else {
            for (let i = page_number + 1; i <= last_page; i++)
                textHTML += `<a href="${window.location.origin}/article.html?id=${id}&PageNumber=${i}&PageSize=${page_size}"><div class="pagination">${i}</div></a>`;
        }

        //вперед
        textHTML += page_number < last_page
            ? `<a class="all_posts_pag" href="${window.location.origin}/article.html?id=${id}&PageNumber=${page_number + 1}&PageSize=${page_size}">Вперед</a>`
            : `<a class="all_posts_pag">Вперед`;
    }
    return textHTML;
}

function get_articles_html(articles, comment_count_array, articles_count) {
    //articles
    const articles_comment_count = comment_count_array;
    let articles_html = '';
    //
    for (let i = 0; i < articles.length; i++)
        articles_html += render_preview_article(articles[i], articles_comment_count[i]);
    const articles_pagination_html = render_articles_pagination(articles_count);
    return { articles_html: articles_html, articles_pagination_html: articles_pagination_html };
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

function get_last_article_html(last_article, last_article_comment_count) {
    //last_article
    //hubs
    let last_article_hubs = '';
    for (let i = 0; i < last_article[0]['hubs'].length; i++) {
        last_article_hubs += i == last_article[0]['hubs'].length - 1
            ? `<p class="advanced_data"><a class="adv_data_a" href="${window.location.origin}/hubs.html?hub=${last_article[0]['hubs'][i].trim().toLowerCase()}">${last_article[0]['hubs'][i]} </a></p>`
            : `<p class="advanced_data"><a class="adv_data_a" href="${window.location.origin}/hubs.html?hub=${last_article[0]['hubs'][i].trim().toLowerCase()}">${last_article[0]['hubs'][i]}, </a></p>`;
    }

    //tags
    let last_article_tags = '';
    for (let i = 0; i < last_article[0]['tags'].length; i++) {
        last_article_tags += i == last_article[0]['tags'].length - 1
            ? `<p class="advanced_data"><a class="adv_data_a" href="${window.location.origin}/tags.html?tag=${last_article[0]['tags'][i].trim().toLowerCase()}">${last_article[0]['tags'][i]} </a></p>`
            : `<p class="advanced_data"><a class="adv_data_a" href="${window.location.origin}/tags.html?tag=${last_article[0]['tags'][i].trim().toLowerCase()}">${last_article[0]['tags'][i]}, </a></p>`;
    }

    //comments
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
                        <p class="advanced_data">Комментарии: ${last_article_comment_count}</p>
                        <p class="advanced_data">Просмотров: ${last_article[0]['readingCount']}</p>
                    </div>
                    <div class="section_new_post_data"><p class="advanced_data">Хабы: ${last_article_hubs}</p></div>
                    <div class="section_new_post_data"><p class="advanced_data">Тэги: ${last_article_tags}</p></div>
                    <p class="section_p_legend">${last_article[0]['text']}</p>                    
                    <button class="read_more_btn" id="button_article_${last_article[0]['hubrId']}"><a class="read_more_btn_text" href="${window.location.origin}/article.html?id=${last_article[0]['id']}">Читать дальше</a></button>
                </div>
                <div class="section_new_post_pic">
                    <img class="section_new_post_img" src="${last_article[0]['imageURL']}" alt="image_${last_article[0]['hubrId']}">
                </div>                    
            </div>               
        </div>`;
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

function render_article(article_html, article_comments_html, article_comments_pagination_html) {
    //статья
    const article_div = document.getElementById('article_div');
    if (article_div == null)
        return false;
    article_div.innerHTML = '';
    article_div.insertAdjacentHTML('afterbegin', article_html);

    //события
    /*const button_back = document.getElementById('button_back');
    button_back.addEventListener('click', () => window.history.back());*/
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

    //комментарии
    const comments_header = document.getElementById('comments_header');
    if (comments_header == null)
        return false;
    comments_header.insertAdjacentHTML('afterend', article_comments_html);

    //пагинация
    const comments_pagination = document.getElementById('comments_pagination');
    if (comments_pagination == null)
        return false;
    comments_pagination.insertAdjacentHTML('afterbegin', article_comments_pagination_html);
}

function render_articles(articles_html, articles_pagination_html) {
    //статьи
    let section_all_posts = document.querySelector('.section_all_posts_block');
    if (section_all_posts == null)
        return false;
    //
    section_all_posts.innerHTML = '';
    section_all_posts.insertAdjacentHTML('afterbegin', articles_html);

    //пагинация
    const all_posts_pagination = document.querySelector('.all_posts_pagination');
    if (all_posts_pagination == null)
        return false;
    all_posts_pagination.innerHTML = '';
    all_posts_pagination.insertAdjacentHTML('afterbegin', articles_pagination_html);
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

function render_last_article(last_article_html) {
    //добавить на страницу
    let section_new_post = document.querySelector('.section_new_post');
    if (section_new_post == null)
        return false;
    //
    section_new_post.innerHTML = '';
    section_new_post.insertAdjacentHTML('afterbegin', last_article_html);
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
                        <p class="advanced_data">Комментарии: ${comment_count}</p>
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

async function render_preview_last_article(last_article) {
    //hubs
    let hubs = '';
    for(let i = 0; i < last_article[0]['hubs'].length; i++) {
        hubs += i == last_article[0]['hubs'].length - 1
            ? `<p class="advanced_data"><a class="adv_data_a" href="${window.location.origin}/hubs.html?hub=${last_article[0]['hubs'][i].trim().toLowerCase()}">${last_article[0]['hubs'][i]} </a></p>`
            : `<p class="advanced_data"><a class="adv_data_a" href="${window.location.origin}/hubs.html?hub=${last_article[0]['hubs'][i].trim().toLowerCase()}">${last_article[0]['hubs'][i]}, </a></p>`;
    }

    //tags
    let tags = '';
    for(let i = 0; i < last_article[0]['tags'].length; i++) {
        tags += i == last_article[0]['tags'].length - 1
            ? `<p class="advanced_data"><a class="adv_data_a" href="${window.location.origin}/tags.html?tag=${last_article[0]['tags'][i].trim().toLowerCase()}">${last_article[0]['tags'][i]} </a></p>`
            : `<p class="advanced_data"><a class="adv_data_a" href="${window.location.origin}/tags.html?tag=${last_article[0]['tags'][i].trim().toLowerCase()}">${last_article[0]['tags'][i]}, </a></p>`;
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
                    <button class="all_read_more_btn" id="button_article_${last_article[0]['hubrId']}"><a class="all_read_more_btn_text" href="${window.location.origin}/article.html?id=${last_article[0]['id']}">Читать дальше</a></p>
                </div>
                <div class="section_new_post_pic">
                    <img class="section_new_post_img" src="${last_article[0]['imageURL']}" alt="image_${last_article[0]['hubrId']}">
                </div>                    
            </div>               
        </div>`;
}

function render_articles_pagination(articles_count) {
    //пагинация
    let textHTML = '';
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
    return textHTML;
}