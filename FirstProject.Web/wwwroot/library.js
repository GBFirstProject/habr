"use strict";

function account() {
    window.location = "/account.html";
}

function account_click() {
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
    const header_flex = document.querySelector('.header_flex');
    const header = document.querySelector('header');
    //
    if (header_flex == null || header == null)
        return;

    const dropbtn_computed = getComputedStyle(dropbtn);
    const header_flex_computed = getComputedStyle(header_flex);
    const header_computed = getComputedStyle(header);

    //right
    let header_computed_width = header_computed.width;
    let header_flex_computed_width = header_flex_computed.width;
    let right = (parseFloat(header_computed_width) - parseFloat(header_flex_computed_width)) / 2;
    dropdown_content.style.right = `${right}px`;

    //top
    dropdown_content.style.top = dropbtn_computed.height;
    dropdown_content.classList.toggle('hidden');
}

function add_header_links() {
    //ссылки в header
    //все хабы (all_hubs)
    const all_hubs_item = document.getElementById('all_hubs');
    if (all_hubs_item == null)
        return false;
    add_link(all_hubs_item, `${window.location.origin}/hubs.html`);

    //все тэги
    const all_tags_item = document.getElementById('all_tags');
    if (all_tags_item == null)
        return false;
    add_link(all_tags_item, `${window.location.origin}/tags.html`);    
    return true;
}

function add_link(item, link) {
    if (item == null || typeof item === 'undefined')
        return false;
    //
    let a_element = null;
    for(let child of item.childNodes) {
        if (child.tagName.toLowerCase() == 'a') {
            a_element = child;
            break;
        }
    }
    
    //добавить
    if (a_element != null)
        a_element.href = link;
    return true;
}

function contains_role(role) {
    return roles.includes(role);
}

function get_account_data() {
    //данные пользователя
    let account_data = {'email': '', 'name': 'гость', 'role': 'guest'};
    //
    if (userClaims != null) {
        for (const claim of userClaims) {
            switch(claim.type) {
                case 'email':
                    account_data.email = claim.value.trim().toLowerCase();
                    break;

                case 'name':
                    account_data.name = claim.value;
                    break;

                case 'role':
                    account_data.role = claim.value.trim().toLowerCase();
                    break;
            }
        }
    }
    return account_data;
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
}

function get_roles() {
    return roles;
}

function login() {
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

function render_account() {
    const account_data = get_account_data();
    if (account_data == null)
        return false;
    //
    const name = account_data.name;
    const role = account_data.role;    
    
    //вывод информации о пользователе
    const account_login = document.getElementById('account_login');
    if (account_login != null)
        account_login.innerText = name;

    //пункты меню для пользователя
    const authorize_dropdown = document.getElementById('authorize_dropdown');
    if (authorize_dropdown == null)
        return;   
    
    //вывод в зависимости от роли
    const personal = document.getElementById('account');
    const sign_in = document.getElementById('sign_in');
    const sign_up = document.getElementById('sign_up');
    const sign_out = document.getElementById('sign_out');
    //
    if (personal == null ||
        sign_in == null ||
        sign_up == null ||
        sign_out == null)
        return false;

    //пункты меню
    if (role == 'user' || role == 'moderator' || role == 'admin') {
        //показать
        if (personal.classList.contains('hidden'))
            personal.classList.remove('hidden');
        if (sign_out.classList.contains('hidden'))
            sign_out.classList.remove('hidden');

        //события
        personal.addEventListener('click', account);
        sign_out.addEventListener('click', logout);

        //скрыть
        if (!sign_in.classList.contains('hidden'))
            sign_in.classList.add('hidden');
        if (!sign_up.classList.contains('hidden'))
            sign_up.classList.add('hidden');
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
        if (!personal.classList.contains('hidden'))
            personal.classList.add('hidden');
        if (!sign_out.classList.contains('hidden'))
            sign_out.classList.add('hidden');
        return true;
    }
    return false;
}

async function render_page() {
    //1 header
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
    //
    add_header_links();//ссылки в header

    //2 footer
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

    //3 main
    if (!render_main())
        return;

    //account
    render_account();
}