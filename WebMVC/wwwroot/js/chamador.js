$('.nav-link').on('click', function (evt) {
    evt.preventDefault();
    evt.stopPropagation();

    $(".nav").find(".active").removeClass("active");
    $(this).addClass("active");

    var $listou = $('#listou'),
        url = $(this).data('url');

    $.get(url, function (data) {
        $listou.html(data);
    });
});