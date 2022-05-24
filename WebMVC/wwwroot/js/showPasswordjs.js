function togglePassword($element) {
    var newtype = $element.prop('type') == 'password' ? 'text' : 'password';
    $element.prop('type', newtype);
}
$(document).ready(function () {
    $('#checkem').on('click', function (evt) {
        evt.preventDefault();
        evt.stopPropagation();

        togglePassword($('#password'));
        togglePassword($('#confirmPass'));
    });
});