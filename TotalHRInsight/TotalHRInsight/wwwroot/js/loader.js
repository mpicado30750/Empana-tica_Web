 wwwroot/js/loader.js
function showLoader() {
    setTimeout(function () {
        var loaderModal = new bootstrap.Modal(document.getElementById('loaderModal'));
        loaderModal.show();
    }, 500);
}
