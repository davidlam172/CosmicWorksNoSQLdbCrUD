function validateForm() {
    var fields = document.querySelectorAll("input[required]");
    var errorMessages = [];

    fields.forEach(function (field) {
        if (!field.value.trim()) {
            field.classList.add("error");
            errorMessages.push(field.labels[0].textContent + " is required.");
        } else {
            field.classList.remove("error");
        }
    });

    var errorDiv = document.getElementById("error-messages");
    errorDiv.innerHTML = "";

    if (errorMessages.length > 0) {
        errorMessages.forEach(function (message) {
            var p = document.createElement("p");
            p.textContent = message;
            errorDiv.appendChild(p);
        });
        return false; // Prevent form submission
    }

    return true; // Allow form submission
}
