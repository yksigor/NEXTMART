$(function () {
    $('#modal-container').on('show.bs.modal', function (event) {

        var button = $(event.relatedTarget);
        var url = button.attr("href");
        var modal = $(this);
        
        modal.find('.modal-content').load(url);

    });

    $('#modal-container').on('hidden.bs.modal', function () {
        $(this).removeData('bs.modal');        
        $('#modal-container .modal-content').empty();
    });
});