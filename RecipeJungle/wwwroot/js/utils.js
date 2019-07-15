function errorBox(text, onClick) {
    Swal.fire({
        allowOutsideClick: false,
        title: 'Error',
        text: text,
        type: 'error',
    }).then((result) => {
        if (onClick != null)
            onClick();
    });
}

function successBox(text, onClick) {
    Swal.fire({
        allowOutsideClick: false,
        title: 'Success',
        text: text,
        type: 'success',
    }).then((result) => {
        if (onClick != null)
            onClick();
    });
}

function infoBox(text, onClick) {
    Swal.fire({
        allowOutsideClick: false,
        title: 'Info',
        text: text,
        type: 'info',
    }).then((result) => {
        if (onClick != null)
            onClick();
    });
}

function confirmBox(text, onClick) {
    Swal.fire({
        allowOutsideClick: false,
        title: 'Are You Sure?',
        text: text,
        type: 'question',
        showCancelButton: true
    }).then((result) => {
        if (!result.value) return;
        if (onClick != null && result)
            onClick();
    });
}

function httpRequest(method, url, bodyObj, onSuccess) {
    //if (objectsToDisable != null) {
    //    for (let x in objectsToDisable) {
    //        let el = document.getElementById(objectsToDisable[x]);
    //        if (el != null)
    //            el.disabled = true;
    //    }
    //}

    let xhr = new XMLHttpRequest();
    xhr.open(method, url, true);
    xhr.setRequestHeader("Content-Type", "application/json");
    xhr.setRequestHeader("Authorization", getCookie("auth_Recipe"));
    xhr.onreadystatechange = function () {
        if (xhr.readyState != 4)
            return;
        /*if (objectsToDisable != null) {
            for (let x in objectsToDisable) {
                let el = document.getElementById(objectsToDisable[x]);
                if (el != null)
                    el.disabled = false;
            }
        }*/
        switch (xhr.status) {
            case 200:
                if (onSuccess != null)
                    onSuccess(JSON.parse(xhr.responseText));
                break;
            case 420:
                let obj = null;
                try {
                    obj = JSON.parse(xhr.responseText);
                } catch {

                }
                if (obj != null)
                    errorBox(obj);
                else
                    errorBox(xhr.responseText);
                break;
            case 401:
                window.location = "/login"
                break;
            default:
                errorBox(xhr.responseText);
                break;
        }
    };
    xhr.send(JSON.stringify(bodyObj));
}

function setCookie(cname, cvalue, exminute, path) {
    var d = new Date();
    d.setTime(d.getTime() + (exminute * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=" + path;
}

function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}
