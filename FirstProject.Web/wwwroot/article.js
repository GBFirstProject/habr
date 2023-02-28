"use strict";
let userClaims = null;
let article = null;
let id = null;
const page_number_deafult = 1,
    page_size_default = 10;
let page_number = null,
    page_size = null;
//arrays
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
    load();
})();

async function get_article() {
    //получить id
    const article_id = id;//get_id();
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

/*function get_id() {
    const queryString = window.location.search;
    const urlParams = new URLSearchParams(queryString);
    return urlParams.get('id');
}*/

async function load() {
    await render_page();
}

async function render_article(article) {
    const prev_link = window.location.origin;
    //hubs
    let hubs = '';
    for(let i = 0; i < article['hubs'].length; i++) {
        hubs += i == article['hubs'].length - 1
            ? `<p class="advanced_data"><a href="${window.location.origin}/hubs.html?hub=${article['hubs'][i].trim().toLowerCase()}">${article['hubs'][i]} </a></p>`
            : `<p class="advanced_data"><a href="${window.location.origin}/hubs.html?hub=${article['hubs'][i].trim().toLowerCase()}">${article['hubs'][i]}, </a></p>`;
    }

    //количество комментариев
    const comment_count = await get_comments_count(id);//get_id()
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
        <div class="all_posts_pagination">
            <p id="button_back"><u>Назад</u></p>
        </div>
        <div class="comments"></div>`;
    //
    const article_div = document.getElementById('article_div');
    if (article_div == null)
        return false;
    article_div.innerHTML = '';
    article_div.insertAdjacentHTML('afterbegin', textHTML);

    //события
    const button_back = document.getElementById('button_back');
    button_back.addEventListener('click', () => window.history.back());
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

    //добавить комментарии
    textHTML = '';
    const comments = await get_comments(id, page_number, page_size);//get_id()
    comments.forEach(comment => {
        textHTML += `
            <div>
                <p class="section_p_attr">${comment.username} | ${get_datetime_string(comment['createdAt'])}</p>
                <p class="article_text">${comment['content']}</p>
                <div class="section_new_post_data">
                    <p class="advanced_data">Лайки: ${comment['likes']}</p>
                    <p class="advanced_data">Дизлайки: ${comment['dislikes']}</p>
                </div>
            </div>`;        
    });
    textHTML += '<div class="all_posts_pagination">';
    textHTML += render_comments_pagination(comment_count);
    textHTML += '</div>';

    //добавить на страницу
    const comments_div = document.querySelector('.comments');
    if (comments_div == null)
        return;
    comments_div.insertAdjacentHTML('afterbegin', textHTML);

    //события
    /*const comments_element = document.getElementById(`comments_${page_number_comment}`);
    if (comments_element != null)
        comments_element.addEventListener('click', render_comments(get_id(), page_number_comment + 1, page_size_comment));*/
    return true;
}

/*async function render_comments(id, page_number, page_size) {
    //добавить комментарии
    let textHTML = '';
    const comments = await get_comments(get_id(), page_number, page_size);
    comments.forEach(comment => {
        textHTML += `
            <div id="comments_${page_number}">
                <p class="section_p_attr">пользователь | ${get_datetime_string(comment['createdAt'])}</p>
                <p class="article_text">${comment['content']}</p>
                <div class="section_new_post_data">
                    <p class="advanced_data">Лайки: ${comment['likes'].length}</p>
                    <p class="advanced_data">Дизлайки: ${comment['dislikes'].length}</p>
                </div>
            </div>`;        
    });
    textHTML += `<p class="section_p_attr"><u>загрузить следующие ${page_size} комментариев</u></p>`;
    return textHTML;
}*/

async function render_main() {
    //статья
    article = await get_article();
    if (article == null)
        return;
    return await render_article(article);
}

function render_comments_pagination(comment_count) {
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
    //
    /*const all_posts_pagination = document.querySelector('.all_posts_pagination');
    if (all_posts_pagination == null)
        return false;
    all_posts_pagination.innerHTML = '';
    all_posts_pagination.insertAdjacentHTML('afterbegin', textHTML);

    //события пагинации
    return true;*/
}