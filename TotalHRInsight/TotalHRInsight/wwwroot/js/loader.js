// Variables globales
let loaderModalInstance = null;

// Funci�n para mostrar el loader
function showLoader() {
    var loaderModal = new bootstrap.Modal(document.getElementById('loaderModal'));
    loaderModal.show();
}

// Funci�n para ocultar el loader
function hideLoader() {
    var loaderModal = new bootstrap.Modal(document.getElementById('loaderModal'));
    loaderModal.hide();
}

