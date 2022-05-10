$(document).ready(function () {
    if (checkCookie("AuthKey")) {
        login();
    }
});
function logout() {
    setCookie("AuthKey", "", 0);
    setCookie("UserId", "", 0);
    $("#NotLoggedIn").css("display", "inline-flex");
    $("#LoggedIn").css("display", "none");
}
$("#logoutButton").click(function () {
    logout();
    login();
});

$("#registerButton").click(function () {
    $.post("/Register", {
        Login: $("#rLogin").val(),
        Password: $("#rPassword").val(),
    }, 'json').done(function (data) {
        switch (JSON.parse(data)["code"]) {
            case 0:
                alert("GIT");
                break;
            case 1:
                alert("Internal server error");
                break;
            case 2:
                alert("Hasło lub login nie spełnia wymagań co do złożoności!");
                break;
            case 3:
                alert("Użytkownik o podanym loginie już istnieje");
                break;
        }
    });
});
$("#loginButton").click(function () {
    $.post("/Login", {
        Login: $("#lLogin").val(),
        Password: $("#lPassword").val(),
    }, 'json').done(function (data) {
        if (data == "00000000-0000-0000-0000-000000000000") {
            alert("Podano bledny login lub haslo");
        }
        else {
            setCookie("AuthKey", data, 2);
            login();
        }
    });
});

function login() {
    if (getCookie("AuthKey") != "") {
        $.post("/ValidateLogin", {
            AuthKey: getCookie("AuthKey"),
        }, 'json').done(function (data) {
            if (data!=-1) {
                $("#NotLoggedIn").css("display", "none");
                $("#LoggedIn").css("display", "block");
                setCookie("UserId", data, 2);
            }
            else {
                logout();
            }
        });
    }
    else {
        logout();
    }
}

function setCookie(cname, cvalue, exdays) {
    const d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    let expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

function getCookie(cname) {
    let name = cname + "=";
    let ca = document.cookie.split(';');
    for (let i = 0; i < ca.length; i++) {
        let c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

function checkCookie(cname) {
    if (cname == "")
        return 0;
    return 1;
}