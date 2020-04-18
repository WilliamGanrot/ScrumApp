function createMessageDOM(logged_in_user_id, user, userId, userImg, message, time) {

    console.log("in createMessageDOM");
    var usr = user.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = msg;

    if (logged_in_user_id.localeCompare(userId) == 0) {

        var textBoxMessage = document.createElement("div");
        textBoxMessage.classList.add("text-box-message");
        textBoxMessage.innerText = encodedMsg;


        var h6 = document.createElement("h6");
        h6.innerText = usr;


        var textBox = document.createElement("div");
        textBox.classList.add("text-box-left");
        textBox.appendChild(h6)
        textBox.appendChild(textBoxMessage);

        var textsection = document.createElement("div");
        textsection.classList.add("text-section-left");
        textsection.classList.add("col-9");
        textsection.appendChild(textBox);
        console.log(textsection);

        var img = document.createElement("img");
        img.src = "../../media/Users/" + userImg;

        var imgSection = document.createElement("div");
        imgSection.classList.add("img-section");
        imgSection.classList.add("col-3");
        imgSection.appendChild(img);

        var li = document.createElement("li");
        li.classList.add("message");
        li.classList.add("row");
        li.appendChild(textsection);
        li.appendChild(imgSection);

        return li;
    }
    else {
        var textBoxMessage = document.createElement("div");
        textBoxMessage.classList.add("text-box-message");
        textBoxMessage.innerText = encodedMsg;

        var h6 = document.createElement("h6");
        h6.innerText = usr;


        var textBox = document.createElement("div");
        textBox.classList.add("text-box-right");
        textBox.appendChild(h6)
        textBox.appendChild(textBoxMessage);

        var textsection = document.createElement("div");
        textsection.classList.add("text-section-right");
        textsection.classList.add("col-9");
        textsection.appendChild(textBox);

        var img = document.createElement("img");
        img.src = "../../media/Users/" + userImg;

        var imgSection = document.createElement("div");
        imgSection.classList.add("img-section");
        imgSection.classList.add("col-3");
        imgSection.appendChild(img);

        var li = document.createElement("li");
        li.classList.add("message");
        li.classList.add("row");
        li.appendChild(imgSection);
        li.appendChild(textsection);

        


        return li;
    }
}

