"use strict";
//variables
let userClaims = null;
const page_number_deafult = 1,
    page_size_default = 10;
let page_number = null,
    page_size = null,
    tag = null;
const tags_count = 10;
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
    page_number = urlParams.get('PageNumber');
    page_size = urlParams.get('PageSize');
    tag = urlParams.get('tag').trim().toLowerCase();
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
    load();
})();

async function get_by_tag(page_number, page_size, tag) {
    const response = await fetch(`/articles/get-by-tag?PageNumber=${page_number}&PageSize=${page_size}&tag=${tag}`, {
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
    return {result: response.result, title: tag};
}

function get_tag() {
    const queryString = window.location.search;
    const urlParams = new URLSearchParams(queryString);
    return urlParams.get('tag');
}

function get_tags_top(tags) {
    //добавить хабы
    let textHTML = '<h4 class="title_row">Все тэги:</h4>';
    tags.forEach(tag => {
        textHTML += `
            <div class="all_tags_new_item">
            <div class="all_tags_new_item_flex">                
                <div class="all_tags_new_item_texts">
                    <h4><a class="site_links" href="${window.location.origin}/tags.html?tag=${tag.tagName.trim().toLowerCase()}">${tag.tagName}</a></h4>
                    <h4>${tag.articlesCount}</h4>
                    <br>                  
                </div>
            </div>
        </div>`;
    });
    return textHTML;
}

async function get_tags_query(value) {
    //добавить тэги
    let textHTML = `<h4 class="title_row">${value.title}</h4>`;
    for(let article of value.result.resultData)
        textHTML += await render_preview_article(article);

    //пагинация
    const tags_count = value.result.count;
    if (tags_count == -1)
        return false;
    textHTML += render_pagination(value);
    return textHTML;
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
    return response.result;
}

async function load() {
    await render_page();
}

async function render_main() {
    const tag = get_tag();
    if (!await render_tags(tag))
        return false;
    return true;
}

async function render_tags(tag) {
    let tags = null;
    const textHTML = tag == null
        ? get_tags_top(await get_top_tags(tags_count))
        : await get_tags_query(await get_by_tag(page_number, page_size, tag));
    
    //добавить на страницу
    let tags_div = document.getElementById('tags_div');
    if (tags_div == null)
        return false;
    //
    tags_div.innerHTML = '';
    tags_div.insertAdjacentHTML('afterbegin', textHTML);    
    return true;
}

function render_pagination(value) {
    //пагинация
    const tags_count = value.result.count;
    const pages_count = Math.ceil(tags_count / page_size);
    //
    let textHTML = '<div class="all_posts_pagination">';
    if (tags_count >= 0 && tags_count < pages_count) {
        //единственная страница            
        textHTML += `<a class="all_posts_pag " href="#prev">Назад</a>
                <div class="pagination">1</div>
                <a class="all_posts_pag" href="#next">Вперед</a>`;
    } else if (tags_count > pages_count) {
        //несколько страниц
        const penultimate_page = pages_count - 1;
        const last_page = pages_count;

        //назад
        textHTML += page_number > 1
            ? `<a class="all_posts_pag" href="${window.location.origin}/tags.html?PageNumber=${page_number - 1}&PageSize=${page_size}&tag=${value.title}">Назад</a>`
            : `<a class="all_posts_pag">Назад`;

        //1 и 2 страницы обязательно
        textHTML += `<a href="${window.location.origin}/tags.html?PageNumber=1&PageSize=${page_size}&tag=${value.title}"><div class="pagination">1</div></a>
            <a href="${window.location.origin}/tags.html?PageNumber=2&PageSize=${page_size}&tag=${value.title}"><div class="pagination">2</div></a>`;

        //предыдущие страницы (если есть)      
        if (page_number > 2) {
            const prev_count = page_number - 2;
            //
            if (prev_count <= 4) {
                for (let i = 3, j = 0; j < prev_count - 1; i++, j++)
                    textHTML += `<a href="${window.location.origin}/tags.html?PageNumber=${i}&PageSize=${page_size}&tag=${value.title}"><div class="pagination">${i}</div></a>`;
            } else {
                textHTML += `<div class="pagination">...</div>`;
                for (let i = page_number - 2; i < page_number; i++)
                    textHTML += `<a href="${window.location.origin}/tags.html?PageNumber=${i}&PageSize=${page_size}&tag=${value.title}"><div class="pagination">${i}</div></a>`;
            }
        }

        //текущая страница
        if (page_number > 2)
            textHTML += `<a href="${window.location.origin}/tags.html?PageNumber=${page_number}&PageSize=${page_size}&tag=${value.title}"><div class="pagination">${page_number}</div></a>`;

        //последующие следующие (если есть)
        const next_count = last_page - page_number;
        if (next_count > 5) {
            //page_number, page_number + 1, page_number + 2 ... penultimate_page, last_page            
            for (let i = 1; i < 3; i++) {
                if (page_number + i > 2)
                    textHTML += `<a href="${window.location.origin}/tags.html?PageNumber=${page_number + i}&PageSize=${page_size}&tag=${value.title}"><div class="pagination">${page_number + i}</div></a>`;
            }
            textHTML += `<div class="pagination">...</div>
                <a href="${window.location.origin}/tags.html?PageNumber=${penultimate_page}&PageSize=${page_size}&tag=${value.title}"><div class="pagination">${penultimate_page}</div></a>
                <a href="${window.location.origin}/tags.html?PageNumber=${last_page}&PageSize=${page_size}&tag=${value.title}"><div class="pagination">${last_page}</div></a>`;
        } else {
            for (let i = page_number + 1; i <= last_page; i++)
                textHTML += `<a href="${window.location.origin}/tags.html?PageNumber=${i}&PageSize=${page_size}&tag=${value.title}"><div class="pagination">${i}</div></a>`;
        }

        //вперед
        textHTML += page_number < last_page
            ? `<a class="all_posts_pag" href="${window.location.origin}/tags.html?PageNumber=${page_number + 1}&PageSize=${page_size}&tag=${value.title}">Вперед</a>`
            : `<a class="all_posts_pag">Вперед`;
    }
    textHTML += '</div>';
    return textHTML;
}