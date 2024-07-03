{
    console.log(`v17`);
    const registerForm = document.getElementById("registerForm");
    //const sendButton = document.getElementById("sendButton");
    registerForm.password.addEventListener("input", validatePassword)
    registerForm.repeatPassword.addEventListener("input", validatePassword)
    function validatePassword(e) {
        let passwordField = registerForm.password;
        let repeatPasswordField = registerForm.repeatPassword;
        passwordField.setCustomValidity("");
        repeatPasswordField.setCustomValidity("");
        if (passwordField.value.length < 6) {
            passwordField.setCustomValidity("Введите не менее 6 символов");
            e.preventDefault();
        }
        else {
            passwordField.setCustomValidity("");
            if (passwordField.value !== repeatPasswordField.value) {
                repeatPasswordField.setCustomValidity("Пароли не совпадают");
                e.preventDefault();
            }
            else {
                repeatPasswordField.setCustomValidity("");
            }
        }
    }
}