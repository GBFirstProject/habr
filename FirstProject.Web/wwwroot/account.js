"use strict";
//variables
let userClaims = null;
let account_name = 'гость',
    account_role = 'guest';
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
})();

async function approve_article(id) {
    const response = await fetch(`/articles/approve-article?articleId=${id}`, {
        method: 'PUT',
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
    if (response.hasOwnProperty('isSuccess')) {
        window.location = `${window.location.origin}/account.html`;
        return response.isSuccess;
    }        
    return false;
}

function get_account_html(account, articles_for_moderation, author_articles, articles_moderation_actions, article_actions) {
    const email = account.email;
    const name = account.name;
    const role = account.role;
    const author_id = account.author_id;

    //гость
    if (role == 'guest') {
        return `          
            <div class="section_account_flex">
                <h4 class="site_h1">Нет доступа</h4>
            </div>`;
    }

    //автор/модератор/администратор
    let textHTML = '';
    //блок "статьи"
    if (role == 'user' || role == 'moderator' || role == 'admin') {
        textHTML += `
            <div class="container">
            <div class="section_account_flex">
                <h2 class="site_h1">Профиль</h2>
                <ul class="lk_ul_flex">
                    <li class="lk_li"><p class="lk_li_p">Логин</p> ${name}</li>
                    <li class="lk_li"><p class="lk_li_p">Почта</p> ${email}</li>
                    <li class="lk_li"><p class="lk_li_p">Роль</p> ${role}</li>

                </ul>                        
            </div>
            <div class="section_account_flex">
                                <h2 class="site_h1">Мои статьи</h2>
                    
                    </div>
                     </div>`;

                    
        //мои статьи
        if (author_articles == null || author_articles.length == 0)
            textHTML += '<p>Список пуст</p>';
        //
        if (author_articles != null) {
            author_articles.forEach(author_article => {
                //действия
                let article_actions_html = '';
                article_actions.forEach(action => {
                    article_actions_html += `
                        <a class="article_action" href="${window.location.origin}/article.html?id=${author_article['articleId']}&action=${action.link}">
                            ${action.name}
                        </a>`;
                });
                //
                textHTML += `
                    <div class="container">
                        <div class="account_li_block">
                            <a class="account_li_item" href="${window.location.origin}/article.html?id=${author_article['articleId']}">${author_article['title']}</a>
                            ${article_actions_html}
                        </div>
                    </div>`;

            });
        }        
        textHTML += '</ul>';
        
        if (role == 'moderator' || role == 'admin') {
            textHTML += `
                <h2 class="header_row">Статьи на проверку</h2>
                `;

            
            //статьи на проверку
            if (articles_for_moderation == null || articles_for_moderation.length == 0)
                textHTML += `<p>Список пуст</p>`;
            //
            if (articles_for_moderation != null) {
                articles_for_moderation.forEach(article_for_moderation => {
                    //действия
                    let articles_moderation_actions_html = '';
                    articles_moderation_actions.forEach(action => {
                        articles_moderation_actions_html += `
                            <a class="article_moderation_action ${action.link}" article_id="${article_for_moderation['articleId']}" style="cursor: pointer;">${action.name}</a>`;
                    });
                    //
                    textHTML += `
                        <li>    
                            <div class="account_li_block">                            
                                <a class="account_li_item" href="${window.location.origin}/article.html?id=${article_for_moderation['articleId']}">${article_for_moderation['title']}</a>
                                ${articles_moderation_actions_html}
                            </div>
                        </li>`;
                });
            }            
            textHTML += '</ul>';
        }
        //
        textHTML += `
		<div class="container">
         <div class="section_account_flex">
                <button class="add_cart_btn"><a href="${window.location.origin}/article.html?action=create">Добавить статью</a></button>
                <!--<a href="">Все статьи</a>-->
            </div>
            </div>`;

    }
    return textHTML;
}

function get_article_moderation_actions() {
    return [
        {
            name: 'Принять', 
            link: 'approve_article'
        }, 
        {
            name: 'Отклонить',
            link: 'reject_article'
        }
    ];
}

function get_article_actions() {
    return [
        {
            name: 'Редактировать',
            link: `update`
        },
        {
            name: 'Удалить',
            link: `delete`
        }
    ];
}

async function get_articles_for_moderation() {
    const response = await fetch(`/articles/get-articles-for-moderation`, {
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

async function get_titles_by_author_id(author_id) {
    const response = await fetch(`/articles/titlesByAuthorId?authorId=${author_id}`, {
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

function get_main_html() {
    return `<div class="container"><div id="account_div"></div></div>`;
}

async function load() {
    //загрузка данных
    response_json = await load_data();
    if (response_json == null)
        return;

    //рендер html
    const header_hubs_html = get_header_links_html(response_json.header_hubs);
    const main_html = get_main_html();

    //контент с ограниченным доступом
    let article_moderation_actions_html = '';
    let article_actions_html = '';
    //
    const account_html = get_account_html(
        response_json.account_data, 
        response_json.articles_for_moderation, 
        response_json.author_articles,
        response_json.article_moderation_actions,
        response_json.article_actions
        );

    //добавить на страницу
    delete_progressbar();
    render_page(
        response_json,
        {
            header_hubs_html: header_hubs_html,
            main_html: main_html,
            account_html: account_html
        }
    );
}

async function load_data() {
    let header_html = '',
        footer_html = '',
        header_hubs = '';
    let account = null,        
        account_data = null,
        articles_for_moderation = null,
        article_moderation_actions = null,
        article_actions = null,
        author_articles = null;

    //загрузка данных
    try {
        header_html = await get_header();
        footer_html = await get_footer();
        header_hubs = await get_top_hubs(4);//популярные хабы
        //
        account_data = get_account_data();

        //данные в зависимости от роли
        if (account_data != null) {
            const role = account_data.role.trim().toLowerCase();
            if (role == 'moderator' || role == 'admin') {
                articles_for_moderation = await get_articles_for_moderation();
                article_moderation_actions = get_article_moderation_actions();
            }                
            //
            if (role == 'user' || role == 'moderator' || role == 'admin') {
                author_articles = await get_titles_by_author_id(account_data.author_id);
                article_actions = get_article_actions();
            }                
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
        articles_for_moderation: articles_for_moderation,
        article_moderation_actions: article_moderation_actions,
        article_actions: article_actions,
        author_articles: author_articles
    };
}

async function reject_article(id) {
    const response = await fetch(`/articles/reject-article?articleId=${id}`, {
        method: 'PUT',
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
    if (response.hasOwnProperty('isSuccess'))
        return response.isSuccess;
    return false;
}

function render_account_layout(account_html) {
    //добавить на страницу
    let account_div = document.getElementById('account_div');
    if (account_div == null)
        return false;
    //
    account_div.innerHTML = '';
    account_div.insertAdjacentHTML('afterbegin', account_html);

    //события
    if (response_json != null) {
        for(const moderator_action of response_json.article_moderation_actions) {
            const elements_array = document.querySelectorAll(`.${moderator_action.link}`);
            for(let element of elements_array)
                element.addEventListener('click', () => window[`${moderator_action.link}`](`${element.getAttribute('article_id')}`));           
        }
        return true;
    }
    return false;
}

function render_page(response_json, html) {
    if (response_json == null)
        return false;
    //
    const header_html = response_json.header_html;
    const header_hubs_html = html.header_hubs_html;
    const main_html = html.main_html;
    const account_html = html.account_html;
    const account_data = response_json.account_data;
    const footer_html = response_json.footer_html;

    render_header({ header_html: header_html, header_hubs_html: header_hubs_html });
    render_account(account_data);
    render_main(main_html);
    render_account_layout(account_html);
    render_footer(footer_html);
}