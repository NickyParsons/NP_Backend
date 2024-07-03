class Person{
    #name;
    #age;
    constructor(pName, pAge){
        this.name = pName;
        this.Age = pAge;
    }
    get age(){
        return this.#age;
    }
    set age(value){
        if(value > 0 || value < 150){
            this.#age = value;
        }
    }
    describe(){
        return `Hello! My name is ${this.name} and i'm ${this.age} years old.`;
    }
}
class Company {
    name;
    constructor(cName) {
        this.name = cName;
    }
}
class Employee extends Person {
    company;
    constructor(pName, pAge, company) {
        super(pName, pAge);
        this.company = company;
    }
    describe() {
        return `Hello! My name is ${this.name} and i'm ${this.age} years old and I'm working in ${company.name}!`;
    }
}

export function init() {
     // всякие тесты
    //let company = new Company("Coca-Cola");
    //let user = new Employee("Blob", 56, company);
    //let pageContent = document.getElementById("pageContent");
    //if (pageContent != null) {
    //    let p1 = document.createElement("p");
    //    pageContent.appendChild(p1);
    //    p1.innerText = "JS element content:";
    //    var stringValue = "100";
    //    document.writeln(`<p>Тип string value: ${typeof (stringValue)}</p>`);
    //    var numberValue = parseInt(stringValue);
    //    document.writeln(`<p>Тип number value: ${typeof (numberValue)}</p>`);
    //    document.writeln(`${user.describe()}`);
    //    document.writeln(`<p>Дата: ${new Date().toTimeString()}</p>`);
    //}
}