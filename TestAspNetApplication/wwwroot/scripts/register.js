{
    let registerForm = document.getElementById("registerForm");
    let sendButton = document.getElementById("sendButton");
    const passwordField = document.getElementById("password");
    sendButton.addEventListener("click", validateForm);
    passwordField.addEventListener("change", validatePassword)
    function validateForm(e) {
        let password = document.getElementById("password").value;
        let repeatPassword = document.getElementById("repeatPassword").value;
        if (password !== repeatPassword) {
            console.log("Пароли не совпадают");
            passwordField.setCustomValidity("Пароли не совпадают");
            e.preventDefault();
        }
        else {
            passwordField.setCustomValidity("");
        }
        console.log(`Длина пароля ${passwordField.value.toString().lenght}`);
        if (passwordField.value.lenght < 6) {
            console.log("Пароль должен быть длинее пяти символов");
            passwordField.setCustomValidity("Пароль должен быть длинее пяти символов");
            e.preventDefault();
        }
        else {
            passwordField.setCustomValidity("");
        }
    }
    function validatePassword(e) {
        let password = document.getElementById("password").value;
        let repeatPassword = document.getElementById("repeatPassword").value;
        if (password !== repeatPassword) {
            console.log("Пароли не совпадают");
            passwordField.setCustomValidity("Пароли не совпадают");
        }
        else {
            passwordField.setCustomValidity("");
        }
        console.log(`Длина пароля ${passwordField.value.toString().lenght}`);
        if (passwordField.value.lenght < 6) {
            console.log("Пароль должен быть длинее пяти символов");
            passwordField.setCustomValidity("Пароль должен быть длинее пяти символов");
        }
        else {
            passwordField.setCustomValidity("");
        }
    }
}