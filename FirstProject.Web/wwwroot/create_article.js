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

async function load() {
    await render_page();
}

async function set_article() {
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

    const article_data = {
        authorId: "3fa85f64-5717-4562-b3fc-2c963f66afa6", 
        authorNickName: "silenceafter", 
        title: article_title.value, 
        textHtml: article_text.value, 
        imageUrl: article_image.value, 
        commentsEnabled: true, 
        tags: article_tags.value.split(','), 
        hubs: article_hubs.value.split(',')
    };
    //
    const response = await fetch(`articles/add-article`, {
        method: 'POST',
        body: JSON.stringify(article_data),
        headers: new Headers({
            "X-CSRF": "1", 
            "Content-Type": "application/json", 
            "Accept": "application/json" 
        })
    })
        .then(response => response.json())
        .catch(e => console.log(e));
    //
    if (response == null || typeof response === 'undefined')
        return null;
    if (!response.hasOwnProperty('result'))
        return null;
    return response.isSuccess;
}

async function render_article() {
    let textHTML = `
        <div class="page_panel">
            <p id="button_back"><a href="${window.location.origin}/account.html">Личный кабинет</a></p>
        </div>
        <div class="article_form">
            <form class="container_flex_column">
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
                <p id="button_save"><u>Сохранить</u></p>
                <div id="message_div"></div>
            </form>
        </div>`;
    
    //добавить на страницу
    const article_div = document.getElementById('article_div');
    if (article_div == null)
        return false;
    article_div.innerHTML = '';
    article_div.insertAdjacentHTML('afterbegin', textHTML);

    //события    
    const button_save = document.getElementById('button_save');
    if (button_save != null) {
        button_save.addEventListener('click', () => {
            textHTML = set_article()
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
                    window.location = `${window.location.origin}/account.html`;
                }                  
            }, 3000);            
        });
    }        
    return true;
}

async function render_main() {
    return await render_article();
}