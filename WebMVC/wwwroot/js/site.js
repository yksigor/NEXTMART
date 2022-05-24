// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    PosicionarRodape();
});

$(window).resize(function () {
    PosicionarRodape();
});

function PosicionarRodape() {

    var _doc = $(document).height();
    var _win = $(window).height();

    if (_doc > _win)
    {
        $("#rodape").css("position", "relative").css("display", "flex");
    }
    else
    {
        $("#rodape").css("position", "fixed").css("display", "flex");
    }
    //console.log("Tamanho Doc:" + _doc + "Tamanho janela: " + _win);
}