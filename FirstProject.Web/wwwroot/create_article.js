"use strict";
let userClaims = null;
let article = null;
let id = null;
const page_number_deafult = 1,
    page_size_default = 10;
let page_number = null,
    page_size = null;
let response_json = null;
//arrays
window.addEventListener("load", load);//DOMContentLoaded
window.onclick = function (e) {
    if (!e.target.matches('.dropbtn')) {
        let authorize_dropdown = document.getElementById("authorize_dropdown");
        if (!authorize_dropdown.classList.contains('hidden'))
            authorize_dropdown.classList.add('hidden');
    }
};
add_progressbar();
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

function get_create_article_html() {
    return `
        <div class="page_panel">
            <h3 class="add_art_btn_back" id="button_back"><a href="${window.location.origin}/account.html">назад в Личный кабинет</a></h3>
        </div>
        <div class="article_form">
            <form class="container_flex_column">
                <label>Название статьи</label>                    
                <input placeholder="Укажите название статьи" type="text" class="article_field" id="article_title">
                <label>Текст статьи</label>                    
                <textarea style="height:250px;" placeholder="Разместите здесь текст статьи" class="article_field article_text" id="article_text"></textarea>
                <label>Изображение</label>                    
                <input placeholder="Разместите здесь ссылку на изображение" type="text" class="article_field" id="article_image">                
                <label>Тэги</label>                    
                <input placeholder="Укажите тэги статьи" type="text" class="article_field" id="article_tags">
                <label>Хабы</label>                    
                <input placeholder="Укажите хабы для статьи" type="text" class="article_field" id="article_hubs">
                <button class="add_art_btn" id="button_save">Сохранить</button>
                <div id="message_div"></div>
            </form>
        </div>`;
}

function get_main_html() {
    return `<div class="container" id="article_div"></div>`;
}

async function load() {
    //загрузка данных
    response_json = await load_data();
    if (response_json == null)
        return;

    //рендер html
    const header_hubs_html = get_header_links_html(response_json.header_hubs);
    const main_html = get_main_html();
    const article_html = get_create_article_html();

    //добавить на страницу
    delete_progressbar();
    render_page(
        response_json,
        {
            header_hubs_html: header_hubs_html,
            main_html: main_html,
            article_html: article_html
        }
    );
}

async function load_data() {
    let header_html = '',
        footer_html = '',
        header_hubs = '';
    let account_data = null;

    //загрузка данных
    try {
        header_html = await get_header();
        footer_html = await get_footer();
        header_hubs = await get_top_hubs(4);//популярные хабы
        //
        account_data = get_account_data();
    } catch (ex) {
        throw new Error('ошибка загрузки данных');
    }

    //объект
    return {
        header_html: header_html,
        footer_html: footer_html,
        header_hubs: header_hubs,
        account_data: account_data
    };
}

function render_create_article(article_html, account_data) {
    //добавить на страницу
    const article_div = document.getElementById('article_div');
    if (article_div == null)
        return false;
    article_div.innerHTML = '';
    article_div.insertAdjacentHTML('afterbegin', article_html);
 
    //события    
    const button_save = document.getElementById('button_save');
    if (button_save != null) {
        button_save.addEventListener('click', () => {
            const response = set_article(account_data);
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
            }, 3000);            
        });
    }        
    return true;
}

function render_page(response_json, html) {
    if (response_json == null)
        return false;
    //
    const header_html = response_json.header_html;
    const header_hubs_html = html.header_hubs_html;
    const main_html = html.main_html;
    const account_data = response_json.account_data;
    const article_html = html.article_html; 
    const footer_html = response_json.footer_html;

    render_header({ header_html: header_html, header_hubs_html: header_hubs_html });
    render_account(account_data);
    render_main(main_html);
    render_create_article(article_html, account_data);
    render_footer(footer_html);
}

async function set_article(account_data) {
    const article_title = document.getElementById('article_title');
    const article_text = document.getElementById('article_text');
    const article_image = document.getElementById('article_image');
    const article_tags = document.getElementById('article_tags');
    const article_hubs = document.getElementById('article_hubs');
    //
    if (article_title == null || article_text == null ||
        article_image == null || article_tags == null ||
        article_hubs == null)
        return false;

        const vvv = userClaims.find(
            (claim) => claim.type === 'sub'
        ).value;

    const article_data = {
        authorId: vvv, 
        authorNickName: account_data.name, 
        title: article_title.value, 
        textHtml: article_text.value, 
        imageUrl: article_image.value, 
        commentsEnabled: true, 
        tags: article_tags.value.split(','), 
        hubs: article_hubs.value.split(',')
    };
    //
    const response = new Request(`/articles/add-article`, {
        method: 'POST',
        body: JSON.stringify(article_data),
        headers: new Headers({
            "X-CSRF": "1", 
            "Content-Type": "application/json",
            "Accept": "*/*"
        })
    })
        /*.then(response => response.json())
        .catch(e => console.log(e));*/
        try {
            var resp = await fetch(response);
    
            let data;
            if (resp.ok) {
                data = await resp.json();
            }
            log(resp.status, data);
        } catch (e) {
            log(e.message);
        }
    //
    if (response == null || typeof response === 'undefined')
        return false;
    if (!response.hasOwnProperty('result'))
        return false;
    return response.isSuccess;
}