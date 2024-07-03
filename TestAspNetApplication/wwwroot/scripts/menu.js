{
    let menuLinks = new Map();
    menuLinks.set("Home", "/");
    menuLinks.set("Counter", "counter")

    let navMenu = document.getElementById("navMenu");

    let i = 0;
    for (let link of menuLinks) {

        aHref = document.createElement("a");
        aHref.setAttribute("href", link[1]);
        aHref.innerText = link[0];
        navMenu.appendChild(aHref);
        i++;
        if (i < menuLinks.size) {
            navMenu.appendChild(document.createTextNode(" | "));
        }
    }
}
