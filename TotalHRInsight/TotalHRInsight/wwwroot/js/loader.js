// Variables globales
let loaderModalInstance = null;

// Función para mostrar el loader
function showLoader() {
    var loaderModal = new bootstrap.Modal(document.getElementById('loaderModal'));
    loaderModal.show();
}

// Función para ocultar el loader
function hideLoader() {
    var loaderModal = new bootstrap.Modal(document.getElementById('loaderModal'));
    loaderModal.hide();
}

