var submitButton = document.getElementById('submitButton');
var responseDiv = document.getElementById('response');
submitButton.addEventListener('click', function () {
    var inputAccount = document.getElementById('inputAccount');
    var inputPassword = document.getElementById('inputPassword');
    var request = new XMLHttpRequest();
    request.open('POST', '/register');
    request.onreadystatechange = function (ev) {
        if (request.readyState === 4) {
            if (request.status === 200) {
                var registerResponse = JSON.parse(request.responseText);
                if (registerResponse.Success) {
                    responseDiv.classList.remove('error');
                    responseDiv.classList.add('success');
                } else {
                    responseDiv.classList.remove('success');
                    responseDiv.classList.add('error');
                }
                responseDiv.innerText = registerResponse.Message;
            } else {
                responseDiv.classList.remove('success');
                responseDiv.classList.add('error');
                responseDiv.innerText = "Network or Server error.";
            }
        }
    };
    var hash = md5(inputPassword.value);
    var jsonData = {Account: inputAccount.value, Hash: hash};
    var formattedJsonData = JSON.stringify(jsonData);
    request.send(formattedJsonData);
    return false;
}, false);