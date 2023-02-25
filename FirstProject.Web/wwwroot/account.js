"use strict";
//variables
let userClaims = null;
let account_name = 'гость',
    account_role = 'guest';
//arrays
window.onclick = function (e) {
    if (!e.target.matches('.dropbtn')) {
        let authorize_dropdown = document.getElementById("authorize_dropdown");
        if (!authorize_dropdown.classList.contains('hidden'))
            authorize_dropdown.classList.add('hidden');
    }
};

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

async function load() {
    await render_page();
}

function render_data() {
    const account_data = get_account_data();
    if (account_data == null)
        return false;
    //
    const email = account_data.email;
    const name = account_data.name;
    const role = account_data.role;

    let textHTML = ``;
    if (role == 'user' || role == 'moderator' || role == 'admin') {        
        switch (role) {
            case 'user':
                textHTML += `
                        <div class="section_account_flex">
                            <h4 class="site_h1">Профиль</h4>
                            <ul>
                                <li>Логин: ${name}</li>
                                <li>Почта: ${email}</li>
                                <li>Роль: ${role}</li>
                            </ul>                        
                        </div>
                        <div class="section_account_flex">
                            <h4 class="site_h1">Статьи</h4>
                                <p class="header_row">Мои статьи</p>
                                <ul>
                                    <li>Статья 1</li>
                                    <li>Статья 2</li>
                                    <li>Статья 3</li>                            
                                </ul>
                            
                            <a href="">Добавить статью</a>
                            <a href="">Все статьи</a>
                        </div>`;
                break;

            case 'moderator':
                break;

            case 'admin':
                textHTML += `          
                        <div class="section_account_flex">
                            <h4 class="site_h1">Профиль</h4>
                            <ul>
                                <li>Логин: ${name}</li>
                                <li>Почта: ${email}</li>
                                <li>Роль: ${role}</li>
                            </ul>                        
                        </div>
                        <div class="section_account_flex">
                            <h4 class="site_h1">Статьи</h4>
                                <p class="header_row">Мои статьи</p>
                                <ul>
                                    <li>Статья 1</li>
                                    <li>Статья 2</li>
                                    <li>Статья 3</li>                            
                                </ul>

                                <p class="header_row">Статьи на проверку</p>
                                <ul>
                                    <li>Статья 1</li>
                                    <li>Статья 2</li>
                                    <li>Статья 3</li>                            
                                </ul>

                            <a href="">Добавить статью</a>
                            <a href="">Все статьи</a>
                        </div>
                        <div class="section_account_flex">
                            <h4 class="site_h1">Пользователи</h4>                            
                            <a href="">Все пользователи</a>
                            <a href="">Изменить права</a>
                        </div>`;
                break;
        }
        textHTML += ``;
    }

    if (role == 'guest') {
        textHTML += `          
            <div class="section_account_flex">
                <h4 class="site_h1">Нет доступа</h4>
            </div>`;
    }

    //добавить на страницу
    let main = document.querySelector('.section_account');
    if (main != null) {
        main.innerHTML = '';
        main.insertAdjacentHTML('afterbegin', textHTML);
    }
    return true;
}

function render_main() {
    return render_data();
}