const mainTitle = "NickyParsons Site";
const title = "Test 2 page title";
const pageTitle = document.getElementById("page-title");
pageTitle.innerText = title;
document.title = `${mainTitle} | ${title}`;

export function init() {

}