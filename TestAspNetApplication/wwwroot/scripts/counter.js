{
    let minusButton = document.getElementById("minusButton");
    let counterButton = document.getElementById("counterButton");
    let plusButton = document.getElementById("plusButton");

    counterButton.innerText = 0;
    let counter = parseInt(counterButton.innerText);

    plusButton.addEventListener("click", addToCounter);
    minusButton.addEventListener("click", reduceToCounter);

    function addToCounter(e) {
        if (!e.ctrlKey) {
            counter = parseInt(counter) + 1;
        }
        else {
            counter = parseInt(counter) + 10;
        }
        counterButton.innerText = counter;
    }

    function reduceToCounter(e) {
        if (!e.ctrlKey) {
            counter = counter - 1;
        }
        else {
            counter = counter - 10;
        }
        if (counter < 0) {
            counter = 0;
        }
        counterButton.innerText = counter;
    }
}