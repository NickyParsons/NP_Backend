const mainTitle = "NickyParsons Site";
const title = "Test 1 page title";
const pageTitle = document.getElementById("page-title");
pageTitle.innerText = title;
document.title = `${mainTitle} | ${title}`;

export function init() {

}