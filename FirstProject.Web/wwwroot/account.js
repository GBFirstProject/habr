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

function get_account_html(account) {
    const email = account.email;
    const name = account.name;
    const role = account.role;

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
                <h2 class="site_h1">Профиль</h4>
                <ul class="lk_ul_flex">
                    <li class="lk_li"><p class="lk_li_p">Логин</p> ${name}</li>
                    <li class="lk_li"><p class="lk_li_p">Почта</p> ${email}</li>
                    <li class="lk_li"><p class="lk_li_p">Роль</p> ${role}</li>
                </ul>                        
            </div>
            <div class="section_account_flex">
                <h2 class="site_h1">Статьи</h4>
                    <p class="header_row">Мои статьи</p>
                    <ul>
                        <li>Статья 1</li>
                        <li>Статья 2</li>
                        <li>Статья 3</li>                            
                    </ul>
                    </div>
                     </div>
                    `;
        //
        if (role == 'moderator' || role == 'admin') {
            textHTML += `
                <p class="header_row">Статьи на проверку</p>
                <ul>
                    <li>Статья 1</li>
                    <li>Статья 2</li>
                    <li>Статья 3</li>                            
                </ul>`;
        }
        //
        textHTML += `
          <div class="container">
          <div class="section_account_flex">
                <button class="add_cart_btn"><a class="add_art_a" href="${window.location.origin}/create_article.html">Написать статью</a></button>
                <a href="">Все статьи</a>
            </div>
            </div>
           `;

        //блок "пользователи"
        if (role == 'admin') {
            textHTML += `
                <div class="section_account_flex">
                    <h4 class="site_h1">Пользователи</h4>                            
                    <a href="">Все пользователи</a>
                    <a href="">Изменить права</a>
                </div>`;
        }
    }
    return textHTML;
}

function get_main_html() {
    return `<div id="account_div"></div>`;
}

async function load() {
    //загрузка данных
    response_json = await load_data();
    if (response_json == null)
        return;

    //рендер html
    const header_hubs_html = get_header_links_html(response_json.header_hubs);
    const main_html = get_main_html();
    const account_html = get_account_html(response_json.account_data);

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
        account_data = null;

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

function render_account_layout(account_html) {
    //добавить на страницу
    let account_div = document.getElementById('account_div');
    if (account_div == null)
        return false;
    //
    account_div.innerHTML = '';
    account_div.insertAdjacentHTML('afterbegin', account_html);
    return true;
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