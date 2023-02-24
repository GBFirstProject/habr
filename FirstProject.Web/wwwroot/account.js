"use strict";
//variables
let userClaims = null;
let account_name = 'гость',
    account_role = 'guest';
//arrays
/*window.onclick = function (e) {
    if (!e.target.matches('.dropbtn')) {
        var myDropdown = document.getElementById("myDropdown");
        if (!myDropdown.classList.contains('hidden'))
            myDropdown.classList.add('hidden');
    }
}*/

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

/*function account_click() {
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
    const dropbtn_computedStyle = getComputedStyle(dropbtn);
    const body_computedStyle = getComputedStyle(document.body);
    //
    dropdown_content.style.top = dropbtn_computedStyle.height;
    dropdown_content.classList.toggle('hidden');
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
}*/

async function load() {
    //header
    const header_html = await get_header();
    if (header_html == null || typeof header_html === 'undefined')
        return;
    //
    const header = document.getElementById('header');
    if (header == null)
        return;
    //
    header.innerHTML = '';
    header.insertAdjacentHTML('afterbegin', header_html);

    //footer
    const footer_html = await get_footer();
    if (footer_html == null || typeof footer_html === 'undefined')
        return;
    //
    const footer = document.getElementById('footer');
    if (footer == null)
        return;
    //
    footer.innerHTML = '';
    footer.insertAdjacentHTML('afterbegin', footer_html);

    //данные пользователя
    if (userClaims != null) {
        for (const claim of userClaims) {
            if (claim.hasOwnProperty('type')) {
                if (claim.type == 'name') {
                    account_name = claim.value;
                    break;
                }
            }
        }
    }

    //вывод информации о пользователе
    const account_login = document.getElementById('account_login');
    if (account_login != null)
        account_login.innerText = account_name;

    //пункты меню для пользователя
    const myDropdown = document.getElementById('myDropdown');
    if (myDropdown == null)
        return;
    //
    if (userClaims != null) {
        for (const claim of userClaims) {
            if (claim.hasOwnProperty('type')) {
                if (claim.type == 'role') {
                    account_role = claim.value.trim().toLowerCase();
                    break;
                }
            }
        }
    }
    render_account(account_role);
    render_data(account_role);
}

/*function login() {
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

function render_account(role) {
    const account = document.getElementById('account');
    const sign_in = document.getElementById('sign_in');
    const sign_up = document.getElementById('sign_up');
    const sign_out = document.getElementById('sign_out');
    //
    if (account == null ||
        sign_in == null ||
        sign_up == null ||
        sign_out == null)
        return false;

    if (role == 'user' || role == 'moderator' || role == 'admin') {
        //пункты меню
        //показать
        if (account.classList.contains('hidden'))
            account.classList.remove('hidden');
        if (sign_out.classList.contains('hidden'))
            sign_out.classList.remove('hidden');

        //события
        sign_out.addEventListener('click', logout);

        //скрыть
        if (!sign_in.classList.contains('hidden'))
            sign_in.classList.add('hidden');
        if (!sign_up.classList.contains('hidden'))
            sign_up.classList.add('hidden');

        //
        let account_email = '';
        if (userClaims != null) {
            for (const claim of userClaims) {
                if (claim.hasOwnProperty('type')) {
                    if (claim.type == 'email') {
                        account_email = claim.value.trim().toLowerCase();
                        break;
                    }
                }
            }
        }

        //
        let textHTML = ``;
        //
        switch (role) {
            case 'user':
                textHTML += `
                    <div class="section_account_flex">
                        <h4 class="site_h1">Профиль</h4>
                        <ul>
                            <li>Логин: ${account_name}</li>
                            <li>Почта: ${account_email}</li>
                            <li>Роль: ${account_role}</li>
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
                            <li>Логин: ${account_name}</li>
                            <li>Почта: ${account_email}</li>
                            <li>Роль: ${account_role}</li>
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
        //
        textHTML += ``;

        let main = document.querySelector('.section_account');
        if (main != null) {
            main.innerHTML = '';
            main.insertAdjacentHTML('afterbegin', textHTML);
        }
        return true;
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
        if (!account.classList.contains('hidden'))
            account.classList.add('hidden');
        if (!sign_out.classList.contains('hidden'))
            sign_out.classList.add('hidden');
        return true;
    }
    return false;
}*/

function render_data(role) {
    if (role == 'user' || role == 'moderator' || role == 'admin') {
        let account_email = '';
        if (userClaims != null) {
            for (const claim of userClaims) {
                if (claim.hasOwnProperty('type')) {
                    if (claim.type == 'email') {
                        account_email = claim.value.trim().toLowerCase();
                        break;
                    }
                }
            }
        }

        //
        let textHTML = ``;
        switch (role) {
            case 'user':
                textHTML += `
                        <div class="section_account_flex">
                            <h4 class="site_h1">Профиль</h4>
                            <ul>
                                <li>Логин: ${account_name}</li>
                                <li>Почта: ${account_email}</li>
                                <li>Роль: ${account_role}</li>
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
                                <li>Логин: ${account_name}</li>
                                <li>Почта: ${account_email}</li>
                                <li>Роль: ${account_role}</li>
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
        //
        textHTML += ``;

        let main = document.querySelector('.section_account');
        if (main != null) {
            main.innerHTML = '';
            main.insertAdjacentHTML('afterbegin', textHTML);
        }
        return true;
    }

    if (role == 'guest') {    
        return true;
    }
    return false;
}