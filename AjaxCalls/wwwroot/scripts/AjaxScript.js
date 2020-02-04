function getNames() {
    $("#log").append(" " + $("#textBox").val());
    var parameters = {
        "text": $("#textBox").val()
    };
    $.ajax({
        type: "POST",
        url: "/main/getnames",
        data: JSON.stringify(parameters),
        contentType: 'application/json',
        statusCode: {
            200: function (response) {
                $("#label").html("");
                for (var i = 0; i < response.length; i++) {
                    $("#label").append(response[i] + "<br>");
                }
            }
        },
        error: function () {
            alert("Error!")
        }
    });
}