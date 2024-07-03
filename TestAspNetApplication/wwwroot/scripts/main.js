{
    const contentDiv = document.getElementById("pageContent");
    // This Xml Http Request method
    function loadContent(pageName) {
        console.log("loadContent v4");
        const xhr = new XMLHttpRequest();
        xhr.onload = () => {
            if (xhr.status == "200") {
                console.log(xhr.response);
                contentDiv.innerHTML = xhr.responseText;
            }
        };
        xhr.open("GET", pageName);
        xhr.setRequestHeader("Accept", "text/html");
        xhr.send();
    }
    let links = document.getElementsByClassName("navLink");
    for (let i = 0; i < links.length; i++) {
        links[i].addEventListener("click", (event) => {
            loadContentAsync(event.currentTarget.getAttribute("href"));
            setTitle(event.currentTarget.innerText);
            event.preventDefault();
        });
    }
    /////////
    async function loadContentAsync(pageName) {
        const response = await fetch(pageName, {
            method: "GET",
            headers: { "Accept": "text/html" }
        });
        if (response.ok) {
            contentDiv.innerHTML = await response.text();
            let module = await import(`../scripts/modules${pageName}.js`);
            module.init();
        }
    }

    async function setTitle(titleName){
        const mainTitle = "NickyParsons Site";
        const pageTitle = document.getElementById("page-title");
        pageTitle.innerText = titleName;
        document.title = `${mainTitle} | ${titleName}`;
    }

    loadContentAsync("/test1");
    setTitle("HOME");
}