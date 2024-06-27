{
    let links = new Map();
    links.set("Register", "/register");
    links.set("Login", "/login");

    let logonMenu = document.getElementById("logon");

    let i = 0;
    for (let link of links) {

        aHref = document.createElement("a");
        aHref.setAttribute("href", link[1]);
        aHref.innerText = link[0];
        logonMenu.appendChild(aHref);
        i++;
        if (i < links.size) {
            logonMenu.appendChild(document.createTextNode(" | "));
        }
    }

}