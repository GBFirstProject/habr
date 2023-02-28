"use strict";
//variables
let userClaims = null;
const page_number_deafult = 1,
    page_size_default = 10;
let page_number = null,
    page_size = null,
    hub = null;
const hubs_count = 10;
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
    hub = urlParams.get('hub').trim().toLowerCase();
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
    load();
})();

async function get_by_hub(page_number, page_size, hub) {
    const response = await fetch(`/articles/get-by-hub?PageNumber=${page_number}&PageSize=${page_size}&hub=${hub}`, {
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
    return {result: response.result, title: hub};
}

function get_hub() {
    const queryString = window.location.search;
    const urlParams = new URLSearchParams(queryString);
    return urlParams.get('hub');
}

function get_hubs_top(hubs) {
    //добавить хабы
    let textHTML = '<h4 class="title_row">Все хабы:</h4>';
    hubs.forEach(hub => {
        textHTML += `
            <div class="all_hubs_new_item">
            <div class="all_hubs_new_item_flex">                
                <div class="all_hubs_new_item_texts">
                    <h4><a class="site_links" href="${window.location.origin}/hubs.html?hub=${hub.title.trim().toLowerCase()}">${hub.title}</a></h4>
                    <h4>${hub.articlesCount}</h4>
                    <br>                  
                </div>
            </div>
        </div>`;
    });
    return textHTML;
}

async function get_hubs_query(value) {
    //добавить хабы
    let textHTML = `<h4 class="title_row">${value.title}</h4>`;
    for(let article of value.result.resultData)
        textHTML += await render_preview_article(article);

    //пагинация
    const hubs_count = value.result.count;
    if (hubs_count == -1)
        return false;
    textHTML += render_pagination(value);
    return textHTML;
}

/*async function get_top_hubs(hubs_count) {
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
}*/

async function load() {
    await render_page();
}

async function render_hubs(hub) {
    let hubs = null;
    const textHTML = hub == null
        ? get_hubs_top(await get_top_hubs(hubs_count))
        : await get_hubs_query(await get_by_hub(page_number, page_size, hub));
    
    //добавить на страницу
    let hubs_div = document.getElementById('hubs_div');
    if (hubs_div == null)
        return false;
    //
    hubs_div.innerHTML = '';
    hubs_div.insertAdjacentHTML('afterbegin', textHTML);    
    return true;
}

async function render_main() {
    const hub = get_hub();
    if (!await render_hubs(hub))
        return false;
    return true;
}

function render_pagination(value) {
    //пагинация
    const hubs_count = value.result.count;
    const pages_count = Math.ceil(hubs_count / page_size);
    //
    let textHTML = '<div class="all_posts_pagination">';
    if (hubs_count >= 0 && hubs_count < pages_count) {
        //единственная страница            
        textHTML += `<a class="all_posts_pag " href="#prev">Назад</a>
                <div class="pagination">1</div>
                <a class="all_posts_pag" href="#next">Вперед</a>`;
    } else if (hubs_count > pages_count) {
        //несколько страниц
        const penultimate_page = pages_count - 1;
        const last_page = pages_count;

        //назад
        textHTML += page_number > 1
            ? `<a class="all_posts_pag" href="${window.location.origin}/hubs.html?PageNumber=${page_number - 1}&PageSize=${page_size}&hub=${value.title}">Назад</a>`
            : `<a class="all_posts_pag">Назад`;

        //1 и 2 страницы обязательно
        textHTML += `<a href="${window.location.origin}/hubs.html?PageNumber=1&PageSize=${page_size}&hub=${value.title}"><div class="pagination">1</div></a>
            <a href="${window.location.origin}/hubs.html?PageNumber=2&PageSize=${page_size}&hub=${value.title}"><div class="pagination">2</div></a>`;

        //предыдущие страницы (если есть)      
        if (page_number > 2) {
            const prev_count = page_number - 2;
            //
            if (prev_count <= 4) {
                for (let i = 3, j = 0; j < prev_count - 1; i++, j++)
                    textHTML += `<a href="${window.location.origin}/hubs.html?PageNumber=${i}&PageSize=${page_size}&hub=${value.title}"><div class="pagination">${i}</div></a>`;
            } else {
                textHTML += `<div class="pagination">...</div>`;
                for (let i = page_number - 2; i < page_number; i++)
                    textHTML += `<a href="${window.location.origin}/hubs.html?PageNumber=${i}&PageSize=${page_size}&hub=${value.title}"><div class="pagination">${i}</div></a>`;
            }
        }

        //текущая страница
        if (page_number > 2)
            textHTML += `<a href="${window.location.origin}/hubs.html?PageNumber=${page_number}&PageSize=${page_size}&hub=${value.title}"><div class="pagination">${page_number}</div></a>`;

        //последующие следующие (если есть)
        const next_count = last_page - page_number;
        if (next_count > 5) {
            //page_number, page_number + 1, page_number + 2 ... penultimate_page, last_page            
            for (let i = 1; i < 3; i++) {
                if (page_number + i > 2)
                    textHTML += `<a href="${window.location.origin}/hubs.html?PageNumber=${page_number + i}&PageSize=${page_size}&hub=${value.title}"><div class="pagination">${page_number + i}</div></a>`;
            }
            textHTML += `<div class="pagination">...</div>
                <a href="${window.location.origin}/hubs.html?PageNumber=${penultimate_page}&PageSize=${page_size}&hub=${value.title}"><div class="pagination">${penultimate_page}</div></a>
                <a href="${window.location.origin}/hubs.html?PageNumber=${last_page}&PageSize=${page_size}&hub=${value.title}"><div class="pagination">${last_page}</div></a>`;
        } else {
            for (let i = page_number + 1; i <= last_page; i++)
                textHTML += `<a href="${window.location.origin}/hubs.html?PageNumber=${i}&PageSize=${page_size}&hub=${value.title}"><div class="pagination">${i}</div></a>`;
        }

        //вперед
        textHTML += page_number < last_page
            ? `<a class="all_posts_pag" href="${window.location.origin}/hubs.html?PageNumber=${page_number + 1}&PageSize=${page_size}&hub=${value.title}">Вперед</a>`
            : `<a class="all_posts_pag">Вперед`;
    }
    textHTML += '</div>';
    return textHTML;
}