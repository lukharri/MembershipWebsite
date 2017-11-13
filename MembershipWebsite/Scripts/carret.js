$(function () {

    // rotate product section carret 90 degrees when clicked
    $(".panel-carret").click(function (e) {
        $(this).toggleClass("pressed");
        $(this).children("glyphicon-play").toggleClass("gly-rotate-90");
        e.preventDefault();
    });
});