"use strict";
let userClaims = null;
let action = null,
    article = null;
let id = null;
const page_number_deafult = 1,
    page_size_default = 10;
let page_number = null,
    page_size = null,
    response_json = null;
//arrays
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
    //
    action = urlParams.get('action');//.trim().toLowerCase();
    if (action == null)
        action = 'read';
    //
    switch(action) {
        case 'read':
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
            break;

        case 'create':
            break;

        case 'update':
            id = urlParams.get('id').trim().toLowerCase();
            break;

        case 'delete':
            id = urlParams.get('id').trim().toLowerCase();
            break;
    } 
} catch (e) {
    action = 'read';
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
})();

async function article_comments_load_click(e) {
    //удалить существующую ссылку на продолжение загрузки
    const article_comments_load = document.querySelector('.article_comments_load');
    if (article_comments_load != null)
        article_comments_load.remove();

    //загрузка комментариев
    page_number += 3;
    const article_comments = await get_comments(id, page_number, 3);
    const article_comments_html = get_article_comments_html(response_json.article, response_json.article_comment_count, article_comments);
    render_added_article_comments(article_comments_html, article_comments);
}

async function button_delete_comment_click(e) {

}

async function button_dislike_comment_click(e) {
    if (e.currentTarget == null || e.currentTarget === 'undefined')
        return;
    if (userClaims == null) {
        alert('Войдите в систему или зарегистрируйтесь, чтобы оценивать комментарии к постам');
        return;
    }
    //
    const id = e.currentTarget.id.substr(16);
    const response = await fetch(`/comments/dislike`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json-patch+json',
            'X-CSRF': '1'
            },
        body: JSON.stringify({commentId: id})
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
    return set_like_dislike_article_comment(response.result, id);
}

async function button_like_comment_click(e) {
    if (e.currentTarget == null || e.currentTarget === 'undefined')
        return;
    if (userClaims == null) {
        alert('Войдите в систему или зарегистрируйтесь, чтобы оценивать комментарии к постам');
        return;
    }
    //
    const id = e.currentTarget.id.substr(13);
    const response = await fetch(`/comments/like`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json-patch+json',
            'X-CSRF': '1'
            },
        body: JSON.stringify({commentId: id})
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
    return set_like_dislike_article_comment(response.result, id);
}

async function button_reply_comment_click(e) {
    if (e.currentTarget == null || e.currentTarget === 'undefined')
        return;
    if (userClaims == null) {
        alert('Войдите в систему или зарегистрируйтесь, чтобы отвечать на комментарии к постам');
        return;
    }
    
    //добавить форму ответа
    let textHTML = `
        <div class="article_comment_reply_block article_comment_item">
            <textarea class="article_field" id="article_comment_reply_${id}"></textarea>
            <button id="send_comment_reply">Отправить</button>
        </div>`;
    
    //поиск родительского элемента
    const parent = e.currentTarget.parentElement.parentElement;
    if (parent == null)
        return;
    //
    const parent_id = parent.id.substr(16);
    const article_comment_reply = document.getElementById(`article_comment_${parent_id}`);
    if (article_comment_reply != null)
        article_comment_reply.insertAdjacentHTML('beforeend', textHTML);

    //события
    const send_comment_reply = document.getElementById('send_comment_reply');
    if (send_comment_reply != null)
        send_comment_reply.addEventListener('click', button_send_comment_reply_click);

    /*const id = e.currentTarget.id.substr(14);
    const response = await fetch(`/comments`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json-patch+json',
            'X-CSRF': '1'
            },
        body: JSON.stringify({articleId: '', content: '', replyTo: id})
    })
        .then(response => response.json())
        .catch(e => console.log(e));
    //
    if (response == null || typeof response === 'undefined')
        return false;
    if (!response.hasOwnProperty('result'))
        return false;*/
    
    //обновить страницу
    /*if (!response.isSuccess)
        return false;
    return set_like_dislike_article_comment(response.result, id);*/
}

async function button_send_comment_click(e) {
    if (e.currentTarget == null || e.currentTarget === 'undefined')
        return;
    
    //сообщение
    let value = '';
    const article_comment_input = document.getElementById('article_comment_input');
    if (article_comment_input != null)
        value = article_comment_input.value.trim();
    //
    const response = await fetch(`/comments`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json-patch+json',
            'X-CSRF': '1'
            },
        body: JSON.stringify({articleId: id, content: value, replyTo: id})
    })
        .then(response => response.json())
        .catch(e => console.log(e));
    //
    if (response == null || typeof response === 'undefined')
        return false;
    if (!response.hasOwnProperty('result'))
        return false;
}

async function button_send_comment_reply_click(e) {
    if (e.currentTarget == null || e.currentTarget === 'undefined')
        return;
    
    //сообщение
    let value = '';
    const textarea_content = document.querySelector(`[id^="article_comment_reply_"]`);
    if (textarea_content != null)
        value = textarea_content.value.trim();
    //
    const comment_id = e.currentTarget.parentElement.parentElement.id.substr(16);
    const response = await fetch(`/comments`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json-patch+json',
            'X-CSRF': '1'
            },
        body: JSON.stringify({articleId: id, content: value, replyTo: comment_id})
    })
        .then(response => response.json())
        .catch(e => console.log(e));
    //
    if (response == null || typeof response === 'undefined')
        return false;
    if (!response.hasOwnProperty('result'))
        return false;
    
    //обновить страницу
    /*if (!response.isSuccess)
        return false;
    return set_like_dislike_article_comment(response.result, id);*/
}

async function checkPosition() {
    if (action != 'read')
        return;
    //
    const height = document.body.offsetHeight;
    const screenHeight = window.innerHeight;
    const scrolled = window.scrollY;
    const threshold = height - screenHeight / 4;//2
    const position = scrolled + screenHeight;
    //
    if (position >= threshold) {
        // Если мы пересекли полосу-порог, вызываем нужное действие.
        let load_pause = page_number % 5;
        if (load_pause == 0) {
            const article_comments_load = document.querySelector('.article_comments_load');
            if (article_comments_load == null) {
                const textHTML = `<div class="article_comments_load"><u>Загрузить комментарии</u></div>`;
                const comments = document.querySelector('.comments');
                if (comments != null)
                    comments.insertAdjacentHTML('beforeend', textHTML);
                //
                const article_comments_load = document.querySelector('.article_comments_load');
                article_comments_load.addEventListener('click', article_comments_load_click);
            }    
            return;                    
        }        
        article_comments_load_click();
    }
}

async function create_article(action, account_data, article) {
    let hubs = '';
    for (let i = 0; i < article['hubs'].length; i++) {
        hubs += i == article['hubs'].length - 1
            ? `${article['hubs'][i]}`
            : `${article['hubs'][i]},`;
    }
    //
    let tags = '';
    for (let i = 0; i < article['tags'].length; i++) {
        tags += i == article['tags'].length - 1
            ? `${article['tags'][i]}`
            : `${article['tags'][i]},`;
    }

    const response = new Request(`/articles/add-article`, {
        method: 'POST',
        body: JSON.stringify(article),
        headers: new Headers({
            "X-CSRF": "1", 
            "Content-Type": "application/json",
            "Accept": "*/*"
        })
    });
    //
    try {
        var resp = await fetch(response);
        let data;
        if (resp.ok)
            data = await resp.json();      
    } catch (e) {
        console.log('');
    }
    //
    if (response == null || typeof response === 'undefined')
        return false;
    if (!response.hasOwnProperty('result'))
        return false;
    return response.isSuccess;
}

async function delete_article(id) {
    const response = await fetch(`/articles/delete-article?id=${id}`, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json',
            'X-CSRF': '1',
            'Accept': '*/*'
            },
    })
        .then(response => response.json())
        .catch(e => console.log(e));
    //
    if (response == null || typeof response === 'undefined')
        return false;

    /*const response = new Request(`/articles/delete-article?id=${id}`, {
        method: 'DELETE',
        headers: new Headers({
            "X-CSRF": "1", 
            "Content-Type": "application/json",
            "Accept": "*//**"
        })
    });
    //
    try {
        var resp = await fetch(response);
        let data;
        if (resp.ok)
            data = await resp.json();      
    } catch (e) {
        console.log('');
    }
    //
    if (response == null || typeof response === 'undefined')
        return false;
    if (!response.hasOwnProperty('result'))
        return false;
    return response.isSuccess;*/
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
    
    //tags
    let tags = '';
    for (let i = 0; i < article['tags'].length; i++) {
        tags += i == article['tags'].length - 1
            ? `<p class="advanced_data"><a class="adv_data_a" href="${window.location.origin}/tags.html?tag=${article['tags'][i].trim().toLowerCase()}">${article['tags'][i]} </a></p>`
            : `<p class="advanced_data"><a class="adv_data_a" href="${window.location.origin}/tags.html?tag=${article['tags'][i].trim().toLowerCase()}">${article['tags'][i]}, </a></p>`;
    }

    let textHTML = `
        <div class="section_new_post_text">
            <p class="section_p_attr">${article['authorNickName']} | ${get_datetime_string(article['timePublished'])}</p>
            <h2 class="section_h2"><a class="site_links" href="#">${article['title']}</a></h2>
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
        </div>
        <!--<p><img src="${article['imageURL']}"/></p>-->
        <p class="article_text">${article['fullTextHtml']}</p>
        <div class="comments">
            <h2 class="section_h2" id="comments_header">Комментарии</h2>
            </div>
        </div>`;
    return textHTML;
}

function get_article_comments_html(articles, comment_count, comments) {
    //добавить комментарии
    let textHTML = '';
    for (let i = comments.length - 1; i >= 0; i--) {
        textHTML += `
            <div class="article_comment_block">
                <div class="article_comment">
                    <p class="section_p_attr article_comment_item">${comments[i]['username']} | ${get_datetime_string(comments[i]['createdAt'])}</p>
                    <div class="article_comment_item">${comments[i]['content']}</div>
                    <div class="section_new_post_data article_comment_item">
                        <p class="advanced_data" id="like_comment_${comments[i]['id']}">Лайки: ${comments[i]['likes']}</p>
                        <p class="advanced_data" id="dislike_comment_${comments[i]['id']}">Дизлайки: ${comments[i]['dislikes']}</p>
                        <p class="advanced_data" id="reply_comment_${comments[i]['id']}">Ответить</p>                        
                    </div>
                    ${get_article_comment_reply(comments[i]['replies'])}
                </div>
            </div>`;                            
    }
    textHTML += `
        <div class="article_comment_input_block">
            <div class="article_comment">
                <textarea id="article_comment_input"></textarea>
                <button id="send_comment">Отправить</button>
            </div>
        </div>`;
    return textHTML;
}

function get_article_comment_reply(comment_replies) {
    let textHTML = '';
    if (comment_replies.length == 0)
        return textHTML;
    //
    for(let comment_reply of comment_replies)
        textHTML += get_article_comment_reply_html(comment_reply);
    return textHTML;
}

function get_article_comment_reply_html(comment) {
    return `
        <div class="article_comment_reply" id="article_comment_${comment['id']}">
            <p class="section_p_attr">${comment['username']} | ${get_datetime_string(comment['createdAt'])}</p>
            <p class="article_comment_item">${comment['content']}</p>
            <div class="section_new_post_data article_comment_item">
                <p class="advanced_data" id="like_comment_${comment['id']}">Лайки: ${comment['likes']}</p>
                <p class="advanced_data" id="dislike_comment_${comment['id']}">Дизлайки: ${comment['dislikes']}</p>
                <p class="advanced_data" id="reply_comment_${comment['id']}">Ответить</p>
            </div>
        </div>`;
}

async function get_comments(id, page_number, page_size) {
    //comments
    ///comments?articleId=${id}&index=${(page_number - 1) * page_size}&count=${page_size}
    const response = await fetch(`/comments?articleId=${id}&index=${page_number - 1}&count=${page_size}`, {
        method: 'GET',
        headers: new Headers({ "X-CSRF": "1" })
    })
        .then(response => response.json())
        .catch(e => console.log(e));
    //
    if (response == null || typeof response === 'undefined')
        return [];//0
    if (!response.hasOwnProperty('result'))
        return [];
    return response.result;
}

function get_create_article_html() {
    return `
        <div class="page_panel">
            <p id="button_back"><a href="${window.location.origin}/account.html">Личный кабинет</a></p>
        </div>
        <div class="article_form">
            <form class="container_flex_column">
                <label>Автор:</label>                    
                <input type="text" class="article_field" id="article_author" value="${userClaims.find((claim) => claim.type === 'name').value}" readonly>
                <label>Название:</label>                    
                <input type="text" class="article_field" id="article_title">
                <label>Текст:</label>                    
                <textarea class="article_field article_text" id="article_text"></textarea>
                <label>Изображение:</label>                    
                <input type="text" class="article_field" id="article_image">                
                <label>Тэги:</label>                    
                <input type="text" class="article_field" id="article_tags">
                <label>Хабы:</label>                    
                <input type="text" class="article_field" id="article_hubs">
                <label>Комментарии разрешены:</label>                    
                <select class="article_field" id="article_comments">
                    <option selected>Да</option>
                    <option >Нет</option>                
                </select>
                <p id="button_save"><u>Сохранить</u></p>
                <div id="message_div"></div>
            </form>
        </div>`;
}

function get_delete_article_html(article, article_id) {
    return article == null
        ? `
            <div class="container_flex_column">
                Статья id = "${article_id}" не найдена
            </div>
        `
        : `
            <div class="container_flex_column">
                Удалить статью "${article['title']}"?
                <div class="container_flex_row">
                    <p class="row_item" id="button_yes"><u>Да</u></p>
                    <p class="row_item" id="button_no" onclick="window.history.back()"><u>Нет</u></p>                
                </div>
                <div id="message_div"></div>
            </div>`;
}

function get_main_html() {
    return `
        <p id="button_back" onclick="window.history.back()"><u>Назад</u></p>
        <div id="article_div"></div>`;
}

function get_update_article_html(article, comment_count) {
    const prev_link = window.location.origin;
    //hubs
    let hubs = '';
    for (let i = 0; i < article['hubs'].length; i++) {
        hubs += i == article['hubs'].length - 1
            ? `${article['hubs'][i]}`
            : `${article['hubs'][i]},`;
    }

    let tags = '';
    for (let i = 0; i < article['tags'].length; i++) {
        tags += i == article['tags'].length - 1
            ? `${article['tags'][i]}`
            : `${article['tags'][i]},`;
    }
    //
    return `
        <div class="article_form">
            <form class="container_flex_column">
                <label>Автор:</label>                    
                <input type="text" class="article_field" id="article_author" value="${article['authorNickName']}" readonly>
                <label>Название:</label>                    
                <input type="text" class="article_field" id="article_title" value="${article['title']}">
                <label>Текст:</label>                    
                <textarea class="article_field article_text" id="article_text">${article['fullTextHtml'].trim()}</textarea>
                <label>Изображение:</label>                    
                <input type="text" class="article_field" id="article_image" value="${article['imageURL']}">
                <label>Тэги:</label>                    
                <input type="text" class="article_field" id="article_tags" value="${tags}">
                <label>Хабы:</label>                    
                <input type="text" class="article_field" id="article_hubs" value="${hubs}">
                <label>Дата публикации:</label>                    
                <input type="text" class="article_field" id="article_date" value="${get_datetime_string(article['timePublished'])}" readonly>
                <label>Комментарии разрешены:</label>                    
                <select class="article_field" id="article_comments">
                    <option ${article['commentsEnabled'] ? 'selected' : ''}>Да</option>
                    <option ${article['commentsEnabled'] ? '' : 'selected'}>Нет</option>                
                </select>
                <p id="button_save"><u>Сохранить</u></p>
                <div id="message_div"></div>
            </form>
        </div>`;
}

async function load() {
    //загрузка данных
    response_json = await load_data();
    if (response_json == null)
        return;

    //рендер html
    const header_hubs_html = get_header_links_html(response_json.header_hubs);
    const main_html = get_main_html();

    let article_html = '';
    let article_comments_html = '';

    //create
    if (action == 'create')
        article_html = get_create_article_html();

    //read
    if (action == 'read') {
        article_html = get_article_html(response_json.article, response_json.article_comment_count);
        article_comments_html = get_article_comments_html(response_json.article, response_json.article_comment_count, response_json.article_comments);
    }

    //update
    if (action == 'update') 
        article_html = get_update_article_html(response_json.article);

    //delete
    if (action == 'delete')
        article_html = get_delete_article_html(response_json.article, id);

    //добавить на страницу
    delete_progressbar();
    render_page(
        response_json,
        {
            header_hubs_html: header_hubs_html,
            main_html: main_html,
            article_html: article_html,
            article_comments_html: article_comments_html
        }
    );
}

async function load_data() {
    let header_html = '',
        footer_html = '',
        header_hubs = '';
    let article = null,
        article_comments = null,
        account_data = null;
    let article_comment_count = 0;

    //arrays
    let comment_count_array = [];

    //загрузка данных
    try {
        header_html = await get_header();
        footer_html = await get_footer();
        header_hubs = await get_top_hubs(4);//популярные хабы
        account_data = get_account_data();        
        
        //all
        if (action != 'create')
            article = await get_article(id);

        //read
        if (action == 'read') {
            article_comment_count = await get_comments_count(id);
            article_comments = await get_comments(id, page_number, 3);//article_comments = await get_comments(id, page_number, page_size);
        }           
    } catch (ex) {
        throw new Error('ошибка загрузки данных');
    }

    //объект
    return {
        header_html: header_html,
        footer_html: footer_html,
        header_hubs: header_hubs,
        account_data: account_data,
        article: article,
        article_comments: article_comments,
        article_comment_count: article_comment_count
    };
}

function render_added_article_comments(article_comments_html, article_comments) {
    //комментарии
    if (article_comments_html.trim() == '')
        return false;

    //убрать блок ввода комментария    
    const article_comment_input_block = document.querySelector('.article_comment_input_block');
    if (article_comment_input_block != null)
        article_comment_input_block.remove();    

    //добавить
    const comments = document.querySelector('.comments');
    if (comments == null)
        return false;
    comments.insertAdjacentHTML('beforeend', article_comments_html);

    /*const img_array = document.getElementsByTagName('img');
    for (let img of img_array) {
        const data_src = img.getAttribute('data-src');
        if (data_src == null)
            continue;
        //
        let src = document.createAttribute('src');
        src.value = data_src;
        //
        img.setAttributeNode(src);
        img.removeAttribute('data-src');
        img.removeAttribute('width');
        img.removeAttribute('height');
        img.classList.add('section_new_post_img_full');
    }*/
    
    //события
    //лайк комментария
    const likes_comment_array = document.querySelectorAll(`[id^="like_comment_"]`);
    for(let button_like_comment of likes_comment_array)
        button_like_comment.addEventListener('click', button_like_comment_click);

    //дизлайк комментария
    const dislikes_comment_array = document.querySelectorAll(`[id^="dislike_comment_"]`);
    for(let button_dislike_comment of dislikes_comment_array)
        button_dislike_comment.addEventListener('click', button_dislike_comment_click);

    //ответ на комментарий
    const replies_comment_array = document.querySelectorAll(`[id^="reply_comment_"]`);
    for(let button_reply_comment of replies_comment_array)
        button_reply_comment.addEventListener('click', button_reply_comment_click);

    //отправить комментарий
    const button_send_comment = document.getElementById('send_comment');
    if (button_send_comment != null)
        button_send_comment.addEventListener('click', button_send_comment_click);

    //удалить комментарий
    const delete_comment_array = document.querySelectorAll(`[id^="delete_comment_"]`);
    for(let button_delete_comment of delete_comment_array)
        button_delete_comment.addEventListener('click', button_delete_comment_click);    
    return true;
}

async function render_article(account_data, action, article, article_html, article_comments_html, comments) {
    //статья
    const article_div = document.getElementById('article_div');
    if (article_div == null)
        return false;
    article_div.innerHTML = '';
    article_div.insertAdjacentHTML('afterbegin', article_html);

    //события
    switch(action) {
        case 'create':
            const button_save_create = document.getElementById('button_save');
            if (button_save_create != null) {
                button_save_create.addEventListener('click', () => {
                    //проверка введенных данных
                    const article_title = document.getElementById('article_title');
                    const article_text = document.getElementById('article_text');
                    const article_image = document.getElementById('article_image');
                    const article_tags = document.getElementById('article_tags');
                    const article_hubs = document.getElementById('article_hubs');
                    const article_comments = document.getElementById('article_comments');
                    //
                    if (article_title == null || article_text == null ||
                        article_image == null || article_tags == null ||
                        article_hubs == null)
                        return false;

                    const sub = userClaims.find((claim) => claim.type === 'sub').value;
                    const name = userClaims.find((claim) => claim.type === 'name').value;

                    //объект
                    const custom_article = {
                        authorId: sub,
                        authorNickName: name,
                        title: article_title.value,
                        textHtml: article_text.value,
                        imageUrl: article_image.value,
                        commentsEnabled: article_comments.selectedIndex == 0 ? true : false,
                        tags: article_tags.value.split(','),
                        hubs: article_hubs.value.split(',')               
                    };
                    //
                    const response = create_article(action, account_data, custom_article);
                    const textHTML = response
                        ? `<p id="message">Сохранено</p>`
                        : `<p id="message">Ошибка</p>`;
                    //
                    const message_div = document.getElementById('message_div');
                    if (message_div != null)
                        message_div.insertAdjacentHTML('afterbegin', textHTML);
                    //
                    setTimeout(() => {
                        let message = document.getElementById('message');
                        if (message != null) {
                            message.remove();
                            //
                            if (response)
                                window.location = `${window.location.origin}/account.html`;
                        }                  
                    }, 2000);            
                });
            }
            break;

        case 'read':            
            //лайк статьи
            const button_like_article = document.getElementById(`like_${id}`);
            if (button_like_article != null)
                button_like_article.addEventListener('click', button_like_article_click);

            //дизлайк статьи
            const button_dislike_article = document.getElementById(`dislike_${id}`);
            if (button_dislike_article != null)
                button_dislike_article.addEventListener('click', button_dislike_article_click);            

            //комментарии
            const comments_header = document.getElementById('comments_header');
            if (comments_header == null)
                return false;
            comments_header.insertAdjacentHTML('afterend', article_comments_html);

            //правка изображений
            const img_array = document.getElementsByTagName('img');
            for (let img of img_array) {
                const data_src = img.getAttribute('data-src');
                if (data_src == null)
                    continue;
                //
                let src = document.createAttribute('src');
                src.value = data_src;
                //
                img.setAttributeNode(src);
                img.removeAttribute('data-src');
                img.removeAttribute('width');
                img.removeAttribute('height');
                img.classList.add('section_new_post_img_full');
            }

            //события
            //лайк комментария
            const likes_comment_array = document.querySelectorAll(`[id^="like_comment_"]`);
            for(let button_like_comment of likes_comment_array)
                button_like_comment.addEventListener('click', button_like_comment_click);

            //дизлайк комментария
            const dislikes_comment_array = document.querySelectorAll(`[id^="dislike_comment_"]`);
            for(let button_dislike_comment of dislikes_comment_array)
                button_dislike_comment.addEventListener('click', button_dislike_comment_click);

            //ответ на комментарий
            const replies_comment_array = document.querySelectorAll(`[id^="reply_comment_"]`);
            for(let button_reply_comment of replies_comment_array)
                button_reply_comment.addEventListener('click', button_reply_comment_click);

            //отправить комментарий
            const send_comment_array = document.querySelectorAll(`[id^="send_comment_"]`);
            for(let button_send_comment of send_comment_array)
                button_send_comment.addEventListener('click', button_send_comment_click);

            //удалить комментарий
            const delete_comment_array = document.querySelectorAll(`[id^="delete_comment_"]`);
            for(let button_delete_comment of delete_comment_array)
                button_delete_comment.addEventListener('click', button_delete_comment_click);
            break;

        case 'update':
            const button_save = document.getElementById('button_save');
            if (button_save != null) {
                button_save.addEventListener('click', async () => {
                    //проверка введенных данных
                    const article_title = document.getElementById('article_title');
                    if (article_title == null)
                        return false;
                    //
                    const article_text = document.getElementById('article_text');
                    if (article_text == null)
                        return false;
                    //
                    const article_image = document.getElementById('article_image');
                    if (article_image == null)
                        return false;
                    //
                    const article_tags = document.getElementById('article_tags');
                    if (article_tags == null)
                        return false;
                    //
                    const article_hubs = document.getElementById('article_hubs');
                    if (article_hubs == null)
                        return false;
                    //
                    const article_comments = document.getElementById('article_comments');
                    if (article_comments == null)
                        return false;
                    //
                    const article_data = {
                        articleId: article['id'],
                        title: article_title.value.trim(), 
                        textHtml: article_text.value.trim(),
                        imageUrl: article_image.value.trim(),
                        commentsEnabled: article_comments.selectedIndex == 0 ? true : false,
                        isPublished: false,
                        tags: article_tags.value.trim(),
                        hubs: article_hubs.value.trim()
                    };

                    const response = await update_article(action, account_data, article_data);
                    const textHTML = response
                        ? `<p id="message">Сохранено</p>`
                        : `<p id="message">Ошибка</p>`;
                    //
                    const message_div = document.getElementById('message_div');
                    if (message_div != null)
                        message_div.insertAdjacentHTML('afterbegin', textHTML);
                    //
                    setTimeout(() => {
                        let message = document.getElementById('message');
                        if (message != null) {
                            message.remove();
                            //
                            if (response)
                                window.location = `${window.location.origin}/account.html`;
                        }                  
                    }, 2000);
                });
            }
            break;
        
        case 'delete':
            const button_yes = document.getElementById('button_yes');
            if (button_yes != null) {
                button_yes.addEventListener('click', async () => {
                    const response = await delete_article(article['id']);
                    const textHTML = response
                        ? `<p id="message">Сохранено</p>`
                        : `<p id="message">Ошибка</p>`;
                    //
                    const message_div = document.getElementById('message_div');
                    if (message_div != null)
                        message_div.insertAdjacentHTML('afterbegin', textHTML);
                    //
                    setTimeout(() => {
                        let message = document.getElementById('message');
                        if (message != null) {
                            message.remove();
                            //
                            if (response)
                                window.location = `${window.location.origin}/account.html`;
                        }                  
                    }, 2000);
                });
            }

            /*const button_no = document.getElementById('button_no');
            if (button_no != null) {
                button_no.addEventListener('click', async () => {
                    //
                });
            }*/
            break;
    }
}

function render_page(response_json, html) {
    if (response_json == null)
        return false;
    //
    const header_html = response_json.header_html;
    const header_hubs_html = html.header_hubs_html;
    const main_html = html.main_html;
    const article_html = html.article_html;
    const article_comments_html = html.article_comments_html;
    const account_data = response_json.account_data;
    const footer_html = response_json.footer_html;

    render_header({ header_html: header_html, header_hubs_html: header_hubs_html });
    render_account(account_data);
    render_main(main_html);
    render_article(account_data, action, response_json.article, article_html, article_comments_html, response_json.article_comments);
    render_footer(footer_html);
}

function set_like_dislike_article_comment(value, id) {
    const like = document.getElementById(`like_comment_${id}`);
    const dislike = document.getElementById(`dislike_comment_${id}`);
    //
    if (like != null && dislike != null) {
        like.innerText = `Лайки: ${value.likes}`;
        dislike.innerText = `Дизлайки: ${value.dislikes}`;
        return true;
    }
    return false;
}

async function update_article(action, account_data, article) {
    if (article == null || typeof article === 'undefined')
        return;
    //
    article.tags = article.tags.split(',');
    article.hubs = article.hubs.split(',');

    const response = await fetch(`/articles/update-article?id=${article['articleId']}`, {
        method: 'PUT',
        body: JSON.stringify(article),
        headers: {
            'Content-Type': 'application/json',
            'X-CSRF': '1'
            },
    })
        .then(response => response.json())
        .catch(e => console.log(e));
    //
    if (response == null || typeof response === 'undefined')
        return false;
    if (!response.hasOwnProperty('isSuccess'))
        return false;
    return response.isSuccess;
    /*const response = new Request(`/articles/update-article?id=${article['articleId']}`, {
        method: 'PUT',
        body: JSON.stringify(article),
        headers: new Headers({
            "X-CSRF": "1", 
            "Content-Type": "application/json"
        })
    });
    //
    try {
        var resp = await fetch(response);
        let data;
        if (resp.ok)
            data = await resp.json();      
    } catch (e) {
        console.log('');
    }
    //
    if (response == null || typeof response === 'undefined')
        return false;
    if (!response.hasOwnProperty('result'))
        return false;
    return response.isSuccess;*/
}