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

function render_account(role) {
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