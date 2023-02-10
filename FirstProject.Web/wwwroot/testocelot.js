function log() {
    document.getElementById("results").innerText = "";

    Array.prototype.forEach.call(arguments, function (msg) {
        if (typeof msg !== "undefined") {
            if (msg instanceof Error) {
                msg = "Error: " + msg.message;
            } else if (typeof msg !== "string") {
                msg = JSON.stringify(msg, null, 2);
            }
            document.getElementById("results").innerText += msg + "\r\n";
        }
    });
}

let userClaims = null;

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

            log("user logged in", userClaims);
        } else if (resp.status === 401) {
            log("user not logged in");
        }
    } catch (e) {
        log("error checking user status");
    }
})();

document.getElementById("getcomments").addEventListener("click", getcomments, false);
document.getElementById("getcommentscount").addEventListener("click", getcommentscount, false);
document.getElementById("createcomment").addEventListener("click", createcomment, false);
document.getElementById("likecomment").addEventListener("click", likecomment, false);
document.getElementById("dislikecomment").addEventListener("click", dislikecomment, false);
document.getElementById("changecomment").addEventListener("click", changecomment, false);
document.getElementById("deletecomment").addEventListener("click", deletecomment, false);


async function getcomments() {
    var articleId = document.getElementById("articleIdGet").value;
    var index = document.getElementById("indexGet").value;
    var count = document.getElementById("countGet").value;

    var req = new Request(`/comments?articleId=${articleId}&index=${index}&count=${count}`, {
        headers: new Headers({
            "X-CSRF": "1",
        }),
        method: 'GET'
    });

    try {
        var resp = await fetch(req);

        let data;
        if (resp.ok) {
            data = await resp.json();
        }
        log(resp.status, data);
    } catch (e) {
        log(e.message);
    }
}

async function getcommentscount() {
    var articleId = document.getElementById("articleIdCount").value;

    var req = new Request(`/comments/getCount?articleId=${articleId}`, {
        headers: new Headers({
            "X-CSRF": "1",
        }),
        method: 'GET'
    });

    try {
        var resp = await fetch(req);

        let data;
        if (resp.ok) {
            data = await resp.json();
        }
        log(resp.status, data);
    } catch (e) {
        log(e.message);
    }
}

async function createcomment() {
    var articleId = document.getElementById("articleIdCreate").value;
    var content = document.getElementById("contentCreate").value;

    var req = new Request(`/comments`, {
        headers: new Headers({
            "X-CSRF": "1",
            "Content-Type": "application/json"
        }),
        method: 'POST',
        body: `{ "articleId": "${articleId}", "content": "${content}" }`
    });

    try {
        var resp = await fetch(req);

        let data;
        if (resp.ok) {
            data = await resp.json();
        }
        log(resp.status, data);
    } catch (e) {
        log(e.message);
    }
}

async function likecomment() {
    var commentId = document.getElementById("commentIdLike").value;

    var req = new Request(`/comments/like`, {
        headers: new Headers({
            "X-CSRF": "1",
            "Content-Type": "application/json"
        }),
        method: 'PUT',
        body: `{ "commentId": "${commentId}" }`
    });

    try {
        var resp = await fetch(req);

        let data;
        if (resp.ok) {
            data = await resp.json();
        }
        log(resp.status, data);
    } catch (e) {
        log(e.message);
    }
}

async function dislikecomment() {
    var commentId = document.getElementById("commentIdDislike").value;

    var req = new Request(`/comments/dislike`, {
        headers: new Headers({
            "X-CSRF": "1",
            "Content-Type": "application/json"
        }),
        method: 'PUT',
        body: `{ "commentId": "${commentId}" }`
    });

    try {
        var resp = await fetch(req);

        let data;
        if (resp.ok) {
            data = await resp.json();
        }
        log(resp.status, data);
    } catch (e) {
        log(e.message);
    }
}

async function changecomment() {
    var commentId = document.getElementById("commentIdChange").value;
    var content = document.getElementById("contentChange").value;

    var req = new Request(`/comments/changecontent`, {
        headers: new Headers({
            "X-CSRF": "1",
            "Content-Type": "application/json"
        }),
        method: 'PUT',
        body: `{ "commentId": "${commentId}", "content": "${content}" }`
    });

    try {
        var resp = await fetch(req);

        let data;
        if (resp.ok) {
            data = await resp.json();
        }
        log(resp.status, data);
    } catch (e) {
        log(e.message);
    }
}

async function deletecomment() {
    var commentId = document.getElementById("commentIdDelete").value;

    var req = new Request(`/comments?id=${commentId}`, {
        headers: new Headers({
            "X-CSRF": "1",
        }),
        method: 'DELETE'
    });

    try {
        var resp = await fetch(req);

        let data;
        if (resp.ok) {
            data = await resp.json();
        }
        log(resp.status, data);
    } catch (e) {
        log(e.message);
    }
}