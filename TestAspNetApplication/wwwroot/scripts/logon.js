{
    console.log("logging v6");
    let logonMenu = document.getElementById("logon");
    let isLoged = false;
    let cookies = document.cookie.split(";");
    for (let cookie of cookies) {
        if (cookie.split("=")[0].trim() === "nasty-boy") {
            isLoged = true;
        }
    }
    if (isLoged) {
        let aHref = document.createElement("a");
        aHref.setAttribute("href", "/logout");
        aHref.innerText = "Logout";
        logonMenu.appendChild(aHref); 
    }
    else {
        let links = new Map();
        links.set("Register", "/register");
        links.set("Login", "/login");
        let i = 0;
        for (let link of links) {
            let aHref = document.createElement("a");
            aHref.setAttribute("href", link[1]);
            aHref.innerText = link[0];
            logonMenu.appendChild(aHref);
            i++;
            if (i < links.size) {
                logonMenu.appendChild(document.createTextNode(" | "));
            }
        }
    }
}