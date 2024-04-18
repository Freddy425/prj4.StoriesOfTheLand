// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
showInPrompt = (url, Title) => {
    $.ajax({
        type: "GET",
        url: url,
        Success: function (res) {
            $( "#faqModal .modal-body" ).html(res);
            $("#faqModal .modal-title").html(title);
            $("#faqModal .modal-body").html(res);
            }
    })
}